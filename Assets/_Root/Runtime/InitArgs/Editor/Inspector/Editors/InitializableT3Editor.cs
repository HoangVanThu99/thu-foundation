﻿using System;
using UnityEditor;

namespace Pancake.Init.EditorOnly
{
	[CanEditMultipleObjects]
    public class InitializableT3Editor : InitializableEditor
    {
        protected override Type BaseTypeDefinition => typeof(IInitializable<,,>);
    }
}