using System.Collections.Generic;
using Bingyan;
using UnityEngine;

namespace Motivation
{
    /// <summary>
    /// 横版2D模式下的跳跃模块
    /// </summary>
    [CreateAssetMenu(fileName = "H2DJumpModule", menuName = "Motivation/H2D/Jump", order = 2)]
    public class H2DJumpModule : ControllerModule
    {
        [Header("键位")]
        [SerializeField] protected List<KeyCode> jumpKeys = new List<KeyCode> { KeyCode.Space, KeyCode.W };

        [Header("配置")]
        [SerializeField, Title("跳跃向上速度")] protected int speed = 5;
        [SerializeField, Title("最大跳跃次数")] protected int maxJumpCnt = 2;
        [SerializeField, Title("按键缓冲时间")] protected float jumpBufferTime = 0.1f;
        [SerializeField, Title("土狼时间")] protected float coyotoTime = 0.1f;

        public int MaxJumpCount { get => maxJumpCnt; set => maxJumpCnt = value; }
        protected int jumpCnt = 0;

        protected virtual uint GroundStateMask { get; } = MotivatorState.Grounded;

        protected DelayedTrigger jumpBuffer, groundBuffer;
        protected bool isJumpPressed = false;

        public override void OnAdd(Motivator m)
        {
            base.OnAdd(m);
            jumpCnt = 0;

            jumpBuffer = new DelayedTrigger(jumpBufferTime);
            groundBuffer = new DelayedTrigger(coyotoTime);
            if (Host.MatchAny(GroundStateMask)) groundBuffer.Trigger();
        }

        public override void OnExit()
        {
            isJumpPressed = false;
        }

        public override KeyCode[] GetRequiredKeys() => jumpKeys.ToArray();

        public override void OnStateChange(uint state)
        {
            if (Host.MatchAny(GroundStateMask))
            {
                jumpCnt = 0;
                groundBuffer.Trigger();
            }
        }

        public override void Process(float time)
        {
            jumpBuffer.Tick(time);

            if (!Host.MatchAny(GroundStateMask))
                groundBuffer.Tick(time);
        }

        public override void PhysicsProcess(float time)
        {
            // 检查起跳
            if (jumpCnt < maxJumpCnt)
            {
                // 在地上 or 在天上且跳过一次
                if (jumpBuffer && (groundBuffer || jumpCnt > 0))
                {
                    jumpBuffer.Clear();
                    groundBuffer.Clear();
                    Jump();
                }
            }

            // 检查持续跳跃
            if (!Host.Grounded && isJumpPressed)
            {
                JumpProcess(time);
            }
        }

        public override void InputKeyDown(KeyCode key)
        {
            if (jumpKeys.Contains(key))
            {
                jumpBuffer.Trigger();
                isJumpPressed = true;
            }
        }

        public override void InputKeyUp(KeyCode key)
        {
            if (jumpKeys.Contains(key)) isJumpPressed = false;
        }

        protected virtual void Jump()
        {
            Host.Velocity -= Host.VelocityUp;
            Host.Velocity += Host.UpDir * speed;
            jumpCnt++;
        }

        protected virtual void JumpProcess(float time) { }
    }
}