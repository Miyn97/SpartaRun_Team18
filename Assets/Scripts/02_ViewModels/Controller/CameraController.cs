using UnityEngine;

//MonoBehaviour 상속 받지 않음
public class CameraController
{
    //View와 연결된 참조
    //카메라 위치 변경 명령을 내릴 수 있어야 하므로 필요
    private readonly CameraView view;
    //카메라가 따라다닐 대상의 Transform
    private readonly Transform target;
    //대상 위치에서 얼마나 떨어져서 따라갈지 정하는 오프셋
    private readonly Vector3 offset;
    //카메라가 대상을 따라가는 속도
    private readonly float followSpeed;

    //생성자
    public CameraController(CameraView view, Transform target, Vector3 offset, float followSpeed = 5f)
    {
        //초기화
        this.view = view;
        this.target = target;
        this.offset = offset;
        this.followSpeed = followSpeed;
    }

    public void Update()
    {
        //대상이 없으면 카메라 움직임이 없으므로 종료 (예외 방지용 코드 ^^)
        if (target == null) return;

        //카메라가 따라가야 할 이상적인 위치 계산 (오프셋을 대상 위치에 더함)
        Vector3 desiredPos = target.position + offset;
        //우리가 배웠던 Lerp 보간 함수. 현 카메라 위치에서 목표 위치까지 점진적으로 이동
        Vector3 smoothedPos = Vector3.Lerp(view.GetPosition(), desiredPos, Time.deltaTime * followSpeed);
        //View에다가 "카메라 위치를 여기로 옮겨줘라" 라고 명령
        view.SetPosition(smoothedPos);
    }
}
