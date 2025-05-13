using UnityEngine;

// ���������� ���� UnityView: MonoBehaviour�� ���
public class StageView : MonoBehaviour
{
    [SerializeField] private MapDataAsset mapDataAsset; // �ν����Ϳ��� ������ ScriptableObject

    private StageViewModel _viewModel; // ���� ViewModel ����

    private void Awake()
    {
        _viewModel = new StageViewModel(mapDataAsset); // ViewModel �ʱ�ȭ
    }

    // �ܺο��� ȣ���Ͽ� ���� ûũ�� �޾ƿ��� �޼���
    public GameObject GenNextChunkPrefab()
    {
        return _viewModel.GenerateNextChunk(); // ViewModel�� ����
    }

    // �ν��Ͻ����� ���� �������� ã�� �޼���
    public GameObject FindOriginalPrefab(GameObject instance)
    {
        return _viewModel.FindOriginalPrefab(instance); // ViewModel�� ����
    }
}
