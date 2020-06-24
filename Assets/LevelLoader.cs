using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public GameObject levelSelection;
    public void LevelSelection()
    {
        
        levelSelection.SetActive(true);
    }

    public void Play()
    {
        SceneManager.LoadScene("Game1");
    }

    public void Level1()
    {
        SceneManager.LoadScene("Game1");
    }

    public void Level2()
    {
        SceneManager.LoadScene("Game2");
    }

    public void Level3()
    {
        SceneManager.LoadScene("Game3");
    }
}
