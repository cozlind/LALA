using UnityEngine;
using System.Collections;

public class LevelInfo1 : MonoBehaviour
{
    void Awake()
    {
        GlobalController.maxx = 3;
        GlobalController.maxy = 4;
        GlobalController.maxz = 1;
    }
}
