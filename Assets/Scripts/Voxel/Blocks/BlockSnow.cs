using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class BlockSnow : Block
{

    public BlockSnow()
        : base()
    {

    }

    public override Tile TexturePosition(Direction direction)
    {
        Tile tile = new Tile();

        switch (direction)
        {
            case Direction.up:
                tile.x = 12;
                tile.y = 9;

            return tile;
        }

        tile.x = 12;
        tile.y = 8;

        return tile;
    }
}