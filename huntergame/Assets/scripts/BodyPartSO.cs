using UnityEngine;


public enum BodyPartType
{
    Head,
    Chest,
    LeftArm,
    RightArm,
    LeftLeg,
    RightLeg
}
[CreateAssetMenu(fileName = "NewBodyPart", menuName = "BodyPart")]
public class BodyPartSO : ScriptableObject
{
   
    public BodyPartType partType;

    public float maxHealth = 100f; //temp

    public bool deathWhenDestroyed;

}
