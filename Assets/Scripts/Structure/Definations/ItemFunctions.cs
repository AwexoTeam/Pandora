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
                gamemaster.fileDatabase.Animation_Database.TryGetValue("Chop_Vertical", out clip);

                if (clip != null && !activator.animationHandler.isPlaying())
                {
                    activator.animationHandler.PlayAnimation(clip, WrapMode.Once);
                }
            }
            
            return false;
        }

        public bool InInventoryConditionCheck() { return false; }
        public bool SelectedConditionCheck() { return false; }
        
    }
}