using UnityEngine;

namespace Astral.Variables
{
    
    [CreateAssetMenu(menuName = "Astral/Variables/Create Game Variable", fileName = "Game  Variable", order = 0)]
    public class GameVariable : Variable<int>
    {
        public int level;
        public int cost;
        [Range(0,100)]
        public float increaseValue;
        
        public void Increase()
        {
            level++;
            value++;
            cost =  (int)(cost*increaseValue);
        }
    }
}
