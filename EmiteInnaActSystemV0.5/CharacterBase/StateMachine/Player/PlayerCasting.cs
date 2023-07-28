using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EmiteInnaACT
{
    /// <summary>
    /// 玩家施法状态，你什么也干不了。
    /// </summary>
    public class PlayerCastingState : StateMachineBase
    {
        public override void OnEnterState()
        {
         //   controller.PlayAnimation(controller.configure.JumpLandingAnimation, 0.1f);
            if (ConfigureInstance.GetValue<EmiteInnaBool>("uniform", "StateMachingDebug").Value)
                Debug.Log("进入Casting状态");
            base.OnEnterState();
            
        }
        
        
    }
}