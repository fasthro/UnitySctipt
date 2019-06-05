/*
 * @Author: fasthro
 * @Date: 2019-06-05 10:20:13
 * @Description: GUIStyle
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityScript
{
    public class Style
    {
        private static GUIStyle style;

        /// <summary>
        /// 字体 Style
        /// </summary>
        /// <param name="other"></param>
        /// <param name="textAnchor"></param>
        /// <param name="fontSize"></param>
        /// <param name="fontStyle"></param>
        public static GUIStyle Font(GUIStyle other, TextAnchor textAnchor = TextAnchor.MiddleLeft, int fontSize = 14, FontStyle fontStyle = FontStyle.Normal)
        {
            style = new GUIStyle(other);
            style.alignment = textAnchor;
            style.fontSize = fontSize;
            style.fontStyle = fontStyle;
            return style;
        }
    }
}
