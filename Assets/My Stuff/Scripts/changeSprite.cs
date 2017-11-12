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
	private int nomerBel2 = 1;
    GameObject everything;
    GameObject video;
	GameObject o;
	Vector3 pos;

    private int layerMask = 1 << 8;
	private List<int> usedPuzz = new List<int>();

    void Awake()
    {
        spr = Resources.LoadAll<Sprite>("pieces/");
    }

    public void On_Click_Button()
    {
		o = EventSystem.current.currentSelectedGameObject; 
		if (estLiKys == false) //если др. картинка в дан. момент не отображается
        { 
            //выставляем картинку на кусочек
            string str = o.name;
            int.TryParse(str, out nomer);

			if (nomer - 1 <= spr.Length && !usedPuzz.Contains(nomer)) 
            {
                pos = GameObject.Find("CanvasMenu").transform.position;
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
			
            if (tekyw.name == o.name)
            {
				
				Destroy (kysok);

                Color myCol = new Color(1F, 1F, 1F, 1F); //возвращаем цвет инвентарю
                var button = tekyw.GetComponent<Button>(); //o
                var colors = button.colors;
                colors.normalColor = myCol;
                tekyw.GetComponent<Button>().colors = colors;
				//EventSystem.current.currentSelectedGameObject.GetComponent<Button>().colors = colors;


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
		if (dragKys) {

			Vector3 raycastDir = Vector3.zero;
			RaycastHit hit;
			Ray landingRay = new Ray ();

			raycastDir = kysok.transform.TransformDirection (Vector3.back);
			landingRay = new Ray (kysok.transform.position, raycastDir);

			if (Physics.Raycast (landingRay, out hit, 1000, layerMask)) {
            
				string str = hit.transform.name;
				int.TryParse (str, out nomerBel);

				if ((nomerBel == nomer) && dragKys) {
                
					hit.transform.gameObject.SetActive (false);

					Destroy (kysok);
					estLiKys = false;
					dragKys = false;
					//Debug.Log ("удаление от белой" + dragKys);
					usedPuzz.Add (nomer); 

					Color myCol2 = new Color (1F, 1F, 1F, 0F);
					var button2 = tekyw.GetComponent<Button> ();
					var colors2 = button2.colors;
					colors2.normalColor = myCol2;
					tekyw.GetComponent<Button> ().colors = colors2;

					if (usedPuzz.Count == spr.Length) {
						Debug.Log ("You are win!");// сюда вставляем показ видео / переход к новой сцене
						everything.SetActive (false);
						GameObject video0 = GameObject.Find ("Video");
						video = video0.transform.Find ("VideoSphere").gameObject;
						video.SetActive (true);
						video.GetComponent<UnityEngine.Video.VideoPlayer> ().Play ();
					}
				}
				
				//GameObject ob = EventSystem.current.currentSelectedGameObject;
			}
		}

		Debug.DrawRay (Camera.main.transform.position, Camera.main.transform.forward, Color.magenta);

		Ray camRay = new Ray(Camera.main.transform.position, Camera.main.transform.forward); 
		RaycastHit hit2;
		GameObject menu = GameObject.Find ("CanvasMenu");

		if (Physics.Raycast (camRay, out hit2, 1000, layerMask)) {

			string str2 = hit2.transform.name;
			int.TryParse(str2, out nomerBel2);
			Debug.Log ("люб стена");
		}

		if (nomerBel2 < 10) {
			menu.transform.position = new Vector3 (0, 2f, -0.5f);
			//menu.transform.Rotate(60f, 0, 0);
			menu.transform.rotation = Quaternion.Euler(menu.transform.rotation.eulerAngles.x, 0, menu.transform.rotation.eulerAngles.z);
			Debug.Log ("stena 1, hit=" + nomerBel2);
		}

		if (nomerBel2 < 28 && nomerBel2 > 18) {
			menu.transform.position = new Vector3 (-0.5f, 2f, 0);
			//menu.transform.Rotate(60f, 180f, 0);
			menu.transform.rotation = Quaternion.Euler (60, 90, menu.transform.rotation.eulerAngles.z);
		}

		if (nomerBel2 < 19 && nomerBel2 > 9) {
			menu.transform.position = new Vector3 (0.5f, 2f, 0);
			//menu.transform.Rotate(60f, 180f, 0);
			menu.transform.rotation = Quaternion.Euler (60, -90, menu.transform.rotation.eulerAngles.z);
		}

		if (nomerBel2 > 27) {
			menu.transform.position = new Vector3 (0, 2f, 0.5f);
			//menu.transform.Rotate(60f, 180f, 0);
			menu.transform.rotation = Quaternion.Euler(menu.transform.rotation.eulerAngles.x, 180, menu.transform.rotation.eulerAngles.z);
			Debug.Log ("stena 2, hit=" + nomerBel2);
		}
		/*float smooth = 2.0F;
		float tiltAngle = 30.0F;
		float tiltAroundZ = Input.GetAxis("Horizontal") * tiltAngle;
		float tiltAroundX = Input.GetAxis("Vertical") * tiltAngle;
		Quaternion target = Quaternion.Euler(tiltAroundX, 0, tiltAroundZ);
		transform.rotation = Quaternion.Slerp(transform.rotation, target, Time.deltaTime * smooth);*/

    }


	public void On_Click_Kysok()
	{
		dragKys = !dragKys;
	}
}

