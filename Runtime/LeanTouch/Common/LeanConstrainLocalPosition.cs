#if PANCAKE_LEANTOUCH

using UnityEngine;
using CW.Common;

namespace Lean.Common
{
    /// <summary>This component allows you to constrain the current <b>transform.localPosition</b> or <b>transform.anchoredPosition3D</b> to the specified min/max values.</summary>
    [DefaultExecutionOrder(200)]
    [HelpURL(LeanCommon.PlusHelpUrlPrefix + "LeanConstrainLocalPosition")]
    [AddComponentMenu(LeanCommon.ComponentPathPrefix + "Constrain LocalPosition")]
    public class LeanConstrainLocalPosition : MonoBehaviour
    {
        /// <summary>Clamp the X axis?</summary>
        public bool X { set { x = value; } get { return x; } }

        [SerializeField] private bool x;

        public float XMin { set { xMin = value; } get { return xMin; } }
        [SerializeField] private float xMin = -1.0f;

        public float XMax { set { xMax = value; } get { return xMax; } }
        [SerializeField] private float xMax = 1.0f;

        /// <summary>Clamp the Y axis?</summary>
        public bool Y { set { y = value; } get { return y; } }

        [SerializeField] private bool y;

        public float YMin { set { yMin = value; } get { return yMin; } }
        [SerializeField] private float yMin = -1.0f;

        public float YMax { set { yMax = value; } get { return yMax; } }
        [SerializeField] private float yMax = 1.0f;

        /// <summary>Clamp the Z axis?</summary>
        public bool Z { set { z = value; } get { return z; } }

        [SerializeField] private bool z;

        public float ZMin { set { zMin = value; } get { return zMin; } }
        [SerializeField] private float zMin = -1.0f;

        public float ZMax { set { zMax = value; } get { return zMax; } }
        [SerializeField] private float zMax = 1.0f;

        protected virtual void LateUpdate()
        {
            var rectTransform = transform as RectTransform;

            if (rectTransform != null)
            {
                var position = rectTransform.anchoredPosition3D;

                if (DoClamp(ref position) == true)
                {
                    rectTransform.anchoredPosition3D = position;
                }
            }
            else
            {
                var position = transform.localPosition;

                if (DoClamp(ref position) == true)
                {
                    transform.localPosition = position;
                }
            }
        }

        private bool DoClamp(ref Vector3 value)
        {
            var modified = false;

            if (x == true)
            {
                if (value.x < xMin)
                {
                    value.x = xMin;
                    modified = true;
                }
                else if (value.x > xMax)
                {
                    value.x = xMax;
                    modified = true;
                }
            }

            if (y == true)
            {
                if (value.y < yMin)
                {
                    value.y = yMin;
                    modified = true;
                }
                else if (value.y > yMax)
                {
                    value.y = yMax;
                    modified = true;
                }
            }

            if (z == true)
            {
                if (value.z < zMin)
                {
                    value.z = zMin;
                    modified = true;
                }
                else if (value.z > zMax)
                {
                    value.z = zMax;
                    modified = true;
                }
            }

            return modified;
        }
    }
}

#if UNITY_EDITOR
namespace Lean.Common.Editor
{
    using UnityEditor;
    using TARGET = LeanConstrainLocalPosition;

    [CanEditMultipleObjects]
    [CustomEditor(typeof(TARGET))]
    public class LeanConstrainLocalPosition_Editor : CwEditor
    {
        protected override void OnInspector()
        {
            TARGET tgt;
            TARGET[] tgts;
            GetTargets(out tgt, out tgts);

            Draw("x", "Clamp the X axis?");
            if (Any(tgts, t => t.X == true))
            {
                BeginIndent();
                Draw("xMin", "", "Min");
                Draw("xMax", "", "Max");
                EndIndent();
            }

            Draw("y", "Clamp the Y axis?");
            if (Any(tgts, t => t.Y == true))
            {
                BeginIndent();
                Draw("yMin", "", "Min");
                Draw("yMax", "", "Max");
                EndIndent();
            }

            Draw("z", "Clamp the Z axis?");
            if (Any(tgts, t => t.Z == true))
            {
                BeginIndent();
                Draw("zMin", "", "Min");
                Draw("zMax", "", "Max");
                EndIndent();
            }
        }
    }
}
#endif
#endif