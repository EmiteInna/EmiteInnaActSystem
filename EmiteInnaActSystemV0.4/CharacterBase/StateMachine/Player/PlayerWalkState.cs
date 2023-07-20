using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Searcher.SearcherWindow.Alignment;

namespace EmiteInnaACT
{
    /// <summary>
    /// �����·״̬��ֻҪֹͣ�ƶ��ͻ��������״̬�����Խ�����״̬��
    /// </summary>
    public class PlayerWalkState : PlayerGroundNormalState
    {
        public override void OnEnterState()
        {

            base.OnEnterState();
            controller.PlayAnimation(controller.configure.WalkAnimation,0.2f);
            if (ConfigureInstance.GetValue<EmiteInnaBool>("uniform", "StateMachingDebug").Value)
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
            float vertical = 0;
            float horizontal = 0;
            if (Input.GetKey(controller.configure.VerticalUp)) vertical++;
            if (Input.GetKey(controller.configure.VerticalDown)) vertical--;
            if (Input.GetKey(controller.configure.HorizontalRight)) horizontal++;
            if (Input.GetKey(controller.configure.HorizontalLeft)) horizontal--;
            if (Mathf.Abs(vertical) > 0.8f || Mathf.Abs(horizontal) > 0.8f) { 

                Vector3 dir = new Vector3(horizontal, 0, vertical);
                controller.CharacterMove(dir, c.MovementSpeed, c.RotationSpeed);
            }
            else
            {
                controller.EnterState("Idle");
            }
        }
        
    }
}