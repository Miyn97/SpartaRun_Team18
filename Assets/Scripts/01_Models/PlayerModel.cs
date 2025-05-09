using UnityEngine;

//MonoBehaviour를 상속받지 않고 순수 데이터만 저장
public class PlayerModel
{
    public int MaxHealth { get; private set; }
    public int CurrentHealth { get; private set; }
    public float Speed { get; private set; }
    public int Score { get; private set; }

    public PlayerModel(int maxHealth, float speed)
    {
        MaxHealth = maxHealth;
        CurrentHealth = maxHealth;
        Speed = speed;
        Score = 0;
    }

    //체력 감소 메서드
    public void TakeDamage(int amount)
    {
        //현재 체력 - amout
        //체력이 0 미만으로 떨어지는 걸 방지
        //Mathf.Max로 하한선을 0으로 고정
        CurrentHealth = Mathf.Max(CurrentHealth - amount, 0);
    }

    //회복
    public void Heal(int amount)
    {
        //현재 체력 + 회복량
        //체력이 최대 체력을 초과하는 걸 방지
        //Mathf.Min으로 상한선을 MaxHealth로 고정
        CurrentHealth = Mathf.Min(CurrentHealth + amount, MaxHealth);
    }

    //점수 추가
    public void AddScore(int value)
    {
        //플레이어 한 명의 점수만 저장
        Score += value;
    }

    //속도 변경
    public void SetSpeed(float newSpeed)
    {
        Speed = newSpeed;
    }
}
