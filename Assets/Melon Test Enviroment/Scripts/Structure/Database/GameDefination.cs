using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameDefinations
{
    public static class CONST
    {
        public const float MAX_STAT = 1500;
        public const int DEF_COUNTER = 10;
        public const float PREPARE_TICK = 100;

        public const float MIN_HEALTH_ACTIVATE = 3;

        public const float DAY_TICK = 0.0005f;
        public const float TICK_PR_SECOND = 0.5f;
        public const float SUN_RISE = 4;
        public const float SUN_MID = 12;
        public const float SUN_SET = 20;
        public static float TICK_PR_DAY { get { return TICK_PR_HOUR * HOUR_PR_DAY; } }
        public static float TICK_PR_HOUR { get { return TICK_PR_MINUTE * MINUTE_PR_HOUR; } }
        public static float TICK_PR_MINUTE { get { return TICK_PR_SECOND * SECONDS_PR_MINUTE; } }
        public static float MAX_DAY_TICK { get { return HOUR_PR_DAY * TICK_PR_HOUR; } }
        public static float HALF_MAX_DAY_TICK { get { return MAX_DAY_TICK / 2; } }
        public static float HALF_DAY_HOUR { get { return HOUR_PR_DAY / 2; } }
        

        public const float HOUR_PR_DAY = 24;
        public const float MINUTE_PR_HOUR = 10;
        public const float SECONDS_PR_MINUTE = 10;

        public static string MOD_PATH { get { return Application.dataPath + "/mods/"; } }
        public static string MOD_CLASS { get { return "Mod_Initializer"; } }
        public static string MOD_METHOD { get { return "Initialize"; } }
    }
    
    public static class UI_CONSTS
    {
        public const string INV_SLOT_NAME = "";
        public const float HOTKEY_BAR_OFFSET = 5;
        public const float INV_SLOT_OFFSET = 4f;
        public static Vector2Int INV_SLOT_SIZE { get { return _INV_SLOT_SIZE; } }
        private static Vector2Int _INV_SLOT_SIZE = new Vector2Int(48,48);
        
        public static Vector2Int INV_SIZE { get { return _INV_SIZE; } }
        private static Vector2Int _INV_SIZE = new Vector2Int(8,3);
    }
    
    public struct NodeInfo
    {
        public GatherType type;
        //public Item resource;
        public int hitpoints;
        public Vector3 pos;

        public NodeInfo(GatherType _type, Vector3 _pos, int _hitpoints)//, Item _resource)
        {   
            type = _type;
            pos = _pos;
            hitpoints = _hitpoints;
            //resource = _resource;
        }

        public static bool operator ==(Vector3 v1, NodeInfo node)
        {
            bool rtn = false;

            if(node.pos == v1) { rtn = true; }

            return rtn;
        }

        public static bool operator !=(Vector3 v1, NodeInfo node)
        {
            bool rtn = false;

            if(node.pos != v1) { rtn = true; }

            return rtn;
        }

        public static bool operator ==(NodeInfo n1, NodeInfo n2)
        {
            bool rtn = true;
            
            if(n1.pos != n2.pos) { rtn = false; }
            if(n1.hitpoints != n2.hitpoints) { rtn = false; }
            //if(n1.resource != n2.resource) { rtn = false; }
            if(n1.type != n2.type) { rtn = false; }

            return rtn;
        }

        public static bool operator !=(NodeInfo n1, NodeInfo n2)
        {
            bool rtn = false;
            
            if(n1.pos != n2.pos) { rtn = false; }
            if(n1.hitpoints != n2.hitpoints) { rtn = false; }
            //if(n1.resource != n2.resource) { rtn = false; }
            if(n1.type != n2.type) { rtn = false; }

            return !rtn;
        }

        
    }
    
    public enum GatherType
    {
        Axe,
        Pickaxe,
        Shovel,
    }

    public enum Stats
    {
        HP,
        MP,
        SP,

        HUNGER,
        THIRST,
        LUST,

    }

    public class Ticks
    {
        void DoPosionTick() { }
        void DoStunTick() { }
        void DoCumDrenchedTick() { }
        void DoHungerTick() { }
    }
}
