using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationHandler : MonoBehaviour {

    public Animation anim;
    public bool shouldLoop = false;

	// Use this for initialization
	void Start () {
        anim = GetComponent<Animation>();
	}
	
	// Update is called once per frame
	void Update () {
		
        if(shouldLoop == false && !anim.isPlaying)
        {
            anim.clip = null;
        }
	}

    public void PlayAnimation(AnimationClip clip, WrapMode mode, string clip_reference = "none", bool shouldSave = true)
    {
        if (shouldSave)
        {
            string name = clip_reference == "none" ? clip.name : clip_reference;
            anim.AddClip(clip, name);
            anim.clip = clip;
            anim.wrapMode = mode;

            if(mode == WrapMode.Loop)
            {
                shouldLoop = true;
            }

            anim.Play(name);
        }
    }
    
    public bool isPlaying() { return anim.isPlaying; }
    public bool isPlaying(string name) { return anim.IsPlaying(name); }

}
