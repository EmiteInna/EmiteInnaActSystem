using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EmiteInnaACT
{
    /// <summary>
    /// NPC和怪物通用的Idle状态，其实就是放了个动画。
    /// </summary>
    public class UniformIdleState : StateMachineBase
    {
        public override void OnEnterState()
        {
            base.OnEnterState();
            controller.PlayAnimation(controller.configure.IdelAnimation);
        }
        public override void OnUpdateState()
        {
            base.OnUpdateState();
            
        }
    }
}