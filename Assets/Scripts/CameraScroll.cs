using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraScroll : MonoBehaviour
{
    [Header("카메라 위치")]
    public Transform startPoint; // 시작 지점 = y값만 이동 위에서 부터 아래로
    public Transform endPoint; // 마지막 지점
    public float scrollSpeed = 0.01f; // 카메라 이동 속도

    private bool isScrolling = true;

    void Start()
    {
        // 카메라의 시작 위치를 설정
        transform.position = new Vector3(transform.position.x, startPoint.position.y, transform.position.z);
    }

    void Update()
    {
        // 카메라가 이동할 시작 지점과 마지막 지점의 y값을 비교하여 카메라가 이동할 수 있는지 확인
        if (!isScrolling) return;

        // 아래 방향으로 이동
        transform.position = Vector3.MoveTowards
            (
            transform.position, // 현재 위치
            // 어디로? 해당 포인트의 y값으로 이동
            new Vector3(transform.position.x, endPoint.position.y, transform.position.z),
            scrollSpeed // 이동 속도
            );

        // 마지막 지점에 도착하면?
        if (Vector3.Distance(transform.position, endPoint.position) < 1f)
        {
            isScrolling = false; // 이동 멈춤
        }
    }


}
