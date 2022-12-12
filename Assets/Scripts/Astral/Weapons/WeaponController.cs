using System;
using Astral.Door;
using Astral.Obstacles;
using Astral.Utility;
using UnityEngine;

namespace Astral.Weapons
{
    public class WeaponController : MonoBehaviour
    {
        [SerializeField] private float speed,horSpeed;
        private GameManager _manager;
        private Joystick _joystick;
        private Camera _mainCamera;
        private void Start()
        {
            _manager = FindObjectOfType<GameManager>();
            _joystick = FindObjectOfType<Joystick>();
            _mainCamera = Camera.main;
        }


        private void Update()
        {
            if (_manager.state is State.Started or State.Finished)
            {
                
                transform.Translate(new Vector3(_joystick.Horizontal*horSpeed*Time.deltaTime,0,speed*Time.deltaTime));
                var pos = transform.position;
                pos.x =  Mathf.Clamp(transform.position.x, -1.5f, 1.5f);
                transform.position = pos;
            }
        }
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.transform.CompareTag("Human"))
            {
                EventManager.OnUpgradeWeapon(other.bounds.center);
                Destroy(other.gameObject);
            }
            if (other.TryGetComponent<BaseObstacle>(out var obstacle))
            {

                if (_manager.state != State.Finished)
                {
                    EventManager.OnDropHuman();
                    other.tag = "Hit";
                    obstacle.Kill();
                }
                else
                {
                    EventManager.OnFinishDropHuman(true);
                }
                
            }
            if (other.TryGetComponent<Gate>(out var gate))
            {
                gate.Execute();
                other.transform.tag = "Hit";
            }
            if (other.transform.CompareTag("Money"))
            {
                _manager.money.value++;
                EventManager.OnMoneyCollect(_mainCamera.WorldToScreenPoint(other.transform.position));
                Destroy(other.gameObject);
            }
            if (other.transform.CompareTag("Finish"))
            {
                _manager.state = State.Finished;
            }
            if (other.transform.CompareTag("Obstacle"))
            {
                EventManager.OnDropHuman();
                other.tag = "Hit";
            }
        }

    }
}