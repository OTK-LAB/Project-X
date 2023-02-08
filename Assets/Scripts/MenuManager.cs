using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public static MenuManager MenuManagerInstance;
    public bool GameState;
    public GameObject menuElement;
    void Start()
    {
        GameState = false;
        MenuManagerInstance = this;
    }

    
    void Update()
    {
        
    }

    public void StartTheGame()
    {
        GameState = true;
        menuElement.SetActive(false);
    }
}
