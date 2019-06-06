/*
 * @Author: fasthro
 * @Date: 2019-06-06 16:33:52
 * @Description: 数学库
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace UnityScript.UMath
{
    public class UMath
    {
        // 最小数常量(0.00001)z
        private const float EPSILON = 1e-05f;

        /// <summary>
        /// 外接圆的包围盒
        /// </summary>
        /// <param name="center"></param>
        /// <param name="radius"></param>
        /// <returns></returns>
        public static Rect CircleBoundBox(Vector2 center, float radius)
        {
            Rect bBox = new Rect();
            bBox.xMin = center.x - radius;
            bBox.xMax = center.x + radius;
            bBox.yMin = center.y - radius;
            bBox.yMax = center.y + radius;
            return bBox;
        }

        /// <summary>
        /// 三角形的外接圆
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <param name="p3"></param>
        /// <returns>Circle</returns>
        public static Circle TriangleCircumscribedCircle(Triangle triangle)
        {
            if (!UMath.IsEqualZero(triangle.point1.y - triangle.point2.y) || !UMath.IsEqualZero(triangle.point2.y - triangle.point3.y))
            {
                double yc, xc;
                float m1 = -(triangle.point2.x - triangle.point1.x) / (triangle.point2.y - triangle.point1.y);
                float m2 = -(triangle.point3.x - triangle.point2.x) / (triangle.point3.y - triangle.point2.y);
                double mx1 = (triangle.point1.x + triangle.point2.x) / 2.0;
                double mx2 = (triangle.point2.x + triangle.point3.x) / 2.0;
                double my1 = (triangle.point1.y + triangle.point2.y) / 2.0;
                double my2 = (triangle.point2.y + triangle.point3.y) / 2.0;

                if (UMath.IsEqualZero(triangle.point1.y - triangle.point2.y))
                {
                    xc = (triangle.point2.x + triangle.point1.x) / 2.0;
                    yc = m2 * (xc - mx2) + my2;
                }
                else if (UMath.IsEqualZero(triangle.point3.y - triangle.point2.y))
                {
                    xc = (triangle.point3.x + triangle.point2.x) / 2.0;
                    yc = m1 * (xc - mx1) + my1;
                }
                else
                {
                    xc = (m1 * mx1 - m2 * mx2 + my2 - my1) / (m1 - m2);
                    yc = m1 * (xc - mx1) + my1;
                }

                double dx = triangle.point2.x - xc;
                double dy = triangle.point2.y - yc;
                double rsqr = dx * dx + dy * dy;
                double r = Math.Sqrt(rsqr);

                return new Circle(new Vector2((float)xc, (float)yc), (float)r);
            }
            return new Circle(Vector2.zero, 0);
        }

        /// <summary>
		/// 计算圆切
		/// </summary>
		/// <param name="circle">C.</param>
		/// <param name="st">St.</param>
        /// <returns></returns>
		public static Vector2[] CalculateCircleTangency(Circle circle, Vector2 st)
        {
            Vector2 dir;
            float dis = (circle.center - st).magnitude;
            float temp = Mathf.Sqrt(dis * dis - circle.radius * circle.radius);
            float sina = temp / dis;
            float cosa = circle.radius / dis;
            dir.x = (st.x - circle.center.x) / dis * circle.radius;
            dir.y = (st.y - circle.center.y) / dis * circle.radius;

            Vector2[] res = new Vector2[2];

            res[0].x = circle.center.x + (dir.x * cosa - dir.y * sina);
            res[0].y = circle.center.y + (dir.x * sina + dir.y * cosa);

            res[1].x = circle.center.x + (dir.x * cosa + dir.y * sina);
            res[1].y = circle.center.y + (-dir.x * sina + dir.y * cosa);

            return res;
        }

        /// <summary>
        /// 判断矩形是否相交
        /// </summary>
        /// <param name="rec1">矩形1</param>
        /// <param name="rec2">矩形2</param>
        /// <returns>是否相交</returns>
        public static bool IsRectCross(Rect rec1, Rect rec2)
        {
            Rect ret = new Rect();
            ret.xMin = Mathf.Max(rec1.xMin, rec2.xMin);
            ret.xMax = Mathf.Min(rec1.xMax, rec2.xMax);
            ret.yMin = Mathf.Max(rec1.yMin, rec2.yMin);
            ret.yMax = Mathf.Min(rec1.yMax, rec2.yMax);

            if (ret.xMin > ret.xMax || ret.yMin > ret.yMax)
                return false;
            return true;
        }

        /// <summary>
        /// 线段是否相交
        /// </summary>
        /// <param name="line1"></param>
        /// <param name="line2"></param>
        /// <returns>is cross</returns>
        public static bool IsLineCross(Line2D line1, Line2D line2)
        {
            if (Math.Max(line1.point1.x, line1.point2.x) < Math.Min(line2.point1.x, line2.point2.x)
                || Math.Min(line1.point1.x, line1.point2.x) > Math.Max(line2.point1.x, line2.point2.x)
                || Math.Max(line1.point1.y, line1.point2.y) < Math.Min(line2.point1.y, line2.point2.y)
                || Math.Min(line1.point1.y, line1.point2.y) > Math.Max(line2.point1.y, line2.point2.y))
                return false;

            float temp1 = CrossProduct((line1.point1 - line2.point1), (line2.point2 - line2.point1)) * CrossProduct((line2.point2 - line2.point1), (line1.point2 - line2.point1));
            float temp2 = CrossProduct((line2.point1 - line1.point1), (line1.point2 - line1.point1)) * CrossProduct((line1.point2 - line1.point1), (line2.point2 - line1.point1));

            if ((temp1 >= 0) && (temp2 >= 0))
                return true;
            return false;
        }

        /// <summary>
        /// 线和三角形是否相交
        /// </summary>
        /// <param name="line">Line.</param>
        /// <param name="triangle">Tri.</param>
        /// <returns></returns>
        public static bool IsLineTriangleCross(Line2D line, Triangle triangle)
        {
            for (int i = 0; i < 3; i++)
            {
                if (IsLineCross(line, triangle.GetSide(i)))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 线段终点延长
        /// </summary>
        /// <param name="line"></param>
        /// <param name="length">延长长度</param>
        /// <returns></returns>
        public static Vector2 LineExtend(Line2D line, int length)
        {
            Vector2 newPos = line.point2;
            float slopeRate = Math.Abs((line.point2.y - line.point1.y) / (line.point2.x - line.point1.x));
            float xLength, yLength;
            if (slopeRate < 1)
            {
                yLength = length;
                xLength = length / slopeRate;
            }
            else
            {
                xLength = length;
                yLength = length * slopeRate;
            }

            if (line.point2.x > line.point1.x)
                newPos.x += xLength;
            else
                newPos.x -= xLength;

            if (line.point2.y > line.point1.y)
                newPos.y += yLength;
            else
                newPos.y -= yLength;

            return newPos;
        }

        /// <summary>
        /// 叉积
        /// </summary>
        /// <param name="p1">opsp</param>
        /// <param name="p2">opep</param>
        /// <returns></returns>
        public static float CrossProduct(Vector2 p1, Vector2 p2)
        {
            return (p1.x * p2.y - p1.y * p2.x);
        }

        /// <summary>
        /// 检查浮点数误差
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        public static bool IsEqualZero(double num)
        {
            return Math.Abs(num) < EPSILON;
        }

        /// <summary>
        /// 是否为(0, 0)点
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static bool IsEqualZero(Vector2 p)
        {
            return IsEqualZero(p.x) && IsEqualZero(p.y);
        }

        /// <summary>
        /// 是否为(0, 0, 0)点
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static bool IsEqualZero(Vector3 p)
        {
            return IsEqualZero(p.x) && IsEqualZero(p.y) && IsEqualZero(p.z);
        }
    }
}
