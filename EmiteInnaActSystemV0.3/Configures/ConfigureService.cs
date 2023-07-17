using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Unity.VisualScripting;
using UnityEngine;

namespace EmiteInnaACT
{
    /// <summary>
    /// 用于存储常用的配置表的配置表
    /// </summary>
    [CreateAssetMenu(fileName ="NewConfigureService",menuName ="EmiteInnaACT/ConfigureService",order = 1)]
    public class ConfigureService:SerializedScriptableObject
    {
        public Dictionary<string, ConfigureBase> configureList=new Dictionary<string, ConfigureBase>();
        public void AddList(string name, ConfigureBase conf)
        {
            if(configureList.TryGetValue(name,out ConfigureBase _conf))
            {
                Debug.Log("名为 " + name + " 的配置表已经存在!");
            }
            else
            {
                configureList.Add(name, conf);
            }
        }
        public void RemoveList(string name)
        {
            if (configureList.TryGetValue(name, out ConfigureBase _conf))
            {
                configureList.Remove(name);
            }
            
        }
    }
    /// <summary>
    /// 配置单例，仅仅适用于全局范围的配置表，单个实例的数据最好不要放进去，而是用别的service存。
    /// </summary>
    public static class ConfigureInstance
    {
        static ConfigureService service;
        /// <summary>
        /// 赋值
        /// </summary>
        /// <param name="_service"></param>
        public static void InitializeConfigureService(ConfigureService _service)
        {
           service = _service;

        }
        /// <summary>
        /// 获取某个表的某个元素。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="listName"></param>
        /// <param name="keyName"></param>
        /// <returns></returns>
        public static T GetValue<T>(string listName,string keyName)
        {
            if(service.configureList.TryGetValue(listName,out ConfigureBase conf))
            {
                return conf.GetValue<T>(keyName);
            }
            return default(T);
        }
    }
    
}