using System;
// �l�޵����\�����O
/* Usage:
 * Step 1: �l�����n�H�������, �u�n override �l������ WinProc �Y�i
 
 public DockUtility myDockHelp=new DockUtility(30); // �l�޵������Ϊ���
 protected override void WndProc(ref Message m) {
			int[] NewLoc=new int[2];
			System.Drawing.Rectangle SelfBound=this.Bounds;
			System.Drawing.Rectangle MotherBound=this.myAVPlayer.Bounds;
            myDockHelp.WndProc2(ref m, SelfBound, this.myAVPlayer, NewLoc, this);
			base.WndProc(ref m);
 }
 
  
 * Step 2: �Y�������ʮ�, �]�n���ʳs�����l���� , �u�n�b�������� OnMove �ƥ󤤥[�J�U���o��Y�i
  
  private void AVPlayer_Move(object sender, System.EventArgs e) {
			if(MyIndexList!=null && MyIndexList.myDockHelp.isConnected){
				int movY=this.Location.Y+MyIndexList.myDockHelp.DMY;
				int movX=this.Location.X+MyIndexList.myDockHelp.DMX;
				MyIndexList.Location=new System.Drawing.Point(movX,movY); 
			}
		}
 
 * Step 3: �B�z��������j�Y�p��, �l�����������H�J�H�X, �h�[�J�U���o��
        // ������W�� Control Box �� MinimumBox �Q���U��, �|�I�s�U���o�� Resize Event
        bool bLastMin = false;
        private void AVPlayer_Resize(object sender, EventArgs e)
        {
       
          switch(this.WindowState){
              case FormWindowState.Minimized:
                  // ��������Y�p��, �l�������� FadeOut
                   this.myFileListForm.FadeOut();
             
                   bLastMin = true;
                  break;
              case FormWindowState.Normal:
                  if (bLastMin == true)
                  {
                     // ��������^�_���`��, �l�������^��Ӫ���m. �ê��� FadeIn
                     this.myFileListForm.Location = this.myFileListForm.oldLocation;  //<< �`�N: �l���������n�O��ۤv����m (Load, Move Event)
                     this.myFileListForm.FadeIn();
                    
                      bLastMin = false;
                  }
                  break;
          }
          
           
        }

       // �l����: Load �d��
        private void FileListForm_Load(object sender, EventArgs e)
        {
            // �H�ɰO���ثe��m, 
            if (WindowState == FormWindowState.Normal)
                oldLocation = this.Location;
        }
    
        // �l����: Move �d��
        // ��������ʮ�, �H�ɰO���ثe��m (�]���Y�p��, �|�ݭn�o�Ӧ�m)
        private void FileListForm_Move(object sender, EventArgs e)
        {
            if (Location.X >= 0 && Location.Y >= 0)
                oldLocation = this.Location;
        }
 
 
*/

namespace Player
{
	/// <summary>
	/// Summary description for DuckUtility.
	/// </summary>
	/// 

	
	public class DockUtility
	{
		
		System.Drawing.Rectangle myRec,recDockToTarget;
		int thYGap;

		public bool isConnected=false;
		public DockUtility(int thYGap)
		{
			
			this.thYGap=thYGap;
		}
	
		// �����]�w���H��
		public bool setDock(System.Drawing.Point newLoc,System.Drawing.Rectangle MyBound,System.Drawing.Rectangle recDockToTarget){
			return this.isDock(newLoc.Y,newLoc.X,new int[]{newLoc.Y,newLoc.X},MyBound,recDockToTarget);
		}

		//int newY,newX;
		public bool isDock(int curY,int curX,int[] NewYX,System.Drawing.Rectangle MyBound,System.Drawing.Rectangle recDockToTarget){
			this.myRec=MyBound;
			this.recDockToTarget=recDockToTarget;

			NewYX[0]=curY;
			NewYX[1]=curX;
			isConnected=false;
			if(isBottomNearMTop(thYGap,curY,myRec,recDockToTarget)){
				// �P�_ X �y�ЬO�_�b�����������k�d��
				if(curX+myRec.Width >= recDockToTarget.Left && curX <= recDockToTarget.Left+recDockToTarget.Width){
					NewYX[1]=curX;
					NewYX[0]=recDockToTarget.Location.Y-myRec.Height;
					isConnected=true;
					return true;
				}else{
					isConnected=false;
					return false;
				}
			}

			if(isTopNearMBottom(thYGap,curY,myRec,recDockToTarget)){
				// �P�_ X �y�ЬO�_�b�����������k�d��
				if(curX+myRec.Width >= recDockToTarget.Left && curX <= recDockToTarget.Left+recDockToTarget.Width){
					NewYX[0]=recDockToTarget.Top+recDockToTarget.Height;
					NewYX[1]=curX;
					isConnected=true;
					return true;
				}else{
					isConnected=false;
					return false;
				}
			}

			if(isLeftNearMRight(thYGap,curX,myRec,recDockToTarget)){
				// �P�_ Y �y�ЬO�_�b���������W�U�d��
				if(curY>= recDockToTarget.Top-myRec.Height && curY <= recDockToTarget.Top+recDockToTarget.Height){
					NewYX[1]=recDockToTarget.Location.X+recDockToTarget.Width;
					NewYX[0]=curY;
					isConnected=true;
					return true;
				}else{
					isConnected=false;
					return false;
				}
			}

			if(isRightNearMLeft(thYGap,curX,myRec,recDockToTarget)){
				// �P�_ Y �y�ЬO�_�b���������W�U�d��
				if(curY>= recDockToTarget.Top-myRec.Height && curY <= recDockToTarget.Top+recDockToTarget.Height){
					NewYX[1]=recDockToTarget.Location.X-myRec.Width;
					NewYX[0]=curY;
					isConnected=true;
					return true;
				}else{
					isConnected=false;
					return false;
				}
			}
			return false;
		}
		
