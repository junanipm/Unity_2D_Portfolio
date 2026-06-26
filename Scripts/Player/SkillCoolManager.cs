<<<<<<< HEAD
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SkillCoolManager : MonoBehaviour
{
    private AnimatorConverter animConverter;
    
    private Dictionary<PlayerState, float> cooldownDurations = new Dictionary<PlayerState, float>()
    {
        {PlayerState.Blue,5f},
        {PlayerState.Yellow, 10f},
        {PlayerState.Purple, 5f},
        {PlayerState.White, 0f}
    };

    private Dictionary<PlayerState, float> currentCooldown = new Dictionary<PlayerState, float>()
    {
        {PlayerState.Blue,0f},
        {PlayerState.Yellow, 0f},
        {PlayerState.Purple, 0f},
        {PlayerState.White, 0f}
    };

    void Awake()
    {
        animConverter = GameObject.FindGameObjectWithTag("Player").GetComponent<AnimatorConverter>();
    }
    void Update()
    {
        UpdateCooldown(Time.deltaTime);
    }
    public PlayerState GetCurrentState()
    {
        return animConverter.currentState;
    }

    public bool CanUseSkill(PlayerState state)
    {
        return currentCooldown[state] <= 0f;
    }
    

    public void StartCooldown(PlayerState state)
    {
        
        currentCooldown[state] = cooldownDurations[state];

    }

    public void UpdateCooldown(float deltaTime)
    {
        foreach (var state in currentCooldown.Keys.ToList())
        {
            if(currentCooldown[state] > 0f)
                currentCooldown[state] -= deltaTime;
        }
    }

    public float GetCooldownProgress(PlayerState state)
    {
        float current = currentCooldown[state];
        float max = cooldownDurations[state];

        return Mathf.Clamp01(current / max);
    }
    public float GetCooldownDuration(PlayerState state)
    {
        return cooldownDurations[state];
    }
}
=======
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SkillCoolManager : MonoBehaviour
{
    private AnimatorConverter animConverter;
    
    private Dictionary<PlayerState, float> cooldownDurations = new Dictionary<PlayerState, float>()
    {
        {PlayerState.Blue,5f},
        {PlayerState.Yellow, 10f},
        {PlayerState.Purple, 5f},
        {PlayerState.White, 0f}
    };

    private Dictionary<PlayerState, float> currentCooldown = new Dictionary<PlayerState, float>()
    {
        {PlayerState.Blue,0f},
        {PlayerState.Yellow, 0f},
        {PlayerState.Purple, 0f},
        {PlayerState.White, 0f}
    };

    void Awake()
    {
        animConverter = GameObject.FindGameObjectWithTag("Player").GetComponent<AnimatorConverter>();
    }
    void Update()
    {
        UpdateCooldown(Time.deltaTime);
    }
    public PlayerState GetCurrentState()
    {
        return animConverter.currentState;
    }

    public bool CanUseSkill(PlayerState state)
    {
        return currentCooldown[state] <= 0f;
    }
    

    public void StartCooldown(PlayerState state)
    {
        
        currentCooldown[state] = cooldownDurations[state];

    }

    public void UpdateCooldown(float deltaTime)
    {
        foreach (var state in currentCooldown.Keys.ToList())
        {
            if(currentCooldown[state] > 0f)
                currentCooldown[state] -= deltaTime;
        }
    }

    public float GetCooldownProgress(PlayerState state)
    {
        float current = currentCooldown[state];
        float max = cooldownDurations[state];

        return Mathf.Clamp01(current / max);
    }
    public float GetCooldownDuration(PlayerState state)
    {
        return cooldownDurations[state];
    }
}
>>>>>>> c1275e0b368542d9fd2997dc6383c3e959a629e3
