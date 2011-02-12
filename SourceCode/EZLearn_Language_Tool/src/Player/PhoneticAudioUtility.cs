// ��y�o������

using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
// ����y������
using Microsoft.DirectX;
using Microsoft.DirectX.AudioVideoPlayback;
// end of �y��

// Usage
/*
strPhoneticDirectory=(string)myPlayer.PropertyTable["�����ɹw�]�ؿ�"];
strPhoneticFilename=(string)myPlayer.PropertyTable["�����ɦW��"];// "PhoneticAudio.wma";
PhoneticAudioUtility symbolVoice=new PhoneticAudioUtility(strPhoneticDirectory,strPhoneticFilename);

// ����M�� 
symbolVoice.PlaySymbol(0,1); // '��'
*/

namespace Player
{
	/// <summary>
	/// Summary description for PhoneticAudioUtility.
	/// </summary>
	public class PhoneticAudioUtility
	{
		public Audio ourAudio; // ����y���M�Ϊ���
		string strPhoneticDirectory; // �����ɹw�]�ؿ�
		string strPhoneticFilename; // �����ɹw�]�ɦW "PhoneticAudio.wma";
		string strPhoneticIndexFilename; // ������ (�O���C�Ӧr�������i���ަ�m) [�� Phonetic_AudioReady ����]
		double [][]PhoneticIndex;       // ���Я����ɯ��ް}�C��� [�� ReadPhoneticIndex ���͸��]

		private bool bPhoneticAudioReady; // �y���ɬO�_�ǳƦn�F [PlaySymbol �|�ˬd]


		// ���r���Ÿ��w�q (�����W,�����W)
		private char[]PhoneticSymbols1={'��','��','��','��','��',
										   '��','��','��','��','��',
										   '��','��','�@','�B','�D',

										   '�F','�H','�K','�M','�O',
										   '�Q','�R','�S','�T','�U',
										   '�V','�Y','�\','�_','�b',

										   '�e','�f','�g','�h','�i',
										   '�k','�m','�o',
										   '�p','�q','�r','�s','�t',

										   '�v','�y',
									   
										   '�|','�~','Ǣ','Ǥ','Ǧ',
										   'ǧ','ǩ','ǫ','ǭ','ǯ',
										   'Ǳ','ǳ','ǵ','Ƿ','ǹ',

										   'ǻ','ǽ','��','��','��',
										   '��','��','��','��','��',
										   '��','��','��','��','��',

										   '��','��','��','��','��',
										   '��','��','��',
										   '��','��','��','��','��',
										   '��','��'
									   };

		// �B�� (��)
		private string P2="���������������A�C�E�G�I�L�N�P�W�Z�]�`�c";
		// �B�� (��)
		private string P3="ǨǪǼǮǰǲǴǶǸǺǼǾ�������������`��";
		private string PhoneticSymbols2;

		// �b�B��(��)
		private string P4="�X�[�^�a�d";
		//�b�B��(��)
		private string P5="����������";
		private string PhoneticSymbols3;

		//��(��)
		private string P6="���j ���l ���n ���j ���l ���n �H�j �H�l �H�n �R�j �R�l �R�n �Y�j �Y�l �Y�n �f�j �f�l �f�n �q�j �q�l �q�n ���j ���l ���n ���j ���l ���n �Z�j �Z�l �Z�n �[�j �[�l �[�n ";
		//��(��)
		private string P7="ǩ�� ǩ�� ǩ�� ǳ�� ǳ�� ǳ�� ǽ�� ǽ�� ǽ�� ���� ���� ���� ���� ���� ���� ���� ���� ���� ���� ���� ���� Ǫ�� Ǫ�� Ǫ�� Ǵ�� Ǵ�� Ǵ�� ���� ���� ���� ���� ���� ���� ";
		private string PhoneticSymbols4;
		// end of �r���Ÿ��w�q

		public PhoneticAudioUtility(string strPhoneticDirectory,string strPhoneticFilename)
		{
			this.strPhoneticDirectory=strPhoneticDirectory;
			this.strPhoneticFilename=strPhoneticFilename;

			// �Ҧ����r��
			PhoneticSymbols2=P2+P3; // �B�� �Ÿ���
			PhoneticSymbols3=P4+P5; // �b�B�� �Ÿ���
			PhoneticSymbols4=P6+P7; // �� �Ÿ���
			// end of �Ҧ����r��

			// Step 1:
			bPhoneticAudioReady=Phonetic_AudioReady();
			if(bPhoneticAudioReady!=true){
				string message="�]���ۧ@�v�����Y,\n1. �A�����n���ۤv����y�y���n����: filename.xxx\n2. �ۦ���w�U�Ÿ�����k�b�y���ɩҦb����m: filename.xxx.txt\n      �_�l���:�������";
				MessageBox.Show(message);
			}else{

			// Step 2:
				bool bOk=ReadPhoneticIndex(); // ���J������
				if(bOk!=true){
					string message="�y���ɮ榡���~";
					MessageBox.Show(message);
				}
			}			
		}

