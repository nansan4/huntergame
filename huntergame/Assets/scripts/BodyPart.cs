using System;
using System.Collections.Generic;

public class BodyPart
{
    public BodyPartSO data;

    public float currentHealth {  get; private set; }

    public bool isDestroyed => currentHealth <= 0;

    public List<InjuryType> Injuries = new List<InjuryType>();
    public BodyPart(BodyPartSO bodyPartData)
    {
        data = bodyPartData;
        currentHealth = data.maxHealth;
    }

    public void AddInjury(InjuryType injury)
    {
        Injuries.Add(injury);
    }

    public void RemoveInjury(InjuryType injury)
    {
        Injuries.Remove(injury);
    }
    public void TakeDamage(float damage)
    {
        currentHealth = Math.Max(0, currentHealth - damage);
    }

    public void Heal(float amount)
    {
        currentHealth = Math.Min(data.maxHealth, currentHealth+amount);
    }
}
