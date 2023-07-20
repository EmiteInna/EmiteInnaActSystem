using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EmiteInnaACT
{
    /// <summary>
    /// NPC�͹���ͨ�õ�Walk״̬����ʵ���Ƿ��˸�������
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