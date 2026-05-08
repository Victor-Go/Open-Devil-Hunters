using System;
using System.Linq;
using Code.Scripts.Src;
using Code.Scripts.Src.Store.Level;
using Code.Scripts.Src.Types;
using Code.Scripts.Src.Utils;
using UnityEngine;

namespace Code.Scripts.Behaviour.Animation
{
  public class SimpleAnimation : PoolableAndPauseableGameObject
  {
    public string[] AudioNamesOnInit;

    public float FadeInDuration;
    public float FadeOutDuration;
    public float RotationRange;

    private Action fadeOutFinishedAction;
    private bool recycleAnimationAfterFinished;

    private float fadeInTimeElapsed;
    private float fadeOutTimeElapsed;
    private bool fadingIn;
    private bool fadingOut;

    private AudioWrapper audioWrapper;

    protected override void Awake()
    {
      base.Awake();

      if (AudioNamesOnInit != null && AudioNamesOnInit.Length > 0)
      {
        audioWrapper = new(AudioNamesOnInit, transform);
      }

      animator = GetComponent<Animator>();

      transform.Rotate(new Vector3(0, 0, UnityEngine.Random.Range(-RotationRange, RotationRange)));
    }

    protected override void Start()
    {
      base.Start();
      if (FadeInDuration > 0)
      {
        fadeInTimeElapsed = 0;
        transform.localScale = Vector2.zero;
        fadingIn = true;
      }

      if (audioWrapper != null)
      {
        audioWrapper.PlayRandomly();
      }
    }

    public override void ObjectReset(Vector2 initialPosition)
    {
      base.ObjectReset(initialPosition);

      if (audioWrapper != null)
      {
        audioWrapper.PlayRandomly();
      }

      transform.Rotate(new Vector3(0, 0, UnityEngine.Random.Range(-RotationRange, RotationRange)));
      fadeOutFinishedAction = null;
      fadeOutTimeElapsed = 0;
      fadeInTimeElapsed = 0;
      fadingOut = false;
      fadingIn = false;
      transform.localScale = Vector2.one;
      Start();
    }

    public override void OnStoreChanged(StoreNames storeName, IState state)
    {
      switch (storeName)
      {
        case StoreNames.GameStateStore:
          HandleGameStateChanged((GameState)state);
          break;
        default:
          throw new InvalidStoreEventException(storeName);
      }
    }

    public void AnimationFinishedCallBack()
    {
      if (recycleAnimationAfterFinished)
      {
        RecycleAnimation();
      }
    }

    public SimpleAnimation SetRecycleAnimationAfterFinished(bool recycle = false)
    {
      recycleAnimationAfterFinished = recycle;
      return this;
    }

    public void StartAnimation(Action fadeOutFinishedAction, bool recycleAnimationAfterFinished = false)
    {
      this.fadeOutFinishedAction = fadeOutFinishedAction;
      SetRecycleAnimationAfterFinished(recycleAnimationAfterFinished);
    }

    public void FadeOutAnimation()
    {
      fadeOutTimeElapsed = 0;
      fadingOut = true;
      fadingIn = false;
    }

    public void RecycleAnimation()
    {
      if (fadeOutFinishedAction != null)
      {
        fadeOutFinishedAction();
      }

      objectPool.Recycle(ObjectName, gameObject);
    }

    private void Update()
    {
      if (fadingOut)
      {
        if (FadeOutDuration == 0)
        {
          transform.localScale = Vector2.zero;
          fadingOut = false;
          RecycleAnimation();
          return;
        }

        fadeOutTimeElapsed += Time.deltaTime;
        transform.localScale = new Vector2(1 - (fadeOutTimeElapsed / FadeOutDuration),
          1 - (fadeOutTimeElapsed / FadeOutDuration));
        if (fadeOutTimeElapsed >= FadeOutDuration)
        {
          fadingOut = false;
          RecycleAnimation();
        }
      }
      else if (fadingIn)
      {
        fadeInTimeElapsed += Time.deltaTime;
        transform.localScale = new Vector2(fadeInTimeElapsed / FadeInDuration, fadeInTimeElapsed / FadeInDuration);

        if (fadeInTimeElapsed >= FadeInDuration)
        {
          transform.localScale = Vector2.one;
          fadingIn = false;
        }
      }
    }
  }
}