using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EmiteInnaACT
{
    /// <summary>
    /// 玩家地面防御状态。
    /// </summary>
    public class PlayerGroundDefendState : StateMachineBase
    {
        float duration;
        public override void OnEnterState()
        {
            //   controller.PlayAnimation(controller.configure.JumpLandingAnimation, 0.1f);
            if (ConfigureInstance.GetValue<EmiteInnaBool>("uniform", "StateMachingDebug").Value)
                Debug.Log("进入GroundDefend状态");
            base.OnEnterState();
            controller.PlayAnimation(controller.configure.DefendAnimation);
            duration = controller.configure.DefendDuration;

        }
        public override void OnFixedUpdateState()
        {
            base.OnFixedUpdateState();
            if (this != controller.currentState) return;
            duration -= Time.fixedDeltaTime;
            if (duration <= 0) controller.EnterState("Idle");
        }
        public override void OnUpdateState()
        {
            if (this != controller.currentState) return;
            base.OnUpdateState();
            CharacterConfigureBase c = controller.configure;
            Vector3 vec = Vector3.zero;
            if (Input.GetKey(c.HorizontalLeft))
            {
                vec += Vector3.left;
            }
            if (Input.GetKey(c.HorizontalRight))
            {
                vec += Vector3.right;
            }
            if (Input.GetKey(c.VerticalUp))
            {
                vec += Vector3.forward;
            }
            if (Input.GetKey(c.VerticalDown))
            {
                vec += Vector3.back;
            }
            bool b = Input.GetKeyDown(c.HorizontalLeft) || Input.GetKeyDown(c.HorizontalRight) || Input.GetKeyDown(c.VerticalDown) || Input.GetKeyDown(c.VerticalUp);
            if (b)
            {
                EChara.CharacterDashToPoint(controller, controller.transform.position + vec.normalized * c.DashDistance, 0.2f);
                return;
            }
            if (Input.GetKeyDown(c.Jump))
            {
                EChara.CharacterLongJump(controller,c.LongJumpHeight);
                controller.EnterState("JumpRising");
                return;
            }
        }

    }
}