using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

// �ϥ� DockUtility �l�޵������Ϊ��� -- �Ъ����Ѧ� DockUtility.cs 


namespace Player {
	/// <summary>
	/// Summary description for IndexListForm.
	/// </summary>
	public class IndexListForm : System.Windows.Forms.Form {
		
		// �y���B�z������
		public System.Collections.ArrayList LastTextList=new  System.Collections.ArrayList(); // �ϥΪ̿�J���޸�Ʀ�
		private int curTextIndex=-1; // �ثe���ަ�m

        public Point oldLocation; // �ثe������m (�B�z��������j�Y�p��, �l�����������H�J�H�X)

		public AVPlayer myAVPlayer=null;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.Button button2;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		public System.Windows.Forms.ListBox listBox1;
		private System.Windows.Forms.Button button3;
		private System.Windows.Forms.Button button4;

		private bool bFirst=true;
		private string Filename;
		string curAudioFileName;
		string strLoadDir,curAudioFullFileName,strFullLoadFileName;
		string strSaveDir,strFullSaveFileName;

		public DockUtility myDockHelp=new DockUtility(30); // �l�޵������Ϊ���
		
		static int CommandsTotalNum=4;
		string[] strSeekTime_End_Repeat_Comment=new string[CommandsTotalNum]; // �x�s�ثe���O�C����T
		public int RepeatCount=0;
		double curSeekTime=0;
		private System.Windows.Forms.TextBox textBox1;
		private System.Windows.Forms.Button button5;
		private System.Windows.Forms.CheckBox checkBox1_SectionAutioRepeat;
		private System.Windows.Forms.Button button6;
		private System.Windows.Forms.Button button7;
		public System.Windows.Forms.CheckBox cbAllRepeat;
		private System.Windows.Forms.Button button8;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		double curEndTime=0;

        public void UpdateFileData(string Filename)
        {

            curAudioFullFileName = Filename;
            curAudioFileName = curAudioFullFileName.Substring(curAudioFullFileName.LastIndexOf("\\") + 1);

            // �]�w�y�������ɪ��s�ɦ�m
            // ���ˬd�y���ɦ�m�O�_�i�H�s��, �Y���檺�ܤ~�]�w�s�ɦ�m���M�ץؿ�
            string curAudioFileDir = curAudioFullFileName.Substring(0, curAudioFullFileName.Length - curAudioFileName.Length - 1);

            //System.IO.Directory curAudioFileDirObj = System.IO.Directory.GetParent(curAudioFullFileName);



            strLoadDir = this.myAVPlayer.ExecutationFileDir;

            strLoadDir = curAudioFileDir;
            strFullLoadFileName = strLoadDir + "\\" + curAudioFileName;

            strSaveDir = strLoadDir;
            strFullSaveFileName = strSaveDir + "\\" + curAudioFileName + "_EZ.txt";


            this.Text = curAudioFileName + ".txt";
            Filename = this.Text;
        }

        // �y������
        public void IndexListFormLanguage(){
            this.label1.Text = this.myAVPlayer.L_Manager.getMessage("IndexListForm_System_Start"); // this.label1.Text = "�_�l
            this.label2.Text = this.myAVPlayer.L_Manager.getMessage("IndexListForm_System_End"); // this.label1.Text = "����:";

            // ToolTips

            string[] IndexListFormToolTipsString ={
                myAVPlayer.L_Manager.getMessage("IndexListForm_Tool_ModifySection"),
                myAVPlayer.L_Manager.getMessage("IndexListForm_Tool_SectionAutoRepeat"),
                myAVPlayer.L_Manager.getMessage("IndexListForm_Tool_RepeatPlay"),
                myAVPlayer.L_Manager.getMessage("IndexListForm_Tool_Delete"),
                myAVPlayer.L_Manager.getMessage("IndexListForm_Tool_Sorting"),
                myAVPlayer.L_Manager.getMessage("IndexListForm_Tool_DeleteAllSection"),
                myAVPlayer.L_Manager.getMessage("IndexListForm_Tool_OpenLesteningBombWindow"),
                myAVPlayer.L_Manager.getMessage("IndexListForm_Tool_AddnewSection")
            };

            toolTip1.SetToolTip(this.textBox1, IndexListFormToolTipsString[0]);// "�ק�y�����q ");
            toolTip1.SetToolTip(this.checkBox1_SectionAutioRepeat, IndexListFormToolTipsString[1]);// "�y�����q�۰ʴ`�� (�q�Ĥ@�q�}�l����)");
            toolTip1.SetToolTip(this.cbAllRepeat, IndexListFormToolTipsString[2]);// "�L�����Ƽ���");
            toolTip1.SetToolTip(this.button6, IndexListFormToolTipsString[3]);//"�R��->�ثe�����q [Del]");
            toolTip1.SetToolTip(this.button5, IndexListFormToolTipsString[4]);//"�Ƨ�->�y�����q(�Ѥp��j)");
            toolTip1.SetToolTip(this.button3, IndexListFormToolTipsString[5]);//"�R���Ҧ������q");
            toolTip1.SetToolTip(this.button2, IndexListFormToolTipsString[6]);//"�}��ť�O�F������");
            toolTip1.SetToolTip(this.button7, IndexListFormToolTipsString[7]);//"�s�W->�ثe���y�����q");

        }
        ToolTip toolTip1 = null;
		public IndexListForm(AVPlayer myAVPlayer,string filename) {
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

            // �M�׬���
            this.myAVPlayer = myAVPlayer;



            // ��s���񭵼��ɮ�, �����@�֧�s
            // this.myAVPlayer.ofdOpen.FileName
            UpdateFileData(filename);


			// �[�J ToolTips
			toolTip1 = new ToolTip();
			toolTip1.AutoPopDelay = 5000;// Set up the delays for the ToolTip.
			toolTip1.InitialDelay = 1000;
			toolTip1.ReshowDelay = 500;
			toolTip1.ShowAlways = true;// Force the ToolTip text to be displayed whether or not the form is active.
      
			// Set up the ToolTip text for the Button and Checkbox.



			// Repeat ����
			string strSectionAutoRepeat=(string)this.myAVPlayer.PropertyTable["SectionAutoRepeat"]; // �y�����q�۰ʼ��� (�̫�@���y�����q���񧹲�,�۰ʱq�Ĥ@���}�l����)
			bool bstrSectionAutoRepeat=bool.Parse(strSectionAutoRepeat);
			this.checkBox1_SectionAutioRepeat.Checked=bstrSectionAutoRepeat;
			


            // �y������
            IndexListFormLanguage();
		}

        // �N Section ���O�M��
        // �]��ť�O�F���������ɼ����, �ݭn��e�@���ɮת����O�M��
        public void ResetSectionInfo()
        {
           // this.ClearList(); // �M�����O��C
            ClearCurSectionInfo(); // �M���ثe���O 
           
        }

        // �M���ثe���O 
        public void ClearCurSectionInfo()
        {  
            for (int i = 0; i < CommandsTotalNum; i++)
            {
                strSeekTime_End_Repeat_Comment[i] = null;
            }
        }
        
		public string[] getCurSectionInfo(){
			return strSeekTime_End_Repeat_Comment;
		}

		public bool hasEndInfo(){
			if(strSeekTime_End_Repeat_Comment[1]==null)
				return false;
			else{
				return true;
			}
		}

		public bool hasRepeatInfo(){
			if(strSeekTime_End_Repeat_Comment[2]==null)
				return false;
			else{
				return true;
			}
		}

		public bool hasCommentInfo(){
			if(strSeekTime_End_Repeat_Comment[3]==null)
				return false;
			else{
				return true;
			}
		}
		public string getCommentInfo(){
			return strSeekTime_End_Repeat_Comment[3];
		}

		// ���o�ثe�t�ί��ު��A
		public double getCurSeekTime(){
			return curSeekTime;
		}

		public double getCurEndtime(){
			return curEndTime;
		}

		// ���� Repeat
		public int RepeatCountDown(){
			RepeatCount--;
			return RepeatCount;
		}


		public static bool CheckCommand(string SectionCommand){
			double[] retData_Seek_End_Repeat=new double[IndexListForm.CommandsTotalNum];
			string[] SectionInfo_Seek_End_Repeat=new string[IndexListForm.CommandsTotalNum];
			return CheckCommand(SectionCommand,retData_Seek_End_Repeat,SectionInfo_Seek_End_Repeat);
		}	

