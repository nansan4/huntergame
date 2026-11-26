using UnityEngine;

public class StatusEffectContext
{
    public CharacterStats Stats { get; private set; }
    public BodyPart BodyPart { get; private set; }

    public StatusEffectContext(
        CharacterStats stats,
        BodyPart bodyPart)
    {
        Stats = stats;
        BodyPart = bodyPart;
    }
}
