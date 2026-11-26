using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using StatusEffectType = StatusEffects.StatusEffectType;
using StatusEffectApplicator = StatusEffects.StatusEffectApplicator;
using PriorityType = StatusEffects.PriorityType;
using ExpirableType = StatusEffects.ExpirableType;
using RemovableType = StatusEffects.RemovableType;

public class StatusEffectManager : MonoBehaviour
{
    private float currTime;
    private List<ActiveStatusEffect> existingEffectsList = new List<ActiveStatusEffect>();
    //private List<StatusEffectType> existingEffectsTypesList = new List<StatusEffectType>();


    private CharacterStats stats;
    private bool hasBuffListChanged = false;


    private void Start()
    {
        stats = gameObject.GetComponent<CharacterStats>();

    }

    private void Update()
    {
        currTime = Time.time;
        ProcAllTickingEffects();
        CheckForEffectListChange();
    }

    private void CheckForEffectListChange()
    {
        if ((hasBuffListChanged))
        {
            ResetAndReapplyPersistentEffects();
        }
        else
        {
            TrimExpiredEffects();
        }
    }
    private void ResetAndReapplyPersistentEffects()
    {
        hasBuffListChanged = false;
        TrimExpiredEffects();
        stats.ResetToInitialValues();

        existingEffectsList.Sort();

        for(int i = existingEffectsList.Count - 1; i >=0; i--)
        {
            ActiveStatusEffect active = existingEffectsList[i];

            if (active.Effect._persistentEffect != null)
            {
                StatusEffectContext context =
                    new StatusEffectContext(stats, active.TargetBodyPart);

                active.Effect._persistentEffect(context);
            }
        }
    }
    private void TrimExpiredEffects()
    {
        for (int i = existingEffectsList.Count - 1; i >= 0; i--)
        {
            ActiveStatusEffect active = existingEffectsList[i];

            if (active.HasExpired(currTime))
            {
                if (active.Effect._onLeaveEffect != null)
                {
                    StatusEffectContext context =
                        new StatusEffectContext(stats, active.TargetBodyPart);

                    active.Effect._onLeaveEffect(context);
                }

                existingEffectsList.RemoveAt(i);

                hasBuffListChanged = true;
            }
        }
    }
    private void ProcAllTickingEffects()
    {
        for (int i = 0; i < existingEffectsList.Count; i++)
        {
            ActiveStatusEffect active = existingEffectsList[i];

            if (active.Effect._tickEffect == null)
                continue;

            if (active.Effect._tickInterval <= 0)
                continue;

            if (active.NextTickTime <= currTime)
            {
                active.NextTickTime =
                    currTime + active.Effect._tickInterval;

                StatusEffectContext context =
                    new StatusEffectContext(stats, active.TargetBodyPart);

                active.Effect._tickEffect(context);
            }
        }
    }
    public void AddEffect(StatusEffects statusEffect,
        BodyPart targetBodyPart = null)
    {
        for (int i = 0; i < existingEffectsList.Count; i++)
        {
            ActiveStatusEffect existing = existingEffectsList[i];

            bool sameEffect =
                existing.Effect._effectType == statusEffect._effectType;

            bool sameTarget =
                existing.TargetBodyPart == targetBodyPart;

            if (sameEffect && sameTarget)
            {
                if (!statusEffect._canStack)
                {
                    if (statusEffect._expirableType ==
                        StatusEffects.ExpirableType.Expirable)
                    {
                        existing.ExpiryTime = Mathf.Max(
                            existing.ExpiryTime,
                            Time.time + statusEffect.duration
                        );
                    }

                    return;
                }
            }
        }

        // Create a new active instance.
        ActiveStatusEffect newEffect =
            new ActiveStatusEffect(statusEffect, targetBodyPart);

        existingEffectsList.Add(newEffect);

        // Execute the initial effect.
        if (statusEffect._entryEffect != null)
        {
            StatusEffectContext context =
                new StatusEffectContext(stats, targetBodyPart);

            statusEffect._entryEffect(context);
        }

        hasBuffListChanged = true;
    }
    public void RemoveEffect(StatusEffects.StatusEffectType type,
        BodyPart targetBodyPart = null,
        bool triggerOnLeaveEffect = true) 
    {
        for (int i = existingEffectsList.Count - 1; i >= 0; i--)
        {
            ActiveStatusEffect active = existingEffectsList[i];

            bool sameEffect = active.Effect._effectType == type;
            bool sameTarget = active.TargetBodyPart == targetBodyPart;

            if (sameEffect && sameTarget)
            {
                if (triggerOnLeaveEffect &&
                    active.Effect._onLeaveEffect != null)
                {
                    StatusEffectContext context =
                        new StatusEffectContext(stats, targetBodyPart);

                    active.Effect._onLeaveEffect(context);
                }

                existingEffectsList.RemoveAt(i);

                hasBuffListChanged = true;

                return;
            }
        }
    }

    //private void RebuildExistingEffectTypesList()
    //{
    //    existingEffectsTypesList.Clear();
    //    for(int i = 0; i < existingEffectsList.Count; i++)
    //    {
    //        existingEffectsTypesList.Add(existingEffectsList[i]._effectType);
    //    }
    //}

    

}