		// Step 1: �ˬd�����ɻP�����ɬO�_�s�b
		// �`�N: �]���ۧ@�v�����Y, 
		//      1. �A�����n���ۤv����y�y���n����: filename.xxx
		//		2. �ۦ���w�U�Ÿ�����k�b�y���ɩҦb����m: filename.xxx_index.txt
		//              �_�l���:�������
		private bool Phonetic_AudioReady(){
			// �P�_�����ɬO�_�s�b
			string strPhoneticFullFilename=strPhoneticDirectory+"\\"+strPhoneticFilename;
			strPhoneticIndexFilename=strPhoneticDirectory+"\\"+strPhoneticFilename+"_index.txt";

			if(System.IO.File.Exists(strPhoneticFullFilename) && System.IO.File.Exists(strPhoneticIndexFilename)){
				ourAudio = new Audio(strPhoneticFullFilename); // ���J������
				ourAudio.Ending += new System.EventHandler(this.ClipEnded); // �]�w���񵲧���, �n�p�󰱤��
				return true;
			}else{
				if(System.IO.File.Exists(strPhoneticFullFilename) !=true){
					MessageBox.Show("�����ɤ��s�b: "+strPhoneticFullFilename+ "\n\n�ݬݬO���O�|���F?");
				}

				if(System.IO.File.Exists(strPhoneticIndexFilename) !=true){
					MessageBox.Show("�����ɤ��s�b: "+strPhoneticIndexFilename+"\n\n�����ɮ榡:  filename.xxx_index.txt");	
				}
				return false;
			}
		}

		// Step 2: Ū��������
		private int VoiceIndexNum=0; // ��ڥ��ɮפ�Ū�쪺�y�����
		private bool ReadPhoneticIndex(){
			// �]�w�O����
			int TotalVoiceNum=PhoneticSymbols1.Length/2+P2.Length+P4.Length+P6.Length;
			PhoneticIndex=new double[TotalVoiceNum][];
			for(int i=0;i<TotalVoiceNum;i++){
				PhoneticIndex[i]=new double[2];
			}
			// end of �O����

			// ���ɮפ�, ���J property
			try {
				//string FullIndexFilename=strPhoneticDirectory+"//"+strPhoneticFilename+".txt";
				using (System.IO.StreamReader sr = new System.IO.StreamReader(strPhoneticIndexFilename)) {
					String line;
					
					int i=0;
					while ((line = sr.ReadLine()) != null) {
						// ���L����
						if(line.StartsWith("//") || line.StartsWith(" ") ||line.Length<3)
							continue;
						// end of ����
						

						int StartIndex=line.LastIndexOf(":");
						string strStart=line.Substring(0,StartIndex);// ���X�_�l���
						string strEnd=line.Substring(StartIndex+1);// �������

						PhoneticIndex[i][0]=Double.Parse(strStart);
						PhoneticIndex[i][1]=Double.Parse(strEnd);
						i++;
						VoiceIndexNum++;
					}
				}
				return true;
			}
			catch (Exception e) {
				Console.WriteLine("�����ɮ榡�����T");
				Console.WriteLine(e.Message);
				return false;
			}
			
		}

		// Step 3: �����n��
		// �Ѽƻ���:
		// SymbolClass=0 �M��, 1 �B��, 2 �b�B��, 3 ��
		// index �Ÿ���m
		public void PlaySymbol(int SymbolClass, int index){
			
			int ClassstartBase=0;

			if(bPhoneticAudioReady!=true){
				string message="�]���ۧ@�v�����Y,\n1. �A�����n���ۤv����y�y���n����: filename.xxx\n2. �ۦ���w�U�Ÿ�����k�b�y���ɩҦb����m: filename.xxx.txt\n      �_�l���:�������";
				MessageBox.Show(message);
			}else{
				
				switch(SymbolClass){
					case 0:
						ClassstartBase=0;
						index=index%(PhoneticSymbols1.Length/2);
						break;
					case 1:
						ClassstartBase=PhoneticSymbols1.Length/2; // (�B��) �b�M�����U��
						index=index%(PhoneticSymbols2.Length/2);
						break;
					case 2:
						ClassstartBase=PhoneticSymbols1.Length/2+P2.Length; // �b�B��
						index=index%(PhoneticSymbols3.Length/2);
						break;
					case 3:
						ClassstartBase=PhoneticSymbols1.Length/2+P2.Length+P4.Length; // ��
						index=index%(P6.Length/3);
						break;
					default:
						break;
				}
				index=index+ClassstartBase;

				if(index < VoiceIndexNum){
					double start=PhoneticIndex[index][0];   // 10.5 sec
					double end=PhoneticIndex[index][1];

					double StartSeek=start*10000000;
					double EndSeek=end*10000000;
				
					ourAudio.SeekCurrentPosition(StartSeek,Microsoft.DirectX.AudioVideoPlayback.SeekPositionFlags.AbsolutePositioning);
					ourAudio.SeekStopPosition(EndSeek,Microsoft.DirectX.AudioVideoPlayback.SeekPositionFlags.AbsolutePositioning);
				
					// �������ި� start, �קK�L�n���p�o�� 07/11/2005
					ourAudio.CurrentPosition=start;
					// end of �קK�L�n���p�o��

					ourAudio.Play();
				}else{
					System.Windows.Forms.MessageBox.Show("�o�Ӧr���n�����q�|�����w\n������=PhoneticAudio.wma123_index.txt");
				}
			}
		}// end of play

		private void ClipEnded(object sender, System.EventArgs e) {
			
			if(ourAudio!=null && ourAudio.Disposed!=true){
				if(ourAudio.Playing==true){
					// �i��O�]�� AudioVideoPlayback ���ҰʤӺC, �|�Ӥ��εo��,�ɭP���ɭԨϥΪ̥i��|�H���S���o��
					// �ץ�: �令�`������ťջy���q�ݬݬO�_��ѨM�o�Ӱ��D
					//ourAudio.Stop(); 

					int start=70;
					int end=72;
					double StartSeek=start*10000000;
					double EndSeek=end*10000000;
				
					ourAudio.SeekCurrentPosition(StartSeek,Microsoft.DirectX.AudioVideoPlayback.SeekPositionFlags.AbsolutePositioning);
					ourAudio.SeekStopPosition(EndSeek,Microsoft.DirectX.AudioVideoPlayback.SeekPositionFlags.AbsolutePositioning);
					
					ourAudio.CurrentPosition=start;
					
				}
			}
			
			
		}
	}
}
