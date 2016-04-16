using UnityEngine;
using System.Collections;

public class LevelInfo : MonoBehaviour
{
    public enum LevelType
    {
        Random,
        Static
    }

    public int Level = -1;
    public string LevelFile;
    public string Name;
    public LevelType Type = LevelType.Random;
    public int Blocks = 0;
    public int MinBlocks = 0;
    public GameObject[] EntityTypes = {};
    public int[] MaxEntityQty = {};
    public int[] MinEntityQty = {};
    

}
