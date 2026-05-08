using System.Collections;
using UnityEngine;

namespace Code.Scripts.Behaviour.UI
{
    public class IntervalAnimation : MonoBehaviour
    {
        public float AnimationInterval;
        public float IntervalFluctuation;

        private Animator animator;

        private void Awake()
        {
            animator = GetComponent<Animator>();
        }

        private void Start()
        {
            StartCoroutine(PlayAnimation());
        }

        IEnumerator PlayAnimation()
        {
            yield return new WaitForSeconds(AnimationInterval * Random.Range(1 - IntervalFluctuation, 1 + IntervalFluctuation));
            animator.SetTrigger("Play");
            StartCoroutine(PlayAnimation());
        }
    }
}
