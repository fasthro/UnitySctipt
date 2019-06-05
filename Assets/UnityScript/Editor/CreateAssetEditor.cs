/*
 * @Author: fasthro
 * @Date: 2019-02-26 10:23:26
 * @Description: 创建*.asset资源文件
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System;

namespace UnityScript
{
    public class CreateAssetEditor : EditorWindow
    {
        private static CreateAssetEditor m_window;

        // 类名
        private string m_className;
        // 保存的文件名
        private string m_saveFileName;
        // 保存路径
        private string m_savePath;


        [MenuItem("UnityScript/创建 .asset 资源")]  
        public static void Open()
        {
            m_window = GetWindow<CreateAssetEditor>();
            m_window.titleContent = new GUIContent("创建 .asset 资源");
            m_window.minSize = new Vector2(380, 260);
            m_window.Show();
        }

        void OnEnabled()
        {
            if (m_window == null) Open();
        }

        void OnGUI()
        {
            GUILayout.Label("Class Name: ", Style.Font(GUI.skin.label, TextAnchor.MiddleLeft, 18, FontStyle.Bold));
            m_className = GUILayout.TextField(m_className, Style.Font(GUI.skin.textField, TextAnchor.MiddleLeft, 16));
            
            GUILayout.Space(5);

            GUILayout.Label("Save File Name: ", Style.Font(GUI.skin.label, TextAnchor.MiddleLeft, 18, FontStyle.Bold));
            m_saveFileName = GUILayout.TextField(m_saveFileName, Style.Font(GUI.skin.textField, TextAnchor.MiddleLeft, 16));

            GUILayout.Space(5);

            GUILayout.Label("Save Path: ", Style.Font(GUI.skin.label, TextAnchor.MiddleLeft, 18, FontStyle.Bold));
            
            GUILayout.BeginHorizontal();
            m_savePath = GUILayout.TextField(m_savePath, Style.Font(GUI.skin.textField, TextAnchor.MiddleLeft, 16));
            if (GUILayout.Button("Browse", Style.Font(GUI.skin.button, TextAnchor.MiddleCenter, 16), GUILayout.Width(80)))
            {
                m_savePath = EditorUtility.OpenFolderPanel("Browse Save *.asset Path", "", "");
            }
            GUILayout.EndHorizontal();

            if (GUILayout.Button("Create", Style.Font(GUI.skin.button, TextAnchor.MiddleCenter, 18), GUILayout.Height(80)))
            {
                CreateAsset();
            }
        }

        private void CreateAsset()
        {
            // 类名检查
            if (string.IsNullOrEmpty(m_className))
            {
                ShowNotification(new GUIContent("请填写 Class Name"));
                return;
            }

            ScriptableObject sto = ScriptableObject.CreateInstance(m_className);
            if (sto == null)
            {
                ShowNotification(new GUIContent(string.Format("无法识别类名[{0}]", m_className)));
                return;
            }

            // 文件名检查
            if (string.IsNullOrEmpty(m_saveFileName))
            {
                ShowNotification(new GUIContent("请填写要保存的文件名"));
                return;
            }

            // 路径检查
            if (string.IsNullOrEmpty(m_savePath))
            {
                ShowNotification(new GUIContent("请选择保存路径"));
                return;
            }

            string fullPath = string.Format("{0}/{1}.asset", m_savePath, m_saveFileName);
            string temp = DateTime.Now.ToString("yyyyMMddHHmmss") + ".asset";

            if (File.Exists(temp))
            {
                File.Delete(temp);
            }

            if (File.Exists(fullPath))
            {
                if (!EditorUtility.DisplayDialog("提示", "资源已经存在,是否替换？", "替换", "取消"))
                {
                    return;
                }
                File.Delete(fullPath);
            }

            AssetDatabase.CreateAsset(sto, "Assets/" + temp);

            string source = string.Format("{0}/{1}", Application.dataPath, temp);
            File.Move(source, fullPath);
            File.Delete(source);

            AssetDatabase.Refresh();

            m_window.Close();
        }
    }
}
