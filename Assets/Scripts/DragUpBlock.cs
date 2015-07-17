using UnityEngine;
using System.Collections;

public class DragUpBlock : MonoBehaviour {

    public GameObject headUpBlock;
    public GameObject bodyUpBlock;
    public int map_i;
    public int map_j;
    public float maxy;

    public float posYBasicOffset;
    public float dragSpeedY = 0.2f;
    public enum BLOCK { STATIC, DRAG, CONFIRM };
    public BLOCK block;
    public AudioSource asMove;
    private bool isPlayerAs = false;
    private float upMoveY = 0.0f;
    private bool isFirstMove = false;
    private int energyValue1=0;
    void Start()
    {
        maxy = GlobalController.maxy;
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
                        energyValue1 = System.Convert.ToInt32(bodyUpBlock.transform.localScale.y);
                        //Debug.Log("energyValue1:" + energyValue1);
                        Cursor.visible = false;
                        //Cursor.lockState = CursorLockMode.Locked;
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
                    if (mousY >= 0 && mousY <= GlobalController.maxy && (mousY < EnergyController.energyQuantity - EnergyController.preLevelConsume || Input.GetAxis("Mouse Y") * dragSpeedY < 0))
                    {
                        bodyUpBlock.transform.localScale = new Vector3(mousX, mousY, mousZ);
                        bodyUpBlock.transform.position = new Vector3(bodyUpBlock.transform.position.x, mousY / 2 - 0.45f, bodyUpBlock.transform.position.z);
                        //headUpBlock
                        mousY = bodyUpBlock.transform.localScale.y;
                        headUpBlock.transform.position = new Vector3(headUpBlock.transform.position.x, posYBasicOffset + mousY, headUpBlock.transform.position.z);
                    }
                    break;
                case BLOCK.CONFIRM:
                    Cursor.visible = true;
                    //Cursor.lockState = CursorLockMode.None;

                    mousY = System.Convert.ToInt32(bodyUpBlock.transform.localScale.y);
                    int energyValue2 = (int)mousY;
                    //Debug.Log(energyValue2);
                    bodyUpBlock.transform.localScale = new Vector3(bodyUpBlock.transform.localScale.x, mousY, bodyUpBlock.transform.localScale.z);
                    bodyUpBlock.transform.position = new Vector3(bodyUpBlock.transform.position.x, mousY / 2 - 0.45f, bodyUpBlock.transform.position.z);
                    //调整贴图
                    scaleX = 1;
                    scaleY = mousY;
                    bodyUpBlock.GetComponent<Renderer>().material.SetTextureScale("_MainTex", new Vector2(scaleX, scaleY));
                    //headUpBlock
                    mousY = bodyUpBlock.transform.localScale.y;
                    headUpBlock.transform.position = new Vector3(headUpBlock.transform.position.x, posYBasicOffset + mousY, headUpBlock.transform.position.z);
                    //更新地图数据
                    GlobalController.isUpdateMap = true;
                    EnergyController.makeValue(energyValue2 - energyValue1);
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
