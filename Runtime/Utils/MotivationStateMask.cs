using System;
using UnityEngine;

namespace Motivation
{
    /// <summary>
    /// 可以暴露给编辑器的位掩码。<br/>
    /// 利用这个，可以指定当 <see cref="Motivator"/> 的状态处于特定规则时，<see cref="ControllerModule"/> 开始工作
    /// </summary>
    [Serializable]
    public class MotivationStateMask
    {
        [SerializeField] private MaskRequirement[] requirements;

        /// <summary>
        /// 获取位掩码
        /// </summary>
        /// <returns>
        /// 三元组, 共 3 个uint掩码<br/>
        /// 第一个是用于筛选出【有要求】的状态量的掩码<br/>
        /// 第二个是用于筛选出【值为1】的状态量的掩码<br/>
        /// 第三个是用于筛选出【值为0】的状态量的掩码<br/>
        /// 可以参考 <see cref="ControllerModule.IsCapable(uint)"/> 来理解这几个掩码的作用<br/>
        /// </returns>
        public (uint, uint, uint) GetMasks()
        {
            uint selector = 0, trues = 0, falses = 0;
            for (int i = 0; i < 32; i++)
            {
                if (requirements[i] == MaskRequirement.Unrestricted) continue;
                selector |= 1u << i;
                if (requirements[i] == MaskRequirement.True) trues |= 1u << i;
                else falses |= 1u << i;
            }
            return (selector, trues, falses);
        }
    }

    /// <summary>
    /// 掩码的匹配规则
    /// </summary>
    public enum MaskRequirement
    {
        [InspectorName("不设限")]
        Unrestricted,

        [InspectorName("是")]
        True,

        [InspectorName("否")]
        False
    }
}