using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour
{
    //�浹�� ȿ�� ���� �� �ı�ó��, ������ ��ƴٸ� �ڵ� ������
    //�����ۿ� �� ��ũ��Ʈ�� ������ �� �ְ� is trigger �ѵα�
    //
    private ItemManager pool;
    private void Awake()
    {
        pool = FindObjectOfType<ItemManager>();
    }

    // �÷��̾ Ʈ���ſ� ������ �� ����Ǵ� �Լ�
    private void OnTriggerEnter2D(Collider2D other)//OnTriggerEnter2D�� 2D Trigger �浹�� �Ͼ�� �� �ڵ����� ȣ�����ִ� ����Ƽ ���� �Լ�
    {
        if (other.CompareTag("Player"))//���� �浹�� ���ӿ�����Ʈ�� �±װ� �÷��̾���
        {
            ItemModel item = GetComponent<ItemModel>();
            if (item != null)
            {
                item.ApplyEffect();  // �����۸𵨿� �ִ� ȿ�� �ߵ�

                pool.ReturnToPool(item.itemEnum, gameObject);//��Ʈ����, SetActiveFalse ��� ������ Ǯ�� Ǯ�� ��ȯ.
            }
        }
    }
}
