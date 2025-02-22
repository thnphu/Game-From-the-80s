using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public int TotalSoldiers { get; private set; }
    public int RescuedSoldiers { get; private set; }

    public Text soldiersInHelicopterText;
    public Text soldiersRescuedText;

    void Start()
    {
        // Find all soldiers with the "Soldier" tag and count them
        GameObject[] soldiers = GameObject.FindGameObjectsWithTag("Soldier");
        TotalSoldiers = soldiers.Length;
        RescuedSoldiers = 0;
    }

    public void SoldierRescued()
    {
        RescuedSoldiers++;
        UpdateUI();
    }

    private void UpdateUI()
    {
        soldiersInHelicopterText.text = "Soldiers in Helicopter: " + (TotalSoldiers - RescuedSoldiers);
        soldiersRescuedText.text = "Soldiers Rescued: " + RescuedSoldiers;

        if (RescuedSoldiers >= TotalSoldiers)
        {
            Debug.Log("You Win!");
        }
    }
}