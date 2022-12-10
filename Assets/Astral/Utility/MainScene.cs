using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainScene : MonoBehaviour
{
    private int _level;
    public int CurrentLevel
    {
        get => _level;
        private set
        {
            _level = value;
            PlayerPrefs.SetInt("Level", _level);
        }
    }
    private void Awake()
    {
        InitScene();
    }

    void InitScene()
    {

        if (CurrentLevel <= 0)
        {
            PlayerPrefs.SetInt("Level", 1);
            _level = PlayerPrefs.GetInt("Level");
            SceneManager.LoadScene(CurrentLevel);
        }
        else SceneManager.LoadScene(CurrentLevel);

    }
}
