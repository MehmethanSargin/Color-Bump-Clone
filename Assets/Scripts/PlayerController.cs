using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody body;
    private Vector3 lastMousePos;
    [SerializeField]
    private float sensivity = .16f, clampDelta = 42f;
    [SerializeField]
    private float bounds = 5;

    void Awake()
    {
        body = GetComponent<Rigidbody>();
    }
    private void Update()
    {
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, -bounds, bounds), transform.position.y, transform.position.z);
        transform.position += FindObjectOfType<CameraFollow>().camVelocity;
    }


    void FixedUpdate()
    {
        if (Input.GetMouseButtonDown(0))
        {
            lastMousePos = Input.mousePosition;
        }
        if (Input.GetMouseButton(0))
        {
            Vector3 vector = lastMousePos - Input.mousePosition;
            lastMousePos = Input.mousePosition;
            vector = new Vector3(vector.x, 0, vector.y);

            Vector3 moveForce = Vector3.ClampMagnitude(vector, clampDelta);
            body.AddForce(-moveForce * sensivity - body.velocity / 5f, ForceMode.VelocityChange);
        }
        body.velocity.Normalize();
    }
}
