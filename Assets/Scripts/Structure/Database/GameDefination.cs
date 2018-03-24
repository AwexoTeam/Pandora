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
        public static string MOD_PATH { get { return Application.dataPath + "/mods/"; } }
        public static string MOD_CLASS { get { return "Mod_Initializer"; } }
        public static string MOD_METHOD { get { return "Initialize"; } }
    }
    
    public static class UI_CONSTS
    {
        public const float HOTKEY_BAR_OFFSET = 5;
        public const float INV_SLOT_OFFSET = 49f;
        public static Vector2 INV_SLOT_SIZE { get { return _INV_SLOT_SIZE; } }
        private static Vector2 _INV_SLOT_SIZE = new Vector2(48,48);
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
}
