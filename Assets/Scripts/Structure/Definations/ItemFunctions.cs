using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Interfaces;

namespace ItemFunctions
{
    public class GatherTool : IItemFunction
    {
        Gamemaster gamemaster;
        
        public void Initialize(Gamemaster gm)
        {
            gamemaster = gm;
        }
        
        public bool OnItemUsed(EntityController activator)
        {
            if(activator.tag == EntityTag.Player)
            {
                AnimationClip clip;
                gamemaster.fileDatabase.Animation_Database.TryGetValue("", out clip);

                if(clip != null)
                {
                    activator.animator.clip = clip;
                    activator.animator.Play();
                }
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool InInventoryConditionCheck() { return false; }
        public bool SelectedConditionCheck() { return false; }
        
    }
}