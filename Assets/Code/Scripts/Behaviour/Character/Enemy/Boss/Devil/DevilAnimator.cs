using System.Collections.Generic;
using Code.Scripts.Behaviour.Character.Enemy.Boss.Devil;
using UnityEngine;

namespace Code.Scripts.Behaviour.Character.Enemy
{
    public class DevilAnimator : MonoBehaviour
    {
        private Devil rootController;

        private void Awake()
        {
            rootController = GetComponentInParent<Devil>();
        }

        private void OnAttackAnimationFinished()
        {
            rootController.OnAttackAnimationFinished();
        }
    }
}