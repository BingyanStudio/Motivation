using Bingyan;
using UnityEngine;

namespace Motivation
{
    /// <summary>
    /// 横版2D物理模块的基类。提供了大部分可能复用的功能，但需要自行实现地面检测。
    /// </summary>
    public abstract class H2DPhysicsModuleBase : PhysicsModule
    {
        [Header("重力")]
        [SerializeField, Title("重力加速度")] private float gravity = 10;
        [SerializeField, Title("重力方向")] private Vector2 gravityDirection = Vector2.down;

        [Header("阻力")]
        [SerializeField, Title("地面阻尼")] private float groundDamp = 0.1f;
        [SerializeField, Title("空中横向阻尼")] private float airHDamp = 0.1f;
        [SerializeField, Title("空中纵向阻尼")] private float airVDamp = 0.1f;
        [SerializeField, Title("水中横向阻尼")] private float waterHDamp = 0.1f;
        [SerializeField, Title("水中纵向阻尼")] private float waterVDamp = 0.1f;

        public Vector2 GravityDirection { get => gravityDirection; set => gravityDirection = value.normalized; }

        public override void OnAdd(Motivator m)
        {
            base.OnAdd(m);
            m.Rigid.bodyType = RigidbodyType2D.Dynamic;
            m.Rigid.gravityScale = 0;
        }

        public override void PhysicsProcess(float time)
        {
            GroundDetection();
            ApplyGravity(time);
            ApplyDamping(time);
        }

        private void GroundDetection()
        {
            if (IsGrounded())
            {
                if (!Host.Grounded) Host.AddState(MotivatorState.Grounded);
            }
            else if (Host.Grounded) Host.RemoveState(MotivatorState.Grounded);
        }

        private void ApplyGravity(float time)
        {
            if (Host.Grounded)
            {
                // 清除 y 轴速度
                Host.Velocity -= Host.VelocityUp;
            }
            else
            {
                // 重力加速
                var dir = (Vector2)Host.transform.TransformDirection(gravityDirection);
                Host.Velocity += dir * gravity * time;
            }
        }

        private void ApplyDamping(float time)
        {
            float hd, vd;

            if (Host.InWater)
            {
                hd = waterHDamp;
                vd = waterVDamp;
            }
            else if (Host.Grounded) hd = vd = groundDamp;
            else
            {
                hd = airHDamp;
                vd = airVDamp;
            }
            var damp = Host.VelocityUp * vd + Host.VelocityRight * hd;
            Host.Velocity -= damp * time;
        }

        protected abstract bool IsGrounded();
    }
}