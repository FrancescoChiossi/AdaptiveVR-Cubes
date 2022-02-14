using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cubecolor : MonoBehaviour
{
    public Material[] materials;
    public Material selectedColor;
    public string cubecolor;
    // Start is called before the first frame update
    void Start()
    {
        reset();
    }

    // Update is called once per frame
    void Update()
    {
        //transform.Rotate(new Vector3(0, 90 * Time.deltaTime, 0));
    }

    /*public void setSelected()
    {
        MeshRenderer renderer = GetComponent<MeshRenderer>();

        Material[] mats = renderer.materials;
        mats[0] = selectedColor;
        renderer.materials = mats;
    }*/


    public void reset()
    {
        if (materials.Length > 0)
        {
            Material m = materials[Random.Range(0, materials.Length)];
            cubecolor = m.name;
            MeshRenderer renderer = GetComponent<MeshRenderer>();

            Material[] mats = renderer.materials;
            mats[0] = m;
            renderer.materials = mats;
        }
        else
        {
            Debug.Log("No materials for shirts set.");
        }
    }
}
