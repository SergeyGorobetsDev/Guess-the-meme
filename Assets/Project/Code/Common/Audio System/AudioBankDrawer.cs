#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;

namespace Assets.Project.Code.Runtime.Gameplay.Common.AudioSystem
{
#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(AudioBank))]
    public class AudioBankDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUI.GetPropertyHeight(property.FindPropertyRelative("kvps"));
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);
            EditorGUI.PropertyField(position, property.FindPropertyRelative("kvps"), label, true);
            EditorGUI.EndProperty();
        }
    }
#endif
}