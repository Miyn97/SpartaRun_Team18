using UnityEngine;

public class CameraView : MonoBehaviour
{
    //카메라의 Transform 정보를 저장해둘 변수
    //위치 변경을 빠르게 하기위해 따로 캐싱
    private Transform camTransform;

    private void Awake()
    {
        //이 오브젝트의 Transform을 변수에 저장
        //성능 최적화를 위해 캐싱
        camTransform = transform;
    }

    //외부에서 카메라 위치를 [지정]할 수 있게 해주는 메서드
    //ViewModel이 Setposition 메서드를 호출할 예정
    public void SetPosition(Vector3 newPosition)
    {
        //실제 카메라 오브젝트의 위치를 새로운 값으로 설정
        camTransform.position = newPosition;
    }

    //외부에서 현재 카메라 위치를 [가져올 수 있도록] 만든 메서드
    public Vector3 GetPosition()
    {
        //현재 카메라 위치 반환
        return camTransform.position;
    }
}
