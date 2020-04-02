using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class SceneChange : MonoBehaviour
{
    public GameObject Panel;
    public InputField input_v, input_h;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void btnClicked()
    {
        if (int.Parse(input_v.text) > 6 || int.Parse(input_h.text) > 4)
            Panel.SetActive(true);
        else
        {
            ButtonManager.roomWidth = int.Parse(input_h.text);
            ButtonManager.roomLength = int.Parse(input_v.text);
            SceneManager.LoadScene(0);
        }
       
        
        //Debug.Log(input_v.text + " " + input_h.text);
        //Debug.Log(int.Parse(input_v.text));
    }
    public void btnPanelClicked()
    {
        Panel.SetActive(false);
    }
}
