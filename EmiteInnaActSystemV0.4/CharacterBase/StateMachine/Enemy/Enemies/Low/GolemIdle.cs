using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EmiteInnaACT
{
    /// <summary>
    /// 傀儡的Idle，会进入Idle状态，在Idle状态时傀儡会一直尝试回到最开始的地方。
    /// </summary>
    public class GolemIdle : StateMachineBase
    {
        Vector3 originalPosition;
        Quaternion originalFace;
        bool initialized = false;
        public override void OnEnterState()
        {
            base.OnEnterState();
            if (initialized == false)
            {
                originalPosition = controller.transform.position;
                originalFace = controller.transform.rotation;
                initialized = true;
            }
        }
        public override void OnUpdateState()
        {
            base.OnUpdateState();
            controller.transform.position=originalPosition;
            controller.transform.rotation = originalFace;
            controller.rb.velocity = Vector3.zero;
        }
    }
}