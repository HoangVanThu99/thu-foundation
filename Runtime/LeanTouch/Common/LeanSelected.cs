﻿using UnityEngine;
using UnityEngine.Events;
using CW.Common;

namespace Lean.Common
{
    /// <summary>This component allows you to detect when any selectable object in the scene has been selected.</summary>
    [HelpURL(LeanCommon.PlusHelpUrlPrefix + "LeanSelected")]
    [AddComponentMenu(LeanCommon.ComponentPathPrefix + "Selected")]
    public class LeanSelected : MonoBehaviour
    {
        [System.Serializable]
        public class LeanSelectableEvent : UnityEvent<LeanSelectable>
        {
        }

        public LeanSelectableEvent OnSelectable
        {
            get
            {
                if (onSelectable == null) onSelectable = new LeanSelectableEvent();
                return onSelectable;
            }
        }

        [SerializeField] private LeanSelectableEvent onSelectable;

        protected virtual void OnEnable() { LeanSelect.OnAnySelected += HandleSelectGlobal; }
        protected virtual void OnDisable() { LeanSelect.OnAnySelected -= HandleSelectGlobal; }

        private void HandleSelectGlobal(LeanSelect select, LeanSelectable selectable)
        {
            if (onSelectable != null)
            {
                onSelectable.Invoke(selectable);
            }
        }
    }
}

#if UNITY_EDITOR
namespace Lean.Common.Editor
{
    using UnityEditor;
    using TARGET = LeanSelected;

    [CanEditMultipleObjects]
    [CustomEditor(typeof(TARGET), true)]
    public class LeanSelected_Editor : CwEditor
    {
        protected override void OnInspector()
        {
            TARGET tgt;
            TARGET[] tgts;
            GetTargets(out tgt, out tgts);

            Draw("onSelectable");
        }
    }
}
#endif