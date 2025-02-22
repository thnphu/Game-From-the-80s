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

    private GameManager gameManager;

    void Start()
    {
        // Initialize references
        audioSource = GetComponent<AudioSource>();
        gameManager = FindObjectOfType<GameManager>(); // Get reference to the GameManager
        
        UpdateUI();
        winText.gameObject.SetActive(false);
        gameOverText.gameObject.SetActive(false);
    }

    void Update()
    {
        // Do nothing if the game is over or won
        if (gameOverText.gameObject.activeSelf || winText.gameObject.activeSelf)
        {
            return;
        }

        // Helicopter movement using arrow keys
        float moveX = Input.GetAxis("Horizontal") * speed * Time.deltaTime;
        float moveY = Input.GetAxis("Vertical") * speed * Time.deltaTime;
        transform.Translate(new Vector3(moveX, moveY, 0));

        // Restart the game if 'R' is pressed
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
            currentSoldiers++; // Increment soldiers in the helicopter
            audioSource.PlayOneShot(pickupSound); // Play sound when picking up soldier
            gameManager.SoldierRescued(); // Update rescued soldiers in GameManager
            UpdateUI(); // Update the UI
        }
        // Empty helicopter at hospital
        else if (other.CompareTag("Hospital"))
        {
            int rescuedSoldiers = currentSoldiers;
            currentSoldiers = 0; // Empty the helicopter
            UpdateUI(); // Update UI after emptying
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
        // Update soldiers in helicopter and rescued soldiers
        soldiersInHelicopterText.text = "Soldiers in Helicopter: " + currentSoldiers;
        soldiersRescuedText.text = "Soldiers Rescued: " + gameManager.GetRescuedSoldiers();

        // Show Win Text if all soldiers are rescued
        if (gameManager.GetRescuedSoldiers() == gameManager.GetTotalSoldiers())
        {
            winText.gameObject.SetActive(true); // Show "You Win"
        }
    }

    private void ResetGame()
    {
        // Reset the scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
