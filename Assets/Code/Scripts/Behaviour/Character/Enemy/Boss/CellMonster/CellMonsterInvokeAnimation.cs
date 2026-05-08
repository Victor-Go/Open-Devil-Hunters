using Code.Scripts.Src.Types;
using Code.Scripts.Src.Types;
using UnityEngine;

namespace Code.Scripts.Behaviour.Character.Enemy
{
    public enum CellMonsterInvokeAnimationDirection
    {
        LEFT,
        RIGHT,
    }

    public class CellMonsterInvokeAnimation : PoolableAndPauseableGameObject
    {
        private System.Action animationFinishedCallback;
        private Vector2 size;

        protected override void Awake()
        {
            base.Awake();

            size = GetComponent<BoxCollider2D>().size;
        }

        public CellMonsterInvokeAnimation SetPosition(Vector2 position, CellMonsterInvokeAnimationDirection direction)
        {
            switch (direction)
            {
                case CellMonsterInvokeAnimationDirection.LEFT:
                    transform.position = position - size / 2;
                    transform.localScale = Vector2.one;
                    break;
                case CellMonsterInvokeAnimationDirection.RIGHT:
                    transform.position = position + new Vector2(size.x / 2, -size.y / 2);
                    transform.localScale = new Vector2(-1, 1);
                    break;
            }

            return this;
        }

        public CellMonsterInvokeAnimation SetAnimationFinishedCallback(System.Action callback)
        {
            animationFinishedCallback = callback;
            return this;

        }

        public void OnAnimationFinished()
        {
            animationFinishedCallback();
            objectPool.Recycle(ObjectName, gameObject);
        }
    }
}