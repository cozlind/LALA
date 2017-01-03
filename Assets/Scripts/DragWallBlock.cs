using UnityEngine;
using System.Collections;

public class DragWallBlock : MonoBehaviour
{

    //基本结构
    public GameObject headWallBlock;
    public GameObject bodyWallBlock;
    public int maxh;
    public int x;
    public int y;
    public int z;
    public string type = "Wall";
    private Vector3 direction;
    public enum BLOCK { STATIC, DRAG, CONFIRM };
    public BLOCK block;
    //拖拽参数
    private float headBasicY;
    private float bodyBasicY;
    private int energyValue1 = 0;
    public float dragSpeedY = 0.2f;

    public AudioSource asMove;
    private bool isPlayerAs = false;
    private bool isFirstMove = false;
    private float upMoveY = 0.0f;
    void updateMap()
    {
        if (direction.x > 85 && direction.x < 105)
        {
            int headz = System.Convert.ToInt32(headWallBlock.transform.position.z);
            for (int i = headz; i >= z; i--)
            {
                updateToMap(x, y, i);
                GlobalController.typeMap[x, y, i] = "Base";
            }
            GlobalController.typeMap[x, y, headz] = type;
        }
        else if (direction.x > 255 && direction.x < 285)
        {
            int headz = System.Convert.ToInt32(headWallBlock.transform.position.z);
            for (int i = headz; i <= z; i++)
            {
                updateToMap(x, y, i);
                GlobalController.typeMap[x, y, i] = "Base";
            }
            GlobalController.typeMap[x, y, headz] = type;
        }
        if (direction.z > 85 && direction.z < 105)
        {
            int headx = System.Convert.ToInt32(headWallBlock.transform.position.x);
            for (int i = headx; i <= x; i++)
            {
                updateToMap(i, y, z);
                GlobalController.typeMap[i, y, z] = "Base";
            }
            GlobalController.typeMap[headx, y, z] = type;
        }
        else if (direction.z > 255 && direction.z < 285)
        {
            int headx = System.Convert.ToInt32(headWallBlock.transform.position.x);
            for (int i = headx; i >= x; i--)
            {
                updateToMap(i, y, z);
                GlobalController.typeMap[i, y, z] = "Base";
            }
            GlobalController.typeMap[headx, y, z] = type;
        }
    }
    void updateToMap(int x, int y, int z)
    {
        //Debug.Log("DragWallBlock updateToMap " + "x:" + x + " y:" + y + " z:" + z);
        GlobalController.map[x, y, z] = 1;
    }
    void Start()
    {
        x = System.Convert.ToInt32(transform.localPosition.x);
        y = System.Convert.ToInt32(transform.localPosition.y);
        z = System.Convert.ToInt32(transform.localPosition.z);
        headBasicY = headWallBlock.transform.localPosition.y;
        bodyBasicY = bodyWallBlock.transform.localPosition.y;
        direction = transform.rotation.eulerAngles;
        if (asMove == null)
        {
            asMove = GameObject.Find("AudioWallMove").GetComponent<AudioSource>();
        }
    }
    void OnMouseOver()
    {
    }
    float getMous()
    {
        float mousX = Input.GetAxis("Mouse X") * dragSpeedY;
        float mousY = Input.GetAxis("Mouse Y") * dragSpeedY;
        Vector3 cameraDir = Camera.main.transform.rotation.eulerAngles;
        if (direction.x > 85 && direction.x < 105)
        {
            if (cameraDir.y >= 0 && cameraDir.y < 45 || cameraDir.y >= 315 && cameraDir.y < 360)
            {
                return mousY;
            }
            if (cameraDir.y >= 45 && cameraDir.y < 135)
            {
                return -mousX;
            }
            if (cameraDir.y >= 135 && cameraDir.y < 225)
            {
                return -mousY;
            }
            if (cameraDir.y >= 225 && cameraDir.y < 315)
            {
                return mousX;
            }
        }
        else if (direction.x > 255 && direction.x < 285)
        {
            if (cameraDir.y >= 0 && cameraDir.y < 45 || cameraDir.y >= 315 && cameraDir.y < 360)
            {
                return -mousY;
            }
            if (cameraDir.y >= 45 && cameraDir.y < 135)
            {
                return mousX;
            }
            if (cameraDir.y >= 135 && cameraDir.y < 225)
            {
                return mousY;
            }
            if (cameraDir.y >= 225 && cameraDir.y < 315)
            {
                return -mousX;
            }
        }
        if (direction.z > 85 && direction.z < 105)
        {
            if (cameraDir.y >= 0 && cameraDir.y < 45 || cameraDir.y >= 315 && cameraDir.y < 360)
            {
                return -mousX;
            }
            if (cameraDir.y >= 45 && cameraDir.y < 135)
            {
                return -mousY;
            }
            if (cameraDir.y >= 135 && cameraDir.y < 225)
            {
                return mousX;
            }
            if (cameraDir.y >= 225 && cameraDir.y < 315)
            {
                return mousY;
            }
        }
        else if (direction.z > 255 && direction.z < 285)
        {
            if (cameraDir.y >= 0 && cameraDir.y < 45 || cameraDir.y >= 315 && cameraDir.y < 360)
            {
                return mousX;
            }
            if (cameraDir.y >= 45 && cameraDir.y < 135)
            {
                return mousY;
            }
            if (cameraDir.y >= 135 && cameraDir.y < 225)
            {
                return -mousX;
            }
            if (cameraDir.y >= 225 && cameraDir.y < 315)
            {
                return -mousY;
            }
        }
        return 1;
    }
    void Update()
    {
        if (GlobalController.isInteract)
        {
            switch (block)
            {
                case BLOCK.DRAG:
                    if (Cursor.visible)
                    {
                        energyValue1 = System.Convert.ToInt32(bodyWallBlock.transform.localScale.y);
                        //Debug.Log("energyValue1:" + energyValue1);
                        Cursor.visible = false;
                    }
                    //bodyWallBlock
                    float mousX = bodyWallBlock.transform.localScale.x;
                    float mousY = bodyWallBlock.transform.localScale.y;
                    float mousZ = bodyWallBlock.transform.localScale.z;
                    mousY += getMous();
                    mousY = mousY > maxh ? maxh : mousY;
                    //调整贴图
                    float scaleX = 1;
                    float scaleY = mousY;
                    bodyWallBlock.GetComponent<Renderer>().material.SetTextureScale("_MainTex", new Vector2(scaleX, scaleY));
                    if (!isFirstMove)
                    {
                        upMoveY = mousY;
                        isFirstMove = true;
                    }
                    if (Mathf.Abs(upMoveY - mousY) > 0.1f)
                    {
                        if (asMove != null && !asMove.isPlaying && !isPlayerAs)
                        {
                            isPlayerAs = true;

                            asMove.Play();
                        }
                        upMoveY = mousY;
                    }
                    if (mousY >= 0 && headBasicY + mousY < GlobalController.maxy - 0.5f && (mousY < EnergyController.energyQuantity - EnergyController.preLevelConsume || Input.GetAxis("Mouse Y") * dragSpeedY < 0))
                    {
                        bodyWallBlock.transform.localScale = new Vector3(mousX, mousY, mousZ);
                        bodyWallBlock.transform.localPosition = new Vector3(bodyWallBlock.transform.localPosition.x, bodyBasicY + mousY / 2, bodyWallBlock.transform.localPosition.z);
                        //headWallBlock
                        mousY = bodyWallBlock.transform.localScale.y;
                        headWallBlock.transform.localPosition = new Vector3(headWallBlock.transform.localPosition.x, headBasicY + mousY, headWallBlock.transform.localPosition.z);
                    }
                    break;
                case BLOCK.CONFIRM:
                    Cursor.visible = true;

                    mousY = System.Convert.ToInt32(bodyWallBlock.transform.localScale.y);
                    int energyValue2 = System.Convert.ToInt32(mousY);
                    //Debug.Log(energyValue2);
                    bodyWallBlock.transform.localScale = new Vector3(bodyWallBlock.transform.localScale.x, mousY, bodyWallBlock.transform.localScale.z);
                    bodyWallBlock.transform.localPosition = new Vector3(bodyWallBlock.transform.localPosition.x, bodyBasicY + mousY / 2, bodyWallBlock.transform.localPosition.z);
                    //调整贴图
                    scaleX = 1;
                    scaleY = mousY;
                    bodyWallBlock.GetComponent<Renderer>().material.SetTextureScale("_MainTex", new Vector2(scaleX, scaleY));
                    //headWallBlock
                    mousY = bodyWallBlock.transform.localScale.y;
                    headWallBlock.transform.localPosition = new Vector3(headWallBlock.transform.localPosition.x, headBasicY + mousY, headWallBlock.transform.localPosition.z);
                    //更新地图数据
                    GlobalController.isUpdateMap = true;
                    EnergyController.makeValue(energyValue2 - energyValue1);
                    //更新状态
                    bodyWallBlock.GetComponent<BodyWallBlock>().block = BodyWallBlock.BLOCK.STATIC;
                    headWallBlock.GetComponent<HeadWallBlock>().block = HeadWallBlock.BLOCK.STATIC;
                    block = BLOCK.STATIC;
                    isPlayerAs = false;
                    isFirstMove = false;
                    break;
            }
        }
    }
}
