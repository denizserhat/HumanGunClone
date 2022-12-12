using System;
using UnityEngine;

namespace Astral.Utility
{
    public static  class EventManager
    {
        public static event Action<Vector3,int> onUpgradeWeapon;

        public static void OnUpgradeWeapon(Vector3  humanPos,int count = 1) => onUpgradeWeapon?.Invoke(humanPos,count);
        
        public static event Action<int> onDropHuman;

        public static void OnDropHuman(int count = 1) => onDropHuman?.Invoke(count);

        public static event Action<bool> onFinishDropHuman;

        public static void OnFinishDropHuman(bool isFinish) => onFinishDropHuman?.Invoke(isFinish);

        public static event Action onWin;
        public static void OnWin() => onWin.Invoke();
        
        public static event Action onLose;
        public static void OnLose() => onLose.Invoke();
        
        public static event Action<Vector3> onMoneyCollect;
        public static void OnMoneyCollect(Vector3 startPos) => onMoneyCollect.Invoke(startPos);
        
        public static event Action onCloseTutorial;
        public static void OnCloseTutorial() => onCloseTutorial.Invoke();



    }
}