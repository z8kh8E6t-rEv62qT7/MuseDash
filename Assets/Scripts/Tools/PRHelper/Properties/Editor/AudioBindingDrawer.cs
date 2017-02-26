using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Tools.Commons;
using Assets.Scripts.Tools.Commons.Editor;
using Assets.Scripts.Tools.Managers;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Tools.PRHelper.Properties.Editor
{
    [CustomPropertyDrawer(typeof(AudioBinding))]
    public class AudioBindingDrawer : PropertyDrawer
    {
        private float m_Height = 15;
        private float m_Gap = 5;

        public override void OnGUI(Rect rect, SerializedProperty property, GUIContent label)
        {
            var parent = Selection.activeGameObject;
            var acs = new List<AudioSource>();
            var ac = parent.GetComponent<AudioSource>();
            if (ac != null) acs.Add(ac);
            var hasRoot = Selection.activeGameObject.GetComponent<PRHelper>().nodes.Exists(n => n.nodeType == NodeType.Model_CollectionBinding);
            var childAc = parent.GetComponentsInChildren<AudioSource>();
            childAc.ToList().ForEach(acs.Add);

            rect = EditorUtils.MakePopupField(property, "name", new GUIContent("AudioSource Name"),
                childAc.Select(c => c.name).ToArray(), rect, m_Gap, m_Height, false, null, false, "None");
            rect = EditorUtils.MakePropertyField("isAutoPlay", property, rect, m_Gap, m_Height);

            rect = EditorUtils.MakePopupField(property, "path", new GUIContent("Json Path"),
               ConfigManager.instance.configs.Select(c => c.path).ToArray(), rect, m_Gap, m_Height);
            var jdata = ConfigManager.instance.Convert(property.FindPropertyRelative("path").stringValue);
            if (jdata == null) return;
            if (!jdata.IsArray)
            {
                rect = EditorUtils.MakePopupField(property, "key", new GUIContent("Json Key"),
                 jdata.Keys.ToArray(), rect, m_Gap, m_Height);
            }
            else
            {
                var sourceObjPpt = property.FindPropertyRelative("reflectObj").FindPropertyRelative("sourceObj");

                rect = EditorUtils.MakePopupField(property, "key", new GUIContent("Json Key"),
             jdata[0].Keys.ToArray(), rect, m_Gap, m_Height);
                if (!hasRoot)
                {
                    rect = sourceObjPpt.objectReferenceValue != null ? EditorUtils.MakeObjectField(property.FindPropertyRelative("reflectObj"), new GUIContent("Param"), rect, m_Gap, m_Height) : EditorUtils.MakePropertyField("index", property, rect, m_Gap, m_Height, new GUIContent("Param"));
                    rect = EditorUtils.MakePropertyField(sourceObjPpt, rect, m_Gap, m_Height);
                }
            }
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            var hasRoot = Selection.activeGameObject.GetComponent<PRHelper>().nodes.Exists(n => n.nodeType == NodeType.Model_CollectionBinding);
            if (hasRoot)
            {
                return 80;
            }
            return 120;
        }
    }
}