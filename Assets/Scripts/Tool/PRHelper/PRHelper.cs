﻿using System;
using System.Linq;
using Assets.Scripts.Tool.PRHelper.Properties;
using EasyEditor;
using Smart.Types;
using UnityEngine;

namespace Assets.Scripts.Tool.PRHelper
{
    public class PRHelper : MonoBehaviour
    {
        [HideInInspector]
        public bool isNewNode = true;

        [Inspector(group = "Create New Node")]
        public string newNodeName;

        [Inspector(group = "Node Function")]
        public PRHelperNode[] nodes;

        public PRHelperNode this[string key]
        {
            get { return nodes.ToList().Find(n => n.key == key); }
        }

        private void Awake()
        {
            if (nodes == null) nodes = new PRHelperNode[0];
            nodes.ToList().ForEach(n => n.Init(gameObject));
            OnEventInvoke(PREvents.EventType.OnAwake);
        }

        public void Play(string key)
        {
            this[key].Play();
        }

        public static UnityEventGameObject OnEvent(GameObject go, PREvents.EventType eventType)
        {
            var prHelper = go.GetComponent<PRHelper>() ?? go.AddComponent<PRHelper>();
            if (prHelper.nodes == null)
            {
                prHelper.nodes = new PRHelperNode[0];
            }
            var allNodes = prHelper.nodes.ToList();
            var node = allNodes.Find(n => n.nodeType == NodeType.Event_PREvents);
            if (node == null)
            {
                node = new PRHelperNode();
                node.pREvents = new PREvents();
                node.nodeType = NodeType.Event_PREvents;
                node.isActive = true;
                allNodes.Add(node);
            }
            var prEvent = node.pREvents.events.Find(e => e.eventType == eventType);
            var unityEvent = new UnityEventGameObject();
            if (prEvent == null)
            {
                prEvent = new PREvents.PREvent(eventType, unityEvent);
                node.pREvents.events.Add(prEvent);
            }
            else
            {
                unityEvent = prEvent.unityEvent;
            }
            prHelper.nodes = allNodes.ToArray();
            return unityEvent;
        }

        #region Event Func

        private void OnEventInvoke(PREvents.EventType eventType, object args = null)
        {
            nodes.ToList().ForEach(n =>
            {
                n.pREvents.events.Where(e => e.eventType == eventType).ToList().ForEach(e => e.unityEvent.Invoke(args as GameObject));
            });
        }

        private void Start()
        {
            OnEventInvoke(PREvents.EventType.OnStart);
        }

        private void OnEnable()
        {
            OnEventInvoke(PREvents.EventType.OnEnable);
        }

        private void OnDisable()
        {
            OnEventInvoke(PREvents.EventType.OnDisable);
        }

        private void OnUpdate()
        {
            OnEventInvoke(PREvents.EventType.OnUpdate);
        }

        private void OnFixedUpdate()
        {
            OnEventInvoke(PREvents.EventType.OnFixedUpdate);
        }

        private void OnDestroy()
        {
            OnEventInvoke(PREvents.EventType.OnDestroy);
        }

        private void OnTriggerEnter(Collider col)
        {
            OnEventInvoke(PREvents.EventType.OnTriggerEnter, col);
        }

        private void OnTriggerStay(Collider col)
        {
            OnEventInvoke(PREvents.EventType.OnTriggerStay, col);
        }

        private void OnTriggerExit(Collider col)
        {
            OnEventInvoke(PREvents.EventType.OnTriggerExit, col);
        }

        private void OnCollisionEnter(Collision col)
        {
            OnEventInvoke(PREvents.EventType.OnCollisionEnter, col);
        }

        private void OnCollisionStay(Collision col)
        {
            OnEventInvoke(PREvents.EventType.OnCollisionStay, col);
        }

        private void OnCollisionExit(Collision col)
        {
            OnEventInvoke(PREvents.EventType.OnCollisionExit, col);
        }

        #endregion Event Func
    }
}