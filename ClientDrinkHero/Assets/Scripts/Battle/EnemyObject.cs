using System.Collections;
using UnityEngine;

public class EnemyObject : MonoBehaviour {

    [SerializeField] private EnemyBattle _enemyData;
    [SerializeField] private Animator _vfx;


    [SerializeField] private float _despawnDelay;
    [SerializeField] private float _spawnNextDelay;
    [SerializeField] private float _deathAnimationDelay;
    [SerializeField] private float _spawnAnimationDelay;



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



        _levelData = new LevelContainer(_enemyData);

        _enemyData.DiedEvent += StartAnimations;
    }

    private void Update() {

        _levelData.Update();
    }


    private IEnumerator DeathAnimation() {
        yield return new WaitForSeconds(_deathAnimationDelay);


        _vfx.SetBool("isOver", false);
        _vfx.SetTrigger("die");
    }

    private IEnumerator SpawnDelay() {
        yield return new WaitForSeconds(_spawnNextDelay);
        _levelData.NextEnemy();
    }
    private IEnumerator DespawnDelay() {
        yield return new WaitForSeconds(_despawnDelay);

        UIDataContainer.Instance.EnemySlot.UnloadSprite();
    }
    private IEnumerator SpawnAnimationDelay() {
        yield return new WaitForSeconds(_spawnAnimationDelay);

        _vfx.SetBool("isOver", false);
        _vfx.SetTrigger("spawn");
    }


    public void StartAnimations() {

        StartCoroutine(DespawnDelay());
        StartCoroutine(DeathAnimation());
        StartCoroutine(SpawnDelay());
        StartCoroutine(SpawnAnimationDelay());
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
