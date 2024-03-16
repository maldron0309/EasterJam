#if UNITY_EDITOR
using service;
using UnityEditor;
using UnityEngine;

namespace editor
{
    public class EditorDropdown : EditorWindow
    {
        [MenuItem("Debug/Player/Suffer 1 Damage", false, 0)]
        private static void Suffer1Damage()
        {
            PlayerHealthService.Instance.SufferDamage(1);
        }

        [MenuItem("Debug/Player/Suffer Random Damage", false, 0)]
        private static void SufferRandomDamage()
        {
            PlayerHealthService.Instance.SufferDamage(Random.Range(1, 5));
        }

        [MenuItem("Debug/Player/Heal 1 Damage", false, 0)]
        private static void Heal1Damage()
        {
            PlayerHealthService.Instance.Heal(1);
        }

        [MenuItem("Debug/Player/Heal Random Damage", false, 0)]
        private static void HealRandomDamage()
        {
            PlayerHealthService.Instance.Heal(Random.Range(1, 5));
        }
    }
}
#endif