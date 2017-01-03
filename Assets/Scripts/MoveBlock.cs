using UnityEngine;
using System.Collections;

public class MoveBlock : MonoBehaviour
{
    public int x;
    public int y;
    public int z;
    public string type = "Move";
    public enum DIRECTION { PX, NX, PY, NY, PZ, NZ };
    public DIRECTION direction;
    public int startx;
    public int starty;
    public int startz;
    public int endx;
    public int endy;
    public int endz;
    public enum BLOCK { START, MOVING, END };
    public BLOCK block;
    public static GameObject actor;
    public float moveSpeed = 1f;
    private Vector3 moveDir;
    void updateMap()
    {
        updateToMap(x, y, z);

        int endP = 0;
        string aStarDir = "";
        switch (direction)
        {
            case DIRECTION.PY:
                aStarDir = "+y";
                endP = endy;
                moveDir = Vector3.up;
                break;
            case DIRECTION.NY:
                aStarDir = "-y";
                endP = endy;
                moveDir = Vector3.down;
                break;
            case DIRECTION.PX:
                aStarDir = "+x";
                endP = endx;
                moveDir = Vector3.right;
                break;
            case DIRECTION.NX:
                aStarDir = "-x";
                endP = endx;
                moveDir = Vector3.left;
                break;
            case DIRECTION.PZ:
                aStarDir = "+z";
                endP = endz;
                moveDir = Vector3.forward;
                break;
            case DIRECTION.NZ:
                aStarDir = "-z";
                endP = endz;
                moveDir = Vector3.back;
                break;
        }
        GlobalController.typeMap[x, y, z] = type + ":" + endP + ":" + aStarDir;
    }
    void updateToMap(int x, int y, int z)
    {
        GlobalController.map[x, y, z] = 1;
    }
    void Awake()
    {
        startx = x = System.Convert.ToInt32(transform.position.x);
        starty = y = System.Convert.ToInt32(transform.position.y);
        startz = z = System.Convert.ToInt32(transform.position.z);
        actor = GameObject.Find("Actor");
    }
    public void startMove()
    {
        if (block == BLOCK.START)
        {
            block = BLOCK.MOVING;
        }
    }
    void endMove()
    {
        if (block == BLOCK.MOVING)
        {
            transform.position = new Vector3(endx, endy, endz);
            block = BLOCK.END;
            x = endx;
            y = endy;
            z = endz;
        }
    }
    void updateMoveDir()
    {
        switch (direction)
        {
            case DIRECTION.PY:
                moveDir = Vector3.up;
                break;
            case DIRECTION.NY:
                moveDir = Vector3.down;
                break;
            case DIRECTION.PX:
                moveDir = Vector3.right;
                break;
            case DIRECTION.NX:
                moveDir = Vector3.left;
                break;
            case DIRECTION.PZ:
                moveDir = Vector3.forward;
                break;
            case DIRECTION.NZ:
                moveDir = Vector3.back;
                break;
        }
    }
    void Update()
    {
        if (block == BLOCK.MOVING)
        {
            updateMoveDir();
            actor.transform.position = transform.position + new Vector3(0, 0.8f, 0);
            transform.Translate(Time.deltaTime * moveSpeed * moveDir, Space.World);
            if (Vector3.Distance(transform.position, new Vector3(startx, starty, startz)) >= Vector3.Distance(new Vector3(startx, starty, startz), new Vector3(endx, endy, endz)))
            {
                endMove();
            }
        }
    }
}
