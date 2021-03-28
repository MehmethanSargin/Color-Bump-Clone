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

    
    public GameObject breakablePlayer;

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
                Time.timeScale = 1;
                Time.fixedDeltaTime = Time.timeScale * 0.02f;
            }
        }else if (!canMove && !finish)
        {
            if (Input.GetMouseButtonDown(0))
            {
                FindObjectOfType<GameManager>().RemoveUI();
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
        GameObject shatterShpere = Instantiate(breakablePlayer, transform.position, Quaternion.identity);
        foreach (Transform t in shatterShpere.transform)
        {
            t.GetComponent<Rigidbody>().AddForce(Vector3.forward * body.velocity.magnitude, ForceMode.Impulse);
        }
        canMove = false;
        gameOver = true;
        GetComponent<MeshRenderer>().enabled = false;
        GetComponent<Collider>().enabled = false;

        Time.timeScale = .3f;
        Time.fixedDeltaTime = Time.timeScale * 0.02f;
    }

    private void OnCollisionEnter(Collision target)
    {
        if (target.gameObject.tag == "Enemy")
        {
            if (!gameOver)
            {
                Debug.Log("GameOver");
                GameOver();
            } 
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
