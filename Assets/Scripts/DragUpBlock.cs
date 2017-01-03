using UnityEngine;
using System.Collections;

public class DragUpBlock : MonoBehaviour
{

    //基本结构
    public GameObject headUpBlock;
    public GameObject bodyUpBlock;
    public int x;
    public int y;
    public int z;
    public string type;
    public enum BLOCK { STATIC, DRAG, CONFIRM };
    public BLOCK block;
    //拖拽参数
    public float headBasicY;
    public float bodyBasicY;
    private int energyValue1 = 0;
    public float dragSpeedY = 0.2f;

    public AudioSource asMove;
    private bool isPlayerAs = false;
    private bool isFirstMove = false;
    private float upMoveY = 0.0f;
    void updateMap()
    {
        int heady = System.Convert.ToInt32(headUpBlock.transform.position.y);
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
        headBasicY = headUpBlock.transform.position.y;
        bodyBasicY = bodyUpBlock.transform.position.y;
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
                        energyValue1 = System.Convert.ToInt32(bodyUpBlock.transform.localScale.y);
                        //Debug.Log("energyValue1:" + energyValue1);
                        Cursor.visible = false;
                    }
                    //bodyUpBlock
                    float mousX = bodyUpBlock.transform.localScale.x;
                    float mousY = bodyUpBlock.transform.localScale.y;
                    float mousZ = bodyUpBlock.transform.localScale.z;
                    float input = Input.GetAxis("Mouse Y") * dragSpeedY;
                    input = input > 0 ? input : 0;
                    mousY += input;
                    //调整贴图
                    float scaleX = 1;
                    float scaleY = mousY;
                    bodyUpBlock.GetComponent<Renderer>().material.SetTextureScale("_MainTex", new Vector2(scaleX, scaleY));
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
                    if (mousY >= 0 && headBasicY + mousY < GlobalController.maxy - 0.5f && mousY < EnergyController.energyQuantity - EnergyController.preLevelConsume)
                    {
                        bodyUpBlock.transform.localScale = new Vector3(mousX, mousY, mousZ);
                        bodyUpBlock.transform.position = new Vector3(bodyUpBlock.transform.position.x, bodyBasicY + mousY / 2, bodyUpBlock.transform.position.z);
                        //headUpBlock
                        mousY = bodyUpBlock.transform.localScale.y;
                        headUpBlock.transform.position = new Vector3(headUpBlock.transform.position.x, headBasicY + mousY, headUpBlock.transform.position.z);
                    }
                    break;
                case BLOCK.CONFIRM:
                    Cursor.visible = true;

                    mousY = System.Convert.ToInt32(bodyUpBlock.transform.localScale.y);
                    int energyValue2 = System.Convert.ToInt32(mousY);
                    //Debug.Log(energyValue2);
                    bodyUpBlock.transform.localScale = new Vector3(bodyUpBlock.transform.localScale.x, mousY, bodyUpBlock.transform.localScale.z);
                    bodyUpBlock.transform.position = new Vector3(bodyUpBlock.transform.position.x, bodyBasicY + mousY / 2 , bodyUpBlock.transform.position.z);
                    //调整贴图
                    scaleX = 1;
                    scaleY = mousY;
                    bodyUpBlock.GetComponent<Renderer>().material.SetTextureScale("_MainTex", new Vector2(scaleX, scaleY));
                    //headUpBlock
                    mousY = bodyUpBlock.transform.localScale.y;
                    headUpBlock.transform.position = new Vector3(headUpBlock.transform.position.x, headBasicY + mousY, headUpBlock.transform.position.z);
                    //更新地图数据
                    GlobalController.isUpdateMap = true;
                    EnergyController.makeValue(energyValue2 - energyValue1);
                    //更新状态
                    bodyUpBlock.GetComponent<BodyUpBlock>().block = BodyUpBlock.BLOCK.STATIC;
                    headUpBlock.GetComponent<HeadUpBlock>().block = HeadUpBlock.BLOCK.STATIC;
                    block = BLOCK.STATIC;
                    isPlayerAs = false;
                    isFirstMove = false;
                    break;
            }
        }
    }
}
