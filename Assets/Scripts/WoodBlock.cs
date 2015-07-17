using UnityEngine;
using System.Collections;

public class WoodBlock : MonoBehaviour {

    public int x;
    public int y;
    public int z;
    public string type;
    void updateMap()
    {
        updateToMap(x, y, z);
        GlobalController.typeMap[x, y, z] = type;
    }
    void updateToMap(int x,int y,int z)
    {
        GlobalController.map[x, y, z] = 1;
    }
    void Start()
    {
        x = System.Convert.ToInt32(transform.position.x);
        y = System.Convert.ToInt32(transform.position.y);
        z = System.Convert.ToInt32(transform.position.z);
        type = "Wood";
    }
}
