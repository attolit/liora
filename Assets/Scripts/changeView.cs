using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class changeView : MonoBehaviour
{
    public Sprite[] backgrounds; //deleye when change view.
    public SpriteRenderer backgroundRenderer;
    private int currentBackgroundIndex = 0;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowNextBackground()
    {
        currentBackgroundIndex++; //haikei susumu

        if (currentBackgroundIndex >= backgrounds.Length)
        {
            currentBackgroundIndex = 0;
        }

        backgroundRenderer.sprite = backgrounds[currentBackgroundIndex];
    }

    public void ShowPreviousBackground()
    {
        currentBackgroundIndex--; //haikei modosu
        if(currentBackgroundIndex < 0)
        {
            currentBackgroundIndex = backgrounds.Length - 1;
        }
        backgroundRenderer.sprite = backgrounds[currentBackgroundIndex];
    }
}
