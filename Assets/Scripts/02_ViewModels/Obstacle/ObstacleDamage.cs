using UnityEngine;

/// <summary>
/// 장애물이 플레이어와 충돌했을 때 데미지를 주는 ViewModel 스크립트
/// </summary>
[RequireComponent(typeof(Collider2D))]
public class ObstacleDamage : MonoBehaviour
{
    private int damage = 1; // 기본값 설정

    /// <summary>
    /// 외부에서 모델을 주입받아 데미지 설정 (MVVM 방식)
    /// </summary>
    public void SetModel(ObstacleModel model)
    {
        damage = model.Damage;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        var playerCtrl = other.GetComponent<PlayerController>();
        if (playerCtrl != null)
        {
            GameManager.Instance.TakeDamage(damage);
            //Debug.Log(other.gameObject.name);
            playerCtrl.TakeDamage(damage); // 플레이어에게 데미지 전달
        }
    }
}
