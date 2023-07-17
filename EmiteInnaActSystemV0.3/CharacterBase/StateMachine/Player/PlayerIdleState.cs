using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EmiteInnaACT
{
    /// <summary>
    /// ��Ҿ�ֹ״̬��ֻҪ�����ƶ��ͻ��������״̬��
    /// </summary>
    public class PlayerIdelState : StateMachineBase
    {
        public override void OnEnterState()
        {
            
            base.OnEnterState();
            controller.PlayAnimation(controller.configure.IdelAnimation,0.2f);
            Debug.Log(controller.name + " ����Idle״̬");
            //TODO������Idle����
        }
        /// <summary>
        /// �ж��Ƿ����ƶ�������ǵĻ�������Walk״̬��
        /// ��Ծͬ��
        /// </summary>
        public override void OnFixedUpdateState()
        {
            base.OnFixedUpdateState();
            CharacterConfigureBase c = controller.configure;
            if(Input.GetKey(controller.configure.HorizontalLeft)||
               Input.GetKey(controller.configure.HorizontalRight) ||
               Input.GetKey(controller.configure.VerticalUp) ||
               Input.GetKey(controller.configure.VerticalDown)){
                controller.EnterState("Walk");
            }
        }
        public override void OnUpdateState()
        {
            base.OnUpdateState();
            if (Input.GetKeyDown(controller.configure.Jump))
            {
                controller.CharacterJump(controller.configure.JumpHeight);
                controller.EnterState("JumpRising");
            }
        }
    }
}