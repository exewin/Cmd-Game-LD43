using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Game1 : MonoBehaviour 
{

	int totalTime = 0;
	
	public System32 sys;
	public GameObject shellScreen;
	
	public Text totalTimeText;
	
	int s1=-1;
	int s2=-1;
	
	bool turn=false;
	
	public GameObject[] fv;
	public GameObject[] sv;
	public int[] times;
	

	void updateTime(int time)
	{
		totalTime+=time;
		totalTimeText.text = "Total time: "+totalTime+" minutes";
	}
	
	void findMax()
	{
		updateTime(Mathf.Max(times[s1],times[s2]));
	}
	
	
	public void send(int type) 
	{
		if(turn)
			return;
		for(int i = 0;i<4;i++)
		{
			if(type==i)
			{
				sets(i);
			}
		}
	}
	
	public void deIni(int type) 
	{
		if(!turn)
			return;
		for(int i = 0;i<4;i++)
		{
			if(type==i)
			{
				sv[i].SetActive(false);
				fv[i].SetActive(true);
				updateTime(times[i]);
				turn=false;
			}
		}
	}
	
	void sets(int t)
	{
		if(s1==-1)
			s1=t;
		else
		{
			s2=t;
			findMax();
			fv[s1].SetActive(false);
			fv[s2].SetActive(false);
			sv[s1].SetActive(true);
			sv[s2].SetActive(true);
			turn=true;
			s1=-1;
			s2=-1;
		}
		
	}
	
	public void reset()
	{
		for(int i = 0;i<4;i++)
		{
			fv[i].SetActive(true);
			sv[i].SetActive(false);
		}
		totalTime=0;
		updateTime(0);
		s1=-1;
		s2=-1;
		turn=false;
	}
	
	public void confirm()
	{
		for(int i = 0;i<4;i++)
		{
			if(sv[i].activeSelf==false)
				return;
		}
		gameObject.SetActive(false);
		shellScreen.SetActive(true);
		sys.takeGame1Result(totalTime);
		
	}

}