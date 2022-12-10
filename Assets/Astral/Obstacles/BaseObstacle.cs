using DG.Tweening;
using TMPro;
using UnityEngine;

namespace Astral.Obstacles
{
    public class BaseObstacle : MonoBehaviour ,IDamageable
    {
        public int health;
        public TextMeshProUGUI healthCountText;


        private void Start()
        {
            healthCountText = GetComponentInChildren<TextMeshProUGUI>();
            healthCountText.text = health.ToString();
        }

        public void TakeDamage(int count)
        {
            transform.DOPunchScale(Vector3.one/2, .1f);
            if (health<=count)
            {
                Kill();
            }
            else
            {
                health -= count;
                healthCountText.text = health.ToString();
            }
        }
        public virtual void Kill()
        {
            Destroy(gameObject);
        }
    }
}
