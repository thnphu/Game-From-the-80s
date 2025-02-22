using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HelicopterController : MonoBehaviour
{
    public float speed = 5f;
    public float boostSpeed = 8f;
    public int maxCapacity = 3;
    private int currentSoldiers = 0;

    public Text soldiersInHelicopterText;
    public Text soldiersRescuedText;
    public Text totalSoldiersText;
    public Text gameOverText;
    public Text winText;

    public AudioClip pickupSound;
    private AudioSource audioSource;

    private GameManager gameManager;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        gameManager = FindObjectOfType<GameManager>(); 

        UpdateUI();
        winText.gameObject.SetActive(false);
        gameOverText.gameObject.SetActive(false);
    }

    void Update()
    {
        if (gameOverText.gameObject.activeSelf || winText.gameObject.activeSelf)
        {
            return; 
        }

        float currentSpeed = Input.GetKey(KeyCode.LeftShift) ? boostSpeed : speed;

        float moveX = Input.GetAxis("Horizontal") * currentSpeed * Time.deltaTime;
        float moveY = Input.GetAxis("Vertical") * currentSpeed * Time.deltaTime;
        transform.Translate(new Vector3(moveX, moveY, 0));

        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Picking up soldiers
        if (other.CompareTag("Soldier") && currentSoldiers < maxCapacity)
        {
            Destroy(other.gameObject); 
            currentSoldiers++; 
            audioSource.PlayOneShot(pickupSound); 
            UpdateUI();
        }
        // Dropping off soldiers at hospital
        else if (other.CompareTag("Hospital") && currentSoldiers > 0)
        {
            gameManager.SoldierRescued(currentSoldiers);
            currentSoldiers = 0;
            UpdateUI();

            // Check if all soldiers are rescued
            if (gameManager.GetRescuedSoldiers() >= gameManager.GetTotalSoldiers())
            {
                WinGame();
            }
        }
        // Hitting a tree (Game Over)
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
        totalSoldiersText.text = "Total Soldiers: " + gameManager.GetTotalSoldiers();
    }

    private void WinGame()
    {
        winText.gameObject.SetActive(true);
        Debug.Log("You Win!");
        Invoke("ResetGame", 3f);
    }

    private void ResetGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
