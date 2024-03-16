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

        [MenuItem("Debug/Player/Gain 1 Mana", false, 0)]
        private static void Gain1Mana()
        {
            CheckpointService.Instance.GainMana(1);
        }

        [MenuItem("Debug/Player/Gain Random Mana", false, 0)]
        private static void GainRandomMana()
        {
            CheckpointService.Instance.GainMana(Random.Range(1, 5));
        }

        [MenuItem("Debug/Player/Consume 1 Mana", false, 0)]
        private static void Consume1Mana()
        {
            CheckpointService.Instance.ConsumeMana(1);
        }

        [MenuItem("Debug/Player/Consume Random Mana", false, 0)]
        private static void ConsumeRandomMana()
        {
            CheckpointService.Instance.ConsumeMana(Random.Range(1, 5));
        }

        [MenuItem("Debug/Player/Try Spawn Checkpoint", false, 0)]
        private static void TrySpawnCheckpoint()
        {
            CheckpointService.Instance.TrySpawnCheckpoint();
        }
    }
}
#endif