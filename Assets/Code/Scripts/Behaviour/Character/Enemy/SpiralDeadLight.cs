using Code.Scripts.Src;
using Code.Scripts.Src.Types;
using Code.Scripts.Src.Utils;
using System;
using Code.Scripts.Src.Types;
using Code.Scripts.Src.Utils;
using UnityEngine;

namespace Code.Scripts.Behaviour.Character.Enemy
{
    public class SpiralDeadLight : PoolableGameObject
    {
        public float Speed;

        public float InitialAngle { get; set; }

        public Gradient physicalGradient;
        public Gradient iceGradient;
        public Gradient fireGradient;
        public Gradient thunderGradient;
        public Gradient poisonGradient;

        private const float e = 2.7182818f;

        private float r0(float theta)
        {
            return 2 * Mathf.Sin(0.6f * theta);
        }

        private float r1(float theta)
        {
            return 2 * theta;
        }

        private TrailRenderer trailRenderer;

        private Vector2 basePosition;
        private float currentAngle;

        private float angleElapsed;

        private void Awake()
        {
            trailRenderer = GetComponent<TrailRenderer>();
        }

        public SpiralDeadLight SetHurtType(HurtTypes hurtType)
        {
            switch (hurtType)
            {
                case HurtTypes.PHYSICAL:
                    trailRenderer.colorGradient = physicalGradient;
                    break;
                case HurtTypes.MAGIC_ICE:
                    trailRenderer.colorGradient = iceGradient;
                    break;
                case HurtTypes.MAGIC_FIRE:
                    trailRenderer.colorGradient = fireGradient;
                    break;
                case HurtTypes.MAGIC_THUNDER:
                    trailRenderer.colorGradient = thunderGradient;
                    break;
                case HurtTypes.POISON:
                    trailRenderer.colorGradient = poisonGradient;
                    break;
            }

            return this;
        }

        public SpiralDeadLight SetPosition(Vector2 position)
        {
            transform.SetParent(null);
            basePosition = position;
            transform.position = position;
            trailRenderer.Clear();

            return this;
        }

        private void Polar(float angle, float radius)
        {
            transform.position = basePosition + (Vector2)(Quaternion.AngleAxis(InitialAngle + currentAngle, Vector3.forward) * Vector2.right * radius);
        }

        private void Update()
        {
            if (angleElapsed < 300)
            {
                Polar(currentAngle, r0(currentAngle * Mathf.Deg2Rad));
            }
            else
            {
                Polar(currentAngle - 300, r1((currentAngle - 300) * Mathf.Deg2Rad));
            }

            var angle = Speed * Time.deltaTime;
            currentAngle += angle;
            angleElapsed += angle;

            if (!GeneralUtils.IsInCamera(transform.position, 5))
            {
                ObjectPool.Instance.Recycle(ObjectName, gameObject);
            }
        }
    }
}
