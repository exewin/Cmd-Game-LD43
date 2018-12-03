using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tooltip : MonoBehaviour 
{

	public Text Ttext;


	public void c(string t)
	{
		Ttext.text=t;
	}
}
