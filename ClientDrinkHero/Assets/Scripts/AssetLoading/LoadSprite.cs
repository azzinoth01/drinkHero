using UnityEngine;
using UnityEngine.UI;


public class LoadSprite : MonoBehaviour, IAssetLoader {

    [SerializeField] private Image _spriteRender;
    [SerializeField] private string _spritePath;
    [SerializeField] private string _spritePathSufix;
    [SerializeField] private int _slot;
    [SerializeField] private bool _isEnemy;
    public int Slot {
        get {
            return _slot;
        }

    }

    private void Awake() {
        if (_spritePath != "" && _spritePath != null) {
            Sprite sprite = AssetLoader.Instance.BorrowSprite(_spritePath + _spritePathSufix);
            if (sprite != null) {

                _spriteRender.sprite = sprite;
                _spriteRender.enabled = true;
            }
        }

        if (_isEnemy == true) {
            UIDataContainer.Instance.EnemySlot = this;
        }
        else {
            UIDataContainer.Instance.CharacterSlots[_slot] = this;
        }

    }

    private void OnDestroy() {
        AssetLoader.Instance.ReturnSprite(_spritePath + _spritePathSufix);
    }

    public void LoadNewSprite(string path) {
        Sprite sprite = AssetLoader.Instance.BorrowSprite(path + _spritePathSufix);
        if (sprite != null) {
            //Debug.Log("Loaded Sprite Paht: " + path);
            _spriteRender.sprite = sprite;
            AssetLoader.Instance.ReturnSprite(_spritePath + _spritePathSufix);
            _spritePath = path;

            _spriteRender.enabled = true;
        }
    }

    public void UnloadSprite() {
        _spriteRender.enabled = false;
        _spriteRender.sprite = null;
        AssetLoader.Instance.ReturnSprite(_spritePath + _spritePathSufix);
    }
}
