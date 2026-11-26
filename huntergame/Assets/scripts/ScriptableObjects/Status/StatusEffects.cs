using UnityEngine;

[CreateAssetMenu(fileName = "tempStatus", menuName = "New Status")]
public class StatusEffects : ScriptableObject
{
    

    

    public delegate void StatusEffectApplicator(StatusEffectContext context);

    public enum StatusEffectType { None,
        ExampleTempStatus,  // expiring statuses
        ExamplePermStatus   // "perm" statuses
    };

    public enum PriorityType { None = 2, Adder = 0, Multiplier = 1 }; //how this status effect will apply to the stats
    public int CompareTo(object o)
    {
        //compares status effect priorities
        StatusEffects otherPair = (StatusEffects) o;
        return _priorityType.CompareTo(otherPair._priorityType);
    }


    public enum RemovableType { Removable, NonRemovable};
    public enum ExpirableType { Expirable, NonExpirable};



    public StatusEffectType _effectType;
    public float _expiryTime;
    public bool _isBeneficial;
    public bool _canStack = false;
    public PriorityType _priorityType;
    public RemovableType _removableType;
    public ExpirableType _expirableType;
    protected int currTickCount = 0;

    [SerializeField] public float duration;

    //Initial effect that is called once when applied
    public StatusEffectApplicator _entryEffect;

    //passive modifiers while the effect is on an entity
    public StatusEffectApplicator _persistentEffect;

    //a DOT effect that occurs once every _tickInterval
    public StatusEffectApplicator _tickEffect;
    public float _tickInterval;
    //public float _nextTickTime = 0f;

    //effect that is called when the effect leaves the entity one way or another
    public StatusEffectApplicator _onLeaveEffect;


    public StatusEffects(
        StatusEffectType effectType,
        StatusEffectApplicator initialEffect,
        StatusEffectApplicator persistentEffect,
        StatusEffectApplicator tickEffect,
        StatusEffectApplicator onLeaveEffect,
        //PriorityType type,
        ExpirableType expirableType,
        RemovableType removableType,
        float tickInterval,
        float duration,
        bool isBeneficial,
        bool canStack){
        

        _effectType = effectType;
        _entryEffect = initialEffect;
        _persistentEffect = persistentEffect;
        _tickEffect = tickEffect;
        _onLeaveEffect = onLeaveEffect;
        _expiryTime = Time.time + duration;
        _isBeneficial = isBeneficial;
        //_priorityType = ProjectileTypeMapper.GetCombatEffectPriorityType(effectType);
        _canStack = canStack;
        _expirableType = expirableType;
        _removableType = removableType;
        _tickInterval = tickInterval;
    
    }

    public bool HasExpired (float currTime)
    {
        if (currTime > _expiryTime) return true;
        return false;
    }

    //public void SetTickedTime(float t)
    //{
    //    _nextTickTime = t;
    //}


    
}
