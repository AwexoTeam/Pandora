using GameDefinations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PostProcessing;

public enum TimePrefix
{
    AM,
    PM,
}

public struct TimeData
{
    public float TotalDay
    {
        get
        {
            if(this.TotalTick > CONST.TICK_PR_DAY)
            {
                return Mathf.FloorToInt(this.TotalTick / CONST.TICK_PR_DAY);
            }
            else
            {
                return 0;
            }
        }
    }
    public float Cycle_Hour
    {
        get
        {
            if(this.prefix == TimePrefix.PM) { return Military_Hour - 12; }
            else { return Military_Hour; }
        }
    }
    public float Military_Hour
    {
        get
        {
            float t = this.TotalTick - (this.Second * CONST.TICK_PR_SECOND);
            t -= this.Minute * CONST.TICK_PR_MINUTE;
            if(t >= CONST.TICK_PR_DAY)
            {
                int val = Mathf.FloorToInt(t / CONST.TICK_PR_DAY);
                t -= val * CONST.TICK_PR_DAY;
            }

            return Mathf.FloorToInt(t / CONST.TICK_PR_HOUR);
        }
    }
    public float Minute
    {
        get
        {
            float t = this.TotalTick - (this.Second * CONST.TICK_PR_SECOND);
            int val = Mathf.FloorToInt(t / CONST.TICK_PR_HOUR);

            t -= val * CONST.TICK_PR_HOUR;
            return Mathf.FloorToInt(t / CONST.TICK_PR_MINUTE);
        }
    }

    public float Second {
        get
        {
            float t = this.TotalTick;
            int val = Mathf.FloorToInt(t / CONST.TICK_PR_MINUTE);

            t -= val * CONST.TICK_PR_MINUTE;
            return Mathf.FloorToInt(t / CONST.TICK_PR_SECOND);
        }
    }
    
    public TimePrefix prefix
    {
        get
        {
            return Military_Hour > 12 ? TimePrefix.PM : TimePrefix.AM;
        }
    }

    public float TotalTick;
    public float GameTick;

    public TimeData(float _t, float _g)
    {
        TotalTick = _t;
        GameTick = _g;
    }
}

public class EffectManager : MonoBehaviour {

    public PostProcessingProfile dayProfile;
    public PostProcessingProfile nightProfile;
    public PostProcessingProfile hpDefinency;
    public Texture2D lut;
    
    public TimeData currTime;

    [Range(1,100)]
    public float health;
    
    private float min_health_activate;

    private Color[] nightColor;
    private Color[] dayColor;
    private PostProcessingBehaviour behaviour;
    private PostProcessingProfile currProfile;
    private PostProcessingProfile healthLowProfile;

    private bool shouldSet = true;
    private int multiplier = 1;

    public Light sun;
    public Color nightMoonColor;
    private Color daySunColor;

    public float nightIntensity;
    private float dayIntensity;
    private float val;

    public Vector4 time;

    public void Start()
    {
        behaviour = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<PostProcessingBehaviour>();
        currProfile = new PostProcessingProfile();
        healthLowProfile = new PostProcessingProfile();

        min_health_activate = CONST.MIN_HEALTH_ACTIVATE;

        dayColor = new Color[3];
        dayColor[0] = dayProfile.colorGrading.settings.colorWheels.linear.lift;
        dayColor[1] = dayProfile.colorGrading.settings.colorWheels.linear.gamma;
        dayColor[2] = dayProfile.colorGrading.settings.colorWheels.linear.gain;

        nightColor = new Color[3];
        nightColor[0] = nightProfile.colorGrading.settings.colorWheels.linear.lift;
        nightColor[1] = nightProfile.colorGrading.settings.colorWheels.linear.gamma;
        nightColor[2] = nightProfile.colorGrading.settings.colorWheels.linear.gain;

        currProfile.name = "Currprofile";
        currProfile.colorGrading.enabled = true;
        currProfile.userLut.enabled = true;

        healthLowProfile.name = "HealthLow";
        healthLowProfile.colorGrading.enabled = true;
        healthLowProfile.userLut.enabled = true;

        daySunColor = sun.color;
        dayIntensity = sun.intensity;

        InvokeRepeating("dayTick", 0, CONST.DAY_TICK);
    }

    public void dayTick()
    {
        setColorWheelSettings();
        setLutSettings();

        if(currTime.GameTick >= CONST.MAX_DAY_TICK / 2)
        {
            multiplier = -1;
        }
        else if(currTime.GameTick <= 0)
        {
            multiplier = 1;
        }

        if (currTime.Military_Hour < CONST.SUN_RISE) { val = 190; }

        else if (currTime.Military_Hour >= CONST.SUN_RISE && currTime.Military_Hour <= CONST.SUN_MID)
        {
            float norm = GetInverseNormalizeValue(CONST.SUN_RISE * CONST.TICK_PR_HOUR, CONST.SUN_MID * CONST.TICK_PR_HOUR, currTime.GameTick);
            val = Mathf.Lerp(90, 190, norm);
        }
        else
        {
            float norm = GetNormalizeValue(CONST.SUN_MID * CONST.TICK_PR_HOUR, CONST.MAX_DAY_TICK, currTime.GameTick) * -1;
            val = Mathf.Lerp(90, 190, norm);
        }

        currTime.GameTick += multiplier;
            currTime.TotalTick += 1;

        sun.gameObject.transform.rotation = Quaternion.Euler(val, 0, 0);

        
        time = new Vector4(currTime.Second, currTime.Minute, currTime.Military_Hour,currTime.TotalDay);
    }

