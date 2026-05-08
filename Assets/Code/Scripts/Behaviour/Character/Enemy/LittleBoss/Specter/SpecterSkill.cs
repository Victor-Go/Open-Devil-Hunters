using Code.Scripts.Src.Types;
using Code.Scripts.Src.Types;
using UnityEngine;

public class SpecterSkill : PauseableAnimation, IPoolableGameObject
{
    private SpriteRenderer spriteRenderer;
    private Color targetColor;

    public string ObjectName { get; set; }

    protected override void Awake()
    {
        base.Awake();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    protected override void Start()
    {
        spriteRenderer.color = GetRandomColor();
        PickRandomColor();
    }

    private Color GetRandomColor()
    {
        return new Color(Random.Range(0, 1f), Random.Range(0, 1f), Random.Range(0, 1f), 1);
    }

    private void PickRandomColor()
    {
        targetColor = GetRandomColor();
    }

    private void FixedUpdate()
    {
        if (paused) return;

        var color = spriteRenderer.color;
        spriteRenderer.color = Color.Lerp(color, targetColor, Time.fixedDeltaTime);
        color = spriteRenderer.color;
        var colorVector = new Vector3(color.r, color.g, color.b);
        var targetColorVector = new Vector3(targetColor.r, targetColor.g, targetColor.b);
        if ((colorVector - targetColorVector).magnitude <= 0.1f)
        {
            PickRandomColor();
        }
    }

    public virtual void ObjectReset(Vector2 initialPosition)
    {
        gameObject.SetActive(true);
        transform.localScale = Vector3.one;
        transform.position = initialPosition;
    }
}
