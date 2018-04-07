using GameDefinations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;

namespace Interfaces
{

    public abstract class Activatable
    {
        public int loadTime = 1;
        public object[] args;

        public string UID;
        public Timer loopTick;

        public Activatable()
        {
            UID = Guid.NewGuid().ToString();
            loopTick = new Timer((double)CONST.PREPARE_TICK);
        }
        
        public abstract bool isActivable(EntityController activator);
        public abstract void Use(EntityController activator);
        public virtual void End(EntityController activator) { this.loopTick.Stop(); }

        public virtual void Prepare(EntityController activator)
        {
            this.loopTick.Start();

            loopTick.Elapsed += (sender, e) => PrepareLoop(sender, e, activator);
        }
        
        public virtual void PrepareLoop(object sender, ElapsedEventArgs e, EntityController activator) { }
        
        public virtual void Interrupted(EntityController activator) { End(activator); }
        public virtual void Cancel(EntityController activator) { End(activator); }

    }
    
    public interface IEffect
    {
        void doEffectTick(EntityController affected);
    }

    public interface IItemFunction
    {
        void Initialize(Gamemaster gm);
        bool SelectedConditionCheck(EntityController activator);
        bool OnItemUsed(EntityController activator);
        bool InInventoryConditionCheck(EntityController activator);
        void OnSelectedUse(EntityController activator);
    }
}
    