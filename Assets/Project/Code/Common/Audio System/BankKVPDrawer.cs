using UnityEditor;
using UnityEngine;

namespace Assets.Project.Code.Runtime.Gameplay.Common.AudioSystem
{
#if UNITY_EDITOR

    [CustomPropertyDrawer(typeof(BankKVP))]
    public class BankKVPDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {

            EditorGUI.BeginProperty(position, label, property);

            Rect rect1 = new Rect(position.x, position.y, position.width / 2 - 4, position.height);
            Rect rect2 = new Rect(position.center.x + 2, position.y, position.width / 2 - 4, position.height);

            EditorGUI.PropertyField(rect1, property.FindPropertyRelative("Key"), GUIContent.none);
            EditorGUI.PropertyField(rect2, property.FindPropertyRelative("Value"), GUIContent.none);

            EditorGUI.EndProperty();
        }
    }
#endif
}