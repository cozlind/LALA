using UnityEngine;
using System.Collections;

public class LevelInfoSuspend : MonoBehaviour
{
    void Awake()
    {
        GlobalController.startx = 0;
        GlobalController.starty = 1;
        GlobalController.startz = 0;
        GlobalController.maxx = 5;
        GlobalController.maxy = 6;
        GlobalController.maxz = 5;
    }
}
