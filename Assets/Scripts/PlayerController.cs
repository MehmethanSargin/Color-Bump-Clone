using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    private Rigidbody body;
    
    private Vector3 lastMousePos;
    
    [SerializeField]
    private float sensivity = .16f, clampDelta = 42f;
    
    [SerializeField]
    private float bounds = 5;
    
    [HideInInspector]
    public bool canMove, gameOver, finish;

    private void Awake()
    {
        body = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, -bounds, bounds), transform.position.y, transform.position.z);
        if (canMove)
        {
            transform.position += FindObjectOfType<CameraFollow>().camVelocity;
        }
        if (!canMove && gameOver)
        {
            if (Input.GetMouseButtonDown(0))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }else if (!canMove && !finish)
        {
            if (Input.GetMouseButtonDown(0))
            {
                canMove = true;
            }
        }
    }   
       


   private void FixedUpdate()
    {
        if (Input.GetMouseButtonDown(0))
        {
            lastMousePos = Input.mousePosition;
        }
        if (canMove)
        {
            if (Input.GetMouseButton(0))
            {
                Vector3 vector = lastMousePos - Input.mousePosition;
                lastMousePos = Input.mousePosition;
                vector = new Vector3(vector.x, 0, vector.y);

                Vector3 moveForce = Vector3.ClampMagnitude(vector, clampDelta);
                body.AddForce(-moveForce * sensivity - body.velocity / 5f, ForceMode.VelocityChange);
            }
        }
        body.velocity.Normalize();
    }

    private void GameOver()
    {
        canMove = false;
        gameOver = true;
        GetComponent<MeshRenderer>().enabled = false;
        GetComponent<Collider>().enabled = false;
    }

    private void OnCollisionEnter(Collision target)
    {
        if (target.gameObject.tag == "Enemy")
        {
            Debug.Log("GameOver");
            GameOver();
        }
    }

    IEnumerator NextLevel()
    {
        finish = true;
        canMove = false;
        PlayerPrefs.SetInt("Level", PlayerPrefs.GetInt("Level", 1) + 1);
        yield return new  WaitForSeconds(1);
        SceneManager.LoadScene("Level" + PlayerPrefs.GetInt("Level"));
    }

    private void OnTriggerEnter(Collider target)
    {
        if (target.gameObject.tag == "Finish")
        {
            StartCoroutine(NextLevel());
        }
    }
}
