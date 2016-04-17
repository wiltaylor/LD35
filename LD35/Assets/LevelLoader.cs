using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
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
    public bool GoingDown;

    public int MaxItemsPerBlock = 5;

    private List<GameObject> _placedBlocks = new List<GameObject>();
    private Vector3 _entryStartPosition = Vector3.zero;
    private Vector3 _exitStartPosition = Vector3.zero;
    private GameObject _playerObj;

    private bool _delayLoadPlayer;

    private struct BlockInfo
    {
        public bool RoomSet;
        public bool Up;
        public bool Down;
        public bool Left;
        public bool Right;
        public bool CornerUPLeft;
        public bool CornerUPRight;
        public bool CornerDownLeft;
        public bool CornerDownRight;
        public List<GameObject> ObjectToPlace;
    }

    public void OnLevelWasLoaded(int level)
    {
        CreateLevel(CurrentLevel);
    }

    public void Start()
    {
        GoingDown = true;
        CreateLevel(CurrentLevel);
    }

    private void SpawnPlayer(Vector3 position)
    {
        var player = Instantiate(PlayerPrefab);
        player.transform.position = new Vector3(position.x, position.y - TileWidth, 0);
        player.SetActive(true);
        _playerObj = player;
    }

    public void DestroyLevel()
    {
        DestroyObject(_playerObj);

        foreach (var block in _placedBlocks)
            DestroyObject(block);
    }

    public void Update()
    {
        if (!_delayLoadPlayer)
            return;

        var playerspawn = GameObject.FindGameObjectWithTag("PlayerSpawn");
        if (playerspawn == null) return;

        SpawnPlayer(playerspawn.transform.position);
        _delayLoadPlayer = false;
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
                    if (SceneManager.GetActiveScene().name != "Blank")
                    {
                        SceneManager.LoadScene("Blank");
                        return;
                    }

                    var blocks = 0;
                    while (blocks < l.MinBlocks)
                    {
                        DestroyLevel();
                        blocks = RandomLevel(l);
                    }

                    SpawnPlayer(GoingDown ? _entryStartPosition : _exitStartPosition);

                    return;
                }

                if (l.Type == LevelInfo.LevelType.Static)
                {
                    if (SceneManager.GetActiveScene().name != l.LevelFile)
                    {
                        SceneManager.LoadScene(l.LevelFile);
                        return;
                    }

                    _delayLoadPlayer = true;
                }
            }
        }
    }

    private int RandomLevel(LevelInfo level)
    {
        var levelmatrix = new BlockInfo[MaxRoomAxis, MaxRoomAxis];
        var currentx = Random.Range(0, MaxRoomAxis);
        var currenty = Random.Range(0, MaxRoomAxis);

        var placementQueue = new Queue<GameObject>();
        var itemslots = 0;

        var blocksleft = level.Blocks;

        var placedblocks = 0;

        placementQueue.Enqueue(Prefabs.StairsDown);
        placementQueue.Enqueue(Prefabs.StairsUp);

        for (var index = 0; index < level.EntityTypes.Length; index++)
        {
            var qty = Random.Range(level.MinEntityQty[index], level.MaxEntityQty[index]);

            for (var i = 0; i < qty; i++)
            {
                placementQueue.Enqueue(level.EntityTypes[index]);
            }
        }
        
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
            levelmatrix[currentx, currenty].ObjectToPlace = new List<GameObject>();
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

                if (!levelmatrix[x, y].RoomSet)
                    continue;

                if (levelmatrix[x, y].Down && levelmatrix[x, y].Left && x != 0 && !levelmatrix[x - 1, y].Down && y != 0 && !levelmatrix[x, y - 1].Left)
                    levelmatrix[x, y].CornerDownLeft = true;

                if (levelmatrix[x, y].Up && levelmatrix[x, y].Left && x != 0 && !levelmatrix[x - 1, y].Up && y != MaxRoomAxis - 1 && !levelmatrix[x, y + 1].Left)
                    levelmatrix[x, y].CornerUPLeft = true;

                if (levelmatrix[x, y].Down && levelmatrix[x, y].Right && x != MaxRoomAxis - 1 && !levelmatrix[x + 1, y].Down && y != 0 && !levelmatrix[x, y - 1].Right)
                    levelmatrix[x, y].CornerDownRight = true;

                if (levelmatrix[x, y].Up && levelmatrix[x, y].Right && x != MaxRoomAxis - 1 && !levelmatrix[x + 1, y].Up && y != MaxRoomAxis - 1 && !levelmatrix[x, y + 1].Right)
                    levelmatrix[x, y].CornerUPRight = true;
            }

        }

        itemslots = placedblocks * MaxItemsPerBlock;

        if (itemslots < placementQueue.Count)
            return -1;

        while (placementQueue.Count > 0)
        {
            var x = Random.Range(0, MaxRoomAxis);
            var y = Random.Range(0, MaxRoomAxis);

            if (!levelmatrix[x, y].RoomSet)
                continue;

            if(levelmatrix[x, y].ObjectToPlace.Count >= MaxItemsPerBlock)
                continue;
            
            if(placementQueue.Peek() == Prefabs.StairsDown && levelmatrix[x, y].ObjectToPlace.Count != 0)
                continue;

            if (placementQueue.Peek() == Prefabs.StairsUp && levelmatrix[x, y].ObjectToPlace.Count != 0)
                continue;


            levelmatrix[x, y].ObjectToPlace.Add(placementQueue.Dequeue());
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
                    controller.CornerBL = levelmatrix[x, y].CornerDownLeft;
                    controller.CornerBR = levelmatrix[x, y].CornerDownRight;
                    controller.CornerTL = levelmatrix[x, y].CornerUPLeft;
                    controller.CornerTR = levelmatrix[x, y].CornerUPRight;
                    controller.Prefabs = Prefabs;
                    controller.TileSet = TileSet;

                    controller.GenerateBlock();

                    //Middle object
                    if (levelmatrix[x, y].ObjectToPlace.Count >= 1)
                    {
                        var obj = Instantiate(levelmatrix[x, y].ObjectToPlace[0]);
                        obj.transform.SetParent(controller.transform);
                        obj.transform.localPosition = new Vector3(TileWidth * 2, TileWidth * 2, 0);
                        obj.SetActive(true);

                        if (levelmatrix[x, y].ObjectToPlace[0] == Prefabs.StairsDown)
                            _exitStartPosition = obj.transform.position;

                        if (levelmatrix[x, y].ObjectToPlace[0] == Prefabs.StairsUp)
                            _entryStartPosition = obj.transform.position;

                    }

                    if (levelmatrix[x, y].ObjectToPlace.Count >= 2)
                    {
                        var obj = Instantiate(levelmatrix[x, y].ObjectToPlace[1]);
                        obj.transform.SetParent(controller.transform);
                        obj.transform.localPosition = new Vector3(TileWidth * 1, TileWidth * 1, 0);
                        obj.SetActive(true);
                    }

                    if (levelmatrix[x, y].ObjectToPlace.Count >= 3)
                    {
                        var obj = Instantiate(levelmatrix[x, y].ObjectToPlace[2]);
                        obj.transform.SetParent(controller.transform);
                        obj.transform.localPosition = new Vector3(TileWidth * 3, TileWidth * 1, 0);
                        obj.SetActive(true);
                    }

                    if (levelmatrix[x, y].ObjectToPlace.Count >= 4)
                    {
                        var obj = Instantiate(levelmatrix[x, y].ObjectToPlace[3]);
                        obj.transform.SetParent(controller.transform);
                        obj.transform.localPosition = new Vector3(TileWidth * 3, TileWidth * 3, 0);
                        obj.SetActive(true);
                    }

                    if (levelmatrix[x, y].ObjectToPlace.Count >= 5)
                    {
                        var obj = Instantiate(levelmatrix[x, y].ObjectToPlace[3]);
                        obj.transform.SetParent(controller.transform);
                        obj.transform.localPosition = new Vector3(TileWidth * 1, TileWidth * 3, 0);
                        obj.SetActive(true);
                    }
                }
            }
        }

        return placedblocks;
    }
}

