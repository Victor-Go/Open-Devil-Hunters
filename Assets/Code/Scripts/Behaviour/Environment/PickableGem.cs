using Code.Scripts.Behaviour.Character.Player;
using Code.Scripts.Src;
using Code.Scripts.Src.Utils;
using Code.Scripts.Behaviour.UI.Gem;
using Code.Scripts.Src;
using Code.Scripts.Src.Store.Player;
using Code.Scripts.Src.Types;
using Code.Scripts.Src.Utils;
using UnityEngine;

namespace Code.Scripts.Behaviour.Environment
{
  public class PickableGem : CircularTrail
  {
    public Gradient redGradient;
    public Gradient blueGradient;
    public Gradient greenGradient;
    public Gradient yellowGradient;
    public Gradient lemonGradient;
    public Gradient purpleGradient;

    private SpriteRenderer spriteRenderer;
    private TrailRenderer trailRenderer;
    private AudioClip audioClip;

    private GemProperties gemProperties;

    private bool triggered;

    protected override void Awake()
    {
      base.Awake();
      spriteRenderer = GetComponent<SpriteRenderer>();
      trailRenderer = GetComponentInChildren<TrailRenderer>();
    }

    protected override void Start()
    {
      base.Start();
      gemProperties = GemUtils.GenerateGemProperties();
      SetGemSprite();

      audioClip = ResourceManager.Instance.GetResource("Audio/Sound/Environment/get-gem_3");
    }

    private void SetGemSprite()
    {
      string gemPicResourceName = GemUtils.GetGemSpritePath(gemProperties);

      switch (gemProperties.Color)
      {
        case GemColors.RED:
          trailRenderer.colorGradient = redGradient;
          break;
        case GemColors.BLUE:
          trailRenderer.colorGradient = blueGradient;
          break;
        case GemColors.GREEN:
          trailRenderer.colorGradient = greenGradient;
          break;
        case GemColors.YELLOW:
          trailRenderer.colorGradient = yellowGradient;
          break;
        case GemColors.LEMON:
          trailRenderer.colorGradient = lemonGradient;
          break;
        case GemColors.PURPLE:
          trailRenderer.colorGradient = purpleGradient;
          break;
      }

      var texture = Resources.Load<Texture2D>(gemPicResourceName);
      spriteRenderer.sprite = Sprite.Create(texture, new Rect(0f, 0f, texture.width, texture.height),
        new Vector2(0.5f, 0.5f), 100f);
    }

    private void OnTriggerStay2D(Collider2D other)
    {
      if (!TriggerGroup.CanInteract(gameObject.layer, other.gameObject.layer) || triggered)
      {
        return;
      }

      triggered = true;

      PlayerNumber = other.GetComponent<PickUpControl>().PlayerNumber;

      var playerPosition = storeManager
        .GetState<PlayerPositionState>(StoreNames.PlayerPositionStore)
        .PlayerPositions[PlayerNumber];
      TriggerCircularTrail(Quaternion.AngleAxis(Random.Range(-10, 10), Vector3.forward) *
                           ((Vector2)transform.position - playerPosition));
    }

    private void ShowGemUi()
    {
      var gemUi = WindowManager.Instance.OpenWindow(WindowNames.GemUI, OpenWindowActions.WaitForOthers);
      gemUi.GetComponent<GemUI>().SetGem(gemProperties);
    }

    protected override void OnTrailFinished()
    {
      AudioWrapper.PlayClip(audioClip, transform.position);
      ShowGemUi();

      base.OnTrailFinished();
    }
  }
}