using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EmiteInnaACT
{
    /// <summary>
    /// �����·״̬��ֻҪֹͣ�ƶ��ͻ��������״̬�����Խ�����״̬��
    /// </summary>
    public class PlayerWalkState : StateMachineBase
    {
        public override void OnEnterState()
        {

            base.OnEnterState();
            controller.PlayAnimation(controller.configure.WalkAnimation,0.2f);
            Debug.Log(controller.name + " ����Walk״̬");
            //TODO�����Ŷ���
        }
        /// <summary>
        /// �ж��Ƿ����ƶ������û�еĻ��ص�Idle���������sprint�򵽳��״̬��
        /// ��Ծͬ��
        /// ��״̬ת����֮��һ��Ҫֱ��return�������ⲻ��Ҫ�Ĵ���
        /// </summary>
        public override void OnFixedUpdateState()
        {
            base.OnFixedUpdateState();
            CharacterConfigureBase c = controller.configure;
            if (Input.GetKey(controller.configure.Sprint))
            {
                controller.EnterState("Sprint");
                return;
            }

            if (Input.GetKey(controller.configure.HorizontalLeft) ||
               Input.GetKey(controller.configure.HorizontalRight) ||
               Input.GetKey(controller.configure.VerticalUp) ||
               Input.GetKey(controller.configure.VerticalDown))
            {
                float vertical = 0;
                float horizontal = 0;
                if (Input.GetKey(controller.configure.VerticalUp)) vertical++;
                if (Input.GetKey(controller.configure.VerticalDown)) vertical--;
                if (Input.GetKey(controller.configure.HorizontalRight)) horizontal++;
                if (Input.GetKey(controller.configure.HorizontalLeft)) horizontal--;
                Vector3 dir = new Vector3(horizontal, 0, vertical);
                controller.CharacterMove(dir, c.MovementSpeed, c.RotationSpeed);
            }
            else
            {
                controller.EnterState("Idle");
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