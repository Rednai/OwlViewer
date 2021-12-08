using System;
using UnityEngine;

public class OrbitCamera : MonoBehaviour
{
    public GameObject target;
    public float distance = 100.0f;

    public float xSpeed = 250.0f;
    public float ySpeed = 120.0f;

    float x = 0.0f;
    float y = 0.0f;

    void Start()
    {
        var angles = transform.eulerAngles;
        x = angles.y;
        y = angles.x;
    }

    float prevDistance;

    void LateUpdate()
    {
        UpdateCamera();
    }

    public void UpdateCamera(bool forceUpdate = false)
    {
        distance -= Input.GetAxis("Mouse ScrollWheel") * (distance * 0.25f);
        if ((target && Input.GetMouseButton(1)) || forceUpdate)
        {
            var pos = Input.mousePosition;
            var dpiScale = 1f;
            if (Screen.dpi < 1) dpiScale = 1;
            if (Screen.dpi < 200) dpiScale = 1;
            else dpiScale = Screen.dpi / 200f;

            if (!forceUpdate && pos.x < 380 * dpiScale && Screen.height - pos.y < 250 * dpiScale) return;

            x += Input.GetAxis("Mouse X") * xSpeed * 0.02f;
            y -= Input.GetAxis("Mouse Y") * ySpeed * 0.02f;

            var rotation = Quaternion.Euler(y, x, 0);
            var position = rotation * new Vector3(0.0f, 0.0f, -distance) + target.transform.position;
            transform.rotation = rotation;
            transform.position = position;
        }

        if (Math.Abs(prevDistance - distance) > 0.001f)
        {
            prevDistance = distance;
            var rot = Quaternion.Euler(y, x, 0);
            var po = rot * new Vector3(0.0f, 0.0f, -distance) + target.transform.position;
            transform.rotation = rot;
            transform.position = po;
        }
    }
}