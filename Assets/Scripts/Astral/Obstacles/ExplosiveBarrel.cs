using System;
using UnityEngine;

namespace Astral.Obstacles
{
    
    
    public class ExplosiveBarrel : BaseObstacle
    {
        [SerializeField] private GameObject explosionPrefab;
        [SerializeField] private LayerMask explosiveObjectLayer;
        [SerializeField] private int explosionDamage;
        [SerializeField] private float explosiveRadius;
        private Collider[] _nearObstacle;



        private void Explode()
        {
            _nearObstacle = Physics.OverlapSphere(transform.position, explosiveRadius,explosiveObjectLayer);
            var effect = Instantiate(explosionPrefab);
            effect.transform.position = transform.position;
            foreach (var obstacle in _nearObstacle)
            {
                if (obstacle.TryGetComponent<Stone>(out var stone))
                {
                    stone.TakeDamage(explosionDamage);
                }
            }
        }

        public override void Kill()
        {
            Explode();
            Destroy(gameObject);
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawWireSphere(transform.position,explosiveRadius);
        }
    }
}