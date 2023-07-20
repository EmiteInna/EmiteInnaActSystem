using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EmiteInnaACT
{
    /// <summary>
    /// ���ܵ�Idle�������Idle״̬����Idle״̬ʱ���ܻ�һֱ���Իص��ʼ�ĵط���
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