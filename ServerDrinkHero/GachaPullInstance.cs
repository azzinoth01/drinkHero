public class GachaPullInstance {

    private UserDatabase _user;
    private GachaDatabase _gacha;

    private int _maxPullRange;
    private List<GachaPullRange> _gachaPullRanges;

    public GachaPullInstance(UserDatabase user, GachaDatabase gacha) {

        _user = user;
        _gacha = gacha;


        _gachaPullRanges = new List<GachaPullRange>();
        _maxPullRange = 0;
        CreatePullInstance();
    }


    private void CreatePullInstance() {
        List<string> foreigenkeys = new List<string>();

        foreigenkeys.Add("RefUser");
        foreigenkeys.Add("RefHero");

        int maxWeigthed = 0;
        List<GachaToGachaCategorieDatabase> gachaCategorieList = _gacha.GachaCetegorieList;

        for (int i = 0; i < gachaCategorieList.Count;) {

            List<GachaCategorieToGachaItemDatabase> itemList = gachaCategorieList[i].GachaCategorie.ItemList;



            GachaPullRange categoriePull = new GachaPullRange();


            int maxWeigthedItem = 0;
            for (int x = 0; x < itemList.Count;) {
                HeroToUserDatabase heros = null;
                if (itemList[x].GachaItemType == "HERO") {

                    List<int> keyValues = new List<int>();



                    keyValues.Add(_user.Id);
                    keyValues.Add(itemList[x].RefGachaItem.Value);

                    heros = DatabaseManager.GetDatabaseItem<HeroToUserDatabase>(foreigenkeys, keyValues);
                }

                if (heros == null) {

                    GachaPullRange itemPull = new GachaPullRange();

                    itemPull.Id = itemList[x].Id;

                    itemPull.From = maxWeigthedItem;
                    maxWeigthedItem = maxWeigthedItem + itemList[x].WeigthedValue;
                    itemPull.To = maxWeigthedItem;

                    categoriePull.GachaPullRanges.Add(itemPull);
                    categoriePull.GachaPullRangeMaxValue = maxWeigthedItem;
                }
                x = x + 1;
            }
            if (categoriePull.GachaPullRanges.Count != 0) {

                categoriePull.Id = gachaCategorieList[i].Id;

                categoriePull.From = maxWeigthed;
                maxWeigthed = maxWeigthed + gachaCategorieList[i].WeigthedValue;

                _maxPullRange = maxWeigthed;

                categoriePull.To = maxWeigthed;
                _gachaPullRanges.Add(categoriePull);
            }


            i = i + 1;
        }

    }

    public List<int> Pull(int amount) {
        List<int> pulledIds = new List<int>();

        for (int i = 0; i < amount;) {
            bool itemFound = false;
            int randomCategorie = RandomGenerator.Instance.NextRandom(0, _maxPullRange);
            for (int x = 0; x < _gachaPullRanges.Count;) {
                if (_gachaPullRanges[x].From >= randomCategorie && _gachaPullRanges[x].To < randomCategorie) {
                    int randomItem = RandomGenerator.Instance.NextRandom(0, _gachaPullRanges[x].GachaPullRangeMaxValue);

                    for (int y = 0; y < _gachaPullRanges[x].GachaPullRanges.Count;) {
                        if (_gachaPullRanges[x].GachaPullRanges[y].From >= randomItem && _gachaPullRanges[x].GachaPullRanges[y].To < randomItem) {
                            pulledIds.Add(_gachaPullRanges[x].GachaPullRanges[y].Id);

                            _gachaPullRanges[x].GachaPullRanges.RemoveAt(y);

                            if (_gachaPullRanges[x].GachaPullRanges.Count == 0) {
                                _gachaPullRanges.RemoveAt(x);
                            }
                            itemFound = true;
                            break;
                        }

                        y = y + 1;
                    }
                    if (itemFound == false) {
                        continue;
                    }

                }
                if (itemFound == true) {
                    break;
                }

                x = x + 1;
            }


            i = i + 1;
        }


        return pulledIds;
    }




}
