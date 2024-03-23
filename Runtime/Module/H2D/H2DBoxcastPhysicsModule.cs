using Bingyan;
using UnityEngine;

namespace Motivation
{
    /// <summary>
    /// 横版2D模式的物理模块，使用箱形射线检测地面
    /// </summary>
    [CreateAssetMenu(fileName = "H2DBoxcastPhysicsModule", menuName = "Motivation/H2D/Physics - Boxcast", order = 1)]
    public class H2DBoxcastPhysicsModule : H2DPhysicsModuleBase
    {
        [Header("地面")]
        [SerializeField, Title("地面层级")] protected LayerMask groundLayers;
        [SerializeField, Title("额外检测距离")] protected float groundDetDistance = 0.1f;
        [SerializeField, Title("检测宽度")] protected float groundDetWidth = 0.5f;
        [SerializeField, Title("最大斜坡角度")] protected float maxSlopeDegree = 45;

        public override void OnGizmos(Motivator m)
        {
            Gizmos.color = new Color(0, 255, 255, 0.5f);

            var bounds = Host.Col.bounds;
            Gizmos.DrawCube((Vector2)bounds.center - Host.UpDir * (bounds.extents.y + groundDetDistance / 2), new(groundDetWidth, groundDetDistance, 0));
        }

        protected override bool IsGrounded()
        {
            return IsGrounded(groundLayers);
        }

        protected virtual bool IsGrounded(LayerMask mask)
        {
            var bounds = Host.Col.bounds;
            var dir = -Host.UpDir;

            // 碰撞箱底部中心的坐标
            var result = Physics2D.BoxCast((Vector2)bounds.center + dir * bounds.extents.y, new Vector2(groundDetWidth, 1e-3f), 0, dir, groundDetDistance, mask);
            return result.collider
                && Vector2.Angle(Host.transform.TransformDirection(Vector3.up), result.normal) <= maxSlopeDegree;

        }
    }
}