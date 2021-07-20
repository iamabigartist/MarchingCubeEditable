using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Billboard : MonoBehaviour {

    Text text;

	void Awake () {

        text = GetComponentInChildren<Text>();

    }
	
	void Update () {

        transform.LookAt(Camera.main.transform);
        transform.forward *= -1;
	}
    
    public void SetText(string _text)
    {
        text.text = _text;
    }
    public void SetText(float scale)
    {
        text.rectTransform.localScale = Vector3.one * scale;
    }
    public void SetColor(Color _color)
    {
        text.color = _color;
    }
}
