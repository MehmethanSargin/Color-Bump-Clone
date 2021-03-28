using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField]
    private float cameraSpeed = 6;
    [SerializeField]
    public Vector3 camVelocity;

    private void Update()
    {
        transform.position += Vector3.forward * cameraSpeed * Time.deltaTime;
        camVelocity = Vector3.forward * cameraSpeed * Time.deltaTime;
    }
}
