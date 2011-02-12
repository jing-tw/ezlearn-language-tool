

// --------------- Utility.h -----------
HRESULT GetPin( IBaseFilter * pFilter, PIN_DIRECTION dirrequired, int iNum, IPin **ppPin);
HRESULT PrintAllFilter(IGraphBuilder *pGB);// �L�X Grpah Manager���޲z���Ҧ� Filters �W��
HRESULT ShowFilterInfo(IBaseFilter *filter); // ��� Filter �����

void DXShowError(HRESULT hr); // Show the error message
// �[�J�@�� Filter �� ���w�� filter graph ��
HRESULT AddFilterByCLSID(
    IGraphBuilder *pGraph,  // [in] �ثe�� Filter Graph Manager
    const GUID& clsid,      // [in] �n�[�J�� Filer class id   �Ҧp: CLSID_AviDest
    LPCWSTR wszName,        // [in] �A�� Filter �n�s���W�r
    IBaseFilter **ppF);      // [out] �^�� Filter instance ��}

// �s����� Filters
HRESULT ConnectFilters(
    IGraphBuilder *pGraph, // [in] ���w��� Filters �Ҧb�� Filter Graph Manager
    IPin *pOut,            // [in] ���w upstream filter ����X pin ���}
    IBaseFilter *pDest) ;
IPin * GetOutPin( IBaseFilter * pFilter, int nPin );

// �s����� Filters
HRESULT ConnectDirectFilters(
    IGraphBuilder *pGraph, // [in] ���w��� Filters �Ҧb�� Filter Graph Manager
    IPin *pOut,            // [in] ���w upstream filter ����X pin ���}
    IBaseFilter *pDest,    // [in] ���w downstream filter.
	const AM_MEDIA_TYPE *pmt)  ;

// �s����� Filters [�������w Filter ����]
HRESULT ConnectDirectFilters(
    IGraphBuilder *pGraph,  // [in] ���w��� Filters �Ҧb�� Filter Graph Manager
    IBaseFilter *pSrc,      // [in] ���w upstream filter 
    IBaseFilter *pDest,
	const AM_MEDIA_TYPE *pmt) ;    // [in] ���w downstream filter

// ����@�� Graph
HRESULT PlayFileWait(IFilterGraph *pFG);

TCHAR *getTimeFormatMessage(GUID *pFormat); // �Ǧ^ TimeFormat ��r��� (�����ϥ�);

// XVID CLSID=64697678-0000-0010-8000-00AA00389B71
DEFINE_GUID(CLSID_XVID,
0x64697678, 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xaa, 0x00, 0x38, 0x9b, 0x71);

// The Following Setting should be added to your project property
// Library Setting
// Library Path 
// F:\Program Files\Microsoft Platform SDK for Windows Server 2003 R2\Samples\Multimedia\DirectShow\Filters\PushSource\Debug_Unicode
// Library Dependencies: PushSource.lib
// --------------- end of PushSource  ------------