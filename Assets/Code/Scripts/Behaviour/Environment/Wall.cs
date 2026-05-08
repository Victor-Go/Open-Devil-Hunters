using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Code.Scripts.Behaviour.Environment
{
    public class Wall : MonoBehaviour
    {
        private float colliderWidth { get; } = 0.25f;

        private SpriteRenderer spriteRenderer;
        private BoxCollider2D[] boxCollider2Ds = new BoxCollider2D[4];

        private void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();

            var colliders = GetComponents<BoxCollider2D>();
            for (int i = 0; i < boxCollider2Ds.Length; i++)
            {
                boxCollider2Ds[i] = colliders[i];
            }
        }

        private void Start()
        {
            SetSize(new Vector2(20, 20));
        }

        public void SetSize(Vector2 size)
        {
            spriteRenderer.size = size;

            // Left
            boxCollider2Ds[0].size = new Vector2(colliderWidth, size.y);
            boxCollider2Ds[0].offset = new Vector2(-size.y / 2 + colliderWidth, 0);

            // Right
            boxCollider2Ds[1].size = new Vector2(colliderWidth, size.y);
            boxCollider2Ds[1].offset = new Vector2(size.y / 2 - colliderWidth, 0);

            // Top
            boxCollider2Ds[2].size = new Vector2(size.x, colliderWidth);
            boxCollider2Ds[2].offset = new Vector2(0, size.y / 2 - colliderWidth);

            // Bottom
            boxCollider2Ds[3].size = new Vector2(size.x, colliderWidth);
            boxCollider2Ds[3].offset = new Vector2(0, -size.y / 2 + colliderWidth);
        }
    }
}