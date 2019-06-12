/*
 * @Author: fasthro
 * @Date: 2019-06-10 10:14:28
 * @Description: Miss Component 工具
 * 查询，清理全部，导出txt文本，清理单个
 * PATHS 配置查询路径，默认 Assets/
 */
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;
using UnityScript.UGUI;

namespace UnityScript.UEditor
{
    public class MissComponent
    {
        private string m_guid;
        public string guid { get { return m_guid; } }

        private string m_path;
        public string path
        {
            get
            {
                if (string.IsNullOrEmpty(m_path)) return "miss component";
                return m_path;
            }
        }

        public MissComponent(string guid, string path)
        {
            m_guid = guid;
            m_path = path;
        }
    }

    public class MissComponentInfo
    {
        private string m_path;
        public string path { get { return m_path; } }

        private List<MissComponent> m_components;
        public List<MissComponent> components { get { return m_components; } }

        public MissComponentInfo(string path)
        {
            this.m_path = path;
            m_components = new List<MissComponent>();
        }

        public void AddComponent(MissComponent component)
        {
            m_components.Add(component);
        }
    }

    public class MissComponentEditor : EditorWindow
    {
        private readonly static string[] PATHS = new string[] { "Assets/" };

        private static MissComponentEditor m_window = null;
        private List<MissComponentInfo> m_infos;
        private Vector2 m_scrollPosition;

        [MenuItem("UnityScript/MissComponent")]
        public static void Open()
        {
            m_window = GetWindow<MissComponentEditor>();
            m_window.titleContent = new GUIContent("MissComponent");
            m_window.minSize = new Vector2(500, 600);
            m_window.Show();
        }

        void OnEnabled()
        {
            if (m_window == null) Open();
        }

        void OnGUI()
        {
            EditorGUILayout.BeginHorizontal("box");
            if (GUILayout.Button("Find", UStyle.Font(GUI.skin.button, TextAnchor.MiddleCenter, 18), GUILayout.Height(50)))
            {
                FindDirectory();
            }
            if (GUILayout.Button("Clean All", UStyle.Font(GUI.skin.button, TextAnchor.MiddleCenter, 18), GUILayout.Height(50)))
            {
                CleanAll();
            }
            if (GUILayout.Button("Export", UStyle.Font(GUI.skin.button, TextAnchor.MiddleCenter, 18), GUILayout.Height(50)))
            {
                Export();
                ShowNotification(new GUIContent("export succeed! -> Assets/MissComponent.txt"));
            }
            EditorGUILayout.EndHorizontal();

            if (m_infos != null)
            {
                m_scrollPosition = EditorGUILayout.BeginScrollView(m_scrollPosition);
                for (var i = 0; i < m_infos.Count; i++)
                {
                    var info = m_infos[i];
                    EditorGUILayout.BeginVertical("box");

                    EditorGUILayout.BeginHorizontal("box");
                    EditorGUILayout.LabelField(info.path, UStyle.Font(GUI.skin.label, TextAnchor.MiddleLeft, 14, FontStyle.Bold), GUILayout.Height(26));
                    if (GUILayout.Button("Goto", GUILayout.Width(45), GUILayout.Height(26)))
                    {
                        EditorGUIUtility.PingObject(AssetDatabase.LoadAssetAtPath(info.path, typeof(GameObject)) as GameObject);
                    }

                    if (GUILayout.Button("Clean", GUILayout.Width(45), GUILayout.Height(26)))
                    {
                        CleanObject(info);
                        m_infos.RemoveAt(i);
                        break;
                    }
                    EditorGUILayout.EndHorizontal();

                    for (int k = 0; k < info.components.Count; k++)
                    {
                        var component = info.components[k];
                        EditorGUILayout.BeginHorizontal("box");
                        EditorGUILayout.LabelField(component.path);
                        EditorGUILayout.EndHorizontal();
                    }

                    EditorGUILayout.EndVertical();
                }

                EditorGUILayout.EndScrollView();
            }
        }

