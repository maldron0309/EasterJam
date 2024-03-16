using System;
using entity;
using UnityEngine.Assertions;

namespace service
{
    /// <summary>
    ///     Allows scripts to manipulate player health by offering methods to damage and heal HP.
    /// </summary>
    public class PlayerHealthService
    {
        private readonly HealthStatus _healthStatus;

        public PlayerHealthService()
        {
            Assert.IsNull(Instance, "The singleton has already been set up.");
            Instance = this;
            _healthStatus = new();
        }

        /// <summary>
        ///     The singleton instance of the service
        /// </summary>
        public static PlayerHealthService Instance { get; private set; }

        /// <summary>
        ///     Triggered when the player has suffered damage.
        ///     The int parameter is the remaining amount of health.
        /// </summary>
        public event Action<int> OnDamageSuffered;

        /// <summary>
        ///     Triggered when the player has died.
        /// </summary>
        public event Action OnDeath;

        /// <summary>
        ///     Triggered when the player has healed.
        ///     The int parameter is the amount of health after having healed.
        /// </summary>
        public event Action<int> OnHealed;

        /// <summary>
        ///     Allows scripts to damage the player by the specified amount.
        ///     Triggers the <see cref="OnDamageSuffered" /> event after execution.
        ///     If the damage suffered ended up killing the player, the <see cref="OnDeath" /> event is called.
        /// </summary>
        /// <param name="amount">The positive amount of damage that should be suffered</param>
        public void SufferDamage(int amount)
        {
            Assert.IsTrue(amount > 0, "Should not take zero or less damage");

            _healthStatus.CurrentHealth -= amount;
            OnDamageSuffered?.Invoke(amount);

            if (_healthStatus.CurrentHealth > HealthStatus.DEATH_THRESHOLD_AMOUNT) return;

            OnDeath?.Invoke();
        }

        /// <summary>
        ///     Heals for the specified amount of points.
        ///     Triggers <see cref="OnHealed" /> event.
        /// </summary>
        /// <param name="amount">the positive amount of points that should be healed for</param>
        public void Heal(int amount)
        {
            Assert.IsTrue(amount > 0, "Should not heal for non-positive amounts");

            _healthStatus.CurrentHealth += amount;

            if (_healthStatus.CurrentHealth >= HealthStatus.MAX_HEALTH_AMOUNT)
                _healthStatus.CurrentHealth = HealthStatus.MAX_HEALTH_AMOUNT;

            OnHealed?.Invoke(_healthStatus.CurrentHealth);
        }
    }
}