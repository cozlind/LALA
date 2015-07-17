using UnityEngine;
using System.Collections;

public class LevelInfo1 : MonoBehaviour {
    void Awake()
    {
        GlobalController.startx = 0;
        GlobalController.starty = 0;
        GlobalController.startz = 0;
        GlobalController.endx = 2;
        GlobalController.endy = 3;
        GlobalController.endz = 2;
        GlobalController.maxx = 3;
        GlobalController.maxy = 5;
        GlobalController.maxz = 3;
    }
}
