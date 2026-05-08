using Code.Scripts.Src;
using Code.Scripts.Src.Types;
using System.Collections;
using System.Collections.Generic;
using Code.Scripts.Src.Types;
using UnityEngine;
using UnityEngine.UI;

namespace Code.Scripts.Behaviour.UI.Gem
{
    public class Streamer : MonoBehaviour, IPoolableGameObject
    {
        public Vector2 InitialSpeed;
        public List<Color> colors;

        private Rigidbody rb;
        private ObjectPool objectPool;
        private RectTransform rectTransform;
        private Image image;
        private float range;
        private float initialY;

        public string ObjectName { get; set; }

        public void ObjectReset(Vector2 initialPosition)
        {
            gameObject.SetActive(true);
            rb.angularVelocity = Vector3.zero;
            rectTransform.localScale = Vector3.one;
        }

        public Streamer SetPosition(Vector2 position)
        {
            rectTransform.anchoredPosition = position;
            initialY = position.y;
            return this;
        }
        public Streamer SetRange(float range)
        {
            this.range = range;
            return this;
        }

        private IEnumerator AddForce()
        {
            float force = 5;
            yield return new WaitForFixedUpdate();
            rb.AddForceAtPosition(
               new Vector3(Random.Range(0, force), Random.Range(0, force), Random.Range(0, force)),
               new Vector3(Random.Range(0, force), Random.Range(0, force), Random.Range(0, force)));
        }

        public Streamer Randomize()
        {
            image.color = colors.Count > 0 ? colors[Random.Range(0, colors.Count)] : Color.white;
            rb.angularVelocity = Vector3.zero;
            rb.velocity = InitialSpeed;

            StartCoroutine(AddForce());

            return this;
        }

        private void Awake()
        {
            objectPool = ObjectPool.Instance;
            rb = GetComponent<Rigidbody>();
            image = GetComponent<Image>();
            rectTransform = GetComponent<RectTransform>();
        }

        private void Update()
        {
            float rectY = rectTransform.anchoredPosition.y;
            if (rectY < initialY - range)
            {
                objectPool.Recycle(ObjectName, gameObject);
            }
            float percentage = 1 - (initialY - rectY) / range;
            rectTransform.localScale = Vector2.one * percentage;
        }
    }
}
