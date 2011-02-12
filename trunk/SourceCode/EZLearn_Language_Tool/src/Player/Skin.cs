/*
Usage:
                // Palm Skin XML �]�w�ɴ���
				string SkinDir=this.ExecutationFileDir+"\\skin\\Palm_by_fugenet";
				mySkin=new Skin(SkinDir,this,textBox1,label2,label3,button1,PlayButton,PauseButton,IndexListButton,CloseButton,hScrollBar1,l1,label1);
				mySkin.conLoadPalmSkin();				
				// end of XML ����
  
 Check list: 
 *   1. �Y�n�ϥ� skin, �h�����n�� Form �� Auto Size �]�w�� false

				
*/

using System;
using System.Windows.Forms; // for Form
using System.Drawing; // for point, Region
using System.Drawing.Drawing2D; // for  GraphicsPath
using System.Collections; // for ArrayList

//  XML �s��/Ū�� �ϥ�
using System.IO;// for save skin infomation 
using System.Xml;

namespace Player {
	/// <summary>
	/// Summary description for Skin.
	/// </summary>
	public class Skin {
		const bool bUseFadeIn=true;
		System.Windows.Forms.Form myForm; // �n��� skin ���ؼе���
		// AVPlayer myForm;

		string SkinDir; // Skin �ؿ�
		System.Collections.Hashtable BottomImageTable=new System.Collections.Hashtable();  // ���s�v�����

		// ���޲z������
		JingTextEdit1 Input;
		System.Windows.Forms.Label StatusLabel;
		System.Windows.Forms.Label FilenameLabel;
		System.Windows.Forms.Label TotalLengthLabel;

		System.Windows.Forms.HScrollBar ScrollBar;
		NumberLabel TimeLabel;


		public System.Drawing.Bitmap  combo_PlayImage;
		public System.Drawing.Bitmap  combo_PauseImage;
		System.Windows.Forms.Button PlayPauseButton;
		System.Windows.Forms.Button PlayButton;
		System.Windows.Forms.Button PauseButton;
		System.Windows.Forms.Button IndexListButton;
		System.Windows.Forms.Button  CloseButton;
		// end of ���޲z������
		
		// �T�ӥD�n�B�J: ���  Skin 
		// Step 0: 
		//  ========================== �@�� Skin ��������@�� Form =====================
		public Skin(string SkinDir,AVPlayer aPlayer) {
			myForm=aPlayer;
			this.SkinDir=SkinDir;

            myForm.AutoSize = false; // �o�� Form �������j�p�ѧڭ̨ӳB�z
		}

		// Step 1
		// ========================== ���ܫ��w Form ���Ϊ� =====================
		public void ChangeFormShape(string maskFile,byte[] BGColor){
			ChangeFormShape(maskFile,BGColor,true);
		}
		public void ChangeFormShape(string maskFile,byte[] BGColor,bool bChangeShape){
			if(bUseFadeIn)		
				myForm.Opacity=0.2;

			string fullmaskFile=SkinDir+"\\"+maskFile;
			ApplyMainSkin(fullmaskFile);   // �����J�ϧ�
			if(bChangeShape==true){
				//byte bR=255,bG=69,bB=245;
				byte bR,bG,bB;
				bR=BGColor[0];
				bG=BGColor[1];
				bB=BGColor[2];
				bool[][] bMask=createMask(fullmaskFile,bR,bG,bB);

				int H=bMask.Length ;
				int W=bMask[0].Length;

				myForm.Height=H+30;
				myForm.Width=W;

			
			
				Region myShape=getRegion(bMask); // ���ۦA���ܽ���
				myForm.Region=myShape;
			}else{
				// �S�� shape �����p, ������j�p�� BG Image �M�w
				//myForm.Height=myForm.BackgroundImage.Height;
				// myForm.Width=myForm.BackgroundImage.Width;
				
				myForm.Height=136+16;
				myForm.Width=312;
			}
			
		}
		// ========================== end of ���ܫ��w Form ���Ϊ� =====================


