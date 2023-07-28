using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EmiteInnaACT
{
    /// <summary>
    /// NPC�͹���ͨ�õ�Hurt״̬����ֹ�Ƿ��˸���������һ��ʱ���ص�Idle״̬��
    /// ����ѣ��ʱ�ǵ�������timer������starttocountΪfalse��
    /// </summary>
    public class UniformHurtState : StateMachineBase
    {
        public float timer;
        public bool starttocount = true;
        Coroutine c = null;
        public override void OnEnterState()
        {
            base.OnEnterState();
            controller.PlayAnimation(controller.configure.HurtAnimation);
            starttocount = true;
        }

        public override void OnUpdateState()
        {
            base.OnUpdateState();
            if (starttocount == false)
            {
                starttocount = true;
                if (c != null) controller.StopCoroutine(c);
                c= controller.StartCoroutine(DoBackToIdle(timer));
            }
        }
        IEnumerator DoBackToIdle(float timer)
        {
            yield return new WaitForSeconds(timer);
            controller.OnApplyCharacterEvent("EnterIdle");
        }
    }
}