using UnityEngine;

public class ParticelEffectPlayer : MonoBehaviour, IAnimation {

    [SerializeField] private ParticleSystem _particle;
    [SerializeField] private float _duration;
    [SerializeField] private int _playTimes;
    [SerializeField] private HitAnimation _hitAnimation;
    [SerializeField] private bool _playHitAfter;
    private int _currentPlayTime;
    private float _time;

    // Update is called once per frame
    void Update() {
        _time = _time + Time.deltaTime;


        if (_time >= _duration) {
            _particle.Stop();
            gameObject.SetActive(false);
            PlayAgain();
            if (_hitAnimation != null) {
                _hitAnimation.Play();
            }
        }
    }

    [ContextMenu("Play ParticelEffect")]
    public void Play() {
        _currentPlayTime = 0;
        _time = 0;
        gameObject.SetActive(true);
        _particle.Play();
        if (_playHitAfter == false) {
            if (_hitAnimation != null) {
                _hitAnimation.Play();
            }

        }
    }

    private void PlayAgain() {
        _currentPlayTime = _currentPlayTime + 1;
        if (_currentPlayTime < _playTimes) {

            _time = 0;
            gameObject.SetActive(true);
            _particle.Play();
            if (_playHitAfter == false) {
                if (_hitAnimation != null) {
                    _hitAnimation.Play();
                }
            }
        }

    }
}
