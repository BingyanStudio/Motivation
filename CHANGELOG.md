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