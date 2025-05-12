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

    float MaxSpeed;

    private Coroutine speedCoroutine;

    void Start()
    {
        SetInitialSpeed();
        SetMaxSpeed();
        speedCoroutine = StartCoroutine(IncreaseSpeedOverTime()); // 코루틴을 통하여 이동속도 증가와 이동속도의 최대 한도값 제어
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

    private void SetMaxSpeed() // 이동속도 상한선값 제어
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
        yield return new WaitForSeconds(speedIncreaseInterval);  // 게임이 시작된 후 speedIncreaseInterval(즉 0.5초)까지 대기하다 이동속도 값이 증가

        while (currentSpeed <= MaxSpeed) // currentSpeed(이동속도)가 증가하여 최대값인 MaxSpeed에 도달 할때 까지 
        {
            currentSpeed += speedIncrement; // 난이도 별로 이동속도 값(0.005)이 증가

            if(currentSpeed >= MaxSpeed) // currentSpeed 가 증가를 반복하여 MaxSpeed 보다 넘게 됫을 경우
                currentSpeed = MaxSpeed; // MaxSpeed보다 증가 되지 않도록 보정

                yield return new WaitForSeconds(speedIncreaseInterval); // 다시 0.5초 대기 후 이동속도 값이 다시 증가
        }
        speedCoroutine = null; // 중복 실행 방지를 위해 값이 증가 된 후 null값 적용
    }

    public void StopIncreasingSpeed() // StageManager, MapData에서 사용
    {
        if (speedCoroutine != null) // speedCoroutine을 통하여 이동속도 값이 증가했을 경우 if 시작
        {
            StopCoroutine(speedCoroutine); // 이동속도가 증가된 후 Coroutine이 종료
            speedCoroutine = null; // 중복 실행 방지를 위해 값이 증가 된 후 null값 적용
        }
    }
}