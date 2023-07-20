using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EmiteInnaACT
{
    /// <summary>
    /// NPC和怪物通用的Stun状态，和hurt放的动画不同，在一会时候会回到Idle状态。
    /// 区别是眩晕的时候逻辑可能不一样。
    /// 调用眩晕时记得先设置timer再设置starttocount为false。
    /// </summary>
    public class UniformStunState : StateMachineBase
    {
        public float timer;
        public bool starttocount = true;
        Coroutine c;
        public override void OnEnterState()
        {
            base.OnEnterState();
            controller.PlayAnimation(controller.configure.StunAnimation);
            starttocount = true;
        }

        public override void OnUpdateState()
        {
            base.OnUpdateState();
            if (starttocount == false)
            {
                starttocount = true;
                if (c != null) controller.StopCoroutine(c);
                c = controller.StartCoroutine(DoBackToIdle(timer));
            }
        }
        IEnumerator DoBackToIdle(float timer)
        {
            yield return new WaitForSeconds(timer);
            controller.OnApplyCharacterEvent("EnterIdle");
        }
    }
}