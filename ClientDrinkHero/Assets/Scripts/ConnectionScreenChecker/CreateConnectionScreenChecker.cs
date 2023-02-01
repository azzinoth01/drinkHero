using UnityEngine;

[DefaultExecutionOrder(-100)]
public class CreateConnectionScreenChecker : MonoBehaviour {
    private void Awake() {
        Debug.Log("Create Networkscreen handle");
        new ConnectionScreenChecker();

    }
}
