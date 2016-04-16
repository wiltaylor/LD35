using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using Mono.Cecil;
using Random = UnityEngine.Random;

public class LevelLoader : MonoBehaviour
{
    public int MaxRoomAxis = 20;
    public GameObject LevelBlockPrefab;
    public TilePrefabs Prefabs;
    public TileSetManager TileSet;
    public int RoomWidth = 5;
    public float TileWidth = 0.32f;
    public int CurrentLevel = 0;
    public GameObject PlayerPrefab;

    private List<GameObject> _placedBlocks = new List<GameObject>();
    private Vector3 _entryStartPosition = Vector3.zero;
    private Vector3 _exitStartPosition = Vector3.zero;

    private enum RoomSpecial
    {
        None,
        Entry,
        Exit
    }

    private struct BlockInfo
    {
        public bool RoomSet;
        public bool Up;
        public bool Down;
        public bool Left;
        public bool Right;
        public RoomSpecial Special;
    }

    public void Start()
    {
        GoDownLevel();
    }

    public void GoUpLevel()
    {
        if (CurrentLevel == 0)
            return;

        CurrentLevel--;

        CreateLevel(CurrentLevel);
        SpawnPlayer(_exitStartPosition);
    }

    public void GoDownLevel()
    {
        CurrentLevel++;

        CreateLevel(CurrentLevel);
        SpawnPlayer(_entryStartPosition);
    }

    private void SpawnPlayer(Vector3 position)
    {
        var player = Instantiate(PlayerPrefab);
        player.transform.position = position;
        player.SetActive(true);
    }

    public void DestroyLevel()
    {
        foreach(var block in _placedBlocks)
            DestroyObject(block);
    }

    public void CreateLevel(int level)
    {
        var levelinfo = GetComponentsInChildren<LevelInfo>();

        foreach (var l in levelinfo)
        {
            if (l.Level == level)
            {
                if (l.Type == LevelInfo.LevelType.Random)
                {
                    var blocks = 0;
                    while (blocks < l.MinBlocks)
                    {
                        DestroyLevel();
                        RandomLevel(l);
                    }
                    
                    return;
                }

                if (l.Type == LevelInfo.LevelType.Static)
                {
                    //todo
                }
            }
        }

        CreateLevel(1);
    }

