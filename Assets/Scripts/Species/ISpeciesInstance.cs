using UnityEngine;
using System.Collections;

public interface ISpeciesInstance
{
    Species GetSpecies();

   void HandleInstance();

    void Init(Species species);

    Sprite GetIcon();

    bool CompareType(ISpeciesInstance otherInst);
}
