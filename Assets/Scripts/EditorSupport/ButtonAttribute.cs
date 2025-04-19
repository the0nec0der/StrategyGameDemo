#if UNITY_EDITOR
using System;

using UnityEngine;

namespace EditorHelper
{
    [AttributeUsage(AttributeTargets.Method)]
    public class ButtonAttribute : PropertyAttribute
    {
        public ButtonAttribute() { }
    }
}
#endif
