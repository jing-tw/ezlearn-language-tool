#include "stdafx.h"
#include <streams.h>

TCHAR *getTimeFormatMessage(GUID *pFormat); // �Ǧ^ TimeFormat ��r��� (�����ϥ�);
// --------------------------- Utility ---------------------------


// �L�X Grpah Manager���޲z���Ҧ� Filters �W��
HRESULT PrintAllFilter(IGraphBuilder *pGB){
	HRESULT hr;
    // IEnumFilters *pEnum = NULL;
    IBaseFilter  *pFilter = NULL;
	// CComPtr<IBaseFilter> pFilter=NULL;
	CComPtr<IEnumFilters> pEnum=NULL;

//    IPin *pPin;
	ULONG ulFetched;

	 // Verify graph builder interface
    if (!pGB)
        return E_NOINTERFACE;

	// Get filter enumerator
    hr = pGB->EnumFilters(&pEnum);
    if (FAILED(hr))
        return hr;

    pEnum->Reset();

	 // Enumerate all filters in the graph
    while((pEnum->Next(1, &pFilter, &ulFetched) == S_OK)){
        // Read filter name for debugging purposes
        FILTER_INFO FilterInfo;
        TCHAR szName[256];
    
        hr = pFilter->QueryFilterInfo(&FilterInfo);
        if (SUCCEEDED(hr)){
            // Show filter name 
#ifdef UNICODE
            lstrcpyn(szName, FilterInfo.achName, 256);
#else
            WideCharToMultiByte(CP_ACP, 0, FilterInfo.achName, -1, szName, 256, 0, 0);
#endif
            FilterInfo.pGraph->Release();
			// pFilter->Release();
        }
		pFilter->Release();
		TRACE(_T("Filter name=%s\n"),szName);
        szName[255] = 0;        // Null-terminate

	}
	//pEnum->Release();
	return hr;
}

// ��� Filter �����
HRESULT ShowFilterInfo(IBaseFilter *filter){
	FILTER_INFO info;
	HRESULT  hr=filter->QueryFilterInfo(&info);
	if(FAILED(hr))
    {
        return hr;
    }

	// �ݤ@�U vendor �O��
	LPWSTR VerdorInfo;
	hr=filter->QueryVendorInfo(&VerdorInfo);
	if(FAILED(hr))
    {
        return hr;
    }
}

void ShowConnectError(HRESULT hr ){
	TCHAR *err=_T("ok");
	switch(hr){
		case VFW_S_PARTIAL_RENDER:
			err=_T("Partial success; some of the streams from this pin use an unsupported format.");
			break;
		case E_ABORT:
			err=_T("Operation aborted.");
			break;
		case E_POINTER:
			err=_T("NULL pointer argument.");
			break;
		case VFW_E_CANNOT_CONNECT:
			err=_T("No combination of intermediate filters could be found to make the connection.");
			break;
		case VFW_E_NOT_IN_GRAPH:
			err=_T("At least one of the filters is not in the filter graph");
			
			break;
		case S_OK:
			break;
	}

	MessageBox(0, err, TEXT("Error!"), MB_OK | MB_ICONERROR);

}

void DXShowError(HRESULT hr){
   
        TCHAR szErr[MAX_ERROR_TEXT_LEN];
        DWORD res = AMGetErrorText(hr, szErr, MAX_ERROR_TEXT_LEN);
        if (res == 0){
			size_t cbDest = MAX_ERROR_TEXT_LEN * sizeof(TCHAR);
            StringCbPrintf(szErr,cbDest,_T("Unknown Error: 0x%2x"),hr);	
        }
        MessageBox(0, szErr, TEXT("Error!"), MB_OK | MB_ICONERROR);
}

