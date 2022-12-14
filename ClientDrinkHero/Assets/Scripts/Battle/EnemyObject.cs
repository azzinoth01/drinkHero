using UnityEngine;

public class EnemyObject : MonoBehaviour {

    [SerializeField] private EnemyBattle _enemyData;
    public EnemyHandler _enemyHandler;

    [SerializeField]
    private SimpleAudioEvent _enemyDamagedSound, _enemyDamageBlockedSound, _enemyHealedSound, _enemyShielUpSound;

    public EnemyBattle Enemy {
        get {
            return _enemyData;
        }

    }



    private void Awake() {
        _enemyData = new EnemyBattle();
        _enemyHandler = new EnemyHandler();
        _enemyHandler.RequestData();
        _enemyHandler.LoadingFinished += EnemyLoaded;
        _enemyData.DiedEvent += _enemyHandler.RequestData;
    }

    private void Update() {
        _enemyHandler.Update();
    }
    private void EnemyLoaded() {
        _enemyData.ResetEnemy(_enemyHandler._enemy);


    }


    private void OnEnable() {
        EnemyBattle.enemyDamageReceived += EnemyDamageFeedback;
        EnemyBattle.enemyDamageBlocked += EnemyDamageBlockedFeedback;
        EnemyBattle.enemyHealed += EnemyHealedFeedback;
        EnemyBattle.enemyShieldUp += EnemyShieldUpFeedback;
    }

    private void OnDisable() {
        EnemyBattle.enemyDamageReceived += EnemyDamageFeedback;
        EnemyBattle.enemyDamageBlocked += EnemyDamageBlockedFeedback;
        EnemyBattle.enemyHealed += EnemyHealedFeedback;
        EnemyBattle.enemyShieldUp += EnemyShieldUpFeedback;
    }

    private void EnemyDamageFeedback() {
        GlobalAudioManager.Instance.Play(_enemyDamagedSound);
    }

    private void EnemyDamageBlockedFeedback() {
        GlobalAudioManager.Instance.Play(_enemyDamageBlockedSound);
    }

    private void EnemyHealedFeedback() {
        GlobalAudioManager.Instance.Play(_enemyHealedSound);
    }

    private void EnemyShieldUpFeedback() {
        GlobalAudioManager.Instance.Play(_enemyShielUpSound);
    }

}
