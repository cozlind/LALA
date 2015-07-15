using UnityEngine;
using System.Collections;

public class HeadUpBlock : MonoBehaviour {

    public GameObject dragUpBlock;
    public enum BLOCK { STATIC, DRAG, CONFIRM };
    public BLOCK block;
    void OnMouseOver()
    {
    }
    void OnMouseDrag()
    {
        if (Input.GetMouseButton(0) && dragUpBlock.GetComponent<DragUpBlock>().block == DragUpBlock.BLOCK.STATIC)
        {
            switch (block)
            {
                case BLOCK.STATIC:
                    block = BLOCK.DRAG;
                    dragUpBlock.GetComponent<DragUpBlock>().block = DragUpBlock.BLOCK.DRAG;
                    break;
            }
        }
    }
    void OnMouseUp()
    {
        if (Input.GetMouseButtonUp(0) && dragUpBlock.GetComponent<DragUpBlock>().block == DragUpBlock.BLOCK.DRAG)
        {
            switch (block)
            {
                case BLOCK.DRAG:
                    block = BLOCK.CONFIRM;
                    dragUpBlock.GetComponent<DragUpBlock>().block = DragUpBlock.BLOCK.CONFIRM;
                    break;
            }
        }
    }
}
