/*************************************************************************************************
 * Copyright 2024 Theai, Inc. (DBA Inworld)
 *
 * Use of this source code is governed by the Inworld.ai Software Development Kit License Agreement
 * that can be found in the LICENSE.md file or at https://www.inworld.ai/sdk-license
 *************************************************************************************************/
#if UNITY_EDITOR
using Inworld.BehaviorEngine;
using UnityEditor;

namespace Inworld.Editors
{
    [CustomEditor(typeof(EntityManager))]
    public class EntityManagerInspector : Editor
    {
        public override void OnInspectorGUI()
        {
            EditorGUI.BeginDisabledGroup(false);
            base.OnInspectorGUI();
            EditorGUI.EndDisabledGroup();
        }
    }
}
#endif
