using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DifficultyPanelScript : MonoBehaviour
{
    private void Start()
    {
        ShowDifficultyPanel(false);
    }
    public void ShowDifficultyPanel(bool enable)
    {
        this.gameObject.SetActive(enable);
    }
}
