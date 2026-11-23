using UnityEngine;

[CreateAssetMenu(fileName = "tempStatus", menuName = "New Status")]
public class StatusEffects : MonoBehaviour
{

    public string _effectName;
    public float _duration;
    public bool _canStack;

    public StatusEffectManager applicator;

    


    // Effects that effect the character when applied
    public void EntryEffect()
    {

    }
    // Effects that effect the character every tick
    public void TickEffect()
    {

    }
    // Effects that effect the character persistently until they deplete
    public void PersistentEffect()
    {

    }
    // Effects that effect the character when they leave
    public void OnLeaveEffect()
    {

    }

    //public StatusEffectManager()
}