		private bool isBottomNearMTop(int thYGap,int myY,System.Drawing.Rectangle myRec,System.Drawing.Rectangle recDockToTarget){
			//int myY=this.Location.Y+this.Height;
			if(System.Math.Abs(myY+myRec.Height-recDockToTarget.Location.Y) < thYGap){
				return true;
			}
			return false;
		}
		private bool isTopNearMBottom(int thYGap,int myY,System.Drawing.Rectangle myRec,System.Drawing.Rectangle recDockToTarget){
			// int myY=this.Location.Y;
			if(System.Math.Abs(myY-(recDockToTarget.Location.Y+recDockToTarget.Height)) < thYGap){
				return true;
			}
			return false;
		}

		private bool isLeftNearMRight(int thXGap,int myX,System.Drawing.Rectangle myRec,System.Drawing.Rectangle recDockToTarget){
			// int myX=this.Location.X;
			if(System.Math.Abs(myX-(recDockToTarget.Location.X+recDockToTarget.Width)) < thXGap){
				return true;
			}
			return false;
		}


		private bool isRightNearMLeft(int thXGap,int myX,System.Drawing.Rectangle myRec,System.Drawing.Rectangle recDockToTarget){
			// int myX=this.Location.X+this.Width;
			if(System.Math.Abs(myX+myRec.Width-recDockToTarget.Location.X) < thXGap){
				return true;
			}
			return false;
		}

		public  struct WINDOWPOS {
			public System.IntPtr hwnd;
			public	System.IntPtr hwndInsertAfter;
			public	int x;
			public	int y;
			public	int cx;
			public	int cy;
			public	System.UInt32 flags;
		}

		public int DMY,DMX;
		public unsafe void WndProc(ref System.Windows.Forms.Message m,System.Drawing.Rectangle myRec,System.Drawing.Rectangle recDockToTarget,int[] NewLoc,System.Windows.Forms.Form test) {
			
			// �o�ӱ`�ƬO�b winuser.h ��쪺
			const int  WM_WINDOWPOSCHANGING     =       0x0046;
			//const int SWP_NOSIZE          = 0x0001;
            //const int SWP_NOMOVE       =   0x0002;

			switch (m.Msg) {
				case WM_WINDOWPOSCHANGING:
					WINDOWPOS* pos=(WINDOWPOS*)m.LParam;
					int y=pos->y;
					int x=pos->x;
						
					//  �P�_ �ثe���쪺�T���� Z-order ���ഫ, ���Χ��� isConnected �����A
					if(pos->flags==3)
						break;

					if(isDock(y,x,NewLoc,myRec,recDockToTarget)){
						// ��� Message �� ������m
						pos->y=NewLoc[0];
						pos->x=NewLoc[1];
						 // end of ��������m

						isConnected=true;

						DMY=NewLoc[0]-recDockToTarget.Location.Y; // �P���������Z��
						DMX=NewLoc[1]-recDockToTarget.Location.X; // �P���������Z��

						//test.Text="c="+this.isConnected+" DMY="+this.DMY+" DMX="+this.DMX;
					}else{

						//test.Text="c="+this.isConnected+" DMY="+this.DMY+" DMX="+this.DMX;
						
						isConnected=false;
                        
						
					}
					break;		
		
				//case WM_NCPaint:
					
				//	break;
			}
		
			// base.WndProc(ref m);
		}

        
        // ��J�ثe�y��, �ثe�X���T, Parent�X���T
        // �۰ʽվ��m�H�� Parent
        public bool DockMain(System.Windows.Forms.Form myForm,int curY, int curX, System.Drawing.Rectangle myRec, System.Drawing.Rectangle MotherFormRec, int[] retNewLoc)
        {
            if (isDock(curY, curX, retNewLoc, myRec, MotherFormRec))
            {
                

                isConnected = true;

               /* 
                * DMY = retNewLoc[0] - recDockToTarget.Location.Y; // �P���������Z��
                DMX = retNewLoc[1] - recDockToTarget.Location.X; // �P���������Z��
                */
                myForm.Left = retNewLoc[1];
                myForm.Top = retNewLoc[0];
                // ������T
                //test.Text="c="+this.isConnected+" DMY="+this.DMY+" DMX="+this.DMX;

                return true;
            }
            else
            {
                // ������T
                //test.Text="c="+this.isConnected+" DMY="+this.DMY+" DMX="+this.DMX;

                isConnected = false;

                return false;
            }
        }
        

