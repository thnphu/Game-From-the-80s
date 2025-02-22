using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HelicopterController : MonoBehaviour
{
    public float speed = 5f;
    public int maxCapacity = 3;
    private int currentSoldiers = 0;

    // UI Elements
    public Text soldiersInHelicopterText;
    public Text soldiersRescuedText;
    public Text gameOverText;
    public Text winText;

    // Sound effect
    public AudioClip pickupSound;
    private AudioSource audioSource;

    // Reference to GameManager
    public GameManager gameManager;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        // Check if gameManager reference is assigned
        if (gameManager == null)
        {
            Debug.LogError("GameManager is not assigned in HelicopterController!");
        }

        // Check if UI elements are assigned
        if (soldiersInHelicopterText == null || soldiersRescuedText == null || gameOverText == null || winText == null)
        {
            Debug.LogError("UI elements are not assigned in HelicopterController!");
        }

        UpdateUI();
        winText.gameObject.SetActive(false);
        gameOverText.gameObject.SetActive(false);
    }

    void Update()
    {
        if (gameOverText.gameObject.activeSelf || winText.gameObject.activeSelf)
        {
            return; // Do nothing if the game is over or won
        }

        // Helicopter movement using arrow keys
        float moveX = Input.GetAxis("Horizontal") * speed * Time.deltaTime;
        float moveY = Input.GetAxis("Vertical") * speed * Time.deltaTime;
        transform.Translate(new Vector3(moveX, moveY, 0));

        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name); // Reset the scene
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Pick up soldier
        if (other.CompareTag("Soldier") && currentSoldiers < maxCapacity)
        {
            Destroy(other.gameObject); // Remove soldier from the scene
            currentSoldiers++;
            audioSource.PlayOneShot(pickupSound); // Play sound when picking up soldier

            if (gameManager != null) // Only call SoldierRescued if gameManager is assigned
            {
                gameManager.SoldierRescued(); // Call SoldierRescued from GameManager to update rescued count
            }

            UpdateUI();
        }
        // Empty helicopter at hospital
        else if (other.CompareTag("Hospital"))
        {
            int rescuedSoldiers = currentSoldiers;
            currentSoldiers = 0; // Empty the helicopter
            UpdateUI();
            Debug.Log("Soldiers taken to hospital: " + rescuedSoldiers);
        }
        // Check if hitting a tree (Game Over)
        else if (other.CompareTag("Tree"))
        {
            Debug.Log("Game Over! Restarting...");
            gameOverText.gameObject.SetActive(true); // Show "Game Over"
            Invoke("ResetGame", 2f); // Wait a moment before resetting the game
        }
    }

    private void UpdateUI()
    {
        // Check if gameManager is assigned before updating UI
        if (gameManager != null)
        {
            if (soldiersInHelicopterText != null)
            {
                soldiersInHelicopterText.text = "Soldiers in Helicopter: " + currentSoldiers;
            }

            if (soldiersRescuedText != null)
            {
                soldiersRescuedText.text = "Soldiers Rescued: " + gameManager.RescuedSoldiers;
            }

            if (gameManager.RescuedSoldiers >= gameManager.TotalSoldiers)
            {
                winText.gameObject.SetActive(true); // Show "You Win"
            }
        }
    }

    private void ResetGame()
    {
        // Reset the scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
