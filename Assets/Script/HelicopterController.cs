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

    // Reference to the GameManager to update game state
    public GameManager gameManager;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        UpdateUI();
        winText.gameObject.SetActive(false);
        gameOverText.gameObject.SetActive(false);
    }

    void Update()
    {
        if (gameOverText.gameObject.activeSelf || winText.gameObject.activeSelf)
        {
            return; // Stop movement when the game is over or won
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
            audioSource.PlayOneShot(pickupSound); // Play pickup sound
            UpdateUI();
        }
        // Drop soldiers at hospital
        else if (other.CompareTag("Hospital"))
        {
            int rescuedSoldiers = currentSoldiers;
            currentSoldiers = 0; // Empty the helicopter
            gameManager.SoldierRescued(rescuedSoldiers); // Inform GameManager
            UpdateUI();
        }
        // Game Over when hitting a tree
        else if (other.CompareTag("Tree"))
        {
            Debug.Log("Game Over!");
            gameOverText.gameObject.SetActive(true);
            Invoke("ResetGame", 2f); // Restart game after 2 seconds
        }
    }

    private void UpdateUI()
    {
        soldiersInHelicopterText.text = "Soldiers in Helicopter: " + currentSoldiers;
        soldiersRescuedText.text = "Soldiers Rescued: " + gameManager.GetRescuedSoldiers();

        if (gameManager.GetRescuedSoldiers() >= gameManager.GetTotalSoldiers())
        {
            winText.gameObject.SetActive(true); // Show "You Win" text
            Invoke("ResetGame", 2f); // Restart game after 2 seconds
        }
    }

    private void ResetGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