    public void Update()
    {
        if(health <= min_health_activate)
        {
            if (shouldSet)
            {
                behaviour.profile = healthLowProfile;
                shouldSet = false;
            }
            healthLowProfile.colorGrading.settings = currProfile.colorGrading.settings;
            healthLowProfile.userLut.settings = currProfile.userLut.settings;

            setFieldOfVision();
            setVignette();
            setAberration();
            
        }
        else
        {
            currProfile.depthOfField.enabled = false;
            currProfile.vignette.enabled = false;
            currProfile.chromaticAberration.enabled = false;

            shouldSet = true;
            behaviour.profile = currProfile;
        }
        
    }

    public float DayBasedLerp(float a, float b)
    {
        float fHour = currTime.GameTick;
        float normalized = fHour / CONST.MAX_DAY_TICK;

        return Mathf.Lerp(a, b, normalized);
    }

    public Color DayBasedLerp(Color a, Color b)
    {
        float fHour = currTime.GameTick;
        float normalized = fHour / CONST.MAX_DAY_TICK;

        float _r = Mathf.Lerp(a.r, b.r, normalized);
        float _g = Mathf.Lerp(a.g, b.g, normalized);
        float _b = Mathf.Lerp(a.b, b.b, normalized);
        float _a = Mathf.Lerp(a.a, b.a, normalized);

        return new Color(_r, _g, _b, _a);
    }

    public float GetNormalizeValue(float min, float max, float val)
    {
        return (val - min) / (max - min);
    }

    public float GetInverseNormalizeValue(float min, float max, float val)
    {
        return 1 - GetNormalizeValue(min, max, val);
    }

    public void setLutSettings()
    {
        var settings = currProfile.userLut.settings;

        settings.lut = lut;

        float fhour = currTime.GameTick;
        settings.contribution = fhour / CONST.MAX_DAY_TICK;

        currProfile.userLut.settings = settings;
    }

    public void setColorWheelSettings()
    {
        ColorGradingModel.Settings currSettings = currProfile.colorGrading.settings;
        ColorGradingModel.Settings daySettings = dayProfile.colorGrading.settings;
        ColorGradingModel.Settings nightSettings = nightProfile.colorGrading.settings;

        Color currLift = DayBasedLerp(dayColor[0], nightColor[0]);
        Color currGamma = DayBasedLerp(dayColor[1], nightColor[1]);
        Color currGain = DayBasedLerp(dayColor[2], nightColor[2]);

        currSettings.colorWheels.linear.lift = currLift;
        currSettings.colorWheels.linear.gamma = currGamma;
        currSettings.colorWheels.linear.gain = currGain;
        
        float currTempature = DayBasedLerp(daySettings.basic.temperature, nightSettings.basic.temperature);
        float currTint = DayBasedLerp(daySettings.basic.tint, nightSettings.basic.tint);
        float currHueShift = DayBasedLerp(daySettings.basic.hueShift, nightSettings.basic.hueShift);
        float currSaturation = DayBasedLerp(daySettings.basic.saturation, nightSettings.basic.saturation);
        
        currSettings.basic.temperature = currTempature;
        currSettings.basic.saturation = currSaturation;
        currSettings.basic.tint = currTint;
        currSettings.basic.hueShift = currHueShift;

        currProfile.colorGrading.settings = currSettings;
    }

    public void setFieldOfVision()
    {
        if(healthLowProfile.depthOfField.enabled == false) { healthLowProfile.depthOfField.enabled = true; }

        var healthSettings = hpDefinency.depthOfField.settings;

        float normalized = GetInverseNormalizeValue(1, min_health_activate, health);

        float currFocusDist = Mathf.Lerp(2.5f, healthSettings.focusDistance,normalized);
        float currAptStop = Mathf.Lerp(2, healthSettings.aperture, normalized);

        healthSettings.focusDistance = currFocusDist;
        healthSettings.aperture = currAptStop;

        healthLowProfile.depthOfField.settings = healthSettings;
    }

    public void setVignette()
    {
        if(healthLowProfile.vignette.enabled == false) { healthLowProfile.vignette.enabled = true; }

        var currSettings = currProfile.vignette.settings;
        var healthSettings = hpDefinency.vignette.settings;
        var newSettings = healthLowProfile.vignette.settings;

        Color a = currSettings.color;
        Color b = healthSettings.color;
        float normalized = GetInverseNormalizeValue(0, min_health_activate, health);

        float _r = Mathf.Lerp(b.r, a.r, normalized);
        float _g = Mathf.Lerp(b.g, a.g, normalized);
        float _b = Mathf.Lerp(b.b, a.b, normalized);
        float _a = Mathf.Lerp(b.a, a.a, normalized);
        
        Color newColor = new Color(_r, _g, _b, _a);

        newSettings.center = new Vector2(0.5f, 0.5f);
        newSettings.intensity = Mathf.Lerp(0,0.5f,normalized);
        Debug.Log(newSettings.intensity);
        newSettings.rounded = true;
        newSettings.roundness = 1;
        newSettings.smoothness = 1; 
        newSettings.color = newColor;

        healthLowProfile.vignette.settings = newSettings;
    }

    public void setAberration()
    {
        if(healthLowProfile.chromaticAberration.enabled == false) { healthLowProfile.chromaticAberration.enabled = true; }
        var settings = healthLowProfile.chromaticAberration.settings;

        settings.intensity = GetInverseNormalizeValue(1, min_health_activate, health);
        healthLowProfile.chromaticAberration.settings = settings;
    }

    public void setSun()
    {
        sun.color = DayBasedLerp(daySunColor, nightMoonColor);
    }
}