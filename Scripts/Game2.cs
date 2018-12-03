using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game2 : MonoBehaviour 
{

	public int[] fields = new int[16];
	public Vector2 pos = new Vector2(0,0);
	
	public GameObject bonzi;
	
	public System32 sys;
	public GameObject shellScreen;
	
	int result=1;
	
	public void reset()
	{
		result=1;
		for(var i = 0;i<16;i++)
		{
			fields[i]=0;
		}
		pos = new Vector2(0,0);
		bonzi.transform.localPosition = new Vector2(-42+pos.x*29,42-pos.y*29);
	}
	
	
	void Update()
	{
		if(Input.GetKeyDown(KeyCode.LeftArrow))
			move(new Vector2(-1,0));
		else if(Input.GetKeyDown(KeyCode.RightArrow))
			move(new Vector2(1,0));
		else if(Input.GetKeyDown(KeyCode.UpArrow))
			move(new Vector2(0,-1));
		else if(Input.GetKeyDown(KeyCode.DownArrow))
			move(new Vector2(0,1));
	}
	
	void move(Vector2 t)
	{
		//exit
		if(pos.x==3&&pos.y==3&&t.x==1)
		{
			pos+=t;Debug.Log(pos);
			bonzi.transform.localPosition = new Vector2(-42+pos.x*29,42-pos.y*29);
			result++;
		}
		else if(pos.x+t.x>=0&&pos.x+t.x<4&&pos.x!=4)
			if(pos.y+t.y>=0&&pos.y+t.y<4)
				if(fields[(int)((pos.x+t.x)+(pos.y+t.y)*4)]==0)
				{
					if((int)(pos.x+pos.y*4)!=0)
					{
						fields[(int)(pos.x+pos.y*4)]=1;
						result++;
					}
					pos+=t;Debug.Log(pos);
					bonzi.transform.localPosition = new Vector2(-42+pos.x*29,42-pos.y*29);
				}
			
	}
	
	public void confirm()
	{
		Debug.Log(result);
		gameObject.SetActive(false);
		shellScreen.SetActive(true);
		sys.takeGame2Result(result);
	}
	
	

	
}
