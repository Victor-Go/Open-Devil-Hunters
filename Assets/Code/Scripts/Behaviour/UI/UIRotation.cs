using UnityEngine;

namespace Code.Scripts.Behaviour.UI
{
    public class UIRotation : MonoBehaviour
    {
        public float Angle;

        private RectTransform rectTransform;

        private void Awake()
        {
            rectTransform = GetComponent<RectTransform>();
        }

        private void Update()
        {
            rectTransform.rotation *= Quaternion.Euler(0f, 0f, Angle * Mathf.Deg2Rad);
        }
    }
}