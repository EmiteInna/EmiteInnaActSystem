using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EmiteInnaACT
{
    /// <summary>
    /// 单个对象池的基类
    /// </summary>
    public class EPool
    {
        public Transform rootTransform;
        public GameObject prefab;
        public string name;
        public Queue<object> queue;
        public int maxSize = -1;
    //    public int currentSize = 0;

        /// <summary>
        /// 新建对象池
        /// </summary>
        /// /// <param name="isGameObjectPool">是否是GameObject池</param>
        /// /// <param name="name">对象池的名字</param>
        /// <param name="maxSize">对象池的最大容量，-1为不限制</param>
        public EPool(bool isGameObjectPool,string name,int maxSize = -1,Transform parent=null)
        {
            if (isGameObjectPool)
            {
                GameObject g = new GameObject(name);
                rootTransform = g.transform;
                if (parent != null) rootTransform.SetParent(parent);
            }
            this.name = name;
            this.maxSize = maxSize;
            if(maxSize <0)queue=new Queue<object>();
            else queue=new Queue<object>(maxSize);
        }
        /// <summary>
        /// 入池。
        /// </summary>
        /// <param name="obj">入池对象</param>
        /// <param name="isGameObject">是否是GameObject</param>
        /// <returns></returns>
        public bool Push(object obj,bool isGameObject)
        {
            if (maxSize >= 0 && queue.Count >= maxSize)
            {
                if (isGameObject) GameObject.Destroy(obj as GameObject);
                return false;
            }
            queue.Enqueue(obj);
            if (isGameObject)
            {
                (obj as GameObject).transform.SetParent(rootTransform);
                (obj as GameObject).SetActive(false);
            }
            return true;
        }
        /// <summary>
        /// 出池。
        /// </summary>
        /// <param name="isGameObject">是否是GameObject</param>
        /// <param name="parent">出池后的父物体，仅对GameObject有用</param>
        /// <returns></returns>
        public object Get(bool isGameObject,Transform parent=null)
        {
            object ret=queue.Dequeue();
            if (isGameObject)
            {
                (ret as GameObject).SetActive(true);
                if (parent!=null)
                {
                    (ret as GameObject).transform.SetParent(parent);
                }
                else
                {
                    (ret as GameObject).transform.SetParent((ret as GameObject).transform.parent);
                    // UnityEngine.SceneManagement.SceneManager.MoveGameObjectToScene((ret as GameObject), UnityEngine.SceneManagement.SceneManager.GetActiveScene());
                }
            }
            return ret;
        }
        /// <summary>
        /// 清空对象池。
        /// </summary>
        public void Clear()
        {
            foreach(object obj in queue)
            {
                if (obj is GameObject) GameObject.Destroy(obj as GameObject);
            }
            queue.Clear();
        }
        /// <summary>
        /// 释放对象池
        /// </summary>
        public void Destroy()
        {
            Clear();
            rootTransform = null;
            prefab = null;
            queue = null;
        }
    }
}