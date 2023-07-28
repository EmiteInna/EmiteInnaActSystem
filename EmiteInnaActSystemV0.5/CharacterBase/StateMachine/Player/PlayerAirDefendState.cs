using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EmiteInnaACT
{
    /// <summary>
    /// 玩家地面防御状态。
    /// </summary>
    public class PlayerAirDefendState : StateMachineBase
    {
        float duration;
        int framecnt = 0;
        public override void OnEnterState()
        {
            //   controller.PlayAnimation(controller.configure.JumpLandingAnimation, 0.1f);
            if (ConfigureInstance.GetValue<EmiteInnaBool>("uniform", "StateMachingDebug").Value)
                Debug.Log("进入AirDefend状态");
            base.OnEnterState();
            controller.PlayAnimation(controller.configure.DefendAnimation);
            duration = controller.configure.DefendDuration;
            framecnt = 0;
        }
        public override void OnFixedUpdateState()
        {
            base.OnFixedUpdateState();
            if (this != controller.currentState) return;
            duration -= Time.fixedDeltaTime;
            if (duration <= 0) controller.EnterState("JumpFalling");
        }
        public override void OnUpdateState()
        {
            if (this != controller.currentState) return;
            framecnt++;
            base.OnUpdateState();
            if (EChara.IsTransformGrounded(controller.transform, controller.col))
            {
                controller.EnterState("Idle");
                return;
            }
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

            if (Input.GetKeyDown(c.Defend) && framecnt >= 3)
            {
                Vector3 grd=EChara.GetGroundPointUnderTransform(controller.transform,vec);
                EChara.CharacterDashToPoint(controller, grd, 0.2f);
                return;
            }

            if(b)
            {
                EChara.CharacterDashToPoint(controller, controller.transform.position + vec.normalized * c.DashDistance, 0.2f);
                return;
            }
            
        }

    }
}