using UnityEngine;
using System.Collections;

public class HeadBlock : MonoBehaviour {

    public GameObject dragBlock;
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
                    dragBlock.GetComponent<DragBlock>().block = DragBlock.BLOCK.DRAG;
                    GlobalController.click = true;
                    break;
            }
        }
    }
}