// ���X�٨S���s���� Pin ���}
HRESULT GetUnconnectedPin(
    IBaseFilter *pFilter,   // [in] ���w�n�B�z�� Filter
    PIN_DIRECTION PinDir,   // [in] ���w�n���o�����}��V
    IPin **ppPin)           // [out] �Ǧ^�� pin ���} 
{
    *ppPin = 0;
    IEnumPins *pEnum = 0;
    IPin *pPin = 0;
    HRESULT hr = pFilter->EnumPins(&pEnum);
    if (FAILED(hr))
    {
        return hr;
    }
    while (pEnum->Next(1, &pPin, NULL) == S_OK)
    {
        PIN_DIRECTION ThisPinDir;
        pPin->QueryDirection(&ThisPinDir);
        if (ThisPinDir == PinDir)
        {
            IPin *pTmp = 0;
            hr = pPin->ConnectedTo(&pTmp);
            if (SUCCEEDED(hr))  // Already connected, not the pin we want.
            {
                pTmp->Release();
            }
            else  // Unconnected, this is the pin we want.
            {
                pEnum->Release();
                *ppPin = pPin;
                return S_OK;
            }
        }
        pPin->Release();
    }
    pEnum->Release();
    // Did not find a matching pin.
    return E_FAIL;
}

// �s����� Filters
HRESULT ConnectFilters(
    IGraphBuilder *pGraph, // [in] ���w��� Filters �Ҧb�� Filter Graph Manager
    IPin *pOut,            // [in] ���w upstream filter ����X pin ���}
    IBaseFilter *pDest)    // [in] ���w downstream filter.
{
    if ((pGraph == NULL) || (pOut == NULL) || (pDest == NULL))
    {
        return E_POINTER;
    }
#ifdef debug
        PIN_DIRECTION PinDir;
        pOut->QueryDirection(&PinDir);
        _ASSERTE(PinDir == PINDIR_OUTPUT);
#endif

    // Find an input pin on the downstream filter.
    IPin *pIn = 0;
    HRESULT hr = GetUnconnectedPin(pDest, PINDIR_INPUT, &pIn);
    if (FAILED(hr))
    {
        return hr;
    }
    // Try to connect them.
    hr = pGraph->Connect(pOut, pIn);
	// pGraph->ConnectDirect(pOut,pIn,pOut->
	if (FAILED(hr))
    {
        // ShowConnectError(hr);
		return hr;
    }
	
    pIn->Release();
    return hr;
}



// �s����� Filters (�������n����L�� Filter)
HRESULT ConnectDirectFilters(
    IGraphBuilder *pGraph, // [in] ���w��� Filters �Ҧb�� Filter Graph Manager
    IPin *pOut,            // [in] ���w upstream filter ����X pin ���}
    IBaseFilter *pDest,    // [in] ���w downstream filter.
	const AM_MEDIA_TYPE *pmt)    
{
    if ((pGraph == NULL) || (pOut == NULL) || (pDest == NULL))
    {
        return E_POINTER;
    }
#ifdef debug
        PIN_DIRECTION PinDir;
        pOut->QueryDirection(&PinDir);
        _ASSERTE(PinDir == PINDIR_OUTPUT);
#endif

    // Find an input pin on the downstream filter.
    IPin *pIn = 0;
    HRESULT hr = GetUnconnectedPin(pDest, PINDIR_INPUT, &pIn);
    if (FAILED(hr))
    {
        return hr;
    }
    // Try to connect them.
    // hr = pGraph->Connect(pOut, pIn);
	hr = pGraph->ConnectDirect(pOut,pIn,pmt);
	if (FAILED(hr))
    {
        ShowConnectError(hr);
		return hr;
    }
	
    pIn->Release();
    return hr;
}


// �s����� Filters (�������n����L�� Filter)
HRESULT ConnectDirectFilters(
    IGraphBuilder *pGraph,  // [in] ���w��� Filters �Ҧb�� Filter Graph Manager
    IBaseFilter *pSrc,      // [in] ���w upstream filter 
    IBaseFilter *pDest,
	const AM_MEDIA_TYPE *pmt)     // [in] ���w downstream filter
{
    if ((pGraph == NULL) || (pSrc == NULL) || (pDest == NULL))
    {
        return E_POINTER;
    }

    // Find an output pin on the first filter.
    IPin *pOut = 0;
    HRESULT hr = GetUnconnectedPin(pSrc, PINDIR_OUTPUT, &pOut);
    if (FAILED(hr)) 
    {
        return hr;
    }

    hr = ConnectDirectFilters(pGraph, pOut, pDest,pmt);
	if (FAILED(hr)) 
    {
		ShowConnectError(hr);
        return hr;
    }

    pOut->Release();
    return hr;
}


