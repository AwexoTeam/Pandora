using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Interfaces;

namespace ItemFunctions
{
    public class Usable : IItemFunction
    {
        Gamemaster gameMaster;
        int index;
        int value;

        public Usable(int i, int v)
        {
            index = i;
            value = v;
        }

        public bool InInventoryConditionCheck(EntityController activator)
        {
            return false;
        }

        public void Initialize(Gamemaster gm)
        {
            return;
        }

        public bool OnItemUsed(EntityController activator)
        {
            Debug.Log("USED POT");
            activator.status.AddToCurrStat(index, value);
            return true;
        }

        public void OnSelectedUse(EntityController activator)
        {
            return;
        }

        public bool SelectedConditionCheck(EntityController activator)
        {
            return false;
        }
    }

    public class GatherTool : IItemFunction
    {
        Gamemaster gamemaster;
        
        public void Initialize(Gamemaster gm)
        {
            gamemaster = gm;
        }
        
        public bool OnItemUsed(EntityController activator)
        {
            return false;
        }

        public bool InInventoryConditionCheck(EntityController activator) { return false; }
        public bool SelectedConditionCheck(EntityController activator)
        {
            return false;
        }

        public void OnSelectedUse(EntityController activator)
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
        }
    }
}