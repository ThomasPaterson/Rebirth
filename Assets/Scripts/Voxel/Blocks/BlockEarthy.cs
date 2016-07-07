using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class BlockEarthy : Block
{

    public BlockEarthy()
        : base()
    {

    }

    public override Tile TexturePosition(Direction direction)
    {
        Tile tile = new Tile();

        switch (direction)
        {
            case Direction.up:
                tile.x = 10;
                tile.y = 15;

                return tile;
        }

        tile.x = 10;
        tile.y = 14;

        return tile;
    }
}