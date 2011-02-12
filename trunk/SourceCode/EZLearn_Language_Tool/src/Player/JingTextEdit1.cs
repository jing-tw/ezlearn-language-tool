// Advanced .NET Framework Progamming Course
// �{���]�p�ޥ�: �S���Y�� Message   (If the control recognized the message, it will be consumed by the preprocess function)
//                                                                                 by ������  8/1/2005
// 
// �ϥ� public override bool PreProcessMessage
// ��n�S����, �Ǧ^ true .
// ��ܳo�� control �w�g�B�z�o�� message �F, ���ݭn�A�i�@�B���B�z
// �`�N:
//      �A�b WndProc �i���d�I�O�S���Ϊ�!!


using System;
using System.Windows.Forms;  // for WinProc 

namespace Player
{
	/// <summary>
	/// Summary description for JingTextEdit1.
	/// </summary>
	public class JingTextEdit1:System.Windows.Forms.TextBox {
		AVPlayer myPlayer;

		public bool bIsExtendedMode=false; // �O�_�ثe�� Seek(,��L��T)* ���A

		public JingTextEdit1()
		{
			
		}
		public bool isExtendeMode(){
			if(this.Text.IndexOf(",")!=-1)
				return true;
			else
				return false;
		}

	    // ================================== ����ʵ{�����q ==============================
		public override bool PreProcessMessage(
			ref Message m
			){

           
			const int  WM_KEYDOWN      =       0x0100;
			bool bDeleted=false; // �O�_�n�S��
			switch (m.Msg) {
				case WM_KEYDOWN:
					System.IntPtr vkc=m.WParam;
					int charCode=(int)vkc;

					if(!(charCode >='0' && charCode <='9')){
						bDeleted=!AllowCode(charCode); // �Y���O�Ʀr �]���O ���\���������, �h�S��
						
						// bDeleted=true;                  // ���O�Ʀr�������Q�S�� 
					}
					break;

			}

			if(bDeleted){
				return bDeleted;   // �o�� message �Q�B�z���F, �Ǧ^ false. �᭱���ʧ@�����Q�S��
			}
		
			return base.PreProcessMessage(ref m);

		}
		// ================================== end of ����ʵ{�����q ==============================

		// ���\�U�C�\����: 
		//        Ender , �W,�U,��,�k �� ���\����, �n�~��Q�B�z
        // �P IndexListForm.cs ���� listBox1_KeyDown method ����
		private bool AllowCode(int charCode){
			const int Enter=13; // ��J����
			const int Up=38; // ���ޤW
			const int Down=40; // ���ޤU
            const int ShiftKey = 16; // Shift ��ܥߧY���sť�ثe�����q   11/29/2006

			const int Left=37; // �ֳt�V�e����
			const int Right=39; // �ֳt�V�����
			const int BackSpace=8; //��J�ץ�
			const int DelKey=46; // ��J�ץ�      8/18/2005  (�Ȱ��w�g�令 Ctrl Key)
			const int Space=32; // �Ȱ�
			const int Dot=190; // �p���I
			const int Comma=188; // �r�� 9/10/2005
			const int R='R'; // �V�e���޼Ҧ� = R �_�l���,�V�e���ު����,Repeat,����

            const int Ctrl = 17; // Ctrl �Ȱ� // �]���ťշ|�Q��J��@ edit ������   7/24/2007
            

            // '\' ��ܥߧY���sť�ثe�����q   11/29/2006
            if (charCode == ShiftKey) {
                // �Y�ثe�O�Ȱ�, �ҥH�{�b�O����
                if (this.myPlayer.isPause())
                {
                    this.myPlayer.PauseOrPlay(); 
                }
                else
                {  // �_�h�����i�����
                    myPlayer.SeekPlay(); // �i�歫�s���ު��u�@
                }
                this.myPlayer.label2.Text = this.myPlayer.L_Manager.getMessage("IndexListForm_System_ListenAgain");// "�Ať�@��";
                return false; // �S�� BackSlash �r������ܳB�z
            }

            if (myPlayer.state == 1) { // �w�]���A�O 0:����  1: �Ȱ�  2: �����
				// �Ȱ���, �u���\ [�ť���]
				// if(charCode==Space){
                if(charCode==Ctrl){ // 7/24/2007
					return true;
				}else{
					return false; // �S�� �ť� �T��
				}
			}

			// �Y�ϥΪ̫� Enter ��, �n���ˬd��J�O�_���Ʀr
			if(charCode==Enter){
				if(this.Text.IndexOf(",")==-1){ //  �X�R�榡���A: �}��Ҧ���J, �o�ӥi�H���ϥΪ̿�J����
					try {
						Double.Parse(this.Text);
						return true;
					}catch(Exception){
						this.myPlayer.label2.Text="��J���X�k";
						return false; // ��J���O�Ʀr, �S�� Enter �T��
					}
				}

				bIsExtendedMode=false;  // ��_����l�Ҧ�(�ϥΪ̥i�ध�e�O��J�X�R�Ҧ�, �o�|���Ҧ���J�����\)
				return true;
			}
			// end of 

            if (charCode == Up || charCode == Down)
            {
                this.myPlayer.bMySectionBasedPlay = true;
            }

			if(charCode==Comma){
				// �{�b�B�� �X�R�榡���A: �}��Ҧ���J, �o�ӥi�H���ϥΪ̿�J����
				bIsExtendedMode=true;
			}


			if(bIsExtendedMode==false){ // ��l���A: �u�����\ Seek ��T�i�J
				if(charCode==R || charCode==DelKey|| charCode==Dot|| charCode==Up || 
                   charCode==Down || charCode == Left || charCode == Right || charCode==BackSpace ||
                   charCode == Ctrl) {
					return true;
				}
				else{
					return false;
				}
				
			}else{
				return true;
			}

           

		}
		

		/* 
		    // �A�b WndProc �i���d�I�O�S���Ϊ�!!
			protected override void WndProc(ref Message m) {
				const int  WM_KEYDOWN      =       0x0100;
				bool bValid=true;
				switch (m.Msg) {
					case WM_KEYDOWN:
						System.IntPtr vkc=m.WParam;
						bValid=false;
						string k=m.ToString();
						break;

				}
				if(bValid)
					base.WndProc(ref m);
			}
	*/
		
		
		public void setPlayer(AVPlayer argPlayer){
			myPlayer=argPlayer;
		}

	}
}
