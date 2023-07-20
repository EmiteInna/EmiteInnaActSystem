using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EmiteInnaACT
{
    /// <summary>
    /// NPC�͹���ͨ�õ�Idle״̬����ʵ���Ƿ��˸�������
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