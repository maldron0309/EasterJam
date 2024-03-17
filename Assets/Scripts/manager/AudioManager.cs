using FMODUnity;
using UnityEngine;

namespace manager
{
    public class AudioManager : MonoBehaviour
    {
        public static void UpdateBus()
        {
            var musicBus = RuntimeManager.GetBus("bus:/MUSIC");
            musicBus.setVolume(GameSettingsManager.MuteMusic ? 0f : 1f);

            var soundsBus = RuntimeManager.GetBus("bus:/SFX");
            var uiBus = RuntimeManager.GetBus("bus:/UI");
            soundsBus.setVolume(GameSettingsManager.MuteSounds ? 0f : 1f);
            uiBus.setVolume(GameSettingsManager.MuteSounds ? 0f : 1f);
        }
    }
}