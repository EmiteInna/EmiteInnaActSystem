using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EmiteInnaACT
{
    /// <summary>
    /// NPC和怪物通用的Hurt状态，不止是放了个动画，在一会时候会回到Idle状态。
    /// 调用眩晕时记得先设置timer再设置starttocount为false。
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