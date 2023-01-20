public interface IAssetLoader {

    public int Slot {

        get;
    }
    public void LoadNewSprite(string path);
    public void UnloadSprite();
}
