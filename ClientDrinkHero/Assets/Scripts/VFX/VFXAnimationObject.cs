using System.Collections.Generic;
using UnityEngine;

public class VFXAnimationObject : MonoBehaviour, IAnimation {
    [SerializeField] private string _animationKey;
    [SerializeField] private List<Animator> _animator;
    [SerializeField] private List<GameObject> _animations;

    [SerializeField] private List<ParticleSystem> _particleEffects;


    private void Awake() {
        VFXObjectContainer.Instance.AddAnimation(_animationKey, this);
    }



    [ContextMenu("PlayAnimation")]
    public void Play() {

        foreach (Animator anim in _animator) {
            anim.SetTrigger("Play");
        }

        foreach (GameObject anim in _animations) {
            anim.GetComponent<IAnimation>().Play();
        }
        foreach (ParticleSystem p in _particleEffects) {
            p.Play();
        }


    }
}
