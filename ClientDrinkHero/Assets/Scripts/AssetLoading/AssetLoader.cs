using System.Collections.Generic;
using UnityEngine;

public class AssetLoader {

    private static AssetLoader _instance;

    private Dictionary<string, SpriteHandle> _spriteHandle;


    public static AssetLoader Instance {
        get {
            if (_instance == null) {
                _instance = new AssetLoader();
            }
            return _instance;
        }

    }

    public Sprite BorrowSprite(string path) {

        if (_spriteHandle.TryGetValue(path, out SpriteHandle handle)) {
            return handle.BorrowSprite(path);
        }
        else {
            handle = new SpriteHandle();
            return handle.BorrowSprite(path);
        }
    }
    public void ReturnSprite(string path) {
        if (_spriteHandle.TryGetValue(path, out SpriteHandle handle)) {
            handle.ReturnSprite();
        }
    }

    public void ReleaseAllSprites() {
        foreach (SpriteHandle handle in _spriteHandle.Values) {
            handle.ReleaseHandle();
        }
    }


    public AssetLoader() {
        _spriteHandle = new Dictionary<string, SpriteHandle>();
    }

}
