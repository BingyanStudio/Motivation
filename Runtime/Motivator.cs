using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using Bingyan;

namespace Motivation
{
    /// <summary>
    /// 一个模块化的角色控制器<br/>
    /// 可以根据定制的物理模块、输入模块和控制模块执行自定义的行为
    /// </summary>
    [RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
    public class Motivator : ProcessableMono
    {
        [Header("基础")]
        [SerializeField, Title("自主运行")] private bool automatic = true;
        [SerializeField, Title("输入模块")] private InputModule preInputModule;
        [SerializeField, Title("物理模块")] private PhysicsModule prePhysicsModule;
        [SerializeField, Title("时间尺度")] private float timeScale = 1;

        [Header("预安装模块")]
        [SerializeField] private ControllerModule[] preModules;

        [Header("Debug")]
        [SerializeField, Title("输出细节信息")] protected bool printLog = false;
        [SerializeField, Title("绘制辅助图形")] protected bool gizmos = false;

        /// <summary>
        /// 这个 <see cref="Motivator"/> 目前的状态码<br/>
        /// <see cref="Motivator"/> 的状态由32位状态码决定，每一位都能作为一个“是否”状态使用<br/>
        /// <see cref="ControllerModule"/> 在更新前会先依据这个值判定，如果符合它的工作状态，才会读取输入/进行更新<br/>
        /// 参考: <seealso cref="ControllerModule"/><br/>
        /// 内置的状态有: <br/>
        /// 第0位: 是否在地面上<br/>
        /// 第1位: 是否在水中
        /// </summary>
        public uint State => state;
        private uint state = MotivatorState.Grounded;

        /// <summary>
        /// 这个角色的本体时间尺度
        /// </summary>
        /// <value></value>
        public float TimeScale { get => timeScale; set => timeScale = value; }

        /// <summary>
        /// 这个角色的输入模块
        /// </summary>
        public InputModule Input
        {
            get => inputModule;
            set => inputModule = InitModule(value);
        }

        /// <summary>
        /// 这个角色的物理模块
        /// </summary>
        public PhysicsModule Physics
        {
            get => physicsModule;
            set => physicsModule = InitModule(value);
        }

        /// <summary>
        /// 这个物体的刚体
        /// </summary>
        public Rigidbody2D Rigid => rb;

        /// <summary>
        /// 这个物体的碰撞箱
        /// </summary>
        public Collider2D Col => col;

        /// <summary>
        /// 这个物体的速度
        /// </summary>
        public Vector2 Velocity { get; set; }

        /// <summary>
        /// 这个物体速度在本地坐标系的纵向分量
        /// </summary>
        public Vector2 VelocityUp => UpDir * Vector2.Dot(UpDir, Velocity);

        /// <summary>
        /// 这个物体的速度在本地坐标系的横向分量
        /// </summary>
        public Vector2 VelocityRight => RightDir * Vector2.Dot(RightDir, Velocity);

        /// <summary>
        /// 这个物体本地坐标系下的“上”方向
        /// </summary>
        public Vector2 UpDir => transform.TransformDirection(Vector3.up);

        /// <summary>
        /// 这个物体本地坐标系下的“右”方向
        /// </summary>
        public Vector2 RightDir => transform.TransformDirection(Vector3.right);

        private Rigidbody2D rb;
        private Collider2D col;

        /// <summary>
        /// 角色目前是否在地面上
        /// </summary>
        public bool Grounded => MatchAny(MotivatorState.Grounded);

        /// <summary>
        /// 角色目前是否在水中
        /// </summary>
        public bool InWater => MatchAny(MotivatorState.Diving);

        private Dictionary<Type, ControllerModule> modules = new();
        private InputModule inputModule;
        private PhysicsModule physicsModule;

        /// <summary>
        /// 这个 Motivator 运行所需要的所有键盘按键，由各个控制模块注册
        /// </summary>
        public List<KeyCode> RequiredKeys => requiredKeys;
        private List<KeyCode> requiredKeys = new List<KeyCode>();

        private List<ControllerModule> capableModules = new();

        protected virtual void Start()
        {
            // 获取组件
            rb = GetComponent<Rigidbody2D>();
            col = GetComponent<Collider2D>();

            // 初始化模块
            if (!prePhysicsModule)
            {
                Debug.LogError($"{name} 缺少 PhysicsModule, 无法运行!");
                enabled = false;
                return;
            }
            LoadAllModules();
        }

        private void Update()
        {
            if (automatic) Process(Time.deltaTime);
        }

        private void FixedUpdate()
        {
            if (automatic) PhysicsProcess(Time.fixedDeltaTime);
        }

        private void OnDrawGizmos()
        {
            if (!gizmos) return;
            physicsModule?.OnGizmos(this);
            inputModule?.OnGizmos(this);
            foreach (var item in modules)
                item.Value.OnGizmos(this);
        }

        public override void Process(float delta)
        {
            delta *= TimeScale;
            if (physicsModule.Active) physicsModule.Process(delta);
            if (inputModule.Active) inputModule?.Process(delta);
            capableModules.ForEach(i =>
            {
                if (i.Active) i.Process(delta);
            });
        }

        public override void PhysicsProcess(float delta)
        {
            delta *= TimeScale;
            if (physicsModule.Active) physicsModule.PhysicsProcess(delta);
            if (inputModule.Active) inputModule?.PhysicsProcess(delta);
            capableModules.ForEach(i =>
            {
                if (i.Active) i.PhysicsProcess(delta);
            });

            Rigid.MovePosition(Rigid.position + Velocity * delta);
        }

        /// <summary>
        /// 为这个 <see cref="Motivator"/> 添加一个控制模块 <br/>
        /// 实际上，添加时，输入的控制模块将会首先被复制。这是为了让它可以保存自己的状态。<br/>
        /// 因此，这个方法将会返回复制后的模块，也就是实际发挥作用的模块<br/>
        /// </summary>
        /// <param name="module">要添加的模块</param>
        /// <returns>复制后的模块</returns>
        public ControllerModule AddModule(ControllerModule module, bool update = true)
        {
            var mod = module.Copy() as ControllerModule;
            var t = mod.GetType();
            if (modules.ContainsKey(t))
            {
                modules[t].OnRemove();
                modules[t] = mod;
                if (printLog) Debug.Log($"{name}: 更新了 {t} 模块");
            }
            else
            {
                modules.Add(t, mod);
                if (printLog) Debug.Log($"{name}: 安装了 {t} 模块");
            }
            mod.OnAdd(this);
            if (update) UpdateCapableModules();
            return mod;
        }

        /// <summary>
        /// 移除指定类型的控制模块
        /// </summary>
        /// <param name="t">类型</param>
        public void RemoveModule(Type t, bool update = true)
        {
            if (modules.ContainsKey(t))
            {
                modules[t].OnRemove();
                modules.Remove(t);
                if (update) UpdateCapableModules();
                if (printLog) Debug.Log($"{name}: 移除了 {t} 模块");
            }
            else Debug.LogWarning($"{name}: 没有 {t} 模块, 但仍调用代码移除");
        }

        /// <summary>
        /// 移除指定的控制模块
        /// </summary>
        /// <param name="module">这个模块的任意一个实体。实际起作用的是module的类型。</param>
        public void RemoveModule(ControllerModule module) => RemoveModule(module.GetType());

        /// <summary>
        /// 移除指定类型的控制模块
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        public void RemoveModule<T>() where T : Module
        {
            RemoveModule(typeof(T), true);
        }

        /// <summary>
        /// 获取指定类型的模块
        /// </summary>
        /// <param name="t">指定的类型</param>
        /// <returns>获取的模块。可能为 <see cref="null"/></returns>
        public Module GetModule(Type t)
        {
            if (inputModule.GetType() == t) return inputModule;
            if (physicsModule.GetType() == t) return physicsModule;
            if (modules.ContainsKey(t)) return modules[t];
            Debug.LogError($"{name} 没有 {t} 模块");
            return null;
        }

        /// <summary>
        /// 获取指定的模块
        /// </summary>
        /// <param name="module">这个模块的任意一个实体。实际起作用的是template的类型。</param>
        public Module GetModule(Module template) => GetModule(template.GetType());

        /// <summary>
        /// 获取指定类型的模块
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        public T GetModule<T>() where T : Module
            => GetModule(typeof(T)) as T;

        /// <summary>
        /// 激活所有的模块
        /// </summary>
        public void ActiveAllModules()
        {
            inputModule.Active = true;
            physicsModule.Active = true;
            foreach (var item in modules)
                item.Value.Active = true;
        }

        /// <summary>
        /// 禁用所有的模块
        /// </summary>
        public void DeactivateAllModules()
        {
            inputModule.Active = false;
            physicsModule.Active = false;
            foreach (var item in modules)
                item.Value.Active = false;
        }

        /// <summary>
        /// 为这个 <see cref="Motivator"/> 添加一个状态。<br/>
        /// 实际上是依据输入的 state 与本身 state 进行或运算
        /// </summary>
        /// <param name="state">要改变的状态位</param>
        public void AddState(uint state)
        {
            // 按位或, 让state中特定几项变为1
            this.state |= state;
            NotifyStateChanged();
        }

        /// <summary>
        /// 为这个 <see cref="Motivator"/> 移除一个状态。<br/>
        /// 实际上是依据输入的 state 与本身 state 的取反进行与运算
        /// </summary>
        /// <param name="state">要改变的状态位</param>
        public void RemoveState(uint state)
        {
            // 先取反再按位与, 让state中的特定几项变为0
            this.state &= ~state;
            NotifyStateChanged();
        }

        /// <summary>
        /// 提醒所有的控制模块，当前 <see cref="Motivator"/> 的状态发生了改变<br/>
        /// 同时更新“当前状态下可用模块”列表
        /// </summary>
        private void NotifyStateChanged()
        {
            if (physicsModule.Active) physicsModule.OnStateChange(state);
            if (inputModule.Active) inputModule.OnStateChange(state);
            foreach (var item in modules)
                if (item.Value.Active)
                    item.Value.OnStateChange(state);

            // 更新在当前state下可以执行的模块
            UpdateCapableModules();
        }

        /// <summary>
        /// 判断输入的状态是否和本身的状态有交集，即进行或运算，如果结果不为0，则返回 true
        /// </summary>
        /// <param name="state">输入的状态</param>
        /// <returns>是否有交集</returns>
        public bool MatchAny(uint state) => (this.state & state) != 0;

        /// <summary>
        /// 判断输入的状态是否和本身的状态是否完全一致
        /// </summary>
        /// <param name="state">输入的状态</param>
        /// <returns>是否有完全一致</returns>
        public bool MatchAll(uint state) => this.state == state;

        /// <summary>
        /// 向所有的控制模块发送【键盘被持续按下】的消息
        /// </summary>
        /// <param name="key">哪个按键?</param>
        public void OnKey(KeyCode key)
        {
            foreach (var item in modules)
            {
                if (item.Value.Active)
                    item.Value.InputKey(key);
            }
        }

        /// <summary>
        /// 向所有可用的控制模块发送【键盘刚刚按下】的消息
        /// </summary>
        /// <param name="key">哪个按键?</param>
        public void OnKeyDown(KeyCode key)
        {
            capableModules.ForEach(i => { if (i.Active) i.InputKeyDown(key); });
        }

        /// <summary>
        /// 向所有可用的控制模块发送【键盘刚刚抬起】的消息
        /// </summary>
        /// <param name="key">哪个按键?</param>
        public void OnKeyUp(KeyCode key)
        {
            capableModules.ForEach(i => { if (i.Active) i.InputKeyUp(key); });
        }

        /// <summary>
        /// 向所有的控制模块发送【鼠标被持续按下】的消息
        /// </summary>
        /// <param name="mouse">哪个键?</param>
        /// <param name="pos">鼠标在屏幕坐标系下的坐标</param>
        public void OnMouse(int mouse, Vector2 pos)
        {
            capableModules.ForEach(i => { if (i.Active) i.InputMouse(mouse, pos); });
        }

        /// <summary>
        /// 向所有的控制模块发送【鼠标刚刚按下】的消息
        /// </summary>
        /// <param name="mouse">哪个键?</param>
        /// <param name="pos">鼠标在屏幕坐标系下的坐标</param>
        public void OnMouseDown(int mouse, Vector2 pos)
        {
            capableModules.ForEach(i => { if (i.Active) i.InputMouseDown(mouse, pos); });
        }

        /// <summary>
        /// 向所有的控制模块发送【鼠标刚刚抬起】的消息
        /// </summary>
        /// <param name="mouse">哪个键?</param>
        /// <param name="pos">鼠标在屏幕坐标系下的坐标</param>
        public void OnMouseUp(int mouse, Vector2 pos)
        {
            capableModules.ForEach(i => { if (i.Active) i.InputMouseUp(mouse, pos); });
        }

        /// <summary>
        /// 向所有的控制模块发送一个方向矢量，可用于适配摇杆等
        /// </summary>
        /// <param name="axis">方向矢量</param>
        public void OnAxis(Vector2 axis)
        {
            capableModules.ForEach(i => { if (i.Active) i.InputAxis(axis); });
        }

        /// <summary>
        /// 注册一个键，由控制模块在初始化时调用<br/>
        /// 注册后，可以通过 <see cref="RequiredKeys"/> 访问该 <see cref="Motivator"/> 运行时所需要的键盘按键<br/>
        /// <see cref="InputModule"/> 往往依赖这个，以注册相关的输入<br/>
        /// 参考这个用旧输入系统写的输入模块: <see cref="SimpleInputModule"/>
        /// </summary>
        /// <param name="codes">所有需要注册的按键代码</param>
        public void RegisterKeys(params KeyCode[] codes)
        {
            foreach (var code in codes)
            {
                if (!requiredKeys.Contains(code)) requiredKeys.Add(code);
                else
                {
                    // TODO: 需要判断冲突
                    Debug.LogWarning($"按键 {code} 疑似冲突");
                }
            }
        }

        /// <summary>
        /// 打印所有模块的状态信息，用于排除bug
        /// </summary>
        public void PrintModuleState()
        {
            Debug.Log($"输入模块: {inputModule.GetType()}, 是否启用: {inputModule.Active}");
            Debug.Log($"物理模块: {physicsModule.GetType()}, 是否启用: {physicsModule.Active}");
            Debug.Log("控制模块: ");
            foreach (var item in modules)
                Debug.Log($"{item.Key}, 是否启用: {item.Value.Active}");
        }

        // 用于初始化模块的工具方法
        private T InitModule<T>(T m) where T : Module
        {
            var cpy = m.Copy();
            cpy.OnAdd(this);
            return cpy as T;
        }

        private void UpdateCapableModules()
        {
            var newCapableModules = modules.Where(i => i.Value.IsCapable(state)).Select(i => i.Value).ToList();
            foreach (var exited in capableModules.Except(newCapableModules)) exited.OnExit();
            foreach (var entered in newCapableModules.Except(capableModules)) entered.OnEnter();
            capableModules = newCapableModules;
        }

        // 用于加载所有模块的工具方法
        public void LoadAllModules()
        {
            requiredKeys = new();

            if (preInputModule) inputModule = InitModule(preInputModule);
            physicsModule = InitModule(prePhysicsModule);
            foreach (var item in preModules)
                AddModule(item, false);

            UpdateCapableModules();
        }
    }

    /// <summary>
    /// 内置的状态码常量，防止硬编码
    /// </summary>
    public static class MotivatorState
    {
        public const uint Grounded = 1;
        public const uint Diving = 0b10;
    }
}