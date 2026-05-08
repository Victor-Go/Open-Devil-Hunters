using UnityEngine;

namespace Code.Scripts.Behaviour.UI
{
    public class QuickDirectionPress : MonoBehaviour
    {
        public float Interval = 0.2f;

        private GameObject left;
        private GameObject right;

        private float timeElapsed;

        private void Awake()
        {
            left = transform.Find("Left").gameObject;
            right = transform.Find("Right").gameObject;
        }

        private void Update()
        {
            var dt = Time.deltaTime;

            timeElapsed += dt;
            var parity = (int)(timeElapsed / 0.2f) % 2 == 0;
            switch (parity)
            {
                case true: left.SetActive(false); right.SetActive(true); break;
                case false: left.SetActive(true); right.SetActive(false); break;
            }
        }
    }
}