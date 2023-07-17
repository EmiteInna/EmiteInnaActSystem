using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EmiteInnaACT
{
    public class EPoolService
    {
        public Transform rootTransform;
        public Dictionary<string, EPool> pools=new Dictionary<string, EPool>();
        public void Init(Transform parent)
        {
            rootTransform = new GameObject("EPoolService").transform;
            rootTransform.SetParent(parent);
        }
        /// <summary>
        /// ����һ�������
        /// </summary>
        /// <typeparam name="T">����</typeparam>
        /// <param name="name">����</param>
        /// <param name="maxSize">���������-1Ϊ����</param>
        /// <param name="defaultAmount">Ĭ��Ԫ������</param>
        /// <param name="isGameObject">�Ƿ���GameObject</param>
        /// <param name="prefab">Ԥ���壬����GameObject��Ч</param>
        public void CreateObjectPool<T>(string name,int maxSize,int defaultAmount, 
            bool isGameObject,GameObject prefab = null) where T:new()
        {
            if(pools.TryGetValue(name, out EPool pool))
            {
                Debug.Log("��Ϊ " + name + " �Ķ�����Ѿ����ڣ������ظ�������");
                return;
            }
            EPool p;
            if (isGameObject) p = new EPool(isGameObject, name, maxSize, rootTransform);
            else p = new EPool(isGameObject, name, maxSize, null);
            if (maxSize > 0) defaultAmount = Mathf.Min(maxSize, defaultAmount);
            for(int i = 0; i < defaultAmount; i++)
            {
                if (isGameObject)
                {
                    GameObject g = GameObject.Instantiate(prefab);
                    p.Push(g,isGameObject);
                }
                else
                {
                    T g = new T();
                    p.Push(g, isGameObject);
                }
            }
            if (isGameObject) p.prefab = prefab;
            pools.Add(name, p);
        }
        /// <summary>
        /// �������м������ɸ����󣬽�����Ϊ����ʱָ���Ķ���
        /// </summary>
        /// <typeparam name="T">����</typeparam>
        /// <param name="name">����</param>
        /// <param name="amount">����</param>
        /// <param name="isGameObject">�Ƿ���GameObj</param>
        public void InsertIntoPool<T>(string name,int amount,bool isGameObject)where T:new()
        {
            if(pools.TryGetValue(name,out EPool pool))
            {
                if (pool.maxSize > 0)
                {
                    amount = Mathf.Min(pool.maxSize - pool.queue.Count, amount);
                    for(int i = 0; i < amount; i++)
                    {
                        if (isGameObject)
                        {
                            GameObject g = GameObject.Instantiate(pool.prefab);
                            pool.Push(g, isGameObject);
                        }
                        else
                        {
                            T g = new T();
                            pool.Push(g, isGameObject);
                        }
                    }
                }
            }
            else
            {
                Debug.Log("��Ϊ " + name + " �Ķ���ز����ڣ�");
            }
        }
        /// <summary>
        /// �Ӷ������ȡ��һ��Ԫ�ء�
        /// </summary>
        /// <typeparam name="T">����</typeparam>
        /// <param name="name">���������</param>
        /// <param name="isGameObject">�Ƿ���GameObject</param>
        /// <returns></returns>
        public T Get<T>(string name,bool isGameObject) where T:new()
        {
            object ret = null;
            if(pools.TryGetValue(name,out EPool pool))
            {
                if (pool.queue.Count > 0)
                {
                    return (T)pool.Get(isGameObject);
                }
                else
                {
                    InsertIntoPool<T>(name, 16, isGameObject);
                    return (T)pool.Get(isGameObject);
                }
            }
            return (T)ret;

        }
        public T Get<T>(string name, bool isGameObject,Transform parent) where T : new()
        {
            object ret = null;
            if (pools.TryGetValue(name, out EPool pool))
            {
                if (pool.queue.Count > 0)
                {
                    return (T)pool.Get(isGameObject,parent);
                }
                else
                {
                    InsertIntoPool<T>(name, 16, isGameObject);
                    return (T)pool.Get(isGameObject,parent);
                }
            }
            return (T)ret;

        }
        /// <summary>
        /// ��������أ��������������ֱ�Ӵݻٶ���
        /// </summary>
        /// <param name="obj">����</param>
        /// <param name="name">����</param>
        /// <param name="isGameObject">�Ƿ���GameObject</param>
        public void Push(object obj,string name,bool isGameObject)
        {
            if(pools.TryGetValue(name,out EPool pool))
            {
                if(pool.maxSize>0&& pool.queue.Count >= pool.maxSize)
                {
                    if (isGameObject) GameObject.Destroy(obj as GameObject);
                    else {
                        obj = null;
                    }
                }
                else
                {
                    pool.Push(obj, isGameObject);
                }
            }
        }
        /// <summary>
        /// ��ն���ء�
        /// </summary>
        /// <param name="name"></param>
        public void ClearPool(string name)
        {
            if (pools.TryGetValue(name, out EPool pool))
            {
                pool.Clear();
            }
        }
        /// <summary>
        /// �Ƴ�����ء�
        /// </summary>
        /// <param name="name"></param>
        public void DestroyPool(string name)
        {
            if (pools.TryGetValue(name, out EPool pool))
            {
                pool.Destroy();
                pool = null;
                pools.Remove(name);
            }
        }
        /// <summary>
        /// �ݻٶ���ء�
        /// </summary>
        public void DestroyAll()
        {
            var iterator = pools.GetEnumerator();
            while (iterator.MoveNext())
            {
                iterator.Current.Value.Destroy();
            }
            pools.Clear();
        }
    }
}