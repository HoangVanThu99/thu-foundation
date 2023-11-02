using System;
using System.Collections.Generic;
using System.Linq;
using Pancake.Apex;
using Pancake.LevelSystem;
using Pancake.Scriptable;
using UnityEngine;

namespace Pancake.SceneFlow
{
    public class LevelNormal : LevelComponent
    {
        [SerializeField, Group("Event")] private ScriptableEventNoParam rollEvent;
        [SerializeField, Group("Event")] private ScriptableEventNoParam diceRollEvent;
        
        private List<Dice> dices;
        private DiceRoller diceRoller;
        
        private bool _isFingerDown;
        private bool _isFingerDrag;

        private void Start()
        {
            dices = GetComponentsInChildren<Dice>().ToList();
            diceRoller = GetComponentInChildren<DiceRoller>();
            
            diceRollEvent.OnRaised += SetupRolling;
        }

        private void OnDestroy()
        {
            diceRollEvent.OnRaised -= SetupRolling;
        }

        protected override void OnSpawned()
        {
            base.OnSpawned();
            Lean.Touch.LeanTouch.OnFingerDown += HandleFingerDown;
            Lean.Touch.LeanTouch.OnFingerUp += HandleFingerUp;
            Lean.Touch.LeanTouch.OnFingerUpdate += HandleFingerUpdate;
        }
        
        protected override void OnDespawned()
        {
            base.OnSpawned();
            Lean.Touch.LeanTouch.OnFingerDown -= HandleFingerDown;
            Lean.Touch.LeanTouch.OnFingerUp -= HandleFingerUp;
            Lean.Touch.LeanTouch.OnFingerUpdate -= HandleFingerUpdate;
        }

        void HandleFingerDown(Lean.Touch.LeanFinger finger)
        {
            if (!finger.IsOverGui)
            {
                _isFingerDown = true;
            
                //Get Object raycast hit
                var ray = finger.GetRay(Camera.main);
                var hit = default(RaycastHit);
        
                if (Physics.Raycast(ray, out hit, float.PositiveInfinity)) { //ADDED LAYER SELECTION
                    Debug.Log(hit.collider.gameObject);
                }
            }
        }
    
        void HandleFingerUp(Lean.Touch.LeanFinger finger)
        {
            _isFingerDown = false;
        }
    
        void HandleFingerUpdate(Lean.Touch.LeanFinger finger)
        {
            if (_isFingerDown)
            {
                _isFingerDrag = true;
            }
        }
        
        protected override void OnSkipLevel() {  }

        protected override void OnReplayLevel() {  }

        protected override void OnLoseLevel() {  }

        protected override void OnWinLevel() {  }

        private void SetupRolling()
        {
            if (dices.TrueForAll(item => item.isDoneRoll))
            {
                diceRoller.gameObject.SetActive(true);
                foreach (var dice in dices)
                {
                    dice.ResetRoll();
                }
            }
        }
    }
}