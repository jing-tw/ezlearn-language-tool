//                                                                                        ������
// �D�n���]�p�ت�: �N�O�n�b���z�Z�ϥΪ̪����p�U, ���ͧ�s�ˬd. 
//                 �ˬd������, �p�G�ϥΪ̤��P�����, �۰���������.

//                 ���F�]�����ǨϥΪ̫ܰQ���۰ʧ�s�����дo (�]�t ��), 
//                 �ҥH�@�w�n�������o���\�઺�ﶵ.

// �{���]�p����:   .Net Framewowrk ���Ѩ�� Timer �Ҧ�, �Ĥ@�جO�Q�� Message �覡�����h�� thread, �Y�����Ӯɭp��, �h�|����L���󪺹B��
//                 �ĤG�جO�u�����ͤ@�� thread, �i��B�z, ���|�v�T���L���󪺰T������
// 
//                 �]�� .Net Windows Form space �� Timer �D�n�O�ϥγ�@ thread �C�j�@�q�ɶ�, ������w���{���X.
//                 �p�G�����ϥγo�ӨӰ�������W���ˬd, �|�����D.
//                 ���ˬd��, �A���D�n�����|�Q�����ʼu.

//                 �D�n��]�N�O�A�������{���Q�������F. �ҥH�ѨM��״N�O���������t�@�� thread
//                 ���n���L���D�n���� thread.

// ��ڧ@�k:       �Q�� System.Threading.Timer �إߥt�@�� thread, �ѥL�Ӱ��氪�Ӯɪ������ˬd�u�@
// ����{�����q:
/*
            lock(this){
				hasFinish=false;
			}
			// �]�w checkVersion �M�ݪ� Timer
			CheckVersion myCVObj=new CheckVersion(this);
			System.Threading.TimerCallback timerDelegate = 
				new System.Threading.TimerCallback(myCVObj.CheckStatus);
			myVersionCheckTimer = 
				new System.Threading.Timer(timerDelegate, null, 0, 0); // ���W�}�l�I�s CheckStatus, �åB�����ƩI�s
			// end of �]�w checkVersion �M�ݪ� Timer 

			// �]�w �ˬd�i�� �M�ݪ� Timer
			myUpdateProgressTimer=new FormUpdateTimer(this);
			myUpdateProgressTimer.Tick += new EventHandler(TimerEventProcessor2);	
			myUpdateProgressTimer.Interval = 50; // �C�j .5 ����, �I�s TimerEventProcessor procedure
			myUpdateProgressTimer.Start();
			// end of �]�w checkVersion �M�ݪ� Timer 
*/

using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Runtime.InteropServices; // for DllImport


namespace Player
{
	/// <summary>
	/// Summary description for UpdateNotifiyForm.
	/// </summary>
	public class UpdateNotifiyForm : System.Windows.Forms.Form
	{
        private IntPtr ParentHandle;

        [DllImport("User32.dll")]
        public static extern int SendMessage(
            IntPtr hWnd,               // handle to destination window
            int Msg,                // message
            int wParam,             // first message parameter
            int lParam);            // second message parameter
       
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.LinkLabel linkLabel1;
		private System.Windows.Forms.ProgressBar progressBar1;
		public AVPlayer myPlayParent=null;
		

		public NumberLabel l1; //  �˼ƭp�ɪ���
		public static int iErrorCountDown=5;     // �����o�Ϳ��~��, ���d�����
		
		public int CountDownStart=5;
		private System.Windows.Forms.CheckBox checkBox1;
		private System.Windows.Forms.Label label3; // �˼ƭp�ɱҩl��

		string strVersionStatus;
        WebBrowser EZwebBrowser; // Tracking �ϥζq�M��
        System.IO.StreamReader PrjStream=null;

		public UpdateNotifiyForm(AVPlayer obj)
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			myPlayParent=obj; // �D�n���� [���F�n���o�ݩʦ�C]
            ParentHandle = obj.Handle;

            string strCurVersion = this.myPlayParent.L_Manager.getMessage("Version_CurrentVersionString");
            this.label1.Text = strCurVersion + AVPlayer.strVersion_subNo; // this.label1.Text="�ثe������T:"+AVPlayer.strVersion_subNo;


