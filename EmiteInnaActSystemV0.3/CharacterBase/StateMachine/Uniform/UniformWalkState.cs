using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EmiteInnaACT
{
    /// <summary>
    /// NPC和怪物通用的Walk状态，其实就是放了个动画。
    /// </summary>
    public class UniformWalkState : StateMachineBase
    {
        public override void OnEnterState()
        {
            base.OnEnterState();
            controller.PlayAnimation(controller.configure.WalkAnimation);
        }
        public override void OnUpdateState()
        {
            base.OnUpdateState();
            
        }
    }
}