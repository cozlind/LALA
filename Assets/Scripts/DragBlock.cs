using UnityEngine;
using System.Collections;

public class DragBlock : MonoBehaviour
{

    //基本结构
    public GameObject headBlock;
    public GameObject bodyBlock;
    public int x;
    public int y;
    public int z;
    public string type;
    public enum BLOCK { STATIC, DRAG, CONFIRM };
    public BLOCK block;
    //拖拽参数
    public float headBasicY;
    public float bodyBasicY;
    public float bodyBasicScale;
    private int energyValue1 = 0;
    public float dragSpeedY = 0.2f;

    public AudioSource asMove;
    private bool isPlayerAs = false;
    private bool isFirstMove = false;
    private float upMoveY = 0.0f;
    public void updateMap()
    {
        int heady = System.Convert.ToInt32(headBlock.transform.position.y);
        for (int i = heady; i >= y; i--)
        {
            updateToMap(x, i, z);
            GlobalController.typeMap[x, i, z] = "Base";
        }
        GlobalController.typeMap[x, heady, z] = type;
    }
    void updateToMap(int x, int y, int z)
    {
        GlobalController.map[x, y, z] = 1;
    }
    void Start()
    {
        x = System.Convert.ToInt32(transform.position.x);
        y = System.Convert.ToInt32(transform.position.y);
        z = System.Convert.ToInt32(transform.position.z);
        headBasicY = headBlock.transform.position.y;
        bodyBasicY = bodyBlock.transform.position.y;
        bodyBasicScale = bodyBlock.transform.localScale.y;
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
                        energyValue1 = System.Convert.ToInt32(bodyBlock.transform.localScale.y);
                        //Debug.Log("energyValue1:" + energyValue1);
                        Cursor.visible = false;
                    }
                    //bodyBlock
                    float mousX = bodyBlock.transform.localScale.x;
                    float mousY = bodyBlock.transform.localScale.y;
                    float mousZ = bodyBlock.transform.localScale.z;
                    mousY += Input.GetAxis("Mouse Y") * dragSpeedY;
                    //调整贴图
                    float scaleX = 1;
                    float scaleY = mousY;
                    bodyBlock.GetComponent<Renderer>().material.SetTextureScale("_MainTex", new Vector2(scaleX, scaleY));
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
                    if (mousY >= 0 && headBasicY - bodyBasicScale + mousY < GlobalController.maxy - 0.7f && (mousY < EnergyController.energyQuantity - EnergyController.preLevelConsume || Input.GetAxis("Mouse Y") * dragSpeedY < 0))
                    {
                        bodyBlock.transform.localScale = new Vector3(mousX, mousY, mousZ);
                        bodyBlock.transform.position = new Vector3(bodyBlock.transform.position.x, bodyBasicY - bodyBasicScale/2 + mousY / 2, bodyBlock.transform.position.z);
                        //headBlock
                        mousY = bodyBlock.transform.localScale.y;
                        headBlock.transform.position = new Vector3(headBlock.transform.position.x, headBasicY - bodyBasicScale+ mousY, headBlock.transform.position.z);
                    }
                    break;
                case BLOCK.CONFIRM:
                    Cursor.visible = true;

                    mousY = System.Convert.ToInt32(bodyBlock.transform.localScale.y);
                    int energyValue2 = System.Convert.ToInt32(mousY);
                    //Debug.Log(energyValue2);
                    bodyBlock.transform.localScale = new Vector3(bodyBlock.transform.localScale.x, mousY, bodyBlock.transform.localScale.z);
                    bodyBlock.transform.position = new Vector3(bodyBlock.transform.position.x, bodyBasicY - bodyBasicScale / 2 + mousY / 2, bodyBlock.transform.position.z);
                    //调整贴图
                    scaleX = 1;
                    scaleY = mousY;
                    bodyBlock.GetComponent<Renderer>().material.SetTextureScale("_MainTex", new Vector2(scaleX, scaleY));
                    //headBlock
                    mousY = bodyBlock.transform.localScale.y;
                    headBlock.transform.position = new Vector3(headBlock.transform.position.x, headBasicY - bodyBasicScale + mousY, headBlock.transform.position.z);
                    //更新地图数据
                    GlobalController.isUpdateMap = true;
                    EnergyController.makeValue(energyValue2 - energyValue1);
                    //更新状态
                    bodyBlock.GetComponent<BodyBlock>().block = BodyBlock.BLOCK.STATIC;
                    headBlock.GetComponent<HeadBlock>().block = HeadBlock.BLOCK.STATIC;
                    block = BLOCK.STATIC;
                    isPlayerAs = false;
                    isFirstMove = false;
                    break;
            }
        }
    }
}
