#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace EditorHelper
{
    [CustomEditor(typeof(MonoBehaviour), true)]
    public class ButtonAttributeDrawer : Editor
    {
        private Dictionary<string, object[]> methodParameters = new();
        private Dictionary<string, bool> foldoutStates = new();

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            DrawButtons(target);
        }

        private void DrawButtons(object targetObject)
        {
            var methods = targetObject.GetType()
                .GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                .Where(m => m.GetCustomAttributes(typeof(ButtonAttribute), true).Length > 0);

            foreach (var method in methods)
            {
                var parameters = method.GetParameters();
                string key = method.Name;

                if (!methodParameters.ContainsKey(key))
                    methodParameters[key] = parameters.Select(p => p.DefaultValue ?? GetDefault(p.ParameterType)).ToArray();

                if (!foldoutStates.ContainsKey(key))
                    foldoutStates[key] = true;

                EditorGUILayout.BeginVertical("box");
                foldoutStates[key] = EditorGUILayout.Foldout(foldoutStates[key], $"▶ {ObjectNames.NicifyVariableName(method.Name)}", true);

                if (foldoutStates[key])
                {
                    GUIStyle paramLabel = new(EditorStyles.label) { fontStyle = FontStyle.Italic, wordWrap = true };

                    for (int i = 0; i < parameters.Length; i++)
                    {
                        var param = parameters[i];
                        var current = methodParameters[key][i];

                        var updated = DrawParameterField(param.Name, param.ParameterType, current);
                        if (updated != null || param.ParameterType.IsClass)
                            methodParameters[key][i] = updated;
                        else
                            EditorGUILayout.LabelField($"{param.Name} Unsupported ({param.ParameterType.Name})", paramLabel);
                    }

                    GUILayout.Space(4);
                    if (GUILayout.Button($"Invoke {method.Name}()", GUILayout.Height(30)))
                    {
                        method.Invoke(targetObject, methodParameters[key]);
                    }
                }

                EditorGUILayout.EndVertical();
                GUILayout.Space(6);
            }
        }

        private object DrawParameterField(string label, Type type, object value)
        {
            if (type == typeof(int))
                return EditorGUILayout.IntField(label, value is int i ? i : 0);
            if (type == typeof(float))
                return EditorGUILayout.FloatField(label, value is float f ? f : 0f);
            if (type == typeof(bool))
                return EditorGUILayout.Toggle(label, value is bool b && b);
            if (type == typeof(string))
                return EditorGUILayout.TextField(label, value?.ToString() ?? "");
            if (typeof(UnityEngine.Object).IsAssignableFrom(type))
                return EditorGUILayout.ObjectField(label, value as UnityEngine.Object, type, true);

            EditorGUILayout.LabelField(label, $"Unsupported ({type.Name})");
            return value;
        }

        private object GetDefault(Type type)
        {
            return type.IsValueType ? Activator.CreateInstance(type) : null;
        }
    }
}
#endif
