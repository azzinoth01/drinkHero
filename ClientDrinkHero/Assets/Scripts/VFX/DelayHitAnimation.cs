using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelayHitAnimation : MonoBehaviour, IAnimation {
    [SerializeField] private float _delay;
    [SerializeField] private List<IAnimation> _delayedAnimation;


    public void Play() {
        StartCoroutine(DelayAnimation());
    }


    private IEnumerator DelayAnimation() {


        yield return new WaitForSeconds(_delay);

        foreach (IAnimation anim in _delayedAnimation) {
            anim.Play();
        }
    }
}
