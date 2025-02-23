using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    public int totalSoldiers;
    public GameObject soldierPrefab;
    public Transform spawnArea;

    private int rescuedSoldiers = 0;

    public Text soldiersRescuedText;
    public Text winText;

    void Start()
    {
        winText.gameObject.SetActive(false);
        SpawnSoldiers();
        UpdateUI();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("StartMenu");
        }
    }

    void SpawnSoldiers()
    {
        for (int i = 0; i < totalSoldiers; i++)
        {
            Vector2 randomPosition = new Vector2(
                Random.Range(spawnArea.position.x - spawnArea.localScale.x / 2, spawnArea.position.x + spawnArea.localScale.x / 2),
                Random.Range(spawnArea.position.y - spawnArea.localScale.y / 2, spawnArea.position.y + spawnArea.localScale.y / 2)
            );

            Instantiate(soldierPrefab, randomPosition, Quaternion.identity);
        }
    }

    public void SoldierRescued(int rescuedSoldiersCount)
    {
        rescuedSoldiers += rescuedSoldiersCount;
        UpdateUI();
        CheckWinCondition();
    }

    void CheckWinCondition()
    {
        if (rescuedSoldiers >= totalSoldiers)
        {
            winText.gameObject.SetActive(true);
            Debug.Log("You Win!");
            Invoke("RestartGame", 3f);
        }
    }

    void UpdateUI()
    {
        soldiersRescuedText.text = "Soldiers Rescued: " + rescuedSoldiers + "/" + totalSoldiers;
    }

    void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    
    public int GetRescuedSoldiers()
    {
        return rescuedSoldiers;
    }

    public int GetTotalSoldiers()
    {
        return totalSoldiers;
    }
}