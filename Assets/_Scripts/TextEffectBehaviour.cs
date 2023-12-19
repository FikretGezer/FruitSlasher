using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Runtime
{
    public class TextEffectBehaviour : MonoBehaviour
    {
        public void DisableTexts()
        {
            gameObject.transform.parent.gameObject.SetActive(false);
            this.gameObject.SetActive(false);
        }
    }
}
