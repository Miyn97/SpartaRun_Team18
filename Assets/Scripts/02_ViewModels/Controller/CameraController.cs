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
        //����� ������ ī�޶� �������� �����Ƿ� ���� (���� ������ �ڵ� ^^)
        if (target == null) return;

        //ī�޶� ���󰡾� �� �̻����� ��ġ ��� (�������� ��� ��ġ�� ����)
        Vector3 desiredPos = target.position + offset;
        //�츮�� ����� Lerp ���� �Լ�. �� ī�޶� ��ġ���� ��ǥ ��ġ���� ���������� �̵�
        Vector3 smoothedPos = Vector3.Lerp(view.GetPosition(), desiredPos, Time.deltaTime * followSpeed);
        //View���ٰ� "ī�޶� ��ġ�� ����� �Ű����" ��� ���
        view.SetPosition(smoothedPos);
    }
}
