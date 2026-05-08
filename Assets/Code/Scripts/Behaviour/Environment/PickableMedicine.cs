using System.Collections.Generic;
using Code.Scripts.Behaviour.Character.Player;
using Code.Scripts.Src;
using Code.Scripts.Src.Utils;
using Code.Scripts.Src.Store.Player;
using UnityEngine;

namespace Code.Scripts.Behaviour.Environment
{
  public class PickableMedicine : CircularTrail
  {
    public List<Sprite> Sprites;

    private float hpIncrement;
    private float hpIncrementPercentage;
    private bool isValueIncrement;
    private bool triggered;

    private SpriteRenderer spriteRenderer;

    protected override void Awake()
    {
      base.Awake();

      spriteRenderer = GetComponent<SpriteRenderer>();
    }

    protected override void Start()
    {
      base.Start();

      isValueIncrement = Random.Range(0, 2) < 1;
      const int maxIncrement = 500;

      var i = Random.Range(0, Sprites.Count);
      spriteRenderer.sprite = Sprites[i];
      
      if (isValueIncrement)
      {
        var range = maxIncrement / Sprites.Count;
        hpIncrement = Mathf.Max(range * i + Random.Range(0, range), 10);
      }
      else
      {
        var range = 1f / Sprites.Count;
        hpIncrementPercentage = Random.Range(range * i + Random.Range(0, range), 0.1f);
      }
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

    protected override void OnTrailFinished()
    {
      base.OnTrailFinished();
      if (isValueIncrement)
      {
        storeManager.Commit(StoreNames.PlayerStore, StoreActions.PlayerStore_ADD_CURRENT_HP, new PlayerActionData()
        {
          PlayerNumber = PlayerNumber,
          AddCurrentHp = hpIncrement,
        });
      }
      else
      {
        storeManager.Commit(StoreNames.PlayerStore, StoreActions.PlayerStore_ADD_CURRENT_HP_PERCENTAGE, new PlayerActionData()
        {
          PlayerNumber = PlayerNumber,
          AddCurrentHpPercentage = hpIncrementPercentage,
        });
      }
    }
  }
}