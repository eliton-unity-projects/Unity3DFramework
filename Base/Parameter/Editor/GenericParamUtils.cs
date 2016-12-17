﻿using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Framework
{
    public static class GenericParamUtils
    {
        public static int LabelWidth = 100;
        public static int FieldWidth = 150;
        public static int FieldHeight = 16;
        
        public static void SetDrawersForKnownTypes()
        {
            SetDrawerForKnownType<int>(DrawAsInt, LayoutAsInt);
            SetDrawerForKnownType<bool>(DrawAsBool, LayoutAsBool);
            SetDrawerForKnownType<float>(DrawAsFloat, LayoutAsFloat);
            SetDrawerForKnownType<Vector2>(DrawAsVec2, LayoutAsVec2);
            SetDrawerForKnownType<Vector3>(DrawAsVec3, LayoutAsVec3);
            SetDrawerForKnownType<GameObject>(DrawAsObject, LayoutAsObject);
            SetDrawerForKnownType<MonoBehaviour>(DrawAsObject, LayoutAsObject);
        }

        private static void SetDrawerForKnownType<T>(GenericParameter.DrawFunc draw, GenericParameter.LayoutFunc layout)
        {
            var info = GenericParameter.GetKnownTypeDictionary()[typeof(T).Name];
            info.DrawFunc = draw;
            info.LayoutFunc = layout;
        }

        public static void DrawParameter(Rect drawRect, GenericParameter parameter)
        {
            GenericParameter.Draw(drawRect, parameter);
        }
        
        public static void LayoutParameter(GenericParameter parameter)
        {
            GenericParameter.Layout(parameter);
        }

        #region Draw

        private static void DrawAsObject(Rect rect, GenericParameter parameter)
        {
            var width = rect.width;
            rect.width = width * 0.45f;

            parameter.Name = EditorGUI.TextField(rect, parameter.Name, SpaceEditorStyles.EditableLabel);

            rect.width = width * 0.5f;
            rect.x += rect.width;

            var result = EditorGUI.ObjectField(rect, parameter.GetAs<Object>(), parameter.HoldType.Type, true);
            parameter.SetAs(result);
        }

        private static void DrawAsVec3(Rect rect, GenericParameter parameter)
        {
            var width = rect.width;
            rect.width = width * 0.45f;

            parameter.Name = EditorGUI.TextField(rect, parameter.Name, SpaceEditorStyles.EditableLabel);

            rect.width = width * 0.5f;
            rect.x += rect.width;

            var result = EditorGUI.Vector3Field(rect, GUIContent.none, parameter.GetAs<Vector3>());
            parameter.SetAs(result);
        }

        private static void DrawAsVec2(Rect rect, GenericParameter parameter)
        {
            var width = rect.width;
            rect.width = width * 0.45f;

            parameter.Name = EditorGUI.TextField(rect, parameter.Name, SpaceEditorStyles.EditableLabel);

            rect.width = width * 0.5f;
            rect.x += rect.width;

            var result = EditorGUI.Vector2Field(rect, GUIContent.none, parameter.GetAs<Vector2>());
            parameter.SetAs(result);
        }

        private static void DrawAsInt(Rect rect, GenericParameter parameter)
        {
            var width = rect.width;
            rect.width = width * 0.45f;

            parameter.Name = EditorGUI.TextField(rect, parameter.Name, SpaceEditorStyles.EditableLabel);

            rect.width = width * 0.5f;
            rect.x += rect.width;

            var result = EditorGUI.IntField(rect, GUIContent.none, parameter.GetAs<int>());
            parameter.SetAs(result);
        }

        private static void DrawAsFloat(Rect rect, GenericParameter parameter)
        {
            var width = rect.width;
            rect.width = width * 0.45f;

            parameter.Name = EditorGUI.TextField(rect, parameter.Name, SpaceEditorStyles.EditableLabel);

            rect.width = width * 0.5f;
            rect.x += rect.width;

            var result = EditorGUI.FloatField(rect, GUIContent.none, parameter.GetAs<float>());
            parameter.SetAs(result);
        }

        private static void DrawAsBool(Rect rect, GenericParameter parameter)
        {
            var width   = rect.width;
            rect.width  = rect.width * 0.45f;

            parameter.Name = EditorGUI.TextField(rect, parameter.Name, SpaceEditorStyles.EditableLabel);

            rect.width  = width * 0.5f;
            rect.x     += rect.width + 2;
            rect.width *= 0.5f;
            rect.y     -= 1;

            var result = EditorGUI.Toggle(rect, GUIContent.none, parameter.GetAs<bool>());
            parameter.SetAs(result);

            rect.y += 1;
            rect.x += 20;

            GUI.Label(rect, result.ToString(), EditorStyles.label);
        }

        #endregion

        #region Layout

        private static void LayoutAsObject(GenericParameter parameter)
        {
            EditorGUILayout.BeginHorizontal();
            {
                parameter.Name = EditorGUILayout.TextField(parameter.Name, GUILayout.Width(LabelWidth));
                var result = EditorGUILayout.ObjectField(parameter.GetAs<Object>(), parameter.HoldType.Type, false, GUILayout.Width(FieldWidth));
                parameter.SetAs(result);
            }
            EditorGUILayout.EndHorizontal();
        }

        private static void LayoutAsVec3(GenericParameter parameter)
        {
            EditorGUILayout.BeginHorizontal();
            {
                parameter.Name = EditorGUILayout.TextField(parameter.Name, GUILayout.Width(LabelWidth));
                var result = EditorGUILayout.Vector3Field(GUIContent.none, parameter.GetAs<Vector3>(), GUILayout.Width(FieldWidth));
                parameter.SetAs(result);
            }
            EditorGUILayout.EndHorizontal();
        }

        private static void LayoutAsVec2(GenericParameter parameter)
        {
            EditorGUILayout.BeginHorizontal();
            {
                parameter.Name = EditorGUILayout.TextField(parameter.Name, GUILayout.Width(LabelWidth));
                var result = EditorGUILayout.Vector2Field(GUIContent.none, parameter.GetAs<Vector2>(), GUILayout.Width(FieldWidth));
                parameter.SetAs(result);
            }
            EditorGUILayout.EndHorizontal();
        }

        private static void LayoutAsInt(GenericParameter parameter)
        {
            EditorGUILayout.BeginHorizontal();
            {
                parameter.Name = EditorGUILayout.TextField(parameter.Name, GUILayout.Width(LabelWidth));
                var result = EditorGUILayout.IntField(parameter.GetAs<int>(), GUILayout.Width(FieldWidth));
                parameter.SetAs(result);
            }
            EditorGUILayout.EndHorizontal();
        }

        private static void LayoutAsFloat(GenericParameter parameter)
        {
            EditorGUILayout.BeginHorizontal();
            {
                parameter.Name = EditorGUILayout.TextField(parameter.Name, GUILayout.Width(LabelWidth));
                var result = EditorGUILayout.FloatField(parameter.GetAs<float>(), GUILayout.Width(FieldWidth));
                parameter.SetAs(result);
            }
            EditorGUILayout.EndHorizontal();
        }

        private static void LayoutAsBool(GenericParameter parameter)
        {
            EditorGUILayout.BeginHorizontal();
            {
                parameter.Name = EditorGUILayout.TextField(parameter.Name, GUILayout.Width(LabelWidth));
                var result = EditorGUILayout.Toggle(parameter.GetAs<bool>(), GUILayout.Width(FieldWidth));
                parameter.SetAs(result);
            }
            EditorGUILayout.EndHorizontal();
        }

        #endregion
    }
}
