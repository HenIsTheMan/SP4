﻿using UnityEngine;
using UnityEditor;

#if UNITY_EDITOR

namespace IdolFever
{
    [CustomPropertyDrawer(typeof(EnumNamedArrayAttribute))]
    public class DrawerEnumNamedArray : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EnumNamedArrayAttribute enumNames = attribute as EnumNamedArrayAttribute;

            //so get the index from there
            int index = System.Convert.ToInt32(property.propertyPath.Substring(property.propertyPath.IndexOf("[")).Replace("[", "").Replace("]", ""));
            //change the label
            label.text = enumNames.names[index];
            //draw field
            EditorGUI.PropertyField(position, property, label, true);
        }
    }

    // credit to https://answers.unity.com/questions/1589226/showing-an-array-with-enum-as-keys-in-the-property.html

}

#endif