		public static bool CheckCommand(string SectionCommand,double[] retData_Seek_End_Repeat,string[] strSeekTime_End_Repeat_Comment){
			string[] SectionInfo_Seek_End_Repeat=strSeekTime_End_Repeat_Comment;
			Parse_getSeekTime_End_Repeat(SectionCommand,SectionInfo_Seek_End_Repeat);
			double dSeekTime=0,dEndTime=0,dRepeatCount=0;
			try{
				dSeekTime=double.Parse(SectionInfo_Seek_End_Repeat[0]);
				if(dSeekTime<0) // SeekTime ����O�t��
					return false;

				if(SectionInfo_Seek_End_Repeat[1]!=null){
					dEndTime=double.Parse(SectionInfo_Seek_End_Repeat[1]);
					if(dEndTime<=dSeekTime) // End Time ����� Start Seek time �٦�
						return false;
				}

				if(SectionInfo_Seek_End_Repeat[2]!=null){
					dRepeatCount=int.Parse(SectionInfo_Seek_End_Repeat[2]);
					if(dRepeatCount<0){ 	// Repeat ����O�t��
						return false;
					}
				}
			}catch(Exception ){
				return false;
			}
			retData_Seek_End_Repeat[0]=dSeekTime;
			retData_Seek_End_Repeat[1]=dEndTime;
			retData_Seek_End_Repeat[2]=dRepeatCount; 

			return true;
		}
		
		public bool UpdateSeekTime_End_Repeat(string SectionCommand){
			curSeekTime=0;curEndTime=0;RepeatCount=0;

			double[] retData_Seek_End_Repeat=new double[IndexListForm.CommandsTotalNum];

			// �ˬd Command �O�_���T
			bool bOk= CheckCommand(SectionCommand,retData_Seek_End_Repeat,strSeekTime_End_Repeat_Comment); 
		
			if(bOk){
				// Update ���
				curSeekTime=retData_Seek_End_Repeat[0];
				curEndTime=retData_Seek_End_Repeat[1];
				RepeatCount=(int)retData_Seek_End_Repeat[2];
			}

			return bOk;


			/*

			Parse_getSeekTime_End_Repeat(SectionCommand,strSeekTime_End_Repeat_Comment);
			curSeekTime=double.Parse(strSeekTime_End_Repeat_Comment[0]);

			if(strSeekTime_End_Repeat_Comment[1]!=null){
				curEndTime=double.Parse(strSeekTime_End_Repeat_Comment[1]);
				// End Time ����� Start Seek time �٦�
				if(curEndTime<=curSeekTime)
					return false;
			}

			if(strSeekTime_End_Repeat_Comment[2]!=null){
				RepeatCount=int.Parse(strSeekTime_End_Repeat_Comment[2]);
				// Repeat ����O�t��
				if(RepeatCount<0){
					return false;
				}
			}

			return true;
			*/
			
		}


		

		
	
		// ��ϥΪ� �� tab ������ list ������, �D�����]�n�@�ֱa�X��
		
		
		/*
				public void FirstRun(){
					this.Opacity=100; // �z���� 0;   (�@�}�l�O�ݤ�����) 
					this.Show(); // ��ܵ���
				}

				public void InitialListBox(){

			
				}
				*/

		// �H�J
		public void  FadeIn(){
			
			//  �p�G�b�@�}�l�{���إ߮�, �N�]�w�z���� =0 , �h�N�ӴN��O�]�w�z���� =100 , ��]�t�����󪺳z���ױN���|��^�� [Bug]?
			if(bFirst==true){  
				this.Opacity=0.0; // �z���� 0;   (�@�}�l�O�ݤ�����) 
				
				this.Show(); // ��ܵ��� (�@�w�n��ܵ���,�~����ܦ�m)
				bFirst=false;

                /* menu ���D�w�g�ѨM, �D�n����]�b��: �� skin ��, �èS���Ҽ{�P�ɭ��s�]�w ����������.  �Ѧ�: AudioVideoPlayer:: LoadSkin method
                int MainMenuHeight=0;
                // �Ҽ{�� menu ������
                if(myAVPlayer.Menu==null){
                    MainMenuHeight=32;
                }else{
                    MainMenuHeight=0;
                }
                // end of menu ������

                // this.Location=new System.Drawing.Point(myAVPlayer.Left,myAVPlayer.Top+myAVPlayer.Height-MainMenuHeight);
                 */
                OpacityUtility myOpacityObj = new OpacityUtility();
                myOpacityObj.FadeIn_Only(this, 20);// ������ܥX��
				this.listBox1.Focus();
			}else{
                if (this.Opacity == 0)
                { // �����z����,�~�ʧ@
                    OpacityUtility myOpacityObj = new OpacityUtility();
                    myOpacityObj.FadeIn_Only(this, 20);// ������ܥX��
                }
			}
		}

		// �H�X
        public void FadeOut(int Interval)
        {
			if(this.Opacity==1) // ������ܮ�, �~�ʧ@
                OpacityUtility.FadeOut_Only(this, Interval); // �����ܳz��
		}
        public void FadeOut()
        {
            FadeOut(30);
        }
		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing ) {
			
			LastTextList=null;

			if( disposing ) {
				if(components != null) {
					components.Dispose();
				}
			}
			base.Dispose( disposing );

		}

		// �[�J�s�� item ����ަ�C��
		public bool bModify=false;
		public bool AddItem(String strIndex){
			if(checkExist(strIndex)!=true){
				// ��̷s�����w��ưO���_��
				LastTextList.Add(strIndex);  // �x�s reference �٬O �s������ ?
				curTextIndex=LastTextList.Count-1;  // �]�w���̫�@����ƪ� index, ���W��|���ޤW�@�ӿ�J

				this.listBox1.Items.Add(strIndex); // �]�w List ���
				this.listBox1.SelectedIndex=curTextIndex;
				
				if(this.Text.EndsWith("*")!=true){
					this.Text=this.Text+"*";
				}
				bModify=true;
				this.button1.Visible=true;

				return true;  // �[�J item ���\
			}
			return false; // ��C���w�g�s�b item, �^�ǥ���
		}// end of �[�J�s�� item

		

		// �s�� item
		public bool EditItem(String strIndex){
			// �ˬd  Command �O�_�X��
			bool bOk=CheckCommand(strIndex);
			if(bOk){
				LastTextList[this.listBox1.SelectedIndex]=this.textBox1.Text;// �ܧ��ƥD��
				this.listBox1.Items[this.listBox1.SelectedIndex]=this.textBox1.Text; // �ܧ����
			
				SetModify();
				return true;  // �[�J item ���\		
			}else{
				return false;
			}
		}// end of �s�� item

		public void SetModify(){
			if(this.Text.EndsWith("*")!=true){
				this.Text=this.Text+"*";
			}
			bModify=true;
			this.button1.Visible=true;
		}

		//�V�W����
		public void MoveUP(){
			curTextIndex--;
			if(curTextIndex<0){
				if(LastTextList.Count >0){
					curTextIndex=0;
					myAVPlayer.SeekPlay();
				}
				else
					curTextIndex=-1;
				myAVPlayer.label2.Text="��Ƴ̳���";

                myAVPlayer.myFileListForm.PlayThePreviouseFile();
			}
			else{ 
				/*
				string strNum=(string) LastTextList[curTextIndex]; // �]�w�n�n���񪺫��w���;
				myAVPlayer.textBox1.Text=strNum;
				myAVPlayer.SeekPlay();// �i�漽��
				myAVPlayer.label3.Text="���ި� "+strNum;
				myAVPlayer.label2.Text="[Enter] �Ať�@��";
				*/

				this.listBox1.SelectedIndex=curTextIndex; // �|�ް_��ܧ��ܨƥ�
			}
		}// end of �V�W����
		
		// �������ަ�m
		public void Seek(int i){
			if(i<LastTextList.Count){
				curTextIndex=i;
				this.listBox1.SelectedIndex=curTextIndex;  // �|�ް_��ܧ��ܨƥ�
			}
		}
		public void InitialSelect(){
			curTextIndex=-1;
		}
		public bool hasNext2(){

            if (CurrentPlayIndex >= LastTextList.Count - 1)
			// if(curTextIndex>=LastTextList.Count-1)
				return false;
			else
				return true;
		}

		// �O�_�٦��U�@�����
		public bool hasNext(){
			if(curTextIndex+1>LastTextList.Count)
				return false;
			else
				return true;
		}

		// ���ܺq�����
		public void ChangeToSelect(){
			this.listBox1.SelectedIndex=curTextIndex;  // �|�ް_��ܧ��ܨƥ�
		}
		
		// ���ި�Ĥ@�Ӥ��q
		public bool MoveFirst(){
			// �ˬd�O�_���Ĥ@�����
			if(this.LastTextList.Count>0){
				myAVPlayer.label2.Text="�Ĥ@�����";
				curTextIndex=0;
				if(this.listBox1.SelectedIndex!=curTextIndex){
					this.listBox1.SelectedIndex=curTextIndex;// �|�ް_��ܧ��ܨƥ�
				}else{
					// �� list ���u���@����Ʈ�, curTextIndex �|�P this.listBox1.SelectedIndex �ȬۦP
					// �o�ɭP �����]�w this.listBox1.SelectedIndex ���|�޵o ��ܧ��ܨƥ�
					// �ҥH�� �o�ɴN�n�����i�����
					SetupAudioSection();
				}
				return true;
			}else{
				// �S�����, ���ʧ@
				return false;
			}
				
		}

