using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
������ ����, ȿ����ġ ��
������ ���� �� ���Ǽ�ġ
�Ŵ�ȭ               �����ð����� ���� �� ũ�Ⱑ Ŀ����. 5�ʵ��� �Ŵ�ȭ?
����                 �����ð� ���� �������·� �����ӵ��� �޸���. 3�ʵ��� speed + 3f?
����=���θ���        ���� ���������� ��ֹ��� ����ȭ? ��ȿȭ �ϰ� ������ ���� �� �ִ� ��� ���� 
�ڼ�                 �����ð����� �÷��̾� �ֺ��� collectable << ������, ���� ���� ���Ƶ��δ� �÷��̾� ������ǥ�� 
ü�¹���             ü�� ȸ��. �ϴ��� ��ĭȸ��

���ε� ���� ����?
�÷��̾� ���� �ʿ�, ��ֹ� �ı�? ��Ȱ��ȭ? �ʿ�, 
 */

public enum ItemEnum
{
    GiantPotion,
    SpeedPotion,
    Bomb,
    magnet,
    healpotion
}


public class ItemModel : MonoBehaviour
{
    public ItemEnum itemenum;
    public float duration; // ���ӽð� (�ʿ� ���� ���� 0)

    public int healAmount; // ȸ���� (�� ����)

    public int Score;
}
