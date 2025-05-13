using UnityEngine;

// 스테이지에 대한 UnityView: MonoBehaviour를 담당
public class StageView : MonoBehaviour
{
    [SerializeField] private MapDataAsset mapDataAsset; // 인스펙터에서 연결할 ScriptableObject

    private StageViewModel _viewModel; // 내부 ViewModel 참조

    private void Awake()
    {
        _viewModel = new StageViewModel(mapDataAsset); // ViewModel 초기화
    }

    // 외부에서 호출하여 랜덤 청크를 받아오는 메서드
    public GameObject GenNextChunkPrefab()
    {
        return _viewModel.GenerateNextChunk(); // ViewModel에 위임
    }

    // 인스턴스에서 원본 프리팹을 찾는 메서드
    public GameObject FindOriginalPrefab(GameObject instance)
    {
        return _viewModel.FindOriginalPrefab(instance); // ViewModel에 위임
    }
}
