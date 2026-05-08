using Code.Scripts.Src;
using Code.Scripts.Src.Store.Level;
using Code.Scripts.Src.Store.Level;
using UnityEngine;
using UnityEngine.UI;

namespace Code.Scripts.Behaviour.UI
{
  public class PlayerTimingSkillDisplayer : MonoBehaviour, IStoreChangedHandler
  {
    public int PlayerNumber;
    public int SkillIndex;

    private StoreManager storeManager;

    private Text remainingTimeText;
    private Image skillImage;
    private Image skillMask;

    private string currentImageName;

    private void Awake()
    {
      storeManager = StoreManager.Instance;

      remainingTimeText = GetComponentInChildren<Text>();
      skillImage = transform.Find("Image").GetComponent<Image>();

      skillMask = transform.Find("Mask").GetComponent<Image>();
      skillMask.material = new Material(skillMask.material);
    }

    private void Start()
    {
      storeManager.Subscribe(StoreNames.TimingSkillStore, this);
      remainingTimeText.text = "";
      skillMask.material.SetFloat("_Arc1", 360);
    }

    public void OnStoreChanged(StoreNames storeName, IState state)
    {
      switch (storeName)
      {
        case StoreNames.TimingSkillStore:
          if (PlayerNumber > ((TimingSkillState)state).PlayerSkills.Length - 1) break;


          var skills = ((TimingSkillState)state).PlayerSkills[PlayerNumber].Skills;

          if (SkillIndex < skills.Count)
          {
            var timeToLaunch = skills[SkillIndex].TimeToLaunch;
            var interval = skills[SkillIndex].Interval;
            remainingTimeText.text = timeToLaunch.ToString("n2");
            skillMask.material.SetFloat("_Arc1", 360 * (1 - timeToLaunch / interval));

            if (currentImageName != skills[SkillIndex].ImageName)
            {
              currentImageName = skills[SkillIndex].ImageName;
              var tex = Resources.Load<Texture2D>(currentImageName);
              if (tex == null) tex = Resources.Load<Texture2D>("UpgradeImage/default");
              skillImage.sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), Vector2.one / 2);
            }
          }

          break;
        default:
          throw new InvalidStoreEventException(storeName);
      }
    }

    private void OnDestroy()
    {
      storeManager.Unsubscribe(this);
    }
  }
}