using UnityEngine;

public class CameraManager : MonoBehaviour
{
    //Inspector���� ������ �� �ְ� CameraView ������ ������
    [SerializeField] private CameraView cameraView;
    //ī�޶� ���� ���(Player)�� Transform�� Inspector���� ����
    [SerializeField] private Transform playerTarget;

    //ViewModel�� CameraController�� ���⿡ �����صΰ� ���
    private CameraController cameraController;

    //Inspector�� ������ �� �ִ� ī�޶� ������.
    //�����°� ���󰡴� �ӵ� ���� ���� (�̰͵� ����)
    [Header("Camera Settings")]
    [SerializeField] private Vector3 offset = new Vector3(0, 2f, -10f);
    [SerializeField] private float followSpeed = 5f;

    private void Awake()
    {
        if (cameraView == null)
            cameraView = GetComponent<CameraView>(); // �ڵ����� ���� ������Ʈ ��������
        //CameraView�� Target, �ӵ� ���� ViewModel�� CameraController�� �Ѱ� �ʱ�ȭ
        cameraController = new CameraController(cameraView, playerTarget, offset, followSpeed);
    }

    //��� �̵��� �ִϸ��̼��� ���� �� ���� (�ܿ����� LateUpdate)
    private void LateUpdate()
    {
        //ViewModel Update()�� ȣ����
        //�� ������ ī�޶� ��ġ�� ����ϰ� View���ٰ� ����
        cameraController.Update();
    }
}
