using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

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
    Coin,
    HealPotion,
    SpeedPotion,
    GiantPotion,
    Magnet
    //Bomb
}

public class ItemModel : MonoBehaviour
{
    private ItemManager pool;
    private void Awake()
    {
        pool = FindObjectOfType<ItemManager>();
        playerController = FindAnyObjectByType<PlayerController>();
    }
    UIManager UIManager;

    public ItemEnum itemEnum;
    [SerializeField] private PlayerController playerController;
    [SerializeField] private PlayerModel model;

    public void ApplyEffect()//�������� ������
    {
        Debug.Log("������ ȿ�� �����: " + itemEnum);
        GameManager.Instance.AddScore(100);

        switch (itemEnum)
        {
            case ItemEnum.Coin:
                GameManager.Instance.AddScore(100);
                pool.ReturnToPool(itemEnum, gameObject);
                break;
            case ItemEnum.HealPotion:
                HealPotion(2);
                break;
            case ItemEnum.SpeedPotion:
                StartCoroutine(SpeedPotion(3));
                break;
            case ItemEnum.GiantPotion:
                StartCoroutine(GiantPotion(3));
                break;
            case ItemEnum.Magnet:
                StartCoroutine(Magnet(5));
                break;
                //case ItemEnum.Bomb:
                //    Bomb();
                //    break;
        }

    }

    private IEnumerator BlinkEffect()//�����̴� �ڷ�ƾ. ����Ƽ�Ǽ��̶� ���� �ʿ�
    {
        SpriteRenderer sr = playerController.GetComponentInChildren<SpriteRenderer>(); // �÷��̾��� SpriteRenderer ������
        Color originalColor = sr.color;            // ���� �� ����

        while (true) // StopCoroutine �� ������ ��� �ݺ�
        {
            sr.color = new Color(1f, 1f, 1f, 0.3f); // ������
            yield return new WaitForSeconds(0.1f); // 0.3�� ���

            sr.color = originalColor;              // ���� ������
            yield return new WaitForSeconds(0.1f); // �� 0.3�� ���
        }
    }
    
    private IEnumerator SpeedPotion(int duration)//��?������
    {
        playerController.SetInvincible(true); ;//����
        playerController.SetSpeed(13) ;       //�̵��ӵ� ����, �������� ���� ��ƼŬó�� ����Ʈ ������ ��?����, �ӵ� �������� �����ʿ�

        yield return new WaitForSeconds(duration);//�ߵ��Ǹ� ������ ���� �� ���ӽð�(duration)�Ŀ� �ؿ��� ����

        playerController.SetSpeed(8.5f);//�ٽ� ����, ������ 1.5���Ŀ� Ǯ��
        Coroutine blink = StartCoroutine(BlinkEffect());//�����̴� ����Ʈ ����

        yield return new WaitForSeconds(1.5f);//1.5���� �ٽ� �������� 

        StopCoroutine(blink);          // �����̱� ����
        playerController.GetComponentInChildren<SpriteRenderer>().color = Color.white; // ���� ���󺹱�
        playerController.SetInvincible(false); ;//���� ����

        pool.ReturnToPool(itemEnum, gameObject);//��Ʈ����, SetActiveFalse ��� ������ Ǯ�� Ǯ�� ��ȯ.�ѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤ�

    }


    private IEnumerator GiantPotion(int duration)//Ŀ����~
    {
        Debug.Log("���̾�Ʈ �ڷ�ƾ ���۵�");
        BoxCollider2D collider = playerController.GetComponent<BoxCollider2D>();
        playerController.SetInvincible(true); ;//����
        //collider.size *= 1f;    // �ڽ� �ݶ��̴� ũ�� 2��
        //collider.offset *= 1f;  // ��ġ�� ���� ������� �Ѵٰ� ����Ƽ�� �׷����
        playerController.transform.localScale *= 2f;//�÷��̾��� ũ�� 2��
        
        yield return new WaitForSeconds(duration);//���ӽð� �Ŀ� ������ ����

        Debug.Log("���̾�Ʈ �ڷ�ƾ ����");
        //collider.size /= 1f;    // �ڽ� �ݶ��̴� ũ�� �������
        //collider.offset /= 1f;  // ��ġ
        playerController.transform.localScale /= 2f;//�÷��̾��� ũ�� �������
        Coroutine blink = StartCoroutine(BlinkEffect());

        Debug.Log("��ũ ����");

        yield return new WaitForSeconds(1.5f);

        Debug.Log("��ũ ��");

        StopCoroutine(blink);          // �����̱� ����
        playerController.GetComponentInChildren<SpriteRenderer>().color = Color.white; // ���� ���󺹱�
        playerController.SetInvincible(false); ;//���� ����

        pool.ReturnToPool(itemEnum, gameObject);
    }

    private IEnumerator Magnet(int duration)
    {
        float radius = 10f;           // �ڼ� ���� �ݰ�
        float pullSpeed = 15f;       // �������� �ӵ�
        float timer = 0f;            // Ÿ�̸� �ʱ�ȭ


        while (timer < duration)//duration ��ŭ 
        {
            //Physics2D.OverlapCircleAll(Ư����ġ, �������� ũ��) = Ư�� ��ġ�� �߽����� radius��ŭ�� ���� ���� �ȿ� �ִ� ��� Collider2D�� ã����.
            Collider2D[] items = Physics2D.OverlapCircleAll(playerController.transform.position, radius);//�÷��̾� �߽� radius���� �ȿ� �ݶ��̴����� �˻�

            foreach (Collider2D item in items)
            {
                if (item.CompareTag("Collectable"))//�ݷ��ͺ� �̶��
                {
                    //Vector dir = ( Ÿ����ġ - ������ġ).normalized.     normalized = ���͸� ���� 1¥�� ���ͷ� �ٲ�
                    Vector3 dir = (playerController.transform.position - item.transform.position).normalized;// �̵� ������ ����
                    item.transform.position += dir * pullSpeed * Time.deltaTime;//�������������� �ٲ㼭 �������
                }
            }

            yield return null;         // ���� �����ӱ��� ���
            timer += Time.deltaTime;    // Ÿ�̸� ����
        }
        pool.ReturnToPool(itemEnum, gameObject);
    }


    public void HealPotion(int heal)
    {
        //model.Heal(heal);
        //int updateHp = model.CurrentHealth;

        //playerController.Heal(heal);
        Debug.Log("�� ��");
        GameManager.Instance.Heal(heal);

        //UIManager.Instance.UpdateHealth(updateHp); //UI ������Ʈ

        pool.ReturnToPool(itemEnum, gameObject);
    }
}



