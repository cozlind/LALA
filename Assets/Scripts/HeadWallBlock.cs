using UnityEngine;
using System.Collections;

public class HeadWallBlock : MonoBehaviour {

    public GameObject wallBlock;
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
                    wallBlock.GetComponent<WallBlock>().block = WallBlock.BLOCK.DRAG;
                    GlobalController.click = true;
                    break;
            }
        }
    }
}
