using System.Collections.Generic;
using UnityEngine;

public class IdleAnimation : MonoBehaviour {
    [SerializeField] private List<GameObject> _sprites;
    [SerializeField] private float _maxRange;
    [SerializeField] private float _minRange;
    [SerializeField] private Vector3 _speed;
    [SerializeField] private bool _isGoingUp;
    [SerializeField] private float _targetRange;
    [SerializeField] private List<Vector3> _startPosition;
    [SerializeField] private List<bool> _positionReached;

    // Start is called before the first frame update
    void Start() {
        _startPosition = new List<Vector3>();
        _positionReached = new List<bool>();
        foreach (GameObject g in _sprites) {
            _startPosition.Add(g.transform.localPosition);
            _positionReached.Add(false);
        }
        _isGoingUp = true;


        _targetRange = Random.Range(0, _maxRange);

    }

    // Update is called once per frame
    void Update() {

        float deltaTime = Time.deltaTime;
        bool checkPositionReached = true;
        for (int i = 0; i < _sprites.Count;) {
            checkPositionReached = checkPositionReached & _positionReached[i];
            if (_positionReached[i] == true) {
                i = i + 1;
                continue;
            }


            GameObject g = _sprites[i];
            Vector3 targetPosition = _startPosition[i] + (_speed.normalized * _targetRange);
            g.transform.localPosition = g.transform.localPosition + (_speed * deltaTime);

            //Debug.Log(_speed * deltaTime);

            if (_isGoingUp == true) {
                if (g.transform.localPosition.x >= targetPosition.x && g.transform.localPosition.y >= targetPosition.y && g.transform.localPosition.z >= targetPosition.z) {
                    g.transform.localPosition = targetPosition;
                    _positionReached[i] = true;
                }
            }
            else {
                if (g.transform.localPosition.x <= targetPosition.x && g.transform.localPosition.y <= targetPosition.y && g.transform.localPosition.z <= targetPosition.z) {
                    g.transform.localPosition = targetPosition;
                    _positionReached[i] = true;
                }
            }


            i = i + 1;
        }


        if (checkPositionReached == true) {
            if (_isGoingUp == true) {
                _isGoingUp = false;

                _targetRange = 0;

            }
            else {
                _isGoingUp = true;
                _targetRange = Random.Range(_minRange, _maxRange);

            }
            for (int i = 0; i < _sprites.Count;) {
                _positionReached[i] = false;
                i = i + 1;
            }
            _speed = _speed * -1;
        }

    }
}
