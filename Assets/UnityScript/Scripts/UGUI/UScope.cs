/*
 * @Author: fasthro
 * @Date: 2019-06-05 10:04:11
 * @Description: GUI Scope
 */
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace UnityScript.UGUI
{
    public class UUndoScope : GUI.Scope
    {
        private int m_currentGroup = 0;

        public UUndoScope(string text)
        {
            Undo.IncrementCurrentGroup();
            Undo.SetCurrentGroupName(text);
            m_currentGroup = Undo.GetCurrentGroup();
        }

        protected override void CloseScope()
        {
            Undo.CollapseUndoOperations(m_currentGroup);
        }
    }

    public class UHorizontalCenteredScope : GUI.Scope
    {
        public UHorizontalCenteredScope(params GUILayoutOption[] options)
        {
            GUILayout.BeginHorizontal(options);
            GUILayout.FlexibleSpace();
        }

        public UHorizontalCenteredScope(GUIStyle style, params GUILayoutOption[] options)
        {
            GUILayout.BeginHorizontal(style, options);
            GUILayout.FlexibleSpace();
        }

        public UHorizontalCenteredScope(string text, GUIStyle style, params GUILayoutOption[] options)
        {
            GUILayout.BeginHorizontal(text, style, options);
            GUILayout.FlexibleSpace();
        }

        public UHorizontalCenteredScope(Texture image, GUIStyle style, params GUILayoutOption[] options)
        {
            GUILayout.BeginHorizontal(image, style, options);
            GUILayout.FlexibleSpace();
        }

        public UHorizontalCenteredScope(GUIContent content, GUIStyle style, params GUILayoutOption[] options)
        {
            GUILayout.BeginHorizontal(content, style, options);
            GUILayout.FlexibleSpace();
        }

        protected override void CloseScope()
        {
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
        }
    }

    public class UVerticalCenteredScope : GUI.Scope
    {
        public UVerticalCenteredScope(params GUILayoutOption[] options)
        {
            GUILayout.BeginVertical(options);
            GUILayout.FlexibleSpace();
        }

        public UVerticalCenteredScope(GUIStyle style, params GUILayoutOption[] options)
        {
            GUILayout.BeginVertical(style, options);
            GUILayout.FlexibleSpace();
        }

        public UVerticalCenteredScope(string text, GUIStyle style, params GUILayoutOption[] options)
        {
            GUILayout.BeginVertical(text, style, options);
            GUILayout.FlexibleSpace();
        }

        public UVerticalCenteredScope(Texture image, GUIStyle style, params GUILayoutOption[] options)
        {
            GUILayout.BeginVertical(image, style, options);
            GUILayout.FlexibleSpace();
        }

        public UVerticalCenteredScope(GUIContent content, GUIStyle style, params GUILayoutOption[] options)
        {
            GUILayout.BeginVertical(content, style, options);
            GUILayout.FlexibleSpace();
        }

        protected override void CloseScope()
        {
            GUILayout.FlexibleSpace();
            GUILayout.EndVertical();
        }
    }
}