/*************************************************************************************************
 * Copyright 2024 Theai, Inc. dba Inworld AI
 *
 * Use of this source code is governed by the Inworld.ai Software Development Kit License Agreement
 * that can be found in the LICENSE.md file or at https://www.inworld.ai/sdk-license
 *************************************************************************************************/

using System.Collections;
using System.Collections.Generic;
using Inworld.Packet;
using UnityEngine;

namespace Inworld.Map
{
    [CreateAssetMenu(fileName = "OpenTask", menuName = "Inworld/Tasks/OpenTask")]
    public class OpenTask : ItemTask
    {
        public override IEnumerator Perform(InworldCharacter inworldCharacter, Dictionary<string, string> parameters)
        {
            if (!parameters.TryGetValue("what", out string itemID) || !EntityManager.Instance.FindItem(itemID, out EntityItem entityItem))
            {
                EntityManager.Instance.FailTask(this, inworldCharacter, $"Failed to find item: {itemID}.", parameters);
                yield break;
            }
            
            if (!IsItemNearby(inworldCharacter, entityItem))
            {
                EntityManager.Instance.FailTask(this, inworldCharacter, $"Item is too far away: {itemID}.", parameters);
                yield break;
            }

            EntityItemLock entityEntityItemLock = entityItem.GetComponent<EntityItemLock>();
            if (entityEntityItemLock)
            {
                EntityManager.Instance.FailTask(this, inworldCharacter, $"Could not open {itemID}, it is locked.", parameters);
                yield break;
            }
            
            AnimatorBoolParamHandler animatorBoolParamHandler = entityItem.GetComponent<AnimatorBoolParamHandler>();
            if (!animatorBoolParamHandler)
            {
                EntityManager.Instance.FailTask(this, inworldCharacter, $"Could not open {itemID}.", parameters);
                yield break;
            }

            if (animatorBoolParamHandler.SetTrue())
            {
                entityItem.UpdateProperty("state", "Opened.");
                EntityManager.Instance.CompleteTask(this, inworldCharacter, parameters);
            }
            else
                EntityManager.Instance.FailTask(this, inworldCharacter, $"{itemID} is already open.", parameters);
        }
    }
}
