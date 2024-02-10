using System.Linq;
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
        [Header("键位")]
        [SerializeField, Header("向左移动")] protected List<KeyCode> leftKeys = new List<KeyCode> { KeyCode.A };
        [SerializeField, Header("向右移动")] protected List<KeyCode> rightKeys = new List<KeyCode> { KeyCode.D };


        [Header("配置")]
        [SerializeField, Title("速度")] protected float speed = 1;

        protected bool leftBuffer = false, rightBuffer = false;

        public override KeyCode[] GetRequiredKeys() => leftKeys.Union(rightKeys).ToArray();

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
        }

        public override void InputKeyDown(KeyCode key)
        {
            if (leftKeys.Contains(key)) leftBuffer = true;
            if (rightKeys.Contains(key)) rightBuffer = true;
        }

        public override void InputKeyUp(KeyCode key)
        {
            if (leftKeys.Contains(key)) leftBuffer = false;
            if (rightKeys.Contains(key)) rightBuffer = false;
        }
    }
}