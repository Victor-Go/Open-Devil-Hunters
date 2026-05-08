using UnityEngine;

namespace Code.Scripts.Behaviour.UI.Main
{
  public class FightPreparationGuideUI : GuideUI
  {
    protected override void Start()
    {
      base.Start();

      PlayerPrefs.SetInt("ShowedFightPreparationGuideUI", 1);
      PlayerPrefs.Save();
    }
  }
}