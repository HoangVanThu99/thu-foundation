﻿using System;
using System.Diagnostics;

namespace Pancake.Attribute
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    [Conditional("UNITY_EDITOR")]
    public class DropdownAttribute : System.Attribute
    {
        public string Values { get; }
        public EMessageType ValidationMessageType { get; set; } = EMessageType.Error;

        public DropdownAttribute(string values) { Values = values; }
    }
}