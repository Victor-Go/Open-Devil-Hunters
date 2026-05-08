using UnityEngine;

namespace Code.Scripts.Behaviour.UI.Main
{
  public class FightReadyGuideUI : GuideUI
  {
    protected override void Start()
    {
      base.Start();
      
      PlayerPrefs.SetInt("ShowedFightReadyGuideUI", 1);
      PlayerPrefs.Save();
    }
  }
}