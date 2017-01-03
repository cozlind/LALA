using UnityEngine;
using System.Collections;

public class DragArrowBlock : MonoBehaviour
{

    //基本结构
    public GameObject headArrowBlock;
    public GameObject bodyArrowBlock;
    public int x;
    public int y;
    public int z;
    public string type = "Arrow";
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
        int heady = System.Convert.ToInt32(headArrowBlock.transform.position.y);
        for (int i = heady; i >= y; i--)
        {
            updateToMap(x, i, z);
        }
        string aStarDir = "";
        if (direction.y > -5 && direction.y < 5)
        {
            aStarDir = "-x";
        }
        if (direction.y > 85 && direction.y < 95)
        {
            aStarDir = "+z";
        }
        if (direction.y > 175 && direction.y < 185)
        {
            aStarDir = "+x";
        }
        if (direction.y > 265 && direction.y < 275)
        {
            aStarDir = "-z";
        }
        GlobalController.typeMap[x, heady, z] = type + ":" + aStarDir;
    }
    void updateToMap(int x, int y, int z)
    {
        GlobalController.map[x, y, z] = 1;
    }
    void Start()
    {
        x = System.Convert.ToInt32(transform.localPosition.x);
        y = System.Convert.ToInt32(transform.localPosition.y);
        z = System.Convert.ToInt32(transform.localPosition.z);
        headBasicY = headArrowBlock.transform.localPosition.y;
        bodyBasicY = bodyArrowBlock.transform.localPosition.y;
        direction = transform.rotation.eulerAngles;
        if (asMove == null)
        {
            asMove = GameObject.Find("AudioWallMove").GetComponent<AudioSource>();
        }
    }
    void OnMouseOver()
    {
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
                        energyValue1 = System.Convert.ToInt32(bodyArrowBlock.transform.localScale.y);
                        //Debug.Log("energyValue1:" + energyValue1);
                        Cursor.visible = false;
                    }
                    //bodyArrowBlock
                    float mousX = bodyArrowBlock.transform.localScale.x;
                    float mousY = bodyArrowBlock.transform.localScale.y;
                    float mousZ = bodyArrowBlock.transform.localScale.z;
                    mousY += Input.GetAxis("Mouse Y") * dragSpeedY;
                    //调整贴图
                    float scaleX = 1;
                    float scaleY = mousY;
                    bodyArrowBlock.GetComponent<Renderer>().material.SetTextureScale("_MainTex", new Vector2(scaleX, scaleY));
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
                        bodyArrowBlock.transform.localScale = new Vector3(mousX, mousY, mousZ);
                        bodyArrowBlock.transform.localPosition = new Vector3(bodyArrowBlock.transform.localPosition.x, bodyBasicY + mousY / 2, bodyArrowBlock.transform.localPosition.z);
                        //headArrowBlock
                        mousY = bodyArrowBlock.transform.localScale.y;
                        headArrowBlock.transform.localPosition = new Vector3(headArrowBlock.transform.localPosition.x, headBasicY + mousY, headArrowBlock.transform.localPosition.z);
                    }
                    break;
                case BLOCK.CONFIRM:
                    Cursor.visible = true;

                    mousY = System.Convert.ToInt32(bodyArrowBlock.transform.localScale.y);
                    int energyValue2 = System.Convert.ToInt32(mousY);
                    //Debug.Log(energyValue2);
                    bodyArrowBlock.transform.localScale = new Vector3(bodyArrowBlock.transform.localScale.x, mousY, bodyArrowBlock.transform.localScale.z);
                    bodyArrowBlock.transform.localPosition = new Vector3(bodyArrowBlock.transform.localPosition.x, bodyBasicY + mousY / 2, bodyArrowBlock.transform.localPosition.z);
                    //调整贴图
                    scaleX = 1;
                    scaleY = mousY;
                    bodyArrowBlock.GetComponent<Renderer>().material.SetTextureScale("_MainTex", new Vector2(scaleX, scaleY));
                    //headArrowBlock
                    mousY = bodyArrowBlock.transform.localScale.y;
                    headArrowBlock.transform.localPosition = new Vector3(headArrowBlock.transform.localPosition.x, headBasicY + mousY, headArrowBlock.transform.localPosition.z);
                    //更新地图数据
                    GlobalController.isUpdateMap = true;
                    EnergyController.makeValue(energyValue2 - energyValue1);
                    //更新状态
                    bodyArrowBlock.GetComponent<BodyArrowBlock>().block = BodyArrowBlock.BLOCK.STATIC;
                    headArrowBlock.GetComponent<HeadArrowBlock>().block = HeadArrowBlock.BLOCK.STATIC;
                    block = BLOCK.STATIC;
                    isPlayerAs = false;
                    isFirstMove = false;
                    break;
            }
        }
    }
}
