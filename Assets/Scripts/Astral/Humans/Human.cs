using System;
using System.Collections.Generic;
using UnityEngine;

namespace Astral.Humans
{
    [Serializable]
    
    public class Human
    {


        //public Dictionary<int, AnimationClip> humanPoses = new Dictionary<int, AnimationClip>();
        public string humanPose;
        public Vector3 humanPositions;
        public Vector3 humanRotation;
        public Vector3 humanScale;
        public Color humanColor;
        
        public Human(string constPose,Vector3 constPos,Vector3 constRot,Vector3 constScale,Color constColor)
        {
            humanPose = constPose;
            humanPositions = constPos;
            humanRotation = constRot;
            humanColor =  constColor;
            humanScale = constScale;
        }
        
    }
}