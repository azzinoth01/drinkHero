using UnityEngine;
public class PullResolver
{
    private int _heroPullChance;
    private int _itemPullChance;

    private Pull[] _pullTypes;
    private float _totalPullTypeWeight;
    
    private Pull[] _availableHeroPulls;
    private Pull[] _availableItemPulls;

    private float _totalHeroPullWeight; 
    private float _totalItemPullWeight;
    
    private bool _isInitialized;
    
    private int _pityCounter;
    
    private void Initialize()
    {
        foreach (var type in _pullTypes)
        {
            _totalPullTypeWeight += type.weight;
        }
    }

    private void ResolvePull(Pull[] pulls)
    {
        float roll = Random.value * _totalPullTypeWeight;
        float sumWeight = 0;
        
        foreach (var pull in pulls)
        {
            sumWeight += pull.weight;

            if (sumWeight >= roll)
            {
                Debug.Log($"Pulled: {pull.name}");
            }
        }
    }
}
