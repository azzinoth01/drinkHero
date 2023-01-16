using UnityEngine;

public class TestGacha : MonoBehaviour {

    [ContextMenu("Test Gacha")]
    public void RollTenGacha() {


        ClientFunctions.GachaMultiPull();

    }
}
