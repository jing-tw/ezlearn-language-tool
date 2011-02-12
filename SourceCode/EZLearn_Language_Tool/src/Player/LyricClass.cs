using System;
using System.Collections;
using System.Windows.Forms;

namespace Player
{
	/// <summary>
	/// Summary description for LyricClass.
	/// </summary>
	public class LyricClass
	{
		AVPlayer myAudioPlayer; // �y������
		Form myShowPannel; // ��ܳo�Ӥ��� pannel
		public Label nextlab; // ��ܤU�@�y�� label
		int myAudioPlayer_oldHeight=0;
		int hScrollBar1_oldTop=0;

		public bool bVisible=true;

		private bool bLyricReady; //  �{�b�q������O�_�w�g Ready
		public  System.Collections.ArrayList LyricInfoArray=new System.Collections.ArrayList();// �q����C
		private bool bLoadingLyric=false; // �{�b�O�_���b���J�q��, ���F�קK IndexListForm �]�����ܦӲ��� Seek Audio Player ���ʧ@

		public SwiftlyLabel mySwiftlabel; // ����q�����ʪ�����
		public int Top;

		public LyricClass(AVPlayer AudioPlayer,int locy, Form ShowPannel){
			myAudioPlayer = AudioPlayer;
			myShowPannel=ShowPannel;
			Top=locy;

			
		}

		public void Prepare(){
			// �]���O�������  (�r�����ϥ�)
			bLyricReady=ReadLyricFile();
			if(bLyricReady){
				/*
				parent.SetStyle(ControlStyles.DoubleBuffer | 
					ControlStyles.UserPaint | 
					ControlStyles.AllPaintingInWmPaint
					,true);	
					*/

				// 1. �إ߶]���O
				mySwiftlabel=new SwiftlyLabel(myAudioPlayer.Width);
				mySwiftlabel.setInfoLabel(myAudioPlayer.label2);
				mySwiftlabel.setMyLyricer(this);
				mySwiftlabel.Top=Top; // �]�w���׬� label3 ����m

				myAudioPlayer_oldHeight=myAudioPlayer.Height;
				hScrollBar1_oldTop=myAudioPlayer.hScrollBar1.Top;
				Visible_Enable();
				

				PlugIn(myShowPannel); // �N����˦b��ܾ��W
				// end of �]���O�������

				// �r���M�έp�ɾ�
				LyricTimer myTimer2=new LyricTimer(myAudioPlayer,this);
				myTimer2.Tick += new EventHandler(TimerEventProcessor2);			
				myTimer2.Interval = 10; // �C�j 1 ����, �I�s TimerEventProcessor procedure
				myTimer2.Start();
				// end of �r���M�έp�ɾ�

				// �[�J��ܤU�@�y�� label

				// �]�w label ���r��
				nextlab =new Label();
				// ���w�r��
					nextlab.Font=new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
					nextlab.ForeColor=System.Drawing.Color.White;
				// end of �r��

				mySwiftlabel.Controls.Add(nextlab);
				nextlab.Top=nextlab.Height; // �]�w���ĤG��
				nextlab.AutoSize=true;

			}

			

		}

		public void Resume(){
			if(this.bLyricReady){
				this.mySwiftlabel.Start();
			}
		}

		public void Pause(){
			if(this.bLyricReady){
				this.mySwiftlabel.Stop();
			}
		}

		// ���s�]�w��ܦr��
		public void ClearShowState(){
			for(int i=0;i<LyricInfoArray.Count ;i++){
				string[] InfoNode= (string[]) (LyricInfoArray[i]);
				InfoNode[2]="false";
			}
			this.mySwiftlabel.ClearAllLabel();
		}

		// �r���M�� �ˬd�ƥ���
		private static void TimerEventProcessor2(Object myObject,
			EventArgs myEventArgs) {
			AVPlayer myPlayer=((LyricTimer)myObject).myPlayer;
			LyricClass myLyric=((LyricTimer)myObject).myLyric;
			// �r���B�z
			if(myPlayer.ourAudio !=null) {
				// ipos
				if(myLyric.isLyricReady()==true){
					double pos=myPlayer.ourAudio.CurrentPosition;
					for(int i=0;i<myLyric.LyricInfoArray.Count ;i++){
						string[] InfoNode= (string[]) (myLyric.LyricInfoArray[i]);
						double s=Double.Parse(InfoNode[0]);

						string strLyric=InfoNode[1]; // Ū���ثe�ɨ誺�q��
						string nextLyric=" "; // �U�@�ӭn�ۺq��

						bool bHasShow=InfoNode[2].CompareTo("false")==0;
						double mods=s+myLyric.mySwiftlabel.Latency;

						if(pos > mods  && bHasShow){
							// �]�w�r����ܳt�� (�]���p�G�r�Ӫ�, �|�y���᭱���r���b�t�ۮɥX����)
							// Ū���U�@�y�}�l���ɶ�
							int next=i+1;
							int ut=0;
							bool bShowNow=true;
							if(next < myLyric.LyricInfoArray.Count){ // �p�G���U�@�Ӻq��
								// �p��i�e�\����o�ӥy�l���ɶ�
								string[] InfoNode2= (string[]) (myLyric.LyricInfoArray[next]);
								double nextSs=Double.Parse(InfoNode2[0]);
								
								double Dur=nextSs-s;
								// end of �e�\�ɶ�

								// ��ܤU�@�y
								nextLyric=InfoNode2[1];
								if(myLyric.bShowNext){
									myLyric.nextlab.Text=nextLyric;
								}
								// end of  ��ܤU�@�y

								// �p��C�Ӧr�ܤֻݭn t �����
								if(strLyric.Length >0){
									ut=(int)((Dur/(strLyric.Length+20))*1000);
									if(ut > 100)
										ut=100;
									myLyric.mySwiftlabel.Speed(ut);
								}
								// end of �p��

								// �p��O�_���W�n���, �]�����i�� pos �Q���޸��L�n�X�ӥy�l
								if(nextSs < pos){
									bShowNow=false; // �U�@�ӥy�l��ثe��m��
								}

								
								
							}
							

							// end of ��ܳt��
							InfoNode[2]="true";// �]�w�w�g���
							if(bShowNow)
								myLyric.mySwiftlabel.AddLabel(strLyric); // ���W���

							// myPlayer.label3.Text="�q������"+InfoNode[0];//+" Speed="+ut;
                            string Info = "�q������" + InfoNode[0];
                            MessageManager.ShowInformation(myPlayer.label3, Info, myPlayer.LabelMessage_DefaultDisplay_Time);
						}
					}
				
				}
				// end of �r���B�z
			}
		}
		// end of �ˬd�ƥ���


