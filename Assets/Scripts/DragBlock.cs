using UnityEngine;
using System.Collections;

public class DragBlock : MonoBehaviour {

    public GameObject headBlock;
    public GameObject bodyBlock;
    public int map_i;
    public int map_j;
    public float maxHeight;

    public float posYBasic;
    public float moveZ_Speed = 0.2f;
    public enum BLOCK { STATIC, DRAG, CONFIRM };
    public BLOCK block;
    public AudioSource asMove;
    private bool isPlayerAs = false;
    private float upMoveY = 0.0f;
    private bool isFirstMove = false;
    private int mapValue1=0;
    void Start()
    {
        maxHeight = GlobalController.maxHeight;
        posYBasic=headBlock.transform.position.y;
        if (asMove == null)
        {
            asMove = GameObject.Find("AudioWallMove").GetComponent<AudioSource>();
        }
    }
    void OnMouseOver()
    {
    }
    void OnMouseDown()
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
                        mapValue1 = System.Convert.ToInt32(bodyBlock.transform.localScale.y);
                        //Debug.Log("mapValue1:" + mapValue1);
                        Cursor.visible = false;
                        //Cursor.lockState = CursorLockMode.Locked;
                    }
                    //bodyBlock
                    float mousX = bodyBlock.transform.localScale.x;
                    float mousY = bodyBlock.transform.localScale.y;
                    float mousZ = bodyBlock.transform.localScale.z;
                    mousY += Input.GetAxis("Mouse Y") * moveZ_Speed;
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
                    if (mousY >= 0 && mousY <= GlobalController.maxHeight && (mousY < EnergyController.energyQuantity - EnergyController.preLevelConsume || Input.GetAxis("Mouse Y") * moveZ_Speed < 0))
                    {
                        bodyBlock.transform.localScale = new Vector3(mousX, mousY, mousZ);
                        bodyBlock.transform.position = new Vector3(bodyBlock.transform.position.x, mousY / 2 - 0.45f, bodyBlock.transform.position.z);
                        //headBlock
                        mousY = bodyBlock.transform.localScale.y;
                        headBlock.transform.position = new Vector3(headBlock.transform.position.x, posYBasic + mousY, headBlock.transform.position.z);
                    }
                    if (GlobalController.mouse == GlobalController.MOUSE.CONFIRM)
                    {
                        bodyBlock.GetComponent<BodyBlock>().block = BodyBlock.BLOCK.CONFIRM;
                        headBlock.GetComponent<HeadBlock>().block = HeadBlock.BLOCK.CONFIRM;
                        block = BLOCK.CONFIRM;
                        GlobalController.mouse = GlobalController.MOUSE.STATIC;
                    }
                    break;
                case BLOCK.CONFIRM:
                    Cursor.visible = true;
                    //Cursor.lockState = CursorLockMode.None;

                    mousY = System.Convert.ToInt32(bodyBlock.transform.localScale.y);
                    int mapValue2 = (int)mousY;
                    //Debug.Log(mapValue2);
                    bodyBlock.transform.localScale = new Vector3(bodyBlock.transform.localScale.x, mousY, bodyBlock.transform.localScale.z);
                    bodyBlock.transform.position = new Vector3(bodyBlock.transform.position.x, mousY / 2 - 0.45f, bodyBlock.transform.position.z);
                    //调整贴图
                    scaleX = 1;
                    scaleY = mousY;
                    bodyBlock.GetComponent<Renderer>().material.SetTextureScale("_MainTex", new Vector2(scaleX, scaleY));
                    //headBlock
                    mousY = bodyBlock.transform.localScale.y;
                    headBlock.transform.position = new Vector3(headBlock.transform.position.x, posYBasic + mousY, headBlock.transform.position.z);
                    //更新地图数据
                    MapController.map[map_i, map_j] = mapValue2;
                    EnergyController.makeValue(mapValue2 - mapValue1);

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
