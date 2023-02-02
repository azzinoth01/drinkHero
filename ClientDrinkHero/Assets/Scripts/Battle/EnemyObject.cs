using System.Collections;
using UnityEngine;

public class EnemyObject : MonoBehaviour {

    [SerializeField] private EnemyBattle _enemyData;
    [SerializeField] private DisolveSprite _disolveAnimation;

    [SerializeField] private float _spawnNextDelay;

    public static int GoldGotThisSession;


    [SerializeField]
    private SimpleAudioEvent _enemyDamagedSound, _enemyDamageBlockedSound, _enemyHealedSound, _enemyShielUpSound;


    private LevelContainer _levelData;

    public EnemyBattle Enemy {
        get {
            return _enemyData;
        }

    }



    private void Awake() {
        GoldGotThisSession = 0;

        _enemyData = new EnemyBattle();



        _levelData = new LevelContainer(_enemyData);

        _enemyData.DiedEvent += StartAnimations;
    }

    private void Update() {

        _levelData.Update();
    }



    private IEnumerator SpawnDelay() {
        yield return new WaitForSeconds(_spawnNextDelay);
        UIDataContainer.Instance.EnemySlot.UnloadSprite();
        _disolveAnimation.ResetEffect();
        _levelData.NextEnemy();
    }




    public void StartAnimations() {

        _disolveAnimation.enabled = true;

        StartCoroutine(SpawnDelay());

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
