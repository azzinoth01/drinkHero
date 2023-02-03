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

        if (type == FlyingTextEnum.dmg && text != "0") {
            AudioController.Instance.PlayAudio(AudioType.SFXDamage);
        }
        else if (type == FlyingTextEnum.shield && text != "0") {
            AudioController.Instance.PlayAudio(AudioType.SFXShield);
        }
        else if (type == FlyingTextEnum.heal && text != "0") {
            AudioController.Instance.PlayAudio(AudioType.SFXHeal);
        }
        else if (type == FlyingTextEnum.effect && text.Contains("POISON")) {
            AudioController.Instance.PlayAudio(AudioType.SFXPoison);
        }
        else if (type == FlyingTextEnum.effect && (text.Contains("ATT UP") || text.Contains("DEF UP"))) {
            AudioController.Instance.PlayAudio(AudioType.SFXBuff);
        }
        else if (type == FlyingTextEnum.effect && (text.Contains("ATT DOWN") || text.Contains("DEF DOWN"))) {
            AudioController.Instance.PlayAudio(AudioType.SFXDebuff);
        }
        else if (type == FlyingTextEnum.effect && text.Contains("REMOVE SHIELD")) {
            AudioController.Instance.PlayAudio(AudioType.SFXShieldBreak);
        }
        else if (type == FlyingTextEnum.effect && text.Contains("REMOVE DEBUFF")) {
            AudioController.Instance.PlayAudio(AudioType.SFXCleanse);
        }
        else if (type == FlyingTextEnum.effect) {
            AudioController.Instance.PlayAudio(AudioType.SFXDebuff);
        }



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
