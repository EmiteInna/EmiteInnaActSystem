using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EmiteInnaACT
{
    /// <summary>
    /// ��Ҿ�ֹ״̬��ֻҪ�����ƶ��ͻ��������״̬��
    /// </summary>
    public class PlayerIdelState : PlayerGroundNormalState
    {
        public override void OnEnterState()
        {
            
            base.OnEnterState();
            controller.PlayAnimation(controller.configure.IdelAnimation,0.2f);
            if(ConfigureInstance.GetValue<EmiteInnaBool>("uniform", "StateMachingDebug").Value)
                Debug.Log(controller.name + " ����Idle״̬");
            //TODO������Idle����
        }
        /// <summary>
        /// �ж��Ƿ����ƶ�������ǵĻ�������Walk״̬��
        /// ��Ծͬ��
        /// </summary>
        public override void OnUpdateState()
        {
            base.OnUpdateState();
            if (controller.currentState != this) return;
            CharacterConfigureBase c = controller.configure;
            float vertical = 0;
            float horizontal = 0;
            if (Input.GetKey(controller.configure.VerticalUp)) vertical++;
            if (Input.GetKey(controller.configure.VerticalDown)) vertical--;
            if (Input.GetKey(controller.configure.HorizontalRight)) horizontal++;
            if (Input.GetKey(controller.configure.HorizontalLeft)) horizontal--;
            if (Mathf.Abs(vertical) > 0.8f || Mathf.Abs(horizontal) > 0.8f) {
                controller.EnterState("Walk");
                if(controller.currentState is PlayerWalkState)
                {
                    if(Input.GetKey(controller.configure.HorizontalLeft))
                        (controller.currentState as PlayerWalkState).UpdateKeyCheck(controller.configure.HorizontalLeft);
                    if (Input.GetKey(controller.configure.HorizontalRight))
                        (controller.currentState as PlayerWalkState).UpdateKeyCheck(controller.configure.HorizontalRight);
                    if (Input.GetKey(controller.configure.VerticalDown))
                        (controller.currentState as PlayerWalkState).UpdateKeyCheck(controller.configure.VerticalDown);
                    if (Input.GetKey(controller.configure.VerticalUp))
                        (controller.currentState as PlayerWalkState).UpdateKeyCheck(controller.configure.VerticalUp);
                }
                return;
            }
        }
        
    }
}