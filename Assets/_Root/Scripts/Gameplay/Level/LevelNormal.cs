using Pancake.LevelSystem;
using UnityEngine;

namespace Pancake.SceneFlow
{
    public class LevelNormal : LevelComponent
    {
        private bool _isFingerDown;
        private bool _isFingerDrag;

        protected override void OnSpawned()
        {
            base.OnSpawned();
            Lean.Touch.LeanTouch.OnFingerDown += HandleFingerDown;
            Lean.Touch.LeanTouch.OnFingerUp += HandleFingerUp;
            Lean.Touch.LeanTouch.OnFingerUpdate += HandleFingerUpdate;
        }
        
        protected override void OnDespawned()
        {
            base.OnDespawned();
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
        
    }
}