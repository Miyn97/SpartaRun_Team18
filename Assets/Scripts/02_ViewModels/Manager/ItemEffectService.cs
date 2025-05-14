using System.Collections;
using UnityEngine;

/// <summary>
/// 아이템 모델에 따라 플레이어에게 효과를 적용하는 서비스 클래스
/// MonoBehaviour 필요 (코루틴용)
/// </summary>
public class ItemEffectService : MonoBehaviour
{
    [SerializeField] private PlayerController player;

    /// <summary>
    /// 아이템 모델에 맞춰 해당 효과 실행
    /// </summary>
    public void ApplyEffect(ItemModel model)
    {
        switch (model.ItemType)
        {
            case ItemEnum.Coin:
                break;

            case ItemEnum.HealPotion:
                //GameManager.Instance.Heal((int)model.Value);
                player.Heal(2);
                break;

            case ItemEnum.SpeedPotion:
                StartCoroutine(SpeedEffect(model));
                break;

            case ItemEnum.GiantPotion:
                StartCoroutine(GiantEffect(model));
                break;

            case ItemEnum.Magnet:
                StartCoroutine(MagnetEffect(model));
                break;
        }
    }

    private IEnumerator SpeedEffect(ItemModel model)
    {
        GameManager.Instance.SetInvincible(true);
        player.SetInvincible(true);
        player.SetSpeed(model.Value); // ex. 13f

        yield return YieldCache.WaitForSeconds(model.Duration);

        player.SetSpeed(8.5f);
        Coroutine blink = StartCoroutine(Blink());

        yield return YieldCache.WaitForSeconds(1.5f);

        StopCoroutine(blink);
        player.GetComponentInChildren<SpriteRenderer>().color = Color.white;
        player.SetInvincible(false);
        GameManager.Instance.SetInvincible(false);
    }

    private IEnumerator GiantEffect(ItemModel model)
    {
        GameManager.Instance.SetInvincible(true);
        player.SetInvincible(true);
        player.transform.localScale *= 2f;

        yield return YieldCache.WaitForSeconds(model.Duration);

        player.transform.localScale /= 2f;
        Coroutine blink = StartCoroutine(Blink());

        yield return YieldCache.WaitForSeconds(1.5f);

        StopCoroutine(blink);
        player.GetComponentInChildren<SpriteRenderer>().color = Color.white;
        player.SetInvincible(false);
        GameManager.Instance.SetInvincible(false);
    }

    private IEnumerator MagnetEffect(ItemModel model)
    {
        float radius = 10f;
        float pullSpeed = 15f;
        float timer = 0f;

        while (timer < model.Duration)
        {
            Collider2D[] items = Physics2D.OverlapCircleAll(player.transform.position, radius);

            foreach (var item in items)
            {
                if (item.CompareTag("Collectable"))
                {
                    Vector3 dir = (player.transform.position - item.transform.position).normalized;
                    item.transform.position += dir * pullSpeed * Time.deltaTime;
                }
            }

            yield return null;
            timer += Time.deltaTime;
        }
    }

    private IEnumerator Blink()
    {
        SpriteRenderer sr = player.GetComponentInChildren<SpriteRenderer>();
        Color originalColor = sr.color;

        while (true)
        {
            sr.color = new Color(1f, 1f, 1f, 0.3f);
            yield return YieldCache.WaitForSeconds(0.1f);

            sr.color = originalColor;
            yield return YieldCache.WaitForSeconds(0.1f);
        }
    }
}
