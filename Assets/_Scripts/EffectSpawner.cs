using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectSpawner : MonoBehaviour
{
    [SerializeField] private GameObject hitEffectPrefab;
    [SerializeField] private GameObject splashEffectPrefab;
    [SerializeField] private GameObject bombEffectPrefab;
    [SerializeField] private GameObject juiceEffectPrefab;

    private List<GameObject> effectsHit = new List<GameObject>();
    private List<GameObject> effectsSplash = new List<GameObject>();
    private List<GameObject> effectsBomb = new List<GameObject>();
    private List<GameObject> effectsJuice = new List<GameObject>();
    private List<EffectHolder> effectHolders = new List<EffectHolder>();

    private GameObject effectParent;

    private EffectHolder hitEfHolder, splashEfHolder, bombEfHolder, juiceEfHolder;

    public static EffectSpawner Instance;
    private void Awake() {
        if(Instance == null) Instance = this;

        effectParent = new GameObject();
        effectParent.name = "Effect Parent";

        hitEfHolder = new EffectHolder(EffectType.HitEffect, hitEffectPrefab, effectsHit);
        splashEfHolder = new EffectHolder(EffectType.SplashEffect, splashEffectPrefab, effectsSplash);
        bombEfHolder = new EffectHolder(EffectType.BombEffect, bombEffectPrefab, effectsBomb);
        juiceEfHolder = new EffectHolder(EffectType.JuiceEffect, juiceEffectPrefab, effectsJuice);

        effectHolders.Add(hitEfHolder);
        effectHolders.Add(splashEfHolder);
        effectHolders.Add(bombEfHolder);
        effectHolders.Add(juiceEfHolder);

        CreateEffects(hitEfHolder);
        CreateEffects(splashEfHolder);
        CreateEffects(bombEfHolder);
        CreateEffects(juiceEfHolder);
    }

    #region General Effect Pool
    private void CreateEffects(EffectHolder _holder)
    {
        for(int i = 0; i < 10; i++)
        {
            var effect = Instantiate(_holder.PrefabObj);
            effect.SetActive(false);
            effect.transform.SetParent(effectParent.transform);
            _holder.EffectList.Add(effect);
        }
    }
    private EffectHolder GetEffectHolder(EffectType _effectType)
    {
        foreach(var holder in effectHolders)
        {
            if(holder.EffectType == _effectType)
            {
                return holder;
            }
        }
        return null;
    }
    public GameObject GetEffect(EffectType _effectType)
    {
        var holder = GetEffectHolder(_effectType);

        foreach(var effect in holder.EffectList)
        {
            if(!effect.activeSelf)
                return effect;
        }

        var newEffect = Instantiate(holder.PrefabObj);
        newEffect.SetActive(false);
        newEffect.transform.SetParent(effectParent.transform);
        holder.EffectList.Add(newEffect);

        return newEffect;
    }
    #endregion

}
class EffectHolder{
    public EffectType EffectType { get; set; }
    public GameObject PrefabObj { get; set; }
    public List<GameObject> EffectList { get; set; }

    public EffectHolder(EffectType _effectType, GameObject _prefabObj, List<GameObject> _effectList)
    {
        EffectType = _effectType;
        PrefabObj = _prefabObj;
        EffectList = _effectList;
    }
}
public enum EffectType{
    HitEffect,
    SplashEffect,
    BombEffect,
    JuiceEffect
}
