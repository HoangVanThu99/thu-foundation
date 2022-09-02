﻿using System;
using UnityEditor;

namespace Pancake.Init.EditorOnly
{
	[CustomEditor(typeof(StateMachineBehaviour<,,,,,>), true, isFallback = true), CanEditMultipleObjects]
    public class StateMachineBehaviourT6Editor : InitializableEditor
    {
        protected override Type BaseTypeDefinition => typeof(StateMachineBehaviour<,,,,,>);
    }
}