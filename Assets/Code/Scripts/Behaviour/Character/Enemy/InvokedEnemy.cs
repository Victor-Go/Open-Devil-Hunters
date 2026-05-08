using Code.Scripts.Src.Types;
using Code.Scripts.Src.Types;
using UnityEngine;

namespace Code.Scripts.Behaviour.Character.Enemy
{
  public interface IEnemyCanInvoke
  {
    public void NoticeInvokedEnemyDied(string objectName);
  }

  public class InvokedEnemy : GeneralEnemy
  {
    protected IEnemyCanInvoke parentController;

    public void SetParentEnemy(IEnemyCanInvoke parent)
    {
      parentController = parent;
    }

    public override void OnEnemyDied(int playerNumber, HurtTypes hurtType)
    {
      base.OnEnemyDied(playerNumber, hurtType);

      parentController?.NoticeInvokedEnemyDied(ObjectName);
    }
  }
}