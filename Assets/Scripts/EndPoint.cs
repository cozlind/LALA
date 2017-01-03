using UnityEngine;
using System.Collections;

public class EndPoint : MonoBehaviour
{
    void Awake()
    {
        GlobalController.endx = System.Convert.ToInt32(transform.position.x);
        GlobalController.endy = (int)transform.position.y;
        GlobalController.endz = System.Convert.ToInt32(transform.position.z);
    }
}
