using UnityEngine;
using UnityEngine.UI;


public class LoadSprite : MonoBehaviour, IAssetLoader {

    [SerializeField] private Image _spriteRender;
    [SerializeField] private string _spritePath;
    [SerializeField] private string _spritePathSufix;
    [SerializeField] private int _slot;
    [SerializeField] private bool _isEnemy;
    [SerializeField] private bool _noSlot;
    [SerializeField] private bool _loadIntoMaterial;
    public int Slot {
        get {
            return _slot;
        }

    }

    private void Awake() {
        if (_spritePath != "" && _spritePath != null) {
            Sprite sprite = AssetLoader.Instance.BorrowSprite(_spritePath + _spritePathSufix);
            if (sprite != null) {

                if (_loadIntoMaterial == true) {
                    _spriteRender.sprite = null;
                    _spriteRender.material.SetTexture("_MainTex", sprite.texture);

                    // reset image componete to reforce load sprite
                    _spriteRender.enabled = false;
                    _spriteRender.enabled = true;

                }
                else {
                    _spriteRender.sprite = sprite;
                }
                _spriteRender.enabled = true;
            }
        }
        if (_noSlot == true) {
            return;
        }
        else if (_isEnemy == true) {
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

        if (path == _spritePath) {
            return;
        }

        Sprite sprite = AssetLoader.Instance.BorrowSprite(path + _spritePathSufix);
        if (sprite != null) {
            //Debug.Log("Loaded Sprite Paht: " + path);

            if (_loadIntoMaterial == true) {
                _spriteRender.sprite = null;

                _spriteRender.material.SetTexture("_MainTex", sprite.texture);
                // reset image componete to reforce load sprite
                _spriteRender.enabled = false;
                _spriteRender.enabled = true;

            }
            else {
                _spriteRender.sprite = sprite;
            }

            AssetLoader.Instance.ReturnSprite(_spritePath + _spritePathSufix);
            _spritePath = path;

            _spriteRender.enabled = true;
        }
    }

    public void UnloadSprite() {
        _spriteRender.enabled = false;
        _spriteRender.sprite = null;
        if (_loadIntoMaterial == true) {
            // reset image componete to reforce load sprite
            _spriteRender.enabled = false;
            _spriteRender.enabled = true;
            _spriteRender.material.SetTexture("_MainTex", null);

        }
        AssetLoader.Instance.ReturnSprite(_spritePath + _spritePathSufix);
    }
}
