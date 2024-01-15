using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Vector3Dropdown : MonoBehaviour
{
    public enum Vector3Option
    {
        Up,
        Down,
        Left,
        Right,
        Forward,
        Back
    };

#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(Vector3Dropdown.Vector3Option))]
    public class Vector3OptionDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            var optionNames = System.Enum.GetNames(typeof(Vector3Dropdown.Vector3Option));
            var selectedIndex = EditorGUI.Popup(position, label.text, property.enumValueIndex, optionNames);
            property.enumValueIndex = selectedIndex;

            EditorGUI.EndProperty();
        }
    }
#endif
}
