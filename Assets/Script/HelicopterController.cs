using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HelicopterController : MonoBehaviour
{
    public float speed = 5f;
    public float boostSpeed = 8f; // Faster speed when holding Shift
    public int maxCapacity = 3;
    private int currentSoldiers = 0;

    public Text soldiersInHelicopterText;
    public Text soldiersRescuedText;
    public Text gameOverText;
    public Text winText;

    public AudioClip pickupSound;
    private AudioSource audioSource;

    private GameManager gameManager;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        gameManager = FindObjectOfType<GameManager>(); // Find GameManager in the scene

        UpdateUI();
        winText.gameObject.SetActive(false);
        gameOverText.gameObject.SetActive(false);
    }

    void Update()
    {
        if (gameOverText.gameObject.activeSelf || winText.gameObject.activeSelf)
        {
            return; // Stop movement if the game is over or won
        }

        // 🚁 **Helicopter Movement with Shift Boost**
        float currentSpeed = Input.GetKey(KeyCode.LeftShift) ? boostSpeed : speed;

        float moveX = Input.GetAxis("Horizontal") * currentSpeed * Time.deltaTime;
        float moveY = Input.GetAxis("Vertical") * currentSpeed * Time.deltaTime;
        transform.Translate(new Vector3(moveX, moveY, 0));

        // Reset game when 'R' key is pressed
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Pick up soldier
        if (other.CompareTag("Soldier") && currentSoldiers < maxCapacity)
        {
            Destroy(other.gameObject); // Remove soldier from the scene
            currentSoldiers++; // Increase the count
            audioSource.PlayOneShot(pickupSound); // Play sound
            UpdateUI();
        }
        // Drop off soldiers at the hospital
        else if (other.CompareTag("Hospital") && currentSoldiers > 0)
        {
            gameManager.SoldierRescued(currentSoldiers); // Notify GameManager
            currentSoldiers = 0; // Empty the helicopter
            UpdateUI();
        }
        // Hit a tree (Game Over)
        else if (other.CompareTag("Tree"))
        {
            Debug.Log("Game Over!");
            gameOverText.gameObject.SetActive(true);
            Invoke("ResetGame", 2f);
        }
    }

    private void UpdateUI()
    {
        soldiersInHelicopterText.text = "Soldiers in Helicopter: " + currentSoldiers;
        soldiersRescuedText.text = "Soldiers Rescued: " + gameManager.GetRescuedSoldiers();
    }

    private void ResetGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
