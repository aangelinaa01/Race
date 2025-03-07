using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour
{
    public GameObject panelSettings;
    private void Start()
    {
        panelSettings.SetActive(false);
    }
    public void OpenSettings()
    {
        panelSettings.SetActive(true);
    }
    public void CloseSettings()
    {
        panelSettings.SetActive(false);
    }
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.LoadLevel("menu");
        }
    }

    public void Play()
    {
        Application.LoadLevel("Choose");
    }
    public void PlayGameGT()
    {
        Application.LoadLevel("Track_gt");
    }
    public void PlayGameF1()
    {
        Application.LoadLevel("Track_f1");
    }
    public void Menu1()
    {
        Application.LoadLevel("menu");
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}
 
