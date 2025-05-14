using UnityEngine;

//MonoBehaviour ��� ���� ����
public class CameraController
{
    //View�� ����� ����
    //ī�޶� ��ġ ���� ����� ���� �� �־�� �ϹǷ� �ʿ�
    private readonly CameraView view;
    //ī�޶� ����ٴ� ����� Transform
    private readonly Transform target;
    //��� ��ġ���� �󸶳� �������� ������ ���ϴ� ������
    private readonly Vector3 offset;
    //ī�޶� ����� ���󰡴� �ӵ�
    private readonly float followSpeed;

    //������
    public CameraController(CameraView view, Transform target, Vector3 offset, float followSpeed = 5f)
    {
        //�ʱ�ȭ
        this.view = view;
        this.target = target;
        this.offset = offset;
        this.followSpeed = followSpeed;
    }

    public void Update()
    {
        if (target == null) return;

        // Y���� �����ϰ� X, Z���� ����
        Vector3 desiredPos = new Vector3(
            target.position.x + offset.x,
            view.GetPosition().y, // Y�� ���� ī�޶� ��ġ ����
            target.position.z + offset.z
        );

        Vector3 smoothedPos = Vector3.Lerp(view.GetPosition(), desiredPos, Time.deltaTime * followSpeed);
        view.SetPosition(smoothedPos);
    }

}
