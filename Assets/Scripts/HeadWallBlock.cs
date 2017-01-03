using UnityEngine;
using System.Collections;

public class HeadWallBlock : MonoBehaviour {

    public GameObject dragWallBlock;
    public enum BLOCK { STATIC, DRAG, CONFIRM };
    public BLOCK block;
    void OnMouseOver()
    {
    }
    void OnMouseDrag()
    {
        if (Input.GetMouseButton(0) && dragWallBlock.GetComponent<DragWallBlock>().block == DragWallBlock.BLOCK.STATIC)
        {
            switch (block)
            {
                case BLOCK.STATIC:
                    block = BLOCK.DRAG;
                    dragWallBlock.GetComponent<DragWallBlock>().block = DragWallBlock.BLOCK.DRAG;
                    break;
            }
        }
    }
    void OnMouseUp()
    {
        if (Input.GetMouseButtonUp(0) && dragWallBlock.GetComponent<DragWallBlock>().block == DragWallBlock.BLOCK.DRAG)
        {
            switch (block)
            {
                case BLOCK.DRAG:
                    block = BLOCK.CONFIRM;
                    dragWallBlock.GetComponent<DragWallBlock>().block = DragWallBlock.BLOCK.CONFIRM;
                    break;
            }
        }
    }
}
