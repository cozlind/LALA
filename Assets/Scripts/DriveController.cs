using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
public class DriveController : MonoBehaviour
{
    public static GameObject actor;
    private static int prex;
    private static int prey;
    private static int prez;

    private static Transform tPos;
    private static Vector3 zero = new Vector3(0, -10, 0);
    public static bool isPlay;
    private static int index;

    public static AudioSource[] asCube;
    private static float playRun = 0.0f;
    private static bool isRun = false;
    void Start()
    {
        tPos = GameObject.Find("TargetPos").transform;
        actor = GameObject.Find("Actor");
        asCube = new AudioSource[2];
        asCube[0] = GameObject.Find("AudioActorJump").GetComponent<AudioSource>();
        asCube[1] = GameObject.Find("AudioActorRun").GetComponent<AudioSource>();
        isPlay = false;
        index = 1;
    }

   void Update()
    {
        float v = Vector3.Distance(tPos.transform.position,actor.transform.position);
        if (v < 0.35f) {
            actor.transform.position =Vector3.Slerp(actor.transform.position, tPos.transform.position,0.2f);
            if (!actor.GetComponent<Rigidbody>().isKinematic)
                actor.GetComponent<Rigidbody>().isKinematic = true;
            if (v < 0.05f) {
                tPos.transform.position = zero;
                isPlay = true;
            }
        }
    
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
    public static void active()
    {
        do
        {
            if (index >= GlobalController.path.Count)
            {
                GameObject.Find("UI").GetComponent<UIController>().end();
                return;
            }
            SpaceAStarFinder.Node node = GlobalController.path[index++];
            if (node.x == prex && node.y == prey && node.z == prez)//始发点
            {
                continue;
            }
            else
            {
                if (GlobalController.typeMap[prex, prey, prez] == "Vine")
                {
                    Camera.main.GetComponent<CameraController>().shake();
                    GlobalController.clearPath();
                    break;
                }
                switch (GlobalController.typeMap[prex, prey, prez])
                {
                    case "Black":
                        EnergyController.unrecover(prex, prey, prez);
                        break;
                    case "Gold":
                        EnergyController.reward(prex, prey, prez);
                        break;
                }
                int option = 1;
                if (Math.Abs(prez - node.z) == 2)
                {
                    option = 2;
                }
                tPos.position = getV3(node.x, node.y, node.z);
                blockJump(actor.transform, getV3(node.x, node.y, node.z), option);
                prex = node.x;
                prey = node.y;
                prez = node.z;
                continue;
            }
        } while (false);
    }
    public static bool blockJump(Transform p, Vector3 t, int m = 1)
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
    public static Vector3 getV3(int x, int y, int z)
    {
        return new Vector3(x, y + 0.8f, z);
    }
}
