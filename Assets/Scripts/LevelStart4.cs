using UnityEngine;
using System.Collections;

public class LevelStart4 : MonoBehaviour {

    public GameObject mapObject;
    void Awake()
    {
        mapObject.GetComponent<MapController>().enabled = false;

        GlobalController.initialMap = new int[3,3] { { 0, 0, 0 }, { 0, 0, 0 }, { 0, 0, 3 } };
        GlobalController.map = new int[3, 3];
        GlobalController.maxHeight = 3f;
        GlobalController.startx = 0;
        GlobalController.starty = 0;
        GlobalController.endx =2;
        GlobalController.endy =2;
        GlobalController.vineBlock = new int[3, 3] { { 0, 0, 0 }, { 0, 0, 0 }, { 0, 0, 0 } };
        GlobalController.woodenBlock = new int[3, 3] { { 1, 0, 0 }, { 0, 0, 0 }, { 0, 0, 1 } };
        GlobalController.blackBlock = new int[3, 3] { { 0, 0, 0 }, { 0, 0, 0 }, { 0, 0, 0 } };
        GlobalController.goldBlock = new int[3, 3] { { 0, 0, 0 }, { 0, 0, 0 }, { 0, 0, 0 } };
        GlobalController.jumpBlock = new int[3, 3] { { 0, 0, 0 }, { 0, 0, 0 }, { 0, 0, 0 } };

        mapObject.GetComponent<MapController>().enabled = true;

    }

}