        // �U���S������y�����q���B�z
        public void MoveDown_SectionNoMoreNext()
        {
            myAVPlayer.label2.Text = "�̫�@�����"; // ��̫ܳ�@�����
            myAVPlayer.myFileListForm.PlayTheNextFile(); // �Y�ثe�y�����q�w�g���񧹲�, �м���U�@���ɮת��y�����q.
        }

		// �V�U����
		public void MoveDown(){
			curTextIndex++;

            // �Y�U���w�g�S��������
			if(hasNext()!=true){
                curTextIndex = LastTextList.Count - 1; // �Y�W�L�̫�@��, �h���ʫ���
                MoveDown_SectionNoMoreNext();
			}else{
				ChangeToSelect();  // �|�ް_��ܧ��ܨƥ�
			}
		}// end of �V�U����

		// �M�ů��ަ�C
		public void ClearList(){
			ClearList(false);
		}
		public void ClearList(bool bShowMessage){
			if(bShowMessage){
				string message = "�M�ů��ަ�C ?";
				string caption = "�`�N�ƶ�";
				MessageBoxButtons buttons = MessageBoxButtons.YesNo;
				DialogResult result;

				result = MessageBox.Show(this, message, caption, buttons,
					MessageBoxIcon.Question);
				if(result==DialogResult.No){   // �ϥΪ̤��Q�M�ů��ަ�C
					return;
				}
			}

			curTextIndex=-1;
			LastTextList.Clear();
			this.listBox1.Items.Clear();
			if(this.Text.EndsWith("*")!=true){
				this.Text=this.Text+"*";
			}

			this.textBox1.Text="";
		}// end of �M�ů��ަ�C

		// �O�_�n�x�s���ަ�C
		public bool AskSaveList(){
			string message = "�O�_�n�x�s���ަ�C ?";
			string caption = "���ަ�C�w�g�ק�";
			MessageBoxButtons buttons = MessageBoxButtons.YesNo;
			DialogResult result;

			result = MessageBox.Show(this, message, caption, buttons,
				MessageBoxIcon.Question);
			if(result==DialogResult.No){   // �ϥΪ̤��Q�M�ů��ަ�C
				return false;
			}else{
				return true;
			}
		}// end of �x�s���ަ�C
		

		public string GetSeekTime(string Command){
			string[] SectionInfo_Seek_End_Repeat=new string[IndexListForm.CommandsTotalNum];
			Parse_getSeekTime_End_Repeat(Command,SectionInfo_Seek_End_Repeat);
			return SectionInfo_Seek_End_Repeat[0];
		}

		// �ˬd Seek Time �O�_�w�g�s�b
		public bool checkExist_SeekTime(string strNum){
			for(int i=0;i<LastTextList.Count;i++){
				string Text=(string) LastTextList[i];
				string SeekTime_Item=GetSeekTime(Text);
				string SeekTime_strNum=GetSeekTime(strNum);
				if(SeekTime_Item.CompareTo(SeekTime_strNum)==0){
					return true;
				}
			}
			return false;
		}

		// �ˬd item �O�_�s�b
		public bool checkExist(string strNum){
			for(int i=0;i<LastTextList.Count;i++){
				string Text=(string) LastTextList[i];
				if(Text.CompareTo(strNum)==0){
					return true;
				}
			}
			return false;
		}
		
		public bool checkExist(string strNum,int[] index){
			for(int i=0;i<LastTextList.Count;i++){
				string Text=(string) LastTextList[i];
				if(Text.CompareTo(strNum)==0){
					index[0]=i;
					return true;
				}
			}
			return false;
		}// end of �ˬd item �O�_�s�b
	
