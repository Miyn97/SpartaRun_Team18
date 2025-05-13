public class SpeedService : ISpeedService
{
    public void Initialize(Difficulty difficulty) { }

    public float GetInitialSpeed(Difficulty difficulty)
    {
        switch (difficulty)
        {
            case Difficulty.Standard: return 4.0f;
            case Difficulty.Challenge: return 5.0f;
            default: return 3.0f; // Basic
        }
    }

    public float GetMaxSpeed(Difficulty difficulty)
    {
        switch (difficulty)
        {
            case Difficulty.Standard: return 6.0f;
            case Difficulty.Challenge: return 7.0f;
            default: return 5.0f; // Basic
        }
    }
}
