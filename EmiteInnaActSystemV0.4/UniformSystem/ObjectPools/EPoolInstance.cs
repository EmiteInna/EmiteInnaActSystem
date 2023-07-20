using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EmiteInnaACT
{
    public static class EPoolInstance
    {
        public const string InstanceName = "EmiteInnaPoolInstance";
        static EPoolService service;
        static Transform root;
        /// <summary>
        /// 初始化对象池系统，创建service
        /// </summary>
        public static void Initialize()
        {
            service = new EPoolService();
            root = new GameObject(InstanceName).transform;
            root.SetParent(GameObject.Find("EmiteInnaACTCore").transform);
            service.Init(root);
        }
        /// <summary>
        /// 创建一个对象池。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <param name="maxSize"></param>
        /// <param name="defaultAmount"></param>
        /// <param name="isGameObject"></param>
        /// <param name="prefab"></param>
        public static void CreateObjectPool<T>(string name, int maxSize, int defaultAmount,
            bool isGameObject, GameObject prefab = null) where T : new()
        {
            service.CreateObjectPool<T>(name, maxSize, defaultAmount, isGameObject, prefab);
        }
        /// <summary>
        /// 在对象池中加入东西。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <param name="amount"></param>
        /// <param name="isGameObject"></param>
        public static void InsertIntoPool<T>(string name, int amount, bool isGameObject) where T : new()
        {
            service.InsertIntoPool<T>(name, amount, isGameObject);
        }
        /// <summary>
        /// 获取一个对象。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <param name="isGameObject"></param>
        /// <returns></returns>
        public static T Get<T>(string name, bool isGameObject) where T : new()
        {
            return service.Get<T>(name, isGameObject);
        }
        public static T Get<T>(string name, bool isGameObject,Transform parent) where T : new()
        {
            return service.Get<T>(name, isGameObject,parent);
        }
        /// <summary>
        /// 将对象放入对象池。
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="name"></param>
        /// <param name="isGameObject"></param>
        public static void Push(object obj, string name, bool isGameObject)
        {
            service.Push(obj, name, isGameObject);
        }
        public static void ClearPool(string name)
        {
            service.ClearPool(name);
        }
        public static void DestroyPool(string name)
        {
            service.DestroyPool(name);
        }
        public static void DestroyAll()
        {
            service.DestroyAll();
        }
    }
}
