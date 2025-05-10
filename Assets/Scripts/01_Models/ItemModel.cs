using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    HealPotion,
    SpeedPotion,
    GiantPotion/*,
    Bomb,
    Magnet*/


}

public class ItemModel : MonoBehaviour
{
    private PlayerController playerController;//�÷��̾�� ��� �ִ°�. ��ü�ʿ�?

    public ItemEnum itemEnum;

    public void ApplyEffect()//�������� ������
    {
        
        GameManager.Instance.AddScore(500);

        switch (itemEnum)
        {
            case ItemEnum.HealPotion:
                HealPotion();
                break;
            case ItemEnum.SpeedPotion:
                StartCoroutine(SpeedPotion(5));
                break;
            case ItemEnum.GiantPotion:
                StartCoroutine(GiantPotion(5));
                break;
                //case ItemEnum.Bomb:
                //    Bomb();
                //    break;
                //case ItemEnum.Magnet:
                //    Magnet();
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
            yield return new WaitForSeconds(0.3f); // 0.3�� ���

            sr.color = originalColor;              // ���� ������
            yield return new WaitForSeconds(0.3f); // �� 0.3�� ���
        }
    }

    private IEnumerator SpeedPotion(int duration)//��?������
    {
        playerController.SetInvincible(true); ;//����
        playerController.SetSpeed(15) ;       //�̵��ӵ� ����, �������� ���� ��ƼŬó�� ����Ʈ ������ ��?����, �ӵ� �������� �����ʿ�

        yield return new WaitForSeconds(duration);//�ߵ��Ǹ� ������ ���� �� ���ӽð�(duration)�Ŀ� �ؿ��� ����

        playerController.SetSpeed(10);//�ٽ� ����, ������ 1.5���Ŀ� Ǯ��
        Coroutine blink = StartCoroutine(BlinkEffect());//�����̴� ����Ʈ ����

        yield return new WaitForSeconds(1.5f);//1.5���� �ٽ� �������� 

        StopCoroutine(blink);          // �����̱� ����
        playerController.GetComponentInChildren<SpriteRenderer>().color = Color.white; // ���� ���󺹱�
        playerController.SetInvincible(false); ;//���� ����

    }


    private IEnumerator GiantPotion(int duration)//Ŀ����~
    {
        BoxCollider2D collider = playerController.GetComponent<BoxCollider2D>();
        playerController.SetInvincible(true); ;//����
        collider.size *= 2f;    // �ڽ� �ݶ��̴� ũ�� 2��
        collider.offset *= 2f;  // ��ġ�� ���� ������� �Ѵٰ� ����Ƽ�� �׷����
        playerController.transform.localScale *= 2f;//�÷��̾��� ũ�� 2��

        yield return new WaitForSeconds(duration);//���ӽð� �Ŀ� ������ ����

        collider.size /= 2f;    // �ڽ� �ݶ��̴� ũ�� �������
        collider.offset /= 2f;  // ��ġ
        playerController.transform.localScale /= 2f;//�÷��̾��� ũ�� �������
        Coroutine blink = StartCoroutine(BlinkEffect());

        yield return new WaitForSeconds(1.5f);

        StopCoroutine(blink);          // �����̱� ����
        playerController.GetComponentInChildren<SpriteRenderer>().color = Color.white; // ���� ���󺹱�
        playerController.SetInvincible(false); ;//���� ����


    }

    //public void Bomb()
    //{

    //}

    //public void Magnet()
    //{

    //}



    public void HealPotion()
    {
        playerController.Heal(1);//�̷������� ���� �ǳ�?

        //+ UI�� ��Ʈ�� �ϳ� �߰�
    }
}

/*
PlayerController.cs�� �߰��Ұ�? �߰��ϸ� ���� ������...

public bool IsInvincible { get; set; } = false; // ----------�߰� �������� Ȯ�ο� bool


public void TakeDamage(int damage)
{
    if (IsInvincible)//-----------�� �ٰ� �Ʒ��� �߰�
        return; // ���� ���¸� �������� ������ ����

    model.TakeDamage(damage);

    if (model.CurrentHealth <= 0)
    {
        Die();
    }
}







*/




//public class ItemModel : MonoBehaviour//�Ʒ����� ����Ƽ�� ��õ���� �ڵ�
//{

//    public ItemEnum itemEnum;
//    public float duration; // ���ӽð� (�ʿ� ���� ���� 0)
//    public int healAmount; // ȸ���� (�� ����)
//    public int itemScore;//������ ȹ��� ���� �߰�?

//    public void ApplyEffect(PlayerController player)
//    {
//        switch (itemEnum)
//        {
//            case ItemEnum.GiantPotion:
//                StartCoroutine(GiantEffect(player));
//                break;
//            case ItemEnum.SpeedPotion:
//                StartCoroutine(SpeedEffect(player));
//                break;
//            case ItemEnum.HealPotion:
//                player.Heal(healAmount);
//                break;
//            case ItemEnum.Bomb:
//                StartCoroutine(BombEffect(player)); 
//                break;
//            case ItemEnum.Magnet:
//                StartCoroutine(MagnetEffect(player)); 
//                break;
//        }
//    }


//   private IEnumerator GiantEffect(PlayerController player)
//    {
//        player.IsInvincible = true;
//        player.transform.localScale *= 2f;
//        yield return new WaitForSeconds(duration);
//        player.transform.localScale /= 2f;
//        player.IsInvincible = false;
//    }

//    private IEnumerator SpeedEffect(PlayerController player)
//    {
//        player.IsInvincible = true;
//        player.speed += 3f;
//        yield return new WaitForSeconds(duration);
//        player.speed -= 3f;
//        player.IsInvincible = false;
//    }
//private IEnumerator BombEffect(PlayerController player)
//{
//    // �÷��̾� ���� ����
//    Vector2 origin = player.transform.position;
//    Vector2 direction = player.transform.right;

//    // ���� ���� �� ��� �浹ü ���� (���� 5f �Ÿ�, ������ 1.5f)
//    RaycastHit2D[] hits = Physics2D.CircleCastAll(origin, 1.5f, direction, 5f);

//    foreach (var hit in hits)
//    {
//        // ��ֹ� ������Ʈ���� Ȯ�� (�±� "Obstacle" �ʿ�)
//        if (hit.collider != null && hit.collider.CompareTag("Obstacle"))
//        {
//            Destroy(hit.collider.gameObject); // ��ֹ� �ı�
//            player.AddScore(itemScore);       // ���� �߰�
//        }
//    }

//    yield return null; // �ڷ�ƾ ���� (���� ����)
//}
//private IEnumerator MagnetEffect(PlayerController player)
//{
//    float radius = 5f;           // �ڼ� ���� �ݰ�
//    float pullSpeed = 10f;       // �������� �ӵ�
//    float timer = 0f;            // Ÿ�̸� �ʱ�ȭ

//    while (timer < duration)
//    {
//        // �ֺ��� �ִ� ��� �ݷ��ͺ� ���� (Collectable �±� �ʿ�)
//        Collider2D[] items = Physics2D.OverlapCircleAll(player.transform.position, radius);

//        foreach (Collider2D item in items)
//        {
//            if (item.CompareTag("Collectable"))
//            {
//                // �÷��̾ ���� �̵� (�ڼ�ó�� ����)
//                Vector3 dir = (player.transform.position - item.transform.position).normalized;
//                item.transform.position += dir * pullSpeed * Time.deltaTime;
//            }
//        }

//        timer += Time.deltaTime;    // Ÿ�̸� ����
//        yield return null;         // ���� �����ӱ��� ���
//    }
//}
//}
