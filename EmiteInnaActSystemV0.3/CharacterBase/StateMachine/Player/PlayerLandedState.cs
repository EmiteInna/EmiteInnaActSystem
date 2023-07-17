using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EmiteInnaACT
{
    /// <summary>
    /// 玩家已着陆状态，逻辑和Idle状态相同，一段时间后会自动进入Idle状态。
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
            Debug.Log(controller.name + " 进入Landed状态");
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
                Debug.Log("是我的锅吗？(不可思议)");

                controller.EnterState("Idle");
            }
        }
        /// <summary>
        /// 判断是否按了移动，如果是的话，进入Walk状态。
        /// 跳跃同理。
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