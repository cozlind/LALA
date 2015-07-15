using UnityEngine;
using System.Collections;

public class HeadWallBlock : MonoBehaviour {

    public GameObject wallBlock;
    public enum BLOCK { STATIC, DRAG, CONFIRM };
    public BLOCK block;
    void OnMouseOver()
    {
    }
    void OnMouseDrag()
    {
        if (Input.GetMouseButton(0) && wallBlock.GetComponent<WallBlock>().block == WallBlock.BLOCK.STATIC)
        {
            switch (block)
            {
                case BLOCK.STATIC:
                    block = BLOCK.DRAG;
                    wallBlock.GetComponent<WallBlock>().block = WallBlock.BLOCK.DRAG;
                    break;
            }
        }
    }
    void OnMouseUp()
    {
        if (Input.GetMouseButtonUp(0) && wallBlock.GetComponent<WallBlock>().block == WallBlock.BLOCK.DRAG)
        {
            switch (block)
            {
                case BLOCK.DRAG:
                    block = BLOCK.CONFIRM;
                    wallBlock.GetComponent<WallBlock>().block = WallBlock.BLOCK.CONFIRM;
                    break;
            }
        }
    }
}
