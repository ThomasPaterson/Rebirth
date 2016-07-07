using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class BlockAridGrass : Block
{

    public BlockAridGrass()
        : base()
    {

    }

    public override Tile TexturePosition(Direction direction)
    {
        Tile tile = new Tile();

        switch (direction)
        {
            case Direction.up:
                tile.x = 5;
                tile.y = 15;

                return tile;
        }

        tile.x = 5;
        tile.y = 14;

        return tile;
    }
}