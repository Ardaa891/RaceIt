using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using Dreamteck.Splines;
using MoreMountains.NiceVibrations;
using ElephantSDK;

public class LevelController : MonoBehaviour
{
    public static LevelController Current;
    public List<GameObject> levels = new List<GameObject>();
    public GameObject winGameOverMenu, failGameOverMenu, levelStartMenu, inGameMenu;
    public bool gameActive = false;
    public bool levelEnd = false;

    public TextMeshProUGUI earnedMoneyText;
    public float earnedMoney;


    [Space]
    [Space]
    public GameObject CurrentLevel;
    public bool isTesting = false;
    // Start is called before the first frame update
    void Awake()
    {
        Current = this;
        Screen.sleepTimeout = 0;
        
        if (isTesting == false)
        {

            if (levels.Count == 0)
            {

                foreach (Transform level in transform)
                {
                    levels.Add(level.gameObject);
                }
            }


            CurrentLevel = levels[PlayerPrefs.GetInt("level") % levels.Count];
            levels[PlayerPrefs.GetInt("level") % levels.Count].SetActive(true);
        }
        else
        {
            CurrentLevel.SetActive(true);
        }
    }

    public void NextLevel()
    {
        //StartCoroutine(LevelUp());
        if ((levels.IndexOf(CurrentLevel) + 1) == levels.Count)
        {




            PlayerPrefs.SetInt("level", PlayerPrefs.GetInt("level") + 1);



            //  GameHandler.Instance.Appear_TransitionPanel();


            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);


        }


        else
        {
            CurrentLevel = levels[(PlayerPrefs.GetInt("level") + 1) % levels.Count];



            levels[(PlayerPrefs.GetInt("level")) % levels.Count].SetActive(false);


            PlayerPrefs.SetInt("level", PlayerPrefs.GetInt("level") + 1);

            levels[PlayerPrefs.GetInt("level") % levels.Count].SetActive(true);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

        }

        PlayerPrefs.SetInt("Money", PlayerPrefs.GetInt("Money") + PlayerPrefs.GetInt("EarnedMoney"));
        

    }

    public void StartLevel()
    {
        Elephant.LevelStarted(PlayerPrefs.GetInt("level") + 1);
        gameActive = true;
        levelStartMenu.gameObject.SetActive(false);
        inGameMenu.gameObject.SetActive(true);
        Debug.Log("start");
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

  

    public void GameOver()
    {
        Elephant.LevelFailed(PlayerPrefs.GetInt("level") + 1);
        gameActive = false;
        inGameMenu.SetActive(false);
        failGameOverMenu.SetActive(true);
        GameObject.FindGameObjectWithTag("Player").GetComponent<SplineFollower>().followSpeed = 0;
        //GameObject.FindGameObjectWithTag("AI").GetComponent<Rigidbody>().velocity = Vector3.zero;
        earnedMoneyText.text = "$ " + "0";
        MMVibrationManager.Haptic(HapticTypes.Failure);

    }

    public void WinGameOver()
    {
        Elephant.LevelCompleted(PlayerPrefs.GetInt("level") + 1);
        gameActive = false;
        inGameMenu.SetActive(false);
        winGameOverMenu.SetActive(true);
        GameObject.FindGameObjectWithTag("Player").GetComponent<SplineFollower>().followSpeed = 0;
        //GameObject.FindGameObjectWithTag("AI").GetComponent<Rigidbody>().velocity = Vector3.zero;
        Debug.Log("win");
        MMVibrationManager.Haptic(HapticTypes.Success);
        earnedMoneyText.text = "$ " + PlayerPrefs.GetInt("EarnedMoney");
    }

}
