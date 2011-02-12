// �M���ΨӳB�z Form ���S��
//    1. �������H�X(����), ����ı�ĪG ==> FadeOut
//    2. �Ȱ��ĪG (�G, �t)   ==> Flash


/*
	OpacityUtility.FadeOut_Close(this,30); //�H�X�����S��
	OpacityUtility.FadeIn_Only(MyIndexList,30); // �H�J
*/
using System;


namespace Player
{ 
	/// <summary>
	/// Summary description for OpacityUtility.
	/// </summary>
	/// 

	public class OpacityUtility {

		class KKTimer:System.Windows.Forms.Timer{
			public System.Windows.Forms.Form tForm;
			public double OpacityLevel=1; // �������H�X(����), ����ı�ĪG�ܼ�
			public int FlashC=0; // ����  �Ȱ��ĪG (�G, �t) �ܼ�
			public bool FlashFirstRun=true; // �O�_�Ĥ@������{�{
            public float FadeOutFinalValue = 0; 
			public KKTimer(System.Windows.Forms.Form tForm) {
				this.tForm=tForm;
			}
		}

		class KKTimer_Label:System.Windows.Forms.Timer{
			public System.Windows.Forms.Form tForm;
			public System.Windows.Forms.Label tLabel;
			
			public int FlashC=0; // ����  �Ȱ��ĪG (�G, �t) �ܼ�
			public bool FlashFirstRun=true; // �O�_�Ĥ@������{�{

			public KKTimer_Label(System.Windows.Forms.Form tForm,System.Windows.Forms.Label tLabel) {
				this.tForm=tForm;
				this.tLabel=tLabel;
			}
		}

		

		public OpacityUtility() {
			//tForm=target;
		}

		// ���Ұ{�{
		public static void Flash_Label(System.Windows.Forms.Form form,System.Windows.Forms.Label  target,int Interval){
			System.Windows.Forms.Label tLabel=target;
			KKTimer_Label myTimer;
			// �]�w �M�ݪ� Timer
			myTimer=new KKTimer_Label(form,tLabel);
			
			myTimer.Tick += new EventHandler(FlashLabel_TimerEventProcessor);			
			myTimer.Interval = Interval; // �C�j 1 ����, �I�s TimerEventProcessor procedure
			myTimer.Start();
			// end of �]�w �M�ݪ� Timer
		}

		

		private static void FlashLabel_TimerEventProcessor(Object myObject,
				EventArgs myEventArgs) {
				System.Windows.Forms.Form tForm =((KKTimer_Label)myObject).tForm;
				System.Windows.Forms.Label tLabel =((KKTimer_Label)myObject).tLabel;
				KKTimer_Label mytimer=(KKTimer_Label)myObject;

			
			
				// �ˬd�O�_����{�{
				if(mytimer.FlashFirstRun!=true && tForm.Opacity==1.0 ){
					mytimer.Stop();
					mytimer.Dispose();
					return;
				}
				// end of  �ˬd�O�_����{�{

			
				mytimer.FlashC=(mytimer.FlashC+1)%2;
				int FlashC=mytimer.FlashC;
				if(FlashC==0){
					tLabel.Text="�Ȱ�";
				}else{
					tLabel.Text="";
				}
				mytimer.FlashFirstRun=false; // �����, �N���O�Ĥ@������{�{
			
			}
		// end of �Ȱ��ĪG

		// Title �{�{ (�Ȱ��ϥ�0
		public static void Flash_Title(System.Windows.Forms.Form target,int Interval){
			System.Windows.Forms.Form tForm=target;
			KKTimer myTimer;
			// �]�w �M�ݪ� Timer
			myTimer=new KKTimer(tForm);
			myTimer.OpacityLevel=1;
			myTimer.Tick += new EventHandler(FlashTitle_TimerEventProcessor);			
			myTimer.Interval = Interval; // �C�j 1 ����, �I�s TimerEventProcessor procedure
			myTimer.Start();
			// end of �]�w �M�ݪ� Timer
		}

