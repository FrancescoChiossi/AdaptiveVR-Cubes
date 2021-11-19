using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleporter : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (this.transform.position.y < 0 && this.transform.parent == null) {
            Debug.LogWarning("Teleport");
            Vector3 pos = this.transform.position;
            pos.y = 0.3f;
            this.transform.position = pos;
        }
    }
}
