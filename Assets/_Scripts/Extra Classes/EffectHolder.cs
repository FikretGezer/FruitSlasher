using System.Collections.Generic;
using UnityEngine;

namespace Runtime
{
    public class EffectHolder{
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
}
