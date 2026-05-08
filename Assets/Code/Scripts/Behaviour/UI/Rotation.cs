using UnityEngine;

namespace Code.Scripts.Behaviour.UI
{
  public class Rotation : PauseableAnimation
  {
    public float Speed;
    public bool AlwaysCanMove;

    public bool Rotating { get; set; } = true;

    private void Update()
    {
      if (AlwaysCanMove || (!paused && Rotating))
      {
        var rotation = transform.eulerAngles;
        transform.eulerAngles = new Vector3(rotation.x, rotation.y, transform.eulerAngles.z - Speed * Time.deltaTime);
      }
    }
  }
}