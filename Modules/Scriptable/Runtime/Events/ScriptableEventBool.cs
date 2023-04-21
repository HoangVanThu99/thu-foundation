﻿using Pancake.Attribute;
using UnityEngine;

namespace Pancake.Scriptable
{
    [EditorIcon("scriptable_event")]
    [CreateAssetMenu(fileName = "scriptable_event_bool.asset", menuName = "Pancake/Scriptable/Events/bool")]
    public class ScriptableEventBool : ScriptableEvent<bool>
    {
    }
}