using System.Collections.Generic;
using UnityEngine;

public static class GachaPullHelper
{

    public static List<ScriptableObject> StartPull(int pullAmount,GachaData gachaData) {
        Dictionary<string,HeroObject> ownedHeroes = new Dictionary<string,HeroObject>();
        foreach(HeroObject hero in GameDataInstance.Instance.OwnedHeroes) {
            ownedHeroes[hero.Id] = hero;
        }
        List<(int, List<GachaItem>)> gachaList = new List<(int, List<GachaItem>)>();

        List<WeightedGachaCategory> weightedGachaCategory = new List<WeightedGachaCategory>(gachaData.Categories);
        foreach(WeightedGachaCategory category in weightedGachaCategory) {
            List<GachaItem> itemList = new List<GachaItem>(category.Category.GachaitemList);
            for(int i = itemList.Count - 1; i >= 0; i--) {
                ScriptableObject item = itemList[i].Item;
                if(item is HeroData) {
                    HeroData heroData = (HeroData) item;
                    if(ownedHeroes.ContainsKey(heroData.Id) == true) {
                        itemList.RemoveAt(i);
                    }
                }
            }
            if(itemList.Count > 0) {
                gachaList.Add((category.Weight, itemList));
            }
        }
        if(gachaList.Count == 0) {
            return null;
        }
        List<ScriptableObject> pullResults = new List<ScriptableObject>();
        for(int i = 0; i < pullAmount; i++) {
            int max = 0;
            foreach((int, List<GachaItem>) category in gachaList) {
                max = max + category.Item1;
            }
            int random = Random.Range(0,max) + 1;
            for(int x = 0; x < gachaList.Count; x++) {
                int categoryWeight = gachaList[x].Item1;
                List<GachaItem> gachaItemList = gachaList[x].Item2;
                if(categoryWeight >= random) {
                    int maxGachaItem = 0;
                    foreach(GachaItem gachaItem in gachaItemList) {
                        maxGachaItem = maxGachaItem + gachaItem.Weight;
                    }
                    int randomGachaItem = Random.Range(0,maxGachaItem) + 1;

                    for(int y = 0; y < gachaItemList.Count; y++) {
                        GachaItem gachaItem = gachaItemList[y];
                        if(gachaItem.Weight >= randomGachaItem) {
                            ScriptableObject item = gachaItem.Item;
                            pullResults.Add(item);
                            if(item is HeroData) {
                                HeroData heroData = (HeroData) item;
                                GameDataInstance.Instance.UserSave.AddHero(heroData);
                                HeroObject heroObject = new HeroObject(heroData,new List<int>());
                                GameDataInstance.Instance.OwnedHeroes.Add(heroObject);
                                gachaItemList.RemoveAt(y);
                                maxGachaItem = maxGachaItem - gachaItem.Weight;
                            }
                            else if(item is UpgradeItemData) {
                                UpgradeItemData upgradeItemData = (UpgradeItemData) item;
                                GameDataInstance.Instance.UserSave.AddItem(upgradeItemData,1);
                            }
                            break;
                        }
                        randomGachaItem = randomGachaItem - gachaItem.Weight;
                    }
                    if(gachaItemList.Count == 0) {
                        gachaList.RemoveAt(x);
                    }
                    break;
                }
                else {
                    random = random - categoryWeight;
                }
            }
        }

        GameDataInstance.Instance.UserSave.Save();
        return pullResults;
    }
}