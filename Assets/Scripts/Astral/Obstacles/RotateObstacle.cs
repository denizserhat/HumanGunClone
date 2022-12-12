using System;
using UnityEngine;

namespace Astral.Obstacles
{
    public class RotateObstacle : MonoBehaviour
    {

        [SerializeField] private float rotateSpeed;


        private void Update()
        {
            transform.Rotate(new Vector3(0,10,0) * (rotateSpeed * Time.deltaTime));
        }
    }
}