// �s����� Filters [�������w Filter ����]
HRESULT ConnectFilters(
    IGraphBuilder *pGraph,  // [in] ���w��� Filters �Ҧb�� Filter Graph Manager
    IBaseFilter *pSrc,      // [in] ���w upstream filter 
    IBaseFilter *pDest)     // [in] ���w downstream filter
{
    if ((pGraph == NULL) || (pSrc == NULL) || (pDest == NULL))
    {
        return E_POINTER;
    }

    // Find an output pin on the first filter.
    IPin *pOut = 0;
    HRESULT hr = GetUnconnectedPin(pSrc, PINDIR_OUTPUT, &pOut);
    if (FAILED(hr)) 
    {
        return hr;
    }

    hr = ConnectFilters(pGraph, pOut, pDest);
	if (FAILED(hr)) 
    {
		ShowConnectError(hr);
        return hr;
    }

    pOut->Release();
    return hr;
}

void ShowFilenameByCLSID(REFCLSID clsid, TCHAR *szFilename, size_t len)
{
    HRESULT hr;
    LPOLESTR strCLSID;

    // Convert binary CLSID to a readable version
    hr = StringFromCLSID(clsid, &strCLSID);
    if(SUCCEEDED(hr))
    {
        TCHAR szKey[512];
        //CString strQuery(strCLSID);
		// LPCTSTR strQuery=strCLSID;
		CComBSTR strQuery(strCLSID);
		CW2CT szMyString( strQuery );

        // Create key name for reading filename registry
        hr = StringCchPrintf(szKey, NUMELMS(szKey), TEXT("Software\\Classes\\CLSID\\%s\\InprocServer32\0"),
                 (LPCTSTR) szMyString);

        // Free memory associated with strCLSID (allocated in StringFromCLSID)
        CoTaskMemFree(strCLSID);

        HKEY hkeyFilter=0;
        DWORD dwSize=MAX_PATH;
        BYTE szFile[MAX_PATH];
        int rc=0;

        // Open the CLSID key that contains information about the filter
        rc = RegOpenKey(HKEY_LOCAL_MACHINE, szKey, &hkeyFilter);
        if (rc == ERROR_SUCCESS)
        {
            rc = RegQueryValueEx(hkeyFilter, NULL,  // Read (Default) value
                                 NULL, NULL, szFile, &dwSize);

            if (rc == ERROR_SUCCESS)
                hr = StringCchPrintf(szFilename, len, TEXT("%s\0"), szFile);
            else
                hr = StringCchCopy(szFilename, len, TEXT("<Unknown>\0"));

            RegCloseKey(hkeyFilter);
        }
    }
}


// �[�J�@�� Filter �� ���w�� filter graph ��
HRESULT AddFilterByCLSID(
    IGraphBuilder *pGraph,  // [in] �ثe�� Filter Graph Manager
    const GUID& clsid,      // [in] �n�[�J�� Filer class id   �Ҧp: CLSID_AviDest
    LPCWSTR wszName,        // [in] �A�� Filter �n�s���W�r
    IBaseFilter **ppF)      // [out] �^�� Filter instance ��}
{
    if (!pGraph || ! ppF) return E_POINTER;
    *ppF = 0;
    IBaseFilter *pF = 0;
    HRESULT hr = CoCreateInstance(clsid, 0, CLSCTX_INPROC_SERVER,
        IID_IBaseFilter, reinterpret_cast<void**>(&pF));
    if (SUCCEEDED(hr))
    {
        hr = pGraph->AddFilter(pF, wszName);
        if (SUCCEEDED(hr))
            *ppF = pF;
        else
            pF->Release();
    }
    return hr;
}

HRESULT GetPin( IBaseFilter * pFilter, PIN_DIRECTION dirrequired, int iNum, IPin **ppPin)
{
    CComPtr< IEnumPins > pEnum;
    *ppPin = NULL;

    if (!pFilter)
       return E_POINTER;

    HRESULT hr = pFilter->EnumPins(&pEnum);
    if(FAILED(hr)) 
        return hr;

    ULONG ulFound;
    IPin *pPin;
    hr = E_FAIL;

    while(S_OK == pEnum->Next(1, &pPin, &ulFound))
    {
        PIN_DIRECTION pindir = (PIN_DIRECTION)3;

        pPin->QueryDirection(&pindir);
        if(pindir == dirrequired)
        {
            if(iNum == 0)
            {
                *ppPin = pPin;  // Return the pin's interface
                hr = S_OK;      // Found requested pin, so clear error
                break;
            }
            iNum--;
        } 

        pPin->Release();
    } 

    return hr;
}

