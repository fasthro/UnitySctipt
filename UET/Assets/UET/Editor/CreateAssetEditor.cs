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

namespace UET
{
    public class CreateAssetEditor : EditorWindow
    {
        private static CreateAssetEditor window;

        private string m_className;
        private string m_path;
        private string m_fileName;

        private GUIStyle style;
        private float width;
        private int beginX = 5;
        private int beginY = 0;
        private int lineHeight = 30;

        [MenuItem("UET/创建 .asset 资源")]
        public static void Window()
        {
            window = GetWindow<CreateAssetEditor>();
            window.titleContent = new GUIContent("CreateAsset");
            window.Show();
        }

        void OnEnabled()
        {
            if (window == null)
            {
                Window();
            }
        }

        void OnGUI()
        {
            width = window.position.width;

            // class name
            GUILayout.BeginVertical();
            TitleStyle();
            GUI.Label(new Rect(beginX, beginY, width - beginX * 2, lineHeight), "Class Name: ", style);

            TextStyle();
            m_className = GUI.TextField(new Rect(beginX, beginY + lineHeight, width - beginX * 2, lineHeight), m_className, style);
            GUILayout.EndVertical();

            // file name
            GUILayout.BeginVertical();
            TitleStyle();
            GUI.Label(new Rect(beginX, beginY + lineHeight * 2, width - beginX * 2, lineHeight), "File Name: ", style);

            TextStyle();
            m_fileName = GUI.TextField(new Rect(beginX, beginY + lineHeight * 3, width - beginX * 2, lineHeight), m_fileName, style);
            GUILayout.EndVertical();

            // path
            GUILayout.BeginVertical();
            TitleStyle();
            GUI.Label(new Rect(beginX, beginY + lineHeight * 4, width - beginX * 2, lineHeight), "Save Path: ", style);

            GUILayout.BeginHorizontal();
            TextStyle();
            float pw = (width - beginX * 2) * 0.8f;
            m_path = GUI.TextField(new Rect(beginX, beginY + lineHeight * 5, pw, lineHeight), m_path, style);
            float pw2 = (width - beginX * 2) * 0.2f;
            if (GUI.Button(new Rect(beginX + pw, beginY + lineHeight * 5, pw2, lineHeight), "Browse"))
            {
                m_path = EditorUtility.OpenFolderPanel("Browse Save *.asset Path", "", "");
            }
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();

            // create
            CreateButtonStyle();
            if (GUI.Button(new Rect(beginX, beginY + lineHeight * 6 + 10, width - beginX * 2, lineHeight), "Create"))
            {
                CreateAsset();
            }
        }

        private void TitleStyle()
        {
            style = new GUIStyle(GUI.skin.label);
            style.alignment = TextAnchor.MiddleLeft;
            style.fontSize = 18;
            style.fontStyle = FontStyle.Bold;
        }

        private void TextStyle()
        {
            style = new GUIStyle(GUI.skin.textField);
            style.alignment = TextAnchor.MiddleLeft;
            style.fontSize = 18;
            style.fontStyle = FontStyle.Normal;
        }

        private void CreateButtonStyle()
        {
            GUIStyle style = new GUIStyle(GUI.skin.button);
            style.alignment = TextAnchor.MiddleCenter;
            style.fontSize = 18;
            style.fontStyle = FontStyle.Bold;
            GUI.backgroundColor = new Color(128.0f / 255, 1, 128.0f / 255, 1);
        }

        private void CreateAsset()
        {
            // 类名检查
            if (string.IsNullOrEmpty(m_className))
            {
                ShowNotification(new GUIContent("Class Name 填写不合法"));
                return;
            }

            ScriptableObject sto = ScriptableObject.CreateInstance(m_className);
            if (sto == null)
            {
                ShowNotification(new GUIContent(string.Format("无法识别类名[{0}]", m_className)));
                return;
            }

            // 文件名检查
            if (string.IsNullOrEmpty(m_fileName))
            {
                ShowNotification(new GUIContent("File Name 填写不合法"));
                return;
            }

            // 路径检查
            if (string.IsNullOrEmpty(m_path))
            {
                ShowNotification(new GUIContent("Save Path 填写不合法"));
                return;
            }

            string savePath = string.Format("{0}/{1}.asset", m_path, m_fileName);
            string tfn = "uet-create-asset-editor.asset";
            string tfp = "Assets/" + tfn;

            // 目标路径检查
            if (File.Exists(tfp))
            {
                File.Delete(tfp);
            }

            if (File.Exists(savePath))
            {
                if (!EditorUtility.DisplayDialog("提示", "资源已经存在,是否替换？", "替换", "取消"))
                {
                    return;
                }
                File.Delete(savePath);
            }

            AssetDatabase.CreateAsset(sto, tfp);

            string source = string.Format("{0}/{1}", Application.dataPath, tfn);
            File.Move(source, savePath);
            File.Delete(source);

            AssetDatabase.Refresh();

            window.Close();
        }
    }
}
