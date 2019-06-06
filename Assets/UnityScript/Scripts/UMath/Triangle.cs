/*
 * @Author: fasthro
 * @Date: 2019-06-06 17:39:02
 * @Description: 三角形描述类
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityScript.UMath
{
    public class Triangle
    {
        private Vector2 m_point1;
        public Vector2 point1 { get { return m_point1; } }

        private Vector2 m_point2;
        public Vector2 point2 { get { return m_point2; } }

        private Vector2 m_point3;
        public Vector2 point3 { get { return m_point3; } }

        private Vector2 m_center;
        public Vector2 center { get { return m_center; } }

        private Rect m_boundBox;
        public Rect BoundBox { get { return m_boundBox; } }

        public Triangle(Vector2 p1, Vector2 p2, Vector2 p3)
        {
            m_point1 = p1;
            m_point2 = p2;
            m_point3 = p3;

            //计算中心点
            Vector2 _center = new Vector2();
            _center.x = (this.m_point1.x + m_point2.x + m_point3.x) / 3;
            _center.y = (this.m_point1.y + m_point2.y + m_point3.y) / 3;
            this.m_center = _center;

            // 计算包围盒
            Rect _boundBox = new Rect();
            _boundBox.xMin = _boundBox.xMax = m_point1.x;
            _boundBox.yMin = _boundBox.yMax = m_point1.y;

            if (m_point2.x < _boundBox.xMin)
                _boundBox.xMin = m_point2.x;
            else if (m_point2.x > _boundBox.xMax)
                _boundBox.xMax = m_point2.x;

            if (m_point2.y < _boundBox.yMin)
                _boundBox.yMin = m_point2.y;
            else if (m_point2.y > _boundBox.yMax)
                _boundBox.yMax = m_point2.y;

            if (m_point3.x < _boundBox.xMin)
                _boundBox.xMin = m_point3.x;
            else if (m_point3.x > _boundBox.xMax)
                _boundBox.xMax = m_point3.x;

            if (m_point3.y < _boundBox.yMin)
                _boundBox.yMin = m_point3.y;
            else if (m_point3.y > _boundBox.yMax)
                _boundBox.yMax = m_point3.y;

            m_boundBox = _boundBox;
        }

        /// <summary>
        /// 根据索引获得相应的边
        /// </summary>
        /// <param name="sideIndex"></param>
        /// <returns></returns>
        public Line2D GetSide(int sideIndex)
        {
            Line2D newSide = null;
            switch (sideIndex)
            {
                case 0:
                    newSide = new Line2D(m_point1, m_point2);
                    break;
                case 1:
                    newSide = new Line2D(m_point2, m_point3);
                    break;
                case 2:
                    newSide = new Line2D(m_point3, m_point1);
                    break;
            }
            return newSide;
        }

        /// <summary>
        /// 测试给定点是否在三角形中(包括点在三角形边上)
        /// </summary>
        /// <param name="p">指定点</param>
        /// <returns></returns>
        public bool IsPointIn(Vector2 p)
        {
            if (m_boundBox.xMin != m_boundBox.xMax && !m_boundBox.Contains(p))
                return false;

            PointSideRelation relation1 = GetSide(0).ClassifyPoint(p);
            PointSideRelation relation2 = GetSide(1).ClassifyPoint(p);
            PointSideRelation relation3 = GetSide(2).ClassifyPoint(p);

            if (relation1 == PointSideRelation.ON_LINE
                || relation2 == PointSideRelation.ON_LINE
                || relation3 == PointSideRelation.ON_LINE)
                return true;
            else if (relation1 == PointSideRelation.RIGHT_SIDE
                && relation2 == PointSideRelation.RIGHT_SIDE
                && relation3 == PointSideRelation.RIGHT_SIDE)
                return true;

            return false;
        }
    }
}
