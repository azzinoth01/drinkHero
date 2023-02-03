using UnityEngine;
using UnityEngine.UI;

public class GachaItemAppear : MonoBehaviour, IAnimation {

    [SerializeField] private Image _image;
    [SerializeField] private float _duration;
    [SerializeField] private Color _disolveColor;
    private float _time;
    private Material _material;


    private void Awake() {
        _material = _image.material;
    }

    // Update is called once per frame
    void Update() {

        _time = _time + Time.deltaTime;
        float state = (_time / _duration);


        _material.SetFloat("_DisolveState", state);

        if (_time >= _duration) {

            enabled = false;
            _material.SetFloat("_DisolveState", 1);
        }


    }

    public void Play() {

        ResetState();
        enabled = true;

    }
    public void ResetState() {
        _time = 0;

        _material.SetColor("_DisolveColor", _disolveColor);
        _material.SetFloat("_DisolveState", 0);
    }

}
