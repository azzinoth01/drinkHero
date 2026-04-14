using System.Collections.Generic;
using UnityEngine;

public class InitGameData : MonoBehaviour
{
    [SerializeField] private GameDataDatabase _database;
    [SerializeField] private UserStartingData _startingData;

    private void Awake() {
        CreateGameDatabase();
        UserSave save = LoadUserSavefile();
        CreateOwnedHeroList(save);
        CreateEnemyList();
    }

    private UserSave LoadUserSavefile() {
        UserSave save = UserSave.LoadSave(_startingData);
        save.Save();
        GameDataInstance.Instance.UserSave = save;
        return save;
    }

    private void CreateGameDatabase() {
        GameDataInstance.Instance.GameDatabase = _database.GetDatabase();
        GameDataInstance.Instance.GameDataDatabase = _database;
    }

    private static void CreateOwnedHeroList(UserSave save) {
        List<HeroObject> list = new List<HeroObject>();
        foreach(SavedHero savedHero in save.CollectedHeroes) {
            HeroData data = (HeroData) GameDataInstance.Instance.GameDatabase[savedHero.Id];
            HeroObject heroObject = new HeroObject(data,savedHero.CardUpgradeLevels);
            list.Add(heroObject);
        }
        GameDataInstance.Instance.OwnedHeroes = list;
    }

    private void CreateEnemyList() {
        List<EnemyData> enemyDatas = _database.EnemyData;
        foreach(EnemyData enemy in enemyDatas) {
            if(enemy.IsBoss) {
                GameDataInstance.Instance.BossEnemies.Add(enemy);
            }
            else {
                GameDataInstance.Instance.NormalEnemies.Add(enemy);
            }
        }
    }
}