		// Step 2
		// ================================= ��露����ܼv���P��m�t�m ======================
		// ���w�Q�n���ܤ��󪺦�m (W or H == -1 �h��� don't care)		
		public void ControlLoc(System.Windows.Forms.Control obj,int[] Loc,int Width,int Height){
			obj.Left=Loc[0];
			obj.Top=Loc[1];
			if(Width!=-1)
				obj.Width=Width;
			if(Height!=-1)
				obj.Height=Height;
		}
		
		// �󴫫��w���s�� skin
		public void ButtonSkin(string strName,System.Windows.Forms.Button aButton,string strNormalBMP,string strUnderBMP,int[] BottonLoc){
			aButton.Left=BottonLoc[0];
			aButton.Top=BottonLoc[1];
			aButton.Visible=true;

			System.Drawing.Bitmap[] PlayImage=(System.Drawing.Bitmap[]) BottomImageTable[aButton.GetHashCode()];
			if(PlayImage==null){
				PlayImage=new System.Drawing.Bitmap[2]; // �إ߷s�� Image
			}
			LoadSkinImage(strNormalBMP,strUnderBMP,PlayImage);
			BottomImageTable[aButton.GetHashCode()]=PlayImage; // Update �v��

			// ���w�ثe���s��ܼv�����@��v��
			System.Drawing.Bitmap NormalForm=PlayImage[0];
			aButton.Image=NormalForm;
			aButton.Height=NormalForm.Height;
			aButton.Width=NormalForm.Width;
			aButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;

			// �]�w�ثe���s�� Event
			aButton.MouseEnter += new System.EventHandler(ButtonMouseEnterHandle);
			aButton.MouseLeave += new System.EventHandler(ButtonMouseLeaveHandle);
		}
		// end of ���s 
		// ================================= end of ��露����ܼv���P��m�t�m ======================



		
		// Palm Skin ��K�禡
		/*
				 ��ʨϥνd�� Palm Skin  ����
				string SkinDir=this.ExecutationFileDir+"\\skin\\Palm_by_fugenet";
				mySkin=new Skin(SkinDir,this);
				mySkin.conChangeSkin_PalmLoc("main.bmp",new byte[]{255,69,245},textBox1,label2,label3,button1,PlayButton,PauseButton,IndexListButton,CloseButton,hScrollBar1,l1,label1);
				
		 */
		 
		public void conChangeSkin_PalmLoc(string maskFile,byte[] BGColor,JingTextEdit1 Input,
			System.Windows.Forms.Label StatusLabel,
			System.Windows.Forms.Label FilenameLabel,
			System.Windows.Forms.Button PlayPauseButton,
			System.Windows.Forms.Button PlayButton,
			System.Windows.Forms.Button PauseButton,
			System.Windows.Forms.Button IndexListButton,
			System.Windows.Forms.Button CloseButton,
			System.Windows.Forms.HScrollBar ScrollBar,
			NumberLabel TimeLabel, 
			System.Windows.Forms.Label TotalLengthLabel){

			PlayPauseButton.Visible =false;   // Palm Skin ���ݭn�ƦX�����s

			
	
			// ���ܥD�������Ϊ�
			string fullMaskFile=this.SkinDir+"\\"+maskFile;
			ChangeFormShape(maskFile,BGColor);
			// end of �D�������Ϊ�

			// �վ� scroll bar ����m (42,125,215,135)
			int[] ScrollLoc=new int[]{42,125,215,135};
			int W=ScrollLoc[2]-ScrollLoc[0];
			int H=ScrollLoc[3]-ScrollLoc[1];
			ControlLoc(ScrollBar,ScrollLoc,W,H);
			// end of scroll bar

			// �վ�Ʀr��m 
			int[] TimeLoc=new int[]{103,78};
			ControlLoc(TimeLabel,TimeLoc,-1,-1);

			int x=TimeLabel.Left+TimeLabel.Width+3;
			int y= TimeLabel.Top+5;
			ControlLoc(TotalLengthLabel,new int[]{x,y},-1,-1);
			// end of �վ�Ʀr��m
			
			// �վ㪬�A�C����m 
			int[] StatusLoc=new int[]{119,114};
			ControlLoc(StatusLabel,StatusLoc,-1,-1);

			// �վ��ɮצW�٦�m  
			int[] FilenameLoc=new int[]{44,46};
			ControlLoc(FilenameLabel,FilenameLoc,-1,-1);

			// �վ��J����m
			int[] InputLoc=new int[]{68,192,193-68-5};
			W=InputLoc[2];
			ControlLoc(Input,InputLoc,W,-1);
			
			int[] PlayLoc=new int[]{100,147};
			ButtonSkin("Play",PlayButton,"bplayn.bmp","bplayu.bmp",PlayLoc); // �վ� Player ���s����m 
		
			int[] PauseLoc=new int[]{123,146};
			ButtonSkin("Pause",PauseButton,"bpausen.bmp","bpauseu.bmp",PauseLoc);// �B�z Pause ���s��m�P�v��

			int[] IndexListLoc=new int[]{205,256};
			ButtonSkin("IndexList",IndexListButton,"btn9n.bmp","btn9u.bmp",IndexListLoc); // �B�z���ެ���ï���s

			int[] CloseLoc=new int[]{189,2};
			ButtonSkin("Close",CloseButton,"bexitn.bmp","bexitu.bmp",CloseLoc); // �B�z�������s


            OpacityUtility myOpacityObj = new OpacityUtility();
            myOpacityObj.FadeIn_Only(myForm, 30);
		}

