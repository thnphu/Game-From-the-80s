using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class HelicopterController : MonoBehaviour
{
    public float speed = 5f;

    public int maxCapacity = 3;

    private int currentSoldiers = 0;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float moveX = Input.GetAxis("Horizontal") * speed * Time.deltaTime;
        float moveY = Input.GetAxis("Vertical") * speed * Time.deltaTime;
        transform.Translate(new Vector3(moveX, moveY, 0));
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Soldier") && currentSoldiers < maxCapacity)
        {
            Destroy(other.gameObject);
            currentSoldiers++;
        }
        else if (other.CompareTag("Hospital"))
        {
            currentSoldiers = 0;
        }
        else if (other.CompareTag("Tree"))
        {
            Debug.Log("Game Over!");
            UnityEngine.SceneManagement.SceneManager.LoadScene(0);
        }
    }
}