        private void FindDirectory()
        {
            m_infos = new List<MissComponentInfo>();
            List<string> fs = new List<string>();
            for (int k = 0; k < PATHS.Length; k++)
            {
                string[] files = Directory.GetFiles(PATHS[k], "*.prefab", SearchOption.AllDirectories);
                for (int i = 0; i < files.Length; i++)
                {
                    fs.Add(files[i]);
                }
            }

            if (fs.Count > 0)
            {
                int startIndex = 0;
                EditorApplication.update = delegate ()
                {
                    var fp = fs[startIndex];

                    bool isCancel = EditorUtility.DisplayCancelableProgressBar("Finding", fp, (float)startIndex / (float)fs.Count);

                    FindObject(fp);

                    startIndex++;
                    if (isCancel || startIndex >= fs.Count)
                    {
                        EditorUtility.ClearProgressBar();
                        EditorApplication.update = null;
                        startIndex = 0;

                        ShowNotification(new GUIContent("Find finished!"));
                    }
                };
            }
            else
            {
                ShowNotification(new GUIContent("not find missing component"));
            }
        }

        private void FindObject(string path)
        {
            if (!string.IsNullOrEmpty(path))
            {
                Regex guidRegex = new Regex("m_Script: {fileID: (.*), guid: (?<GuidValue>.*?), type:");
                MatchCollection matchList = guidRegex.Matches(File.ReadAllText(path));
                if (matchList != null)
                {
                    MissComponentInfo info = new MissComponentInfo(path);
                    for (int i = 0; i < matchList.Count; i++)
                    {
                        string guid = matchList[i].Groups["GuidValue"].Value;
                        string p = AssetDatabase.GUIDToAssetPath(guid);
                        if (string.IsNullOrEmpty(p) || !File.Exists(p))
                        {
                            MissComponent mc = new MissComponent(guid, p);
                            info.AddComponent(mc);
                        }
                    }
                    if (info.components.Count > 0) m_infos.Add(info);
                }
            }
        }

        private void CleanObject(MissComponentInfo info)
        {
            string content = File.ReadAllText(info.path);
            string[] strArray = content.Split(new string[] { "---" }, StringSplitOptions.RemoveEmptyEntries);
            Regex regBlock = new Regex("MonoBehaviour");

            for (int i = 0; i < strArray.Length; i++)
            {
                string blockStr = strArray[i];
                if (regBlock.IsMatch(blockStr))
                {
                    Match guidMatch = Regex.Match(blockStr, "m_Script: {fileID: (.*), guid: (?<GuidValue>.*?), type:");
                    if (guidMatch.Success)
                    {
                        string guid = guidMatch.Groups["GuidValue"].Value;
                        for (int k = 0; k < info.components.Count; k++)
                        {
                            var mc = info.components[k];
                            if (mc.guid == guid)
                            {
                                // remove MonoBehaviour
                                content = content.Replace("---" + blockStr, "");

                                // 移除 MonoBehaviour 引用
                                Match fileIdMatch = Regex.Match(blockStr, " !u!(.*) &(?<FileIdValue>.*?)\n");
                                if (fileIdMatch.Success)
                                {
                                    string fileId = fileIdMatch.Groups["FileIdValue"].Value;
                                    Regex quote = new Regex("  - (.*): {fileID: " + fileId + "}");
                                    content = quote.Replace(content, "");
                                }

                                Debug.Log(string.Format("miss component -> remove {0} guid:{1}", info.path, guid));
                                break;
                            }
                        }
                    }
                }
            }

            File.WriteAllText(info.path, content);
        }

        private void Export()
        {
            StringBuilder builder = new StringBuilder();
            for (var i = 0; i < m_infos.Count; i++)
            {
                var info = m_infos[i];
                builder.AppendLine(info.path);

                for (int k = 0; k < info.components.Count; k++)
                {
                    var component = info.components[k];
                    builder.AppendLine("\t-> " + component.path);
                }
                builder.AppendLine("");
            }

            File.WriteAllText("Assets/MissComponent.txt", builder.ToString());
        }

        private void CleanAll()
        {
            int startIndex = 0;
            EditorApplication.update = delegate ()
            {
                var info = m_infos[startIndex];

                bool isCancel = EditorUtility.DisplayCancelableProgressBar("Cleaning", info.path, (float)startIndex / (float)m_infos.Count);

                CleanObject(info);

                startIndex++;
                if (isCancel || startIndex >= m_infos.Count)
                {
                    EditorUtility.ClearProgressBar();
                    EditorApplication.update = null;
                    startIndex = 0;

                    ShowNotification(new GUIContent("clean finished!"));

                    FindDirectory();
                }
            };
        }
    }
}
