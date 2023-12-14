using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectSpawner : MonoBehaviour
{
    [SerializeField] private GameObject hitEffectPrefab;
    [SerializeField] private GameObject splashEffectPrefab;
    private List<GameObject> effectsHit = new List<GameObject>();
    private List<GameObject> effectsSplash = new List<GameObject>();
    GameObject effectParent;
    public static EffectSpawner Instance;
    private void Awake() {
        if(Instance == null) Instance = this;

        effectParent = new GameObject();
        effectParent.name = "Effect Parent";

        CreateEffects();
        GetEffectSplash();
    }
    #region Effect Pool
    private void CreateEffects()
    {
        for(int i = 0; i < 10; i++)
        {
            var hit = Instantiate(hitEffectPrefab);
            hit.SetActive(false);
            hit.transform.SetParent(effectParent.transform);
            effectsHit.Add(hit);
        }
    }
    public GameObject GetEffect()
    {
        foreach(var hit in effectsHit)
        {
            if(!hit.activeSelf)
                return hit;
        }
        var newHit = Instantiate(hitEffectPrefab);
        newHit.SetActive(false);
        newHit.transform.SetParent(effectParent.transform);
        effectsHit.Add(newHit);
        return newHit;
    }
    #endregion
    #region Splash Effect Pool
    private void CreateEffectsSplash()
    {
        for(int i = 0; i < 10; i++)
        {
            var splash = Instantiate(splashEffectPrefab);
            splash.SetActive(false);
            splash.transform.SetParent(effectParent.transform);
            effectsSplash.Add(splash);
        }
    }
    public GameObject GetEffectSplash()
    {
        foreach(var splash in effectsSplash)
        {
            if(!splash.activeSelf)
                return splash;
        }

        var newSplash = Instantiate(splashEffectPrefab);
        newSplash.SetActive(false);
        newSplash.transform.SetParent(effectParent.transform);
        effectsSplash.Add(newSplash);

        return newSplash;
    }
    #endregion
}
