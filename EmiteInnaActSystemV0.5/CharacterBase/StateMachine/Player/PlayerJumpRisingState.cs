using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EmiteInnaACT
{
    /// <summary>
    /// �����Ծ������״̬����Idle��walk��sprint�����Խ������״̬���ڵ�����ߴ�ʱת��Ϊ�½�״̬��
    /// </summary>
    public class PlayerJumpRisingState : PlayerAirNormalState
    {
        public override void OnEnterState()
        {

            base.OnEnterState();
            controller.PlayAnimation(controller.configure.JumpRisingAnimation,0.1f);
            if (ConfigureInstance.GetValue<EmiteInnaBool>("uniform", "StateMachingDebug").Value)
                Debug.Log(controller.name + " ������Ծ����״̬");
            //TODO�����Ŷ���
        }
        /// <summary>
        /// ��Ծʱ��Ȼ���Խ����ƶ�������Ч������룬���ʹ�����
        /// ��״̬ת����֮��һ��Ҫֱ��return�������ⲻ��Ҫ�Ĵ���
        /// </summary>
        public override void OnFixedUpdateState()
        {
            base.OnFixedUpdateState();
            CharacterConfigureBase c = controller.configure;
            

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
                controller.CharacterMove(dir, c.MovementSpeed*0.5f, c.RotationSpeed*0.5f);
            }
            //�Ƿ񵽴ﶥ�㡣
            if (controller.rb.velocity.y < -0.02f)
            {
                controller.EnterState("JumpLanding");
                return;
            }
        }
    }
}