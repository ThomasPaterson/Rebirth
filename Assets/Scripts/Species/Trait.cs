using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Trait : ScriptableObject
{
    public enum Type { ExtraFood, ExtraArid, ExtraAltitude, ExtraWater, Fruit, Weed, Poisonous, Travelling, PoisonResistant,
    Climbing, LongNecked, Vicious, Fertile, SlowEater, LongLegs, SlowMetabolism, Claws, Huge, Tiny, Hardy}

    public string description;
    public string traitName;
    public Type effect;

    public virtual void AddToSpecies(Species species) { }

    public virtual void RemoveFromSpecies(Species species) { }

}
