using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Astral.Utility
{
    public class UIManager : MonoBehaviour
    {
        private GameManager _manager;
        private LevelManager _levelManager;
        
        public GameObject winLayout,loseLayout;

        [SerializeField] private Image goldImage;
        [SerializeField] private GameObject goldImagePrefab;
        [SerializeField] private GameObject tutorial;
        [SerializeField] private TextMeshProUGUI levelCount;
        [SerializeField] private TextMeshProUGUI moneyCountText;
        [SerializeField] private TextMeshProUGUI winMoneyCountText;
        [SerializeField] private GameObject gameStartButton;


        private List<GameUpgrade> _gameUpgrades;

        private void OnEnable()
        {
            EventManager.onWin += Win;
            EventManager.onLose += Lose;
            EventManager.onMoneyCollect += OnMoneyCollect;
        }
        private void OnDisable()
        {
            EventManager.onWin -= Win;
            EventManager.onLose -= Lose;
            EventManager.onMoneyCollect -= OnMoneyCollect;
        }

        private void Start()
        {
            _manager = FindObjectOfType<GameManager>();
            _levelManager = FindObjectOfType<LevelManager>();
            levelCount.text = $"LEVEL {_levelManager.CurrentUILevel+1}";
            moneyCountText.text = "$"+_manager.money.value;
            _gameUpgrades = GetComponentsInChildren<GameUpgrade>(true).ToList();
            InitUpgrade();
        }


        public void GameStart()
        {
            _manager.state = State.Started;
            CloseTutorial();
        }

        private async void OnMoneyCollect(Vector3 startPos)
        {
            
            for (int i = 0; i < 4; i++)
            { 
                await UniTask.Delay(50);
             var animRect = Instantiate(goldImagePrefab,transform);
             animRect.gameObject.SetActive(true);
             animRect.transform.position = startPos;
             animRect.transform.DOMove(goldImage.transform.position, 1)
                 .OnComplete(() =>
                 {
                     Destroy(animRect);
                 });      
            }
            moneyCountText.text = "$"+_manager.money.value;

        }

        private  async void Lose()
        {
            _manager.state = State.Lose;
            await UniTask.Delay(1000);
            loseLayout.SetActive(true);
        }

        private async void Win()
        {
            _manager.state = State.Win;
            await UniTask.Delay(1000);
            winLayout.SetActive(true);
            winMoneyCountText.text = "$"+_manager.money.value;

        }

        private void CloseTutorial()
        {
            tutorial.SetActive(false);
            gameStartButton.SetActive(false);
        }

        public void LoadNextLevel()
        {
            _levelManager.LoadNextLevel();
        }

        public void RestartLevel()
        {
            _levelManager.RestartLevel();
        }

        void InitUpgrade()
        {
            moneyCountText.text = "$"+_manager.money.value;
            foreach (var upgrade in _gameUpgrades)
            {
                upgrade.UpdateUI();
            }

        }

        public void IncreaseHuman()
        {
            if (_manager.money.value>=_manager.humanCount.cost)
            {
                _manager.money.value -= _manager.humanCount.cost;
                _manager.humanCount.Increase();
                EventManager.OnUpgradeWeapon(Vector3.zero);
                InitUpgrade(); 
            }
        }
        public void IncreaseDamage()
        {
            if (_manager.money.value>=_manager.damage.cost)
            {
                _manager.money.value -= _manager.damage.cost;
                _manager.damage.Increase();
                InitUpgrade(); 
            }
        }
    }
}