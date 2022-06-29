using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class CanvasVisualizer : MonoBehaviour
{
    public Button speedUpgradeButton;
    public Button brakeUpgradeButton;
    public Button incomeUpgradeButton;


    void Start()
    {
        StartCoroutine(ScaleButtons());
    }

    
    void Update()
    {
        
    }


    IEnumerator ScaleButtons()
    {
        while (true)
        {
            yield return new WaitForSecondsRealtime(1f);

            if (speedUpgradeButton.interactable)
            {
                speedUpgradeButton.transform.DOScale(new Vector3(2.2f, 17.6f, 1), 0.1f).SetEase(Ease.Linear).SetLoops(2, LoopType.Yoyo);
            }

            

            yield return new WaitForSecondsRealtime(1f);

            if (brakeUpgradeButton.interactable)
            {
                brakeUpgradeButton.transform.DOScale(new Vector3(2.2f, 17.6f, 1), 0.1f).SetEase(Ease.Linear).SetLoops(2, LoopType.Yoyo);
            }
            

            yield return new WaitForSecondsRealtime(1f);

            if (incomeUpgradeButton.interactable)
            {
                incomeUpgradeButton.transform.DOScale(new Vector3(2.2f, 17.6f, 1), 0.1f).SetEase(Ease.Linear).SetLoops(2, LoopType.Yoyo);
            }
            
        }
        
    }
}
