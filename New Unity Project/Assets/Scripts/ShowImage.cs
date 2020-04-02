using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ShowImage : MonoBehaviour
{
    // Start is called before the first frame update
    Texture2D text;
    new string name;

    void Start()
    {
        name = gameObject.name;
        text = Resources.Load(name, typeof(Texture2D)) as Texture2D;
        gameObject.GetComponent<RawImage>().texture = text;

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
