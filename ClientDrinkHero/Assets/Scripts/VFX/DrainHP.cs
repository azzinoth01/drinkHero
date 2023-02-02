using UnityEngine;

public class DrainHP : MonoBehaviour, IAnimation {
    [SerializeField] private GameObject _target;
    [SerializeField] private GameObject _user;
    [SerializeField] private GameObject _image;
    [SerializeField] private float _moveDelayTime;
    [SerializeField] private float _animationTime;
    [SerializeField] private float _disableTime;
    [SerializeField] private string _playAnimationOnFinishKey;
    private float _time;
    private Vector3 _moveSpeed;


    private void Update() {

        _time = _time + Time.deltaTime;
        if (_time <= _moveDelayTime) {
            return;
        }

        transform.position = transform.position + (_moveSpeed * Time.deltaTime);

        if (_time >= _animationTime) {

            transform.position = _user.transform.position;
            if (_playAnimationOnFinishKey != "" && _playAnimationOnFinishKey != null) {
                VFXObjectContainer.Instance.PlayAnimation(_playAnimationOnFinishKey);
            }

        }
        if (_time >= _disableTime) {
            enabled = false;
            _image.SetActive(false);
        }
    }

    [ContextMenu("Play")]
    public void Play() {
        transform.position = _target.transform.position;
        Vector3 dif = _user.transform.position - _target.transform.position;
        _moveSpeed = new Vector3(dif.x / (_animationTime - _moveDelayTime), dif.y / (_animationTime - _moveDelayTime), dif.z / (_animationTime - _moveDelayTime));
        _time = 0;
        enabled = true;
        _image.SetActive(true);
    }
}
