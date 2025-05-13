using UnityEngine;

public class CameraManager : MonoBehaviour
{
    //Inspector에서 연결할 수 있게 CameraView 참조를 가져옴
    [SerializeField] private CameraView cameraView;
    //카메라가 따라갈 대상(Player)의 Transform을 Inspector에서 연결
    [SerializeField] private Transform playerTarget;

    //ViewModel인 CameraController를 여기에 저장해두고 사용
    private CameraController cameraController;

    //Inspector에 보여줄 수 있는 카메라 설정값.
    //오프셋과 따라가는 속도 조정 가능 (이것도 배운내용)
    [Header("Camera Settings")]
    [SerializeField] private Vector3 offset = new Vector3(0, 2f, -10f);
    [SerializeField] private float followSpeed = 5f;

    private void Awake()
    {
        if (cameraView == null)
            cameraView = GetComponent<CameraView>(); // 자동으로 붙은 컴포넌트 가져오기
        //CameraView와 Target, 속도 등을 ViewModel인 CameraController에 넘겨 초기화
        cameraController = new CameraController(cameraView, playerTarget, offset, followSpeed);
    }

    //모든 이동과 애니메이션이 끝난 뒤 실행 (외워두자 LateUpdate)
    private void LateUpdate()
    {
        //ViewModel Update()를 호출함
        //매 프레임 카메라 위치를 계산하고 View에다가 적용
        cameraController.Update();
    }
}
