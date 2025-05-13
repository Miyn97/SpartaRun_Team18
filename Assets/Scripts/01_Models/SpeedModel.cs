public class SpeedModel
{
    public Difficulty Difficulty { get; set; } = Difficulty.Basic; // 기본 난이도 설정
    public float CurrentSpeed { get; set; }                        // 현재 이동속도
    public float MaxSpeed { get; set; }                            // 최대 이동속도
}

