using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Runtime
{
    public class BladeUI : MonoBehaviour
    {

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            var origin = Camera.main.ScreenPointToRay(Input.mousePosition).origin;
            RaycastHit2D hit = Physics2D.Raycast(origin, Vector3.forward);
            if(hit.collider != null)
            {
                Debug.Log(hit.collider.name);
            }
        }
    }
}
