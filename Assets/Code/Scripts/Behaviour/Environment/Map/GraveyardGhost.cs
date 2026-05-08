using Code.Scripts.Src.Types;
using Code.Scripts.Src.Types;
using UnityEngine;

namespace Code.Scripts.Behaviour.Environment
{
    public class GraveyardGhost : EnvironmentMovableObject
    {
        private SpriteRenderer spriteRenderer;

        private float randomCoef;

        protected override void Awake()
        {
            base.Awake();

            spriteRenderer = GetComponent<SpriteRenderer>();

            randomCoef = Random.Range(0, 1);
            rad = x => Mathf.Sin(5 * x + randomCoef) / 2;
        }

        public override void ObjectReset(Vector2 initialPosition)
        {
            base.ObjectReset(initialPosition);
            randomCoef = Random.Range(0, Mathf.PI);
        }

        protected override void FixedUpdate()
        {
            base.FixedUpdate();

            if (paused) return;

            var color = spriteRenderer.color;
            spriteRenderer.color = new Color(color.r, color.g, color.b, Mathf.Sin(lifeTime) / 5 + 0.3f);
        }
    }
}