using UnityEngine;

public class ActiveStatusEffect
{
    public StatusEffects Effect { get; private set; }
    public BodyPart TargetBodyPart { get; private set; }

    public float ExpiryTime { get; set; }
    public float NextTickTime { get; set; }

    public ActiveStatusEffect(
        StatusEffects effect,
        BodyPart targetBodyPart)
    {
        Effect = effect;
        TargetBodyPart = targetBodyPart;

        ExpiryTime = effect._expirableType ==
            StatusEffects.ExpirableType.Expirable
            ? Time.time + effect.duration
            : float.PositiveInfinity;

        NextTickTime = Time.time + effect._tickInterval;
    }

    public int CompareTo(ActiveStatusEffect other)
    {
        return Effect._priorityType.CompareTo(
            other.Effect._priorityType
        );
    }

    public bool HasExpired(float currentTime)
    {
        return currentTime >= ExpiryTime;
    }
}

