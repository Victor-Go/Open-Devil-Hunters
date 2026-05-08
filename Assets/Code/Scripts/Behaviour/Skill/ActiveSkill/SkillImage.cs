using UnityEngine;
using UnityEngine.UI;

namespace Code.Scripts.Behaviour.Level
{
    public class SkillImage : MonoBehaviour
    {
        private Image skillImage;
        private Image mask;

        private void Awake()
        {
            skillImage = transform.Find("Image").GetComponent<Image>();
            mask = transform.Find("Mask").GetComponent<Image>();
        }

        private void Start()
        {
            SetProgress(1);
        }

        public void SetImage(Texture2D texture)
        {
            Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
            skillImage.sprite = sprite;
        }

        public void SetProgress(float progress)
        {
            progress = Mathf.Min(progress, 1);
            mask.fillAmount = 1 - progress;
        }
    }
}
