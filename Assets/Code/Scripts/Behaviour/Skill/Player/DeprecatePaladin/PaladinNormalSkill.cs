using Code.Scripts.Src;
using Code.Scripts.Src.Store.Level;
using Code.Scripts.Src.Types;
using Code.Scripts.Src.Utils;
using Code.Scripts.Src.Types;
using UnityEngine;

public class PaladinNormalSkill : PlayerBasicSkill, IStoreChangedHandler, IPoolableGameObject
{
    public new BasicSkillConfigurations SkillConfigurations;

    private float rangeElapsed;

    protected override void Awake()
    {
        base.Awake();

        objectPool = ObjectPool.Instance;
        storeManager = StoreManager.Instance;
        
        animator = GetComponent<Animator>();

        var r = transform.eulerAngles;
        transform.eulerAngles = new Vector3(r.x, r.y, Random.Range(0, 360));
    }

    protected override void Start()
    {
        base.Start();

        storeManager.Subscribe(StoreNames.GameStateStore, this);
        transform.localScale = Vector2.zero;
    }

    public override void ObjectReset(Vector2 initialPosition)
    {
        base.ObjectReset(initialPosition);

        rangeElapsed = 0;
        transform.localScale = Vector3.one;
    }

    private void FixedUpdate()
    {
        if (paused)
        {
            return;
        }

        var dt = Time.fixedDeltaTime;
        var distance = SkillConfigurations.Speed * dt;

        rangeElapsed += distance;

        if (rangeElapsed >= SkillConfigurations.Range)
        {
            objectPool.Recycle(ObjectName, gameObject);
        }

        var localScale = transform.localScale;
        transform.localScale = new Vector2(localScale.x + distance, localScale.y + distance);
    }
}
