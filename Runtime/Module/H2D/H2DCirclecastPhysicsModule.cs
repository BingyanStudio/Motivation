using Bingyan;
using UnityEngine;

namespace Motivation
{
    /// <summary>
    /// 横版2D模式的物理模块，使用圆形射线检测地面
    /// </summary>
    [CreateAssetMenu(fileName = "H2DCirclecastPhysicsModule", menuName = "Motivation/H2D/Physics - Circlecast", order = 0)]
    public class H2DCirclecastPhysicsModule : H2DPhysicsModuleBase
    {
        [Header("地面")]
        [SerializeField, Header("地面层级名称")] private string[] groundLayerNames = { "Ground" };
        [SerializeField, Title("额外检测距离")] private float groundDetDistance = 0.1f;
        [SerializeField, Title("地面检测半宽")] private float groundDetRadius = 0.5f;
        [SerializeField, Title("中心-脚底距离")] private float charactorExtent = 0.5f;
        [SerializeField, Title("最大斜坡角度")] private float maxSlopeDegree = 45;

        private int groundLayerMask = 0;

        public override void OnAdd(Motivator m)
        {
            base.OnAdd(m);
            groundLayerMask = LayerMask.GetMask(groundLayerNames);
        }

        public override void OnGizmos(Motivator m)
        {
            Gizmos.color = new Color(0, 255, 255, 0.5f);
            Gizmos.DrawSphere((Vector2)m.transform.position - m.UpDir * (charactorExtent - groundDetRadius + groundDetDistance), groundDetRadius);
        }

        protected override bool IsGrounded()
        {
            var bounds = Host.Col.bounds;
            var dir = -Host.UpDir;

            // 碰撞箱底部中心的坐标
            var result = Physics2D.CircleCast(bounds.center, groundDetRadius, dir, charactorExtent - groundDetRadius + groundDetDistance, groundLayerMask);
            return result.collider
                && Vector2.Angle(Host.transform.TransformDirection(Vector3.up), result.normal) <= maxSlopeDegree;
        }
    }
}