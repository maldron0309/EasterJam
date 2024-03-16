using System;
using entity;
using mono.spawner;
using UnityEngine.Assertions;

namespace service
{
    /// <summary>
    ///     Allows scripts to increase and decrease mana required to spawn checkpoints.
    /// </summary>
    public class CheckpointService
    {
        private readonly CheckpointStatus _checkpointStatus;

        public CheckpointService()
        {
            Assert.IsNull(Instance, "Checkpoint service has already been set up!");
            Instance = this;
            _checkpointStatus = new();
        }

        /// <summary>
        ///     The singleton instance of the service
        /// </summary>
        public static CheckpointService Instance { get; private set; }

        /// <summary>
        ///     Triggered when the player gains any amount of checkpoint mana.
        ///     The int parameter is the amount of mana after having gained mana.
        /// </summary>
        public event Action<int> OnManaGained;

        /// <summary>
        ///     Triggered when the player loses any amount of checkpoint mana.
        ///     The int parameter is the amount of mana left after having lost mana.
        /// </summary>
        public event Action<int> OnManaLost;

        /// <summary>
        ///     Triggered when the player consumes mana to spawn a checkpoint at the position of the player
        /// </summary>
        public event Action OnCheckpointSpawned;

        /// <summary>
        ///     Increases the amount of checkpoint mana by the respective amount.
        ///     Also triggers the <see cref="OnManaGained" /> event.
        /// </summary>
        /// <param name="amount">the positive amount of mana that should be gained</param>
        public void GainMana(int amount)
        {
            Assert.IsTrue(amount > 0, "Should only increase checkpoint mana by positive amounts");

            _checkpointStatus.CurrentCheckpointMana += amount;
            if (_checkpointStatus.CurrentCheckpointMana > CheckpointStatus.MAX_CHECKPOINT_MANA_AMOUNT)
                _checkpointStatus.CurrentCheckpointMana = CheckpointStatus.MAX_CHECKPOINT_MANA_AMOUNT;

            OnManaGained?.Invoke(amount);
        }

        /// <summary>
        ///     Decreases the amount of checkpoint mana by the respective amount.
        ///     Also triggers the <see cref="OnManaLost" /> event.
        /// </summary>
        /// <param name="amount">the positive amount of mana that should be lost</param>
        public void ConsumeMana(int amount)
        {
            Assert.IsTrue(amount > 0, "Should only decrease checkpoint mana by positive amounts");

            _checkpointStatus.CurrentCheckpointMana -= amount;
            if (_checkpointStatus.CurrentCheckpointMana <= CheckpointStatus.EMPTY_MANA_THRESHOLD)
                _checkpointStatus.CurrentCheckpointMana = CheckpointStatus.EMPTY_MANA_THRESHOLD;

            OnManaLost?.Invoke(amount);
        }

        public void TrySpawnCheckpoint()
        {
            // TODO return out if player is not grounded
            if (_checkpointStatus.CurrentCheckpointMana <= CheckpointStatus.EMPTY_MANA_THRESHOLD) return;

            ConsumeMana(CheckpointStatus.MANA_REQUIRED_TO_SPAWN_CHECKPOINT);
            CheckpointSpawner.Instance.SpawnCheckpoint();

            OnCheckpointSpawned?.Invoke();
        }
    }
}