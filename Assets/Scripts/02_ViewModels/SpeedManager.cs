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

    private Coroutine speedCoroutine;

    void Start()
    {
        SetInitialSpeed();
        speedCoroutine = StartCoroutine(IncreaseSpeedOverTime()); // �ڷ�ƾ�� ���Ͽ� �̵��ӵ� ������ �̵��ӵ��� �ִ� �ѵ��� ����
    }

    private IEnumerator IncreaseSpeedOverTime()
    {
        yield return new WaitForSeconds(speedIncreaseInterval);  // ������ ���۵� �� speedIncreaseInterval(�� 0.5��)���� ����ϴ� �̵��ӵ� ���� ����

        while (true)
        {
            currentSpeed += speedIncrement; // ���̵� ���� �̵��ӵ� ��(0.005)�� ����
            yield return new WaitForSeconds(speedIncreaseInterval); // �ٽ� 0.5�� ��� �� �̵��ӵ� ���� �ٽ� ����
        }
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
    public void StopIncreasingSpeed()
    {
        if (speedCoroutine != null) // speedCoroutine�� ���Ͽ� �̵��ӵ� ���� �������� ��� if ����
        {
            StopCoroutine(speedCoroutine); // �̵��ӵ��� ������ �� Coroutine�� ����
        }
    }
}