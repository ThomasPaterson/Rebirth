using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class Species
{

    public List<ISpeciesInstance> speciesObjects = new List<ISpeciesInstance>();
    public List<Trait> traits = new List<Trait>();
    public GameObject prefab;

    public void AddMember(ISpeciesInstance newMember)
    {
        speciesObjects.Add(newMember);
    }

    public void ProcessSpecies()
    {
        //converted to array, since probably more coming in this frame
        ISpeciesInstance[] instances = speciesObjects.ToArray();

        for (int i = 0; i < instances.Length; i++)
            if (instances[i] != null)
                instances[i].HandleInstance();
    }

    public Trait GetTrait(int index)
    {
        return traits.Count > index ? traits[index] : null;
    }

    public void AddTrait(Trait t)
    {
        if (traits.Count < 3 && t != null)
        {
            traits.Add(t);
            ProcessTrait(t, 1f);
        }
           
    }

    public bool HasTrait(Trait.Type type)
    {
        foreach (Trait t in traits)
            if (t.effect == type)
                return true;

        return false;
    }

    public void RemoveTrait(int index)
    {
        if (traits.Count > index)
        {
            ProcessTrait(traits[index], -1f);
            traits.RemoveAt(index);
        }
            
    }

    public Sprite GetIcon()
    {
        if (prefab == null)
            return null;

        return prefab.GetComponentInChildren<ISpeciesInstance>().GetIcon();
    }

    public virtual bool CanEnter(GridLocation newLoc)
    {
        return false;
    }

    public void RemoveInstance(ISpeciesInstance inst)
    {
        speciesObjects.Remove(inst);
    }

    protected virtual void ProcessTrait(Trait t, float direction)
    {

    }
}
