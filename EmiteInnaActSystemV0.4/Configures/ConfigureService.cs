using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Unity.VisualScripting;
using UnityEngine;

namespace EmiteInnaACT
{
    /// <summary>
    /// ���ڴ洢���õ����ñ�����ñ�
    /// </summary>
    [CreateAssetMenu(fileName ="NewConfigureService",menuName ="EmiteInnaACT/ConfigureService",order = 1)]
    public class ConfigureService:SerializedScriptableObject
    {
        public Dictionary<string, ConfigureBase> configureList=new Dictionary<string, ConfigureBase>();
        public void AddList(string name, ConfigureBase conf)
        {
            if(configureList.TryGetValue(name,out ConfigureBase _conf))
            {
                Debug.Log("��Ϊ " + name + " �����ñ��Ѿ�����!");
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
    /// ���õ���������������ȫ�ַ�Χ�����ñ�����ʵ����������ò�Ҫ�Ž�ȥ�������ñ��service�档
    /// </summary>
    public static class ConfigureInstance
    {
        static ConfigureService service;
        /// <summary>
        /// ��ֵ
        /// </summary>
        /// <param name="_service"></param>
        public static void InitializeConfigureService(ConfigureService _service)
        {
           service = _service;

        }
        /// <summary>
        /// ��ȡĳ�����ĳ��Ԫ�ء�
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