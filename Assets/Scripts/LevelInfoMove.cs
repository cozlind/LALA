using UnityEngine;
using System.Collections;

public class LevelInfoMove : MonoBehaviour
{
    void Awake()
    {
        GlobalController.startx = 1;
        GlobalController.starty = 3;
        GlobalController.startz = 0;
        GlobalController.maxx = 8;
        GlobalController.maxy = 6;
        GlobalController.maxz = 5;
    }
}