		public bool isLyricReady(){
			return bLyricReady;
		}

		public bool isLoadingLyric(){
			return bLoadingLyric;
		}

		// ��ܤ��󪺤j�p �P�X���m
		public int getHeight(){
			return mySwiftlabel.Height;
		}

		public int getBottom(){
			return mySwiftlabel.Bottom;
		}
		// end of ��ܤ���

		// ��ۤv�[�������
		public void PlugIn(Form showPannel){
			showPannel.Controls.Add(mySwiftlabel);
			mySwiftlabel.Visible=true;
		}

		public string VisibleSwitch(){
			String Infotext;
			this.bVisible=!this.bVisible;
			if(this.bVisible==true){
				// menuItem24.Text="����ܦr��";
				Infotext="����ܦr��";
				Visible_Enable();
			}else{
				//menuItem24.Text="��ܦr��";
				Infotext="��ܦr��";
				Visible_Disable();
			}
			return Infotext;
		}

		public void Visible_Enable(){
			if(mySwiftlabel!=null){
				this.bVisible=true;
			
				mySwiftlabel.Visible=true;
				// ���s�վ� Form �����׻P scrollbar ����m
				myAudioPlayer.hScrollBar1.Top=mySwiftlabel.Bottom+2;
				myAudioPlayer.Height=myAudioPlayer_oldHeight+mySwiftlabel.Height+myAudioPlayer.hScrollBar1.Height;	
				// end of �վ�������󪺦�m
			}
		}

		public void Visible_Disable(){
			if(mySwiftlabel!=null){   // �u���b �]���O����Q�إߪ����p�U, �~���N�q
				this.bVisible=false;
				mySwiftlabel.Visible=false;
				// ���s�վ� Form �����׻P scrollbar ����m
				myAudioPlayer.Height=myAudioPlayer_oldHeight;
				myAudioPlayer.hScrollBar1.Top=hScrollBar1_oldTop;
				// end of �վ�������󪺦�m
			}

		}

		public bool bShowNext=false; // �ثe���A�O�_����ܤU�@�y�q��
		public void showNext(){
			if(!bShowNext){
				mySwiftlabel.Height=mySwiftlabel.Height*2;

				this.Visible_Enable();

				bShowNext=true;
			}
		}
		public void DisableShowNext(){
			if(bShowNext){
				mySwiftlabel.Height=mySwiftlabel.Height/2;
				this.Visible_Enable();
				bShowNext=false;
			}
		}

		
		// Lyric File format (�r��)
		// filename: [lyric]_filename.txt
		// content:
		// [second] text
		private bool ReadLyricFile(){
			string curAudioFullFileName=myAudioPlayer.ofdOpen.FileName;
			string curAudioFileName=curAudioFullFileName.Substring(curAudioFullFileName.LastIndexOf("\\")+1);
			string strFullLyricFileName=myAudioPlayer.ExecutationFileDir+"\\[lyric]_"+curAudioFileName+".txt";

			IndexListForm MyIndexList=myAudioPlayer.MyIndexList;
			// ���ɮפ�, ���J�q��
			try {
				using (System.IO.StreamReader sr = new System.IO.StreamReader(strFullLyricFileName)) {
					
					MyIndexList.ClearList(false);

					String line;
					bLoadingLyric=true;
					while ((line = sr.ReadLine()) != null) {
						// �ˬd�O�_�� ��ܰ_�l���
						int s=line.IndexOf("[");
						if(s!=-1){
							int e=line.IndexOf("]");
							string strNum=line.Substring(s+1,e-s-1);
							MyIndexList.AddItem(strNum);

							string strLyric=line.Substring(e-s+1);
							
							//mySwiftlabel.AddLabel(strLyric);

							// �[�J��T��C
							string[] InfoNode=new String[3];
							InfoNode[0]=strNum; // �_�Ϯɶ�
							InfoNode[1]=strLyric; // �q��
							InfoNode[2]="false"; // �O�_�w�g��� (���[�i��, �ҥH�@�w�S�����)
							
							LyricInfoArray.Add(InfoNode);
						}
						
					}
					MyIndexList.InitialSelect();

					bLoadingLyric=false; // �аO�������J�q�� (�P MyIndexList ����)
					//label3.Text=label3.Text+" (���q��)";
					
					return true;
				}
			}
			catch (Exception e) {
				// Let the user know what went wrong.
				//Console.WriteLine("The file could not be read:");
				//Console.WriteLine(e.Message);
				return false;
			}
		
		}
	}

	class LyricTimer:System.Windows.Forms.Timer {
		public LyricClass myLyric;
		public AVPlayer myPlayer;
		public LyricTimer(AVPlayer showPannel,LyricClass myLyric) {
			myPlayer=showPannel;
			this.myLyric=myLyric;
		}
	}
}