            this.label2.Text = this.myPlayParent.L_Manager.getMessage("Version_CheckingWaitingMessage"); // this.label2.Text="���A:"+"  ���A�ˬd��, �е��@�U...";


            MessageManager.ShowInformation(this.myPlayParent.label3, this.label2.Text, 5000, false);
           //  MessageManager.ShowInformation(this.myPlayParent.label3, "�ثe���b�i�檩���ˬd ...", 5000,false);

			// �[�J�˼ƭp�ɾ���
			l1=new NumberLabel(myPlayParent.imgNumArray);
			// l1=new NumberLabel(myPlayParent.ExecutationFileDir,"Num",0,0,0);   // ���w�Ʀr�ɦW, ���w�z���C�� r,g,b
			l1.Visible=false;
			l1.Left= label1.Right; l1.Top=label1.Top;
			l1.Height=30;//l1.Width=30;
			l1.SetWidth(4); // ���|�ռƦr
			//l1.Text=""+CountDownStart; // ��ܼƦr
			this.Controls.Add(l1); // �[�J�����O
			// end of �˼ƭp�ɾ���

            

			// �u�W�ˬd
			string strOnlineU=(string)myPlayParent.PropertyTable["�O�_�n�u�W�ˬd����"];
			if(strOnlineU.IndexOf("true")!=-1)
				this.checkBox1.Checked=true;
			else
				this.checkBox1.Checked=false;
			// end of �u�W�ˬd


		}

		System.Threading.Timer myVersionCheckTimer;
		FormUpdateTimer myUpdateProgressTimer;

		public bool hasFinish=false;


		public void FireCheck(){
			// �]�� Form space ���� Timer �Ҳ��ͪ� Notification �ƥ�, �I�s�ƥ�禡�ϥΪ���@ thread
			// �b���ӮɹB���, �|�Ϩ�L�����ʧ@�����Ȱ�.
			// �ҥH���Ӯɨ禡�I�s��, �����ϥ� Thread ������ Timer, �إߥt�@�� thread ���氪�Ӯɤu�@
			lock(this){
				hasFinish=false;
			}

			// �]�w checkVersion �M�ݪ� Thread ���� Timer
			CheckVersion myCVObj=new CheckVersion(this);
			System.Threading.TimerCallback timerDelegate =new System.Threading.TimerCallback(myCVObj.CheckStatus);
			myVersionCheckTimer = new System.Threading.Timer(timerDelegate, null, 0, 0); // ���W�}�l�I�s CheckStatus, �åB�����ƩI�s
			// end of �]�w checkVersion �M�ݪ� Timer 
            /*
			// �]�w �ˬd�i�� �M�ݪ� Timer
			myUpdateProgressTimer=new FormUpdateTimer(this);
			myUpdateProgressTimer.Tick += new EventHandler(TimerEventProcessor2);	
			myUpdateProgressTimer.Interval = 50; // �C�j .5 ����, �I�s TimerEventProcessor procedure
			myUpdateProgressTimer.Start();
            */
			// end of �]�w checkVersion �M�ݪ� Timer 

		}

		// �t�@�� thread �W���ˬd������T
		public bool hasNewVersion;
		public void TimerEventProcessor1() {
			hasNewVersion=checkVersion(); // ���� ���Ӯ� �ˬd������T
			
			this.myVersionCheckTimer.Dispose();

			// �ˬd����, ��s��ܸ�T
			lock(this){
				this.hasFinish=true;// �ˬd����
			}
		}
		

