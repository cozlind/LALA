using UnityEngine;
using System.Collections;

public class LevelStart5 : MonoBehaviour
{

    public GameObject mapObject;
    void Awake()
    {
        mapObject.GetComponent<DriveController>().enabled = false;

        //GlobalController.initialMap = new int[5, 5] { { -1, 0, 0, -1, -1 }, { 0, 5, 0, 0, 0 }, { 0, 0, 2, 3, 0 }, { 0, 0, 2, 0, 0 }, { -1, 0, 0, 0, 0 } };
        //GlobalController.map = new int[5, 5];
        //GlobalController.maxy = 5f;
        GlobalController.startx = 1;
        GlobalController.starty = 1;
        GlobalController.endx = 4;
        GlobalController.endy = 4;
        //GlobalController.vineBlock = new int[5, 5] { { 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0 } };
        //GlobalController.woodenBlock = new int[5, 5] { { 0, 0, 0, 0, 0 }, { 0, 1, 0, 0, 0 }, { 0, 0, 1, 1, 0 }, { 0, 0, 1, 0, 0 }, { 0, 0, 0, 0, 1 } };
        //GlobalController.blackBlock = new int[5, 5] { { 0, 0, 0, 0, 0 }, { 1, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0 } };
        //GlobalController.goldBlock = new int[5, 5] { { 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0 }, { 0, 0, 0, 1, 0 }, { 0, 1, 0, 0, 0 } };
        //GlobalController.jumpBlock = new int[5, 5] { { 0, 0, 0, 0, 0 }, { 0, 0, 1, 0, 0 }, { 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0 } };

        mapObject.GetComponent<DriveController>().enabled = true;

    }

}
