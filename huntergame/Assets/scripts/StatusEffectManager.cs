using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class StatusEffectManager : MonoBehaviour
{
    private float currTime;
    public List<StatusEffects> effectsList = new List<StatusEffects>();



    private CharacterStats stats;
    private bool hasBuffListChanged = false;


    private void Start()
    {
        
    }

    private void Update()
    {
        currTime = Time.time;
        ProcAllTickingEffects();
        CheckForEffectListChange();
    }
    private void ProcAllTickingEffects()
    {

    }

    private void CheckForEffectListChange()
    {

    }

}
