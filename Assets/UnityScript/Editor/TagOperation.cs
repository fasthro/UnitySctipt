/*
 * @Author: fasthro
 * @Date: 2019-06-04 19:24:25
 * @Description: Tag 操作, Unity 2018 版本
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace UnityScript.UEditor
{
    public class TagOperation
    {
        /// <summary>
        /// 添加 TAG
        /// </summary>
        /// <param name="tag"></param>
        public static void AddTag(string tag)
        {
            if (IsHasTag(tag))
            {
                Debug.LogWarning("AddTag -> " + tag + " already exists!");
                return;
            }

            SerializedObject tagManager = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset")[0]);
            SerializedProperty it = tagManager.GetIterator();
            SerializedProperty tagsProp = tagManager.FindProperty("tags");
            int index = tagsProp.arraySize;
            tagsProp.InsertArrayElementAtIndex(index);
            SerializedProperty sp = tagsProp.GetArrayElementAtIndex(index);
            sp.stringValue = tag;
            tagManager.ApplyModifiedProperties();
        }

        /// <summary>
        /// 清理 TAG
        /// </summary>
        public static void ClearTag()
        {
            SerializedObject tagManager = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset")[0]);
            SerializedProperty it = tagManager.GetIterator();
            SerializedProperty tagsProp = tagManager.FindProperty("tags");
            tagsProp.ClearArray();
            tagManager.ApplyModifiedProperties();
        }

        /// <summary>
        /// 是否有 TAG
        /// </summary>
        /// <param name="tag"></param>
        public static bool IsHasTag(string tag)
        {
            for (int i = 0; i < UnityEditorInternal.InternalEditorUtility.tags.Length; i++)
            {
                if (UnityEditorInternal.InternalEditorUtility.tags[i].Contains(tag))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 添加 LAYER
        /// </summary>
        /// <param name="layer"></param>
        public static void AddLayer(string layer)
        {
            if (IsHasLayer(layer))
            {
                Debug.LogWarning("AddLayer -> " + layer + " already exists!");
                return;
            }

            SerializedObject tagManager = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset")[0]);
            SerializedProperty it = tagManager.GetIterator();
            SerializedProperty tagsProp = tagManager.FindProperty("layers");

            for (int i = 0; i < tagsProp.arraySize; i++)
            {
                if (i > 7)
                {
                    SerializedProperty sp = tagsProp.GetArrayElementAtIndex(i);
                    if (string.IsNullOrEmpty(sp.stringValue))
                    {
                        sp.stringValue = layer;
                        tagManager.ApplyModifiedProperties();
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// 清理 LAYER
        /// </summary>
        public static void ClearLayers()
        {
            SerializedObject tagManager = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset")[0]);
            SerializedProperty it = tagManager.GetIterator();
            SerializedProperty layersProp = tagManager.FindProperty("layers");
            layersProp.ClearArray();
            tagManager.ApplyModifiedProperties();
        }

        /// <summary>
        /// 是否有LAYER
        /// </summary>
        /// <param name="tag"></param>
        public static bool IsHasLayer(string layer)
        {
            for (int i = 0; i < UnityEditorInternal.InternalEditorUtility.layers.Length; i++)
            {
                if (UnityEditorInternal.InternalEditorUtility.layers[i].Contains(layer))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
