using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UpgradeSystem : MonoBehaviour
{
    public static UpgradeSystem Current;

    public Button speedUpgradeButton;
    public Button brakeUpgradeButton;
    public Button incomeUpgradeButton;

    public TextMeshProUGUI speedUpgradeLevelText, speedUpgradePriceText;
    public TextMeshProUGUI brakeUpgradeLevelText, brakeUpgradePriceText;
    public TextMeshProUGUI incomeUpgradeLevelText, incomeUpgradePriceText;
    public TextMeshProUGUI moneyText;

    public float speedUpgradePrice;
    public float brakeUpgradePrice;
    public float incomeUpgradePrice;

    public int maxSpeed;

    




    
    void Start()
    {
        Current = this;
        CheckPrefs();
        CheckButtons();
        //PlayerPrefs.SetInt("Money", 1000);
        
        if(PlayerPrefs.GetInt("MaxSpeed") <= 0)
        {
            PlayerPrefs.SetInt("MaxSpeed", 10);
        }
        else
        {
            PlayerPrefs.SetInt("MaxSpeed", PlayerPrefs.GetInt("MaxSpeed"));
        }



    }

    
    void Update()
    {
        moneyText.text = "$ " + PlayerPrefs.GetInt("Money");
        CheckButtons();
     
    }

    void CheckPrefs()
    {
        if(PlayerPrefs.GetInt("SpeedUpgradePrice") <= 0)
        {
            PlayerPrefs.SetInt("SpeedUpgradePrice", 50);
        }

        if(PlayerPrefs.GetInt("BrakeUpgradePrice") <= 0)
        {
            PlayerPrefs.SetInt("BrakeUpgradePrice", 50);
        }

        if(PlayerPrefs.GetInt("IncomeUpgradePrice") <= 0)
        {
            PlayerPrefs.SetInt("IncomeUpgradePrice", 50);
        }


       
    }

    void CheckButtons()
    {
        if(PlayerPrefs.GetInt("Money") >= PlayerPrefs.GetInt("SpeedUpgradePrice"))
        {
            speedUpgradeButton.interactable = true;
        }
        else
        {
            speedUpgradeButton.interactable = false;
        }

        if(PlayerPrefs.GetInt("Money") >= PlayerPrefs.GetInt("BrakeUpgradePrice"))
        {
            brakeUpgradeButton.interactable = true;
        }
        else
        {
            brakeUpgradeButton.interactable = false;
        }

        if(PlayerPrefs.GetInt("Money") >= PlayerPrefs.GetInt("IncomeUpgradePrice"))
        {
            incomeUpgradeButton.interactable = true;
        }
        else
        {
            incomeUpgradeButton.interactable = false;
        }

        speedUpgradePriceText.text = "$ " + PlayerPrefs.GetInt("SpeedUpgradePrice");
        brakeUpgradePriceText.text = "$ " + PlayerPrefs.GetInt("BrakeUpgradePrice");
        incomeUpgradePriceText.text = "$ " + PlayerPrefs.GetInt("IncomeUpgradePrice");

        speedUpgradeLevelText.text = "lvl " + PlayerPrefs.GetInt("SpeedUpgradeLevel");
        brakeUpgradeLevelText.text = "lvl " + PlayerPrefs.GetInt("BrakeUpgradeLevel");
        incomeUpgradeLevelText.text = "lvl " + PlayerPrefs.GetInt("IncomeUpgradeLevel");

        moneyText.text = "$ " + PlayerPrefs.GetInt("Money");

    }

    public void BuySpeedUpgrade()
    {
        if(PlayerPrefs.GetInt("Money") >= PlayerPrefs.GetInt("SpeedUpgradePrice"))
        {
            
            PlayerPrefs.SetInt("Money", PlayerPrefs.GetInt("Money") - PlayerPrefs.GetInt("SpeedUpgradePrice"));
            PlayerPrefs.SetInt("SpeedUpgradeLevel", PlayerPrefs.GetInt("SpeedUpgradeLevel") + 1);
            PlayerPrefs.SetInt("SpeedUpgradePrice", PlayerPrefs.GetInt("SpeedUpgradePrice") + 50);
            PlayerPrefs.SetInt("MaxSpeed", PlayerPrefs.GetInt("MaxSpeed") + 2);
            
        }

        CheckButtons();
    }

    public void BuyBrakeUpgrade()
    {
        if(PlayerPrefs.GetInt("Money") >= PlayerPrefs.GetInt("BrakeUpgradePrice"))
        {
            PlayerPrefs.SetInt("Money", PlayerPrefs.GetInt("Money") - PlayerPrefs.GetInt("BrakeUpgradePrice"));
            PlayerPrefs.SetInt("BrakeUpgradeLevel", PlayerPrefs.GetInt("BrakeUpgradeLevel") + 1);
            PlayerPrefs.SetInt("BrakeUpgradePrice", PlayerPrefs.GetInt("BrakeUpgradePrice") + 50);
        }
        CheckButtons();
    }

    public void BuyIncomeUpgrade()
    {
        if(PlayerPrefs.GetInt("Money") >= PlayerPrefs.GetInt("IncomeUpgradePrice"))
        {
            PlayerPrefs.SetInt("Money", PlayerPrefs.GetInt("Money") - PlayerPrefs.GetInt("IncomeUpgradePrice"));
            PlayerPrefs.SetInt("IncomeUpgradeLevel", PlayerPrefs.GetInt("IncomeUpgradeLevel") + 1);
            PlayerPrefs.SetInt("IncomeUpgradePrice", PlayerPrefs.GetInt("IncomeUpgradePrice") + 50);
        }

        CheckButtons();
    }
}
