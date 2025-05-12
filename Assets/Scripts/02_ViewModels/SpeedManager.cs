using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Difficulty  // Difficulty라는 이름의 enum을 만들어 난이도 추가
{
    Basic,
    Standard,
    Challenge
}

public class SpeedManager : MonoBehaviour
{
    Difficulty difficulty = Difficulty.Basic; // 기본값 설정
    public float currentSpeed { get; private set; } // 난이도별 이동속도 

    private float speedIncreaseInterval = 0.5f; // 0.5초 마다 이동속도를 증가 시켜주는 speedIncreaseInterval 선언
    private float speedIncrement = 0.005f;

    private Coroutine speedCoroutine;

    void Start()
    {
        SetInitialSpeed();
        speedCoroutine = StartCoroutine(IncreaseSpeedOverTime()); // 코루틴을 통하여 이동속도 증가와 이동속도의 최대 한도값 제어
    }

    private IEnumerator IncreaseSpeedOverTime()
    {
        yield return new WaitForSeconds(speedIncreaseInterval);  // 게임이 시작된 후 speedIncreaseInterval(즉 0.5초)까지 대기하다 이동속도 값이 증가

        while (true)
        {
            currentSpeed += speedIncrement; // 난이도 별로 이동속도 값(0.005)이 증가
            yield return new WaitForSeconds(speedIncreaseInterval); // 다시 0.5초 대기 후 이동속도 값이 다시 증가
        }
    }

    private void SetInitialSpeed() // 난이도 별 이동속도 초기값 설정
    {
        switch (difficulty)
        {
            case Difficulty.Basic:
                currentSpeed = 3.0f; // 베이직
                break;
            case Difficulty.Standard:
                currentSpeed = 4.0f; // 스탠다드
                break;
            case Difficulty.Challenge:
                currentSpeed = 5.0f; // 챌린지
                break;
        }
    }
    public void StopIncreasingSpeed()
    {
        if (speedCoroutine != null) // speedCoroutine을 통하여 이동속도 값이 증가했을 경우 if 시작
        {
            StopCoroutine(speedCoroutine); // 이동속도가 증가된 후 Coroutine이 종료
        }
    }
}