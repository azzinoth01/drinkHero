using UnityEngine;

public class HitAnimation : MonoBehaviour, IAnimation {

    [SerializeField] private IdleAnimation _idleAnimation;
    [SerializeField] private AnimationCurve _curve;
    [SerializeField] private float _duration;
    [SerializeField] private Vector3 _range;

    private Vector3 _startPosition;
    private float _time;
    private bool _inExecution;

    private void Awake() {

        _time = 0;
        _startPosition = transform.localPosition;
        _inExecution = false;
    }



    // Update is called once per frame
    void Update() {

        if (_inExecution == false) {
            enabled = false;
            _idleAnimation.enabled = true;
            _time = 0;
            return;
        }




        _time = _time + Time.deltaTime;



        transform.localPosition = _startPosition + (_curve.Evaluate(_time / _duration) * _range);
        if (_time >= _duration) {
            transform.localPosition = _startPosition;

            _inExecution = false;


        }
    }

    [ContextMenu("Play")]
    public void Play() {

        if (_inExecution == false) {
            ResetValues();
        }



    }
    private void ResetValues() {
        _time = 0;

        _startPosition = transform.localPosition;
        _idleAnimation.enabled = false;
        enabled = true;
        _inExecution = true;
    }

    public void StopAnimation() {
        _time = 0;

        _startPosition = transform.localPosition;
        _idleAnimation.enabled = true;
        _inExecution = false;
    }

}
