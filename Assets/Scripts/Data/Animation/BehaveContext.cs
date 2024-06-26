﻿using System.Threading.Tasks;
using UnityEngine;

namespace Data.Animation
{
    /// <summary>
    /// 表现接口。
    /// </summary>
    public interface IBehaveController
    {
        /// <summary>
        /// 获得模型动画播放器。
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public IModelAnimPlayer GetModelPlayer(AnimTarget target);

        /// <summary>
        /// 获得位置信息。
        /// </summary>
        /// <returns></returns>
        public IPositionInfo GetPositionInfo();

        /// <summary>
        /// 获得特效播放器。
        /// </summary>
        /// <returns></returns>
        public IVfxPlayer GetVfxPlayer();

        /// <summary>
        /// 获得音效播放器。
        /// </summary>
        /// <returns></returns>
        public IAudioEffectPlayer GetAudioPlayer();
    }

    /// <summary>
    /// 模型播放接口。
    /// </summary>
    public interface IModelAnimPlayer
    {
        
        /// <summary>
        /// 播放动作。
        /// </summary>
        /// <param name="name"></param>
        /// <param name="speed"></param>
        public void PlayAnim(string name, float speed);
        
        /// <summary>
        /// 控制动画播放。
        /// </summary>
        public void SetPlay(bool isPlay);

        /// <summary>
        /// 移动到某个位置。
        /// </summary>
        /// <param name="param"></param>
        public void ChangeTo(ModelChangeParam param);

        /// <summary>
        /// 返回原位。
        /// </summary>
        public void ChangeBack();

        /// <summary>
        /// 播放受击动作
        /// </summary>
        public void PlayHurt()
        {
            PlayAnim(AnimNameDefine.Hurt);
        }

        /// <summary>
        /// 播放动作。
        /// </summary>
        /// <param name="name"></param>
        public void PlayAnim(string name)
        {
            PlayAnim(name, 1.0f);
        }
    }

    /// <summary>
    /// 游戏内变换信息。
    /// </summary>
    public interface IPositionInfo
    {
        /// <summary>
        /// 获得动画对象的位置信息。
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public Vector3 GetAnimTargetPos(AnimTarget target);

        /// <summary>
        /// 获得敌方中心位置。
        /// </summary>
        /// <returns></returns>
        public Vector3 GetEnemyCenter();

        /// <summary>
        /// 获得友方中心位置。
        /// </summary>
        /// <returns></returns>
        public Vector3 GetPlayerCenter();

        /// <summary>
        /// 获得一个随机的非事件敌人位置。
        /// </summary>
        /// <returns></returns>
        public Vector3 GetARandomNonEventEnemy();
    }

    /// <summary>
    /// 特效播放器。
    /// </summary>
    public interface IVfxPlayer
    {
        /// <summary>
        /// 播放激光。
        /// </summary>
        /// <returns></returns>
        public Task PlayLaser(Vector3 from, Vector3 to, float duration, Color color, Color subColor);

        /// <summary>
        /// 控制播放时停特效。
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public Task TimeStop(Vector3 target);
        
        /// <summary>
        /// 播放投射物。
        /// <returns></returns>
        /// </summary>
        /// <param name="projectileName"></param>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="duration"></param>
        /// <returns></returns>
        public Task PlayProjectile(string projectileName, Vector3 from, Vector3 to, float duration);

        /// <summary>
        /// 播放静止特效。
        /// </summary>
        /// <param name="vfxName"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public Task PlayStillVfx(string vfxName, StillVfxParam param);

        /// <summary>
        /// 播放资源溶解特效。
        /// </summary>
        /// <param name="res"></param>
        /// <returns></returns>
        public Task PlayDissolveRes(Resource res);

        /// <summary>
        /// 播放职业溶解特效。
        /// </summary>
        /// <param name="clsType"></param>
        /// <returns></returns>
        public Task PlayDissolveClass(Class clsType);
    }

    /// <summary>
    /// 音效播放器。
    /// </summary>
    public interface IAudioEffectPlayer
    {
        /// <summary>
        /// 播放全局音效。
        /// </summary>
        /// <param name="param"></param>
        public void PlayGlobal(AudioParam param);

        /// <summary>
        /// 在某个目标处播放音效。
        /// </summary>
        /// <param name="target"></param>
        /// <param name="param"></param>
        public void PlayOnTarget(AnimTarget target, AudioParam param);
    }
}