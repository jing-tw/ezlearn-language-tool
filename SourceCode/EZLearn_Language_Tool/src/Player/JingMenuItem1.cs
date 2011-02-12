// Advanced .NET Framework Progamming Course

// Target: �Ʊ������� Hello Kitty ���檺 Menu Item

// ���D:
//   1. Main �p�󪾹D �C�@�� Item ���j�p ?
//   MeasureItem Event ?
// Knowledge:
//   1. �i�H�ϥ� MenuItem.DrawItem Event �ۭq Draw Item ���Φ� (OwnerDraw = true) [1,2]


// Reference: 
// 1. ms-help://MS.MSDNQTR.2004JAN.1033/cpref/html/frlrfsystemwindowsformsmenuitemclassownerdrawtopic.htm
// 2. ms-help://MS.MSDNQTR.2004JAN.1033/cpref/html/frlrfsystemwindowsformsmenuitemclassdrawitemtopic.htm
// 3. .Net Framework Source Code http://msdn.microsoft.com/library/default.asp?url=/library/en-us/Dndotnet/html/mssharsourcecli.asp

using System;
using System.Windows.Forms; // for   DrawItemEventArgs 
using System.Drawing; // for Font

namespace Player
{
	/// <summary>
	/// Summary description for JingMenuItem1.
	/// </summary>
	public class JingMenuItem1:System.Windows.Forms.MenuItem {
		Font aFont;

		public JingMenuItem1() {
			//
			// TODO: Add constructor logic here
			//	
			SetCustomDraw();
		}

		
		
		private void SetCustomDraw(){
			this.OwnerDraw=true; // �]�w�n�ۤv�e item 
			this.DrawItem += new DrawItemEventHandler(DrawCustomMenuItem); // �]�w�ۤv�e item ���~�[
			this.MeasureItem+=new MeasureItemEventHandler(MeasureItemEventHandler); // �]�w �����᪺ Item �j�p
			this.Popup+=new EventHandler(PopupEventHandler);
			// �]�w�r��
			aFont = new Font("�ө���", 9, FontStyle.Regular, GraphicsUnit.Point);
		}

		// Step 1: �]�w�ثe Item ���j�p
		public  void MeasureItemEventHandler(object sender,	MeasureItemEventArgs e){
			MenuItem customItem = (MenuItem) sender;
			
			SizeF stringSize = e.Graphics.MeasureString(customItem.Text, aFont);


			// Set the height and width of the item
			e.ItemHeight = (int)stringSize.Height;
			e.ItemWidth = (int)stringSize.Width;
		}
	
		// Step 2: �ۤv�e
		private void DrawCustomMenuItem(object sender, 			DrawItemEventArgs e) {
			MenuItem customItem = (MenuItem) sender;
			System.Drawing.Color BGColor=System.Drawing.SystemColors.Control;
			System.Drawing.Color FGColor=System.Drawing.Color.Green;
			
			
			// Step 2: ���o�r�Ϊ��j�p, ���@�U�n�e����¶�L.
			SizeF astringSize = e.Graphics.MeasureString(customItem.Text, aFont);
			


			// ���o�ثe  Item �����A
			
			if((e.State & DrawItemState.HotLight)==DrawItemState.HotLight){
				// �Y�ثe�O�Q Hot Track
				// Step 1: �إ� ����
				System.Drawing.Brush aBrush = System.Drawing.Brushes.DarkMagenta;

				// Step 3: �e�X���
				e.Graphics.DrawRectangle(new Pen(FGColor),new Rectangle(e.Bounds.X+1, e.Bounds.Y+1, 
					e.Bounds.Width-1,
					e.Bounds.Height-1)); 

				e.Graphics.DrawString(customItem.Text, aFont, aBrush, e.Bounds.X+4, e.Bounds.Y+4);
				
			}else{
				// Step 1: �إ� ����
				System.Drawing.Brush aBrush = System.Drawing.Brushes.Black;

				// Step 3: �M�����
				e.Graphics.DrawRectangle(new Pen(BGColor),new Rectangle(e.Bounds.X+1, e.Bounds.Y+1, 
					e.Bounds.Width-1,
					e.Bounds.Height-1)); 
				

				e.Graphics.DrawString(customItem.Text, aFont, aBrush, e.Bounds.X+4, e.Bounds.Y+4);
			}

			


			

			

			/*
			 * 
			 
			// Step 2: ���o�r�Ϊ��j�p, ���@�U�n�e����¶�L.
			SizeF astringSize = e.Graphics.MeasureString(customItem.Text, aFont);
			SizeF BigSize= e.Graphics.MeasureString(customItem.Text, aFont);
			
			// �åB�b�~���e�W���
			e.Graphics.DrawEllipse(new Pen(System.Drawing.Color.Black, 2),
				new Rectangle(e.Bounds.X, e.Bounds.Y, 
				(System.Int32)stringSize.Width,
				(System.Int32)stringSize.Height));
			
			*/
		}

		
		// PopupEventHandler

		private void PopupEventHandler(object sender, System.EventArgs e){
			
		}


		



	}
}
