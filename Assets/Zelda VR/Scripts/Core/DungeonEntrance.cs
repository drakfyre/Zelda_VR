﻿using UnityEngine;


public class DungeonEntrance : MonoBehaviour
{
    public EntranceBlock_Dungeon entranceBlock;


    public int DungeonNum
    {
        get { return entranceBlock.dungeon; }
        set { entranceBlock.dungeon = value; }
    }
}