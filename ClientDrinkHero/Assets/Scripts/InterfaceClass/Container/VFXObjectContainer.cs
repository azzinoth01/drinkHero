using System.Collections.Generic;

public class VFXObjectContainer {


    private static VFXObjectContainer _instance;
    private Dictionary<string, IAnimation> _animationObjects;

    public static VFXObjectContainer Instance {
        get {
            if (_instance == null) {
                _instance = new VFXObjectContainer();
            }
            return _instance;
        }

    }


    private VFXObjectContainer() {
        _animationObjects = new Dictionary<string, IAnimation>();
    }

    public void AddAnimation(string key, IAnimation animation) {

        _animationObjects[key] = animation;
    }

    public void PlayAnimation(string key) {


        if (_animationObjects.TryGetValue(key, out IAnimation animation)) {
            animation.Play();
        }

    }

}
