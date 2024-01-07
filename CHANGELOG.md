# 更新日志 / ChangeLog

这里是 Motivation 包的更新日志！

## [1.0.0] - 2024-1-1
### 新增
* 全部主体内容
  * 状态控制器 `Motivator`
  * 横版2D模块家族
  * 简单输入模块 `SimpleInputModule`

## [1.0.1] - 2024-1-1
### 修改
* 将 `H2DBoxcastPhysicsModule` 更名为 `H2DCirclecastPhysicsModule` 以与实际行为匹配

### 修复
* 修复了 `Motivator` 的 `capableModules` 列表为 `null` 的bug
* 修复了从未打开过设置界面时，状态码编辑界面报错的bug

## [1.0.2] - 2024-1-2
### 修改
* 现在 `H2DPhysicsModuleBase` 将不会自动修改 `RigidBody2D` 的模式为 `Dynamic` 了
* 现在 `Motivator.Velocity` 不再与 `RigidBody2D` 绑定

## [1.0.3] - 2024-1-3
### 修改
* 将“参数”小标题移动至“键位”下方, 使继承 `H2DMoveModule` 或 `H2DJumpModule` 时添加的参数可以放在正确的位置  
* 将 H2D 系列模块的 `private` 成员改为 `protected`
* 取消了 `MotivatorState` 的 `partial` 修饰
* 允许 `H2DJumpModule` 的子类重写对【地面】的判定，以实现在其他情况下重置跳跃次数

## [1.0.4] - 2024-1-8
### 修改
* 增加模块的泛型获取方法
* 增加模块的泛型移除方法
* `Motivator` 现在不再强行要求物理模块被设置
* 将 `Motivator` 的大部分 `private` 成员改为 `protected`