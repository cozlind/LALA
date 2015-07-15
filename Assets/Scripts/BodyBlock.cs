using UnityEngine;
using System.Collections;

public class BodyBlock : MonoBehaviour {

    public GameObject dragBlock;
    public enum BLOCK { STATIC, DRAG, CONFIRM };
    public BLOCK block;
    void OnMouseOver()
    {
    }
    void OnMouseDrag()
    {
        if (Input.GetMouseButton(0) && dragBlock.GetComponent<DragBlock>().block == DragBlock.BLOCK.STATIC)
        {
            switch (block)
            {
                case BLOCK.STATIC:
                    block = BLOCK.DRAG;
                    dragBlock.GetComponent<DragBlock>().block = DragBlock.BLOCK.DRAG;
                    break;
            }
        }
    }
    void OnMouseUp()
    {
        if (Input.GetMouseButtonUp(0) && dragBlock.GetComponent<DragBlock>().block == DragBlock.BLOCK.DRAG)
        {
            switch (block)
            {
                case BLOCK.DRAG:
                    block = BLOCK.CONFIRM;
                    dragBlock.GetComponent<DragBlock>().block = DragBlock.BLOCK.CONFIRM;
                    break;
            }
        }
    }
}
