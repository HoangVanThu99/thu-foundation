﻿using System;
using UnityEditor;

namespace Pancake.Init.EditorOnly
{
	[CanEditMultipleObjects]
    public class InitializableT1Editor : InitializableEditor
    {
		protected override Type BaseTypeDefinition => typeof(IInitializable<>);
    }
}