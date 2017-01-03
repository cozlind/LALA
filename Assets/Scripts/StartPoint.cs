using UnityEngine;
using System.Collections;

public class StartPoint : MonoBehaviour
{
    void Awake()
    {
        GlobalController.startx = System.Convert.ToInt32(transform.position.x);
        GlobalController.starty = (int)transform.position.y;
        GlobalController.startz = System.Convert.ToInt32(transform.position.z);
    }
}
