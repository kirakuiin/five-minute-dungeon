# Five-Minute-Dungeon

- [Five-Minute-Dungeon](#five-minute-dungeon)
  - [About](#about)
  - [Features](#features)
  - [Video](#video)
  - [Editor](#editor)
  - [Implementation](#implementation)

## About

本项目旨在实现一个可以进行局域网联机的[《5分钟地下城》](https://boardgamegeek.com/boardgame/207830/5-minute-dungeon)

## Features

* 支持最多5人的局域网联机
  * 支持IP直连
  * 支持局域网搜索加入
* 支持断线重连
* 支持关卡保存，挑战失败后可以继续从当前关卡继续

## Video

https://github.com/user-attachments/assets/3325f75c-59e3-4c09-bc77-eb7d52d43cc2

# Editor

基于XNode实现了3套蓝图编辑系统，分别为：
- 动作编辑系统
- 合法性检查编辑系统
- 表现编辑系统

如图：
![fireball_anim](https://github.com/user-attachments/assets/a3f87370-e058-4779-9645-3d07b0eae88b)
![fireball_action](https://github.com/user-attachments/assets/ecb36ab4-19e5-4b6f-a5cd-ef95466b11c3)

## Implementation

* 网络部分基于Netcode for GameObject实现；
* 游戏内数据存储于ScriptableObject；
* 游戏内运行时资源加载采用Addressable；
* 游戏内UI动画使用Dotween完成；
* 游戏内各种特效，粒子使用ShaderGraph和VisualEffectGraph完成；
* 游戏测试使用ParrelSync创建多个项目备份来完成；
* 各种卡牌，怪物事件使用XNode构建的节点来实现；

游戏服务端核心逻辑如下图所示：
![5分钟地下城核心类图](https://github.com/user-attachments/assets/03f902cc-efce-4bf4-ab2e-80d83822272e)