		// Ū�� XML �]�w�ɪ���

		// �`�N: Ū���� XML �ɮץ����n�� UTF-8 �榡�s��
	
		/*
		 xml �d��:
		 <EZLearn_Skin_Layout Project_Code="EZLearn �y���ǲ߾�">
				 <MainWindow Filename="main.bmp" />
				 <Scroll_Bar left="42" top="125" right="215" bottom="135" />
				 </EZLearn_Skin_Layout >
		 */
		
		// Step 1: �غc�l
		public Skin(string SkinDir,AVPlayer aPlayer,JingTextEdit1 Input,
			System.Windows.Forms.Label StatusLabel,
			System.Windows.Forms.Label FilenameLabel,
			System.Windows.Forms.Button PlayPauseButton,
			System.Windows.Forms.Button PlayButton,
			System.Windows.Forms.Button PauseButton,
			System.Windows.Forms.Button IndexListButton,
			System.Windows.Forms.Button CloseButton,
			System.Windows.Forms.HScrollBar ScrollBar,
			NumberLabel TimeLabel, 
			System.Windows.Forms.Label TotalLengthLabel){

			this.combo_PlayImage=combo_PlayImage;
			this.combo_PauseImage=combo_PauseImage;

			myForm=aPlayer;
			this.SkinDir=SkinDir;

			this.Input=Input;
			this.StatusLabel =StatusLabel;
			this.FilenameLabel=FilenameLabel;
			this.ScrollBar=ScrollBar;
			this.TimeLabel=TimeLabel;
			this.TotalLengthLabel=TotalLengthLabel;


			this.PlayPauseButton=PlayPauseButton;
			this.PlayPauseButton.Visible =false;
			this.PlayButton=PlayButton;
			this.PlayButton.Visible =false;

			this.PauseButton=PauseButton;
			this.PauseButton.Visible =false;

			this.IndexListButton=IndexListButton;
			this.IndexListButton.Visible=false;

			this.CloseButton=CloseButton;
			this.CloseButton.Visible=false;

			
		}

	
		
		public void conLoadPalmSkin(){
			string strFullSkinConfigFile=this.SkinDir+"\\skin.xml";
			LoadSkin(strFullSkinConfigFile);
		}

		public void conLoadHelloKittySkin(){
			string strFullSkinConfigFile=this.SkinDir+"\\skin.xml";
			LoadSkin(strFullSkinConfigFile);
		}

		public void LoadSkin(){
			string strFullSkinConfigFile=this.SkinDir+"\\skin.xml";
			LoadSkin(strFullSkinConfigFile);
		}


		

