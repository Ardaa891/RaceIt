using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LevelText : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<TextMeshProUGUI>().text = ("lvl " + (PlayerPrefs.GetInt("level") + 1));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
