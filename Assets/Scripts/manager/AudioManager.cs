using FMODUnity;
using UnityEngine;

namespace Core
{
    public class AudioManager : MonoBehaviour
    {
        private void Awake()
        {
            DontDestroyOnLoad(this);
        }

        public static void UpdateBus()
        {
            var musicBus = RuntimeManager.GetBus("bus:/MUSIC");
            musicBus.setVolume(GameSettingsManager.muteMusic ? 0f : 1f);

            var soundsBus = RuntimeManager.GetBus("bus:/SFX");
            var uiBus = RuntimeManager.GetBus("bus:/UI");
            soundsBus.setVolume(GameSettingsManager.muteSounds ? 0f : 1f);
            uiBus.setVolume(GameSettingsManager.muteSounds ? 0f : 1f);
        }
    }
}