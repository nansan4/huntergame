using UnityEngine;



public enum InjuryType
{
    Bleeding,
    Fracture,
    Pain,
    Infection
}
[CreateAssetMenu(fileName = "NewInjury", menuName = "Injury")]
public class InjurySO : ScriptableObject
{

    public InjuryType injuryType;

    public float duration;
    public float damagePerTick;

    

}
