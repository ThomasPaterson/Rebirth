using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class BlockDirt : Block
{

    public BlockDirt()
        : base()
    {

    }

    public override Tile TexturePosition(Direction direction)
    {
        Tile tile = new Tile();

        switch (direction)
        {
            case Direction.up:
                tile.x = 2;
                tile.y = 15;
                return tile;
            case Direction.down:
                tile.x = 2;
                tile.y = 14;
                return tile;
        }

        tile.x = 2;
        tile.y = 14;

        return tile;
    }
}