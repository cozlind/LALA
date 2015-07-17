using UnityEngine;
using System.Collections;

public class LevelStart3 : MonoBehaviour {

    public GameObject mapObject;
    void Awake()
    {
        mapObject.GetComponent<DriveController>().enabled = false;

        //GlobalController.initialMap = new int[4,4] { { 0, 0, 0, -1 }, { 1, 0, 2, 0 }, { 0, 0, 0, 0 }, { -1, 2, 0, 4 } };
        //GlobalController.map = new int[4, 4];
        //GlobalController.maxy = 4f;
        GlobalController.startx = 0;
        GlobalController.starty = 0;
        GlobalController.endx = 3;
        GlobalController.endy = 3;
        //GlobalController.vineBlock = new int[4, 4] { { 0, 0, 1, 0 }, { 0, 0, 0, 0 }, { 0, 0, 0, 0 }, { 0, 0, 0, 0 } };
        //GlobalController.woodenBlock = new int[4, 4] { { 1, 0, 0, 0 }, { 1, 0, 1, 0 }, { 0, 0, 0, 0 }, { 0,1, 0, 1 } };
        //GlobalController.blackBlock = new int[4, 4] { { 0, 0, 0, 0 }, { 0, 0, 0, 0 }, { 0, 0, 0, 0 }, { 0, 0, 0, 0 } };
        //GlobalController.goldBlock = new int[4, 4] { { 0, 0, 0, 0 }, { 0, 0, 0, 0 }, { 0, 0, 0, 0 }, { 0, 0, 0, 0 } };
        //GlobalController.jumpBlock = new int[4, 4] { { 0, 0, 0, 0 }, { 0, 0, 0, 0 }, { 0, 0, 0, 1 }, { 0, 0, 0, 0 } };

        mapObject.GetComponent<DriveController>().enabled = true;

    }

}
