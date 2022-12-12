using System;
using Astral.Variables;
using UnityEngine;

namespace Astral.Utility
{

    public enum State
    {
        Idle,
        Started,
        Finished,
        Win,
        Lose
    }
    public class GameManager : MonoBehaviour
    {
        public State state;
        public Variable<int> money;
        public GameVariable humanCount;
        public GameVariable range;
        public GameVariable damage;

    }
}