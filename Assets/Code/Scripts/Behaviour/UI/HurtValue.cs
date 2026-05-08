using Code.Scripts.Src;
using Code.Scripts.Src.Store.Level;
using Code.Scripts.Src.Types;
using System;
using Code.Scripts.Behaviour.Level;
using Code.Scripts.Src.Types;
using TMPro;
using UnityEngine;

public class HurtValue : PoolableAndPauseableGameObject, IStoreChangedHandler
{
  public Color PhysicsColor;
  public Color IceColor;
  public Color FireColor;
  public Color ThunderColor;
  public Color PoisonColor;

  public float Duration = 1;
  public float FadeDistance = 0.1f;
  public float FadeInDuration = 0.1f;
  public float FadeOutDuration = 0.25f;

  private TextMeshPro textMeshPro;
  private Vector2 defaultPosition;

  private Scheduling scheduling;
  private string scheduleId;

  private bool fadingIn, fadingOut;
  private float fadeInSpeed, fadeOutSpeed;

  private MeshRenderer meshRenderer;

  protected override void Awake()
  {
    base.Awake();
    objectPool = ObjectPool.Instance;
    scheduling = Scheduling.Instance;
    storeManager = StoreManager.Instance;
    storeManager.Subscribe(StoreNames.GameStateStore, this);

    textMeshPro = GetComponent<TextMeshPro>();
    fadeInSpeed = FadeDistance / FadeInDuration;
    fadeOutSpeed = FadeDistance / FadeOutDuration;

    meshRenderer = GetComponent<MeshRenderer>();
  }

  private void WaitForFadeOut()
  {
    scheduleId = scheduling.SetTimeout(() => { fadingOut = true; }, Duration + FadeInDuration);
  }

  protected override void Start()
  {
    base.Start();
    HandleGameStateChanged(storeManager.GetState<GameState>(StoreNames.GameStateStore));

    scheduling.ClearSchedule(scheduleId);
    WaitForFadeOut();
  }

  public HurtValue SetDefaultPosition(Vector2 position)
  {
    fadingIn = true;
    defaultPosition = position;
    transform.position = position - Vector2.up * FadeDistance;
    return this;
  }

  public override void ObjectReset(Vector2 initialPosition)
  {
    base.ObjectReset(initialPosition);

    fadingIn = true;
    fadingOut = false;

    HandleGameStateChanged(storeManager.GetState<GameState>(StoreNames.GameStateStore));

    scheduling.ClearSchedule(scheduleId);
    WaitForFadeOut();
  }

  public HurtValue SetHurtType(HurtTypes hurtType)
  {
    switch (hurtType)
    {
      case HurtTypes.PHYSICAL:
        textMeshPro.color = PhysicsColor;
        break;
      case HurtTypes.MAGIC_FIRE:
        textMeshPro.color = FireColor;
        break;
      case HurtTypes.MAGIC_ICE:
        textMeshPro.color = IceColor;
        break;
      case HurtTypes.MAGIC_THUNDER:
        textMeshPro.color = ThunderColor;
        break;
      case HurtTypes.POISON:
        textMeshPro.color = PoisonColor;
        break;
    }

    return this;
  }

  public HurtValue SetText(float value)
  {
    textMeshPro.text = Mathf.RoundToInt(value).ToString();
    return this;
  }

  void Update()
  {
    if (paused)
    {
      return;
    }

    if (fadingIn)
    {
      transform.position += (Vector3)(fadeInSpeed * Time.deltaTime * Vector2.up);
      Color color = textMeshPro.color;
      textMeshPro.color = new Color(color.r, color.g, color.b,
        1 - Mathf.Abs((transform.position.y - defaultPosition.y) / FadeDistance));
      if (transform.position.y >= defaultPosition.y)
      {
        color.a = 1;
        transform.position = defaultPosition;
        fadingIn = false;
      }
    }
    else if (fadingOut)
    {
      transform.position += (Vector3)(fadeOutSpeed * Time.deltaTime * Vector2.down);
      Color color = textMeshPro.color;
      textMeshPro.color = new Color(color.r, color.g, color.b,
        1 - Mathf.Abs((transform.position.y - defaultPosition.y) / FadeDistance));
      if (transform.position.y <= defaultPosition.y - FadeDistance)
      {
        objectPool.Recycle(ObjectName, gameObject);
      }
    }
  }
}