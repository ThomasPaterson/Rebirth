using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class BlockSand : Block
{

    public BlockSand()
        : base()
    {

    }

    public override Tile TexturePosition(Direction direction)
    {
        Tile tile = new Tile();

        tile.x = 14;
        tile.y = 13;

        return tile;
    }
}