using System.Collections;
using UnityEngine;

public class EnemyObject : MonoBehaviour {

    [SerializeField] private EnemyBattle _enemyData;
    [SerializeField] private Animator _vfx;
    [SerializeField] private float _enemySpawnDelay;
    //public EnemyHandler _enemyHandler;

    [SerializeField]
    private SimpleAudioEvent _enemyDamagedSound, _enemyDamageBlockedSound, _enemyHealedSound, _enemyShielUpSound;


    private LevelContainer _levelData;

    public EnemyBattle Enemy {
        get {
            return _enemyData;
        }

    }



    private void Awake() {
        _enemyData = new EnemyBattle();
        //_enemyHandler = new EnemyHandler();
        //_enemyHandler.RequestData();
        //_enemyHandler.LoadingFinished += EnemyLoaded;
        //_enemyData.DiedEvent += _enemyHandler.RequestData;


        _levelData = new LevelContainer(_enemyData);
        //_enemyData.DiedEvent += _levelData.NextEnemy;
        _enemyData.DiedEvent += StartDieAnimation;
    }

    private void Update() {
        //_enemyHandler.Update();

        _levelData.Update();
    }
    //private void EnemyLoaded() {
    //    _enemyData.ResetEnemy(_enemyHandler._enemy);


    //}

    private IEnumerator DeathAnimation() {

        UIDataContainer.Instance.EnemySlot.UnloadSprite();
        _vfx.SetBool("isOver", false);
        _vfx.SetTrigger("die");

        while (_vfx.GetBool("isOver") == false) {
            //Debug.Log("Delay enemy spawn");
            yield return null;
        }
        yield return new WaitForSeconds(_enemySpawnDelay);
        _levelData.NextEnemy();
    }


    public void StartDieAnimation() {


        StartCoroutine(DeathAnimation());
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
