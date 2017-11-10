using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;
using System;

public class changeSprite : MonoBehaviour
{
    public Sprite[] spr;
    //public GameObject kysok;
    //private int count = 0;
    //private GameObject kysok;
    private GameObject tekyw;
    //private GameObject belyi;
    int nomer = -1;
    int nomerBel;

    //private int layerMask = 1 << 8;
    private int layerMask = 1 << 8; //0 | 1 << 1 | 1 << 2 | 1 << 3 | 1 << 4 | 1 << 5;




    void Awake()
    {
        spr = Resources.LoadAll<Sprite>("sprites");
        //GameObject kysok = GameObject.Find("piece");

    }

    public void On_Click_Button()
    {
        GameObject pl = GameObject.Find("Player");
        GameObject kysok = pl.transform.Find("piece").gameObject;

        Debug.Log(kysok.name);

        if (kysok.activeSelf == false)
        { //если др. картинка в дан. момент не отображается

            kysok.SetActive(true);

            /*Vector3 pos = GameObject.Find("CanvasMenu").transform.position;
            pos.y += 1f;
            kysok.transform.position = pos;*/

            Vector3 pos = GameObject.Find("Reticle").transform.position;
            pos.y += 1f;
            kysok.transform.position = pos;


            //выставляем картинку на кусочек
            GameObject o = EventSystem.current.currentSelectedGameObject;
            string str = o.name;
            int.TryParse(str, out nomer);
            Debug.Log(nomer);

            if (nomer - 1 <= spr.Length)
            {
                kysok.GetComponent<SpriteRenderer>().sprite = spr[nomer - 1];
                kysok.GetComponent<SpriteRenderer>().size = new Vector3(16.93333f, 8.466667f);
            }
            else
            {
                Debug.Log("wrong nomer");
            }

            //высветляем кусочек из инвентаря
            Color myCol = new Color(1F, 1F, 1F, 0.5F);
            var button = o.GetComponent<Button>();
            var colors = button.colors;
            colors.normalColor = myCol;
            EventSystem.current.currentSelectedGameObject.GetComponent<Button>().colors = colors;

            tekyw = o; //запоминает какая картинка стоит

        }
        else
        { //на второй щелчок

            GameObject o = EventSystem.current.currentSelectedGameObject;
            //Debug.Log ("o=" + o.name + "tekyw=" + tekyw.name);

            if (kysok.activeSelf == true && tekyw.name == o.name)
            {


                kysok.SetActive(false); //убираем кусочек


                Color myCol = new Color(1F, 1F, 1F, 1F); //возвращаем цвет инвентарю
                var button = o.GetComponent<Button>();
                var colors = button.colors;
                colors.normalColor = myCol;
                EventSystem.current.currentSelectedGameObject.GetComponent<Button>().colors = colors;

            }
        }
    }

    // Use this for initialization
    void Start()
    {
        //layerMask = ~layerMask;

    }

    // Update is called once per frame
    void Update()
    {
        GameObject laser = GameObject.Find("Laser");
        GameObject point = GameObject.Find("Reticle");


        RaycastHit hit;

        //GameObject reticle = this.gameObject.transform.GetChild(0).gameObject;

        Vector3 raycastDir = point.transform.position - laser.transform.position;

        Ray landingRay = new Ray(laser.transform.position, raycastDir);


        if (Physics.Raycast(landingRay, out hit, 1000, layerMask))
        {
            //Debug.DrawRay(laser.transform.position, raycastDir * 1000, Color.white);
            //Debug.Log(hit.transform.name);

            string str = hit.transform.name;
            int.TryParse(str, out nomerBel);

            //Debug.Log("nomer = " + nomer + ", nomerBel = " + nomerBel);

            if (nomerBel == nomer)
            { //-delau
                hit.transform.gameObject.SetActive(false);
            }
        }
        /*else
        {
            Debug.DrawRay(laser.transform.position, raycastDir * 1000, Color.red);
            //Debug.Log(hit.transform.name);
        }*/
    }
}

