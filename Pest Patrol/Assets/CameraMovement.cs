using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public float minCameraDistance, maxCameraDistance, minPos, maxPos;
    public float speed;
    private void Update()
    {
        var cameraDistance = (Input.mouseScrollDelta.y * Time.deltaTime * 10);
        var hor = Input.GetAxis("Horizontal") * Time.deltaTime;
        var vert = Input.GetAxis("Vertical") * Time.deltaTime;
        Camera.main.transform.Translate(new Vector3(hor, vert, cameraDistance) * speed);
        Vector3 clampedPos = Camera.main.transform.position;
        clampedPos.y = Mathf.Clamp(clampedPos.y, minCameraDistance, maxCameraDistance);
        clampedPos.x = Mathf.Clamp(clampedPos.x, minPos, maxPos);
        clampedPos.z = Mathf.Clamp(clampedPos.z, minPos, maxPos);
        Camera.main.transform.position = clampedPos;

    }
}
