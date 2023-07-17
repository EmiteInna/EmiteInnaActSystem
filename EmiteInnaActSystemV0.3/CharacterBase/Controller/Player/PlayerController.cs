using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EmiteInnaACT
{
    public class PlayerController : CharacterControllerBase
    {

        //暂时用public来赋值，之后会用assetbundle，注意关心是否需要实例化配置。
        public CharacterConfigureBase playerConfigure;

        /// <summary>
        /// 我们先想想这个initialize里应该initialize些什么。
        /// 配置表、状态机、事件注册，还有呢？
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();
            this.configure = playerConfigure;
            PlayerIdelState idle = new PlayerIdelState();
            PlayerWalkState walk = new PlayerWalkState();
            PlayerSprinttate sprint = new PlayerSprinttate();
            PlayerJumpRisingState jumpRising = new PlayerJumpRisingState();
            PlayerJumpLandingState jumpLanding = new PlayerJumpLandingState();
            PlayerLandedState landed = new PlayerLandedState();
            RegisterStateMachine("Idle", idle);
            RegisterStateMachine("Walk", walk);
            RegisterStateMachine("Sprint", sprint);
            RegisterStateMachine("JumpRising", jumpRising);
            RegisterStateMachine("JumpLanding", jumpLanding);
            RegisterStateMachine("JumpLanded", landed);
            EnterState("Idle");
            UseCharacterEvent("PlayFootStepSound", PlayFootStepSound);
        }
        /// <summary>
        /// 播放脚步声
        /// </summary>
        public void PlayFootStepSound()
        {
            ESoundInstance.PlaySFX(playerConfigure.Footstep, transform.position, 2f);
        }
    }
}