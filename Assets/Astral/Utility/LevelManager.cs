using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Astral.Utility
{
    public class LevelManager : MonoBehaviour
    {

        private bool _isCompletedLevels;
        
        public int CurrentLevel
        {
            get => _currentLevel;
            private set
            {
                _currentLevel = value;
                PlayerPrefs.SetInt("Level", _currentLevel);
            }
        }
        
        public int CurrentUILevel
        {
            get => _currentUILevel;
            set
            {
                _currentUILevel = value;
                PlayerPrefs.SetInt("LevelUI", _currentUILevel);
            }
        }
        private int _currentLevel;
        private int _currentUILevel;
        private int _previousLevel;

        private void Awake()
        {
            Initialize();
        }

        void Initialize()
        {
            _currentLevel = PlayerPrefs.GetInt("Level");
            _currentUILevel = PlayerPrefs.GetInt("LevelUI");
        }

        
        public  void LoadNextLevel()
        {
            CurrentUILevel++;
            if (CurrentLevel >= SceneManager.sceneCountInBuildSettings)CurrentLevel = 1;
            else CurrentLevel++;
            
            SceneManager.LoadScene(CurrentLevel);

        }

        public void RestartLevel()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}