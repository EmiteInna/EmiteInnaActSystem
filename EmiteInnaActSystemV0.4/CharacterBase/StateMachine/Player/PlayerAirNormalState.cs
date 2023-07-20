using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EmiteInnaACT
{
    /// <summary>
    /// 空中两个状态的上层状态（上升、下降）
    /// 
    /// </summary>
    public class PlayerAirNormalState : StateMachineBase
    {
        public override void OnUpdateState()
        {
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                controller.CharacterApplySpell("dashdown");
            }
        }
    }
}