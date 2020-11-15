using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSelectScript : MonoBehaviour
{
    public Button xoButton;
    public Image xImage;
    public Image oImage;
    public bool humansTurn = true;

    
    public void buttonClicked()
    {
        
        xImage.gameObject.SetActive(true);
        xoButton.interactable = false;
        humansTurn = false; // toggle whose turn it is
                            // set a value on the board Array

    }

    public void AIplayed()
    {
        oImage.gameObject.SetActive(true);
        xoButton.interactable = false;
    }

}
