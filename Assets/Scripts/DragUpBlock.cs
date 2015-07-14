using UnityEngine;
using System.Collections;

public class DragUpBlock : MonoBehaviour {

    public GameObject headUpBlock;
    public GameObject bodyUpBlock;
    public int map_i;
    public int map_j;
    public float maxHeight = 3;
    public float moveX_Speed = 1f;
    public float moveY_Speed = 1f;
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
        //if (Input.GetMouseButton(0)&&GlobalController.mouse==GlobalController.MOUSE.STATIC)
        //{
        //    switch (block)
        //    {
        //        case BLOCK.STATIC:
        //            block = BLOCK.DRAG;
        //            mapValue1 =System.Convert.ToInt32(bodyUpBlock.transform.localScale.y);
        //            Debug.Log(mapValue1);
        //            GlobalController.click = true;
        //            break;
        //    }
        //}
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
                        mapValue1 = System.Convert.ToInt32(bodyUpBlock.transform.localScale.y);
                        //Debug.Log("mapValue1:" + mapValue1);
                        Cursor.visible = false;
                        Cursor.lockState = CursorLockMode.Locked;
                    }
                    //bodyUpBlock
                    float mousX = bodyUpBlock.transform.localScale.x;
                    float mousY = bodyUpBlock.transform.localScale.y;
                    float mousZ = bodyUpBlock.transform.localScale.z;
                    float input = Input.GetAxis("Mouse Y") * moveZ_Speed;
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
                    if (mousY >= 0 && mousY <= GlobalController.maxHeight && (mousY < EnergyController.energyQuantity - EnergyController.preLevelConsume || Input.GetAxis("Mouse Y") * moveZ_Speed < 0))
                    {
                        bodyUpBlock.transform.localScale = new Vector3(mousX, mousY, mousZ);
                        bodyUpBlock.transform.position = new Vector3(bodyUpBlock.transform.position.x, mousY / 2 - 0.45f, bodyUpBlock.transform.position.z);
                        //headUpBlock
                        mousY = bodyUpBlock.transform.localScale.y + 0.4f;
                        headUpBlock.transform.position = new Vector3(headUpBlock.transform.position.x, mousY - 0.4f, headUpBlock.transform.position.z);
                    }
                    if (GlobalController.mouse == GlobalController.MOUSE.CONFIRM)
                    {
                        bodyUpBlock.GetComponent<BodyUpBlock>().block = BodyUpBlock.BLOCK.CONFIRM;
                        headUpBlock.GetComponent<HeadUpBlock>().block = HeadUpBlock.BLOCK.CONFIRM;
                        block = BLOCK.CONFIRM;
                        GlobalController.mouse = GlobalController.MOUSE.STATIC;
                    }
                    break;
                case BLOCK.CONFIRM:
                    Cursor.visible = true;
                    Cursor.lockState = CursorLockMode.None;

                    mousY = System.Convert.ToInt32(bodyUpBlock.transform.localScale.y);
                    int mapValue2 = (int)mousY;
                    //Debug.Log(mapValue2);
                    bodyUpBlock.transform.localScale = new Vector3(bodyUpBlock.transform.localScale.x, mousY, bodyUpBlock.transform.localScale.z);
                    bodyUpBlock.transform.position = new Vector3(bodyUpBlock.transform.position.x, mousY / 2 - 0.45f, bodyUpBlock.transform.position.z);
                    //调整贴图
                    scaleX = 1;
                    scaleY = mousY;
                    bodyUpBlock.GetComponent<Renderer>().material.SetTextureScale("_MainTex", new Vector2(scaleX, scaleY));
                    //headUpBlock
                    mousY = bodyUpBlock.transform.localScale.y + 0.4f;
                    headUpBlock.transform.position = new Vector3(headUpBlock.transform.position.x, mousY - 0.4f, headUpBlock.transform.position.z);
                    //更新地图数据
                    MapController.map[map_i, map_j] = mapValue2;
                    EnergyController.makeValue(mapValue2 - mapValue1);
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