		public void LoadSkin(string strFullSkinConfigFile){
			XmlValidatingReader reader = null;

			try {
				XmlTextReader txtreader = new XmlTextReader(strFullSkinConfigFile);
				txtreader.WhitespaceHandling = WhitespaceHandling.None;

				//Implement the validating reader over the text reader. 
				reader = new XmlValidatingReader(txtreader);
				reader.ValidationType = ValidationType.None;

  
				//Parse the XML fragment.  If they exist, display the   
				//prefix and namespace URI of each element.
				while (reader.Read()){
					switch (reader.NodeType){
						case XmlNodeType.Element:
							Console.WriteLine("<{0}>", reader.LocalName);

							LoadSkin(reader);
							// �C�X Attribute
							int num=reader.AttributeCount;
							for(int i=0;i<num;i++){
								reader.MoveToAttribute(i);
								Console.WriteLine("    Attribute Name {0} ={1}", reader.Name,reader.Value);
							}

							// �C�X Element 
							reader.MoveToElement();
							break;
					}
				}

                if (bUseFadeIn)
                {
                    OpacityUtility myOpacityObj = new OpacityUtility();
                    myOpacityObj.FadeIn_Only(myForm, 30);
                }

			}catch(System.Xml.XmlException ee){
				Console.WriteLine("Exception "+ee);
			}

			finally {
				if (reader != null)
					reader.Close();
			}

		}

		//===========================================  XML ���γ��� =========================================
		private void LoadSkin(XmlValidatingReader reader){
			string ElementName=reader.LocalName;
			//string Normal=null,Under=null;
		    int iRight=-1,width=-1;
			string strRight=null;
			
			switch(ElementName){
				case "MainWindow":
					// Ū�� mask �ɮצW��
					string maskFile=reader["Shape"];
					string fullMaskFile=null;
					fullMaskFile=this.SkinDir+"\\"+maskFile;

					string bstrShape=reader.GetAttribute("NOSHAPE");
					if(bstrShape==null){
						// Ū���I���C��ƭ�
						byte[] BGColor=new byte[3];
						BGColor[0]=byte.Parse(reader["BG_R"]);
						BGColor[1]=byte.Parse(reader["BG_G"]);
						BGColor[2]=byte.Parse(reader["BG_B"]);

						ChangeFormShape(maskFile,BGColor);
					}else{
						// �S�� shape information , �u���I�����
						ChangeFormShape(maskFile,null,false);
					}
					break;
				case "Scroll_Bar":
					int[] ScrollLoc=new int[4];
					ScrollLoc[0]=int.Parse(reader["left"]);
					ScrollLoc[1]=int.Parse(reader["top"]);
					ScrollLoc[2]=int.Parse(reader["right"]);
					ScrollLoc[3]=int.Parse(reader["bottom"]);

					int W=ScrollLoc[2]-ScrollLoc[0];
					int H=ScrollLoc[3]-ScrollLoc[1];
					ControlLoc(ScrollBar,ScrollLoc,W,H);
					break;

				case "TimeLabel":
					int[] TimeLoc=new int[2];
					TimeLoc[0]=int.Parse(reader["x"]);
					TimeLoc[1]=int.Parse(reader["y"]);

					ControlLoc(TimeLabel,TimeLoc,-1,-1);

					int x=TimeLabel.Left+TimeLabel.Width+3;
					int y= TimeLabel.Top+5;
					ControlLoc(TotalLengthLabel,new int[]{x,y},-1,-1);
					SetLabelColor(TotalLengthLabel,reader); // �]�w�C��
					break;

				case "StatusLabel":
					int[] StatusLoc=new int[2];
					StatusLoc[0]=int.Parse(reader["x"]);
					StatusLoc[1]=int.Parse(reader["y"]);
					strRight=reader["right"];

					if(strRight!=null){
						iRight=int.Parse(strRight);
					    width=iRight-StatusLoc[0];
					}else{
						width=-1;
					}
					ControlLoc(StatusLabel,StatusLoc,width,-1);

					SetLabelColor(StatusLabel,reader); // �]�w�C��

					break;

				case "FilenameLabel":
					int[] FilenameLoc=new int[2];
					FilenameLoc[0]=int.Parse(reader["x"]);
					FilenameLoc[1]=int.Parse(reader["y"]);

					strRight=reader["right"];
					if(strRight!=null){
						iRight=int.Parse(strRight);
						width=iRight-FilenameLoc[0];
					}else{
						width=-1;
					}
					ControlLoc(FilenameLabel,FilenameLoc,width,-1);

					SetLabelColor(FilenameLabel,reader); // �]�w�C��
					break;

				case "Input":
					int[] InputLoc=new int[3];
					InputLoc[0]=int.Parse(reader["x"]);
					InputLoc[1]=int.Parse(reader["y"]);
					InputLoc[2]=int.Parse(reader["width"]);

					W=InputLoc[2];
					ControlLoc(Input,InputLoc,W,-1);
					break;

				case "PlayPause":
					ComboxPlayPauseSkin(ElementName,PlayPauseButton,reader);
					/*
										PlayPauseButton.Visible =true;

										int[] PalyPauseLoc=new int[2];
										PalyPauseLoc[0]=int.Parse(reader["x"]);
										PalyPauseLoc[1]=int.Parse(reader["y"]);

										try{
											Normal=reader["Normal_Shape"];
											Under=reader["Under_Shape"];
											ButtonSkin(ElementName,PlayPauseButton,Normal,Under,PalyPauseLoc); // �վ� PalyPause ���s����m 
										}catch(Exception){
											// �Y�ϥΪ̨S�����w��, �h�ϥιw�]���
											//PlayPauseButton.setSelfPaint();
											ControlLoc(PlayPauseButton,PalyPauseLoc,-1,-1);
										}
					*/
					break;

				case "Play":
					SetCommonButtonSkin(ElementName,PlayButton,reader);// �@�� PlayButton ���s
					break;
				case "Pause":
					SetCommonButtonSkin(ElementName,PauseButton,reader);// �@�� PauseButton ���s
					break;

				case "IndexList":
					SetCommonButtonSkin(ElementName,IndexListButton,reader);// �@�� IndexList ���s
					break;
				case "Close":
					SetCommonButtonSkin(ElementName,CloseButton,reader); // �@�� Close ���s
					break;
			}
		}
		//===========================================  end of XML ���γ��� =========================================
		
