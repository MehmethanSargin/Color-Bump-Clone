using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField]
    private float cameraSpeed = 6;
    [SerializeField]
    public Vector3 camVelocity;

    PlayerController playerController;

    private void Start()
    {
        playerController = FindObjectOfType<PlayerController>();
    }

    private void Update()
    {
        if (playerController.canMove)
        {
            transform.position += Vector3.forward * cameraSpeed;
            camVelocity = Vector3.forward * cameraSpeed;
        }
        
    }
}
