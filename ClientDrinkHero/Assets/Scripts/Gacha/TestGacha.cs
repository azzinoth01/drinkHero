using UnityEngine;

public class TestGacha : MonoBehaviour {

    [ContextMenu("Test Gacha")]
    public void RollTenGacha() {


        string request = ClientFunctions.GachaMultiPull();
        HandleRequests.Instance.HandleRequest(request, typeof(ResponsMessageObject));

    }
}