		// ����ˬd�i��
        bool bTrackRun = true;
		bool bCountDown;
		private static void TimerEventProcessor2(Object myObject,
			EventArgs myEventArgs) {

            
			UpdateNotifiyForm myParentForm=((FormUpdateTimer)myObject).myParentForm;

            // Web Tracking ������
            /*
             * The WebBrowser control is resource-intensive. 
             * Be sure to call the Dispose method when you are finished using the control to ensure that all resources are released 
             * in a timely fashion. You must call the Dispose method on the same thread that attached the events, which should always
             * be the message or user-interface (UI) thread.
             */
            if (myParentForm.bTrackRun == true) {
                myParentForm.EZwebBrowser = new WebBrowser();
                myParentForm.EZwebBrowser.Navigate(@"http://mqjing.twbbs.org.tw/~ching/Course/JapaneseLanguageLearner/__page/UsageCounter.html");
                myParentForm.bTrackRun = true;


               // myParentForm.EZwebBrowser.Navigate(@"http://debut.cis.nctu.edu.tw/~ching/Course/JapaneseLanguageLearner/__page/Download2.htm");
               // myParentForm.PrjStream = new System.IO.StreamReader(myParentForm.EZwebBrowser.DocumentStream, System.Text.Encoding.GetEncoding(myParentForm.EZwebBrowser.Document.Encoding));

            }


			myParentForm.bCountDown=false; // �O�_�Ұʭ˼���������
			bool bF;
			lock(typeof(UpdateNotifiyForm)){
				bF=myParentForm.hasFinish;
			}

			if(!bF){
				// �Y�i�ת� = 100%, �h�k�s �~��
				if(myParentForm.progressBar1.Value == myParentForm.progressBar1.Maximum)
					myParentForm.progressBar1.Value=0;
				myParentForm.progressBar1.Increment(1);
				
			}else{
				// �ˬd����, ��s��ܸ�T
				myParentForm.UpdateInfo(myParentForm.hasNewVersion);
				myParentForm.progressBar1.Value=myParentForm.progressBar1.Maximum;

				myParentForm.bCountDown=true; // �Ұʭ˼���������
				myParentForm.myUpdateProgressTimer.Interval=1000; // ���s�]�w�C�j�@����, �˼ƭp�ɤ@��
				myParentForm.l1.Visible=true;
			}
			
			// �˼���������
			if(myParentForm.bCountDown){
				myParentForm.l1.Text=""+myParentForm.CountDownStart; // ��ܼƦr
				if(myParentForm.bAutoClose){ // �u���b�۰������\��U,�|�˼ƭp��. (��ϥΪ� Active ������, �۰������\��|�Q����)
					string strTitle="������T ("+myParentForm.CountDownStart+" ������۰�����)";
					myParentForm.Text=strTitle;

					myParentForm.CountDownStart--;

					if(myParentForm.CountDownStart <0){
						myParentForm.myUpdateProgressTimer.Stop();
						myParentForm.myUpdateProgressTimer.Dispose();
						OpacityUtility.FadeOut_Close(myParentForm,30);
					}
				}else{
					myParentForm.Text="������T"+"(�˼������Ȱ�)";
				}

			}
		}
		// end of �ˬd�i��


		public void UpdateInfo(bool hasNewVersion){
			if(hasNewVersion){
				this.label2.Text="���A:"+strVersionStatus;
				string strPrjaddress=(string)myPlayParent.PropertyTable["�M�׺��}"];
				this.linkLabel1.Visible=true;
			}else{
				this.label2.Text="���A:"+strVersionStatus;
			}

		}

       
        // �T���q�����Ψ禡
        public void SendNeedToUpdateMessage(System.Windows.Forms.Form Form)
        {
            //const int WM_APP = 0x8000;
            //const int WM_NeedToUpdateMessage = (WM_APP + 1);
            int wparam = 1; // 1: ��ܻݭnUpdate, 0 ��ܤ��ݭn Update, -1 ���������D
            SendMessage(ParentHandle, AVPlayer.WM_NeedToUpdateMessage, wparam, 0);
        }

        public void SendNetworkError(System.Windows.Forms.Form Form)
        {
            int wparam = -11; // 1: ��ܻݭnUpdate, 0 ��ܤ��ݭn Update, -1 ���������D
            SendMessage(ParentHandle, AVPlayer.WM_NeedToUpdateMessage, wparam, 0);
        }

        public void Send_VersionOK(System.Windows.Forms.Form Form)
        {
            //const int WM_APP = 0x8000;
            //const int WM_NeedToUpdateMessage = (WM_APP + 1);
            int wparam = 0; // 1: ��ܻݭnUpdate, 0 ��ܤ��ݭn Update, -1 ���������D
            SendMessage(ParentHandle, AVPlayer.WM_NeedToUpdateMessage, wparam, 0);
        }

        



