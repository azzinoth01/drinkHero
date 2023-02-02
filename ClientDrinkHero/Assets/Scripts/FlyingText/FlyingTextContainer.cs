using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingTextContainer : MonoBehaviour, IFlyingTextHandler {
    [SerializeField] private Dictionary<FlyingTextEnum, List<FlyingText>> _flyingTextList;


    [SerializeField] private List<FlyingText> _flyingTextPrefabList;

    private Queue<(FlyingTextEnum, string)> _spawnQueue;

    [SerializeField] private float _spawnDelay;
    private bool _inExecution;


    [SerializeField] private bool _isEnemy;
    public Dictionary<FlyingTextEnum, List<FlyingText>> FlyingTextList {
        get {
            return _flyingTextList;
        }
    }

    // Start is called before the first frame update
    void Start() {
        _flyingTextList = new Dictionary<FlyingTextEnum, List<FlyingText>>();
        _spawnQueue = new Queue<(FlyingTextEnum, string)>();
        if (_isEnemy) {
            UIDataContainer.Instance.EnemyText = this;
        }
        else {
            UIDataContainer.Instance.PlayerText = this;
        }
    }

    private void Update() {
        if (_inExecution == true) {
            return;
        }
        else if (_spawnQueue.Count == 0) {
            return;
        }
        else {
            StartCoroutine(SpawnText());
            _inExecution = true;
        }


    }

    private IEnumerator SpawnText() {


        yield return new WaitForSeconds(_spawnDelay);

        (FlyingTextEnum, string) item = _spawnQueue.Dequeue();

        FlyingTextEnum type = item.Item1;
        string text = item.Item2;
        FlyingText flyingTextObject = null;
        if (_flyingTextList.TryGetValue(type, out List<FlyingText> list)) {
            if (list.Count == 0) {
                flyingTextObject = GetAddObject(type);
                flyingTextObject.SetText(text);
                flyingTextObject.Play();

            }
            else {
                flyingTextObject = list[list.Count - 1];
                list.RemoveAt(list.Count - 1);
                flyingTextObject.SetText(text);
                flyingTextObject.Play();


            }
        }
        else {
            list = new List<FlyingText>();
            _flyingTextList[type] = list;
            flyingTextObject = GetAddObject(type);
            flyingTextObject.SetText(text);
            flyingTextObject.Play();


        }
        _inExecution = false;
    }


    public void SpawnFlyingText(FlyingTextEnum type, string text) {
        _spawnQueue.Enqueue((type, text));

        //FlyingText flyingTextObject = null;
        //if (_flyingTextList.TryGetValue(type, out List<FlyingText> list)) {
        //    if (list.Count == 0) {
        //        flyingTextObject = GetAddObject(type);

        //    }
        //    else {
        //        flyingTextObject = list[list.Count - 1];
        //        list.RemoveAt(list.Count - 1);

        //    }
        //}
        //else {
        //    list = new List<FlyingText>();
        //    _flyingTextList[type] = list;
        //    flyingTextObject = GetAddObject(type);

        //}
        //flyingTextObject.SetText(text);
        //flyingTextObject.Play();
    }


    private FlyingText GetAddObject(FlyingTextEnum type) {
        foreach (FlyingText prefab in _flyingTextPrefabList) {
            if (prefab.Type == type) {
                FlyingText flyingTextObject = Instantiate(prefab, transform);
                flyingTextObject.Container = this;
                return flyingTextObject;

            }
        }
        return null;
    }

}