		private void SetLabelColor(Label label,XmlValidatingReader reader){
			// �C�⪺�]�w

			string R=reader.GetAttribute("R");
			string G=reader.GetAttribute("G");
			string B=reader.GetAttribute("B");

			if(R!=null && G!=null && B!=null){
				int iR=int.Parse(R);
				int iG=int.Parse(G);
				int iB=int.Parse(B);
				label.ForeColor=System.Drawing.Color.FromArgb(iR,iG,iB);
			}
			// end of �C�⪺�]�w
		}

		private void ComboxPlayPauseSkin(String ElementName,Button bt,XmlValidatingReader reader){
			bt.Visible =true;

			int[] Loc=new int[2];
			Loc[0]=int.Parse(reader["x"]);
			Loc[1]=int.Parse(reader["y"]);

			PlayPauseButton.Visible =true;

			int[] PalyPauseLoc=new int[2];
			PalyPauseLoc[0]=int.Parse(reader["x"]);
			PalyPauseLoc[1]=int.Parse(reader["y"]);

			

			string strCombo_PlayImage=this.SkinDir+"\\"+reader["Combo_Play"];
			string strCombo_PauseImage=this.SkinDir+"\\"+reader["Combo_Pause"];
			// ���J�v��
			System.Drawing.Bitmap imgCombo_Play=new System.Drawing.Bitmap(strCombo_PlayImage);
			System.Drawing.Bitmap imgCombo_Pause=new System.Drawing.Bitmap(strCombo_PauseImage);

			// �]�w PlayPause �v��
			this.combo_PlayImage=imgCombo_Play;
			this.combo_PauseImage =imgCombo_Pause;
			bt.Image=combo_PauseImage;
			bt.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			bt.Height=imgCombo_Play.Height;
			bt.Width=imgCombo_Play.Width;
				
			ControlLoc(PlayPauseButton,PalyPauseLoc,-1,-1); // ���� Control ����m
			
		}

