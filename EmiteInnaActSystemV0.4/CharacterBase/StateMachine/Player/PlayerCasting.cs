using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EmiteInnaACT
{
    /// <summary>
    /// ���ʩ��״̬����ʲôҲ�ɲ��ˡ�
    /// </summary>
    public class PlayerCastingState : StateMachineBase
    {
        public override void OnEnterState()
        {
         //   controller.PlayAnimation(controller.configure.JumpLandingAnimation, 0.1f);
            if (ConfigureInstance.GetValue<EmiteInnaBool>("uniform", "StateMachingDebug").Value)
                Debug.Log("����Casting״̬");
            base.OnEnterState();
            
        }
        
        
    }
}