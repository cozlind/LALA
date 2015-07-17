using UnityEngine;
using System.Collections;

public class LevelInfoN : MonoBehaviour {
    void Awake()
    {
        GlobalController.startx = 0;
        GlobalController.starty = 1;
        GlobalController.startz = 0;
        GlobalController.endx = 4;
        GlobalController.endy = 4;
        GlobalController.endz = 4;
        GlobalController.maxx = 5;
        GlobalController.maxy = 6;
        GlobalController.maxz = 5;
    }
}
