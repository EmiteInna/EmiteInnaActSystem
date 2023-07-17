using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EmiteInnaACT
{
    /// <summary>
    /// �������½״̬���߼���Idle״̬��ͬ��һ��ʱ�����Զ�����Idle״̬��
    /// </summary>
    public class PlayerLandedState : StateMachineBase
    {
        bool quit = false;
        public override void OnEnterState()
        {
            quit = false;
            base.OnEnterState();
            controller.StartCoroutine(DoEnterIdle());
           // controller.PlayAnimation(controller.configure.IdelAnimation,0.2f);
            Debug.Log(controller.name + " ����Landed״̬");
        }
        public override void OnLeftState()
        {
            base.OnLeftState();
            quit = true;
            controller.StopCoroutine(DoEnterIdle());
        }
        IEnumerator DoEnterIdle()
        {
            yield return new WaitForSeconds(3f);
            if (!quit)
            {
                Debug.Log("���ҵĹ���(����˼��)");

                controller.EnterState("Idle");
            }
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
    }
}