		private static void FlashTitle_TimerEventProcessor(Object myObject,
			EventArgs myEventArgs) {

			System.Windows.Forms.Form tForm =((KKTimer)myObject).tForm;
			KKTimer mytimer=(KKTimer)myObject;

			if(mytimer.FlashFirstRun){
				// �Ĥ@�������, ���C�G��
				tForm.Opacity=0.7;
			}
			
			// �ˬd�O�_����{�{
			if(mytimer.FlashFirstRun!=true && tForm.Opacity==1.0){
				mytimer.Stop();
				mytimer.Dispose();
				return;
			}
			// end of  �ˬd�O�_����{�{

			
			mytimer.FlashC=(mytimer.FlashC+1)%2;
			int FlashC=mytimer.FlashC;
			if(FlashC==0){
				tForm.Text=" �Ȱ����� ... ";
			}else{
				tForm.Text="";
			}
			mytimer.FlashFirstRun=false; // �����, �N���O�Ĥ@������{�{
			
		}
		// end of �Ȱ��ĪG


        // �Ȱ��ĪG
		public static void Flash_Form(System.Windows.Forms.Form target,int Interval){
			System.Windows.Forms.Form tForm=target;
			KKTimer myTimer;
			// �]�w FadeOut �M�ݪ� Timer
			myTimer=new KKTimer(tForm);
			myTimer.OpacityLevel=1;
			myTimer.Tick += new EventHandler(Flash_TimerEventProcessor);			
			myTimer.Interval = Interval; // �C�j 1 ����, �I�s TimerEventProcessor procedure
			myTimer.Start();
			// end of �]�w FadeOut �M�ݪ� Timer 
		}      

		private static void Flash_TimerEventProcessor(Object myObject,
			EventArgs myEventArgs) {

			System.Windows.Forms.Form tForm =((KKTimer)myObject).tForm;
			
			// �ˬd�O�_����{�{
			if(((KKTimer)myObject).FlashFirstRun!=true && tForm.Opacity >0.99 && tForm.Opacity<=1){
				((KKTimer)myObject).Stop();
				((KKTimer)myObject).Dispose();
				return;
			}
			// end of  �ˬd�O�_����{�{

			
			((KKTimer)myObject).FlashC=(((KKTimer)myObject).FlashC+1)%2;
			int FlashC=((KKTimer)myObject).FlashC;
			if(FlashC==0){
				tForm.Opacity=0.5;
			}else{
				tForm.Opacity=0.2;
			}
			((KKTimer)myObject).FlashFirstRun=false; // �����, �N���O�Ĥ@������{�{
			
		}
		// end of �Ȱ��ĪG

		// �H�X�ĪG�åB����
		// Usage
		/*
		 * OpacityUtility.FadeOut_Close(this,30);
		 */
        