		// �ˬd������T
		// ���ӮɯS��
		// System.IO.StreamReader PrjStream;
		public bool checkVersion(){

			CountDownStart=10;
			string strTitle="������T ("+CountDownStart+" ������۰�����)";
			//this.Text=strTitle;

             
			bool bFoundNewVersion=false;
			bool hasFoundSelf=false;
			// string strPrjAddress=(string)myPlayParent.PropertyTable["�M�׺��}"];
            string strPrjAddress = "http://mqjing.twbbs.org.tw/~ching/Course/JapaneseLanguageLearner/__page/Download2.htm";

            
			try{

               // EZwebBrowser.Navigate(@"http://debut.cis.nctu.edu.tw/~ching/Course/JapaneseLanguageLearner/__page/Download2.htm");
               // PrjStream = new System.IO.StreamReader(EZwebBrowser.DocumentStream, System.Text.Encoding.GetEncoding(EZwebBrowser.Document.Encoding));

                
				System.Net.WebClient PrjClient=new System.Net.WebClient();
				PrjStream=new System.IO.StreamReader(PrjClient.OpenRead(strPrjAddress));
                /*
                while (PrjStream == null)
                {
                    System.Threading.Thread.Sleep(100);
                    // Application.pro
                };
                */
				String line;
					
				
				while ((line = PrjStream.ReadLine()) != null) {
					int i=line.IndexOf(AVPlayer.strVersion_subNo+"_GreenInstall.zip");
					if(i!=-1){
						hasFoundSelf=true;
						break;
					}
				}

				if(hasFoundSelf){
					bFoundNewVersion=false;
                    if (AVPlayer.bBeta == false)
                        this.strVersionStatus = "�̷s����";
                    else
                        this.strVersionStatus = "�o�O Beta ����";

                    //MessageManager.ShowInformation(this.myPlayParent.label3, "�ثe�����O�̷s����", 10000);
                    Send_VersionOK(myPlayParent);
					CountDownStart=3;
					
				}else{
					bFoundNewVersion=true;
					
                    if (AVPlayer.bBeta == false)
                        this.strVersionStatus = "  �z���n�骩���O�ª�, �����W�w�g���s������";
                    else
                        this.strVersionStatus = "�o�O Beta ����";

                    //MessageManager.ShowInformation(this.myPlayParent.label3, "�����W�w�g���s������", 10000);
					this.myPlayParent.PropertyTable["�ثe�������A"]="�ª�"; // 07/21/2005 [�\��վ�] 
					this.myPlayParent.SaveProperty(); // Update �ݩ��� 
                    //this.myPlayParent.Text = strVersionStatus;

                    SendNeedToUpdateMessage(myPlayParent);
                    // SendMessage(this.myPlayParent.Handle , AVPlayer.WM_NeedToUpdateMessage, 0, 0);

					// this.myPlayParent.Text="�y���ǲ߾� (�����W�w�g���s������)";  
				}

               

			}catch(Exception e){
				System.Windows.Forms.MessageBox.Show("�M�׺��}:"+strPrjAddress+"\n\n�������G�����D:\n"+"\n\n�Բӿ��~��T:"+e.ToString(),"�۰ʧ�s����");
				this.strVersionStatus="�������G�����D: "+e.Message;
				CountDownStart=iErrorCountDown;
				
				// this.Text="������T�ˬd��...   ("+CountDownStart+" �����۰�����)";;

                SendNetworkError(this.myPlayParent); // �e�X���������D�T����������
				return false;
			}

			return bFoundNewVersion;
			// 
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
            // �����l�ܶ�
            if(EZwebBrowser!=null)
                EZwebBrowser.Dispose();

			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();

					if(PrjStream!=null)
						PrjStream.Close();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.label3 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label1.Location = new System.Drawing.Point(8, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(368, 23);
            this.label1.TabIndex = 0;
            this.label1.Text = "������T";
            // 
            // label2
            // 
            this.label2.ForeColor = System.Drawing.SystemColors.ControlLight;
            this.label2.Location = new System.Drawing.Point(8, 40);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(392, 32);
            this.label2.TabIndex = 1;
            this.label2.Text = "���A";
            // 
            // linkLabel1
            // 
            this.linkLabel1.LinkColor = System.Drawing.Color.Aqua;
            this.linkLabel1.Location = new System.Drawing.Point(144, 88);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new System.Drawing.Size(72, 23);
            this.linkLabel1.TabIndex = 3;
            this.linkLabel1.TabStop = true;
            this.linkLabel1.Text = "�U���s����";
            this.linkLabel1.Visible = false;
            this.linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(8, 72);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(392, 8);
            this.progressBar1.TabIndex = 4;
            // 
            // checkBox1
            // 
            this.checkBox1.Enabled = false;
            this.checkBox1.Location = new System.Drawing.Point(288, 88);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(10, 10);
            this.checkBox1.TabIndex = 5;
            this.checkBox1.Text = "checkBox1";
            this.checkBox1.Visible = false;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // label3
            // 
            this.label3.ForeColor = System.Drawing.SystemColors.ControlLight;
            this.label3.Location = new System.Drawing.Point(304, 88);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(96, 23);
            this.label3.TabIndex = 6;
            this.label3.Text = "�C���Ұʳ��ˬd";
            // 
            // UpdateNotifiyForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 15);
            this.BackColor = System.Drawing.Color.DarkGreen;
            this.ClientSize = new System.Drawing.Size(416, 102);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.linkLabel1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "UpdateNotifiyForm";
            this.Text = "������T�ˬd��...  ";
            this.Deactivate += new System.EventHandler(this.UpdateNotifiyForm_Deactivate);
            this.MouseEnter += new System.EventHandler(this.UpdateNotifiyForm_MouseEnter);
            this.Activated += new System.EventHandler(this.UpdateNotifiyForm_Activated);
            this.Closing += new System.ComponentModel.CancelEventHandler(this.UpdateNotifiyForm_Closing);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.UpdateNotifiyForm_MouseDown);
            this.Load += new System.EventHandler(this.UpdateNotifiyForm_Load);
            this.ResumeLayout(false);

		}
		#endregion

