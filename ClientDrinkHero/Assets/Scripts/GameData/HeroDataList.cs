using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HeroData",menuName = "Scriptable Objects/HeroData")]
public class HeroDataList : ScriptableObject
{
    [SerializeField] private List<HeroData> _heroData;

    public List<HeroData> HeroData {
        get {
            if(_heroData == null) {
                _heroData = new List<HeroData>();
            }
            return _heroData;
        }
    }
}
