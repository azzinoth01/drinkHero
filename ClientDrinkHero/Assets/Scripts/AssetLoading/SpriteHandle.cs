using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class SpriteHandle {
    private Sprite _sprite;
    private AsyncOperationHandle<Sprite> _handle;
    private int _uses;
    private bool _loaded;

    public bool Loaded {
        get {
            return _loaded;
        }
    }

    public SpriteHandle() {
        _uses = 0;
        _loaded = false;
    }

    public Sprite BorrowSprite(string path) {
        if (_loaded == false) {
            if (LoadSprite(path) == false) {
                return null;
            }
        }
        _uses = _uses + 1;
        return _sprite;
    }

    public void ReturnSprite() {
        _uses = _uses - 1;
        if (_uses == 0) {
            Addressables.Release(_handle);
            _loaded = false;
        }
    }


    private bool LoadSprite(string path) {

        if (path == "" || path == null) {
            return false;
        }

        AsyncOperationHandle<Sprite> _handle = Addressables.LoadAssetAsync<Sprite>(path);

        _handle.WaitForCompletion();

        if (_handle.Status == AsyncOperationStatus.Succeeded) {
            _sprite = _handle.Result;
            _loaded = true;
            return true;
        }

        return false;
    }
    public void ReleaseHandle() {
        Addressables.Release(_handle);
        _loaded = false;
        _uses = 0;
    }
}
