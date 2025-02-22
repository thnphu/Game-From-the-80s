using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int totalSoldiers; 
    public GameObject soldierPrefab; 
    public Transform spawnArea; 

    private int rescuedSoldiers = 0;

    void Start()
    {
        SpawnSoldiers();
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
        CheckWinCondition();
    }

    void CheckWinCondition()
    {
        if (rescuedSoldiers >= totalSoldiers)
        {
            Debug.Log("You Win!");
        }
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