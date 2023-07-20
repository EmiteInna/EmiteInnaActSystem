using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Searcher.SearcherWindow.Alignment;

namespace EmiteInnaACT
{
    /// <summary>
    /// 玩家走路状态，只要停止移动就会脱离这个状态，可以进入冲刺状态。
    /// </summary>
    public class PlayerWalkState : PlayerGroundNormalState
    {
        public override void OnEnterState()
        {

            base.OnEnterState();
            controller.PlayAnimation(controller.configure.WalkAnimation,0.2f);
            if (ConfigureInstance.GetValue<EmiteInnaBool>("uniform", "StateMachingDebug").Value)
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
            float vertical = 0;
            float horizontal = 0;
            if (Input.GetKey(controller.configure.VerticalUp)) vertical++;
            if (Input.GetKey(controller.configure.VerticalDown)) vertical--;
            if (Input.GetKey(controller.configure.HorizontalRight)) horizontal++;
            if (Input.GetKey(controller.configure.HorizontalLeft)) horizontal--;
            if (Mathf.Abs(vertical) > 0.8f || Mathf.Abs(horizontal) > 0.8f) { 

                Vector3 dir = new Vector3(horizontal, 0, vertical);
                controller.CharacterMove(dir, c.MovementSpeed, c.RotationSpeed);
            }
            else
            {
                controller.EnterState("Idle");
            }
        }
        
    }
}