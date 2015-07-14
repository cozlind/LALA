using UnityEngine;
using System.Collections;

public class ScaleBlock : MonoBehaviour {
    public GameObject centerCube;
    public float moveX_Speed = 1f;
    public float moveY_Speed = 1f;
    public float moveZ_Speed = 0.2f;
    public enum BLOCK { STATIC, DRAG, CONFIRM };
    public BLOCK block;
    void OnMouseOver()
    {
    }
    void OnMouseDown()
    {
        if (Input.GetMouseButton(0) && GlobalController.mouse == GlobalController.MOUSE.STATIC)
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
        float mousX = transform.localScale.x;
        float mousY = transform.localScale.y;
        float mousZ = transform.localScale.z;
        GameObject parent = transform.parent.gameObject;
        switch (parent.GetComponent<DragBlock>().block)
        {
            case DragBlock.BLOCK.DRAG:
               // mousY +=Input.GetAxis("Mouse Y") * moveZ_Speed;
                mousY = parent.transform.position.y;
                Debug.Log(transform.localScale);
                transform.localScale = new Vector3(mousX, mousY, mousZ);
                transform.position = new Vector3(transform.position.x,mousY / 2 - 0.5f,transform.position.z);

                Debug.Log(transform.localScale);
                //transform.Translate(mousX, mousY, mousZ, centerCube.transform);
                break;
            case DragBlock.BLOCK.CONFIRM:
                Cursor.visible = true;
                mousY = parent.transform.position.y;
                transform.localScale = new Vector3(mousX, mousY, mousZ);
                transform.position = new Vector3(transform.position.x,mousY / 2-0.5f,transform.position.z);
                block = BLOCK.STATIC;
                break;
        }
    }
}
