﻿using System;
using Pancake.Init;
using UnityEditor;

namespace Pancake.Editor.Init
{
    [CustomEditor(typeof(ConstructorBehaviour<,,,,>), true, isFallback = true), CanEditMultipleObjects]
    public class ConstructorBehaviourT5Editor : BaseConstructorBehaviourEditor
    {
        protected override Type BaseTypeDefinition => typeof(ConstructorBehaviour<,,,,>);
    }
}