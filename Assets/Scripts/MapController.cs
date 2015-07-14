using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
public class MapController : MonoBehaviour
{
    public GameObject actor;
    public static int[,] map;
    private AStarFinder asf;
    public static int startx;
    public static int starty;
    private Transform tPos;
    public static bool isPlay;
    private List<AStarFinder.Node> path;
    private int prex;
    private int prey;
    private int index;
    public AudioSource[] asCube;
    private float playRun = 0.0f;
    private bool isRun = false;
    void Start()
    {
        path = new List<AStarFinder.Node>();
        startx = GlobalController.startx;
        starty = GlobalController.starty;
        map = GlobalController.map;
        asf = new AStarFinder();
        tPos = GameObject.Find("tragetPos").transform;
        isPlay = false;
        index = 1;
    }
    public void clearPath()
    {
        path = new List<AStarFinder.Node>();
    }
    void Update()
    {
        //输出路线
        //if (path != null)
        //{
        //    string str = "";
        //    foreach (var item in path)
        //    {
        //        str += (item.x + "" + item.y);
        //    }
        //    Debug.Log(str);
        //}
        //else { Debug.Log("null"); }
        if (isPlay)
        {
            if (asCube[1].isPlaying)
            {
                asCube[1].Stop();
            }
            isRun = false;
            active();
            isPlay = false;
        }
        if (isRun && !asCube[1].isPlaying)
        {
            playRun += Time.deltaTime;
            if (playRun > 0.5f)
            {
                asCube[1].Play();
                playRun = 0.0f;
            }
        }
    }
    public void active()
    {
        do
        {
            if (index >= path.Count)
            {
                GameObject.Find("GlobalController").GetComponent<GlobalController>().end();
                return;
            }
            AStarFinder.Node node = path[index++];
            if (node.x == prex && node.y == prey)//始发点
            {
                continue;
            }
            else
            {
                //响应藤蔓方块
                if (GlobalController.vineBlock[prex, prey] > 0)
                {
                    Camera.main.GetComponent<CameraController>().shake();
                    clearPath();
                    break;
                }
                if (GlobalController.blackBlock[prex, prey] > 0)
                {
                    EnergyController.unrecover(prex, prey);
                }
                if (GlobalController.goldBlock[prex, prey] > 0)
                {
                    EnergyController.reward(prex, prey);
                }
                if (node.x == prex + 1 && node.y == prey
                    || node.x == prex && node.y == prey + 1
                    || node.x == prex && node.y == prey - 1
                    || node.x == prex - 1 && node.y == prey)
                {
                    int option = 1;
                    if (Math.Abs(map[prex, prey] - map[node.x, node.y]) == 2)
                    {
                        option = 2;
                    }
                    tPos.position = getV3(node.x, node.y);
                    blockJump(actor.transform, getV3(node.x, node.y), option);
                    //BlockActive.blockJump(getV3(prex, prey), getV3(node.x, node.y),option);
                    prex = node.x;
                    prey = node.y;
                    continue;
                }
            }

        } while (false);
    }
    public void play()
    {
        if (GlobalController.isPlayBtnActive)
        {
            prex = startx;
            prey = starty;
            asf = new AStarFinder();
            path = asf.find(map, startx, starty, GlobalController.endx, GlobalController.endy);
            if (path == null)
            {
                Camera.main.GetComponent<CameraController>().shake();
                return;
            }
            else
            {
                isPlay = true;
                GlobalController.isPlayBtnActive = false;
                GlobalController.isInteract = false;
            }
        }
    }
    public bool blockJump(Transform p, Vector3 t, int m = 1)
    {
        Vector3 traget = t;
        Vector3 player = p.transform.position;
        float x = traget.x - player.x;
        float y = traget.y - player.y;
        float z = traget.z - player.z;
        float fx = 0.0f;
        float fy = 0.0f;
        float fz = 0.0f;
        if (x > 0.25f)
        {
            fx = 1.7f;
        }
        else if (x < -0.25f)
        {
            fx = -1.7f;
        }

        if (y > 0.25f)
        {
            if (m == 1)
            {
                fy = 12.0f;
            }
            else if (m == 2)
            {
                fy = 15.0f;
            }
        }
        else if (y < -0.25f)
        {
            //fy = -12.0f;
        }
        if (z > 0.25f)
        {
            fz = 1.7f;
        }
        else if (z < -0.25f)
        {
            fz = -1.7f;
        }
        p.gameObject.GetComponent<Rigidbody>().isKinematic = false;
        p.gameObject.GetComponent<Rigidbody>().AddForce(new Vector3(fx, fy, fz), ForceMode.Impulse);
        if (asCube[0].isPlaying)
        {
            asCube[0].Stop();
        }

        if (fy > 0.0f)
        {
            asCube[0].Play();
        }
        else if (!asCube[1].isPlaying)
        {
            isRun = true;
        }
        return true;
    }
    //public Vector3 getV3(int map_i, int map_j)
    //{
    //    int height = map[map_i, map_j];
    //    float x = -1, y = 0, z = -1;
    //    switch (map_j)
    //    {
    //        case 0: x = -1; break;
    //        case 1: x = 0; break;
    //        case 2: x = 1; break;
    //    }
    //    switch (map_i)
    //    {
    //        case 0: z = -1; break;
    //        case 1: z = 0; break;
    //        case 2: z = 1; break;
    //    }
    //    y = -height*0.05f +height+ 0.8f;
    //    return new Vector3(x, y, z);
    //}
    public Vector3 getV3(int map_i, int map_j)
    {
        int map_h = map[map_i, map_j];

        float x = map_i;
        float z = -map_j;
        float y = map_h + 0.8f;

        return new Vector3(x, y, z);
    }
}