		// �@����s�]�w: �]�w��m (�e�����B�z), ��ܼv��, �ƹ��W�żv��
		private void SetCommonButtonSkin(String ElementName,Button bt,XmlValidatingReader reader){
			bt.Visible =true;
			string Normal=null,Under=null;

			int[] Loc=new int[2];
			Loc[0]=int.Parse(reader["x"]);
			Loc[1]=int.Parse(reader["y"]);
			try{
				Normal=reader["Normal_Shape"];
				Under=reader["Under_Shape"];
				ButtonSkin(ElementName,bt,Normal,Under,Loc);  // �B�z�������s
			}catch(Exception){
				// �Y�ϥΪ̨S�����w��, �h�ϥιw�]���
				//CloseButton.setSelfPaint();
				ControlLoc(bt,Loc,-1,-1); // �u�B�z x,y �e�����B�z
			}
		}
		/*



		


				// �U���O���Ψ禡

		
		
				public void SaveInfo(){
					// �g�J��ƨ���w���ɮפ�
					using (System.IO.StreamWriter sw=new System.IO.StreamWriter("c:\\test.xml")){ // �إ߷s�� property �ɮ�
						XmlTextWriter writer = new XmlTextWriter(sw);
						writer.WriteStartElement("EZLearn_Skin_Layout"); // �[�J EZLearn Skin Layout ����
						writer.WriteAttributeString("Project_Code", "EZLearn �y���ǲ߾�"); // �[�J�ݩ�

						writer.Formatting = Formatting.Indented;
						SaveInfo_Sub(writer, "MSFT", 74.125, 5.89, 69020000);
						writer.Close();
					}
					// end of  �g�J���

			
			
				}
		
				// �D�������Ϊ���T�g�J
				private void save_MainShape(XmlTextWriter writer,string strFullMaskFilename){
					writer.WriteStartElement("�D�����Ϊ�"); 
					writer.WriteAttributeString("�ɮצ�m",strFullMaskFilename);
					writer.WriteEndElement();
				}

				private void save_ControlLocYX(string ControlName,int[] Loc){
					writer.WriteStartElement(ControlName); 
					writer.WriteAttributeString("��m_y",Loc[0]);
					writer.WriteAttributeString("��m_x",Loc[1]);
					writer.WriteEndElement();
				}

				private void save_ControlLocRec(string ControlName,int[] Loc){
					writer.WriteStartElement(ControlName); 
					writer.WriteAttributeString("��m_left",Loc[0]);
					writer.WriteAttributeString("��m_top",Loc[1]);
					writer.WriteAttributeString("��m_right",Loc[2]);
					writer.WriteAttributeString("��m_bottom",Loc[3]);
					writer.WriteEndElement();
				}
		

				// 
				private void SaveInfo_Sub(XmlTextWriter writer,string symbol,double price,double change,int volume){
					writer.WriteElementString("Price", XmlConvert.ToString(price));
					writer.WriteStartElement("Sub1"); 
					writer.WriteElementString("Change", XmlConvert.ToString(change));
			
			

					writer.WriteElementString("Volume", XmlConvert.ToString(volume));
					writer.WriteEndElement();
				}

		*/
































		private void ApplyMainSkin(string full){
			myForm.Region=null;
			try{
				System.Drawing.Bitmap imgPlayerBG=new System.Drawing.Bitmap(full); // �ثe���J�v��
				myForm.BackgroundImage=imgPlayerBG;
				/*
					// PropertyTable["Player�D����"]=full;

					if(bsave){
						this.SaveProperty(); // �x�s�_��,�U���i�H��
						this.label2.Text="������s����";
					
					}
					*/
			}catch(Exception ee){
				MessageBox.Show(full+" �i�ण�s�b\n�ԲӸ�T: "+ee.ToString());	
			}
		}



		// ============================== ���s�ƥ� =================================

		// �ƹ�������s�W�ųB�z
		private void ButtonMouseEnterHandle(object sender, System.EventArgs e) {
			System.Windows.Forms.Button myButton=(System.Windows.Forms.Button)sender;

			// ���X Under form �v���åB��ܥX��
			System.Drawing.Bitmap[] PlayImage=(System.Drawing.Bitmap[]) BottomImageTable[myButton.GetHashCode()];
			System.Drawing.Bitmap UnderForm=PlayImage[1];
			myButton.Image=UnderForm;
			// end of ���X Under form �v���åB���
		}
		// end of �ƹ�������s�W�ųB�z

