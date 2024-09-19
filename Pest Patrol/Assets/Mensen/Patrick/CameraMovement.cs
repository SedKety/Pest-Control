using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public float minCameraDistance, maxCameraDistance;
    public float cameraDistance;
    public float horizontalLimit, verticalLimit;
    public float speed;

    private Vector3 initialPosition;

    private void Start()
    {
        initialPosition = Camera.main.transform.position;
    }

    private void Update()
    {
        cameraDistance = (-Input.mouseScrollDelta.y * Time.deltaTime * speed * 10);
        float hor = Input.GetAxis("Horizontal") * speed * Time.deltaTime;
        float vert = Input.GetAxis("Vertical") * speed * Time.deltaTime;
        Vector3 movement = new Vector3(hor, cameraDistance, vert);

        Camera.main.transform.Translate(movement, Space.World);

        Vector3 currentPos = Camera.main.transform.position;

        currentPos.y = Mathf.Clamp(currentPos.y, initialPosition.y - minCameraDistance, initialPosition.y + maxCameraDistance);

        currentPos.x = Mathf.Clamp(currentPos.x, initialPosition.x - horizontalLimit, initialPosition.x + horizontalLimit);
        currentPos.z = Mathf.Clamp(currentPos.z, initialPosition.z - verticalLimit, initialPosition.z + verticalLimit);

        Camera.main.transform.position = currentPos;
    }
}
