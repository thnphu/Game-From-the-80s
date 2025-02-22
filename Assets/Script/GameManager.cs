using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private int totalSoldiers;
    private int rescuedSoldiers = 0;

    public Text soldiersInHelicopterText;
    public Text soldiersRescuedText;

    void Start()
    {
        GameObject[] soldiers = GameObject.FindGameObjectsWithTag("Soldier");
        totalSoldiers = soldiers.Length; // Count total soldiers in the game
    }

    // This method is called when a soldier is picked up
    public void SoldierRescued()
    {
        rescuedSoldiers++;
        UpdateUI();
    }

    // Get the total number of soldiers
    public int GetTotalSoldiers()
    {
        return totalSoldiers;
    }

    // Get the number of rescued soldiers
    public int GetRescuedSoldiers()
    {
        return rescuedSoldiers;
    }

    // Update the UI
    private void UpdateUI()
    {
        soldiersInHelicopterText.text = "Soldiers in Helicopter: " + (totalSoldiers - rescuedSoldiers);
        soldiersRescuedText.text = "Soldiers Rescued: " + rescuedSoldiers;

        if (rescuedSoldiers >= totalSoldiers)
        {
            Debug.Log("You Win!");
        }
    }
}