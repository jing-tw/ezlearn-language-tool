 EZLearn �y���ǲ߾�                                                  �@�̺���: ������ http://debut.cis.nctu.edu.tw/~ching
 
�M�׺���: http://debut.cis.nctu.edu.tw/~ching/Course/JapaneseLanguageLearner/__page/JapaneseLanguageLearner.htm
  
�w��:
   �����Ѷ}���� (�A�������w�� .net framework) 
    .net framework �Ա��Ш�: http://debut.cis.nctu.edu.tw/~ching/Course/JapaneseLanguageLearner/__page/Download2.htm

����:
   �����R��



���§A���ϥ�.
�w�ﴣ�ѥ���\��W����ĳ


��s����
2006 �~�H�᪺����, �� 
http://debut.cis.nctu.edu.tw/~ching/Course/JapaneseLanguageLearner/__page/UpdateNote.htm



10/1/2005 Preleased �����o�G (Jenny ��쪺���D�P�n�D) see ���� Update Note
1.�Ȱ������s�|�ñ�,���յ��G�O���εe���٨S�X�{�N���Ȱ��ҾɭP => fixed
2.�}�ɤ�����,��scroll bar�|�� ==> fixed
3.�Ȱ����䥦�ɮ�,�w�}�l������o��ܼȰ�,�ɭP�@�}�l�L�k���� => fixed
4.����R�����w������ 
=> �b list ������ [Del] ��, �Y�i�R��
=> �s�W�R�����s
5.��}��L��,�������,�����񤤪����ַ|�Qreset,�Ȱ����]�|�Qreset,�|�n�D�x�s����,�Y�藍�s,���޸�Ʒ|�Qreset
=> fixed

9/24/2005
1. ������ Option �� skin �d�Ҥ]�i�H�� skin (�R�O�J�� �n�D���\��)
2. �Y�ϥΪ̨S���}��, �����N���s�|�y�����D ==> �ץ�

9/23/2005
1. �y�����q���������ѨC�����˴��@���令�C0.2 ���˴��@�� ==> �X�R�榡�i�H�ϥκ�Ǩ�p���I
   
9/18/2005
PCHome Toget �n��

