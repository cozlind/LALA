using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class TestController : MonoBehaviour
{

    public static int[, ,] map;
    public static int startx;
    public static int starty;
    public static int startz;
    public static int endx;
    public static int endy;
    public static int endz;
    public static int maxx;
    public static int maxy;
    public static int maxz;
    public static List<SpaceAStarFinder.Node> path;

    public static bool isClearMap = false;
    public GameObject mPrefab;

    public void play()
    {
        SpaceAStarFinder asf = new SpaceAStarFinder();
        path = asf.find(map, startx, starty, startz, endx, endy, endz);
        if (path == null)
        {
            Debug.Log("Path:null");
        }
        else
        {
            String str = "";
            foreach (var item in path)
            {
                str += "(" + item.x + "," + item.y + "," + item.z + ") ";
                GameObject objPrefab = Instantiate(mPrefab, Vector3.zero, Quaternion.identity) as GameObject;
                objPrefab.transform.position=new Vector3(item.x,item.y,item.z);
            }
            Debug.Log("Path:" + str);
        }
        return;
    }
    public void getMap()
    {
        for (int j = 0; j < map.GetLength(1); j++)
        {
            string line = "";
            for (int k = map.GetLength(2) - 1; k >= 0; k--)
            {
                for (int i = 0; i < map.GetLength(0); i++)
                {
                    line += map[i, j, k];
                }
                line += "\n";
            }
            Debug.Log(line);
        }
    }
    public void clearMap()
    {
        map = new int[maxx, maxy, maxz];
    }
    void Update()
    {
        if (isClearMap)
        {
            clearMap();
            isClearMap = false;
        }
    }
}
