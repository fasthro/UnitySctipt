/*
 * @Author: fasthro
 * @Date: 2019-06-06 17:11:35
 * @Description: 圆描述类
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityScript.UMath
{
    public class Circle
    {
        public Vector2 center;
        public float radius;

        public Circle(Vector2 center, float radius)
        {
            this.center = center;
            this.radius = radius;
        }
    }
}
