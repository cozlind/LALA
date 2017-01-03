using UnityEngine;
using System.Collections;

public class UpdateMap : MonoBehaviour {

    public int x;
    public int y;
    public int z;

    void Update()
    {
        updateToMap();
    }
    void updateToMap()
    {
        x = (int)transform.position.x;
        y = (int)transform.position.y;
        z = (int)transform.position.z;

        if (y + 1 < TestController.maxy)
        {
            if (TestController.map[x, y + 1, z] != 0)
            {
                TestController.map[x, y, z] = -1;
            }
            else
            {
                TestController.map[x, y, z] = 1;
            }
        }
        else
        {
            TestController.map[x, y, z] = 1;
        }
    }
}