		private void UpdateNotifiyForm_Load(object sender, System.EventArgs e) {
			
		}

		private void linkLabel1_LinkClicked(object sender, System.Windows.Forms.LinkLabelLinkClickedEventArgs e) {

			// ���o�M�ת����}
			string strPrjAddress=(string)myPlayParent.PropertyTable["�M�׺��}"];
			
			// �]�w�s���ت��a
			this.linkLabel1.Links[0].LinkData = strPrjAddress;
			string target = e.Link.LinkData as string;

			// �I�s IE �s����, �s������w�����}
			System.Diagnostics.Process.Start("IExplore.exe",target);
		}

		private void UpdateNotifiyForm_Closing(object sender, System.ComponentModel.CancelEventArgs e) {
			
		}

		private void checkBox1_CheckedChanged(object sender, System.EventArgs e) {
			if(this.checkBox1.Checked){
				myPlayParent.PropertyTable["�O�_�n�u�W�ˬd����"]="true";
			}else{
				myPlayParent.PropertyTable["�O�_�n�u�W�ˬd����"]="false";
			}
			myPlayParent.SaveProperty();
		}

	    bool bAutoClose=true;
		private void UpdateNotifiyForm_Activated(object sender, System.EventArgs e) {
			// ��ϥΪ̹�o�ӵ����������, �����۰��������\��
			if(this.bCountDown){
				bAutoClose=false;
				this.Opacity=1.0; // ��_�����M����
			}
		}
		
		private void UpdateNotifiyForm_MouseEnter(object sender, System.EventArgs e) {
			
		}
		 
		private void UpdateNotifiyForm_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e) {
			
		}

		private void UpdateNotifiyForm_Deactivate(object sender, System.EventArgs e) {
			if(this.bCountDown){
				bAutoClose=true;
			}
		}
	}

	class FormUpdateTimer:System.Windows.Forms.Timer {
		public UpdateNotifiyForm myParentForm;
		public FormUpdateTimer(UpdateNotifiyForm showPannel) {
			myParentForm=showPannel;
		}
	}


	class CheckVersion{
		public UpdateNotifiyForm myParentForm;
		public CheckVersion(UpdateNotifiyForm showPannel) {
			myParentForm=showPannel;
		}

        

		// This method is called by the timer delegate.
		public void CheckStatus(Object stateInfo) {
			myParentForm.TimerEventProcessor1(); // ���Ӯ�

		}

       

	}

}
