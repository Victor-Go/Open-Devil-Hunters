using System;
using System.Collections.Generic;
using System.Linq;
using Code.Scripts.Src;
using Code.Scripts.Src.Types;
using Code.Scripts.Src;
using UnityEngine;

namespace Code.Scripts.Behaviour.Main
{
  public class MainUiManager : WindowManager
  {
    private void Start()
    {
      var mails = Mailer.Instance.GetMails(MailAddresses.MAIN_ROUTER);
      if (mails.Any(mail => ((RouteRequest)mail.MailContent).Route == "FightPreparation"))
      {
        OpenWindow(WindowNames.FightPreparation, OpenWindowActions.CloseOthers);
      }
      else
      {
        OpenWindow(WindowNames.InitialUI, OpenWindowActions.CloseOthers);
      }
    }
  }
}