		public static void FadeOut_Close(System.Windows.Forms.Form target,int Interval){
			System.Windows.Forms.Form tForm=target;
			KKTimer myTimer;
			// �]�w FadeOut �M�ݪ� Timer
			myTimer=new KKTimer(tForm);
			myTimer.OpacityLevel=1;
			myTimer.Tick += new EventHandler(FadeOut_Close_TimerEventProcessor);			
			myTimer.Interval = Interval; // �C�j 1 ����, �I�s TimerEventProcessor procedure
			myTimer.Start();
			// end of �]�w FadeOut �M�ݪ� Timer 
		}
		private static void FadeOut_Close_TimerEventProcessor(Object myObject,
			EventArgs myEventArgs) {

			System.Windows.Forms.Form tForm =((KKTimer)myObject).tForm;

			double OpacityLevel=((KKTimer)myObject).OpacityLevel;
            if (OpacityLevel > ((KKTimer)myObject).FadeOutFinalValue)
            {
				tForm.Opacity=((KKTimer)myObject).OpacityLevel-=0.05;
				
			}else{
				((KKTimer)myObject).Stop();
				((KKTimer)myObject).Dispose();
				tForm.Close();
				//tForm.Dispose();
			}
		}
		// end of  �H�X�ĪG

		
		// �D���u���H�X�ĪG
		// Usage:
		/*
		 *    OpacityUtility.FadeOut_Only(MyIndexList,30,0);
		 * 
		 */
        public static void FadeOut_Only(System.Windows.Forms.Form target, int Interval, float FadeOutFinalValue)
        {
            System.Windows.Forms.Form tForm = target;
            KKTimer myTimer;
            // �]�w FadeOut �M�ݪ� Timer
            myTimer = new KKTimer(tForm);
            myTimer.FadeOutFinalValue = FadeOutFinalValue;
            myTimer.OpacityLevel = 1;
            myTimer.Tick += new EventHandler(FadeOut_Close_TimerEventProcessor);
            myTimer.Interval = Interval; // �C�j 1 ����, �I�s TimerEventProcessor procedure
            myTimer.Start();
            // end of �]�w FadeOut �M�ݪ� Timer 
        }
		public static void FadeOut_Only(System.Windows.Forms.Form target,int Interval){
			System.Windows.Forms.Form tForm=target;
			KKTimer myTimer;
			// �]�w FadeOut �M�ݪ� Timer
			myTimer=new KKTimer(tForm);
			myTimer.OpacityLevel=1;
			myTimer.Tick += new EventHandler(FadeOut_Only_TimerEventProcessor);			
			myTimer.Interval = Interval; // �C�j 1 ����, �I�s TimerEventProcessor procedure
			myTimer.Start();
			// end of �]�w FadeOut �M�ݪ� Timer 
		}
		private static void FadeOut_Only_TimerEventProcessor(Object myObject,
			EventArgs myEventArgs) {
			System.Windows.Forms.Form tForm =((KKTimer)myObject).tForm;

			double OpacityLevel=((KKTimer)myObject).OpacityLevel;
            // System.Console.WriteLine(tForm.Opacity);
            if (OpacityLevel > ((KKTimer)myObject).FadeOutFinalValue)
            {
				tForm.Opacity=((KKTimer)myObject).OpacityLevel-=0.05;
                
			}else{
				((KKTimer)myObject).Stop();     // ����p�ɾ�
				((KKTimer)myObject).Dispose();  // �����p�ɾ� (�H�K���O�귽)
				tForm.Opacity=((KKTimer)myObject).OpacityLevel=0;// �O�ҳ̫�z���� = 0;
			}
		}
		// end of  �u���H�X�ĪG



		// �D���u���H�J�ĪG
		// Usage:
		/*
		 *    OpacityUtility.FadeIn_Only(MyIndexList,30);
		 * 
		 */
		public double FadeIn_FinalValue;
		public void FadeIn_Only(System.Windows.Forms.Form target,int Interval){
			FadeIn_FinalValue=1.0;
			System.Windows.Forms.Form tForm=target;
			KKTimer myTimer;
			// �]�w FadeOut �M�ݪ� Timer
			myTimer=new KKTimer(tForm);
			myTimer.OpacityLevel=0;       // �]�w�_�l�z���� = 0
			myTimer.Tick += new EventHandler(FadeIn_Only_TimerEventProcessor);			
			myTimer.Interval = Interval; // �C�j 1 ����, �I�s TimerEventProcessor procedure
			myTimer.Start();
			// end of �]�w FadeOut �M�ݪ� Timer 
		}
	
		public void FadeIn_Only(System.Windows.Forms.Form target,int Interval,double TargetValue){
			FadeIn_FinalValue=TargetValue;
			System.Windows.Forms.Form tForm=target;
			KKTimer myTimer;
			// �]�w FadeOut �M�ݪ� Timer
			myTimer=new KKTimer(tForm);
			myTimer.OpacityLevel=0;       // �]�w�_�l�z���� = 0
			myTimer.Tick += new EventHandler(FadeIn_Only_TimerEventProcessor);			
			myTimer.Interval = Interval; // �C�j 1 ����, �I�s TimerEventProcessor procedure
			myTimer.Start();
			// end of �]�w FadeOut �M�ݪ� Timer 
		}

		private void FadeIn_Only_TimerEventProcessor(Object myObject,
			EventArgs myEventArgs) {

			System.Windows.Forms.Form tForm =((KKTimer)myObject).tForm;

			double OpacityLevel=((KKTimer)myObject).OpacityLevel;
			if(OpacityLevel< FadeIn_FinalValue){
				tForm.Opacity=((KKTimer)myObject).OpacityLevel+=0.05;
			}else{
				((KKTimer)myObject).Stop();     // ����p�ɾ�
				((KKTimer)myObject).Dispose();  // �����p�ɾ� (�H�K���O�귽)
				tForm.Opacity=((KKTimer)myObject).OpacityLevel=FadeIn_FinalValue;// �O�ҳ̫�z���� = FadeIn_FinalValue;
			}
		}
		// end of  �H�J�ĪG

	}
}
