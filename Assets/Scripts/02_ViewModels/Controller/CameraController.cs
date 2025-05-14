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
        if (target == null) return;

        // Y값을 고정하고 X, Z값만 따라감
        Vector3 desiredPos = new Vector3(
            target.position.x + offset.x,
            view.GetPosition().y, // Y는 현재 카메라 위치 유지
            target.position.z + offset.z
        );

        Vector3 smoothedPos = Vector3.Lerp(view.GetPosition(), desiredPos, Time.deltaTime * followSpeed);
        view.SetPosition(smoothedPos);
    }

}
