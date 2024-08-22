/*************************************************************************************************
 * Copyright 2024 Theai, Inc. (DBA Inworld)
 *
 * Use of this source code is governed by the Inworld.ai Software Development Kit License Agreement
 * that can be found in the LICENSE.md file or at https://www.inworld.ai/sdk-license
 *************************************************************************************************/

using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Inworld.Playground
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
        private List<Property> m_Properties;
        [SerializeField]
        private List<string> m_Entities;

        private Dictionary<string, string> m_PropertiesDictionary = new Dictionary<string, string>();

        private void Awake()
        {
            foreach (Property property in m_Properties)
                m_PropertiesDictionary.Add(property.Key, property.Value);
        }

        public Packet.EntityItem Get()
        {
            return new Packet.EntityItem(m_ID, m_DisplayName, m_Description, m_PropertiesDictionary);
        }

        public List<string> GetEntities()
        {
            return m_Entities;
        }
    }
}
