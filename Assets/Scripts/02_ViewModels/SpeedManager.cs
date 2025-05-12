using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Difficulty  // Difficulty��� �̸��� enum�� ����� ���̵� �߰�
{
    Basic,
    Standard,
    Challenge
}

public class SpeedManager : MonoBehaviour
{
    Difficulty difficulty = Difficulty.Basic; // �⺻�� ����
    public float currentSpeed { get; private set; } // ���̵��� �̵��ӵ� 

    private float speedIncreaseInterval = 0.5f; // 0.5�� ���� �̵��ӵ��� ���� �����ִ� speedIncreaseInterval ����
    private float speedIncrement = 0.005f;

    float MaxSpeed;

    private Coroutine speedCoroutine;

    void Start()
    {
        SetInitialSpeed();
        SetMaxSpeed();
        speedCoroutine = StartCoroutine(IncreaseSpeedOverTime()); // �ڷ�ƾ�� ���Ͽ� �̵��ӵ� ������ �̵��ӵ��� �ִ� �ѵ��� ����
    }
    private void SetInitialSpeed() // ���̵� �� �̵��ӵ� �ʱⰪ ����
    {
        switch (difficulty)
        {
            case Difficulty.Basic:
                currentSpeed = 3.0f; // ������
                break;
            case Difficulty.Standard:
                currentSpeed = 4.0f; // ���Ĵٵ�
                break;
            case Difficulty.Challenge:
                currentSpeed = 5.0f; // ç����
                break;
        }
    }

    private void SetMaxSpeed() // �̵��ӵ� ���Ѽ��� ����
    {
        switch (difficulty)
        {
            case Difficulty.Basic:
                MaxSpeed = 5.0f;
                break;
            case Difficulty.Standard:
                MaxSpeed = 6.0f;
                break;
            case Difficulty.Challenge:
                MaxSpeed = 7.0f;
                break;
        }
    }

    private IEnumerator IncreaseSpeedOverTime()
    {
        yield return new WaitForSeconds(speedIncreaseInterval);  // ������ ���۵� �� speedIncreaseInterval(�� 0.5��)���� ����ϴ� �̵��ӵ� ���� ����

        while (currentSpeed <= MaxSpeed) // currentSpeed(�̵��ӵ�)�� �����Ͽ� �ִ밪�� MaxSpeed�� ���� �Ҷ� ���� 
        {
            currentSpeed += speedIncrement; // ���̵� ���� �̵��ӵ� ��(0.005)�� ����

            if(currentSpeed >= MaxSpeed) // currentSpeed �� ������ �ݺ��Ͽ� MaxSpeed ���� �Ѱ� ���� ���
                currentSpeed = MaxSpeed; // MaxSpeed���� ���� ���� �ʵ��� ����

                yield return new WaitForSeconds(speedIncreaseInterval); // �ٽ� 0.5�� ��� �� �̵��ӵ� ���� �ٽ� ����
        }
        speedCoroutine = null; // �ߺ� ���� ������ ���� ���� ���� �� �� null�� ����
    }

    public void StopIncreasingSpeed() // StageManager, MapData���� ���
    {
        if (speedCoroutine != null) // speedCoroutine�� ���Ͽ� �̵��ӵ� ���� �������� ��� if ����
        {
            StopCoroutine(speedCoroutine); // �̵��ӵ��� ������ �� Coroutine�� ����
            speedCoroutine = null; // �ߺ� ���� ������ ���� ���� ���� �� �� null�� ����
        }
    }
}