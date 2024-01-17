# 更新日志 / ChangeLog

这里是 Motivation 包的更新日志！

## [1.0.0] - 2024-1-1
### 新增
* 全部主体内容
  * 状态控制器 `Motivator`
  * 横版2D模块家族
  * 简单输入模块 `SimpleInputModule`
  

## [1.0.1] - 2024-1-1
### 更改
* 将 `H2DBoxcastPhysicsModule` 更名为 `H2DCirclecastPhysicsModule` 以与实际行为匹配

### 修复
* 修复了 `Motivator` 的 `capableModules` 列表为 `null` 的bug
* 修复了从未打开过设置界面时，状态码编辑界面报错的bug
  

## [1.0.2] - 2024-1-2
### 更改
* 现在 `H2DPhysicsModuleBase` 将不会自动更改 `RigidBody2D` 的模式为 `Dynamic` 了
* 现在 `Motivator.Velocity` 不再与 `RigidBody2D` 绑定
  

## [1.0.3] - 2024-1-3
### 更改
* 将“参数”小标题移动至“键位”下方, 使继承 `H2DMoveModule` 或 `H2DJumpModule` 时添加的参数可以放在正确的位置  
* 将 H2D 系列模块的 `private` 成员改为 `protected`
* 取消了 `MotivatorState` 的 `partial` 修饰
* 允许 `H2DJumpModule` 的子类重写对【地面】的判定，以实现在其他情况下重置跳跃次数
  

## [1.0.4] - 2024-1-8
### 更改
* 增加模块的泛型获取方法
* 增加模块的泛型移除方法
* `Motivator` 现在不再强行要求物理模块被设置
* 将 `Motivator` 的大部分 `private` 成员改为 `protected`
  

## [1.0.5] - 2024-1-8
### 新增
* 增加了 `Motivator.Message(uint)` 与 `Module.OnMessage(uint)` 方法，作为跨模块信息传递的渠道之一 
  
### 更改
* 提供 `DelayedTrigger.Time` 和 `DelayedTrigger.Percent` 用于获取其内部计时状态
* 现在 `Motivator` 在更新激活的模块时，将不会调用未启用模块的 `OnEnter()` 和 `OnExit()` 方法
  

## [1.0.6] - 2024-1-8
### 新增
* 增加了 `Motivator` 在编辑器面板的状态显示
* 将层级选择器改用为原生的 `LayerMask`

### 修复
* 修复了在 `Module.OnEnter()` 和 `Module.OnExit()` 内更改 `Motivator` 的状态码导致堆栈溢出的bug
  

## [1.0.6] - 2024-1-8
### 新增
* 增加了 `Motivator` 在编辑器面板的状态显示
* 将层级选择器改用为原生的 `LayerMask`

### 修复
* 修复了在 `Module.OnEnter()` 和 `Module.OnExit()` 内更改 `Motivator` 的状态码导致堆栈溢出的bug


## [1.0.7] - 2024-1-9
### 新增
* 新的输入注册系统
  * 由原先从 `ControllerModule.OnAdd(Motivator)` 主动注册改为重写 `ControllerModule.GetRequiredKeys()` 被动提供
  * `InputModule` 现在拥有 `RequiredKeys` 属性，无需从 `Host` 读取
  * `InputModule` 可以重写 `OnKeyAdd` 和 `OnKeyRemove` 以监听需求按键的变化

### 更改
* 将 `Motivator.RegisterKeys(KeyCode[])` 和 `Motivator.RequiredKeys` 标记为 `[Obsolete]`


## [1.0.8] - 2024-1-16
### 新增
* `KeyMap` 按键映射机制
  * `KeyMap` 按键映射抽象类，指定按键应当如何在 【用户输入】和【`Motivator`需求】 之间进行映射
    * `DictionaryKeyMap`: 使用一个简单的字典来实现 `KeyMap` 的功能需求，需要继承并调用 `ApplyKeyMap(IDictionary<KeyCode,KeyCode>)` 方法
    * `StaticKeyMap`: 静态的 `KeyMap` ，仅保存在编辑器内，用于团队内不同操作习惯的成员进行测试
  * `KeyMapInputModule` ，自动映射不同的按键并提供给 `Motivator` 使用
* `H2DJumpModule` 增加了一个回调，在玩家跳跃并持续按住跳跃键时调用，可用于控制跳跃高度

### 更改
* `SimpleInputModule` 改为继承自 `KeyMapInputModule` 而非 `InputModule`

## [1.1.0] - 2024-1-17
### 新增
* 较为完备的 `KeyMap` 按键映射体系
  * `KeyMap` 及其子类: 按键映射关系的抽象类型
    * `EditorKeyMap`: 使用 `EditorPrefs` 存储的按键映射类，适合团队合作时不同成员采用不同的键位对游戏进行测试
    * `KeyPair` 和 `KeyPairs`: 用于定义按键组合的工具类
  * `KeyMapInputModule`: 支持自动应用按键映射的输入模块
* 适用于 `KeyPairs` 以及 `EditorKeyMap` 的编辑器UI改进

### 更改
* 将 `MotivationStateMaskEditor` 重命名为 `MotivationStateDrawer`