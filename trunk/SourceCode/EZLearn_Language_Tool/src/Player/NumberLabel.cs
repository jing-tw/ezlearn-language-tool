// �ۭq�����d��: ��ܦۤv�e���Ʀr label
//                                                                ������
// �Ʀr bmp �R�W�榡: 
//       Num_0.bmp,..., Num_9.bmp
// Usage:
/*
            // �]�� Bitmap ����Ū�����ɮ�, �|���������� Dispose �~�|��},
			// �ҥH��������ɻP�Ʀr�ާ@�޿���}�B�z.
			//  1. ��Ū��(���)���� ==> ��@�귽
			//  2. �浹�Ʀr�ާ@�޿� ���
			
            // �ۭq�Ʀr����
			imgNumArray=NumberLabel.CreateNumImage(ExecutationFileDir,"Num",0,0,0);
			l1=new NumberLabel(imgNumArray);
			l1.Left= 136;
			l1.Top=48;
			l1.Height=30;   // <<--  �����ӥѨϥΪ̫��w���� (��s��, �o�����ӭ�����s)
			l1.SetWidth(4); // ���|�ռƦr
			this.Controls.Add(l1);
			// end of �ۭq�Ʀr
 */
using System;
using System.Windows.Forms;   // �ϥ� PaintEventArgs

namespace Player {
	/// <summary>
	/// Summary description for NumberLabel.
	/// </summary>
	public class NumberLabel:System.Windows.Forms.Label {
		
		public int NumTab=2; // �Ÿ��P�Ÿ��������Z��
		
		// �e�X�����ı��H
		protected override void OnPaint(PaintEventArgs e) {
			// base.OnPaint(e); // �e�X��Ӫ��F�� (���e�X�n�O�n�e���{��)
			

			/*
			 System.Drawing.Rectangle rec=e.ClipRectangle;
			// ���յe���
			e.Graphics.FillRectangle(new System.Drawing.SolidBrush(System.Drawing.Color.Red),0,0,rec.Width/2,rec.Height/2);
			// end of �e���
			
			// �ХX���󪺽d��
			e.Graphics.DrawRectangle(new System.Drawing.Pen(System.Drawing.Color.Blue,1),0,0,rec.Width-1,rec.Height-1);
			// end of ����	
			
             */

			// ���յe�v��
			PaintText(e.Graphics,0,0);
			// end of �e�v��

				
		}
		// end of ����

		


		// ���C��]�w
		private void init(){
			this.BackColor=System.Drawing.Color.Transparent; // ���w�I���C�⬰�z����
			
		}

		

		
		// �Ʀr���󪺫غc�l
		System.Drawing.Bitmap[] imgNumArray;
		public NumberLabel(System.Drawing.Bitmap[] imgNumArray){
			init(); // ���C��]�w,�}�C�t�m
			this.imgNumArray=imgNumArray;
		}
		// end of �غc�l

		// �̾ڭn��ܪ��Ʀr�ӼƨM�w���󪺼e��
		public void SetWidth(int SymbolNum){
			int W=SymbolNum*(imgNumArray[0].Width +NumTab);
			this.Width=W;
		}

		public static System.Drawing.Bitmap[] CreateNumImage(string ImagePath,string strNumImage,int bgR,int bgG,int bgB){
			return LoadNumImage(ImagePath,strNumImage,System.Drawing.Color.FromArgb(bgR,bgG,bgB));
		}

