using Code.Scripts.Src.Types;
using Code.Scripts.Src.Types;
using UnityEngine;

public class ForestFog : PauseableGameObject
{
    public float Speed;
    public float AngleSpeed;
    public float SelfAngleSpeed;

    private float speed;
    private Vector2 direction;
    private float angleChangePerSecond;
    private float selfRotation;

    protected override void Awake()
    {
        base.Awake();
        speed = Speed * Random.Range(0.5f, 1.5f);
        angleChangePerSecond = AngleSpeed * Random.Range(0.5f, 1.5f) * new[] { -1, 1 }[Random.Range(0, 2)];
        selfRotation = SelfAngleSpeed * Random.Range(0.5f, 1.5f) * new[] { -1, 1 }[Random.Range(0, 2)];
        direction = Quaternion.AngleAxis(Random.Range(0, 360), Vector3.forward) * Vector2.right;
    }

    private void FixedUpdate()
    {
        if (paused) return;

        var dt = Time.fixedDeltaTime;

        direction = (Quaternion.AngleAxis(angleChangePerSecond * dt * SelfAngleSpeed, Vector3.forward) * direction).normalized;
        transform.position = (Vector2)transform.position + direction * dt * speed;

        var eulerZ = transform.eulerAngles.z;
        transform.eulerAngles = new Vector3(0, 0, eulerZ + selfRotation * dt);
    }
}
