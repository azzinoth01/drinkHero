using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DisolveCard : MonoBehaviour {

    [SerializeField] private Image _image;
    private Material _material;
    [SerializeField] private float _disolveTime;
    [SerializeField] private Color _disolveColor;
    [SerializeField] private bool _loop;
    [SerializeField] private float _loopRestartTime;
    [SerializeField] private float _textDisapearTime;
    [SerializeField] private bool _setGameObjectInactive;
    private bool _isTextDisapear;
    private float _time;


    private void OnEnable() {
        ResetEffect();

    }

    // Start is called before the first frame update
    void Start() {

        _material = _image.material;

        _material.SetColor("_DisolveColor", _disolveColor);
        _material.SetFloat("_DisolveState", 1);
        _time = 0;
    }

    private void Update() {

        _time = _time + Time.deltaTime;



        float state = 1 - (_time / _disolveTime);


        _material.SetFloat("_DisolveState", state);

        if (_time >= _disolveTime) {
            enabled = false;
            if (_setGameObjectInactive) {
                gameObject.SetActive(false);
            }


            if (_loop == true) {
                StartCoroutine(LoopStart());
            }
        }
        if (_time >= _textDisapearTime && _isTextDisapear == false) {

            foreach (Transform t in transform) {
                t.gameObject.SetActive(false);
            }
            _isTextDisapear = true;
        }
    }


    private IEnumerator LoopStart() {

        yield return new WaitForSeconds(_loopRestartTime);



        enabled = true;
    }

    public void ResetEffect() {
        _material = _image.material;

        _material.SetColor("_DisolveColor", _disolveColor);
        _material.SetFloat("_DisolveState", 1);
        _time = 0;
        _isTextDisapear = false;

        foreach (Transform t in transform) {
            t.gameObject.SetActive(true);
        }
        if (_setGameObjectInactive) {
            gameObject.SetActive(true);
        }
    }
}
