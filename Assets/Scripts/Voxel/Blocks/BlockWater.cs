using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class BlockWater : Block
{

    public BlockWater()
        : base()
    {

    }

    public override Tile TexturePosition(Direction direction)
    {
        Tile tile = new Tile();

        tile.x = 6;
        tile.y = 7;

        return tile;
    }
}