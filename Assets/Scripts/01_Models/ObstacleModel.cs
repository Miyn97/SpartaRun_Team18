using System.Collections.Generic; // Dictionary ����� ���� ���ӽ����̽�

// ��ֹ� ���� enum ����
public enum ObstacleType
{
    RedLineTrap,          // ���� ��ֹ� (���� ȸ��)
    SyntaxErrorBox,       // ���� ��ֹ� (���� ���� ȸ��)
    CompileErrorWall,     // ���� ��ֹ� (�����̵� ȸ��)
    UnhandledExceptionBox // ���� ���� (���� Ȯ�� ����)
}

// ��ֹ� ȸ�� ��� enum ����
public enum AvoidType
{
    Jump,                 // ����
    DoubleJump,           // ���� ����
    Slide                 // �����̵�
}

// ��ֹ� �ϳ��� �����͸� ��� ���� �� Ŭ���� (MonoBehaviour ����)
public class ObstacleModel
{
    public ObstacleType Type { get; private set; } // ��ֹ� ���� ����
    public AvoidType Avoid { get; private set; }   // ȸ�� ��� ����
    public int Damage { get; private set; }        // ���ط� ����

    // ��ֹ� ������ ȸ�� ����� ���� static readonly Dictionary
    private static readonly Dictionary<ObstacleType, AvoidType> AvoidTypeMap = new()
    {
        { ObstacleType.RedLineTrap, AvoidType.Jump },          // ���� ��ֹ�
        { ObstacleType.SyntaxErrorBox, AvoidType.DoubleJump }, // ���� ��ֹ�
        { ObstacleType.CompileErrorWall, AvoidType.Slide },    // ���� ��ֹ�
        { ObstacleType.UnhandledExceptionBox, AvoidType.Jump } // ���� ����
    };

    // ��ֹ� ������ ���ط��� ���� static readonly Dictionary
    private static readonly Dictionary<ObstacleType, int> DamageMap = new()
    {
        { ObstacleType.RedLineTrap, 1 },           // ���� ��ֹ� ������
        { ObstacleType.SyntaxErrorBox, 1 },        // ���� ��ֹ� ������
        { ObstacleType.CompileErrorWall, 1 },      // ���� ��ֹ� ������
        { ObstacleType.UnhandledExceptionBox, 6 } // ���� ���� (���)
    };

    // ������ - ��ֹ� ������ �޾� ������ �ڵ� �ʱ�ȭ
    public ObstacleModel(ObstacleType obstacleType)
    {
        Type = obstacleType; // ���޹��� Ÿ�� ����

        // Dictionary���� ���ΰ� �������� - ���ǵ��� ���� ��� �⺻�� ó��
        Damage = DamageMap.TryGetValue(Type, out int dmg) ? dmg : 0; // �����ϸ� ������, �ƴϸ� 0
        Avoid = AvoidTypeMap.TryGetValue(Type, out AvoidType at) ? at : 0; // �����ϸ� ȸ�� ���, �ƴϸ� �⺻��
    }
}
