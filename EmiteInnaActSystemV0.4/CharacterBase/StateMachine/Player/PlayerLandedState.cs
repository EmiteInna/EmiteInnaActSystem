using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EmiteInnaACT
{
    /// <summary>
    /// �������½״̬���߼���Idle״̬��ͬ��һ��ʱ�����Զ�����Idle״̬��
    /// </summary>
    public class PlayerLandedState : PlayerGroundNormalState
    {
        Coroutine co=null;
        public override void OnEnterState()
        {
            base.OnEnterState();
            co=controller.StartCoroutine(DoEnterIdle());
             controller.PlayAnimation(controller.configure.JumpLandedAnimation,0.1f);
            if (ConfigureInstance.GetValue<EmiteInnaBool>("uniform", "StateMachingDebug").Value)
                Debug.Log(controller.name + " ����Landed״̬");
        }
        public override void OnLeftState()
        {
            base.OnLeftState();
            controller.StopCoroutine(co);
        }
        IEnumerator DoEnterIdle()
        {
            yield return new WaitForSeconds(2f);
            
                Debug.Log("���ҵĹ���(����˼��)");

                controller.EnterState("Idle");
            
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
                return;
            }
            
        }
       
    }
}