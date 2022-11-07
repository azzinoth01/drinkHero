using UnityEngine;

public class EnemyObject : MonoBehaviour
{
    public Enemy enemy;
    [SerializeField] private SimpleAudioEvent _enemyDamageSound;
    [SerializeField] private SimpleAudioEvent _enemyDamageShieldedSound;

    private void Awake()
    {
        Enemy.enemyDamageReceived += EnemyDamageFeedback;
        Enemy.enemyDamageShielded += EnemyDamageShieldedFeedback;
    }

    private void OnDisable()
    {
        Enemy.enemyDamageReceived -= EnemyDamageFeedback;
        Enemy.enemyDamageShielded -= EnemyDamageShieldedFeedback;
    }

    private void EnemyDamageFeedback()
    {
        GlobalAudioManager.Instance.Play(_enemyDamageSound);
    }
    
    private void EnemyDamageShieldedFeedback()
    {
        GlobalAudioManager.Instance.Play(_enemyDamageShieldedSound);
    }
}