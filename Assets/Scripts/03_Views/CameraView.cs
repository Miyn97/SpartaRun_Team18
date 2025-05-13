using UnityEngine;

public class CameraView : MonoBehaviour
{
    //ī�޶��� Transform ������ �����ص� ����
    //��ġ ������ ������ �ϱ����� ���� ĳ��
    private Transform camTransform;

    private void Awake()
    {
        //�� ������Ʈ�� Transform�� ������ ����
        //���� ����ȭ�� ���� ĳ��
        camTransform = transform;
    }

    //�ܺο��� ī�޶� ��ġ�� [����]�� �� �ְ� ���ִ� �޼���
    //ViewModel�� Setposition �޼��带 ȣ���� ����
    public void SetPosition(Vector3 newPosition)
    {
        //���� ī�޶� ������Ʈ�� ��ġ�� ���ο� ������ ����
        camTransform.position = newPosition;
    }

    //�ܺο��� ���� ī�޶� ��ġ�� [������ �� �ֵ���] ���� �޼���
    public Vector3 GetPosition()
    {
        //���� ī�޶� ��ġ ��ȯ
        return camTransform.position;
    }
}
