﻿#if UNITY_EDITOR

using UnityEditor;

namespace Pancake.Editor
{
    /// <summary>
    /// BaseWindow
    /// </summary>
    public abstract class BaseWindow : EditorWindow
    {
        protected virtual void OnEnable() { Undo.undoRedoPerformed += OnUndoRedoPerformed; }


        protected virtual void OnDisable() { Undo.undoRedoPerformed -= OnUndoRedoPerformed; }


        protected virtual void OnUndoRedoPerformed() { Repaint(); }
    } // class BaseWindow
} // namespace Pancake.Editor

#endif // UNITY_EDITOR