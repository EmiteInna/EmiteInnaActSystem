using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EmiteInnaACT
{
    /// <summary>
    /// 玩家静止状态，只要进行移动就会脱离这个状态。
    /// </summary>
    public class PlayerIdelState : PlayerGroundNormalState
    {
        public override void OnEnterState()
        {
            
            base.OnEnterState();
            controller.PlayAnimation(controller.configure.IdelAnimation,0.2f);
            if(ConfigureInstance.GetValue<EmiteInnaBool>("uniform", "StateMachingDebug").Value)
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
            float vertical = 0;
            float horizontal = 0;
            if (Input.GetKey(controller.configure.VerticalUp)) vertical++;
            if (Input.GetKey(controller.configure.VerticalDown)) vertical--;
            if (Input.GetKey(controller.configure.HorizontalRight)) horizontal++;
            if (Input.GetKey(controller.configure.HorizontalLeft)) horizontal--;
            if (Mathf.Abs(vertical) > 0.8f || Mathf.Abs(horizontal) > 0.8f) {
                controller.EnterState("Walk");
            }
        }
        
    }
}