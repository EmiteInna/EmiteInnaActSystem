using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EmiteInnaACT
{
    //TODO:�¼��Ķ���ػ��ջ���
    public interface ICharacterEvent { };
   
    public class CharacterEventBase : ICharacterEvent
    {
        public Action action;
    }
    #region 7�ֲ�������
    public class CharacterEventBase<E> : ICharacterEvent
    {
        public Action<E> action;
    }
    public class CharacterEventBase<E,M> : ICharacterEvent
    {
        public Action<E,M> action;
    }
    public class CharacterEventBase<E, M,I> : ICharacterEvent
    {
        public Action<E, M,I> action;
    }
    public class CharacterEventBase<E, M, I,T> : ICharacterEvent
    {
        public Action<E, M, I,T> action;
    }
    public class CharacterEventBase<E, M, I, T,N> : ICharacterEvent
    {
        public Action<E, M, I, T,N> action;
    }
    public class CharacterEventBase<E, M, I, T, N,A> : ICharacterEvent
    {
        public Action<E, M, I, T, N,A> action;
    }
    #endregion
    public struct CharacterEventStructer
    {
        public bool enabled;
        public Action action;

    }
    /// <summary>
    /// ��ɫ�¼��࣬ÿ��controller��һ�����С�
    /// </summary>
    public class CharacterEvent {
        public Dictionary<string, ICharacterEvent> eventList=new Dictionary<string, ICharacterEvent>();
        /// <summary>
        /// ע��һ����ɫ�¼�
        /// </summary>
        /// <param name="name"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public bool RegisterCharacterEvent(string name,Action action)
        {
            if(eventList.TryGetValue(name,out ICharacterEvent ces))
            {
                return false;
            }
            else
            {
                CharacterEventBase c = new CharacterEventBase();
                c.action = action;
                eventList.Add(name, c);
                return true;
            }
        }
        #region ��ͬ�汾
        public bool RegisterCharacterEvent<E>(string name, Action<E> action)
        {
            if (eventList.TryGetValue(name, out ICharacterEvent ces))
            {
                return false;
            }
            else
            {
                CharacterEventBase<E> c = new CharacterEventBase<E>();
                c.action = action;
                eventList.Add(name, c);
                return true;
            }
        }
        public bool RegisterCharacterEvent<E,M>(string name, Action<E, M> action)
        {
            if (eventList.TryGetValue(name, out ICharacterEvent ces))
            {
                return false;
            }
            else
            {
                CharacterEventBase<E, M> c = new CharacterEventBase<E, M>();
                c.action = action;
                eventList.Add(name, c);
                return true;
            }
        }
        public bool RegisterCharacterEvent<E,M,I>(string name, Action<E, M, I> action)
        {
            if (eventList.TryGetValue(name, out ICharacterEvent ces))
            {
                return false;
            }
            else
            {
                CharacterEventBase<E, M, I> c = new CharacterEventBase<E, M, I>();
                c.action = action;
                eventList.Add(name, c);
                return true;
            }
        }
        public bool RegisterCharacterEvent<E,M,I,T>(string name, Action<E, M, I, T> action)
        {
            if (eventList.TryGetValue(name, out ICharacterEvent ces))
            {
                return false;
            }
            else
            {
                CharacterEventBase<E, M, I, T> c = new CharacterEventBase<E, M, I, T>();
                c.action = action;
                eventList.Add(name, c);
                return true;
            }
        }
        public bool RegisterCharacterEvent<E, M, I, T,N>(string name, Action<E, M, I, T, N> action)
        {
            if (eventList.TryGetValue(name, out ICharacterEvent ces))
            {
                return false;
            }
            else
            {
                CharacterEventBase<E, M, I, T, N> c = new CharacterEventBase<E, M, I, T, N>();
                c.action = action;
                eventList.Add(name, c);
                return true;
            }
        }
        public bool RegisterCharacterEvent<E, M, I, T, N,A>(string name, Action<E, M, I, T, N, A> action)
        {
            if (eventList.TryGetValue(name, out ICharacterEvent ces))
            {
                return false;
            }
            else
            {
                CharacterEventBase<E, M, I, T, N, A> c = new CharacterEventBase<E, M, I, T, N, A>();
                c.action = action;
                eventList.Add(name, c);
                return true;
            }
        }
        #endregion
        /// <summary>
        /// ����һ���¼�
        /// �Ѿ�����
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        //public bool EnableCharacterEvent(string name)
        //{
        //    if(eventList.TryGetValue(name,out ICharacterEvent ces))
        //    {
        //        ces.enabled = true;
        //        eventList[name] = ces;
        //        return true;
        //    }
        //    else
        //    {
        //        return false;
        //    }
        //}
        /// <summary>
        /// ����һ���¼�,�Ѿ�����
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        //public bool DisableCharacterEvent(string name)
        //{
        //    if (eventList.TryGetValue(name, out CharacterEventStructer ces))
        //    {
        //        ces.enabled = false;
        //        eventList[name] = ces;
        //        return false;
        //    }
        //    else
        //    {
        //        return false;
        //    }
        //}
        /// <summary>
        /// ����һ���¼������û����ע�ᡣ
        /// �Ѿ�������ֱ����register
        /// </summary>
        /// <param name="name"></param>
        /// <param name="action"></param>
        //public void UseCharacterEvent(string name, Action action)
        //{
        //    if (EnableCharacterEvent(name)) return;
        //    RegisterCharacterEvent(name, action);
        //    EnableCharacterEvent(name);
        //}
        /// <summary>
        /// ɾ��һ���¼�
        /// </summary>
        /// <param name="name"></param>
        public void DeleteCharacterEvent(string name)
        {
            if (eventList.TryGetValue(name, out ICharacterEvent ces))
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
            if (eventList.TryGetValue(name, out ICharacterEvent ces))
            {
                if (!(ces is CharacterEventBase)) return false;
                CharacterEventBase c = ces as CharacterEventBase;
                c.action?.Invoke();
                return true;
            }
            return false;
        }
        #region ��ͬ�汾
        public bool ApplyCharacterEvent<E>(string name,E arg1)
        {
            if (eventList.TryGetValue(name, out ICharacterEvent ces))
            {
                if (!(ces is CharacterEventBase<E>)) return false;
                CharacterEventBase<E> c = ces as CharacterEventBase<E>;
                c.action?.Invoke(arg1);
                return true;
            }
            return false;
        }
        public bool ApplyCharacterEvent<E,M>(string name, E arg1,M arg2)
        {
            if (eventList.TryGetValue(name, out ICharacterEvent ces))
            {
                if (!(ces is CharacterEventBase<E, M>))
                    return false;
                CharacterEventBase<E, M> c = ces as CharacterEventBase<E, M>;
                c.action?.Invoke(arg1,arg2);
                return true;
            }
            return false;
        }
        public bool ApplyCharacterEvent<E, M,I>(string name, E arg1, M arg2,I arg3)
        {
            if (eventList.TryGetValue(name, out ICharacterEvent ces))
            {
                if (!(ces is CharacterEventBase<E, M, I>))
                    return false;
                CharacterEventBase<E, M,I> c = ces as CharacterEventBase<E, M,I>;
                c.action?.Invoke(arg1, arg2,arg3);
                return true;
            }
            return false;
        }
        public bool ApplyCharacterEvent<E, M,I,T>(string name, E arg1, M arg2,I arg3,T arg4)
        {
            if (eventList.TryGetValue(name, out ICharacterEvent ces))
            {
                if (!(ces is CharacterEventBase<E, M, I, T>))
                    return false;
                CharacterEventBase<E, M, I, T> c = ces as CharacterEventBase<E, M, I, T>;
                c.action?.Invoke(arg1, arg2,arg3,arg4);
                return true;
            }
            return false;
        }
        public bool ApplyCharacterEvent<E, M, I, T,N>(string name, E arg1, M arg2, I arg3, T arg4,N arg5)
        {
            if (eventList.TryGetValue(name, out ICharacterEvent ces))
            {
                if (!(ces is CharacterEventBase<E, M, I, T, N>))
                    return false;
                CharacterEventBase<E, M, I, T, N> c = ces as CharacterEventBase<E, M, I, T, N>;
                c.action?.Invoke(arg1, arg2, arg3, arg4,arg5);
                return true;
            }
            return false;
        }
        public bool ApplyCharacterEvent<E, M, I, T, N,A>(string name, E arg1, M arg2, I arg3, T arg4, N arg5,A arg6)
        {
            if (eventList.TryGetValue(name, out ICharacterEvent ces))
            {
                if (!(ces is CharacterEventBase<E, M, I, T, N, A>))
                    return false;
                CharacterEventBase<E, M, I, T, N, A> c = ces as CharacterEventBase<E, M, I, T, N, A>;
                c.action?.Invoke(arg1, arg2, arg3, arg4, arg5,arg6);
                return true;
            }
            return false;
        }
        #endregion
    }

}