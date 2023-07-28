using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EmiteInnaACT
{
    /// <summary>
    /// 玩家跳跃的着陆状态，从上升状态转变过来，着陆后变为已着陆状态。
    /// </summary>
    public class PlayerJumpLandingState : PlayerAirNormalState
    {
        public override void OnEnterState()
        {

            base.OnEnterState();
            controller.PlayAnimation(controller.configure.JumpLandingAnimation,0.1f);
            if (ConfigureInstance.GetValue<EmiteInnaBool>("uniform", "StateMachingDebug").Value)
                Debug.Log(controller.name + " 进入跳跃下落状态");
            //TODO：播放动画
        }
        /// <summary>
        /// 跳跃时依然可以进行移动，但是效果会减半，国际惯例。
        /// 在状态转换的之后一定要直接return掉，避免不必要的错误。
        /// </summary>
        public override void OnFixedUpdateState()
        {
            base.OnFixedUpdateState();
            CharacterConfigureBase c = controller.configure;
            

            if (Input.GetKey(controller.configure.HorizontalLeft) ||
               Input.GetKey(controller.configure.HorizontalRight) ||
               Input.GetKey(controller.configure.VerticalUp) ||
               Input.GetKey(controller.configure.VerticalDown))
            {
                float vertical = 0;
                float horizontal = 0;
                if (Input.GetKey(controller.configure.VerticalUp)) vertical++;
                if (Input.GetKey(controller.configure.VerticalDown)) vertical--;
                if (Input.GetKey(controller.configure.HorizontalRight)) horizontal++;
                if (Input.GetKey(controller.configure.HorizontalLeft)) horizontal--;
                Vector3 dir = new Vector3(horizontal, 0, vertical);
                controller.CharacterMove(dir, c.MovementSpeed*0.5f, c.RotationSpeed*0.5f);
            }
            //是否到达地面。
            if (EChara.IsTransformGrounded(controller.transform,controller.col))
            {
                controller.EnterState("JumpLanded");
                return;
            }
        }
    }
}