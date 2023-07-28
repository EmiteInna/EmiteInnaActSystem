using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EmiteInnaACT
{
    /// <summary>
    /// ��������صĻ���
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
        /// �½������
        /// </summary>
        /// /// <param name="isGameObjectPool">�Ƿ���GameObject��</param>
        /// /// <param name="name">����ص�����</param>
        /// <param name="maxSize">����ص����������-1Ϊ������</param>
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
        /// ��ء�
        /// </summary>
        /// <param name="obj">��ض���</param>
        /// <param name="isGameObject">�Ƿ���GameObject</param>
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
        /// ���ء�
        /// </summary>
        /// <param name="isGameObject">�Ƿ���GameObject</param>
        /// <param name="parent">���غ�ĸ����壬����GameObject����</param>
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
        /// ��ն���ء�
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
        /// �ͷŶ����
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