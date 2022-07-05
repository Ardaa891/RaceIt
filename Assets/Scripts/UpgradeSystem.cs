using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UpgradeSystem : MonoBehaviour
{
    public static UpgradeSystem Current;

    public Button speedUpgradeButton;
    
    public Button incomeUpgradeButton;

    public TextMeshProUGUI speedUpgradeLevelText, speedUpgradePriceText;
    //public TextMeshProUGUI earnedMoneyText;
    public TextMeshProUGUI incomeUpgradeLevelText, incomeUpgradePriceText;
    public TextMeshProUGUI moneyText;

    public float speedUpgradePrice;
    
    public float incomeUpgradePrice;

    public int maxSpeed;

    




    
    void Start()
    {
        Current = this;
        CheckPrefs();
        CheckButtons();
        //PlayerPrefs.SetInt("Money", 1000);
        //PlayerPrefs.SetInt("SpeedUpgradeLevel", 1);
        //PlayerPrefs.SetInt("IncomeUpgradeLevel", 1);

        //PlayerPrefs.SetInt("EarnedMoney", 50);

        if (PlayerPrefs.GetInt("MaxSpeed") <= 0)
        {
            PlayerPrefs.SetInt("MaxSpeed", 150);
        }
        else
        {
            PlayerPrefs.SetInt("MaxSpeed", PlayerPrefs.GetInt("MaxSpeed"));
        }

        if(PlayerPrefs.GetInt("AIMaxSpeed") <= 0)
        {
            PlayerPrefs.SetInt("AIMaxSpeed", 140);
        }
        else
        {
            PlayerPrefs.SetInt("AIMaxSpeed", PlayerPrefs.GetInt("AIMaxSpeed"));
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


        if(PlayerPrefs.GetInt("IncomeUpgradePrice") <= 0)
        {
            PlayerPrefs.SetInt("IncomeUpgradePrice", 50);
        }

        if(PlayerPrefs.GetInt("EarnedMoney") <= 0)
        {
            PlayerPrefs.SetInt("EarnedMoney", 50);
        }
        else
        {
            PlayerPrefs.SetInt("EarnedMoney", PlayerPrefs.GetInt("EarnedMoney"));
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

       

        if(PlayerPrefs.GetInt("Money") >= PlayerPrefs.GetInt("IncomeUpgradePrice"))
        {
            incomeUpgradeButton.interactable = true;
        }
        else
        {
            incomeUpgradeButton.interactable = false;
        }

        speedUpgradePriceText.text = "$ " + PlayerPrefs.GetInt("SpeedUpgradePrice");
        
        incomeUpgradePriceText.text = "$ " + PlayerPrefs.GetInt("IncomeUpgradePrice");

        speedUpgradeLevelText.text = PlayerPrefs.GetInt("SpeedUpgradeLevel").ToString();
        
        incomeUpgradeLevelText.text = PlayerPrefs.GetInt("IncomeUpgradeLevel").ToString();

        moneyText.text = "$ " + PlayerPrefs.GetInt("Money");

    }

    public void BuySpeedUpgrade()
    {
        if(PlayerPrefs.GetInt("Money") >= PlayerPrefs.GetInt("SpeedUpgradePrice"))
        {
            
            PlayerPrefs.SetInt("Money", PlayerPrefs.GetInt("Money") - PlayerPrefs.GetInt("SpeedUpgradePrice"));
            PlayerPrefs.SetInt("SpeedUpgradeLevel", PlayerPrefs.GetInt("SpeedUpgradeLevel") + 1);
            PlayerPrefs.SetInt("SpeedUpgradePrice", PlayerPrefs.GetInt("SpeedUpgradePrice") + 50);
            PlayerPrefs.SetInt("MaxSpeed", PlayerPrefs.GetInt("MaxSpeed") + 10);
            PlayerPrefs.SetInt("AIMaxSpeed", PlayerPrefs.GetInt("AIMaxSpeed") + 7);
            
        }

        CheckButtons();
    }

    

    public void BuyIncomeUpgrade()
    {
        if(PlayerPrefs.GetInt("Money") >= PlayerPrefs.GetInt("IncomeUpgradePrice"))
        {
            int randomEarnedMoney = Random.Range(50, 101);

            PlayerPrefs.SetInt("Money", PlayerPrefs.GetInt("Money") - PlayerPrefs.GetInt("IncomeUpgradePrice"));
            PlayerPrefs.SetInt("IncomeUpgradeLevel", PlayerPrefs.GetInt("IncomeUpgradeLevel") + 1);
            PlayerPrefs.SetInt("IncomeUpgradePrice", PlayerPrefs.GetInt("IncomeUpgradePrice") + 50);
            //int randomEarnedMoney = Random.Range(50, 101);
            PlayerPrefs.SetInt("EarnedMoney", PlayerPrefs.GetInt("EarnedMoney") + 50);
        }

        CheckButtons();
    }
}
