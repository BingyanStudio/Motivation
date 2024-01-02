using UnityEngine;

namespace Motivation
{
    /// <summary>
    /// 所有定义Motivator行为的模块的基类
    /// </summary>
    public abstract class Module : ScriptableObject
    {
        /// <summary>
        /// 是否启用<br/>
        /// 当设置为 <see cref = "false"/> 时，这个模块将不再执行 <see cref="Process"/>, <see cref="PhysicsProcess"/>, <see cref="OnStateChange(uint)"/>和 所有的监听输入的函数
        /// </summary>
        public bool Active { get; set; } = true;

        /// <summary>
        /// 这个模块的所有者
        /// </summary>
        protected Motivator Host { get; private set; }

        // 这个模组是由谁复制而来的? 
        private Module origin;

        /// <summary>
        /// 当这个模块安装到 <see cref="Motivator"/> 时触发<br/>
        /// 重写时需要调用 base.Init(m)
        /// </summary>
        /// <param name="m">拥有这个模块的 <see cref="Motivator"/></param>
        public virtual void OnAdd(Motivator m) => Host = m;

        /// <summary>
        /// 绘制辅助线的回调
        /// </summary>
        /// <param name="m">拥有这个模块的 <see cref="Motivator"/></param>
        public virtual void OnGizmos(Motivator m) { }

        /// <summary>
        /// 当模块被移除的时候的回调
        /// </summary>
        public virtual void OnRemove() => Host = null;

        /// <summary>
        /// 更新回调，相当于 Update
        /// </summary>
        /// <param name="time">经过的时间，相当于 <see cref="Time.deltaTime"/></param>
        public virtual void Process(float time) { }

        /// <summary>
        /// 物理更新回调，相当于 FixedUpdate
        /// </summary>
        /// <param name="time">经过的时间，相当于 <see cref="Time.fixedDeltaTime"/></param>
        public virtual void PhysicsProcess(float time) { }

        /// <summary>
        /// 当 <see cref="Motivator"/> 状态更新时触发的回调
        /// </summary>
        /// <param name="state">新的状态</param>
        public virtual void OnStateChange(uint state) { }

        /// <summary>
        /// 复制这个模组
        /// </summary>
        /// <returns>复制的模块</returns>
        public virtual Module Copy()
        {
            var inst = Instantiate(this);
            inst.origin = this;
            return inst;
        }

        /// <summary>
        /// 重新加载这个模组<br/>
        /// 实际上就是获取到这个模组的源头，将它重新复制一份并返回
        /// </summary>
        /// <returns>重载后的模组</returns>
        public Module Reload()
        {
            if (origin) return origin.Copy();
            Debug.LogError($"模块 {this} 未被复制过! ");
            return null;
        }
    }

    /// <summary>
    /// 所有输入模块的基类。使用输入模块来向 <see cref="Motivator"/> 发送输入事件
    /// </summary>
    public abstract class InputModule : Module
    {
        /// <summary>
        /// 向 <see cref="Motivator"/> 发送【键盘持续按下】事件
        /// </summary>
        /// <param name="k">按键</param>
        protected void OnKey(KeyCode k) => Host.OnKey(k);

        /// <summary>
        /// 向 <see cref="Motivator"/> 发送【键盘刚刚按下】事件
        /// </summary>
        /// <param name="k">按键</param>
        protected void OnKeyDown(KeyCode k) => Host.OnKeyDown(k);

        /// <summary>
        /// 向 <see cref="Motivator"/> 发送【键盘刚刚抬起】事件
        /// </summary>
        /// <param name="k">按键</param>
        protected void OnKeyUp(KeyCode k) => Host.OnKeyUp(k);

        /// <summary>
        /// 向 <see cref="Motivator"> 发送一个方向矢量
        /// </summary>
        /// <param name="dir">方向矢量</param>
        protected void OnAxis(Vector2 dir) => Host.OnAxis(dir);

        /// <summary>
        /// 向 <see cref="Motivator"> 发送一个【鼠标持续按下】事件
        /// </summary>
        /// <param name="m">按键</param>
        /// <param name="pos">鼠标在屏幕坐标下的位置</param>
        protected void OnMouse(int m, Vector2 pos) => Host.OnMouse(m, pos);

