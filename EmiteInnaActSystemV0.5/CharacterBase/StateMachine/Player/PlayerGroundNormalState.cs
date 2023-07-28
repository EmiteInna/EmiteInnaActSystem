using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EmiteInnaACT
{
    /// <summary>
    /// 地面四个状态的上层状态（静止、走路、奔跑、着陆）
    /// 目前唯一的功能是跳
    /// </summary>
    public class PlayerGroundNormalState : StateMachineBase
    {
        public override void OnUpdateState()
        {
            base.OnUpdateState();
            if (Input.GetKeyDown(controller.configure.Jump))
            {
                controller.CharacterJump(controller.configure.JumpHeight);
                controller.EnterState("JumpRising");
                return;
            }
            if (Input.GetKeyDown(KeyCode.V))
            {
                controller.CharacterApplySpell("spin");
                return;
            }
            if(Input.GetKeyDown(controller.configure.Attack))
            {
                controller.CharacterApplySpell("attack1");
                return;
            }
            if (Input.GetKeyDown(controller.configure.Defend))
            {
                controller.EnterState("GroundDefend");
            }
        }
    }
}