9/16/2005
[New]
1. �W�[�ƧǦ�C���\��

9/14/2005
[New]
1. �W�[�i�H�ק�s��y�����q�\�� (Now, the Section list can be editing.)
[Bug]
1. �X�R��J�榡�b�̫�q���|�۰ʸ��� 0 ==> �ץ� (The extended repeat mode's bug was fixed)
2. �Ȱ��|�۰ʱN��J�欰�M���� 0 => �ץ� 


9/13/2005
Modify
1. Add a new attribute "right" to the Status Label of user defined skin.
2. �ϥΪ̫��w�� skin �w�g�Q�����_�ӤF. Now, the system can remember the skin setting.

[Bugs]
1. �H���������S�� menu �|����������{�H => �ץ�


9/10/2005
a. �s�W���ƥ\�� (�������� tt_man ��ĳ���\��)
   �ϥΪ̥i�H���w�y�����q��
                �_�l�I�ɶ�,�����ɶ�,���Ƽ��񦸼�,����
�d�Ҥ@
      113       <-- �ǲΪ����w�y���_�l�ɶ�
�d�ҤG
      113,115   <-- ���w�����ɶ�. ���F�����ɶ���, �۰ʸ��U�@�Ӭq��
�d�ҤT
      113,115,20 <-- ���w���м��񦸼�, ���񧹲�, �۰��~�򼷤U�h
�d�ҥ|
      113,115,20,Ū��s�D <--�b���ަ�C�W�Хܵ���
     

9/6/2005
a. �s�W�ѨϥΪ̫��w���Ҫ��C��
example:
    <!-- ��� label -->                             
    <TimeLabel x="58" y="50" R="255" G="255" B="255"/>
 
9/5/2005
[Bugs]
a. Skin �������ഫ���D, �� XML Skin �ѨM
b. �W�[�s�� skin => iPod-Mini

9/3/2005
�Q�� XML �]�w skin ���u�@����

--------------------------------------------------------------
8/12/2005
a. [�ƹ��ާ@] �ƹ��i�H�b����������a��, ������즲��������. (�� skin �@�ǳ� => �p�G�� skin, �h�����W�����D�h��������, �H�������[)
   �@�k:
        �Q�ε{�������ƹ��즲���ʧ@
        
   // ====================================== ����{�����q ====================================
   // [�ƹ��\��] ��ƹ����ʮ�
		bool DRAGGING=	true;
		bool m_dwFlags=false;
		int Py,Px;
		private void AVPlayer_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e) {
			if ( m_dwFlags && DRAGGING ) {
				this.Left+=e.X-Px;
				this.Top+=e.Y-Py;
			}
		}

		//[�ƹ��\��] ��ƹ�����u�_��
		private void AVPlayer_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e) {
			switch (e.Button) {
				case MouseButtons.Left:
					if ( m_dwFlags && DRAGGING ) {
	                               // �Y�ثe�O dragging ���A
						// m_dwFlags &= ~DRAGGING;
						m_dwFlags=!DRAGGING;
					}
					break;
			}
		}

		
		// [�ƹ��\��] ��ƹ�������U��
		private void AVPlayer_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e) {
			switch (e.Button) {
				case MouseButtons.Left:
					// if ( !(m_dwFlags & DRAGGING) ) {
					if ( !(m_dwFlags && DRAGGING) ){
	                               // �Y�ثe���O dragging ���A
						Py=e.Y; Px=e.X;

						// m_dwFlags |= DRAGGING;
						m_dwFlags=DRAGGING;
					}
					break;
			}

		}
		
   // ====================================== end of ����{�����q ====================================
   
8/8/2005
a. [��ı��] �վ� context menu.  (�� skin �@�ǳ� => �D��楲������, �ѷƹ��k����N)

8/3/2005
a. [��ı��] ���F�Ϫ����󰮲b, 
            1. �N�M����C�P�R�����w��ƥ\��X�֨� OptionForm ��, �����W�����������s�M��
            2. �N Pause �P Resume �ⶵ�\���X�b�@�ӫ��s��, �H�`�٪���
            3. �D�����������
b. [�q���t��]
            1. ���F���q���t�m�󶶺Z, �ϥΪ̥i�H���w �q�����@�I�X�{
c. [�q���t�� Bug]
            1. �� �q������ �P �@��y���m�� ��X���D ==> �ץ�

8/1/2005
a.
[�{���X�j��] ����ϥΪ̥��J���X�k����J
    1. �]�p�s����J textBox ����, ���N������@�뤸��
    �B�z����: ��ϥΪ̿�J���X�k�������, �ߧY�S���� message. ������ message �~��U�h.
    
    2. �ϥΪ̼v�T: ������J���X�k����J, �p('a') ������, �|�i�J�򥻨����{��, ���ͤ@�Ӵ�������, �n�D�ϥΪ̭ק��J.
             <�ץ�> �H�k��J�ڥ��N���|��ܥX��, �ϥΪ̤��ݭn�ק�������J
             
[�����{���X����]  �S�� Window Message    (see JingtextEdit1.cs)
  
b. �ק��U�ﶵ����r, �ϱo������M��!!

c. �ק�Ȱ��ĪG: �אּ ���D�{�{. �Ȱ��N�O�ϥΪ̭n�@��L�Ʊ�, ����������{�{�ӤޤH�`��, �Ϧӥ��h�Ȱ����ηN. 

07/21/2005
[�\��վ�] Update ����
    1. ���s������, ��ܦb Player ���������D�W

07/19/2005
[�\��s�W] �ϥΪ̥i�H�ۦ�󴫳��w���I��
[�y���վ�] 1. �M���������y���վ� (���ǭ��|�����q�q��) -> �ץ�
          
07/16/2005
[�\��վ�] �C���Ұʳ��|�X�{�ˬd����������, �u���ܰQ��. ===> �ק令�X�ѥu�|�ˬd�@��
����:
     �b�O���ɤ�, �[�J���� Update �����. ���� Update �\���, ���ˬd�O�_�P�W������@��, �p�G�@�˴N���ˬd.

07/11/2005
[�\��s�W] �ϥΪ̫��k��/����, �ֳt���U/���W���� 1 �� (�ϥ���L�N�i����, ���ݨϥηƹ��즲 scroll bar)

[Bug �ץ�] ���ɷ|�X�{�L�n�����p���D (jungping ���X)
�i���]: �u���]�w���񾹰_�l�P������m, �|�ϼ��񾹤�����]�Ȱ�
�ثe�ѨM����: ������񾹨åB�j���������ި���w���а_�I
�Բӵ{���X:
   // �������ި� start, �קK�L�n���p�o�� 
		ourAudio.CurrentPosition=start;
   // end of �קK�L�n���p�o��
���A: �ڪ����� 1.4G NB �w�g�S���L�n�����p, ���ݨ�L���ͪ��^��(�w������t�פ֩� 1 GHz������)

					
07/08/2005
[�\��s�W] �����۰��ˬd�s����
           * �C���Ұʳ��|�۰��ˬd�O�_���s������
           * �ˬd����, �۰��������� (�ɶq���n�ѤF��@�o�ӳn�骺�򥻥ت�: ²��)
           * �i�H�H�������o�ӥ\��
           
           
[�ʾ�]  �p�����ϥΪ̪��D�n�骺�ʳ��w�g�Q��s�γQ�`�N, �O�����n���Ʊ�.  
        �ר�O�������s���n��Ө�, �q���ϥΪ̦��s����, ��O�@�ӭ��j�����D. 
        �o�˥i�H�קK�ϥΪ̤@�����򪺨ϥΦ� Bug ������. 
        �P�ɨϥΪ̤]�S������h�ɶ�, �H�ɺʱ��O�_���s�������X�{.
        
[Bug �ץ�]
1.���ɷ|�X�{�L�n�����p���D (jungping ���X)
��]: �D�n�O�i��O�]�����K�� Microsoft.DirectX.AudioVideoPlayback �ե�Ұʮɶ��ܺC, �ɭP���ɷ|�L�n. 
      �o�ز{�H�b�t�׸��C�������W, �|�����. �̲׸ѨM��׬O���s�Q�� C++ �ϥ� DirectShow SDK �g�@��
      �󦳼u�ʪ�����, �ǥH���� AudiovideoPlayback. ���O�o�ӳ����i��n�d��᭱�A�B�z.
      
      �ثe���ѨM��׬O�ھ� AudioVideoPlayback ���S��, �����N���񧹦��᪺�ʧ@. 
      �� ����� �令 ���򼽩� �@�ӵL�n���q, �ӧ�i�Ұʮɶ��C�����D, �]���o�˪��ʧ@�N�|�� AudiovideoPlayback
      �S�������P�Ұʪ��ʧ@. �ݬݬO�_��β{�����귽�ﵽ�ثe�D�J�����D. �i�ӹF��̤j���}�o�įq.
      
      �򥻤W, �g�L�ڭק��. �ڪ�����(1.4 GHz) �w�g�S���o�{�L�n�����D�F. ���O�����D��L���ͬO�_�ٷ|�D�J�o�Ӱ��D?
      
���A: ����ѨM��



06/29/2005
[Bug �ץ�]
1. �� list box ���W�U��, �|���ʨ⦸�� bug
��]: listbox �|�۰ʳB�z UP �P Down, �o�޵o�F value changed �ƥ�
      �i�ӾɭP curTextIndex �[���ƨ⦸
���A: �ץ�
  
=========================================================================================================
[���� Released]   
06/27/2005
[�\��s�W]
1. ���r��50���� �[�J�y���\��
�ʾ�: �C���ѰO�Ÿ�, �N�n½��. �ݵۨ��ӹ���ù��������X��, 
      �`�O�Pı��ꪺ.�ۤv�쩳�᪺�Ф��зǪ����D�@���X�{�b����.
      
      �g�@�ӵ{��, ������w���y��. �����ѱ�ù�������a.

[Bug]
1. ����y��: �����ѨM���ɷ|�L�n�����p


06/26/2005
[�\��s�W]
1. ���r���u��]�t �M��,�B��,�b�B��, �� 
2. �Ʀr��ƪ���� (�ϥΪ̥i�H�ۦ�e�ۤv���Ʀr)

[Bug �ץ�]
1. �ץ� �{�Ѥ��r���u�� ����b�Y�q�ާ@�{�ǤU,�|�o�� null reference �����D
2. �üƧ令�üƱƦC: �O�ҨϥΪ̫� 45*2 �U��, �Ҧ������Ÿ��O�ҥX�{�@��
3. �L�n�����p, �ץ�
��]�O : Clicked �ƥ󤣬O��í�w, �ҥH��� �ƹ�������U����P�ƹ��i�J, 
�o�Ӱ��D���G�ﵽ����. (�����٬O�|���o�Ӱ��D,�o���٦b���])

4. �b���礭�Q�����[(���Q�����a�H)��A���X�@�ӿ��~���,  �ץ�
�o�Ӱ��D�b�� �ڪ��{���޿���~, �� Exit �ɷ|����⦸ CloseGame() . 
�o�� function, �o�譱�ڤw�g�ץ�, �Ʊ椣�|�o�ͦP�˪����~. 

5. �����ɼ�����~���D, �ץ�



06/20/2005
[Bug �ץ�]
1. �{�Ѥ��r���u�� ������ �P ������ ���s�R�W, �H�K�y���x�Z
2. "�l�޵����\��" ���ʮ�, Bug �ץ� 
3. list �����b�S�w�}�ɶ��ǤU�|�X���D, Bug �ץ�


06/17/2005
[�s�\�� �l�[]
 1. �� List ���� �P Game �������� �M Winamp �����@�˪�"�l�޵����\��"
    ��l�����a���������, �|�۰ʾa�W�h

06/15/2005
[�s�\�� �l�[]
 1. �{�Ѥ��r���u��
 [�`�N: �]���ۧ@�v�����Y, �ҥH���{�������ѥ���y���ɮ�. 
        �Ʊ����ϥλy���ѵ����B��, �Цۦ���s�P�]�w�r�����_�l����]

 �y�������ɽd�ҤU��: PhoneticAudio.wma.txt
 ���ջy����:(�_�Ǫ��Ʀr ???)  
 ������, �N�o����ɮש�b�����ɪ��ؿ��U, �Y�i�Ұʻy���ѵ����\��
 (���ջy���� PhoneticAudio.wma �����ըϥ�, �Цb 24 �p�ɤ�����, �_�h��G�ۦ�t�d)
 
�ʾ�: 
�D�n�O�]���ڭI�F 50 ����, �C���b �q���W�ݨ쪺����٬O�L�k�ήɤ���, �٦b�Q���ɭ�, �e�����N�L�F. 
�ҥH�ڷQ�i��O�]�� 50 ������ܬO�����Ǫ�, �q���W�X�{�����Ÿ�, �i�H�����ü�, �ҥH�ڤ~�L�k�ݨ�r�N���X������. 

�]�N�]���p��,�h�U�M�߰��ܼg�@�ӫD�`²�檺���Q���m�ߤu��. �Ʊ��d�w 50 ��. 

�o�Ӥu��.�򥻤W�|�åX�r��, ����A�|���|��, 
��J�줣�|�����Ÿ���, 
���@�U�N�i�H���D���T��k, �A�i�H�ۤv����ۤv��. 

�C�Ѫ��� 2 ����, �ݬݯण��ﵽ�ڪ����D. �]�N���K���ɵ��j�a��. 

�]���᪺�ɶ����h, �ӥB�S���O�ӷ~�n��, �� Testing �ζ�, 
�ҥH���K�i��|�� Bug. 

��A�o�{�� Bug ��, �ٽЦh�h�]�[.  


06/14/2005
[�s�\�� �l�[]
 1. �W�[ �ϥΪ̦�C�޲z���� [�۰ʸ��J�W��������] [�x�s����] 
    �U���}�����s�y���ɮ�, �|�۰ʸ��J���e���O��

06/11/2005
[�s�\�� �l�[]
 1. �M�����w�y������
 
[Bug ��]
 1. �� Option ��, �� [����] �|���� Null Reference �����D
 2. �� Option ��, �� [����] �� [�T�w] ��, �����èS���u�����������D