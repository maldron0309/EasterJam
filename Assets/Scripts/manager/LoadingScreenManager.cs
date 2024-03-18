using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace manager
{
    /// <summary>
    ///     Used to asynchronously load FMOD data, render a loading spinner,
    ///     and then change to the main menu scene
    /// </summary>
    public class LoadingScreenManager : MonoBehaviour
    {
        [Header("FMOD Audio")] [BankRef] [SerializeField]
        private List<string> _banks;

        [SerializeField] private Image _studioIconImage;
        [SerializeField] private TMP_Text _studioIconText;

        [Header("Scene Management")] [SerializeField]
        private string _sceneToLoadOnFinish;

        private void Awake()
        {
            Assert.IsFalse(_banks.Count is 0, "There are no FMOD banks set up!");
        }

        private void Start()
        {
            StartCoroutine(LoadGameAsync());
        }

        /// <summary>
        ///     Loads the game settings, sets up FMOD, and switches to the menu scene
        /// </summary>
        private IEnumerator LoadGameAsync()
        {
            // Load settings
            GameSettingsManager.Load();

            var studioTransform = _studioIconImage.transform;
            studioTransform.localScale = Vector2.zero;
            studioTransform.rotation = Quaternion.identity;

            _studioIconText.transform.localScale = Vector2.zero;

            LeanTween
                .scale(_studioIconImage.gameObject, Vector2.one, 1f)
                .setEase(LeanTweenType.easeInOutCirc);

            LeanTween
                .rotateZ(_studioIconImage.gameObject, -12f, 1f)
                .setEase(LeanTweenType.easeInOutSine);

            LeanTween
                .scale(_studioIconText.gameObject, Vector2.one, 0.7f)
                .setEase(LeanTweenType.easeInExpo)
                .setDelay(0.2f);

            // Play the studio card intro for three seconds
            yield return new WaitForSeconds(3);

            LeanTween
                .scale(_studioIconImage.gameObject, Vector2.zero, .4f)
                .setEase(LeanTweenType.easeOutBounce);

            LeanTween
                .scale(_studioIconText.gameObject, Vector2.zero, 0.4f)
                .setEase(LeanTweenType.easeOutSine)
                .setDelay(0.2f);

            yield return new WaitForSeconds(0.6f);

            // Start an asynchronous operation to load the scene
            var asyncSceneLoad = SceneManager.LoadSceneAsync(_sceneToLoadOnFinish);

            // Don't lead the scene start until all Studio Banks have finished loading
            asyncSceneLoad.allowSceneActivation = false;

            // Iterate all the Studio Banks and start them loading in the background
            // including the audio sample data
            foreach (var bank in _banks) RuntimeManager.LoadBank(bank, true);

            // Keep yielding the co-routine until all the Bank loading is done
            while (RuntimeManager.AnySampleDataLoading()) yield return null;

            // Allow the scene to be activated. This means that any OnActivated() or Start()
            // methods will be guaranteed that all FMOD Studio loading will be completed and
            // there will be no delay in starting events
            asyncSceneLoad.allowSceneActivation = true;

            // Keep yielding the co-routine until scene loading and activation is done.
            while (!asyncSceneLoad.isDone) yield return null;
        }
    }
}