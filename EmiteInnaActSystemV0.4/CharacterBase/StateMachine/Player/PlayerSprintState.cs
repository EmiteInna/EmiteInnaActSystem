using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EmiteInnaACT
{
    /// <summary>
    /// 玩家冲刺状态，松开冲刺键就能脱离这个状态。
    /// </summary>
    public class PlayerSprinttate : PlayerGroundNormalState
    {
        public override void OnEnterState()
        {

            base.OnEnterState();
            controller.PlayAnimation(controller.configure.SprintAnimation,0.2f,1.8f);
            if (ConfigureInstance.GetValue<EmiteInnaBool>("uniform", "StateMachingDebug").Value)
                Debug.Log(controller.name+" 进入Sprint状态");
            //TODO：播放动画
        }
        /// <summary>
        /// 类似walk
        /// </summary>
        public override void OnFixedUpdateState()
        {
            base.OnFixedUpdateState();
            CharacterConfigureBase c = controller.configure;
            if (Input.GetKey(controller.configure.Sprint))
            {
                
            }
            else
            {
                controller.EnterState("Walk");
                return;
            }
            float vertical = 0;
            float horizontal = 0;
            if (Input.GetKey(controller.configure.VerticalUp)) vertical++;
            if (Input.GetKey(controller.configure.VerticalDown)) vertical--;
            if (Input.GetKey(controller.configure.HorizontalRight)) horizontal++;
            if (Input.GetKey(controller.configure.HorizontalLeft)) horizontal--;
            if (Mathf.Abs(vertical)>0.8f|| Mathf.Abs(horizontal) > 0.8f)
            {
                
                Vector3 dir = new Vector3(horizontal, 0, vertical);
                controller.CharacterMove(dir, c.MovementSpeed*c.SprintSpeedMultiplier,
                    c.RotationSpeed*c.SprintSpeedMultiplier);
            }
            else
            {
                controller.EnterState("Idle");
                return;
            }
        }
        
    }
}