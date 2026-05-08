using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Code.Scripts.Behaviour.UI
{
    public class ProgressBarController : MonoBehaviour
    {
        private float width;
        private RectTransform fillComponentRectTransform;
        private Image fillImage;
        private Text textComponent;
        private float currentPercentage;

        private void Awake()
        {
            GameObject fill = transform.Find("Fill").gameObject;
            fillComponentRectTransform = fill.GetComponent<RectTransform>();
            fillImage = fill.GetComponent<Image>();

            width = GetComponent<RectTransform>().rect.width;
            textComponent = GetComponentInChildren<Text>();

            StartCoroutine(SetWidth());
        }

        private IEnumerator SetWidth()
        {
            yield return new WaitForEndOfFrame();
            width = GetComponent<RectTransform>().rect.width;
            SetPencentage(currentPercentage);
        }

        public ProgressBarController SetFillColor(Color color)
        {
            fillImage.color = color;
            return this;
        }

        public Color GetFillColor()
        {
            return fillImage.color;
        }

        public ProgressBarController SetTextColor(Color color)
        {
            textComponent.color = color;
            return this;
        }

        public ProgressBarController SetPencentage(float percentage)
        {
            currentPercentage = Mathf.Clamp01(percentage);
            fillComponentRectTransform.sizeDelta = new Vector2(currentPercentage * width, fillComponentRectTransform.rect.height);
            return this;
        }

        public ProgressBarController SetText(string text)
        {
            textComponent.text = text;
            return this;
        }
    }
}
