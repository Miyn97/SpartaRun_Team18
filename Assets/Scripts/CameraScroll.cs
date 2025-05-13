using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraScroll : MonoBehaviour
{
    [Header("ī�޶� ��ġ")]
    public Transform startPoint; // ���� ���� = y���� �̵� ������ ���� �Ʒ���
    public Transform endPoint; // ������ ����
    public float scrollSpeed = 0.01f; // ī�޶� �̵� �ӵ�

    private bool isScrolling = true;

    void Start()
    {
        // ī�޶��� ���� ��ġ�� ����
        transform.position = new Vector3(transform.position.x, startPoint.position.y, transform.position.z);
    }

    void Update()
    {
        // ī�޶� �̵��� ���� ������ ������ ������ y���� ���Ͽ� ī�޶� �̵��� �� �ִ��� Ȯ��
        if (!isScrolling) return;

        // �Ʒ� �������� �̵�
        transform.position = Vector3.MoveTowards
            (
            transform.position, // ���� ��ġ
            // ����? �ش� ����Ʈ�� y������ �̵�
            new Vector3(transform.position.x, endPoint.position.y, transform.position.z),
            scrollSpeed // �̵� �ӵ�
            );

        // ������ ������ �����ϸ�?
        if (Vector3.Distance(transform.position, endPoint.position) < 1f)
        {
            isScrolling = false; // �̵� ����
        }
    }


}