        /// <summary>
        /// 向 <see cref="Motivator"> 发送一个【鼠标刚刚按下】事件
        /// </summary>
        /// <param name="m">按键</param>
        /// <param name="pos">鼠标在屏幕坐标下的位置</param>
        protected void OnMouseDown(int m, Vector2 pos) => Host.OnMouse(m, pos);

        /// <summary>
        /// 向 <see cref="Motivator"> 发送一个【鼠标刚刚抬起】事件
        /// </summary>
        /// <param name="m">按键</param>
        /// <param name="pos">鼠标在屏幕坐标下的位置</param>
        protected void OnMouseUp(int m, Vector2 pos) => Host.OnMouse(m, pos);
    }

    /// <summary>
    /// 所有物理模块的基类。使用物理模块来决定 <see cref="Motivator"/> 的物理行为，例如重力等
    /// </summary>
    public abstract class PhysicsModule : Module { }

    /// <summary>
    /// 所有控制模块的基类。使用控制模块来控制 <see cref="Motivator"/> 的移动，跳跃等行为
    /// </summary>
    public abstract class ControllerModule : Module
    {
        [SerializeField] private MotivationStateMask mask;

        protected (uint, uint, uint) masks;

        public override void OnAdd(Motivator m)
        {
            base.OnAdd(m);
            masks = mask.GetMasks();
        }

        /// <summary>
        /// 判定目前 <see cref="Motivator"/> 的状态是否与 <see cref="mask"/> 要求的状态相符
        /// </summary>
        /// <param name="state">当前 <see cref="Motivator"/> 的状态</param>
        /// <returns>是否相符</returns>
        public virtual bool IsCapable(uint state)
        {
            if (!Active) return false;
            var maskedState = state & masks.Item1;
            return (maskedState & masks.Item2) == masks.Item2 && (~maskedState & masks.Item3) == masks.Item3;
        }

        /// <summary>
        /// 当这个模块被启用的时候触发的回调<br/>
        /// 【启用】意味着此时 Motivator 的状态码符合该模块的 mask
        /// </summary>
        public virtual void OnEnter() { }

        /// <summary>
        /// 当这个模块被禁用的时候触发的回调<br/>
        /// 【禁用】意味着此时 Motivator 的状态码不符合该模块的 mask
        /// </summary>
        public virtual void OnExit() { }

        /// <summary>
        /// 接收到 【按键持续按下】 事件时的回调
        /// </summary>
        /// <param name="key">按键</param>
        public virtual void InputKey(KeyCode key) { }

        /// <summary>
        /// 接收到 【按键刚刚按下】 事件时的回调
        /// </summary>
        /// <param name="key">按键</param>
        public virtual void InputKeyDown(KeyCode key) { }

        /// <summary>
        /// 接收到 【按键刚刚松开】 事件时的回调
        /// </summary>
        /// <param name="key">按键</param>
        public virtual void InputKeyUp(KeyCode key) { }

        /// <summary>
        /// 接收到方向矢量的回调
        /// </summary>
        /// <param name="dir">方向矢量</param>
        public virtual void InputAxis(Vector2 dir) { }

        /// <summary>
        /// 接收到 【鼠标持续按下】 事件时的回调
        /// </summary>
        /// <param name="mouse">按键</param>
        /// <param name="pos">鼠标在屏幕坐标下的位置</param>
        public virtual void InputMouse(int mouse, Vector2 pos) { }

        /// <summary>
        /// 接收到 【鼠标刚刚按下】 事件时的回调
        /// </summary>
        /// <param name="mouse">按键</param>
        /// <param name="pos">鼠标在屏幕坐标下的位置</param>
        public virtual void InputMouseDown(int mouse, Vector2 pos) { }

        /// <summary>
        /// 接收到 【鼠标刚刚松开】 事件时的回调
        /// </summary>
        /// <param name="mouse">按键</param>
        /// <param name="pos">鼠标在屏幕坐标下的位置</param>
        public virtual void InputMouseUp(int mouse, Vector2 pos) { }
    }
}