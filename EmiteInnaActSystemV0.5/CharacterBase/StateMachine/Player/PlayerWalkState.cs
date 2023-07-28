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
        float lastW;
        float lastA;
        float lastS;
        float lastD;
        public override void OnEnterState()
        {

            base.OnEnterState();
            //lastW = -100;
            //lastA = -100;
            //lastS = -100;
            //lastD = -100;
            controller.PlayAnimation(controller.configure.WalkAnimation,0.2f);
            if (ConfigureInstance.GetValue<EmiteInnaBool>("uniform", "StateMachingDebug").Value)
                Debug.Log(controller.name + " ����Walk״̬");
            //TODO�����Ŷ���
            
            //if (Input.GetKeyDown(controller.configure.HorizontalLeft))
            //    UpdateKeyCheck(controller.configure.HorizontalLeft);
            //if (Input.GetKeyDown(controller.configure.HorizontalRight))
            //    UpdateKeyCheck(controller.configure.HorizontalRight);
            //if (Input.GetKeyDown(controller.configure.VerticalDown))
            //    UpdateKeyCheck(controller.configure.VerticalDown);
            //if (Input.GetKeyDown(controller.configure.VerticalUp))
            //    UpdateKeyCheck(controller.configure.VerticalUp);
            
        }
        /// <summary>
        /// �����Ƿ������״̬�Ķ������߼��ܼ򵥣��������μ���ʱ�������������±�ķ�����Ļ�������������ʱ�䡣
        /// </summary>
        /// <param name="k"></param>
        public bool UpdateKeyCheck(KeyCode k)
        {
            float rc = 0.2f;
            CharacterConfigureBase c = controller.configure;
            if (k == c.VerticalDown)
            {
                if (Time.realtimeSinceStartup - lastS <= rc)
                {
                    controller.EnterState("Sprint");
                    return true;
                }
                lastW = -100;
                lastS = Time.realtimeSinceStartup;
                lastA = -100;
                lastD = -100;
            }else if (k == c.VerticalUp)
            {
                if (Time.realtimeSinceStartup - lastW <= rc)
                {
                    controller.EnterState("Sprint");
                    return true;
                }
                lastS = -100;
                lastW = Time.realtimeSinceStartup;
                lastA = -100;
                lastD = -100;
            }
            else if (k == c.HorizontalLeft)
            {
                if (Time.realtimeSinceStartup - lastA <= rc)
                {
                    controller.EnterState("Sprint");
                    return true;
                }
                lastS = -100;
                lastA = Time.realtimeSinceStartup;
                lastW = -100;
                lastD = -100;
            }
            else if (k == c.HorizontalRight)
            {
                if (Time.realtimeSinceStartup - lastD <= rc)
                {
                    controller.EnterState("Sprint");
                    return true;
                }
                lastS = -100;
                lastD = Time.realtimeSinceStartup;
                lastA = -100;
                lastW = -100;
            }
            return false;
        }
        /// <summary>
        /// �ж��Ƿ����ƶ������û�еĻ��ص�Idle���������˫��������״̬��
        /// ��Ծͬ��
        /// ��״̬ת����֮��һ��Ҫֱ��return�������ⲻ��Ҫ�Ĵ���
        /// </summary>
        public override void OnFixedUpdateState()
        {
            base.OnFixedUpdateState();
            CharacterConfigureBase c = controller.configure;
            
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
        public override void OnUpdateState()
        {
            base.OnUpdateState();
            if (controller.currentState != this) return;
            if (Input.GetKeyDown(controller.configure.HorizontalLeft))
            {
                if (UpdateKeyCheck(controller.configure.HorizontalLeft))
                {
                    return;
                }
            }
            if (Input.GetKeyDown(controller.configure.HorizontalRight))
            {
                if (UpdateKeyCheck(controller.configure.HorizontalRight))
                {
                    return;
                }
            }
            if (Input.GetKeyDown(controller.configure.VerticalUp))
            {
                if (UpdateKeyCheck(controller.configure.VerticalUp))
                {
                    return;
                }
            }
            if (Input.GetKeyDown(controller.configure.VerticalDown))
            {
                if (UpdateKeyCheck(controller.configure.VerticalDown))
                {
                    return;
                }
            }
        }
    }
}