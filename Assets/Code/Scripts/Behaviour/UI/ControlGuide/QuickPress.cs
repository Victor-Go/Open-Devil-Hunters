using UnityEngine;

namespace Code.Scripts.Behaviour.UI
{
    public class QuickPress : MonoBehaviour
    {
        public float Interval = 0.2f;
        private float timeElapsed;

        private Vector2 defaultScale;

        private void Awake()
        {
            defaultScale = transform.localScale;
        }

        private void Update()
        {
            var dt = Time.deltaTime;

            timeElapsed += dt;
            var parity = (int)(timeElapsed / Interval) % 2 == 0;
            switch (parity)
            {
                case true: transform.localScale = defaultScale; break;
                case false: transform.localScale = defaultScale * 0.8f; break;
            }
        }
    }
}