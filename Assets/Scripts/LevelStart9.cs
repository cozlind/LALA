using UnityEngine;
using System.Collections;

public class LevelStart9 : MonoBehaviour
{

    public GameObject mapObject;
    void Awake()
    {
        mapObject.GetComponent<MapController>().enabled = false;

        GlobalController.initialMap = new int[6, 6] { { 0, 0, -1, 0, 0, 0 }, { 0, 0, 1, 0, 0, 0 }, { 0, 0, 0, 2, 0, 0 }, { 0, 2, 0, 0, 3, 4 }, { 3, 0, 0, 0, 0, 3 }, { 0, 3, 0, 0, 0, 0 } };
        GlobalController.map = new int[6, 6];
        GlobalController.maxHeight = 4f;
        GlobalController.startx = 0;
        GlobalController.starty = 0;
        GlobalController.endx = 3;
        GlobalController.endy = 5;
        GlobalController.vineBlock = new int[6, 6] { { 0, 0, 0, 0, 0, 0 }, { 0, 1, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 1, 0, 0 }, { 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0 } };
        GlobalController.woodenBlock = new int[6, 6] { { 1, 0, 0, 0, 0, 0 }, { 0, 0, 1, 0, 0, 0 }, { 0, 0, 0, 1, 0, 0 }, { 0, 1, 0, 0, 1, 1 }, { 1, 0, 0, 0, 0, 1 }, { 0, 1, 0, 0, 0, 0 } };
        GlobalController.blackBlock = new int[6, 6] { { 0, 0, 0, 1, 0, 0 }, { 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 1 }, { 1, 0, 0, 0, 0, 0 }, { 0, 0, 1, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0 } };
        GlobalController.goldBlock = new int[6, 6] { { 0, 0, 0, 0, 0, 1 }, { 0, 0, 0, 1, 0, 0 }, { 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0 }, { 1, 0, 1, 0, 0, 0 } };
        GlobalController.jumpBlock = new int[6, 6] { { 0, 0, 0, 0, 1, 0 }, { 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0 }, { 0, 0, 1, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 1 } };

        mapObject.GetComponent<MapController>().enabled = true;

    }

}
