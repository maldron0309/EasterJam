using UnityEngine;

namespace Core
{
    public static class GameSettingsManager
    {
        public static bool muteMusic;
        public static bool muteSounds;

        public static void Load()
        {
            if (HasBeenSetOnce())
            {
                muteMusic = PlayerPrefs.GetInt("muteMusic") == 1;
                muteSounds = PlayerPrefs.GetInt("muteSounds") == 1;
            }
            else
            {
                LoadDefaultValues();
            }
        }

        public static void Save()
        {
            PlayerPrefs.SetInt("muteMusic", muteMusic ? 1 : 0);
            PlayerPrefs.SetInt("muteSounds", muteSounds ? 1 : 0);
            AudioManager.UpdateBus();
        }

        public static void LoadDefaultValues()
        {
            muteMusic = false;
            muteSounds = false;
        }

        private static bool HasBeenSetOnce()
        {
            return PlayerPrefs.HasKey("muteMusic");
        }
    }
}