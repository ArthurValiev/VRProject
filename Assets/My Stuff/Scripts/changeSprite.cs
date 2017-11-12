using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;
using System;
using System.Collections.Generic;

public class changeSprite : MonoBehaviour
{
    public Sprite[] spr;
	private bool estLiKys = false;
	public static bool dragKys = false;
    private GameObject kysok;
    private GameObject tekyw;
    private int nomer = -1;
    private int nomerBel;
    GameObject everything;
    GameObject video;

    private int layerMask = 1 << 8;
	private List<int> usedPuzz = new List<int>();

    void Awake()
    {
        spr = Resources.LoadAll<Sprite>("pieces/");
    }

    public void On_Click_Button()
    {
        
		if (estLiKys == false) //если др. картинка в дан. момент не отображается
        { 
            //выставляем картинку на кусочек
            GameObject o = EventSystem.current.currentSelectedGameObject;
            string str = o.name;
            int.TryParse(str, out nomer);

			if (nomer - 1 <= spr.Length && !usedPuzz.Contains(nomer)) 
            {
                Vector3 pos = GameObject.Find("CanvasMenu").transform.position;
				pos.y -= 2f;
                pos.z += 2f;

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
				kysok.GetComponent<SpriteRenderer>().size = new Vector3(4, 4);

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
            }
        }
    }

    // Use this for initialization
    void Start()
    {
        everything = GameObject.Find("Everything");

    }

    void Update()
    {
        
        Vector3 raycastDir = Vector3.zero;
        RaycastHit hit;
        Ray landingRay = new Ray();

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

				if (usedPuzz.Count == spr.Length) {
					Debug.Log ("You are win!");// сюда вставляем показ видео / переход к новой сцене
                    everything.SetActive(false);
					GameObject video0 = GameObject.Find("Video");
					video = video0.transform.Find("VideoSphere").gameObject;
                    video.SetActive(true);
					video.GetComponent<UnityEngine.Video.VideoPlayer>().Play();
				}
            }

            GameObject ob = EventSystem.current.currentSelectedGameObject;
        }

    }


	public void On_Click_Kysok()
	{
		dragKys = !dragKys;
	}
}

