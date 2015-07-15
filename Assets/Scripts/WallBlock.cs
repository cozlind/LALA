using UnityEngine;
using System.Collections;

public class WallBlock : MonoBehaviour {

    public GameObject centerCube;
    public GameObject headBlock;
    public GameObject bodyBlock;
    public int map_i;
    public int map_j;
    public float maxHeight = 3;
    public float moveZ_Speed = 0.2f;
    public enum BLOCK { STATIC, DRAG, CONFIRM };
    public BLOCK block;
    void OnMouseOver()
    {
    }
    void OnMouseDown()
    {
        if (Input.GetMouseButton(0)&&GlobalController.mouse==GlobalController.MOUSE.STATIC)
        {
            switch (block)
            {
                case BLOCK.STATIC:
                    block = BLOCK.DRAG;
                    GlobalController.click = true;
                    break;
            }
        }
    }
    void Update()
    {
        switch (block)
        {
            case BLOCK.DRAG:
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
                //bodyBlock
                float mousX = bodyBlock.transform.localScale.x;
                float mousY = bodyBlock.transform.localScale.y;
                float mousZ = bodyBlock.transform.localScale.z;
                mousY += Input.GetAxis("Mouse X") * moveZ_Speed;
                //调整贴图
                float scaleX = 1;
                float scaleY = mousY;
                bodyBlock.GetComponent<Renderer>().material.SetTextureScale("_MainTex", new Vector2(scaleX, scaleY));

                if (mousY >= 0 && mousY <= GlobalController.maxHeight)
                {
                    bodyBlock.transform.localScale = new Vector3(mousX, mousY, mousZ);
                    bodyBlock.transform.position = new Vector3(mousY / 2 - 2.5f, bodyBlock.transform.position.y, bodyBlock.transform.position.z);
                    //headBlock
                    mousY=bodyBlock.transform.localScale.y+0.5f;
                    headBlock.transform.position = new Vector3(mousY - 2.5f, headBlock.transform.position.y, headBlock.transform.position.z);
                }
                if (GlobalController.mouse == GlobalController.MOUSE.CONFIRM)
                {
                    bodyBlock.GetComponent<BodyWallBlock>().block = BodyWallBlock.BLOCK.CONFIRM;
                    headBlock.GetComponent<HeadWallBlock>().block = HeadWallBlock.BLOCK.CONFIRM;
                    block = BLOCK.CONFIRM;
                }
                break;
            case BLOCK.CONFIRM:
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;

                mousY = System.Convert.ToInt32(bodyBlock.transform.localScale.y);
                int mapValue = (int)mousY;
                bodyBlock.transform.localScale = new Vector3(bodyBlock.transform.localScale.x, mousY, bodyBlock.transform.localScale.z);
                bodyBlock.transform.position = new Vector3(mousY / 2 - 2.5f, bodyBlock.transform.position.y, bodyBlock.transform.position.z);
                //bodyBlock.transform.position = new Vector3(bodyBlock.transform.position.x, mousY / 2 - 0.5f, bodyBlock.transform.position.z);
                //调整贴图
                scaleX = 1;
                scaleY = mousY;
                bodyBlock.GetComponent<Renderer>().material.SetTextureScale("_MainTex", new Vector2(scaleX, scaleY));
                //headBlock
                mousY=bodyBlock.transform.localScale.y+0.5f;
                headBlock.transform.position = new Vector3(mousY - 2.5f, headBlock.transform.position.y, headBlock.transform.position.z);
                //headBlock.transform.position = new Vector3(headBlock.transform.position.x, mousY - 0.5f, headBlock.transform.position.z);
                //更新地图数据
                //MapController.map[map_i, map_j] = mapValue;
                bodyBlock.GetComponent<BodyWallBlock>().block = BodyWallBlock.BLOCK.STATIC;
                headBlock.GetComponent<HeadWallBlock>().block = HeadWallBlock.BLOCK.STATIC;
                block = BLOCK.STATIC;
                break;
        }
    }
}
