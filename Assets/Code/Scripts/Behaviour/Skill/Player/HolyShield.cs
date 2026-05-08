using System.Collections;
using System.Collections.Generic;
using Code.Scripts.Src.Store.Level;
using Code.Scripts.Src.Types;
using Code.Scripts.Src.Utils;
using Code.Scripts.Src.Types;
using UnityEngine;

namespace Code.Scripts.Behaviour.Skill
{
    public enum HolyShieldStatus
    {
        FADING_IN,
        NORMAL,
        FADING_OUT,
    }

    public class HolyShield : PauseableGameObject
    {
        private SpriteRenderer spriteRenderer;

        private Color fadeInColor;
        private Color fadeOutColor;

        private HolyShieldStatus holyShieldStatus;

        protected override void Awake()
        {
            base.Awake();
            spriteRenderer = GetComponent<SpriteRenderer>();
            fadeInColor = spriteRenderer.color;
            var color = spriteRenderer.color;
            color.a = 0;
            spriteRenderer.color = color;
            fadeOutColor = color;
            transform.localScale = Vector2.zero;

            holyShieldStatus = HolyShieldStatus.FADING_IN;
        }

        protected override void HandleGameStateChanged(GameState state)
        {
            GameStates gameState = state.CurrentGameState;
            paused = !LevelUtils.PlayerCanMove(gameState);
            if (animator != null)
            {
                animator.speed = paused ? 0 : 1;
            }
        }

        public void FadeOut()
        {
            holyShieldStatus = HolyShieldStatus.FADING_OUT;
        }

        private void Update()
        {
            if (!paused)
            {
                switch (holyShieldStatus)
                {
                    case HolyShieldStatus.FADING_IN:
                        spriteRenderer.color = Color.Lerp(spriteRenderer.color, fadeInColor, Time.deltaTime * 5);
                        transform.localScale = Vector2.Lerp(transform.localScale, Vector2.one, Time.deltaTime * 5);
                        if (((Vector2)transform.localScale - Vector2.one).magnitude <= 0.05f)
                        {
                            holyShieldStatus = HolyShieldStatus.NORMAL;
                        }
                        break;
                    case HolyShieldStatus.FADING_OUT:
                        spriteRenderer.color = Color.Lerp(spriteRenderer.color, fadeOutColor, Time.deltaTime * 5);
                        transform.localScale = Vector2.Lerp(transform.localScale, Vector2.zero, Time.deltaTime * 5);
                        if (((Vector2)transform.localScale - Vector2.zero).magnitude <= 0.05f)
                        {
                            Destroy(gameObject);
                        }
                        break;
                }
            }
        }
    }
}