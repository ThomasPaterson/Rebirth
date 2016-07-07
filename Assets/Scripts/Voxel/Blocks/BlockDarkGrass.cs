using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class BlockDarkGrass : Block
{

    public BlockDarkGrass()
        : base()
    {

    }

    public override Tile TexturePosition(Direction direction)
    {
        Tile tile = new Tile();

        switch (direction)
        {
            case Direction.up:
                tile.x = 1;
                tile.y = 7;

                return tile;
        }

        tile.x = 1;
        tile.y = 6;

        return tile;
    }
}