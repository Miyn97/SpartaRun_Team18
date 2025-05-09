using UnityEngine;

//MonoBehaviour�� ��ӹ��� �ʰ� ���� �����͸� ����
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

    //ü�� ���� �޼���
    public void TakeDamage(int amount)
    {
        //���� ü�� - amout
        //ü���� 0 �̸����� �������� �� ����
        //Mathf.Max�� ���Ѽ��� 0���� ����
        CurrentHealth = Mathf.Max(CurrentHealth - amount, 0);
    }

    //ȸ��
    public void Heal(int amount)
    {
        //���� ü�� + ȸ����
        //ü���� �ִ� ü���� �ʰ��ϴ� �� ����
        //Mathf.Min���� ���Ѽ��� MaxHealth�� ����
        CurrentHealth = Mathf.Min(CurrentHealth + amount, MaxHealth);
    }

    //���� �߰�
    public void AddScore(int value)
    {
        //�÷��̾� �� ���� ������ ����
        Score += value;
    }

    //�ӵ� ����
    public void SetSpeed(float newSpeed)
    {
        Speed = newSpeed;
    }
}
