namespace entity
{
    public class CheckpointStatus
    {
        public const int MAX_CHECKPOINT_MANA_AMOUNT = 3;
        public const int MANA_REQUIRED_TO_SPAWN_CHECKPOINT = 1;
        private const int STARTER_CHECKPOINT_MANA_AMOUNT = MAX_CHECKPOINT_MANA_AMOUNT;

        public const int EMPTY_MANA_THRESHOLD = 0;

        public int CurrentCheckpointMana { get; set; } = STARTER_CHECKPOINT_MANA_AMOUNT;
    }
}