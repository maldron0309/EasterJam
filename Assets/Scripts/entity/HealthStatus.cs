namespace entity
{
    public class HealthStatus
    {
        public const int DEATH_THRESHOLD_AMOUNT = 0;
        public const int MAX_HEALTH_AMOUNT = 5;

        private const int BASE_HEALTH_AMOUNT = MAX_HEALTH_AMOUNT;
        public int CurrentHealth { get; set; } = BASE_HEALTH_AMOUNT;
    }
}