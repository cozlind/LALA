using UnityEngine;
using System.Collections;

public class HeadArrowBlock : MonoBehaviour {

    public GameObject dragArrowBlock;
    public enum BLOCK { STATIC, DRAG, CONFIRM };
    public BLOCK block;
    void OnMouseOver()
    {
    }
    void OnMouseDrag()
    {
        if (Input.GetMouseButton(0) && dragArrowBlock.GetComponent<DragArrowBlock>().block == DragArrowBlock.BLOCK.STATIC)
        {
            switch (block)
            {
                case BLOCK.STATIC:
                    block = BLOCK.DRAG;
                    dragArrowBlock.GetComponent<DragArrowBlock>().block = DragArrowBlock.BLOCK.DRAG;
                    break;
            }
        }
    }
    void OnMouseUp()
    {
        if (Input.GetMouseButtonUp(0) && dragArrowBlock.GetComponent<DragArrowBlock>().block == DragArrowBlock.BLOCK.DRAG)
        {
            switch (block)
            {
                case BLOCK.DRAG:
                    block = BLOCK.CONFIRM;
                    dragArrowBlock.GetComponent<DragArrowBlock>().block = DragArrowBlock.BLOCK.CONFIRM;
                    break;
            }
        }
    }
}
