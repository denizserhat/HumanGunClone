using System;
using Astral.Utility;
using TMPro;
using UnityEngine;

namespace Astral.Door
{
    public enum GateType
    {
        Plus,
        Minus
    }
    public class Gate : MonoBehaviour
    {
        public GateType gateType;
        public int gateCount;
        public TextMeshProUGUI gateCountText;
        [SerializeField] private Material disableMat;

        private Renderer _renderer;

        private void Start()
        {
            _renderer = GetComponentInChildren<Renderer>();
            gateCountText = GetComponentInChildren<TextMeshProUGUI>();
            gateCountText.text = gateType == GateType.Plus ? "+" + gateCount : "-" + gateCount;
        }

        public void Execute()
        {
            _renderer.material = disableMat;
            if (gateType  == GateType.Plus)
            {
                EventManager.OnUpgradeWeapon(transform.position,gateCount);
            }
            else if (gateType == GateType.Minus)
            {
                EventManager.OnDropHuman(Mathf.Abs(gateCount));
            }
        }

    }
}