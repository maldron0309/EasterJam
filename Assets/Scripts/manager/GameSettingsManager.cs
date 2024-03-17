using UnityEngine;

namespace manager
{
    public static class GameSettingsManager
    {
        public static bool MuteMusic { get; private set; }
        public static bool MuteSounds { get; private set; }

        public static void Load()
        {
            if (HasBeenSetOnce())
            {
                MuteMusic = PlayerPrefs.GetInt("muteMusic") == 1;
                MuteSounds = PlayerPrefs.GetInt("muteSounds") == 1;
            }
            else
            {
                LoadDefaultValues();
            }
        }

        public static void Save()
        {
            PlayerPrefs.SetInt("muteMusic", MuteMusic ? 1 : 0);
            PlayerPrefs.SetInt("muteSounds", MuteSounds ? 1 : 0);
            AudioManager.UpdateBus();
        }

        private static void LoadDefaultValues()
        {
            MuteMusic = false;
            MuteSounds = false;
        }

        private static bool HasBeenSetOnce() => PlayerPrefs.HasKey("muteMusic");
    }
}