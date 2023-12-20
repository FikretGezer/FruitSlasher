using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class EffectSpawner : MonoBehaviour
{
    [SerializeField] private GameObject hitEffectPrefab;
    [SerializeField] private GameObject splashEffectPrefab;
    [SerializeField] private GameObject bombEffectPrefab;
    [SerializeField] private GameObject juiceEffectPrefab;
    [SerializeField] private TMP_Text _tIndividual;
    [SerializeField] private TMP_Text _tTotal;
    [SerializeField] private TMP_Text _tCombo;

    private List<GameObject> effectsHit = new List<GameObject>();
    private List<GameObject> effectsSplash = new List<GameObject>();
    private List<GameObject> effectsBomb = new List<GameObject>();
    private List<GameObject> effectsJuice = new List<GameObject>();
    private List<EffectHolder> effectHolders = new List<EffectHolder>();

    private GameObject effectParent;
    private Camera cam;
    private Animator indAnim, totalAnim;

    private EffectHolder hitEfHolder, splashEfHolder, bombEfHolder, juiceEfHolder;

    public static EffectSpawner Instance;
    private void Awake() {
        if(Instance == null) Instance = this;

        cam = Camera.main;

        indAnim = _tIndividual.GetComponent<Animator>();
        totalAnim = _tTotal.GetComponent<Animator>();

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
        for(int i = 0; i < 50; i++)
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
    #region Text Effect
    private int hitCount = 0;
    public void GetTextEffect(Vector3 pos)
    {
        if(!_tIndividual.gameObject.activeInHierarchy)
        {
            _tTotal.gameObject.SetActive(false);
            _tIndividual.gameObject.SetActive(true);
        }
        hitCount++;
        _tIndividual.text = hitCount.ToString() + " HITS";
        indAnim.SetTrigger("indPop");
        _tIndividual.transform.position = cam.WorldToScreenPoint(pos);
        _tTotal.transform.position = cam.WorldToScreenPoint(pos);
    }
    public void SpawnBigText()
    {
        if(_tIndividual != null && _tIndividual.gameObject.activeInHierarchy)
        {
            _tIndividual.gameObject.SetActive(false);
            _tTotal.text = $"+{hitCount}";
            // _tTotal.gameObject.SetActive(true);
            UIUpdater.Instance.IncreaseScore(hitCount);
            ResetCount();
            StartCoroutine(nameof(ResetTextCor));
        }
    }

    private void ResetCount()
    {
        hitCount = 0;
    }
    IEnumerator ResetTextCor()
    {
        yield return new WaitForSeconds(0.2f);
        _tTotal.gameObject.SetActive(true);
        indAnim.SetTrigger("totalPop");

        yield return new WaitForSeconds(2f);
        _tTotal.gameObject.SetActive(false);
    }
    #endregion
    #region Combo Text Effect
    public void GetComboTextEffect(Vector3 pos, int count)
    {
        if(!_tCombo.gameObject.activeInHierarchy)
        {
            _tCombo.text = $"Combo x{count}";
            _tCombo.transform.position = pos;
            UIUpdater.Instance.IncreaseScore(3*count);
            StartCoroutine(ResetComboTextCor());
        }
    }

    IEnumerator ResetComboTextCor()
    {
        yield return new WaitForSeconds(0.2f);
        _tCombo.gameObject.SetActive(true);
        // indAnim.SetTrigger("totalPop");

        yield return new WaitForSeconds(2f);
        _tCombo.gameObject.SetActive(false);
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
