using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraScroll : MonoBehaviour
{
    [Header("ī�޶� ��ġ")]
    public Transform startPoint; // ���� ���� = y���� �̵� ������ ���� �Ʒ���
    public Transform endPoint; // ������ ����
    //public float scrollSpeed = 0.03f; // �̵� �ӵ� (�����ӵ� �����ÿ��� �ʿ�)

    [Header("��ũ���ϴµ� �ɸ��� �ð�")]
    public float scrollTime = 5f; // �̵� �ð�

    private float endTime = 0f; // ��� �ð�
    private bool isScrolling = true;

    private Vector3 startPositionY;
    private Vector3 endPositionY;

    void Start()
    {
        // ī�޶��� ���� ��ġ�� ����
        transform.position = new Vector3(transform.position.x, startPoint.position.y, transform.position.z);

        //ī�޶� ���� ��ġ ������ Vector3�� ����
        startPositionY = transform.position;

        // ī�޶��� �� ��ġ�� ����
        endPositionY = new Vector3(transform.position.x, endPoint.position.y, transform.position.z);
    }

    void Update()
    {
        // ī�޶� �̵��� ���� ������ ������ ������ y���� ���Ͽ� ī�޶� �̵��� �� �ִ��� Ȯ��
        if (!isScrolling) return;

        endTime += Time.deltaTime; // ��� �ð��� ���� ����
        float t = endTime / scrollTime; // 0���� 1�� �����ϴ� ������ ���

        t = Mathf.Clamp01(t); // ���� ����� ���� 0�� 1������ ������ ����

        // ���� ������ ���� ���� ó��
        // EasyOut ����= ������ �����ؼ� õõ�� ���ߴ� ������ �ִ� ����� ����غ�
        // t �� 0 ���� 1���� �ö󰡴� ���� �⺻�̶�, 1f - t�� ���� ó��, 3f�� ���� �ӵ� ���� ��
        float easeOutT = 1f - Mathf.Pow(1f - t, 3f);

        // ī�޶� ������ ��ġ�� ������ ��ġ�� ��
        // easeOutT��ŭ ī�޶��� y���� �̵���Ŵ
        // Lerp = �������� : (A, B, t) A�� B ������ ���� t ������ ���� ���
        transform.position = Vector3.Lerp(startPositionY, endPositionY, easeOutT);

        if (t >= 1f) // t �� 1�� �Ǹ�
        {
            isScrolling = false; // �̵� ����
        }



        //// ���� ���� �ӵ� �̵��� ��� �Ʒ� �ڵ� ���, ���� ���� �����ϸ�, �ּ� ��� �����ص� ��.
        //// �Ʒ� �������� �̵�
        //transform.position = Vector3.MoveTowards
        //    (
        //    transform.position, // ���� ��ġ
        //    // ����? �ش� ����Ʈ�� y������ �̵�
        //    new Vector3(transform.position.x, endPoint.position.y, transform.position.z),
        //    scrollSpeed // �̵� �ӵ�
        //    );

        //// ������ ������ �����ϸ�?
        //if (Vector3.Distance(transform.position, endPoint.position) < 1f)
        //{
        //    isScrolling = false; // �̵� ����
        //}
    }


}
