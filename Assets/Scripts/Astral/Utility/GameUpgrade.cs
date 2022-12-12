using Astral.Variables;
using TMPro;
using UnityEngine;

namespace Astral.Utility
{
    public class GameUpgrade : MonoBehaviour
    {
        public GameVariable variable;
        public TextMeshProUGUI levelCountText;
        public TextMeshProUGUI costText;



        public void UpdateUI()
        {
            levelCountText.text = $"LEVEL{variable.level}";
            costText.text = $"${variable.cost}";
        }

    }
}
