using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class GlobalController : MonoBehaviour
{
    public static bool isInteract;
    public static bool isUpdateMap;
    public static bool isNormalLook;
    private static bool isInit;

    public GameObject emptyFillCube;
    private static GameObject Map;
    //地图信息
    public static int[, ,] map;
    public static string[, ,] typeMap;
    public static List<List<List<GameObject>>> rows;
    public static int startx;
    public static int starty;
    public static int startz;
    public static int endx;
    public static int endy;
    public static int endz;
    public static int maxx;
    public static int maxy;
    public static int maxz;
    //路径信息
    public static int prex;
    public static int prey;
    public static int prez;
    public static List<SpaceAStarFinder.Node> path;
    public GameObject mPrefab;
    public GameObject shellPrefab;
    void Awake()
    {
        Map = GameObject.Find("Map");
    }
    void Start()
    {
        clearPath();

        isNormalLook = true;
        isInteract = true;
        isInit = true;
    }
    void Update()
    {
        if (isInit)
        {
            resetMap();
            initRows();
            isInit = false;
        }
        if (isUpdateMap)
        {
            resetMap();
            isUpdateMap = false;
            //GlobalController.printMap();
            GlobalController.printTypeMap();
        }
    }
    #region 动态模式DynamicBlock
    public static GameObject getMoveBlock(int x, int y, int z)
    {
        GameObject[] dynamicBlocks = GameObject.FindGameObjectsWithTag("DynamicBlock");
        foreach (var block in dynamicBlocks)
        {
            if (block.GetComponent<MoveBlock>() != null)
            {
                int movex = block.GetComponent<MoveBlock>().x;
                int movey = block.GetComponent<MoveBlock>().y;
                int movez = block.GetComponent<MoveBlock>().z;
                if (x == movex && y == movey && z == movez)
                {
                    return block;
                }
            }
        }
        return null;
    }
    #endregion
    #region 隧道模式TubeBlock
    public static void initRows()
    {
        rows = new List<List<List<GameObject>>>();
        for (int i = 0; i < maxx; i++)
        {
            rows.Add(new List<List<GameObject>>());
            for (int j = 0; j < maxy; j++)
            {
                rows[i].Add(new List<GameObject>());
            }
        }
        GameObject[] blocks = GameObject.FindGameObjectsWithTag("TubeBlock");
        foreach (var block in blocks)
        {
            rows[System.Convert.ToInt32(block.transform.position.x)][System.Convert.ToInt32(block.transform.position.y)].Add(block);
        }
    }
    public static List<GameObject> getRow(int x, int y)
    {
        return rows[x][y];
    }
    //public static void printRow(int x, int y,string head="")
    //{
    //    String str = head;
    //    foreach (var item in rows[x][y])
    //    {
    //        str += "(" + item.GetComponent<ExchangeBlock>().x + "," + item.GetComponent<ExchangeBlock>().y + "," + item.GetComponent<ExchangeBlock>().z + ") ";
    //    }
    //    Debug.Log(str);
    //}
    public static void exchangeRow(int x, int y, string mode)
    {
        List<GameObject> preRow = getRow(x, y);
        switch (mode)
        {
            case "Up":
                List<GameObject> upRow = getRow(x, y + 1);
                foreach (var rowItem in upRow)
                {
                    int z = System.Convert.ToInt32(rowItem.transform.position.z);
                    rowItem.transform.position = new Vector3(x, y, z);
                    rowItem.SendMessage("updatePosY", y, SendMessageOptions.DontRequireReceiver);
                }
                foreach (var rowItem in preRow)
                {
                    rowItem.SendMessage("updatePosY", y + 1, SendMessageOptions.DontRequireReceiver);
                }
                rows[x][y + 1] = preRow;
                rows[x][y] = upRow;
                break;
            case "Down":
                List<GameObject> downRow = getRow(x, y - 1);
                foreach (var rowItem in downRow)
                {
                    int z = System.Convert.ToInt32(rowItem.transform.position.z);
                    rowItem.transform.position = new Vector3(x, y, z);
                    rowItem.SendMessage("updatePosY", y, SendMessageOptions.DontRequireReceiver);
                }
                foreach (var rowItem in preRow)
                {
                    rowItem.SendMessage("updatePosY", y - 1, SendMessageOptions.DontRequireReceiver);
                }
                rows[x][y - 1] = preRow;
                rows[x][y] = downRow;
                break;
        }
    }
    public void xray()
    {
        GameObject[] blocks = GameObject.FindGameObjectsWithTag("TubeBlock");
        if (isNormalLook)
        {
            for (int i = 0; i < blocks.Length; i++)
            {
                GameObject block = blocks[i];
                if (block.GetComponent<Renderer>() != null)
                {
                    block.GetComponent<Renderer>().enabled = false;
                    block.GetComponent<ExchangeBlock>().shellBlock.GetComponent<Renderer>().enabled = true;
                }
                else
                {
                    bool isNoShell = true;
                    block.SendMessage("updateMap");
                    foreach (var childRenderer in block.GetComponentsInChildren<Renderer>())
                    {
                        if (childRenderer.gameObject.name.Equals("Shell"))
                        {
                            childRenderer.enabled = true;
                            isNoShell = false;
                        }
                        else
                        {
                            childRenderer.enabled = false;
                        }
                    }
                    if (isNoShell)
                    {
                        GameObject shellBlock = Instantiate(shellPrefab, block.transform.position, Quaternion.identity) as GameObject;
                        shellBlock.name = "Shell";
                        shellBlock.GetComponent<Renderer>().enabled = true;
                        shellBlock.transform.SetParent(block.transform);
                    }
                }
            }
            for (int i = 0; i < maxx; i++)
            {
                for (int j = 0; j < maxy; j++)
                {
                    for (int k = 0; k < maxz; k++)
                    {
                        if (map[i, j, k] == 0)
                        {
                            GameObject objPrefab = Instantiate(emptyFillCube, new Vector3(i, j, k), Quaternion.identity) as GameObject;
                            objPrefab.tag = "Temp";
                        }
                    }
                }
            }
            isNormalLook = false;
        }
        else
        {
            for (int i = 0; i < blocks.Length; i++)
            {
                GameObject block = blocks[i];
                if (block.GetComponent<Renderer>() != null)
                {
                    block.GetComponent<Renderer>().enabled = true;
                    block.GetComponent<ExchangeBlock>().shellBlock.GetComponent<Renderer>().enabled = false;
                }
                else
                {
                    for (int j = 0; j < block.transform.childCount; j++)
                    {
                        GameObject child = block.transform.GetChild(j).gameObject;
                        if (child.name.Equals("Shell"))
                        {
                            child.GetComponent<Renderer>().enabled = false;
                        }
                        else if (child.GetComponent<Renderer>() != null)
                        {
                            child.GetComponent<Renderer>().enabled = true;
                        }
                    }
                }
            }
            GameObject[] tempObj = GameObject.FindGameObjectsWithTag("Temp");
            foreach (var tempItem in tempObj)
            {
                Destroy(tempItem);
            }
            isNormalLook = true;
        }
    }
    #endregion
    public static void clearMap()
    {
        map = new int[maxx, maxy, maxz];
        typeMap = new string[maxx, maxy, maxz];
    }
    public static void checkMap()
    {
        for (int j = maxy - 1; j >= 0; j--)
        {
            for (int i = 0; i < maxx; i++)
            {
                for (int k = 0; k < maxz; k++)
                {
                    if (typeMap[i, j, k] == null)
                    {
                        typeMap[i, j, k] = "0";
                    }
                    if (j > 0)
                    {
                        if (map[i, j, k] != 0 && map[i, j - 1, k] != 0)
                        {
                            map[i, j - 1, k] = -1;
                        }
                    }
                }
            }
        }
    }
    public static void resetMap()
    {
        clearMap();
        List<GameObject> blocks = new List<GameObject>();
        for (int i = 0; i < Map.transform.childCount; i++)
        {
            GameObject block = Map.transform.GetChild(i).gameObject;
            blocks.Add(block);
            block.SendMessage("updateMap");
        }
        checkMap();
    }
    public static void printMap()
    {
        for (int j = 0; j < map.GetLength(1); j++)
        {
            string line = "";
            for (int k = map.GetLength(2) - 1; k >= 0; k--)
            {
                for (int i = 0; i < map.GetLength(0); i++)
                {
                    line += map[i, j, k].ToString().PadLeft(2);
                }
                line += "\n";
            }
            Debug.Log(line);
        }
    }
    public static void printTypeMap()
    {
        for (int j = 0; j < typeMap.GetLength(1); j++)
        {
            string line = "";
            for (int k = typeMap.GetLength(2) - 1; k >= 0; k--)
            {
                for (int i = 0; i < typeMap.GetLength(0); i++)
                {
                    line += typeMap[i, j, k].ToString().PadLeft(9);
                }
                line += "\n";
            }
            Debug.Log(line);
        }
    }
    public static void clearPath()
    {
        path = new List<SpaceAStarFinder.Node>();
    }
    public void printPath()
    {
        String str = "";
        foreach (var item in path)
        {
            str += "(" + item.x + "," + item.y + "," + item.z + ") ";
            GameObject objPrefab = Instantiate(mPrefab, Vector3.zero, Quaternion.identity) as GameObject;
            objPrefab.transform.position = new Vector3(item.x, item.y, item.z);
        }
        Debug.Log("Path:" + str);
    }
    public void play()
    {
        if (UIController.isPlayBtnActive)
        {
            prex = startx;
            prey = starty;
            prez = startz;
            SpaceAStarFinder asf = new SpaceAStarFinder();
            path = asf.find(map, startx, starty, startz, endx, endy, endz);
            if (path == null)
            {
                Camera.main.GetComponent<CameraController>().shake();
                return;
            }
            else
            {
                DriveController.isPlay = true;
                UIController.isPlayBtnActive = false;
                isInteract = false;
                //printPath();
            }
        }
    }
}
