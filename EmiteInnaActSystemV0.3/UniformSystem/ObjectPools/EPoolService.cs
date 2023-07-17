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
        /// 创建一个对象池
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="name">名称</param>
        /// <param name="maxSize">最大容量，-1为无穷</param>
        /// <param name="defaultAmount">默认元素数量</param>
        /// <param name="isGameObject">是否是GameObject</param>
        /// <param name="prefab">预制体，仅对GameObject有效</param>
        public void CreateObjectPool<T>(string name,int maxSize,int defaultAmount, 
            bool isGameObject,GameObject prefab = null) where T:new()
        {
            if(pools.TryGetValue(name, out EPool pool))
            {
                Debug.Log("名为 " + name + " 的对象池已经存在，请勿重复创建！");
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
        /// 向对象池中加入若干个对象，将生成为创建时指定的对象。
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="name">名称</param>
        /// <param name="amount">数量</param>
        /// <param name="isGameObject">是否是GameObj</param>
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
                Debug.Log("名为 " + name + " 的对象池不存在！");
            }
        }
        /// <summary>
        /// 从对象池中取出一个元素。
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="name">对象池名称</param>
        /// <param name="isGameObject">是否是GameObject</param>
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
        /// 将对象入池，如果池已满，将直接摧毁对象。
        /// </summary>
        /// <param name="obj">对象</param>
        /// <param name="name">池名</param>
        /// <param name="isGameObject">是否是GameObject</param>
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
        /// 清空对象池。
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
        /// 移除对象池。
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
        /// 摧毁对象池。
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