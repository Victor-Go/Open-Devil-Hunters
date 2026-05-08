using UnityEngine;

namespace Code.Scripts.Behaviour.UI
{
  public enum RotationAxis
  {
    X,
    Y,
    Z
  }

  public class RandomRotation : MonoBehaviour
  {
    public RotationAxis axis;

    private void Start()
    {
      transform.rotation = axis switch
      {
        RotationAxis.X => Quaternion.Euler(Random.Range(0f, 360f), 0f, 0f),
        RotationAxis.Y => Quaternion.Euler(0f, Random.Range(0f, 360f), 0f),
        RotationAxis.Z => Quaternion.Euler(0f, 0f, Random.Range(0f, 360f)),
        _ => transform.rotation
      };
    }
  }
}