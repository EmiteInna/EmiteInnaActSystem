using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EmiteInnaACT
{
    /// <summary>
    /// 玩家静止状态，只要进行移动就会脱离这个状态。
    /// </summary>
    public class PlayerIdelState : StateMachineBase
    {
        public override void OnEnterState()
        {
            
            base.OnEnterState();
            controller.PlayAnimation(controller.configure.IdelAnimation,0.2f);
            Debug.Log(controller.name + " 进入Idle状态");
            //TODO：播放Idle动画
        }
        /// <summary>
        /// 判断是否按了移动，如果是的话，进入Walk状态。
        /// 跳跃同理。
        /// </summary>
        public override void OnFixedUpdateState()
        {
            base.OnFixedUpdateState();
            CharacterConfigureBase c = controller.configure;
            if(Input.GetKey(controller.configure.HorizontalLeft)||
               Input.GetKey(controller.configure.HorizontalRight) ||
               Input.GetKey(controller.configure.VerticalUp) ||
               Input.GetKey(controller.configure.VerticalDown)){
                controller.EnterState("Walk");
            }
        }
        public override void OnUpdateState()
        {
            base.OnUpdateState();
            if (Input.GetKeyDown(controller.configure.Jump))
            {
                controller.CharacterJump(controller.configure.JumpHeight);
                controller.EnterState("JumpRising");
            }
        }
    }
}