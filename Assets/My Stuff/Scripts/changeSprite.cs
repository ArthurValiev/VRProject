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
    GameObject everything;
    GameObject video;


    //private int layerMask = 1 << 8;
    private int layerMask = 1 << 8; //0 | 1 << 1 | 1 << 2 | 1 << 3 | 1 << 4 | 1 << 5;
	private List<int> usedPuzz = new List<int>();





    void Awake()
    {
        spr = Resources.LoadAll<Sprite>("pieces/");

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

                for (int i = 0; i < spr.Length; i++)
                {
                    for (int j = 0; j < spr.Length - 1; j++)
                    {
                        int ima1, ima2;
                        string st1 = spr[j].name;
                        int.TryParse(st1, out ima1);
                        string st2 = spr[j + 1].name;
                        int.TryParse(st2, out ima2);
                        if (ima1 > ima2)
                        {
                            Sprite vrem = spr[j];
                            spr[j] = spr[j + 1];
                            spr[j + 1] = vrem;
                        }
                    }
                }

				kysok.GetComponent<SpriteRenderer>().sprite = spr[nomer - 1];
                //kysok.GetComponent<SpriteRenderer>().size = new Vector3(16.93333f, 8.466667f);
				kysok.GetComponent<SpriteRenderer>().size = new Vector3(4, 4);

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
				//Debug.Log ("нажатие на инвентарь" + dragKys);
            }
        }
    }

    // Use this for initialization
    void Start()
    {
        //layerMask = ~layerMask;
        everything = GameObject.Find("Everything");
        //video = GameObject.Find("VideoSphere");

        //video.SetActive(true);
        //video.GetComponent<UnityEngine.Video.VideoPlayer>().Play();

    }

    // Update is called once per frame
    void Update()
    {
        
        Vector3 raycastDir = Vector3.zero;
        RaycastHit hit;
        Ray landingRay = new Ray();

        //GameObject reticle = this.gameObject.transform.GetChild(0).gameObject;

        raycastDir = kysok.transform.TransformDirection(Vector3.back);
        landingRay = new Ray(kysok.transform.position, raycastDir);

        if (Physics.Raycast(landingRay, out hit, 1000, layerMask))
        {
            
            string str = hit.transform.name;
            int.TryParse(str, out nomerBel);

            if ((nomerBel == nomer) && dragKys)
            {
                
                hit.transform.gameObject.SetActive(false);

				Destroy (kysok);
				estLiKys = false;
				dragKys = false;
				//Debug.Log ("удаление от белой" + dragKys);
				usedPuzz.Add(nomer); 

				if (usedPuzz.Count == 2/*spr.Length*/) {
					Debug.Log ("You are win!");// сюда вставляем показ видео / переход к новой сцене
                    everything.SetActive(false);
					GameObject video0 = GameObject.Find("Video");
					GameObject video = video0.transform.Find("VideoSphere").gameObject;
                    video.SetActive(true);
                    //video.GetComponent<VideoPlayerReference>().player.Play();
					video.GetComponent<UnityEngine.Video.VideoPlayer>().Play();
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
		//Debug.Log ("живой клик" + dragKys);
	}
}

