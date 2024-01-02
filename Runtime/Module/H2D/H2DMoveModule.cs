using System.Collections.Generic;
using Bingyan;
using UnityEngine;

namespace Motivation
{
    /// <summary>
    /// 横版2D模式的移动模块
    /// </summary>
    [CreateAssetMenu(fileName = "H2DMoveModule", menuName = "Motivation/H2D/Move", order = 1)]
    public class H2DMoveModule : ControllerModule
    {
        [Header("配置")]
        [SerializeField, Title("速度")] private float speed = 1;

        [SerializeField, Header("向左移动")] private List<KeyCode> leftKeys = new List<KeyCode> { KeyCode.A };
        [SerializeField, Header("向右移动")] private List<KeyCode> rightKeys = new List<KeyCode> { KeyCode.D };

        private bool leftBuffer = false, rightBuffer = false;

        public override void OnAdd(Motivator m)
        {
            base.OnAdd(m);
            Host.RegisterKeys(leftKeys.ToArray());
            Host.RegisterKeys(rightKeys.ToArray());
        }

        public override void PhysicsProcess(float time)
        {
            float dir = 0;
            if (leftBuffer) dir--;
            if (rightBuffer) dir++;
            if (dir != 0)
            {
                // 在Motiator本体坐标系下，将向左/向右分量设为 speed
                Host.Velocity += Host.RightDir * dir * speed - Host.VelocityRight;
            }

            leftBuffer = false;
            rightBuffer = false;
        }

        public override void InputKey(KeyCode key)
        {
            leftBuffer |= leftKeys.Contains(key);
            rightBuffer |= rightKeys.Contains(key);
        }

        public void InputLeft()
        {
            leftBuffer = true;
        }

        public void InputRight()
        {
            leftBuffer = true;
        }
    }
}