using Code.Scripts.Src.Types;
using Code.Scripts.Src.Types;
using UnityEngine;

namespace Code.Scripts.Behaviour.Environment
{
    public class Blood : PoolableAndPauseableGameObject
    {
        public float LifeTime = 5;
        public float FadeOutDuration = 2;

        private readonly string[] bloodImageNames = new string[]
        {
            "EffectImages/Blood/blood_0",
            "EffectImages/Blood/blood_1",
            "EffectImages/Blood/blood_2",
            "EffectImages/Blood/blood_3",
            "EffectImages/Blood/blood_4",
            "EffectImages/Blood/blood_5",
            "EffectImages/Blood/blood_6",
        };

        private SpriteRenderer spriteRenderer;

        private float lifeTime;
        private float fadeTimeElapsed;
        private bool fadingOut;

        protected override void Awake()
        {
            base.Awake();
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        protected override void Start()
        {
            base.Start();
            RandomizeImage();
        }

        public override void ObjectReset(Vector2 initialPosition)
        {
            base.ObjectReset(initialPosition);
            RandomizeImage();

            lifeTime = 0;
            fadeTimeElapsed = 0;
            spriteRenderer.color = Color.white;
            fadingOut = false;
        }

        private void RandomizeImage()
        {
            var texture = Resources.Load<Texture2D>(bloodImageNames[Random.Range(0, bloodImageNames.Length)]);
            spriteRenderer.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.one / 2, 200);
            transform.rotation = Quaternion.AngleAxis(Random.Range(0, 360), Vector3.forward);
        }

        private void Update()
        {
            if (paused) return;

            var dt = Time.deltaTime;

            lifeTime += dt;

            if (lifeTime > LifeTime)
            {
                fadingOut = true;
            }

            if (fadingOut)
            {
                if (FadeOutDuration <= 0f)
                {
                    spriteRenderer.color = new Color(0, 0, 0, 0);
                    objectPool.Recycle(ObjectName, gameObject);
                    return;
                }

                fadeTimeElapsed += dt;

                var finalColor = new Color(0, 0, 0, 0);
                spriteRenderer.color = Color.Lerp(Color.white, finalColor, fadeTimeElapsed / FadeOutDuration);
                if (fadeTimeElapsed > FadeOutDuration)
                {
                    objectPool.Recycle(ObjectName, gameObject);
                }
            }
        }
    }
}