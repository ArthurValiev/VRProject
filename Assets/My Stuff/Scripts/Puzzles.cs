using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puzzles : MonoBehaviour {

    Dictionary<int, Sprite> sprites = new Dictionary<int, Sprite>();
    Sprite[] numberSprites;


    void Start()
    {
        
        numberSprites = Resources.LoadAll<Sprite>("Streetview");
        for (int i = 0; i < numberSprites.Length; i++)
        {
            sprites.Add(i + 1, numberSprites[i]);
        }

        //foreach (KeyValuePair<int, Sprite> kvp in sprites)
        //{
        //    Debug.Log("Key = " + kvp.Key + " Value = " + kvp.Value);
        //}


        //GameObject sprite = GameObject.Find("StreetView_0");

        //sprite.transform.localScale -= new Vector3(1, 0, 0);


    }

	
	// Update is called once per frame
	void Update () {
        
	}
}
