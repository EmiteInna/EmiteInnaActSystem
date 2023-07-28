using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EmiteInnaACT
{
    /// <summary>
    /// 空中防御状态
    /// 
    /// </summary>
    public class PlayerAirNormalState : StateMachineBase
    {
        public override void OnUpdateState()
        {
            base.OnUpdateState();
            
            
            if (Input.GetKeyDown(controller.configure.Defend))
            {
                controller.EnterState("AirDefend");
                return;
            }
        }
    }
}