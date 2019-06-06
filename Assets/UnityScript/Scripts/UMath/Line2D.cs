/*
 * @Author: fasthro
 * @Date: 2019-06-06 17:49:44
 * @Description: 线段描述类
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityScript.UMath
{
    // 点与线的关系
    public enum PointSideRelation
    {
        // 在线段上
        ON_LINE,
        // 在线段左边
        LEFT_SIDE,
        // 在线段右边
        RIGHT_SIDE
    }

    /// <summary>
    /// 两线段交叉状态
    /// </summary>
    public enum LineCrossState
    {
        // 外线口
        COLINE,
        // 平行线
        PARALLEL,
        // 相交
        CROSS,
        // 无相交  
        NOT_CROSS
    }

    public class Line2D
    {
        private Vector2 m_point1;
        public Vector2 point1 { get { return m_point1; } }

        private Vector2 m_point2;
        public Vector2 point2 { get { return m_point2; } }

        public Vector2 direction
        {
            get
            {
                return this.point2 - this.point1;
            }
        }

        public float length
        {
            get
            {
                return Vector2.Distance(m_point1, m_point2);
            }
        }

        public Line2D(Vector2 point1, Vector2 point2)
        {
            this.m_point1 = point1;
            this.m_point2 = point2;
        }

        /// <summary>
        /// 判断点与直线的关系
        /// </summary>
        /// <param name="point">判断点</param>
        /// <returns></returns>
        public PointSideRelation ClassifyPoint(Vector2 point)
        {
            if (point == m_point1 || point == m_point2)
                return PointSideRelation.ON_LINE;

            Vector2 vectorA = m_point2 - m_point1;
            Vector2 vectorB = point - m_point1;
            float crossResult = UMath.CrossProduct(vectorA, vectorB);

            if (UMath.IsEqualZero(crossResult))
                return PointSideRelation.ON_LINE;
            else if (crossResult < 0)
                return PointSideRelation.RIGHT_SIDE;
            else
                return PointSideRelation.LEFT_SIDE;
        }

        /// <summary>
        /// 计算两条二维线段的交点
        /// </summary>
        /// <param name="other"></param>
        /// <param name="intersectPoint">线段交点</param>
        /// <returns></returns>
        public LineCrossState Intersection(Line2D other, out Vector2 intersectPoint)
        {
            intersectPoint.x = intersectPoint.y = float.NaN;

            if (!UMath.IsLineCross(this, other))
                return LineCrossState.NOT_CROSS;

            double A1, B1, C1, A2, B2, C2;

            A1 = point2.y - point1.y;
            B1 = point1.x - point2.x;
            C1 = point2.x * point1.y - point1.x * point2.y;

            A2 = other.point2.y - other.point1.y;
            B2 = other.point1.x - other.point2.x;
            C2 = other.point2.x * other.point1.y - other.point1.x * other.point2.y;

            if (UMath.IsEqualZero(A1 * B2 - B1 * A2))
            {
                if (UMath.IsEqualZero((A1 + B1) * C2 - (A2 + B2) * C1))
                    return LineCrossState.COLINE;
                else
                    return LineCrossState.PARALLEL;
            }

            intersectPoint.x = (float)((B2 * C1 - B1 * C2) / (A2 * B1 - A1 * B2));
            intersectPoint.y = (float)((A1 * C2 - A2 * C1) / (A2 * B1 - A1 * B2));
            return LineCrossState.CROSS;
        }
    }
}