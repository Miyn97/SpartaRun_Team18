using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraScroll : MonoBehaviour
{
    [Header("카메라 위치")]
    public Transform startPoint; // 시작 지점 = y값만 이동 위에서 부터 아래로
    public Transform endPoint; // 마지막 지점
    //public float scrollSpeed = 0.03f; // 이동 속도 (일정속도 구현시에만 필요)

    [Header("스크롤하는데 걸리는 시간")]
    public float scrollTime = 5f; // 이동 시간

    private float endTime = 0f; // 경과 시간
    private bool isScrolling = true;

    private Vector3 startPositionY;
    private Vector3 endPositionY;

    void Start()
    {
        // 카메라의 시작 위치를 설정
        transform.position = new Vector3(transform.position.x, startPoint.position.y, transform.position.z);

        //카메라 시작 위치 설정을 Vector3로 설정
        startPositionY = transform.position;

        // 카메라의 끝 위치를 설정
        endPositionY = new Vector3(transform.position.x, endPoint.position.y, transform.position.z);
    }

    void Update()
    {
        // 카메라가 이동할 시작 지점과 마지막 지점의 y값을 비교하여 카메라가 이동할 수 있는지 확인
        if (!isScrolling) return;

        endTime += Time.deltaTime; // 경과 시간이 점점 증가
        float t = endTime / scrollTime; // 0에서 1로 증가하는 비율을 계산

        t = Mathf.Clamp01(t); // 비율 계산한 것을 0과 1사이의 값으로 제한

        // 감속 느낌을 위한 보간 처리
        // EasyOut 형태= 빠르게 시작해서 천천히 멈추는 느낌을 주는 방식을 사용해봄
        // t 는 0 부터 1까지 올라가는 것이 기본이라서, 1f - t로 감속 처리, 3f는 감속 속도 조절 값
        float easeOutT = 1f - Mathf.Pow(1f - t, 3f);

        // 카메라가 시작할 위치와 마지막 위치를 비교
        // easeOutT만큼 카메라의 y값을 이동시킴
        // Lerp = 선형보간 : (A, B, t) A와 B 사이의 값을 t 비율에 따라 계산
        transform.position = Vector3.Lerp(startPositionY, endPositionY, easeOutT);

        if (t >= 1f) // t 가 1이 되면
        {
            isScrolling = false; // 이동 멈춤
        }



        //// 기존 일정 속도 이동의 경우 아래 코드 사용, 감속 구현 성공하면, 주석 모두 삭제해도 됨.
        //// 아래 방향으로 이동
        //transform.position = Vector3.MoveTowards
        //    (
        //    transform.position, // 현재 위치
        //    // 어디로? 해당 포인트의 y값으로 이동
        //    new Vector3(transform.position.x, endPoint.position.y, transform.position.z),
        //    scrollSpeed // 이동 속도
        //    );

        //// 마지막 지점에 도착하면?
        //if (Vector3.Distance(transform.position, endPoint.position) < 1f)
        //{
        //    isScrolling = false; // 이동 멈춤
        //}
    }


}
