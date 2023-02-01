using System.Collections.Generic;

public class CachedServerData {

    private static CachedServerData _instance;

    public Dictionary<string, HeroDatabase> _heroData;
    public Dictionary<string, CardDatabase> _cardData;
    public Dictionary<string, EnemyDatabase> _enemyData;
    public Dictionary<string, EnemySkillDatabase> _enemySkillData;
    public Dictionary<string, CardToHero> _cardToHeroData;
    public Dictionary<string, EnemyToEnemySkill> _enemyToEnemySkillData;


    public static CachedServerData Instance {
        get {
            if (_instance == null) {
                _instance = new CachedServerData();
            }
            return _instance;
        }

    }

    private CachedServerData() {
        _heroData = new Dictionary<string, HeroDatabase>();
        _cardData = new Dictionary<string, CardDatabase>();
        _enemyData = new Dictionary<string, EnemyDatabase>();
        _enemySkillData = new Dictionary<string, EnemySkillDatabase>();
        _cardToHeroData = new Dictionary<string, CardToHero>();
        _enemyToEnemySkillData = new Dictionary<string, EnemyToEnemySkill>();


    }





    public void SetHeroData(List<HeroDatabase> list) {
        foreach (HeroDatabase item in list) {
            _heroData.TryAdd(item.Id.ToString(), item);
        }
    }
    public void SetCardData(List<CardDatabase> list) {
        foreach (CardDatabase item in list) {
            _cardData.TryAdd(item.Id.ToString(), item);
        }
    }
    public void SetEnemyData(List<EnemyDatabase> list) {
        foreach (EnemyDatabase item in list) {
            _enemyData.TryAdd(item.Id.ToString(), item);
        }
    }
    public void SetEmemySkillData(List<EnemySkillDatabase> list) {
        foreach (EnemySkillDatabase item in list) {
            _enemySkillData.TryAdd(item.Id.ToString(), item);
        }
    }
    public void SetCardToHeroData(List<CardToHero> list) {
        foreach (CardToHero item in list) {
            _cardToHeroData.TryAdd(item.Id.ToString(), item);
        }
    }
    public void SetEnemyToEnemySkillData(List<EnemyToEnemySkill> list) {
        foreach (EnemyToEnemySkill item in list) {
            _enemyToEnemySkillData.TryAdd(item.Id.ToString(), item);
        }
    }

}
