using System.Collections.Generic;
using Code.Scripts.Behaviour.Character.Player;
using Code.Scripts.Behaviour.Character.Player;
using Code.Scripts.Src.Utils;

namespace Code.Scripts.Src
{
  public class PlayerControllers : Singleton<PlayerControllers>
  {
    public PlayerManager PlayerManager { get; set; }
    public PlayerHurtHandling PlayerHurtHandling { get; set; }
  }
}