using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShowResult : MonoBehaviour
{
    public TextMeshProUGUI winnerText;
    public void ShowWinner(int result)
    {
        if (result == 0)
            winnerText.text = "Xs Won!";
        else if (result == 1)
            winnerText.text = "Os Won!";
        else
            winnerText.text = "It's a tie!";
    }
}
