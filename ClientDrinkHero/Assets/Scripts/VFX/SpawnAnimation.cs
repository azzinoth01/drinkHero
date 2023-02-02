using UnityEngine;

public class SpawnAnimation : MonoBehaviour, IAnimation {

    [SerializeField] private int _moveInDirection;
    [SerializeField] private float _moveInTime;
    [SerializeField] private string _animationKey;
    [SerializeField] private Animator _animator;
    [SerializeField] private Vector3 _startPosition;
    [SerializeField] private Vector3 _moveInSpeed;
    [SerializeField] private float _time;


    private void Awake() {
        _startPosition = transform.localPosition;

        VFXObjectContainer.Instance.AddAnimation(_animationKey, this);

        //Debug.Log(transform.position);
        //Debug.Log(transform.localPosition);
    }


    private void Update() {


        transform.localPosition = transform.localPosition - (_moveInSpeed * Time.deltaTime);
        _time = _time + Time.deltaTime;

        if (_time >= _moveInTime) {
            transform.localPosition = _startPosition;
            enabled = false;
        }

    }

    [ContextMenu("PlaySpawn")]
    public void Play() {

        Debug.Log(transform.position);
        float moveoutside = Screen.width * 2 * (_moveInDirection * -1);



        transform.localPosition = new Vector3(transform.localPosition.x + moveoutside, transform.localPosition.y, transform.localPosition.z);

        _moveInSpeed = new Vector3(moveoutside / _moveInTime, 0, 0);
        _time = 0;

        _animator.SetTrigger("Play");



        enabled = true;
    }

}
