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

        //Console.Write("Gacha Categorie List count " + gachaCategorieList.Count + "\r\n");

        for (int i = 0; i < gachaCategorieList.Count;) {

            List<GachaCategorieToGachaItemDatabase> itemList = gachaCategorieList[i].GachaCategorie.ItemList;



            GachaPullRange categoriePull = new GachaPullRange();


            int maxWeigthedItem = 0;

            //Console.Write("Gacha itemlist count " + itemList.Count + "\r\n");

            for (int x = 0; x < itemList.Count;) {
                HeroToUserDatabase heros = new HeroToUserDatabase();
                if (itemList[x].GachaItemType == "Hero") {

                    List<int> keyValues = new List<int>();



                    keyValues.Add(_user.Id);
                    keyValues.Add(itemList[x].RefGachaItem.Value);

                    heros = DatabaseManager.GetDatabaseItem<HeroToUserDatabase>(foreigenkeys, keyValues);
                    //Console.Write("HeroToUser ID: " + heros.Id + "\r\n");

                }

                if (heros.Id == 0) {

                    GachaPullRange itemPull = new GachaPullRange();
                    if (itemList[x].GachaItemType == "Hero") {
                        itemPull.RemoveAfterPull = true;
                    }
                    itemPull.Id = itemList[x].Id;

                    itemPull.From = maxWeigthedItem;
                    maxWeigthedItem = maxWeigthedItem + itemList[x].WeigthedValue;
                    itemPull.To = maxWeigthedItem;

                    //Console.Write("item pull ID: " + itemPull.Id + " From: " + itemPull.From + " To: " + itemPull.To + " maxWeigthed: " + maxWeigthedItem + "\r\n");

                    categoriePull.GachaPullRanges.Add(itemPull);
                    categoriePull.GachaPullRangeMaxValue = maxWeigthedItem;
                }
                x = x + 1;
            }
            if (categoriePull.GachaPullRanges.Count != 0) {
                //Console.Write("Categories filled\r\n");

                categoriePull.Id = gachaCategorieList[i].Id;

                categoriePull.From = maxWeigthed;
                maxWeigthed = maxWeigthed + gachaCategorieList[i].WeigthedValue;

                _maxPullRange = maxWeigthed;

                categoriePull.To = maxWeigthed;

                //Console.Write("Categorie pull ID: " + categoriePull.Id + " From: " + categoriePull.From + " To: " + categoriePull.To + " maxWeigthed: " + maxWeigthed + "\r\n");


                _gachaPullRanges.Add(categoriePull);
            }


            i = i + 1;
        }

        //Console.Write("Categories Count: " + _gachaPullRanges.Count + "\r\n");


    }

    public List<int> Pull(int amount) {
        List<int> pulledIds = new List<int>();

        //Console.Write("Gacha Categorie Count: " + _gachaPullRanges.Count + "\r\n");

        for (int i = 0; i < amount;) {
            bool itemFound = false;
            int randomCategorie = RandomGenerator.Instance.NextRandom(0, _maxPullRange);
            //Console.Write("Random Categroie roll: " + randomCategorie + "\r\n");
            for (int x = 0; x < _gachaPullRanges.Count;) {
                //Console.Write("From: " + _gachaPullRanges[x].From + " TO: " + _gachaPullRanges[x].To + "\r\n");
                if (_gachaPullRanges[x].From <= randomCategorie && _gachaPullRanges[x].To > randomCategorie) {
                    int randomItem = RandomGenerator.Instance.NextRandom(0, _gachaPullRanges[x].GachaPullRangeMaxValue);
                    //Console.Write("Random Item roll: " + randomItem + "\r\n");
                    for (int y = 0; y < _gachaPullRanges[x].GachaPullRanges.Count;) {
                        //Console.Write("From: " + _gachaPullRanges[x].GachaPullRanges[y].From + " TO: " + _gachaPullRanges[x].GachaPullRanges[y].To + "\r\n");
                        if (_gachaPullRanges[x].GachaPullRanges[y].From <= randomItem && _gachaPullRanges[x].GachaPullRanges[y].To > randomItem) {
                            //Console.Write("Add To pullIDs: " + _gachaPullRanges[x].GachaPullRanges[y].Id + "\r\n");
                            pulledIds.Add(_gachaPullRanges[x].GachaPullRanges[y].Id);

                            if (_gachaPullRanges[x].GachaPullRanges[y].RemoveAfterPull == true) {
                                _gachaPullRanges[x].GachaPullRanges.RemoveAt(y);
                            }


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

            if (itemFound == true) {
                i = i + 1;
            }

        }


        return pulledIds;
    }




}
