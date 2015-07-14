﻿using UnityEngine;
using System.Collections;

public class LevelStart6 : MonoBehaviour
{

    public GameObject mapObject;
    void Awake()
    {
        mapObject.GetComponent<MapController>().enabled = false;

        GlobalController.initialMap = new int[4, 4] { { -1, 0, -1, -1 }, { 0, 0, 0, 0 }, { 0, -1, -1, 0 }, { 0, 0, 3, 4 } };
        GlobalController.map = new int[4, 4];
        GlobalController.maxHeight = 4f;
        GlobalController.startx = 0;
        GlobalController.starty = 1;
        GlobalController.endx = 3;
        GlobalController.endy = 3;
        GlobalController.vineBlock = new int[4, 4] { { 0, 0, 0, 0 }, { 0, 0, 0, 0 }, { 0, 0, 0, 0 }, { 0, 0, 0, 0 } };
        GlobalController.woodenBlock = new int[4, 4] { { 0, 1, 0, 0 }, { 0, 0, 0, 0 }, { 0, 0, 0, 0 }, { 0, 0, 1, 1 } };
        GlobalController.blackBlock = new int[4, 4] { { 0, 0, 0, 0 }, { 0, 0, 0, 0 }, { 0, 0, 0, 0 }, { 0, 0, 0, 0 } };
        GlobalController.goldBlock = new int[4, 4] { { 0, 0, 0, 0 }, { 0, 0, 0, 0 }, { 0, 0, 0, 0 }, { 0, 0, 0, 0 } };
        GlobalController.jumpBlock = new int[4, 4] { { 0, 0, 0, 0 }, { 0, 0, 0, 0 }, { 0, 0, 0, 1 }, { 0, 1, 0, 0 } };

        mapObject.GetComponent<MapController>().enabled = true;

    }

}
