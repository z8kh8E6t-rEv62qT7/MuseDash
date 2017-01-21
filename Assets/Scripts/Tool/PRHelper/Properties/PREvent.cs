﻿using System;
using System.Collections.Generic;
using Smart.Types;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Tool.PRHelper.Properties
{
    [Serializable]
    public class PREvents
    {
        public enum EventType
        {
            OnAwake,
            OnStart,
            OnEnable,
            OnUpdate,
            OnFixedUpdate,
            OnDisable,
            OnDestroy,

            OnCollisionEnter,
            OnCollisionStay,
            OnCollisionExit,

            OnTriggerEnter,
            OnTriggerStay,
            OnTriggerExit,

            OnButtonClick,
            OnButtonHover,
            OnButtonUp,
            OnButtonDown,
            OnButton,
        }

        [Serializable]
        public class PREvent
        {
            public EventType eventType;
            public UnityEventGameObject unityEvent;
            public Button button;
            public GameObject gameObject;
        }

        public List<PREvent> events;

        public void Play()
        {
            Debug.Log("Play Event");
        }

        public void Init()
        {
            events.ForEach(e =>
            {
                switch (e.eventType)
                {
                    /* case EventType.OnNGUIButtonClick:
                     {
                         e.NGUIButton.onClick.Add(new EventDelegate(() =>
                         {
                             e.unityEvent.Invoke(null);
                         }));
                     }
                     break;*/
                }
            });
        }
    }
}