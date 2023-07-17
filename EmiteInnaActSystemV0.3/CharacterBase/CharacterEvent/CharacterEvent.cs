using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EmiteInnaACT
{
    public struct CharacterEventStructer
    {
        public bool enabled;
        public Action action;
    }
    /// <summary>
    /// ��ɫ�¼��࣬ÿ��controller��һ�����С�
    /// </summary>
    public class CharacterEvent {
        public Dictionary<string, CharacterEventStructer> eventList=new Dictionary<string, CharacterEventStructer>();
        /// <summary>
        /// ע��һ����ɫ�¼�
        /// </summary>
        /// <param name="name"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public bool RegisterCharacterEvent(string name,Action action)
        {
            if(eventList.TryGetValue(name,out CharacterEventStructer ces))
            {
                return false;
            }
            else
            {
                CharacterEventStructer c = new CharacterEventStructer();
                c.enabled = false;
                c.action = action;
                eventList.Add(name, c);
                return true;
            }
        }
        /// <summary>
        /// ����һ���¼�
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool EnableCharacterEvent(string name)
        {
            if(eventList.TryGetValue(name,out CharacterEventStructer ces))
            {
                ces.enabled = true;
                eventList[name] = ces;
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// ����һ���¼�
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool DisableCharacterEvent(string name)
        {
            if (eventList.TryGetValue(name, out CharacterEventStructer ces))
            {
                ces.enabled = false;
                eventList[name] = ces;
                return false;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// ����һ���¼������û����ע�ᡣ
        /// </summary>
        /// <param name="name"></param>
        /// <param name="action"></param>
        public void UseCharacterEvent(string name, Action action)
        {
            if (EnableCharacterEvent(name)) return;
            RegisterCharacterEvent(name, action);
            EnableCharacterEvent(name);
        }
        /// <summary>
        /// ɾ��һ���¼�
        /// </summary>
        /// <param name="name"></param>
        public void DeleteCharacterEvent(string name)
        {
            if (eventList.TryGetValue(name, out CharacterEventStructer ces))
            {
                eventList.Remove(name);
            }
        }
        /// <summary>
        /// ����һ���¼���û�л���δ�����򷵻�false��
        /// </summary>
        /// <param name="name"></param>
        public bool ApplyCharacterEvent(string name)
        {
            if (eventList.TryGetValue(name, out CharacterEventStructer ces))
            {
                if (ces.enabled == false)
                {
                    return false;
                }
                ces.action?.Invoke();
                return true;
            }
            return false;
        }
    }

}