		public unsafe void WndProc2(ref System.Windows.Forms.Message m,System.Drawing.Rectangle myRec,System.Windows.Forms.Form MotherForm,int[] NewLoc,System.Windows.Forms.Form test) {
			
			// �o�ӱ`�ƬO�b winuser.h ��쪺
			const int  WM_WINDOWPOSCHANGING     =       0x0046;
			//const int SWP_NOSIZE          = 0x0001;
			//const int SWP_NOMOVE       =   0x0002;
            System.Drawing.Rectangle MotherFormRec;
            if (MotherForm.Menu == null)
            {
                // int MainMenuHeight=32;
                int MainMenuHeight = 0;
                // MotherFormRec = new System.Drawing.Rectangle(MotherForm.Left, MotherForm.Top, MotherForm.Width, MotherForm.Height - MainMenuHeight);
                MotherFormRec = MotherForm.Bounds;
            }
            else
            {
                // System.Drawing.Rectangle MotherFormRec = new System.Drawing.Rectangle(MotherForm.Left, MotherForm.Top, MotherForm.Width, MotherForm.Height);
                MotherFormRec = MotherForm.Bounds;
            }

            // �u�B�z���`���p (�䤤���@�ӵ����٨S�X�{, ������ΨӧP�_�O�_�s��)
            if (myRec.Y < 0 || myRec.X < 0 || MotherFormRec.X < 0 || MotherFormRec.Y < 0)
                return;

			switch (m.Msg) {
				case WM_WINDOWPOSCHANGING:
					WINDOWPOS* pos=(WINDOWPOS*)m.LParam;
											
					//  �P�_ �ثe���쪺�T���� Z-order ���ഫ, ���Χ��� isConnected �����A
					if(pos->flags==3)
						break;

                    int y = pos->y;
                    int x = pos->x;
                    
                    if (x != 0 && y != 0)
                    {
                        if (isDock(y, x, NewLoc, myRec, MotherFormRec))
                        {
                            // ������� Message ����������m (unsafe ����])
                            pos->y = NewLoc[0];
                            pos->x = NewLoc[1];
                            // end of ��������m

                            isConnected = true;

                            DMY = NewLoc[0] - recDockToTarget.Location.Y; // �P���������Z��
                            DMX = NewLoc[1] - recDockToTarget.Location.X; // �P���������Z��

                            // ������T
                            //test.Text="c="+this.isConnected+" DMY="+this.DMY+" DMX="+this.DMX;
                        }
                        else
                        {
                            // ������T
                            //test.Text="c="+this.isConnected+" DMY="+this.DMY+" DMX="+this.DMX;

                            isConnected = false;

                        }
                    }
					break;		
		
					//case WM_NCPaint:
					
					//	break;
			}
		
			// base.WndProc(ref m);
		}

        // �N�ثe�� Form �H�� parent �����䤤������m
        public static void MoveToMotherLeftMiddle(System.Windows.Forms.Form curForm, System.Windows.Forms.Form MotherForm)
        {
            curForm.Left = MotherForm.Left - curForm.Width;
            curForm.Top = MotherForm.Top + MotherForm.Height / 2 - curForm.Height / 2;
        }

        // �N�ثe�� Form �H�� parent ���k�䤤������m
        public static void MoveToMotherRightMiddle(System.Windows.Forms.Form curForm, System.Windows.Forms.Form MotherForm)
        {
            curForm.Left = MotherForm.Left + MotherForm.Width;
            curForm.Top = MotherForm.Top + MotherForm.Height / 2 - curForm.Height / 2;
        }

        // �N�ثe�� Form �H�� parent ���U����m
        public static void MoveToMotherBottom(System.Windows.Forms.Form curForm, System.Windows.Forms.Form MotherForm)
        {
            curForm.Left = MotherForm.Left;
            curForm.Top = MotherForm.Bottom;
        }
	}// end of Utility
}
