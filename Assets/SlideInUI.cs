using System;
using UnityEngine;

public class SlideInUI : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    internal void SlideIn(float slideTime = 1f)
    {
        this.gameObject.SetActive(true);
        this.gameObject.transform.localPosition = new Vector3(0, 400, 0);
        LeanTween.moveLocal(this.gameObject, new Vector3(0, 0, 0), slideTime).setEase(LeanTweenType.easeOutCubic);
    }

    internal void SlideOut(float slideTime = 1f)
    {
        LeanTween.moveLocal(this.gameObject, new Vector3(0, 400, 0), slideTime).setEase(LeanTweenType.easeOutCubic).setOnComplete(() => {
            this.gameObject.SetActive(false);
        });
    }
}
