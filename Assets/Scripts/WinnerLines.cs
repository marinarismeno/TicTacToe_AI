using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinnerLines : MonoBehaviour
{
    public GameObject[] lines;
    private void Start()
    {
        //ResetWinnerLines();
    }
    // Start is called before the first frame update
    public void ShowWinnerLine(int index)
    {
        lines[index].SetActive(true); // show the winner line
    }

    public void ResetWinnerLines()
    {
        for (int i = 0; i < lines.Length; i++)
        {
            lines[i].SetActive(false); // hide all lines
        }
    }
}
