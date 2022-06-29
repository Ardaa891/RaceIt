using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Barriers : MonoBehaviour
{

    
   
    void Start()
    {
        Sequence seq = DOTween.Sequence();

        seq.Append(transform.DOLocalMoveY(2, 1).SetEase(Ease.Linear).SetDelay(3f));
        seq.Join(transform.DOLocalMoveY(-2.33f, 1).SetEase(Ease.Linear).SetDelay(3f));
        seq.SetLoops(-1, LoopType.Restart);
    }

    
    void Update()
    {
        
    }
}
