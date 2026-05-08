using System.Collections.Generic;
using System.Linq;
using Code.Scripts.Src;
using Code.Scripts.Src.Types;
using UnityEngine;

namespace Code.Scripts.Behaviour.Character.Player
{
  public struct AnimatorFinishedActionInfo
  {
  }

  public class AnimatorControl : PauseableGameObject, IObservable<AnimatorFinishedActionInfo>
  {
    public bool IndependentWeapon; // Don't forget to change this value in AttackControl and MovementControl.

    public AnimatorOverrideController[] OverrideControllers;

    private readonly List<IObserver<AnimatorFinishedActionInfo>> observers = new();

    private GameObject weaponGameObject;
    private Transform localRotationAxis;
    private bool playingAttackAnimation;

    protected override void Awake()
    {
      base.Awake();
      animator = GetComponent<Animator>();

      if (IndependentWeapon)
      {
        weaponGameObject = transform.Find("Weapon").gameObject;
        localRotationAxis = weaponGameObject.transform.Find("LocalRotationAxis");

        weaponGameObject.SetActive(false);
      }
    }

    public void SetAttackAnimationSpeed(float speed)
    {
      animator.SetFloat("AttackAnimationSpeed", speed);
    }

    public void PlayAttackAnimation()
    {
      if (!playingAttackAnimation && OverrideControllers.Any())
      {
        animator.runtimeAnimatorController = OverrideControllers[Random.Range(0, OverrideControllers.Length)];
      }

      playingAttackAnimation = true;
      animator.SetTrigger("TriggerAttack");
    }

    public void ResetAttackAnimation()
    {
      animator.ResetTrigger("TriggerAttack");
    }

    public void SetTargetPosition(Vector3 rotationAxisGlobalPosition, Vector2 targetPosition)
    {
      if (IndependentWeapon)
      {
        weaponGameObject.SetActive(true);
        var tf = weaponGameObject.transform;

        int direction = transform.localScale.x > 0 ? 1 : -1;

        tf.position = rotationAxisGlobalPosition - direction * localRotationAxis.localPosition;
        tf.rotation = Quaternion.Euler(0, 0, 0);
        var angle = Vector2.SignedAngle(Vector2.left * direction, targetPosition - (Vector2)localRotationAxis.position);
        weaponGameObject.transform.RotateAround(localRotationAxis.position, Vector3.forward, angle);
      }
    }

    public void SetWeaponActive(bool active)
    {
      if (IndependentWeapon)
      {
        weaponGameObject.SetActive(active);
      }
    }

    public IDisposable Subscribe(IObserver<AnimatorFinishedActionInfo> observer)
    {
      if (!observers.Contains(observer))
      {
        observers.Add(observer);
      }

      return new Unsubscriber<AnimatorFinishedActionInfo>(observers, observer);
    }

    public void AttackAnimationFinished()
    {
      playingAttackAnimation = false;
      if (IndependentWeapon)
      {
        weaponGameObject.SetActive(false);
      }

      foreach (var observer in observers)
      {
        observer.OnNext(new() { });
      }
    }
  }
}