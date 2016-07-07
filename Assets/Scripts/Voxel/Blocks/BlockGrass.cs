using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class BlockGrass : Block
{

    public BlockGrass()
        : base()
    {

    }

    public override Tile TexturePosition(Direction direction)
    {
        Tile tile = new Tile();

        switch (direction)
        {
            case Direction.up:
                tile.x = 0;
                tile.y = 15;
                return tile;
            case Direction.down:
                tile.x = 3;
                tile.y = 14;
                return tile;
        }

        tile.x = 0;
        tile.y = 14;

        return tile;
    }
}