		// ===========================   �Ʀr�v���B�z������ ========================
		
		
		// ���J�Ʀr�v��, ���w�z����
		private static System.Drawing.Bitmap[]  LoadNumImage(string ImagePath,string strNumImage,System.Drawing.Color TransparentColor){ 
			const int imgNum=10;
			System.Drawing.Bitmap[] imgNumArray;
/*
			if(imgNumArray==null){
				imgNumArray=new System.Drawing.Bitmap[imgNum];
			}
			*/
			//int NeedWidth=0;
			imgNumArray=new System.Drawing.Bitmap[imgNum];
			
				for(int i=0;i<imgNum;i++){
					string filename= strNumImage+"_"+i+".bmp";//  Num_1
					// �ɷ|�Q lock, ���쪫��Q�P��
					// �ɮץi��Q��L�������
					//     imgNumArray[i]=new System.Drawing.Bitmap(filename); // �ثe�u���J imgNum �i�v��
					System.IO.FileStream  r=new System.IO.FileStream(ImagePath+"\\"+filename,System.IO.FileMode.Open);
					imgNumArray[i]=new System.Drawing.Bitmap(r);
					imgNumArray[i].MakeTransparent(TransparentColor); // ���w�Y�@���C�⬰�z���C��
					r.Close(); // ������y    8/17/2005
				}
		
			return imgNumArray;
		}

		// �e�X�@�ӼƦr  (�|�Q PaintText �I�s)
		private void PaintNumImage(System.Drawing.Graphics g,int y,int x,int NumIndex){
			System.Drawing.Image img=imgNumArray[NumIndex];
			if(img!=null){
				g.DrawImage(img,new System.Drawing.Point(x,y));
			}
			
		}

		
		// �e�X Text  (�|�Q OnPaint �I�s)
		private void PaintText(System.Drawing.Graphics g,int y,int x){
			string strTarget=this.Text;
			int xc=x;
			for(int i=0;i<Text.Length;i++){
				char c=strTarget[i];
				if(c >='0' && c<='9'){
					int NumIndex=c-(int)'0'; // ���X�Ʀr

					if(NumIndex<imgNumArray.Length ){ // �ˬd�Ʀr�v���O�_�w�g���J
						PaintNumImage(g,y,xc,NumIndex); // �e�X�Ʀr
						xc+=imgNumArray[NumIndex].Width+NumTab; // ���ʵe�U�@��
					}
					
				}
			}
		}

		//===========================   end of �Ʀr�v���B�z������ ========================

		// �̥k��Ʀr��x�y�� (�۹��ۤv����) �i���Ѩ�L����, �H�b�᭱��ܪ��ƾ�)
		// k = �� k �Ӧ�m
		// Usage:
		/*
				string strSec=""+ipos; // ��Ƹ�T
				myPlayer.label1.Left=myPlayer.l1.Left+myPlayer.l1.getRightMostX(strSec.Length); // �N���׸�T�H�b l1 ��Ƹ�T�᭱
				myPlayer.label1.Text=""+ilength+" sec";
				myPlayer.l1.Text=strSec;  // �ۭq�Ʀr�������
		 */
		public int getRightMostX(int k){
			int xc=0;
			for(int i=0;i<k;i++){
				xc+=imgNumArray[i].Width+NumTab; // ���ʨ�U�@��
			}
			return xc;
		}


		

		/*
		// �O����޲z�譱�٤��O�ܼ�,�n�T���A�� Dispose ���B�@
		// managed object �ݭn �ۤv dispose ��?
		bool disposed=false;
		protected override void Dispose( bool disposing ){
			if(!this.disposed){
				if(disposing){
					// �R�� managed code �Ұt�m���귽
					//FreeImage(disposing );
				}
				base.Dispose();
				disposed=true;
			}	
		}
	   */

		/*
		// ����v���O���� (�|�Q Dispose �I�s)
		private void FreeImage( bool disposing ) {
			if(!this.disposed){
				for(int i=0;i<imgNumArray.Length;i++){
					if(imgNumArray[i]!=null){
						imgNumArray[i].Dispose();// ����v��
						imgNumArray[i]=null;
					}
				}
				disposed=true;
			}
		}
		 */

		
		/*
		// �B�z�I���C��
		protected override void OnPaintBackground(PaintEventArgs pevent){
			// 1. �p�G���@����ʧ@, �N�|�O �¦⪺�I��
			this.BackColor=System.Drawing.Color.Transparent; // ���w�I���C�⬰�z����
			base.OnPaintBackground(pevent);
			
		}
		*/


	}
}