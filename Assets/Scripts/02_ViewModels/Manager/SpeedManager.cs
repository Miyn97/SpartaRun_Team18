using System.Collections;
using UnityEngine;

public class SpeedManager : MonoBehaviour
{
    [SerializeField] private Difficulty difficulty = Difficulty.Basic; // 인스펙터에서 난이도 설정

    private SpeedModel model;
    private ISpeedService speedService;

    private Coroutine speedCoroutine;
    private static readonly WaitForSeconds waitInterval = new WaitForSeconds(0.5f); // GC 최소화를 위해 재사용

    [SerializeField] private float speedIncrement = 0.005f; // 1회 증가량 (인스펙터에서 조절 가능)

    private void Awake()
    {
        // 모델 및 서비스 초기화
        model = new SpeedModel();
        speedService = new SpeedService();

        model.Difficulty = difficulty;

        // 초기 속도 및 최대 속도 설정
        model.CurrentSpeed = speedService.GetInitialSpeed(model.Difficulty); // 한줄 추가: 초기 속도 설정
        model.MaxSpeed = speedService.GetMaxSpeed(model.Difficulty); // 한줄 추가: 최대 속도 설정
    }

    private void Start()
    {
        speedCoroutine = StartCoroutine(IncreaseSpeedOverTime()); // 한줄 추가: 코루틴 시작
    }

    private IEnumerator IncreaseSpeedOverTime()
    {
        while (model.CurrentSpeed < model.MaxSpeed)
        {
            yield return waitInterval; // GC 피하기 위해 static 변수 사용

            model.CurrentSpeed += speedIncrement; // 한줄 추가: 속도 증가

            if (model.CurrentSpeed > model.MaxSpeed) // 한줄 추가: 최대 속도 보정
                model.CurrentSpeed = model.MaxSpeed;
        }

        speedCoroutine = null; // 한줄 추가: 중복 실행 방지용 null 처리
    }

    public float GetCurrentSpeed() => model.CurrentSpeed; // 한줄 추가: 외부에서 속도 조회

    public void StopIncreasingSpeed()
    {
        if (speedCoroutine != null)
        {
            StopCoroutine(speedCoroutine); // 한줄 추가: 코루틴 중단
            speedCoroutine = null;
        }
    }
}
