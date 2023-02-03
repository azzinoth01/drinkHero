using System.Collections;
using TMPro;
using UnityEngine;

public class FlyingText : MonoBehaviour, IAnimation {
    [SerializeField] private AnimationCurve _yAnimationCurve;
    private float _yDuration;
    [SerializeField] private float _yRange;
    [SerializeField] private AnimationCurve _xAnimationCurve;
    private float _xDuration;
    [SerializeField] private float _xRange;
    private Vector3 _startposition;
    private int _finishedPlaying;

    [SerializeField] private TextMeshProUGUI _text;

    private FlyingTextContainer _container;
    [SerializeField] private FlyingTextEnum _type;



    public FlyingTextContainer Container {
        get {
            return _container;
        }

        set {
            _container = value;
        }
    }

    public FlyingTextEnum Type {
        get {
            return _type;
        }

    }

    private void Awake() {

        _startposition = transform.localPosition;

        _yDuration = _yAnimationCurve[_yAnimationCurve.length - 1].time;
        _xDuration = _xAnimationCurve[_xAnimationCurve.length - 1].time;
        _finishedPlaying = 0;
    }

    private IEnumerator YMove() {
        float time = 0;

        while (time < _yDuration) {

            transform.localPosition = new Vector3(transform.localPosition.x, _startposition.y + (_yAnimationCurve.Evaluate(time) * _yRange), transform.localPosition.z);

            time = time + Time.deltaTime;
            yield return null;
        }

        transform.localPosition = new Vector3(transform.localPosition.x, _startposition.y + (_yAnimationCurve.Evaluate(_yDuration) * _yRange), transform.localPosition.z);

        FinishedPlay();
    }
    private IEnumerator XMove() {
        float time = 0;

        while (time < _xDuration) {
            //Debug.Log("xMove Time");
            transform.localPosition = new Vector3(_startposition.x + (_xAnimationCurve.Evaluate(time) * _xRange), transform.localPosition.y, transform.localPosition.z);

            time = time + Time.deltaTime;
            yield return null;
        }
        transform.localPosition = new Vector3(_startposition.x + (_xAnimationCurve.Evaluate(_xDuration) * _xRange), transform.localPosition.y, transform.localPosition.z);

        FinishedPlay();
    }

    private void FinishedPlay() {
        _finishedPlaying = _finishedPlaying + 1;

        if (_finishedPlaying == 2) {
            gameObject.SetActive(false);


        }
    }

    [ContextMenu("Play")]
    public void Play() {
        _finishedPlaying = 0;
        transform.localPosition = _startposition;
        gameObject.SetActive(true);
        StopAllCoroutines();
        StartCoroutine(YMove());
        StartCoroutine(XMove());

    }

    public void SetText(string text) {
        _text.SetText(text);

    }

    private void OnDisable() {
        _container.FlyingTextList[_type].Add(this);
    }
}
