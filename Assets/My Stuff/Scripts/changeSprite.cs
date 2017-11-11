using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;
using System;
using System.Collections.Generic;

public class changeSprite : MonoBehaviour
{
    public Sprite[] spr;
	//public GameObject prehab;
	private bool estLiKys = false;
	public static bool dragKys = false;
	//private int kysClick = 0;
    //public GameObject kysok;
    //private int count = 0;
    private GameObject kysok;
    private GameObject tekyw;
    //private GameObject belyi;
    private int nomer = -1;
    private int nomerBel;

    //private int layerMask = 1 << 8;
    private int layerMask = 1 << 8; //0 | 1 << 1 | 1 << 2 | 1 << 3 | 1 << 4 | 1 << 5;
	private List<int> usedPuzz = new List<int>();





    void Awake()
    {
        spr = Resources.LoadAll<Sprite>("sprites");
        //GameObject kysok = GameObject.Find("piece");

    }

    public void On_Click_Button()
    {
        //GameObject pl = GameObject.Find("Player");
        //kysok = pl.transform.Find("piece").gameObject;

        //Debug.Log(kysok.name);

		if (estLiKys == false) //если др. картинка в дан. момент не отображается
        { 


            //выставляем картинку на кусочек
            GameObject o = EventSystem.current.currentSelectedGameObject;
            string str = o.name;
            int.TryParse(str, out nomer);
            //Debug.Log(nomer);

			if (nomer - 1 <= spr.Length && !usedPuzz.Contains(nomer)) 
            {
				Vector3 pos = GameObject.Find("Reticle").transform.position;
				pos.y += 1f;

				GameObject pl = GameObject.Find("Player");
				GameObject pi0 = pl.transform.Find("piece0").gameObject;
				Quaternion rot = pi0.transform.rotation;

				kysok = Instantiate<GameObject>(pi0, pos, rot);
			

				kysok.GetComponent<SpriteRenderer>().sprite = spr[nomer - 1];
                //kysok.GetComponent<SpriteRenderer>().size = new Vector3(16.93333f, 8.466667f);
				kysok.GetComponent<SpriteRenderer>().size = new Vector3(8.93333f, 4.466667f);

				//kysok.GetComponent<SpriteRenderer>().flipX = true;


            	//высветляем кусочек из инвентаря
            	Color myCol = new Color(1F, 1F, 1F, 0.5F);
            	var button = o.GetComponent<Button>();
            	var colors = button.colors;
            	colors.normalColor = myCol;
            	EventSystem.current.currentSelectedGameObject.GetComponent<Button>().colors = colors;

            	tekyw = o; //запоминает какая картинка стоит
				estLiKys = true;
			}

        }
        else
        { //на второй щелчок

            GameObject o = EventSystem.current.currentSelectedGameObject;
            //Debug.Log ("o=" + o.name + "tekyw=" + tekyw.name);

            if (tekyw.name == o.name)
            {
				
				Destroy (kysok);

                Color myCol = new Color(1F, 1F, 1F, 1F); //возвращаем цвет инвентарю
                var button = o.GetComponent<Button>();
                var colors = button.colors;
                colors.normalColor = myCol;
                EventSystem.current.currentSelectedGameObject.GetComponent<Button>().colors = colors;

				estLiKys = false;
				dragKys = false;
				Debug.Log ("нажатие на инвентарь" + dragKys);
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

			if (nomerBel == nomer && dragKys)
            { 
                hit.transform.gameObject.SetActive(false);
				Destroy (kysok);
				estLiKys = false;
				dragKys = false;
				Debug.Log ("удаление от белой" + dragKys);
				usedPuzz.Add(nomer); 

				if (usedPuzz.Count == spr.Length) {
					Debug.Log ("You are win!"); // сюда вставляем показ видео / переход к новой сцене
				}
            }

			GameObject ob = EventSystem.current.currentSelectedGameObject;
			//Debug.Log (ob.name);


        }
        /*else
        {
            Debug.DrawRay(laser.transform.position, raycastDir * 1000, Color.red);
            //Debug.Log(hit.transform.name);
        }*/
    }


	public void On_Click_Kysok()
	{
		dragKys = !dragKys;
		Debug.Log ("живой клик" + dragKys);
	}
}

