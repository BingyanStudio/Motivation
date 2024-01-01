namespace Motivation
{
    /// <summary>
    /// 一个延迟计时器，在一段时间内保存 <see cref="bool"/> 变量<br/>
    /// 可以直接作为 <see cref="bool"/> 类型变量使用<br/>
    /// 适合用于制作按键延迟、土狼时间等功能<br/>
    /// 用例参考: <see cref="H2DJumpModule"/> 
    /// </summary>
    public class DelayedTrigger
    {
        private float value;
        private float timer = 0;

        /// <summary>
        /// 一个延迟计时器，在一段时间内保存 <see cref="bool"/> 变量<br/>
        /// 适合用于制作按键延迟、土狼时间等功能<br/>
        /// 用例参考: <see cref="H2DJumpModule"/> 
        /// </summary>
        /// <param name="bufferedTime">延迟的最长时间</param>
        public DelayedTrigger(float bufferedTime)
        {
            value = bufferedTime;
        }

        /// <summary>
        /// 更新这个计时器
        /// </summary>
        /// <param name="time">两帧之间的时间，往往是 <see cref="UnityEngine.Time.deltaTime"/></param>
        public void Tick(float time)
        {
            if (timer > 0)
            {
                timer -= time;
                if (timer < 0) timer = 0;
            }
        }

        /// <summary>
        /// 触发这个计时器，使之变为 <see cref="true"/> 并持续一段时间
        /// </summary>
        public void Trigger()
        {
            timer = value;
        }

        /// <summary>
        /// 将这个计时器清空
        /// </summary>
        public void Clear()
        {
            timer = 0;
        }

        public static implicit operator bool(DelayedTrigger trigger) => trigger.timer > 0;
    }
}