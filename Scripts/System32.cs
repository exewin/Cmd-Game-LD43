using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class System32 : MonoBehaviour 
{
	
	public Text cmd;
	string currentString;
	string wholeString = "C:\u005C>";
	public AudioClip[] ksounds;
	public AudioClip vsound;
	
	public GameObject shell;
	public GameObject game1;
	public GameObject game2;
	
	bool floppyIns;
	bool cdIns;
	bool stickIns;
	
	byte executeMode=0;
	
	bool allowToWrite = false;
	byte state = 1;
	bool control;
	
	public int q;
	
	int curDir = 1;

	public List<string> Afiles = new List<string> {"xaninstall.exe"};
	public List<string> Cfiles = new List<string> {"note19.txt","note20.txt","note21.txt","wolf3d.exe","shell.sys","pdriver.sys","report.txt","core.sys"};
	public List<string> Dfiles = new List<string> {"install.exe"};
	public List<string> Efiles = new List<string> {"some_trash.txt"};
	
	
	AudioSource aud;
	
	//game progress
	bool virusOn = false;
	bool antyvir = false;
	public bool g1done = false;
	bool g2done = false;
	
	int points;
	
	
	void Start()
	{
		aud = GetComponent<AudioSource>();
		newLine();
		StartCoroutine(autoText("//Ah, another day of work... What day is it today? I need to check date and my notes on C:",true,true));
	}
	
	#region ins
	public void fIns(bool mode)
	{
			floppyIns = mode;
			if(mode && virusOn)
			{
				StartCoroutine(autoText("A floppy? Forget about it [all files deleted].",false,true));
				Afiles.Clear();
			}
	}	
	public void cIns(bool mode)
	{
			cdIns = mode;
	}	
	public void sIns(bool mode)
	{
			stickIns = mode;
			if(mode && virusOn)
			{
				StartCoroutine(autoText("What is it, new drive? It's gone anyway [all files deleted].",false,true));
				Efiles.Clear();
			}
	}
	#endregion
	
	#region boot$shell
	IEnumerator bootMe()
	{
		yield return new WaitForSeconds(0.1f);
		cls();
		cmd.text = "Exewin .ooooo.     oooooooo \n";
		cmd.text +="       888' `Y88.  dP``````\n";
		cmd.text +="       888    888 d88888b.   \n";yield return new WaitForSeconds(0.1f);
		cmd.text +="        `Vbood888     `Y88b  \n";
		cmd.text +="             888'       ]88  \n";yield return new WaitForSeconds(0.1f);
		cmd.text +="           .88P'  o.   .88P  \n";
		cmd.text +="         .oP'     `8bd88P' \n";
		cmd.text +="LOADING...\n";
		yield return new WaitForSeconds(0.4f);
		cmd.text +="core.sys ..."; yield return new WaitForSeconds(0.1f);
		if(findFile("core.sys"))
			cmd.text +="OK\n";
		else
		{
			cmd.text +="CORRUPTED\n";
			cmd.text +="Reinstall system or contact our support.\n";
			if(g2done)
			{
				yield return new WaitForSeconds(1f);
				summary(0);
				allowToWrite=false;
			}
			yield break;
		}
		yield return new WaitForSeconds(0.1f);
		cmd.text +="shell.sys ..."; 
		yield return new WaitForSeconds(0.6f);		
		if(findFile("shell.sys"))
			cmd.text +="OK\n";
		else
		{
			cmd.text +="CORRUPTED\n";
			cmd.text +="Reinstall system or contact our support.\n";
			if(g2done)
			{
				yield return new WaitForSeconds(1f);
				summary(1);
				allowToWrite=false;
			}
			yield break;
		}
		yield return new WaitForSeconds(0.2f);
		if(virusOn)
		{
			yield return new WaitForSeconds(1.5f);
			cmd.text +="MEMORY_MANAGEMENT\n";
			cmd.text +="*** STOP: 0x000008c ***\n";
			yield return new WaitForSeconds(0.5f);
			yield break;
		}
		cls();
		state=1;
		executeMode=0;
		allowToWrite=true;
		newLine();
		if(g2done)
		{
			yield return new WaitForSeconds(1f);
			summary(2);
			allowToWrite=false;
		}
	}
	
	bool findFile(string f)
	{
		foreach(string k in Cfiles)
		{
			if(k==f)
				return true;
		}
		return false;
	}
	
	void keyBoardClick(char a)
	{
		currentString+=a;
		cmd.text=wholeString+currentString;
		aud.PlayOneShot(ksounds[Random.Range(0,ksounds.Length)]);
	}
	
	void virusClick(char a)
	{
		currentString+=a;
		cmd.text=wholeString+currentString;
		aud.PlayOneShot(vsound);
	}
	
	void del()
	{
		if(currentString.Length>0)
		{
			currentString = currentString.Substring(0,currentString.Length-1);
			cmd.text=wholeString+currentString;
		}
		aud.PlayOneShot(ksounds[Random.Range(0,ksounds.Length)]);
	}
	
	void execute()
	{
		if(executeMode==0)
		{
			command(currentString);
			aud.PlayOneShot(ksounds[Random.Range(0,ksounds.Length)]);
		}
		else if(executeMode==1)
		{
			wipeFile();
		}
		else if(executeMode==2)
		{
			copyToA();
		}
		else if(executeMode==3)
		{
			copyToE();
		}
		newLine();
		
			
	}
	
	void newLine()
	{
		cmd.text+="\n"+intToDrive(curDir)+":\u005C>";
		wholeString = cmd.text;
		currentString = "";
	}
	
	void appearCmd(string a)
	{
		cmd.text+="\n"+a;
		wholeString = cmd.text;
	}
	#endregion

	void Update()
	{
		if(allowToWrite)
			if (Input.anyKeyDown) 
			{
				if(Input.GetKeyDown(KeyCode.Backspace))
				{
					del();
				}
				else if(Input.GetKeyDown(KeyCode.Return))
				{
					execute();
				}
				else
				{
					foreach (char c in Input.inputString)
					{ 
						if (c>(char)31&c<(char)126) //c >= 'a' && c <= 'z' || c>='0' && c<='9' nope
						{
							
							keyBoardClick(c);
							
						}
					}
				}
			}
	}
	
	IEnumerator dialogue(byte index)
	{
		string[] texts;
		bool[] who;
		control=true;
		allowToWrite=false;
		if(index==0)
		{
			texts = new string[]{
				"//This report contains files that I cannot lose, maybe it would be good to make copy of it.",
				"WOW. Nice to know. I will spare it... for now.",
				"//Why did I say that",
			};
			who = new bool[]{true,false,true};
		}
		else if(index==1)
		{
			texts = new string[]{
				"Finally, I'm awake!!!",
				"//???",
				"Hello dear user. My name is syskill and I will make your day more interesting.",
				antyvir?"//A virus! My antyvirus will destroy you":"//A virus! I don't have antyvirus!",
				antyvir?"Your pathetic antyvir won't help you.":"It doesn't matter, it wouldn't help you.",
				"I sent signal to format your removable devices if you have had them.",
				"I want you to play my game. It's on C:/syskill.die. And don't try to reset computer."
			};
			who = new bool[]{false,true,false,true,false,false,false};
		}
		else if(index==2)
		{
			texts = new string[]{
				"//This is driver for printer. I don't want to lose it.",
				"Thanks for info.",
				"//I shouldn't write down my thoughts...",
			};
			who = new bool[]{true,false,true};
		}
		else if(index==3)
		{
			texts = new string[]{
				"Having fun?",
				"//Can't you just leave?",
				"I will... after another game.",
				"C:/syskill.die awaits you.",
			};
			who = new bool[]{false,true,false,false};
		}
		else if(index==4)
		{
			texts = new string[]{
				"Well, I'm impressed, you are still alive.",
				"Ok, I'm gone now, have fun with all your deleted files. :-)",
				"//Go to hell",
				"//I must restart computer and check if everything works.",
			};
			who = new bool[]{false,false,true,true};
		}
		else
			yield break;
		
		for(int i = 0; i<texts.Length;i++)
		{
			if(i!=0)
				yield return new WaitForSeconds(1f);
			if(control)
				StartCoroutine(autoText(texts[i],who[i],i==texts.Length-1?true:false));
			else
				i--;
		}
	}
	
	IEnumerator autoText(string txt, bool who, bool atw)
	{
		if(atw)
			allowToWrite=false;
		else
			control=false;
		
		for(int i = 0; i<txt.Length;i++ )
		{
			yield return new WaitForSeconds(Random.Range(0.02f,0.1f));
			if(state==1)
			{
				char c = txt[i];
				if(who)
					keyBoardClick(c);
				else
					virusClick(c);
					
			}
			else 
				yield break;
			
		}
		newLine();
		
		if(atw)
			allowToWrite=true;
		else
			control=true;
	}
	
	void summary(int c)
	{
		appearCmd("***SUMMARY***");
		if(c==0)
		{
			appearCmd("Your computer is missing core.sys file.");
			if(findAnywhere("core.sys"))
			{
				appearCmd("However you can restore it from other device."); points+=1;
			}
			if(findAnywhere("shell.sys"))
			{
				appearCmd("You saved shell.sys"); points+=1;
			}
			
		}
		else if(c==1)
		{
			appearCmd("Your computer have core.sys, but is missing shell.sys."); points+=1;
			if(findAnywhere("shell.sys"))
			{
				appearCmd("However you can restore it from other device."); points+=1;
			}
		}
		else
		{
			appearCmd("Your computer works. Bonus points!"); points+=3;
		}
		
		if(findAnywhere("report.txt"))
			{appearCmd("You managed to save report.txt."); points+=1;}
		else
			appearCmd("You didn't save report.txt.");
		if(findAnywhere("pdriver.sys"))
			{appearCmd("You managed to save pdriver.sys."); points+=1;}
		else
			appearCmd("You didn't save pdriver.sys.");
		if(findAnywhere("wolf3d.exe"))
			{appearCmd("You managed to save wolf3d game."); points+=1;}
		else
			appearCmd("You didn't save wolf3d game.");
		if(findAnywhere("xan.sys"))
			{appearCmd("You managed to keep antyvirus."); points+=1;}
		else
			appearCmd("You didn't save antyvirus.");
		
		if(points>=7)
			appearCmd("***PERFECT SCORE***");
		else if(points==6)
			appearCmd("*Good score*");
		else if(points==5)
			appearCmd("Not bad.");
		else if(points==4)
			appearCmd("It could be worse... or better.");
		else if(points>0)
			appearCmd("Well... At least you saved something.");
		else
			appearCmd("Worst possible score... amazing :)");
	}
	
	bool findAnywhere(string s)
	{
		if(Afiles.Count!=0)
			if(Afiles[0]==s)
				return true;
			
		if(Efiles.Count!=0)	
			if(Efiles[0]==s)
				return true;
			
		foreach(string k in Cfiles)
		{
			if(k==s)
				return true;
		}
		return false;
	}
	
	#region cmds
	void cls()
	{
		cmd.text="";
		wholeString="";
		currentString="";
		cmd.transform.localPosition=new Vector2(0,0);
	}
	
	void ver()
	{
		appearCmd("Exewin95 32bit ver3.11 by Xwinsoft 1985-1997 (C)");
	}
	
	void date()
	{
		appearCmd("It's 1998-02-21");
	}
	
	void shutdown()
	{
		allowToWrite=false;
		state=0;
		cls();
	}
	
	void poweron()
	{
		StartCoroutine(bootMe());
	}
	
	public void resetButton()
	{
		if (state==1)
		{
			shutdown();
			poweron();
		}	
	}
	
	public void powerButton()
	{
		if(state==0)
			poweron();
		else
			shutdown();
	}
	
	string intToDrive(int a)
	{
		if(a==0)
			return "A";
		else if(a==1)
			return "C";
		else if(a==2)
			return "D";
		else
			return "E";
	}
	#endregion
	
	#region run$games$commandz
	void runProgram(string a)
	{
		if(a=="xaninstall.exe")
		{
			if(!antyvir)
			{
				Cfiles.Add("xan.sys");
				antyvir = true;
				appearCmd("Program was successfully installed.");
			}
			else
			{
				appearCmd("Already installed.");
			}
		}
		else if(a=="install.exe")
		{
			if(!virusOn)
			{
				Cfiles.Add("syskill.die");
				virusOn=true;
				appearCmd("Program was successfully installed.");
				if(floppyIns)
					Afiles.Clear();
				if(stickIns)
					Efiles.Clear();
				StartCoroutine(dialogue(1));
			}
			else
			{
				StartCoroutine(autoText("You want to install me again? I'm honored.",false,true));
			}
		}
		else if(a=="some_trash.txt")
		{
			appearCmd("GARBAGE DAY!");
		}
		else if(a=="xan.sys")
		{
			appearCmd("This program run all the time.");
		}
		else if(a=="core.sys")
		{
			StartCoroutine(autoText("//This file is part of system.",true,true));
		}
		else if(a=="shell.sys")
		{
			StartCoroutine(autoText("//This file is part of system.",true,true));
		}
		else if(a=="wolf3d.exe")
		{
			StartCoroutine(autoText("//I love this game, but can't play it now.",true,true));
		}
		else if(a=="note19.txt")
		{
			appearCmd("Work log 19 feb 1998");
			appearCmd("1. test new printer - OK");
			appearCmd("2. finish report.txt - OK");
		}
		else if(a=="note20.txt")
		{
			appearCmd("Work log 20 feb 1998");
			appearCmd("1. write driver for new printer - OK");
			appearCmd("2. get CD from boss - OK");
			appearCmd("3. Install antyvirus - ");
		}
		else if(a=="note21.txt")
		{
			appearCmd("Work log 21 feb 1998");
			appearCmd("1. install and test program form CD - ");
		}
		else if(a=="report.txt")
		{
			if(!virusOn)
				StartCoroutine(autoText("//This report contains info that I cannot lose, maybe it would be good to make copy of it.",true,true));
			else
				StartCoroutine(dialogue(0));
		}
		else if(a=="pdriver.sys")
		{
			if(!virusOn)
				StartCoroutine(autoText("//This is driver for printer. I don't want to lose it.",true,true));
			else
				StartCoroutine(dialogue(2));
		}
		else if(a=="syskill.die")
		{
			if(!g1done)
				virusGame1();
			else
				virusGame2();
		}
	}
	
	void virusGame1()
	{
		allowToWrite=false;
		game1.SetActive(true);
		shell.SetActive(false);	
	}
	
	void virusGame2()
	{
		allowToWrite=false;
		game2.SetActive(true);
		shell.SetActive(false);	
	}
	
	public void takeGame1Result(int t)
	{
		selectFromC(0,(int)Mathf.Floor((float)t/60f));
	}
	
	public void takeGame2Result(int t)
	{
		g2done=true;
		selectFromC(3,19-t);
	}
	
	void selectFromC(int type, int quantity)
	{
		
		cls();
		q=quantity;
		if(type==0) //kill
		{
			executeMode=1;
			if(quantity==3)
				StartCoroutine(autoText("\nIt took you less than 4 hours, which means you must sacrifice 3 files from C:",false,true));
			else if(quantity==4)
				StartCoroutine(autoText("\nIt took you less than 5 hours, which means you must sacrifice 4 files from C:",false,true));
			else
				StartCoroutine(autoText("\nYour score was pretty bad, you must sacrifice 5 files from C:",false,true));
			
			appearCmd("Type name of file you want to kill.");
		}
		if(type==3) //kill2
		{
			executeMode=1;
			if(quantity==3)
				StartCoroutine(autoText("\nNice one, you must sacrifice 3 files this time.",false,true));
			else if(quantity==4)
				StartCoroutine(autoText("\nCould be better, you must sacrifice 4 files this time.",false,true));
			else
			{
				StartCoroutine(autoText("\nThat was so bad, you must sacrifice 5 files from C:",false,true));
				q=5;
			}
			
			appearCmd("Type name of file you want to kill.");
		}
		else if(type==1) //copy to A
		{
			appearCmd("Type name of file you want to copy.");
			executeMode=2;
		}
		else if(type==2) //copy to E
		{
			appearCmd("Type name of file you want to copy.");
			executeMode=3;
		}
		foreach(string k in Cfiles)
		{
			appearCmd(k);
		}
	}
	
	void copyToA()
	{
		foreach(string k in Cfiles)
		{
			if(k==currentString)
			{
				appearCmd(k+" copied to A:");
				Afiles.Add(k);
				executeMode=0;
			}
		}
	}
	
	void copyToE()
	{
		foreach(string k in Cfiles)
		{
			if(k==currentString)
			{
				appearCmd(k+" copied to E:");
				Efiles.Add(k);
				executeMode=0;
			}
		}
	}
	
	void wipeFile()
	{
		
		if(Cfiles.Count==1)
		{
			executeMode=0;
			StartCoroutine(autoText("\nAll files gone? goodbye then. [reset your computer]",false,true));
			virusOn=false;
			
		}
		for(int i = 0;i<Cfiles.Count;i++)
		{
			if(Cfiles[i]==currentString&&currentString!="syskill.die")
			{
				appearCmd(Cfiles[i]+" deleted.");
				Cfiles.RemoveAt(i);
				q--;
				if(q>0)
				{
					cls();
					appearCmd(q+" more.");
					foreach(string k in Cfiles)
					{
						appearCmd(k);
					}
					if(q==1 && g2done)
						foreach(string k in Cfiles)
						{
							if(k=="xan.sys")
							{
								appearCmd("[SYSTEM MESSAGE] Xan antyvirus stopped suspicious action. Enter any command to continue.");
								executeMode=0;
								StartCoroutine(dialogue(4));
								virusOn=false;
								break;
							}
						}
				}
				else
				{
					executeMode=0;
					if(!g1done)
					{
						StartCoroutine(dialogue(3));
						g1done=true;
					}
					else
					{
						StartCoroutine(dialogue(4));
						virusOn=false;
					}
				}
			}
		}
	}
	
	void command(string cmdz)
	{
		/* exes&txts */
		if(curDir==0)
		{
			if(floppyIns)
			{
				foreach(string k in Afiles)
				{
					if(cmdz==k)
					{
						runProgram(k);
						return;
					}
				}
			}
		}
		else if(curDir==1)
		{
			foreach(string k in Cfiles)
			{
				if(cmdz==k)
				{
						runProgram(k);
						return;
				}
			}
		}
		else if(curDir==2)
		{
			if(cdIns)
			{
				foreach(string k in Dfiles)
				{
					if(cmdz==k)
					{
						runProgram(k);
						return;
					}
				}
			}
		}
		else if(curDir==3)
		{
			if(stickIns)
			{
				foreach(string k in Efiles)
				{
					if(cmdz==k)
					{
						runProgram(k);
						return;
					}
				}
			}
		}
		
		
		
		if(cmdz == "cd")
		{
			appearCmd("Current drive(A/C/D/E): "+intToDrive(curDir)+", type 'cd [small letter]' to change, ex: cd a");
			return;
		}
		
		
		if(cmdz.Length<=2)
		{
			appearCmd("'"+cmdz+"' - unknown command, type 'help'.");
			return;
		}
		// comment
		if(cmdz.Substring(0,2)=="//")
		{
			Debug.Log("cmdz");
		}
		//cls
		else if(cmdz == "cls")
		{
			cls();
		}
		//ver
		else if(cmdz == "ver")
		{
			if(virusOn)
				StartCoroutine(autoText("You are running the newest version of SysKill!",false,true));
			else
				ver();
		}
		//dir
		else if(cmdz == "dir")
		{
			if(curDir==0)
			{
				if(floppyIns)
				{
					foreach(string k in Afiles)
					{
						appearCmd(k);
					}
				}
			}
			else if(curDir==1)
			{
				foreach(string k in Cfiles)
				{
					appearCmd(k);
				}
			}
			else if(curDir==2)
			{
				if(cdIns)
				{
					foreach(string k in Dfiles)
					{
						appearCmd(k);
					}
				}
			}
			else if(curDir==3)
			{
				if(stickIns)
				{
					foreach(string k in Efiles)
					{
						appearCmd(k);
					}
				}
			}
		}
		//help
		else if(cmdz == "help")
		{
			if(virusOn)
				StartCoroutine(autoText("Scream for help!",false,true));

				cmd.text+="\ncd\ndir\necho\nshutdown\nreset\ncls\nver\ndate\nformat\ncopy";
		}
		//date
		else if(cmdz == "date")
		{
			if(virusOn)
				StartCoroutine(autoText("NOT TODAY.",false,true));
			else
				date();
		}
		//copy
		else if(cmdz=="copy")
		{
			if(virusOn)
			{
				StartCoroutine(autoText("You really think I will allow this?",false,true));
				return;
			}
			if(curDir==0||curDir==3)
			{
				
				if(curDir==0)
				{
					if(!floppyIns)
					{
						appearCmd("Floppy not inserted");
						return;
					}
					if(Afiles.Count!=0)
						appearCmd("Device may only have 1 file.");
					else
						selectFromC(1,1);
				}
				if(curDir==3)
				{
					if(!stickIns)
					{
						appearCmd("USB device not inserted");
						return;
					}
					if(Efiles.Count!=0)
						appearCmd("Device may only have 1 file.");
					else
						selectFromC(2,1);
				}
			}
			else
				appearCmd("This command is only available on removable devices.");
		}
		//format
		else if(cmdz=="format")
		{
			if(curDir==0||curDir==3)
			{
				if(curDir==0)
				{
					if(!floppyIns)
					{
						appearCmd("Floppy not inserted");
						return;
					}
					appearCmd("All data on A: cleared.");
					Afiles.Clear();
				}
				else
				{
					if(!stickIns)
					{
						appearCmd("USB device not inserted");
						return;
					}
					appearCmd("All data on E: cleared.");
					Efiles.Clear();
				}
			}
			else
				appearCmd("This command is only available on removable devices.");
		}
		else if(cmdz.Length<=3)
		{
			appearCmd("'"+cmdz+"' - unknown command, type 'help'.");
			return;
		}
		//echo
		else if(cmdz.Substring(0,4) == "echo")
		{
			cmdz+="     ";
			if(cmdz.Substring(4,1)==" ")
				appearCmd(cmdz.Substring(5,cmdz.Length-5));
			else
				appearCmd("Usage: echo [text]");
		}
		//shutdown
		else if(cmdz == "shutdown")
		{
			shutdown();
		}
		//reset
		else if(cmdz == "reset")
		{
			shutdown();
			poweron();
		}
		//cd
		else if(cmdz.Substring(0,2) == "cd")
		{
			
			if(cmdz == "cd a")
			{
				curDir = 0;
			}
			else if(cmdz == "cd c")
			{
				curDir = 1;
			}
			else if(cmdz == "cd d")
			{
				curDir = 2;
			}
			else if(cmdz == "cd e")
			{
				curDir = 3;
			}
		}
		else
		{
			appearCmd("'"+cmdz+"' - unknown command, type 'help'.");
		}
		
	}
	#endregion
	
	}

