﻿/*************************************************************************************************
 * Copyright 2022 Theai, Inc. (DBA Inworld)
 *
 * Use of this source code is governed by the Inworld.ai Software Development Kit License Agreement
 * that can be found in the LICENSE.md file or at https://www.inworld.ai/sdk-license
 *************************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;


namespace Inworld.Sample
{
    public class CharacterHandler3D : CharacterHandler
    {
        [SerializeField] protected CharSelectingMethod m_SelectingMethod = CharSelectingMethod.SightAngle;
        [Range(0.1f, 1f)]
        [SerializeField] float m_RefreshRate = 0.5f;

        float m_CurrentTime;
        /// <summary>
        ///     Return if any character is speaking.
        /// </summary>
        public override bool IsAnyCharacterSpeaking => m_CharacterList.Any(character => character.IsSpeaking);
        /// <summary>
        ///     Get the current Character Selecting Method.
        /// </summary>
        public override CharSelectingMethod SelectingMethod  => m_SelectingMethod;
        /// <summary>
        ///     Change the method of how to select character.
        /// </summary>
        public override void ChangeSelectingMethod()
        {
            if (m_SelectingMethod == CharSelectingMethod.Manual || m_SelectingMethod == CharSelectingMethod.KeyCode)
                m_SelectingMethod = CharSelectingMethod.SightAngle;
            else if (m_SelectingMethod == CharSelectingMethod.SightAngle)
                m_SelectingMethod = CharSelectingMethod.KeyCode;
        }
        
        void Update()
        {
            switch (m_SelectingMethod)
            {
                case CharSelectingMethod.KeyCode:
                    SelectCharacterByKey();
                    break;
                case CharSelectingMethod.SightAngle:
                    SelectCharacterBySightAngle();
                    break;
            }
        }
        protected virtual void SelectCharacterBySightAngle()
        {
            m_CurrentTime += Time.deltaTime;
            if (m_CurrentTime < m_RefreshRate)
                return;
            m_CurrentTime = 0;
            float fPriority = float.MaxValue;
            InworldCharacter targetCharacter = null;
            foreach (InworldCharacter inworldCharacter in m_CharacterList)
            {
                SightAngle sight = inworldCharacter.GetComponent<SightAngle>();
                if (sight && sight.Priority >= 0 && sight.Priority < fPriority)
                {
                    fPriority = sight.Priority;
                    targetCharacter = inworldCharacter;
                }
            }

            CurrentCharacter = targetCharacter;
        }
        protected virtual void SelectCharacterByKey()
        {
            int minIndex = Mathf.Min(9, m_CharacterList.Count);
            for (int i = 0; i < minIndex; i++)
            {
                if (!Input.GetKeyUp(KeyCode.Alpha1 + i))
                    continue;
                CurrentCharacter = m_CharacterList[i];
                return;
            }
        }
    }
}
