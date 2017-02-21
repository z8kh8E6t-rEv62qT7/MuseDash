using System;
using System.Linq;
using Assets.Scripts.Common;
using Assets.Scripts.Tools.Commons;
using Assets.Scripts.Tools.Managers;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Assets.Scripts.Tools.PRHelper.Properties
{
    [Serializable]
    public class AudioBinding
    {
        public string name;
        public UnityEngine.Object sourceObj;
        public string fieldName;

        public string path;
        public string key;
        public string index;
        private string m_ResourcePath;
        private AudioSource m_AudioSource;

        public bool isAutoPlay;

        public void Play(GameObject go)
        {
            var collectionBinding = go.GetComponent<PRHelper>().nodes.Find(n => n.nodeType == NodeType.Model_CollectionBinding).collectionBinding;
            index = collectionBinding != null ? collectionBinding.index : ReflectionUtil.Reflect(sourceObj, fieldName);

            var jdata = ConfigManager.instance.GetFromFilePath(path);
            var resourcePath = (string)jdata[index][key];
            if (m_ResourcePath == resourcePath)
            {
                return;
            }
            else
            {
                m_ResourcePath = resourcePath;
            }
            if (!m_AudioSource)
            {
                m_AudioSource = go.GetComponentsInChildren<AudioSource>().ToList().Find(t => t.gameObject.name == name);
            }

            var clip = ResourcesLoader.Load<AudioClip>(m_ResourcePath);
            if (clip)
            {
                m_AudioSource.clip = clip;
                if (isAutoPlay)
                {
                    m_AudioSource.Play();
                }
            }
        }
    }
}