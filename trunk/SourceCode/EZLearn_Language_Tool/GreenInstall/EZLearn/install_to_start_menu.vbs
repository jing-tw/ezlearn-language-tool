	'�b�ثe�ؿ��إ� shortcut
	Set oWS = WScript.CreateObject("WScript.Shell")
	sLinkFile = oWS.CurrentDirectory+"\EZLearn.LNK"
    Set oLink = oWS.CreateShortcut(sLinkFile)
	oLink.TargetPath = oWS.CurrentDirectory+"\EZLearn.exe"
    oLink.WorkingDirectory = oWS.CurrentDirectory' �]�w���|���u�@���|: �P�����ɤ@��   (�p�G�����w ,�h�w�]���|��  C:\Documents and Settings\�ϥΪ̱b��)
	oLink.Save
	
	'�� shortcut �T�w��}�l menu
	Set objShell = CreateObject("Shell.Application")
    Set objFolder = objShell.Namespace(oWS.CurrentDirectory)
    Set objApp = objFolder.ParseName("EZLearn.LNK")
    If objApp Is Nothing Then
        MsgBox ("Executable not found.  Make sure the path and file name are correct.")
    Else
        objApp.InvokeVerb ("P&in to Start Menu")
    End If

	' �ѦҸ��:
	'http://www.vbforums.com/showthread.php?t=534820
	