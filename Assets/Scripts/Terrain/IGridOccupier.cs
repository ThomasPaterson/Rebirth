using UnityEngine;
using System.Collections;

public interface IGridOccupier
{
    GridLocation loc { get; set; }

    bool CanEnter(GridLocation newLoc);

    Vector3 MoveLocation(GridLocation newLoc);

    void MoveTo(GridLocation newLoc, bool animate = false);

    Species GetSpecies();

    UtilityGenerator GetGenerator();

    GameObject GetGameObject();
}
