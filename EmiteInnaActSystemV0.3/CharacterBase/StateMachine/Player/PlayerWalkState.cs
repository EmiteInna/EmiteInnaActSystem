using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EmiteInnaACT
{
    /// <summary>
    /// 玩家走路状态，只要停止移动就会脱离这个状态，可以进入冲刺状态。
    /// </summary>
    public class PlayerWalkState : StateMachineBase
    {
        public override void OnEnterState()
        {

            base.OnEnterState();
            controller.PlayAnimation(controller.configure.WalkAnimation,0.2f);
            Debug.Log(controller.name + " 进入Walk状态");
            //TODO：播放动画
        }
        /// <summary>
        /// 判断是否按着移动，如果没有的话回到Idle，如果按了sprint则到冲刺状态。
        /// 跳跃同理。
        /// 在状态转换的之后一定要直接return掉，避免不必要的错误。
        /// </summary>
        public override void OnFixedUpdateState()
        {
            base.OnFixedUpdateState();
            CharacterConfigureBase c = controller.configure;
            if (Input.GetKey(controller.configure.Sprint))
            {
                controller.EnterState("Sprint");
                return;
            }

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
                controller.CharacterMove(dir, c.MovementSpeed, c.RotationSpeed);
            }
            else
            {
                controller.EnterState("Idle");
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