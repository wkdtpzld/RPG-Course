using UnityEngine;
using UnityEngine.AI;

public class SwordThunderStrike_Controller : MonoBehaviour
{
    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Enemy>() != null)
        {
            PlayerStatus playerStatus = PlayerManager.instance.player.GetComponent<PlayerStatus>();
            EnemyStatus enemyStatus = collision.GetComponent<EnemyStatus>();
            playerStatus.DoMagicalDamage(enemyStatus);
        }
    }
}
