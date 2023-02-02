using UnityEngine;

public class ParticelEffectPlayer : MonoBehaviour, IAnimation {

    [SerializeField] private ParticleSystem _particle;
    [SerializeField] private float _duration;
    private float _time;

    // Update is called once per frame
    void Update() {
        _time = _time + Time.deltaTime;


        if (_time >= _duration) {
            _particle.Stop();
            gameObject.SetActive(false);
        }
    }

    [ContextMenu("Play ParticelEffect")]
    public void Play() {

        _time = 0;
        gameObject.SetActive(true);
        _particle.Play();
    }
}
