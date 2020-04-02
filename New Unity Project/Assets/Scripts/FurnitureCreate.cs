using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class FurnitureCreate : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler, IPointerDownHandler
{

    public string childname = "77";
    private Vector3 screenSpace;
    private Vector3 offset;
    public static float boardWidth = ButtonManager.roomWidth, boardLength = ButtonManager.roomLength;
    private float thiswidth, thislength;
    private int state = ButtonManager.pm_state;
    // Start is called before the first frame update
    void Start()
    {
        transform.Find("77").gameObject.SetActive(false);
        transform.Find(childname).gameObject.SetActive(true);
        thiswidth = transform.Find(childname).gameObject.GetComponent<RectTransform>().rect.width;
        thislength = transform.Find(childname).gameObject.GetComponent<RectTransform>().rect.height;
    }

    // Update is called once per frame
    void Update()
    {
        state = ButtonManager.pm_state;
        if ((transform.localRotation.eulerAngles.z > 40 && transform.localRotation.eulerAngles.z < 130) || (transform.rotation.z > 220 && transform.rotation.z < 310))
        {
            if (transform.position.x > -2)
            {
                if (transform.position.x > (38.476 + (boardWidth * 10) - thislength / 10)) //6
                {
                    gameObject.transform.position = new Vector3((float)(38.476 + (boardWidth * 10) - (thislength / 10)),
                        transform.position.y, transform.position.z);
                }
                if (transform.position.x < (38.476 - (boardWidth * 10) + thislength / 10))
                {
                    gameObject.transform.position = new Vector3((float)(38.476 - (boardWidth * 10) + (thislength / 10)),
                        transform.position.y, transform.position.z);
                }
                if (transform.position.y > +boardLength * 10 - thiswidth / 10)
                {
                    gameObject.transform.position = new Vector3(transform.position.x,
                        boardLength * 10 - thiswidth / 10, transform.position.z);
                }
                if (transform.position.y < 2 - (boardLength * 10) + thiswidth / 10)
                {
                    gameObject.transform.position = new Vector3(transform.position.x,
                        2 - (boardLength * 10) + thiswidth / 10, transform.position.z);
                }
            }
        }
        else
        {
            if (transform.position.x > -2)
            {
                if (transform.position.x > (38.476 + (boardWidth * 10) - thiswidth / 10)) //6
                {
                    gameObject.transform.position = new Vector3((float)(38.476 + (boardWidth * 10) - (thiswidth / 10)),
                        transform.position.y, transform.position.z);
                }
                if (transform.position.x < (38.476 - (boardWidth * 10) + thiswidth / 10))
                {
                    gameObject.transform.position = new Vector3((float)(38.476 - (boardWidth * 10) + (thiswidth / 10)),
                        transform.position.y, transform.position.z);
                }
                if (transform.position.y > +boardLength * 10 - thislength / 10)
                {
                    gameObject.transform.position = new Vector3(transform.position.x,
                        boardLength * 10 - thislength / 10, transform.position.z);
                }
                if (transform.position.y < 2 - (boardLength * 10) + thislength / 10)
                {
                    gameObject.transform.position = new Vector3(transform.position.x,
                        2 - (boardLength * 10) + thislength / 10, transform.position.z);
                }
            }
        }

        if (transform.position.x < 2)
        {
            if (transform.position.x > 1 || transform.position.x < -70 || //4
                transform.position.y > 6 || transform.position.y < -42) //6
            {
                Destroy(gameObject);
            }
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (state == 0) transform.Rotate(new Vector3(0, 0, -10));
        else if (state == 1) transform.localScale *= 0.8f;
        else if (state == 2) transform.localScale *= 1.25f;
        screenSpace = Camera.main.WorldToScreenPoint(transform.position);
        offset = transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenSpace.z));

        //throw new System.NotImplementedException();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        var curScreenSpace = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenSpace.z);
        var curposition = Camera.main.ScreenToWorldPoint(curScreenSpace);
        transform.position = curposition;
        //throw new System.NotImplementedException();

    }

    public void OnDrag(PointerEventData eventData)
    {
        var curScreenSpace = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenSpace.z);
        var curposition = Camera.main.ScreenToWorldPoint(curScreenSpace);
        transform.position = curposition;
        //throw new System.NotImplementedException();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (state == 0) transform.Rotate(new Vector3(0, 0, 10));
        else if (state == 1) transform.localScale *= 1.25f;
        else if (state == 2) transform.localScale *= 0.8f;
        else if (state == 3)
        {
            Destroy(gameObject); ButtonManager.pm_state = 0;
        }
        //throw new System.NotImplementedException();
    }
}