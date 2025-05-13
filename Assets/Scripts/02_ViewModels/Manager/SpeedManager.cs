using System.Collections;
using UnityEngine;

public class SpeedManager : MonoBehaviour
{
    [SerializeField] private Difficulty difficulty = Difficulty.Basic; // �ν����Ϳ��� ���̵� ����

    private SpeedModel model;
    private ISpeedService speedService;

    private Coroutine speedCoroutine;
    private static readonly WaitForSeconds waitInterval = new WaitForSeconds(0.5f); // GC �ּ�ȭ�� ���� ����

    [SerializeField] private float speedIncrement = 0.005f; // 1ȸ ������ (�ν����Ϳ��� ���� ����)

    private void Awake()
    {
        // �� �� ���� �ʱ�ȭ
        model = new SpeedModel();
        speedService = new SpeedService();

        model.Difficulty = difficulty;

        // �ʱ� �ӵ� �� �ִ� �ӵ� ����
        model.CurrentSpeed = speedService.GetInitialSpeed(model.Difficulty); // ���� �߰�: �ʱ� �ӵ� ����
        model.MaxSpeed = speedService.GetMaxSpeed(model.Difficulty); // ���� �߰�: �ִ� �ӵ� ����
    }

    private void Start()
    {
        speedCoroutine = StartCoroutine(IncreaseSpeedOverTime()); // ���� �߰�: �ڷ�ƾ ����
    }

    private IEnumerator IncreaseSpeedOverTime()
    {
        while (model.CurrentSpeed < model.MaxSpeed)
        {
            yield return waitInterval; // GC ���ϱ� ���� static ���� ���

            model.CurrentSpeed += speedIncrement; // ���� �߰�: �ӵ� ����

            if (model.CurrentSpeed > model.MaxSpeed) // ���� �߰�: �ִ� �ӵ� ����
                model.CurrentSpeed = model.MaxSpeed;
        }

        speedCoroutine = null; // ���� �߰�: �ߺ� ���� ������ null ó��
    }

    public float GetCurrentSpeed() => model.CurrentSpeed; // ���� �߰�: �ܺο��� �ӵ� ��ȸ

    public void StopIncreasingSpeed()
    {
        if (speedCoroutine != null)
        {
            StopCoroutine(speedCoroutine); // ���� �߰�: �ڷ�ƾ �ߴ�
            speedCoroutine = null;
        }
    }
}
