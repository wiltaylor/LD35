using UnityEngine;

public class LevelBlockController : MonoBehaviour
{
    public TilePrefabs Prefabs;
    public TileSetManager TileSet;
    public bool Up;
    public bool Left;
    public bool Right;
    public bool Down;
    public bool CornerBL;
    public bool CornerBR;
    public bool CornerTL;
    public bool CornerTR;


    public int BlocksPerAxis = 5;
    public float BlockUnits = 0.3f;

    public void GenerateBlock()
    {
        for (var x = 0; x < BlocksPerAxis; x++)
        {
            for (var y = 0; y < BlocksPerAxis; y++)
            {
                var tileType = Prefabs.EmptyTile;

                if (x == 0 && Left)
                    tileType = Prefabs.WallLeftTile;
                if (x == BlocksPerAxis - 1 && Right)
                    tileType = Prefabs.WallRightTile;
                if (y == BlocksPerAxis - 1 && Up)
                    tileType = Prefabs.WallUpTile;
                if (y == 0 && Down)
                    tileType = Prefabs.WallDownTile;
                if (y == 0 && Down && x == 0 && Left)
                    tileType = Prefabs.WallDownLeftTile;
                if (y == 0 && Down && x == BlocksPerAxis - 1 && Right)
                    tileType = Prefabs.WallDownRightTile;
                if (y == BlocksPerAxis - 1 && Up && x == 0 && Left)
                    tileType = Prefabs.WallUpLeftTile;
                if (y == BlocksPerAxis - 1 && Up && x == BlocksPerAxis - 1 && Right)
                    tileType = Prefabs.WallUpRightTile;

                if(x == 0 && y == 0 && CornerBL)
                    tileType = Prefabs.WallCornerDownLeft;

                if(x == 0 & y == BlocksPerAxis - 1 && CornerTL)
                    tileType = Prefabs.WallCornerUpLeft;

                if(x == BlocksPerAxis - 1 && y == 0 && CornerBR)
                    tileType = Prefabs.WallCornerDownRight;

                if (x == BlocksPerAxis - 1 && y == BlocksPerAxis - 1 && CornerTR)
                    tileType = Prefabs.WallCornerUpRight;

                var obj = Instantiate(tileType);

                obj.transform.SetParent(gameObject.transform);
                obj.transform.localPosition = new Vector3((x*BlockUnits),
                    (y*BlockUnits), 0);

                obj.SetActive(true);

            }
        }
    }
}