		// ������C�����y������
		public bool RemoveItem(string strNum){
			int[] index=new int[1];
			if(checkExist(strNum,index)){
                // �ߧY�R�����
				LastTextList.RemoveAt(index[0]); // ��C������
				this.listBox1.Items.RemoveAt(index[0]);  // ��ܪ�����

                // �վ�U�@�ӯ���
                int newIndex = index[0] - 1;
                if (newIndex < 0 && this.listBox1.Items.Count>0)
                    newIndex = 0;
                this.curTextIndex = newIndex;
                CurrentPlayIndex = curTextIndex;
				this.listBox1.SelectedIndex=curTextIndex; // ��ܤW�@�Ӹ��

                //  �ߧY������w��m�����
                this.PlayCurrentSection2();

                // ��ܸ�Ƥw�g����
				if(this.Text.EndsWith("*")!=true){
					this.Text=this.Text+"*";
				}
				bModify=true;
				this.button1.Visible=true;
				return true;
			}else{
				return false;
			}
		}// end of ����

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		
		// System.EventHandler MoveHandler;
		private void InitializeComponent() {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(IndexListForm));
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.button5 = new System.Windows.Forms.Button();
            this.checkBox1_SectionAutioRepeat = new System.Windows.Forms.CheckBox();
            this.button6 = new System.Windows.Forms.Button();
            this.button7 = new System.Windows.Forms.Button();
            this.cbAllRepeat = new System.Windows.Forms.CheckBox();
            this.button8 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Image = ((System.Drawing.Image)(resources.GetObject("button1.Image")));
            this.button1.ImageAlign = System.Drawing.ContentAlignment.BottomRight;
            this.button1.Location = new System.Drawing.Point(280, 215);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(24, 21);
            this.button1.TabIndex = 1;
            this.button1.Visible = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Image = ((System.Drawing.Image)(resources.GetObject("button2.Image")));
            this.button2.Location = new System.Drawing.Point(280, 194);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(24, 21);
            this.button2.TabIndex = 2;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // listBox1
            // 
            this.listBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.listBox1.Location = new System.Drawing.Point(0, 0);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(304, 171);
            this.listBox1.TabIndex = 3;
            this.listBox1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.listBox1_MouseDoubleClick);
            this.listBox1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.listBox1_MouseDown);
            this.listBox1.SelectedValueChanged += new System.EventHandler(this.listBox1_SelectedValueChanged);
            this.listBox1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.listBox1_KeyDown);
            // 
            // button3
            // 
            this.button3.Image = ((System.Drawing.Image)(resources.GetObject("button3.Image")));
            this.button3.Location = new System.Drawing.Point(256, 215);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(24, 21);
            this.button3.TabIndex = 4;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button4
            // 
            this.button4.Enabled = false;
            this.button4.Location = new System.Drawing.Point(208, 229);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(16, 7);
            this.button4.TabIndex = 6;
            this.button4.Visible = false;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(8, 194);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(184, 20);
            this.textBox1.TabIndex = 7;
            this.textBox1.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            this.textBox1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBox1_KeyDown);
            // 
            // button5
            // 
            this.button5.Image = ((System.Drawing.Image)(resources.GetObject("button5.Image")));
            this.button5.ImageAlign = System.Drawing.ContentAlignment.BottomRight;
            this.button5.Location = new System.Drawing.Point(256, 194);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(24, 21);
            this.button5.TabIndex = 8;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // checkBox1_SectionAutioRepeat
            // 
            this.checkBox1_SectionAutioRepeat.Image = ((System.Drawing.Image)(resources.GetObject("checkBox1_SectionAutioRepeat.Image")));
            this.checkBox1_SectionAutioRepeat.Location = new System.Drawing.Point(192, 215);
            this.checkBox1_SectionAutioRepeat.Name = "checkBox1_SectionAutioRepeat";
            this.checkBox1_SectionAutioRepeat.Size = new System.Drawing.Size(32, 21);
            this.checkBox1_SectionAutioRepeat.TabIndex = 9;
            this.checkBox1_SectionAutioRepeat.Visible = false;
            this.checkBox1_SectionAutioRepeat.CheckedChanged += new System.EventHandler(this.checkBox1_SectionAutioRepeat_CheckedChanged);
            // 
            // button6
            // 
            this.button6.Image = ((System.Drawing.Image)(resources.GetObject("button6.Image")));
            this.button6.Location = new System.Drawing.Point(232, 215);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(24, 21);
            this.button6.TabIndex = 10;
            this.button6.Click += new System.EventHandler(this.button6_Click);
            // 
            // button7
            // 
            this.button7.Image = ((System.Drawing.Image)(resources.GetObject("button7.Image")));
            this.button7.Location = new System.Drawing.Point(232, 194);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(24, 21);
            this.button7.TabIndex = 12;
            this.button7.Click += new System.EventHandler(this.button7_Click);
            // 
            // cbAllRepeat
            // 
            this.cbAllRepeat.Font = new System.Drawing.Font("PMingLiU", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.cbAllRepeat.Image = ((System.Drawing.Image)(resources.GetObject("cbAllRepeat.Image")));
            this.cbAllRepeat.Location = new System.Drawing.Point(192, 194);
            this.cbAllRepeat.Name = "cbAllRepeat";
            this.cbAllRepeat.Size = new System.Drawing.Size(32, 21);
            this.cbAllRepeat.TabIndex = 13;
            this.cbAllRepeat.CheckStateChanged += new System.EventHandler(this.cbAllRepeat_CheckStateChanged);
            // 
            // button8
            // 
            this.button8.Image = ((System.Drawing.Image)(resources.GetObject("button8.Image")));
            this.button8.Location = new System.Drawing.Point(8, 215);
            this.button8.Name = "button8";
            this.button8.Size = new System.Drawing.Size(24, 21);
            this.button8.TabIndex = 14;
            this.button8.Click += new System.EventHandler(this.button8_Click);
            // 
            // label1
            // 
            this.label1.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.label1.Location = new System.Drawing.Point(48, 215);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(56, 14);
            this.label1.TabIndex = 15;
            this.label1.Text = "�_�l:";
            // 
            // label2
            // 
            this.label2.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.label2.Location = new System.Drawing.Point(120, 215);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(56, 14);
            this.label2.TabIndex = 16;
            this.label2.Text = "����:";
            // 
            // IndexListForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(306, 278);
            this.ControlBox = false;
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button8);
            this.Controls.Add(this.cbAllRepeat);
            this.Controls.Add(this.button7);
            this.Controls.Add(this.button6);
            this.Controls.Add(this.checkBox1_SectionAutioRepeat);
            this.Controls.Add(this.button5);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.listBox1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.HelpButton = true;
            this.Name = "IndexListForm";
            this.ShowInTaskbar = false;
            this.Text = "�ϥΪ̿�J���ެ���";
            this.Deactivate += new System.EventHandler(this.IndexListForm_Deactivate);
            this.Load += new System.EventHandler(this.IndexListForm_Load);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.IndexListForm_MouseUp);
            this.Activated += new System.EventHandler(this.IndexListForm_Activated);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.IndexListForm_MouseDown);
            this.Move += new System.EventHandler(this.IndexListForm_Move);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.IndexListForm_KeyDown);
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion

		private void button1_Click(object sender, System.EventArgs e) {
			SaveList();
			this.myAVPlayer.label2.Text="���޸���x�s���\";

			this.button1.Visible=false;
			this.myAVPlayer.textBox1.Focus();
			//button2_Click(sender,e); // ��������
		}


        /* �{���ܼưѦҸ��
			string strLoadDir=this.myAVPlayer.ExecutationFileDir;
			string curAudioFullFileName=this.myAVPlayer.ofdOpen.FileName;
			string curAudioFileName=curAudioFullFileName.Substring(curAudioFullFileName.LastIndexOf("\\")+1);
			string strFullLoadFileName=	strLoadDir+	curAudioFileName+".txt";
        */
        // ���J��C��� (���ˬd�O�_�b�y���ɥؿ�, �Y���b�h�}�ұM���ɥؿ������y��������)
		public void LoadList(){
			// �}�һy���ɥؿ��Ҧb��m, �O�_��������
            if (System.IO.File.Exists(strFullLoadFileName+"_EZ.txt") == true) {
                LoadList(strFullLoadFileName+"_EZ.txt");
            }
            else {
                // �ˬd�O�_���ª��y�������ɪ����s�b
                if (System.IO.File.Exists(strFullLoadFileName + ".txt") == true) {
                    LoadList(strFullLoadFileName + ".txt");
                }
                else {
                    // �ˬd�M�ץؿ���,�O�_�s�b�y��������
                    // �}�ұM���ɥؿ������y��������
                    strLoadDir = this.myAVPlayer.ExecutationFileDir;
                    strFullLoadFileName = strLoadDir + "\\" + curAudioFileName + "_EZ.txt";
                    LoadList(strFullLoadFileName);
                }
            }
		}

        private string EatBlank(string line) {
            string command;
            int i;
            for (i = 0; i < line.Length; i++) {
                if (line[i] != ' ')
                    break;
            }
            
            command = line.Substring(i);
            return command;
        }
        private void LoadList(string FullFilename) {
           
            LastTextList.Clear(); // LastTextList�����C��ƲM��
            this.listBox1.Items.Clear();
            this.curTextIndex = -1;

            
            // ���ɮפ�, ���J property
            try {
                using (System.IO.StreamReader sr = new System.IO.StreamReader(FullFilename)) {
                    String line;
                    while ((line = sr.ReadLine()) != null) {

                        string strcommand = EatBlank(line);

                        // �Y line �O ����ť�, �h strcommand ���׷|���� 0
                        if (strcommand.Length == 0)
                            continue;

                        // �Y�D�J����, �h���L
                        if (strcommand.StartsWith("#")) { 
                            continue;
                        }

                        
                        LastTextList.Add(line);
                        this.listBox1.Items.Add(line);
                    }
                    // MessageBox.Show("before");
                    //this.Text = this.Filename;
                }
            }
            catch (Exception e) {
                // Let the user know what went wrong.
                Console.WriteLine("�ݩ��ɸ��J����");
                Console.WriteLine(e.Message);
            }

          //  MessageBox.Show("2");
        }

        // �s�ɤ����{��
        // �w�]�ؿ����s,�p�G�O��Ū�C��, �h�s��M�ץؿ���
		public void SaveList(){
            try {
                SaveList(strFullSaveFileName);
            }
            catch (Exception e) {
                // �o�ͨҥ~, ��s��M�ץؿ���
                strSaveDir = this.myAVPlayer.ExecutationFileDir; 
                strFullSaveFileName = strSaveDir + "\\" + curAudioFileName + "_EZ.txt";
                SaveList(strFullSaveFileName);
            }
			
		}

        // �s�ɮ֤ߵ{��
        private void SaveList(string FullFilename){
          
            // �s��
            if (System.IO.File.Exists(FullFilename) == true) {
                bool oldWindowOrder = this.myAVPlayer.TopMost;
               // this.myAVPlayer.TopMost = false;

                string message = "�O�_�n�л\ " + strFullSaveFileName + " ?";
                string caption = "�`�N�ƶ�";
                MessageBoxButtons buttons = MessageBoxButtons.YesNo;
                DialogResult result;

                result = MessageBox.Show(this, message, caption, buttons,
                    MessageBoxIcon.Question);

               // this.myAVPlayer.TopMost = oldWindowOrder;

                if (result == DialogResult.No) {   // �ϥΪ̤��Q�s��
                    return;
                }
            }

            // ���� list �s���ɮפ�
            using (System.IO.StreamWriter sw = new System.IO.StreamWriter(FullFilename)) {// �إ߷s�� property �ɮ�
                sw.WriteLine("# �o�O EZLearn �y���ǲ߾����y��������");
                sw.WriteLine("# �M�׺��}: http://mqjing.twbbs.org.tw/~ching/Course/JapaneseLanguageLearner/__page/JapaneseLanguageLearner.htm");
                sw.WriteLine("# �����ɷj�M����: (1) �y���ɩҦb�ؿ�   (2) �Y�j�M����, �~�|�j�M EZLearn.exe �����ɩҦb�ؿ�");
                sw.WriteLine("#                                             ������T: " + AVPlayer.strVersion_subNo);
                sw.WriteLine("             ");
                for (int i = 0; i < LastTextList.Count; i++) {
                    string strIndexValue = (string)LastTextList[i];
                    string WriteString = strIndexValue;
                    sw.WriteLine(WriteString);
                }
            }

           
            this.Filename = Utility.getShortFilename(FullFilename);
            this.Text = this.Filename;
            bModify = false;
            // end of �s��
        }

        // �}��ť�O�F������
        // bool bShowOrCloseFileListForm = true;
		private void button2_Click(object sender, System.EventArgs e) {
            if (this.myAVPlayer.myFileListForm.bClosed == true)
            {
                //OpacityUtility.FadeOut_Only(this,30);
                this.myAVPlayer.myFileListForm.FadeIn();

                // �Yť�O�F�������O������, �h���}, �åB���s�վ��m. �]���������i��w�g���ʦ�m�F
                if (this.myAVPlayer.myFileListForm.bClosed == true)
                {
                    this.myAVPlayer.myFileListForm.bClosed = false;
                    this.myAVPlayer.myFileListForm.SetDefaultLocation(); // ���ʨ�w�]��m
                }
                
                // bShowOrCloseFileListForm = true;
            }
            else
            {
                // ����ť�O�F������
                this.myAVPlayer.myFileListForm.FadeOut();
                this.myAVPlayer.myFileListForm.bClosed = true;
                // bShowOrCloseFileListForm = false; // �аO�ثe�O���_��
            }

            //this.myAVPlayer.Focus();
		}

		
		
		private void IndexListForm_Move(object sender, System.EventArgs e) {
            // �]�� Minmin �|�� x, y <0 , �ҥH�ڭ̥u�O�� Normal ���A������
            if (Location.X >= 0 && Location.Y >= 0)
            {
                oldLocation = this.Location;


                if (this.myAVPlayer.myFileListForm.MyMother == this)
                {
                    if (this.myAVPlayer.myFileListForm != null && this.myAVPlayer.myFileListForm.myDockHelp.isConnected)
                    {
                        int movY = this.Location.Y + myAVPlayer.myFileListForm.myDockHelp.DMY;
                        int movX = this.Location.X + myAVPlayer.myFileListForm.myDockHelp.DMX;
                        myAVPlayer.myFileListForm.Location = new System.Drawing.Point(movX, movY);
                    }
                }
            }
		}




		private void IndexListForm_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e) {
			int kk=0;
			kk++;
		}

		
		int lastIndex=-1;
        // �P JingTextEdit1.cs ���� AllowCode method ����
		private void listBox1_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e) {
            this.myAVPlayer.bMySectionBasedPlay = true; // �Ұʻy�����q����¦������
			switch(e.KeyCode){
					// 06/29/2005 Bug �ץ� (�� list box ���W�U��, �|���ʨ⦸�� bug)
					// ��]: listbox �|�۰ʳB�z UP �P Down, �o�޵o�F value changed �ƥ�
					//       �i�ӾɭP curTextIndex �[���ƨ⦸
				case Keys.Up:
                    
                    this.PlayPreviousSection();
                    //System.Console.WriteLine("CurrentPlayIndex=" + CurrentPlayIndex);
                    /*
					// this.MoveUP(); //�b list box ���B�z: �]���W�U�� list �۰ʷ|�B�z, �ڭ̥u�n�b changed value ���ܧY�i
					if(curTextIndex==lastIndex){
						myAVPlayer.SeekPlay();
						myAVPlayer.label2.Text="��Ƴ̳���";
                        myAVPlayer.myFileListForm.PlayThePreviouseFile();
					}
					lastIndex=curTextIndex;
                     * */
					break;
				case Keys.Down:
                    
                    this.PlayNextSection();
                    //System.Console.WriteLine("CurrentPlayIndex=" + CurrentPlayIndex);
                    /*
                    // �ƥ�޵o�y�{:
                    // �b listBox ���U��, �|���޵o Keys.Down �ƥ�, 
                    // �p�G���u�����U��, �N�|�޵o value changed �ƥ�

                    // �Y�ثe���ޤw�g�O�̫�@�ӤF, �o�ɨϥΪ̤S���U "�U" ��
                    // �h����U�@���ɮת��Ĥ@�y�����q
                    if (this.listBox1.SelectedIndex == this.listBox1.Items.Count - 1)
                    {
                        MoveDown_SectionNoMoreNext();
                    }
					*/
                    /*
					if(curTextIndex==lastIndex){ // �Y�W�L�̫�@��, �h���ʫ���
						myAVPlayer.label2.Text="�̫�@�����";
						myAVPlayer.SeekPlay();

                        myAVPlayer.myFileListForm.PlayTheNextFile();
					}
					lastIndex=curTextIndex;
                     */
					break;
					
				case Keys.Delete:
                    if (CurrentPlayIndex >= 0)
                    {
                        string strNum = (string)LastTextList[CurrentPlayIndex]; // �]�w�n�n���񪺫��w���;
						this.RemoveItem(strNum);
					}
					break;

				
				case Keys.Enter: 
					// myAVPlayer.SeekPlay();
                    PlayCurrentSection2();
					break;

                case Keys.ShiftKey:  // 20061213 �ץ� �b IndexList Form �W���U Shift ��L�Ī��\��
                    // myAVPlayer.label2.Text = "�Ať�@��";

                    MessageManager.ShowInformation(myAVPlayer.label2, this.myAVPlayer.L_Manager.getMessage("IndexListForm_System_ListenAgain"), 5000, false);
                    PlayCurrentSection2();
                    // myAVPlayer.SeekPlay();
                    break;
			}

            /*
			this.myAVPlayer.textBox1_KeyDown(sender,e); // �� Player �� textbox1 �o�X KeyDown ���ƥ�
			// ���� text ���󪺥\��
			string strnum="";
			
			bool bNum=true;
			switch(e.KeyCode){
				case Keys.D0:
					strnum="0";
					break;
				case Keys.D1:
					strnum="1";
					break;
				case Keys.D2:
					strnum="2";
					break;
				case Keys.D3:
					strnum="3";
					break;
				case Keys.D4:
					strnum="4";
					break;
				case Keys.D5:
					strnum="5";
					break;
				case Keys.D6:
					strnum="6";
					break;
				case Keys.D7:
					strnum="7";
					break;
				case Keys.D8:
					strnum="8";
					break;
				case Keys.D9:
					strnum="9";
					break;
				default:
					bNum=false;
					break;
			}

			if(bNum==true){
				try{
					int Num=(int)Double.Parse (myAVPlayer.textBox1.Text+strnum);
					this.myAVPlayer.textBox1.Text=""+Num;
				}catch(Exception){
				}
			}

			if(e.KeyCode==Keys.Back){
				// �R���̥k�䪺�Ʀr
				string strTarget=this.myAVPlayer.textBox1.Text;
				if(strTarget.Length>0){
					string newString=strTarget.Substring(0,strTarget.Length-1);
					this.myAVPlayer.textBox1.Text=newString;
				}
			}
			// end of ���� text ���󪺥\��
            */
			
		}

		// �_�l�I�� �r���������r��
		private static string getStart_Comma_String(int s,string SectionCommand,int[]retCommaPos){
			int CommaPos=SectionCommand.IndexOf(",",s);
			retCommaPos[0]=CommaPos;
			string strTimeInfo=SectionCommand.Substring(s,CommaPos-s);
			return strTimeInfo;
		}
		private static string getLastCommandString(int s, string SectionCommand){
			// ���o End Time
			string strCommand=SectionCommand.Substring(s);
			return strCommand;
		}

		public static double getSeekTime(string SectionCommand){
			string[] SectionInfo=new string[IndexListForm.CommandsTotalNum];
			Parse_getSeekTime_End_Repeat(SectionCommand,SectionInfo);
			double dSeekTime=double.Parse(SectionInfo[0]);
			return dSeekTime; 
		}


		// �ѫ��O Seek[,End][,Repeat][,Comment] �ѪR�X Seek Time
		public static void Parse_getSeekTime_End_Repeat(string SectionCommand,string[] SectionInfo){
			// �ˬd�{�b�쩳���X�� , ��
			int Num=0;
			for(int i=0;i<SectionCommand.Length;i++){
				if(SectionCommand[i]==','){
					Num++;
				}
			}

			int[] retCommaPos=new int[1];
			string strSeek=null,strEnd=null,strRepeat=null,strComment=null;



			
			string[] NewCommand=new string[1];
			switch(Num){
				case 0:
					// ��l�u�� Seek ����
					// ���O�Φ�= Seek
					string strNum=SectionCommand; // �]�w�n�n���񪺫��w���;
					SectionInfo[0]=strNum;
					// end of ��l����

					break;
				case 1:
					if(IsRelateMode(SectionCommand,NewCommand)){ // �ˬd�ثe��J�O�_�� Reverse Mode
						bool bHasRepeat=false;
						RelateModeProcess(SectionInfo,NewCommand[0],false,bHasRepeat);
						
					}else{
						// ���O�Φ�= Seek,End
						strSeek=getStart_Comma_String(0,SectionCommand,retCommaPos); // �_�l�I��r���������r��
						SectionInfo[0]=strSeek;

						// ���o End Time
						strEnd=getLastCommandString(retCommaPos[0]+1,SectionCommand); // ���o�̫�@�Ӧr��
						SectionInfo[1]=strEnd;
					}
					break;
				case 2:
					if(IsRelateMode(SectionCommand,NewCommand)){ // �ˬd�ثe��J�O�_�� Reverse Mode
						bool bHasRepeat=true;
						RelateModeProcess(SectionInfo,NewCommand[0],false,bHasRepeat);
						
					}else{
						// ���O�Φ�=  Seek,End, Repeat
						strSeek=getStart_Comma_String(0,SectionCommand,retCommaPos); // �_�l�I�� �r���������r��
						SectionInfo[0]=strSeek;

						strEnd=getStart_Comma_String(retCommaPos[0]+1,SectionCommand,retCommaPos); // �_�l�I�� �r���������r��
						SectionInfo[1]=strEnd;


						strRepeat=getLastCommandString(retCommaPos[0]+1,SectionCommand); // ���o�̫�@�Ӧr��
						SectionInfo[2]=strRepeat;
					}
					break;
				case 3:
					if(IsRelateMode(SectionCommand,NewCommand)){ // �ˬd�ثe��J�O�_�� Reverse Mode
						bool bHasRepeat=true;
						RelateModeProcess(SectionInfo,NewCommand[0],true,bHasRepeat);
						
					}else{
						// ���O�Φ�=  Seek,End, Repeat, Comment
						strSeek=getStart_Comma_String(0,SectionCommand,retCommaPos); // �_�l�I�� �r���������r��
						SectionInfo[0]=strSeek;

						strEnd=getStart_Comma_String(retCommaPos[0]+1,SectionCommand,retCommaPos); // �_�l�I�� �r���������r��
						SectionInfo[1]=strEnd;

						strRepeat=getStart_Comma_String(retCommaPos[0]+1,SectionCommand,retCommaPos); // �_�l�I�� �r���������r��
						SectionInfo[2]=strRepeat;

						strComment=getLastCommandString(retCommaPos[0]+1,SectionCommand); // ���o�̫�@�Ӧr��
						SectionInfo[3]=strComment;
					}
					break;
			}
			
			for(int i=Num+1;i<SectionInfo.Length;i++){
				SectionInfo[i]=null;
			}		
		}

		public static bool IsRelateMode(String strSeek,string[] NewCommand){
			int i;
			for(i=0;i<strSeek.Length;i++){
				if(strSeek[i]==',')
					return false;
				if(strSeek[i]==' ')
					continue;
				if(strSeek[i]=='R' || strSeek[i]=='r')
					break;
			}

			if(strSeek[i]=='r' || strSeek[i]=='R'){
				i++;
				for(;i<strSeek.Length;i++){
					NewCommand[0]=NewCommand[0]+strSeek[i];
				}
				return true;
			}else{
				return false;
			}
		}
   
		// ���o���`���p�� Seek, End, Repeat ��T
		public static void RelateModeProcess(string[] SectionInfo,string SectionCommand,bool bHasCommend,bool bHasRepeat){
			int[] retCommaPos=new int[1];
			// ���O�Φ�=  REnd,Back Second, Repeat
			// EX:
			//         R100,-10,3,�q��100���e�T��}�l����, �s��T��
			//         R20,+5,10  <--- �q 20 ��}�l, ���쩹�᤭�����q���� 10 ��
			
			string strStartup=getStart_Comma_String(0,SectionCommand,retCommaPos); // �_�l�I�� �r���������r��
			
			string strBack=null;
			if(bHasRepeat==true)
				strBack=getStart_Comma_String(retCommaPos[0]+1,SectionCommand,retCommaPos); // �_�l�I�� �r���������r��
			else
				strBack=getLastCommandString(retCommaPos[0]+1,SectionCommand); // ���o�̫�@�Ӧr��
			double iBack=Double.Parse(strBack);
			double iStart=Double.Parse(strStartup);
			double newSeek=iStart+iBack;
			if(newSeek<0){
				MessageBox.Show("�_�l����<0");
			}

			double newEnd=iStart;
			double newStart=newSeek;

			if(newEnd < newStart){// �Y���O�� R20,5,10  <--- �q 20 ��}�l, ���쩹�᤭�����q���� 10 ��
				double temp=newEnd;
				newEnd=newStart;
				newStart=temp;
			}
			string strSeek=""+newStart;
			string strEnd=""+newEnd;
			SectionInfo[0]=strSeek;
			

			
			if(bHasRepeat){
				SectionInfo[1]=strEnd;
				if(bHasCommend){
					string strRepeat=getStart_Comma_String(retCommaPos[0]+1,SectionCommand,retCommaPos); // �_�l�I�� �r���������r��
					SectionInfo[2]=strRepeat;
					string strComment=getLastCommandString(retCommaPos[0]+1,SectionCommand); // ���o�̫�@�Ӧr��
					SectionInfo[3]=strComment;
				}else{
					string strRepeat=getLastCommandString(retCommaPos[0]+1,SectionCommand); // ���o�̫�@�Ӧr��
					SectionInfo[2]=strRepeat;
				}
			}else{
				// ���o End Time
				SectionInfo[1]=strEnd;
			}
		}

		public static bool IsForwardMode(String strSeek,string[] NewCommand){
			int i;
			for(i=0;i<strSeek.Length;i++){
				if(strSeek[i]==',')
					return false;
				if(strSeek[i]==' ')
					continue;
				if(strSeek[i]=='f' || strSeek[i]=='F')
					break;
				
			}

			if(strSeek[i]=='f' || strSeek[i]=='F'){
				i++;
				for(;i<strSeek.Length;i++){
					NewCommand[0]=NewCommand[0]+strSeek[i];
				}
				return true;
			}else{
				return false;
			}
		}

		// ���o���`���p�� Seek, End, Repeat ��T
		// Forward Mode ���O�榡: F�_�Ϭ��,Forward���,Repeat
		public static void FowardModeProcess(string[] SectionInfo,string SectionCommand,bool bHasCommend,bool bHasRepeat){
			int[] retCommaPos=new int[1];
			// ���O�Φ�=  B End,Back Second, Repeat
			
			string strStartup=getStart_Comma_String(0,SectionCommand,retCommaPos); // �_�l�I�� �r���������r��
			string strForward=null;
			if(bHasRepeat==true)
				strForward=getStart_Comma_String(retCommaPos[0]+1,SectionCommand,retCommaPos); // �_�l�I�� �r���������r��
			else
				strForward=getLastCommandString(retCommaPos[0]+1,SectionCommand); // ���o�̫�@�Ӧr��
			double iForward=Double.Parse(strForward);
			double iStart=Double.Parse(strStartup);

			double newSeek=iStart;
			double newEnd=iStart+iForward;
			

			string strSeek=""+newSeek;
			string strEnd=""+newEnd;
			SectionInfo[0]=strSeek;
			
			if(bHasRepeat){
				SectionInfo[1]=strEnd;
				if(bHasCommend){
					string strRepeat=getStart_Comma_String(retCommaPos[0]+1,SectionCommand,retCommaPos); // �_�l�I�� �r���������r��
					SectionInfo[2]=strRepeat;

					string strComment=getLastCommandString(retCommaPos[0]+1,SectionCommand); // ���o�̫�@�Ӧr��
					SectionInfo[3]=strComment;
				}else{
					string strRepeat=getLastCommandString(retCommaPos[0]+1,SectionCommand); // ���o�̫�@�Ӧr��
					SectionInfo[2]=strRepeat;
				}
			}else{
				// ���o End Time
				//strEnd=getLastCommandString(retCommaPos[0]+1,SectionCommand); // ���o�̫�@�Ӧr��
				SectionInfo[1]=strEnd;
			}
		}

        public bool IsEmpty()
        {
            if (this.listBox1.Items.Count == 0)
                return true;
            else
                return false;
        }

        // ����Ĥ@�Ӥ��q���y��
        public bool PlayTheFirstSection()
        {
            if (IsEmpty())
                return false;
            //this.listBox1.SelectedIndex = 0;
            //SetupAudioSection();

            CurrentPlayIndex = 0;
            this.PlayCurrentSection2();
            return true;
        }

        // ����Ĥ@�Ӥ��q���y��
        public bool PlayTheLastSection()
        {
            if (IsEmpty())
                return false;
           // this.listBox1.SelectedIndex = listBox1.Items.Count-1;
            //SetupAudioSection();

            CurrentPlayIndex = listBox1.Items.Count - 1;
            this.PlayCurrentSection2();
            return true;
        }

        public bool PlayPreviousSection()
        {
            // �Y���O�Ĥ@��, �h�i�H����
            if (CurrentPlayIndex > 0)
            {
                CurrentPlayIndex--;
                PlayCurrentSection2();
                return true;
            }
            else
            {

                this.myAVPlayer.label2.Text = "��Ƴ̳���";

                this.myAVPlayer.myFileListForm.bChangeFromOtherForm = true; // �ХܨӦۨ�L�a��, �ڭ̭n�ۤv��ܤϥ�
                bool bOk = this.myAVPlayer.myFileListForm.PlayThePreviouseFile();
                if (bOk == false)
                {
                    // this.myAVPlayer.AllStop();
                }

                return bOk;
            }
        }
        public bool PlayNextSection()
        {
            // �Y���O�̫�@��, �h�i�H����
            if (CurrentPlayIndex < this.listBox1.Items.Count - 1)
            {
                CurrentPlayIndex++;
                PlayCurrentSection2();
                return true;
            }
            else
            {
                this.myAVPlayer.label2.Text = "��Ƴ̩���";
                this.myAVPlayer.myFileListForm.bChangeFromOtherForm = true; // �ХܨӦۨ�L�a��, �ڭ̭n�ۤv��ܤϥ�
                bool bOk=this.myAVPlayer.myFileListForm.PlayTheNextFile();
                if (bOk == false)
                {
                    this.myAVPlayer.AllStop();
                }
                return bOk;
            }
        }
        public int CurrentPlayIndex = -1;

        public bool bChangeFromProgramm = false;
        public void PlayCurrentSection2()
        {
            if (CurrentPlayIndex >= 0 && CurrentPlayIndex <= this.listBox1.Items.Count - 1)
            {
                // �ߧY����
                string strNum = (string)LastTextList[CurrentPlayIndex]; // �]�w�n�n���񪺫��w���;
                myAVPlayer.textBox1.Text = strNum;
                myAVPlayer.SeekPlay();

                // ��ܫ��O���Ѹ�T
                string strStatusInfo = myAVPlayer.textBox1.Text;// Ū���O�_�������� ���޸�ƪ� Comment ��T
                if (this.hasCommentInfo())
                    strStatusInfo = this.getCommentInfo();
                myAVPlayer.label2.Text = "���ި� " + strStatusInfo;

                // �]�w�s�����
                this.textBox1.Text = strNum;

                // �Y�Ӧۨ�L������, �h�n�ۤv�B�z index Selected �����
                if (bChangeFromProgramm)
                {
                    this.listBox1.SelectedIndex = CurrentPlayIndex; // ��s���
                    bChangeFromProgramm = false;
                }
            }
        }

        // ����ثe���ު��y�����q, �åB�@���n�������ܻP��s�ʧ@
        public void PlayCurrentSection()
        {
            int index = this.listBox1.SelectedIndex;
            string strNum = (string)LastTextList[index]; // �]�w�n�n���񪺫��w���;
            myAVPlayer.textBox1.Text = strNum;
            myAVPlayer.SeekPlay();

            // ��ܫ��O���Ѹ�T
            string strStatusInfo = myAVPlayer.textBox1.Text;// Ū���O�_�������� ���޸�ƪ� Comment ��T
            if (this.hasCommentInfo())
                strStatusInfo = this.getCommentInfo();
            myAVPlayer.label2.Text = "���ި� " + strStatusInfo;


            // ��s�ثe���ު��A
            curTextIndex = this.listBox1.SelectedIndex;


            // �]�w�s�����
            this.textBox1.Text = strNum;
            // end of �s�����
        }
		public void SetupAudioSection(){
			if(this.listBox1.SelectedIndex >=0 && myAVPlayer.myLyricer.isLoadingLyric()==false){
				string strNum=(string) LastTextList[this.listBox1.SelectedIndex]; // �]�w�n�n���񪺫��w���;
				myAVPlayer.textBox1.Text=strNum;
				myAVPlayer.SeekPlay();

                // ��ܫ��O���Ѹ�T
                string strStatusInfo = myAVPlayer.textBox1.Text;// Ū���O�_�������� ���޸�ƪ� Comment ��T
				if(this.hasCommentInfo())
					strStatusInfo=this.getCommentInfo();
				myAVPlayer.label2.Text="���ި� "+strStatusInfo;

				
                // ��s�ثe���ު��A
				curTextIndex=this.listBox1.SelectedIndex;


				// �]�w�s�����
				this.textBox1.Text=strNum;
				// end of �s�����
			}
		}

		// ����ަ�m���ܮ�
        bool bFromSelectedValueChanged = false;
		private void listBox1_SelectedValueChanged(object sender, System.EventArgs e) {
            
            // �p�G���ܬO�Ӧ۷ƹ��I��, �h�ݭn�ߧY����
            if (bFromMouseDown)
            {
                CurrentPlayIndex = this.listBox1.SelectedIndex;
                this.PlayCurrentSection2();
                bFromMouseDown = false;

            }
            else
            {   
                // ������]�y�� SelectedValueChanged �~�ʧ@, �b�o�̭n�j��P�B
                if (CurrentPlayIndex < this.listBox1.Items.Count)
                {
                    if (bFromSelectedValueChanged == false)
                    {
                        bFromSelectedValueChanged = true;
                        this.listBox1.SelectedIndex = CurrentPlayIndex;
                        bFromSelectedValueChanged = false;
                    }
                }
            }
           
		}

		private void IndexListForm_SizeChanged(object sender, System.EventArgs e) {
			//listBox1.Size.Height=this.button1
		}

		private void button3_Click(object sender, System.EventArgs e) {
			this.ClearList(true);
			button1.Visible=true;
			this.myAVPlayer.textBox1.Focus();
		}

		private void button4_Click(object sender, System.EventArgs e) {
			this.LoadList();
			this.myAVPlayer.textBox1.Focus();
		}

		private void IndexListForm_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e) {
		}


		
		
		
		protected override void WndProc(ref Message m) {
			int[] NewLoc=new int[2];
			System.Drawing.Rectangle SelfBound=this.Bounds;
			System.Drawing.Rectangle MotherBound=this.myAVPlayer.Bounds;
			// myDockHelp.WndProc(ref m,SelfBound,MotherBound,NewLoc,this);
			myDockHelp.WndProc2(ref m,SelfBound,this.myAVPlayer,NewLoc,this);

			base.WndProc(ref m);

			//this.Text="c="+myDockHelp.isConnected+" DMY="+myDockHelp.DMY+" DMX="+myDockHelp.DMX;
		}

		private void IndexListForm_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e) {
			
			
		}
        public bool bActived=false;

        bool bFirstLoad = true;
		private void IndexListForm_Load(object sender, System.EventArgs e) {
            // Ū���������� Z-Order, �M�w�ۤv�� Z-Order
            this.TopMost = this.myAVPlayer.MyTopMost;

            // �H�ɰO���ثe��m, 
            if (WindowState == FormWindowState.Normal)
                oldLocation = this.Location;

            // �]�w��l��m
            if (bFirstLoad)
            {
                SetDefaultLocation();
                bFirstLoad = false;
            }
		}

        public void SetDefaultLocation()
        {
            // ���ʥثe��m����������U��, �|�۰ʩI�s WndProc 
            // �|�_���H�K�\��. 
            DockUtility.MoveToMotherBottom(this, this.myAVPlayer);
        }

		private void textBox1_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e) {
			// �ϥΪ̽s�觹��, Update �ثe item list �����
			bool bOk=false;
			if(e.KeyCode==Keys.Enter) {
				// ���ˬd�ثe��J�O�_�s�b
				bool Exist=checkExist_SeekTime(this.textBox1.Text);
				if(Exist==true){
					// �s�b��, �i�J�s��Ҧ�
					if(this.textBox1.Text.IndexOf(",")==-1){
						// �ǲο�J, �@�w�O�Ʀr
						try{
							int.Parse(this.textBox1.Text);
							bOk=EditItem(this.textBox1.Text);
						
						}catch{
						}
					}else{
						// �X�R�榡 
						bOk=EditItem(this.textBox1.Text);	
					}

                    if (!bOk)
                    {
                        this.myAVPlayer.label2.Text = "�榡���~";
                    }
                    else
                    {
                        PlayCurrentSection2(); // �ק�L��, �ߧY����
                    }
				}else{
					// �ثe�r�ꤣ�s�b, �i�J�s�W�Ҧ�
					this.AddItem(this.textBox1.Text);
				}
			}

			if(e.KeyCode==Keys.Up){
				// this.MoveUP();
                this.bChangeFromProgramm = true; // �ۤv�B�z�ϥ�
                this.PlayPreviousSection();
			}

			if(e.KeyCode==Keys.Down){
				// this.MoveDown();
                this.bChangeFromProgramm = true; // �ۤv�B�z�ϥ�
                this.PlayNextSection();
			}

		}
		
		class Section_Compare:System.Collections.IComparer{

			public int Compare(
				object x,
				object y
				){

				// ���X x ���_�ϯ���
				string Commandx=(string)x;
				double dSeekX=IndexListForm.getSeekTime(Commandx);
				string Commandy=(string)y;
				double dSeekY=IndexListForm.getSeekTime(Commandy);

				return (int)(dSeekX-dSeekY);
			}
		}

		private void button5_Click(object sender, System.EventArgs e) {
			
			if(LastTextList.Count >0){
				LastTextList.Sort(new Section_Compare()); // �i��Ƨ�

				this.listBox1.Items.Clear(); // �M����ܭ��������e

				// �N LastTextList ��Ƥ@�@�[�J���� 
				for(int i=0;i<LastTextList.Count;i++){
					string sCommand=(string)LastTextList[i];
					this.listBox1.Items.Add(sCommand);      // �]�w List ���
				}

				curTextIndex=0; // �Ƨǧ���,���w�Ĥ@����Ƭ��_�l�I
				this.listBox1.SelectedIndex=curTextIndex;

				SetModify();
			}else{
				// �S����Ƥ��ʧ@
			}

			
		}

		private void checkBox1_SectionAutioRepeat_CheckedChanged(object sender, System.EventArgs e) {
			this.myAVPlayer.PropertyTable["SectionAutoRepeat"]=""+this.checkBox1_SectionAutioRepeat.Checked;
			this.myAVPlayer.SaveProperty();

			if(this.checkBox1_SectionAutioRepeat.Checked){
				this.MoveFirst();
				this.myAVPlayer.label2.Text ="�y�����q�۰ʴ`��"+"  ON";
			}else{
				this.myAVPlayer.label2.Text ="�y�����q�۰ʴ`��"+"  OFF";
			}
		}

		public bool isTheLast(){
			return !hasNext2();
		}

		// �ˬd�O�_��F
		double WaitCount=0;
		bool NowWaitToNext(){
			// Ū���w�] Repeat  ���ݮɶ�
			string strDefaultBlank=(string)this.myAVPlayer.PropertyTable["DefaultCommand_Blank"];
			double BlankSecnod=Double.Parse(strDefaultBlank);
			if(WaitCount<BlankSecnod){
				WaitCount+=0.2;
				int WaitBlankSecnod=(int)(BlankSecnod-WaitCount)+1;
				myAVPlayer.ShowState(WaitBlankSecnod+" ��᭫��");
				myAVPlayer.ourAudio.Pause();
				return true;
			}else{
				WaitCount=0;
				myAVPlayer.ourAudio.Play();
				return false;
			}
		}

		// ���� Repeat ���ʧ@
		public void DoRepeat(){
			if(NowWaitToNext()==true)
				return;
			DoRepeat(true);
		}

		public void DoRepeat(bool bCountDown){
			if(bCountDown){
				int curRepeatCount=this.RepeatCountDown();
				
				if(curRepeatCount>0){
					bool bRepeat=true;
					myAVPlayer.SeekPlay(bRepeat);
					myAVPlayer.ShowState("����: "+curRepeatCount);
				}else{
					// Repeat ���񧹲�,���B�z�ƶ�
					EndOfRepeat();			
				}
			}else{
				bool bRepeat=true;
				myAVPlayer.SeekPlay(bRepeat);
				myAVPlayer.ShowState("�L���`��");
			}
		}

		public void EndOfRepeat(){
			// Repeat ���񧹲�

            // �O�_���̫�y�����q
            if (isTheLast() == true)
            {
                // �O�_��@�ɮ׭n�`������Ҧ������q
                if (this.checkBox1_SectionAutioRepeat.Checked == true)
                {
                    // ����`�����q����
                    this.MoveFirst();
                    this.myAVPlayer.label2.Text = "�۰ʴ`��";
                }
                else
                {    // �_�h�浹ť�O�F�������B�z: ��Ҧ��ɮפ����y�����q�����񧹲�, �h�����, �_�h���X�U�@���ɮפ����Ĥ@�ӻy�����q
                    if (this.myAVPlayer.myFileListForm.PlayTheNextFile() == false)
                        this.myAVPlayer.AllStop();
                }
                return;
            }

            // �D�̫�y�����q���@�뱡�p�B�z
            this.myAVPlayer.ShowState("�~�򼽩�");
            if (this.hasNext2())
            {
                // this.MoveDown();
                this.bChangeFromProgramm = true; // �ݭn�ۤv�B�z��ܤϥ�
                this.PlayNextSection();
            }
            else
            {
                // .. do not thing
            }


            /*

			bool bNormalRepeat=false;

			// �O�_���̫�y�����q
			if(isTheLast()==true){
				// �ˬd�O�_���y�����q�`������
				if(this.checkBox1_SectionAutioRepeat.Checked==true){
					// ����`�����q����
					this.MoveFirst();
					this.myAVPlayer.label2.Text ="�۰ʴ`��";
				}else{
					bNormalRepeat=true;
				}

			}else{
				bNormalRepeat=true;
			}// end of �@����q����

			// ����@�� Repeat ���� �ʧ@
			if(bNormalRepeat){
				this.myAVPlayer.ShowState("�~�򼽩�");
				if(this.hasNext2()){
					this.MoveDown();
				}else{
					// .. do not thing
				}
			}
             */
		}

		private void button6_Click(object sender, System.EventArgs e) {
			if(this.listBox1.SelectedIndex >=0 && this.textBox1.Text!=null){
				string strNum=(string) LastTextList[this.listBox1.SelectedIndex]; // �]�w�n�n���񪺫��w���;
				this.RemoveItem(strNum);
				this.myAVPlayer.label2.Text="�R�����\";
			}else{
				this.myAVPlayer.label2.Text="���q���b��C��";
			}
		}

		private void button7_Click(object sender, System.EventArgs e) {
			if(CheckCommand(this.textBox1.Text)==true){
				if(checkExist(this.textBox1.Text)!=true){
					AddItem(this.textBox1.Text);
				}else{
					this.myAVPlayer.label2.Text="���q�w�g�s�b";
				}
			}else{
				this.myAVPlayer.label2.Text="�榡���~";
			}
		}

		private void textBox1_TextChanged(object sender, System.EventArgs e) {
			this.button6.Enabled=true; 
			this.button7.Enabled=true; 
		}

		private void cbAllRepeat_CheckStateChanged(object sender, System.EventArgs e) {
			if(this.cbAllRepeat.Checked==true){
				myAVPlayer.ShowState("�L���`��");
				System.Drawing.Bitmap imgRepeatOn=new System.Drawing.Bitmap(myAVPlayer.ExecutationFileDir+"\\RepeatOn.gif"); // �ثe���J�v��
				this.cbAllRepeat.Image=imgRepeatOn;

				this.checkBox1_SectionAutioRepeat.Enabled =false;
			}
			else{
				myAVPlayer.ShowState("�L���`������");
				System.Drawing.Bitmap imgRepeatOff=new System.Drawing.Bitmap(myAVPlayer.ExecutationFileDir+"\\RepeatOff.gif"); // �ثe���J�v��
				this.cbAllRepeat.Image=imgRepeatOff;
				this.checkBox1_SectionAutioRepeat.Enabled =true;
			}
		}

		// ��ϥΪ̫��U�w�]���O���s
		private void button8_Click(object sender, System.EventArgs e) {
			string Repeat=(string) myAVPlayer.PropertyTable["DefaultCommand_Repeat"];
			string Index=(string) myAVPlayer.PropertyTable["DefaultCommand_Index"];
			double curPos=(int)(myAVPlayer.ourAudio.CurrentPosition*100)/100.0;

			string Command=GetDefaultCommand(curPos.ToString(),Repeat,Index);
			this.textBox1.Text=Command;
			
			bool	bOk=AddItem(this.textBox1.Text);
			if(bOk){
				//UpdateCommandLabel(this.textBox1.Text);
			}
			
		}// end of method

        

        // ��s�Х�
        public void UpdateCommandLabel(string Command)
        {
			string[] SectionInfo_Seek_End_Repeat=new string[IndexListForm.CommandsTotalNum];
			Parse_getSeekTime_End_Repeat(Command,SectionInfo_Seek_End_Repeat);
			label1.Text="�_�l: "+SectionInfo_Seek_End_Repeat[0];
			label2.Text="����: "+SectionInfo_Seek_End_Repeat[1];
		}

		public string GetDefaultCommand(string strCurPos,string Repeat,string RIndex){
			return "R"+strCurPos+","+RIndex+","+Repeat+", �o�O�w�]���O";
		}

        private void IndexListForm_Activated(object sender, EventArgs e)
        {
            bActived = true;
            this.myAVPlayer.iStat++;

            //System.Console.WriteLine("iStat=" + this.myAVPlayer.iStat);
        }

        private void IndexListForm_Deactivate(object sender, EventArgs e)
        {
            
        }

        private void listBox1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
          
        }
        bool bFromMouseDown = false;
        private void listBox1_MouseDown(object sender, MouseEventArgs e)
        {
            this.myAVPlayer.bMySectionBasedPlay = true; // �Ұʻy�����q����¦������
            
            // this.PlayCurrentSection2();
            // �ƹ����U�h���ɭ�, �۵M�|�Ұʿ�� listBox1_SelectedValueChanged
            bFromMouseDown = true;
        }

	}
}
