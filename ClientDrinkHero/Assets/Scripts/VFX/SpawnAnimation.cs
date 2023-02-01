using UnityEngine;

public class SpawnAnimation : MonoBehaviour, IAnimation {

    [SerializeField] private int _moveInDirection;
    [SerializeField] private float _moveInTime;
    [SerializeField] private string _animationKey;
    [SerializeField] private Animator _animator;
    private Vector3 _startPosition;
    private Vector3 _moveInSpeed;
    private float _time;


    private void Awake() {
        _startPosition = transform.position;

        VFXObjectContainer.Instance.AddAnimation(_animationKey, this);
    }


    private void Update() {


        transform.position = transform.position - (_moveInSpeed * Time.deltaTime);
        _time = _time + Time.deltaTime;

        if (_time >= _moveInTime) {
            transform.position = _startPosition;
            enabled = false;
        }

    }

    [ContextMenu("PlaySpawn")]
    public void Play() {


        float moveoutside = Screen.width * 1.5f * (_moveInDirection * -1);


        transform.position = new Vector3(transform.position.x + moveoutside, transform.position.y, transform.position.z);

        _moveInSpeed = new Vector3(moveoutside / _moveInTime, 0, 0);
        _time = 0;

        _animator.SetTrigger("Play");



        enabled = true;
    }

}
