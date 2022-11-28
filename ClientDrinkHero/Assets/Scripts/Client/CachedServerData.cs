using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CachedServerData {
    public Dictionary<long, HeroDatabase> _heroData;
    public Dictionary<long, CardDatabase> _cardData;
    public Dictionary<long, EnemyDatabase> _enemyData;
    public Dictionary<long, EnemySkillDatabase> _enemySkillData;
    public Dictionary<long, CardToHero> _cardToHeroData;
    public Dictionary<long, EnemyToEnemySkill> _enemyToEnemySkillData;


    public CachedServerData() {
        _heroData = new Dictionary<long, HeroDatabase>();
        _cardData = new Dictionary<long, CardDatabase>();
        _enemyData = new Dictionary<long, EnemyDatabase>();
        _enemySkillData = new Dictionary<long, EnemySkillDatabase>();
        _cardToHeroData = new Dictionary<long, CardToHero>();
        _enemyToEnemySkillData = new Dictionary<long, EnemyToEnemySkill>();
    }

    public void SetHeroData(List<HeroDatabase> list) {
        foreach (HeroDatabase item in list) {
            _heroData.TryAdd(item.Id, item);
        }
    }
    public void SetCardData(List<CardDatabase> list) {
        foreach (CardDatabase item in list) {
            _cardData.TryAdd(item.Id, item);
        }
    }
    public void SetEnemyData(List<EnemyDatabase> list) {
        foreach (EnemyDatabase item in list) {
            _enemyData.TryAdd(item.Id, item);
        }
    }
    public void SetEmemySkillData(List<EnemySkillDatabase> list) {
        foreach (EnemySkillDatabase item in list) {
            _enemySkillData.TryAdd(item.Id, item);
        }
    }
    public void SetCardToHeroData(List<CardToHero> list) {
        foreach (CardToHero item in list) {
            _cardToHeroData.TryAdd(item.Id, item);
        }
    }
    public void SetEnemyToEnemySkillData(List<EnemyToEnemySkill> list) {
        foreach (EnemyToEnemySkill item in list) {
            _enemyToEnemySkillData.TryAdd(item.Id, item);
        }
    }

}