IPin * GetOutPin( IBaseFilter * pFilter, int nPin )
{
    CComPtr<IPin> pComPin;
    GetPin(pFilter, PINDIR_OUTPUT, nPin, &pComPin);
    return pComPin;
}

IPin * GetInPin( IBaseFilter * pFilter, int nPin )
{
    CComPtr<IPin> pComPin;
    GetPin(pFilter, PINDIR_INPUT, nPin, &pComPin);
    return pComPin;
}




HRESULT PlayFileWait(IFilterGraph *pFG)
{
    CheckPointer(pFG,E_POINTER);

    HRESULT hr;
    IMediaControl *pMC=0;
    IMediaEvent   *pME=0;

	IMediaSeeking *pSeek=0;


    hr = pFG->QueryInterface(IID_IMediaControl, (void **)&pMC);
    if(FAILED(hr))
    {
        return hr;
    }

    hr = pFG->QueryInterface(IID_IMediaEvent, (void **)&pME);
    if(FAILED(hr))
    {
        pMC->Release();
        return hr;
    }

	hr = pFG->QueryInterface(IID_IMediaSeeking, (void **)&pSeek);
    if(FAILED(hr))
    {
        return hr;
    }

	
    OAEVENT oEvent;
    hr = pME->GetEventHandle(&oEvent);
    // if(SUCCEEDED(hr))
	for(int i=0;i<5&&SUCCEEDED(hr);i++)
    {
        hr = pMC->Run();

        if(SUCCEEDED(hr))        {

            LONG levCode;
			hr = pME->WaitForCompletion(INFINITE, &levCode);

			// �p�G Graph ���� Seek �\��, �h���Y����
			DWORD dwCap = 0;
			hr = pSeek->GetCapabilities(&dwCap);
			if(FAILED(hr))    {
				return hr;
			}
			if (AM_SEEKING_CanSeekAbsolute & dwCap){
				// Graph can seek to absolute positions.
				// �o�� Graph ���� Seek ���\��

				// ���o Time Format �榡
				GUID TimeFormat;
				hr=pSeek->GetTimeFormat(&TimeFormat);
				if(FAILED(hr))    {
					return hr;
				}
				TCHAR *FormatMessage=getTimeFormatMessage(&TimeFormat);

				// ���o Stop ��m
				LONGLONG Stop;
				hr=pSeek->GetStopPosition(&Stop);
				if(FAILED(hr))    {
					return hr;
				}

				// �]�w�q�Y����
				LONGLONG Current=0;
				hr=pSeek->SetPositions(&Current,AM_SEEKING_AbsolutePositioning,&Stop,AM_SEEKING_AbsolutePositioning);
				if(FAILED(hr))    {
					return hr;
				}
			}
		}
	}
	pSeek->Release();
	pMC->Release();
	pME->Release();

    return hr;
}

// �Ǧ^ TimeFormat ��r��� (�����ϥ�)
TCHAR *getTimeFormatMessage(GUID *pFormat){

	if(pFormat->Data1 ==  TIME_FORMAT_NONE.Data1){
		return _T("No format");
	}
	
	if(pFormat->Data1 == TIME_FORMAT_FRAME.Data1){
		return _T("Video frames");
	}

	if(pFormat->Data1 == TIME_FORMAT_SAMPLE.Data1){
		return _T("Samples in the stream");
	}

	if(pFormat->Data1 == TIME_FORMAT_FIELD.Data1){
		return _T("Interlaced video fields");
	}

	if(pFormat->Data1 ==  TIME_FORMAT_BYTE.Data1){
		return _T(" Byte offset within the stream");
	}

	if(pFormat->Data1 ==  TIME_FORMAT_MEDIA_TIME.Data1){
		return _T(" Reference time (100-nanosecond units)");
	}

	return _T("Unknown time format");

}

// ------------------------ end of Utility -----------------------