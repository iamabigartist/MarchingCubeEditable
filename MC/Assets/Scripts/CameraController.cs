using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    const float speed = 3f;
    const float moveSpeed = 5f;
    const float scrollSpeed = 20f;
    const float rotateSpeed = 60f;

    Transform cam;

    void Start()
    {

        cam = Camera.main.transform;
    }

    private void LateUpdate()
    {
        //Debug.Log("__________________________");
    }

    void Update()
    {
        if (Input.GetMouseButton(1))
        {
            float rotY = -Input.GetAxis("Mouse Y") * Time.deltaTime * rotateSpeed;
            float rotX = Input.GetAxis("Mouse X") * Time.deltaTime * rotateSpeed;

            cam.Rotate(cam.right, rotY, Space.World);
            cam.Rotate(Vector3.up, rotX, Space.World);

            float _forward = Input.GetAxis("Vertical") * Time.deltaTime * moveSpeed;
            float _right = Input.GetAxis("Horizontal") * Time.deltaTime * moveSpeed;

            transform.Translate(cam.forward * _forward + cam.right * _right, Space.World);
        }
        else if (Input.GetMouseButton(2))
        {
            float u = -Input.GetAxis("Mouse X") * Time.deltaTime * speed;
            float v = -Input.GetAxis("Mouse Y") * Time.deltaTime * speed;

            transform.Translate(cam.up * v + cam.right * u, Space.World);
        }
        else if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            transform.Translate(cam.forward * Time.deltaTime * scrollSpeed, Space.World);
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            transform.Translate(-cam.forward * Time.deltaTime * scrollSpeed, Space.World);
        }
    }
}
