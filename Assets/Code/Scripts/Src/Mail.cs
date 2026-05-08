using System.Collections.Generic;
using System.Linq;
using Code.Scripts.Src.Types;
using Code.Scripts.Src.Utils;

namespace Code.Scripts.Src
{
  public enum MailAddresses
  {
    MAIN_ROUTER,
    GAME_OVER,
  }

  public enum MailSenders
  {
    ROUTE_REQUEST,
    GAME_OVER_REQUEST,
  }

  public class Mail
  {
    public MailSenders Sender { get; set; }
    public MailContent MailContent { get; set; }
  }

  public class MailContainer
  {
    public Mail Mail { get; set; }
    public int Quantity { get; set; }
  }

  public interface MailContent
  {
  }

  public class RouteRequest : MailContent
  {
    public string Route { get; set; }
  }


  public class GameOverRequest : MailContent
  {
    public GameOverStatus Status { get; set; }
  }

  public class Mailer : Singleton<Mailer>
  {
    private readonly Dictionary<MailAddresses, List<MailContainer>> mails = new();

    public void SendMail(MailSenders sender, MailAddresses mailAddress, MailContent content, int quantity = 1)
    {
      if (!mails.ContainsKey(mailAddress))
      {
        mails[mailAddress] = new();
      }

      mails[mailAddress].Add(new()
      {
        Mail = new()
        {
          MailContent = content,
          Sender = sender,
        },
        Quantity = quantity,
      });
    }

    public List<Mail> GetMails(MailSenders sender, MailAddresses mailAddress)
    {
      if (!mails.TryGetValue(mailAddress, value: out var mail))
      {
        return new();
      }

      var mailContainers = mail
        .Where(mc => mc.Mail.Sender == sender)
        .ToList(); // This is required for deep cloning.

      foreach (var mailContainer in mailContainers)
      {
        mailContainer.Quantity -= 1;
      }

      mails[mailAddress].RemoveAll(mc => mc.Quantity <= 0);

      return mailContainers.Select(mc => mc.Mail).ToList();
    }

    public List<Mail> GetMails(MailAddresses mailAddress)
    {
      if (!mails.TryGetValue(mailAddress, out var mail))
      {
        return new();
      }

      var mailContainers = mail.ToList(); // This is required for deep cloning.
      foreach (var mailContainer in mailContainers)
      {
        mailContainer.Quantity -= 1;
      }

      mails[mailAddress].RemoveAll(mc => mc.Quantity <= 0);

      return mailContainers.Select(mc => mc.Mail).ToList();
    }

    public void ClearMails()
    {
      mails.Clear();
    }
  }
}