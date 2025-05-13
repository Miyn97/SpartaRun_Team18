public interface ISpeedService
{
    void Initialize(Difficulty difficulty);
    float GetInitialSpeed(Difficulty difficulty);
    float GetMaxSpeed(Difficulty difficulty);
}