    private int RandomLevel(LevelInfo level)
    {
        var levelmatrix = new BlockInfo[MaxRoomAxis, MaxRoomAxis];
        var currentx = Random.Range(0, MaxRoomAxis);
        var currenty = Random.Range(0, MaxRoomAxis);

        var blocksleft = level.Blocks;

        var placedblocks = 0;

        while (blocksleft > 0)
        {
            blocksleft--;
            
            var up = Random.Range(0, 2);
            var down = Random.Range(0, 2);
            var left = Random.Range(0, 2);
            var right = Random.Range(0, 2);

            if (currentx != 0 && levelmatrix[currentx - 1, currenty].RoomSet)
                left = levelmatrix[currentx - 1, currenty].Right ? 1 : 0;

            if(currentx != MaxRoomAxis - 1 && levelmatrix[currentx + 1, currenty].RoomSet)
                right = levelmatrix[currentx + 1, currenty].Left ? 1 : 0;

            if (currenty != 0 && levelmatrix[currentx, currenty - 1].RoomSet)
                down = levelmatrix[currentx, currenty - 1].Up ? 1 : 0;

            if (currenty != MaxRoomAxis - 1 && levelmatrix[currentx, currenty + 1].RoomSet)
                up = levelmatrix[currentx, currenty + 1].Down ? 1 : 0;

            if (up == 0 && down == 0 && left == 0 & right == 0)
            {
                blocksleft++;
                continue;
            }

            levelmatrix[currentx, currenty].RoomSet = true;
            levelmatrix[currentx, currenty].Special = RoomSpecial.None;
            levelmatrix[currentx, currenty].Up = (up == 1);
            levelmatrix[currentx, currenty].Down = (down == 1);
            levelmatrix[currentx, currenty].Left = (left == 1);
            levelmatrix[currentx, currenty].Right = (right == 1);

            var found = false;

            for (var x = 0; x < MaxRoomAxis; x++)
            {
                if (found)
                    break;

                for (var y = 0; y < MaxRoomAxis; y++)
                {
                    if(levelmatrix[x,y].RoomSet)
                        continue;

                    if (x != 0 && levelmatrix[x - 1, y].RoomSet && levelmatrix[x - 1, y].Right)
                    {
                        currentx = x;
                        currenty = y;
                        found = true;
                        break;
                    }

                    if (x != MaxRoomAxis - 1 && levelmatrix[x + 1, y].RoomSet && levelmatrix[x + 1, y].Left)
                    {
                        currentx = x;
                        currenty = y;
                        found = true;
                        break;
                    }

                    if (y != 0 && levelmatrix[x, y - 1].RoomSet && levelmatrix[x, y - 1].Up)
                    {
                        currentx = x;
                        currenty = y;
                        found = true;
                        break;
                    }

                    if (y != MaxRoomAxis - 1 && levelmatrix[x, y + 1].RoomSet && levelmatrix[x, y + 1].Down)
                    {
                        currentx = x;
                        currenty = y;
                        found = true;
                        break;
                    }
                }
            }

            if (!found)
                break;

            placedblocks++;
        }

        //Clean up level
                for (var x = 0; x < MaxRoomAxis; x++)
        {
            for (var y = 0; y < MaxRoomAxis; y++)
            {
                if(x == 0)
                    levelmatrix[x, y].Left = false;

                if (x != 0 && levelmatrix[x - 1, y].RoomSet && levelmatrix[x - 1, y].Right)
                    levelmatrix[x, y].Left = true;

                if (x != 0 && !levelmatrix[x - 1, y].RoomSet)
                    levelmatrix[x, y].Left = false;

                if(x == MaxRoomAxis - 1)
                    levelmatrix[x, y].Right = false;

                if (x != MaxRoomAxis - 1 && levelmatrix[x + 1, y].RoomSet && levelmatrix[x + 1, y].Left)
                    levelmatrix[x, y].Right = true;

                if (x != MaxRoomAxis - 1 && !levelmatrix[x + 1, y].RoomSet)
                    levelmatrix[x, y].Right = false;

                if(y == 0)
                    levelmatrix[x, y].Down = false;

                if (y != 0 && levelmatrix[x, y - 1].RoomSet && levelmatrix[x, y - 1].Up)
                    levelmatrix[x, y].Down = true;

                if (y != 0 && !levelmatrix[x, y - 1].RoomSet)
                    levelmatrix[x, y].Down = false;

                if(y == MaxRoomAxis - 1)
                    levelmatrix[x, y].Up = false;

                if (y != MaxRoomAxis - 1 && levelmatrix[x, y + 1].RoomSet && levelmatrix[x, y + 1].Down)
                    levelmatrix[x, y].Up = true;

                if (y != MaxRoomAxis - 1 && !levelmatrix[x, y + 1].RoomSet)
                    levelmatrix[x, y].Up = false;
            }
        }

        for (var x = 0; x < MaxRoomAxis; x++)
        {
            for (var y = 0; y < MaxRoomAxis; y++)
            {
                if (!levelmatrix[x, y].RoomSet) continue;

                if (!levelmatrix[x, y].Up && !levelmatrix[x, y].Down && !levelmatrix[x, y].Left && !levelmatrix[x, y].Right)
                    levelmatrix[x, y].RoomSet = false;
            }
        }

        var roomtype = RoomSpecial.Entry;

        while (true)
        {
            var x = Random.Range(0, MaxRoomAxis);
            var y = Random.Range(0, MaxRoomAxis);

            if (!levelmatrix[x, y].RoomSet || levelmatrix[x, y].Special != RoomSpecial.None) continue;

            levelmatrix[x, y].Special = roomtype;

            if (roomtype == RoomSpecial.Entry)
                roomtype = RoomSpecial.Exit;
            else
                break;
        }


        for (var x = 0; x < MaxRoomAxis; x++)
        {
            for (var y = 0; y < MaxRoomAxis; y++)
            {
                if (levelmatrix[x, y].RoomSet)
                {
                    var block = (GameObject)Instantiate(LevelBlockPrefab,
                        new Vector3(x*(RoomWidth*TileWidth), y*(RoomWidth*TileWidth), 0), Quaternion.identity);

                    _placedBlocks.Add(block);

                    block.SetActive(true);
                    block.transform.name = "Block[" + x + "," + y + "]";

                    var controller = block.GetComponent<LevelBlockController>();

                    controller.Down = !levelmatrix[x, y].Down;
                    controller.Up = !levelmatrix[x, y].Up;
                    controller.Left = !levelmatrix[x, y].Left;
                    controller.Right = !levelmatrix[x, y].Right;
                    controller.Prefabs = Prefabs;
                    controller.TileSet = TileSet;

                    controller.GenerateBlock();

                    if (levelmatrix[x, y].Special == RoomSpecial.Entry)
                    {
                        var special = Instantiate(Prefabs.StairsUp);
                        special.transform.SetParent(controller.transform);
                        special.transform.localPosition = new Vector3(TileWidth * RoomWidth /2 , TileWidth * RoomWidth / 2, 0);
                        special.SetActive(true);
                        _entryStartPosition = special.transform.position;
                    }

                    if (levelmatrix[x, y].Special == RoomSpecial.Exit)
                    {
                        var special = Instantiate(Prefabs.StairsDown);
                        special.transform.SetParent(controller.transform);
                        special.transform.localPosition = new Vector3(TileWidth * RoomWidth / 2, TileWidth * RoomWidth / 2, 0);
                        special.SetActive(true);
                        _exitStartPosition = special.transform.position;
                    }

                }
            }
        }

        return placedblocks;
    }
}

