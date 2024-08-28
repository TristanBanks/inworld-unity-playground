/*************************************************************************************************
 * Copyright 2024 Theai, Inc. (DBA Inworld)
 *
 * Use of this source code is governed by the Inworld.ai Software Development Kit License Agreement
 * that can be found in the LICENSE.md file or at https://www.inworld.ai/sdk-license
 *************************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Inworld.Map
{
#if UNITY_EDITOR
    [CustomEditor(typeof(EntityItem))]
    public class EntityItemEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            if (GUILayout.Button("Generate ID"))
            {
                serializedObject.FindProperty("m_ID").stringValue = Guid.NewGuid().ToString();
            }
            serializedObject.ApplyModifiedProperties();
            
            base.OnInspectorGUI();
        }
    }
#endif
    public class EntityItem : MonoBehaviour
    {
        public string ID => m_ID;
        public string DisplayName => m_DisplayName;
        public string Description => m_Description;
        
        [Serializable]
        public struct Property
        {
            public string Key;
            public string Value;
        }
        [SerializeField]
        private string m_ID;
        [SerializeField]
        private string m_DisplayName;
        [SerializeField]
        private string m_Description;
        [SerializeField]
        private bool m_CreateOnStart = true;
        [SerializeField]
        private List<Property> m_Properties;
        [SerializeField]
        private List<Entity> m_Entities;

        private Dictionary<string, string> m_PropertiesDictionary = new Dictionary<string, string>();

        public void UpdateDescription(string description, bool sync = true)
        {
            m_Description = description;
            if(sync)
                EntityManager.Instance.UpdateItem(this);
        }

        public bool UpdateProperty(string key, string value, bool sync = true)
        {
            if (m_PropertiesDictionary.ContainsKey(key))
            {
                m_PropertiesDictionary[key] = value;
                if(sync)
                    EntityManager.Instance.UpdateItem(this);
                return true;
            }
            return false;
        }

        public Packet.EntityItem Get()
        {
            return new Packet.EntityItem(m_ID, m_DisplayName, m_Description, m_PropertiesDictionary);
        }

        public List<string> GetEntityIDs()
        {
            return new List<string>(m_Entities.Select(entity => entity.ID));
        }

        protected virtual void Awake()
        {
            foreach (Property property in m_Properties)
                m_PropertiesDictionary.Add(property.Key, property.Value);
        }

        protected virtual void Start()
        {
            if(m_CreateOnStart)
                EntityManager.Instance.AddItem(this);
        }

        protected void OnDestroy()
        {
            if(EntityManager.Instance)
                EntityManager.Instance.RemoveItem(this);
        }
    }
}
