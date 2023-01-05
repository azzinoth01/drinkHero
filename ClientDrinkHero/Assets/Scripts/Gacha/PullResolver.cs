using UnityEngine;
public class PullResolver : MonoBehaviour
{
    [SerializeField] private int heroPullChance;
    [SerializeField] private int itemPullChance;

    [SerializeField] private Pull[] pullTypes;
    
    private float _totalPullTypeWeight;
    
    [SerializeField] private Pull[] availableHeroPulls;
    [SerializeField] private Pull[] availableItemPulls;

    private float _totalHeroPullWeight; 
    private float _totalItemPullWeight;
    
    private bool _isInitialized;
    
    private int _pityCounter;
    
    private void Initialize()
    {
        // check whether PullResolver is initialized
        // if so return
        // else    
            // Get all available Items and Characters and their respective DropRates from Server
            // fill arrays
            // sum up chances into totalweight
            foreach (var type in pullTypes)
            {
                _totalPullTypeWeight += type.weight;
            }
            // set isInitialized to true
    }

    private void CheckForSufficientCurrency()
    {
        // check which currency is needed and amount
        // if sufficient, deduct and resolve pull type
        // not really part of this classes job...
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