		// �ƹ����}���s�B�z
		private void ButtonMouseLeaveHandle(object sender, System.EventArgs e) {
			System.Windows.Forms.Button myButton=(System.Windows.Forms.Button)sender;

			// ���X Normal form �v���åB���
			System.Drawing.Bitmap[] PlayImage=(System.Drawing.Bitmap[]) BottomImageTable[myButton.GetHashCode()];
			System.Drawing.Bitmap NormForm=PlayImage[0];
			myButton.Image=NormForm;
			// end of ���X Under form �v���åB���
		}
		// end of  �ƹ����}���s�B�z
		// ============================== end of ���s�ƥ� =================================


		// ���J���s skin �ϧ�
		// Note:
		// ButtonImage[0]== Normal Form  ;  ButtonImage[1]== �ƹ��b���s�W�� Form
		private void LoadSkinImage(string strNormal,string strUnder,System.Drawing.Bitmap[] ButtonImage){
			// ���J Playn �P Playu
			// �Y���e�����J�ϧ�, �{�b��
			// �`�N: �Q�� FileStream �}���ɮצ�y, ���J���ɧ�����, �O�o�n������y,
			// �_�h�N�|�o�ͤU�C�{�H:
			//             (�_��, �w�g���� MSDN �һ��� Dispose , ���� �����ٷ|�Q lock ?)
			for(int i=0;i<2;i++){
				if(ButtonImage[i]!=null)
					ButtonImage[i].Dispose();
			}
			// end of ���e���J�ϧ�

			string fullNor=SkinDir+"\\"+strNormal;
			string fullUnder=SkinDir+"\\"+strUnder;

			// ��ڸ��J�ϧ�
			System.IO.FileStream  rPlayn=new System.IO.FileStream(fullNor,System.IO.FileMode.Open);
			ButtonImage[0]=new System.Drawing.Bitmap(rPlayn);
			System.IO.FileStream  rPlayu=new System.IO.FileStream(fullUnder,System.IO.FileMode.Open);
			ButtonImage[1]=new System.Drawing.Bitmap(rPlayu);
			// end of ��ڸ��J�ϧ�
			
			rPlayu.Close(); // ������y    // 8/17/2005
			rPlayn.Close(); // ������y 
		}


		// bMask  == true ��ܤ��e, false ��� �I��
		private bool[][] createMask(string full,byte bR,byte bG,byte bB){
			bool[][] bMask=null;
			try{
				Bitmap Image=new Bitmap(full); // �ثe���J�v��

				// �t�m mask �O����
				bMask=new bool[Image.Height][];
				for(int y=0;y<Image.Height;y++)
					bMask[y]=new bool[Image.Width];

				// ���I�����
				for(int y=0;y<Image.Height;y++){
					for(int x=0;x<Image.Width;x++){
						Color c=Image.GetPixel(x,y);
						bool isMask=c.R==bR && c.B==bB && c.G==bG;
						if(isMask){
							bMask[y][x]=false;
						}
						else{
							bMask[y][x]=true;
						}
					}
				}
			
			}catch(Exception ee){
				MessageBox.Show(full+" �i�ण�s�b\n�ԲӸ�T: "+ee.ToString());	
			}

			return bMask;


		}

		
		


		private Region getRegion(bool[][] bImage ){
			// �Q�� �p��� �إ� Region
			GraphicsPath myGraphPath=new GraphicsPath();
			int H=bImage.Length ;
			int W=bImage[0].Length;

			for(int y=1;y<H-1;y++){
				for(int x=1;x<W-1;x++){
					bool bLeftEdge=bImage[y][x-1]==true && bImage[y][x];
					bool bRightEdge=bImage[y][x]==true && bImage[y][x+1];
					bool bTopEdge=bImage[y-1][x]==true && bImage[y][x];
					bool bBottomEdge=bImage[y][x]==true && bImage[y+1][x+1];

					if(bLeftEdge || bRightEdge || bTopEdge || bBottomEdge){
						Rectangle R=new Rectangle(x,y,1,1);
						myGraphPath.AddRectangle(R);
					}
				}
			}

			// create the region
			Region myRegion=new Region(myGraphPath);
			return myRegion;
		}
	}
}
