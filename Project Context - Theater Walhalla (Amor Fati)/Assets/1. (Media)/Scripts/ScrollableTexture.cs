using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollableTexture : MonoBehaviour
{
    [SerializeField] private Material material;

    private IEnumerator Start()
    {
        //material = GetComponent<Renderer>().material;
        Debug.Log(material.mainTextureOffset.y);

        while (true)
        {

            material.mainTextureOffset = new Vector2(0f, 0.21f) * Time.deltaTime;
            yield return null;
        }

        yield return null;
    }

    private void Update()
    {
        //poep.mainTextureOffset = new Vector2(0f,0.21f) * Time.deltaTime;
        //Debug.Log(poep.mainTextureOffset);
       
       // material.mainTextureOffset = new Vector2(0, material.mainTextureOffset.y + 1);
    }
}
