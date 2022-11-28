using UnityEngine;

public class EnemyObject : MonoBehaviour {

    public Enemy enemy;

    [SerializeField]
    private SimpleAudioEvent _enemyDamagedSound, _enemyDamageBlockedSound, _enemyHealedSound, _enemyShielUpSound;

    private void OnEnable()
    {
        Enemy.enemyDamageReceived += EnemyDamageFeedback;
        Enemy.enemyDamageBlocked += EnemyDamageBlockedFeedback;
        Enemy.enemyHealed += EnemyHealedFeedback;
        Enemy.enemyShieldUp += EnemyShieldUpFeedback;
    }

    private void OnDisable()
    {
        Enemy.enemyDamageReceived += EnemyDamageFeedback;
        Enemy.enemyDamageBlocked += EnemyDamageBlockedFeedback;
        Enemy.enemyHealed += EnemyHealedFeedback;
        Enemy.enemyShieldUp += EnemyShieldUpFeedback;
    }

    private void EnemyDamageFeedback()
    {
        GlobalAudioManager.Instance.Play(_enemyDamagedSound);
    }
    
    private void EnemyDamageBlockedFeedback()
    {
        GlobalAudioManager.Instance.Play(_enemyDamageBlockedSound);
    }
    
    private void EnemyHealedFeedback()
    {
        GlobalAudioManager.Instance.Play(_enemyHealedSound);
    }
    
    private void EnemyShieldUpFeedback()
    {
        GlobalAudioManager.Instance.Play(_enemyShielUpSound);
    }

}
