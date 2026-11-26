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
    private List<StatusEffects> existingEffectsList = new List<StatusEffects>();
    private List<StatusEffectType> existingEffectsTypesList = new List<StatusEffectType>();


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
            if (existingEffectsList[i]._persistentEffect != null)
            {
                existingEffectsList[i]._persistentEffect(stats);
            }
        }
    }
    private void TrimExpiredEffects()
    {
        for(int i = existingEffectsList.Count-1; i >=0; i--)
        {
            if (existingEffectsList[i]._expirableType.Equals(ExpirableType.Expirable) && existingEffectsList[i].HasExpired(currTime){
                if (existingEffectsList[i]._onLeaveEffect != null)
                {
                    existingEffectsList[i]._onLeaveEffect(stats);
                }
                existingEffectsList.RemoveAt(i);
                RebuildExistingEffectTypesList();
                hasBuffListChanged = true;
            }
        }
    }
    private void ProcAllTickingEffects()
    {
        for(int i = 0; i < existingEffectsList.Count; i++)
        {
            if (existingEffectsList[i]._tickEffect != null) // if the status can tick
            {
                if (existingEffectsList[i]._nextTickTime <= currTime) // if the tick is ready to proc
                {
                    existingEffectsList[i].SetTickedTime(existingEffectsList[i]._nextTickTime + existingEffectsList[i]._tickInterval);
                    existingEffectsList[i]._tickEffect(stats);
                }
            }
        }
    }
    public void AddEffect(StatusEffects statusEffect)
    {
        if (existingEffectsTypesList.Contains(statusEffect._effectType))
        {
            if (statusEffect._canStack)
            {
                Debug.Log("it doesnt");
            }
            else
            {
                for(int i = 0; i < existingEffectsList.Count; i++)
                {
                    if (existingEffectsList[i]._effectType == statusEffect._effectType)
                    {
                        if(statusEffect._expiryTime > existingEffectsList[i]._expiryTime)
                        {
                            existingEffectsList[i]._expiryTime = statusEffect._expiryTime;
                        }
                        return;
                    }
                }
                Debug.LogError("Bug");
            }

        }
        else
        {
            existingEffectsList.Add(statusEffect);
            statusEffect.SetTickedTime(Time.time + statusEffect._tickInterval);
            hasBuffListChanged = true;
            RebuildExistingEffectTypesList();
        }
    }
    public void RemoveEffect(StatusEffectType type, bool triggerOnLeaveEffect) 
    {
        if (existingEffectsTypesList.Contains(type))
        {
            for (int i = 0; i < existingEffectsTypesList.Count; i++)
            {
                if (existingEffectsList[i]._effectType == type)
                {
                    if (triggerOnLeaveEffect && existingEffectsList[i]._onLeaveEffect != null)
                    {
                        existingEffectsList[i]._onLeaveEffect(stats);
                    }
                    existingEffectsList.RemoveAt(i);
                    hasBuffListChanged = true;
                    RebuildExistingEffectTypesList();
                    return;
                }
            }
        }
    }
    private void RebuildExistingEffectTypesList()
    {
        existingEffectsTypesList.Clear();
        for(int i = 0; i < existingEffectsList.Count; i++)
        {
            existingEffectsTypesList.Add(existingEffectsList[i]._effectType);
        }
    }

    

}
