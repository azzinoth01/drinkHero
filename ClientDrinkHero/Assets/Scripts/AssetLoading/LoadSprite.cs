using UnityEngine;
using UnityEngine.UI;


public class LoadSprite : MonoBehaviour {

    [SerializeField] public Image spriteRender;
    [SerializeField] public string spritePath;
    [SerializeField] public string spritePathSufix;

    // Start is called before the first frame update
    void Start() {

        Sprite sprite = AssetLoader.Instance.BorrowSprite(spritePath + spritePathSufix);
        if (sprite != null) {

            spriteRender.sprite = sprite;
        }
    }

    private void OnDestroy() {
        AssetLoader.Instance.ReturnSprite(spritePath + spritePathSufix);
    }

    public void LoadNewSprite(string path) {
        Sprite sprite = AssetLoader.Instance.BorrowSprite(path + spritePathSufix);
        if (sprite != null) {
            spriteRender.sprite = sprite;
            AssetLoader.Instance.ReturnSprite(spritePath + spritePathSufix);
            spritePath = path;
        }


    }
}
