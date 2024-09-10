using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class GameManager : MonoBehaviour
{
    public TextMeshProUGUI goldText;
    public int curGold;

    // Start is called before the first frame update
    void Start()
    {
        //Load the amount of "Gold" from PlayerPrefs and set it to be "curGold".
        curGold = PlayerPrefs.GetInt("Gold");
        //Call the function
        UpdateGoldText();
    }
    //Update the goldText to display the current amount of gold that we have.
    void UpdateGoldText()
    {
        goldText.text = "Gold: " + curGold.ToString();
    }

    public void AddGold(int amount)
    {
        //Add amount to curGold.
        curGold += amount;
        //Save our curGold to the PlayerPrefs "Gold".
        PlayerPrefs.GetInt("Gold", curGold);
        UpdateGoldText();
    }
}
