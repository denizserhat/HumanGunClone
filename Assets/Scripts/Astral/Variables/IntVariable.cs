using UnityEngine;

namespace Astral.Variables
{
    [CreateAssetMenu(menuName = "Astral/Variables/Create Int Variable", fileName = "IntVariable", order = 0)]
    public class IntVariable : Variable<int>
    {
        public void Increase(int count)
        {
            value += count;
        }
    }
}
