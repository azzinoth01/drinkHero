using System.Collections.Generic;
using UnityEngine;

public class HitAnimation : MonoBehaviour, IAnimation {

    [SerializeField] private IdleAnimation _idleAnimation;
    [SerializeField] private AnimationCurve _curve;
    [SerializeField] private float _duration;
    [SerializeField] private Vector3 _range;

    [SerializeField] private Queue<bool> _playQueue;
    private Vector3 _startPosition;
    private float _time;
    private bool _inExecution;

    private void Awake() {

        _time = 0;
        _startPosition = transform.localPosition;
        _playQueue = new Queue<bool>();
        _inExecution = false;
    }



    // Update is called once per frame
    void Update() {

        if (_playQueue.Count == 0 && _inExecution == false) {
            enabled = false;
            _idleAnimation.enabled = true;
            return;
        }
        if (_inExecution == false) {
            _playQueue.Dequeue();
            ResetValues();

        }



        _time = _time + Time.deltaTime;


        //float curveTime = _curve[_curve.length - 1].time;

        //curveTime = curveTime * _duration;

        transform.localPosition = _startPosition + (_curve.Evaluate(_time / _duration) * _range);
        if (_time >= _duration) {
            transform.localPosition = _startPosition;

            _inExecution = false;


        }
    }

    [ContextMenu("Play")]
    public void Play() {

        _playQueue.Enqueue(true);

        if (enabled == false) {
            enabled = true;
        }


    }
    private void ResetValues() {
        _time = 0;

        _startPosition = transform.localPosition;
        _idleAnimation.enabled = false;
        enabled = true;
        _inExecution = true;
    }

}
