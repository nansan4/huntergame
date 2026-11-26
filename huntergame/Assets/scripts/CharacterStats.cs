using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    public float maxHp;

    public float currHp;

    public float stamina;

    public List<BodyPartSO> bodyPartData;

    private Dictionary<BodyPartType, BodyPart> bodyParts;


    private void Awake()
    {
        bodyParts = new Dictionary<BodyPartType, BodyPart>();

        foreach (BodyPartSO data in bodyPartData)
        {
            bodyParts.Add(
                data.partType,
                new BodyPart(data)
                );
        }
    }
    public void DamagePart(
        BodyPartType part,
        float damage
        )
    {
        if (!bodyParts.TryGetValue(part, out BodyPart bodyPart))
        {
            return;
        }

        bodyPart.TakeDamage(damage);

        if (bodyPart.isDestroyed)
        {
            HandleDestroyedPart(bodyPart);
        }
    }

    private void HandleDestroyedPart(BodyPart part)
    {
        if (part.data.deathWhenDestroyed)
        {
            Debug.Log("death");
        }
        bodyParts.Remove(part.data.partType);
        //remove body part from dictionary
    }
    public float GetPartHealth(BodyPartType part)
    {
        return bodyParts.TryGetValue(part, out BodyPart bodyPart)
            ? bodyPart.currentHealth
            : 0f;
    }
    public void ResetToInitialValues()
    {

    }
}
