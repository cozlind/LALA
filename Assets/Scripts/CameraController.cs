using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour
{

    public GameObject target;
    public float minFov = 3.5f;
    public float maxFov = 7f;
    public float sensitivity = 1f;
    public float rotate_Speed = 6f;//旋转速度
    public float moveX_Speed = 0.7f;
    public float moveY_Speed = 0.7f;

    private Vector3 originPosition;
    private Quaternion originRotation;
    public float shake_decay;
    public float shake_intensity;

    private static float startTime = 0f;
    public static bool isEntering = false;
    public static bool isEnd = false;
    void Start()
    {
        isEntering = true;

        GetComponent<UnityStandardAssets.ImageEffects.BlurOptimized>().blurSize = 10;
        GetComponent<UnityStandardAssets.ImageEffects.BlurOptimized>().blurIterations = 4;
    }
    void FixedUpdate()
    {
        if (isEntering)
        {
            startTime += Time.fixedDeltaTime * 10;
            Camera.main.GetComponent<UnityStandardAssets.ImageEffects.BlurOptimized>().blurSize = 10 - startTime;
            Camera.main.GetComponent<UnityStandardAssets.ImageEffects.BlurOptimized>().blurIterations = (int)(4 - startTime * 0.4f);
            if (Camera.main.GetComponent<UnityStandardAssets.ImageEffects.BlurOptimized>().blurSize <= 0)
            {
                startTime = 0;
                Camera.main.GetComponent<UnityStandardAssets.ImageEffects.BlurOptimized>().enabled = false;
                isEntering = false;
                UIController.normalUI.SetActive(true);
            }
        }
        if (isEnd)
        {
            startTime += Time.fixedDeltaTime * 10;
            Camera.main.GetComponent<UnityStandardAssets.ImageEffects.BlurOptimized>().blurSize = startTime;
            Camera.main.GetComponent<UnityStandardAssets.ImageEffects.BlurOptimized>().blurIterations = (int)(startTime * 0.4f);
            if (Camera.main.GetComponent<UnityStandardAssets.ImageEffects.BlurOptimized>().blurSize >= 9.9f)
            {
                startTime = 0;
                isEnd = false;
            }
        }


        //滚轮缩放
        float fov = Camera.main.orthographicSize;
        fov += -Input.GetAxis("Mouse ScrollWheel") * sensitivity;
        fov = Mathf.Clamp(fov, minFov, maxFov);
        Camera.main.orthographicSize = fov;
        //右键绕中心旋转
        if (Input.GetMouseButton(1))
        {
            float mousX = Input.GetAxis("Mouse X");//得到鼠标移动距离
            transform.RotateAround(target.transform.position, new Vector3(0, mousX, 0), rotate_Speed);
        }
        if (shake_intensity > 0)
        {
            transform.position = originPosition + Random.insideUnitSphere * shake_intensity;

            transform.rotation = new Quaternion(
            originRotation.x + Random.Range(-shake_intensity, shake_intensity) * .2f,
            originRotation.y + Random.Range(-shake_intensity, shake_intensity) * .2f,
            originRotation.z + Random.Range(-shake_intensity, shake_intensity) * .2f,
            originRotation.w + Random.Range(-shake_intensity, shake_intensity) * .2f);
            shake_intensity -= shake_decay;
        }
    }
    //void OnGUI()
    //{
    //    if (GUI.Button(new Rect(20, 40, 80, 20), "Shake"))
    //    {
    //        shake();
    //    }
    //}
    public void shake()
    {
        originPosition = transform.position;
        originRotation = transform.rotation;
        shake_intensity = .15f;
        shake_decay = 0.0024f;
    }

}
