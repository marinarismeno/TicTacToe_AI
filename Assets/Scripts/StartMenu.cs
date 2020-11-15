using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartMenu : MonoBehaviour
{
    public GameObject gameController_go;

    private void OnEnable()
    {
        this.gameObject.transform.GetChild(0).GetComponent<Single_multi_Script>().ShowSingleMultiPanel(true);
    }
    public void StartPanelEnable(bool enable)
    {
        this.gameObject.SetActive(enable);
        gameController_go.GetComponent<GameControllerScript>().NewGame();
    }
    public void Manually()
    {
        gameController_go.GetComponent<GameControllerScript>().AI_game = 0;
        StartPanelEnable(false);  
    }

    public void AI(int difficulty)
    {
        gameController_go.GetComponent<GameControllerScript>().AI_game = difficulty;
        //StartPanelEnable(false);
    }

    public void ExitGame()
    {
        Debug.Log("Exit");
        Application.Quit();
    }

}
