using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EmiteInnaACT
{
    /// <summary>
    /// ��ɫ״̬���Ļ��࣬�ṩһЩ�鷽����������ֱ��ʹ�á�
    /// ��ô�����������ʵ�Ǹ��ӿڰɡ�
    /// �ĸ���������Ҫ���أ���Ȼû�����ݡ�
    /// </summary>
    public abstract class StateMachineBase
    {
        public CharacterControllerBase controller;
        /// <summary>
        /// Ϊ״̬����һ��controller�����ڻ�ȡ��ɫ��Ϣ��
        /// </summary>
        /// <param name="controller"></param>
        public void BindController(CharacterControllerBase controller)
        {
            this.controller = controller;
        }
        /// <summary>
        /// �����״̬ʱ���õķ���
        /// </summary>
        public virtual void OnEnterState()
        {

        }
        /// <summary>
        /// �뿪��״̬ʱ���õķ���
        /// </summary>
        public virtual void OnLeftState()
        {

        }
        /// <summary>
        /// ���ڸ�״̬ʱ��update
        /// </summary>
        public virtual void OnUpdateState()
        {

        }
        /// <summary>
        /// ���ڸ�״̬ʱ��fixedUpdate
        /// </summary>
        public virtual void OnFixedUpdateState()
        {

        }
    }
}