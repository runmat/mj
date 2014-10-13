// touch.cpp : Defines the entry point for the console application.
//

#include "stdafx.h"

// Matches the command line arguments/options
#define OPT_MODIFY_ATIME       (1 << 0)
#define OPT_MODIFY_MTIME       (1 << 1)
#define OPT_MODIFY_CTIME       (1 << 2)
#define OPT_NO_CREATE	       (1 << 3)
#define OPT_USE_TEMPLATE       (1 << 4)
#define OPT_USER_TIME          (1 << 5)
#define OPT_DIR_IF_NOT_EXIST   (1 << 6)

// The magic function which does all the touching
static DWORD touch(LPCTSTR lpszFile, FILETIME* atime, FILETIME* mtime, FILETIME* ctime, WORD wOpts)
{
	SetLastError(ERROR_SUCCESS);

	DWORD dwFileAttributes = GetFileAttributes(lpszFile);
	DWORD dwErr = GetLastError();
	if(dwFileAttributes == INVALID_FILE_ATTRIBUTES && dwErr != ERROR_FILE_NOT_FOUND)
		return dwErr;

	bool bUpdateOrCreateDirectory =
		(OPT_DIR_IF_NOT_EXIST & wOpts) && dwFileAttributes == INVALID_FILE_ATTRIBUTES ||
		dwFileAttributes & FILE_ATTRIBUTE_DIRECTORY && dwFileAttributes != INVALID_FILE_ATTRIBUTES;

	// If there is no directory, create it!
	if(bUpdateOrCreateDirectory && dwFileAttributes == INVALID_FILE_ATTRIBUTES) {
		SetLastError(ERROR_SUCCESS);
		BOOL bResult = CreateDirectory(lpszFile, NULL);
		
		if(!bResult) {
			DWORD dwErr = GetLastError();
			if(dwErr != ERROR_ALREADY_EXISTS)
				return dwErr;
		} else {
			// We're done, because we obviously created a directory!
			return ERROR_SUCCESS;
		}
	}

	SetLastError(ERROR_SUCCESS);

	HANDLE hFile = CreateFile(
		lpszFile, 
		GENERIC_WRITE, 
		FILE_SHARE_READ | FILE_SHARE_WRITE, 
		NULL, 
		((wOpts & OPT_NO_CREATE) || bUpdateOrCreateDirectory) ? OPEN_EXISTING : OPEN_ALWAYS, 
		FILE_ATTRIBUTE_NORMAL | (bUpdateOrCreateDirectory ? FILE_FLAG_BACKUP_SEMANTICS : 0), 
		0);

	DWORD dwRetVal = GetLastError();

	// Check for CreateFile() special cases
	if(hFile == INVALID_HANDLE_VALUE) {
		if((wOpts & OPT_NO_CREATE) && dwRetVal == ERROR_FILE_NOT_FOUND)
			return ERROR_SUCCESS; // not an error
		else if(dwRetVal == ERROR_ALREADY_EXISTS)
			dwRetVal = ERROR_SUCCESS; // not an error according to MSDN docs

		return dwRetVal;
	}

	// Is there any template timestamp?  
	if(atime || mtime || ctime) {
		BOOL bResult = SetFileTime(
			hFile, 
			(wOpts & OPT_MODIFY_CTIME) ? ctime : NULL,
			(wOpts & OPT_MODIFY_ATIME) ? atime : NULL,
			(wOpts & OPT_MODIFY_MTIME) ? mtime : NULL
		);

		if(bResult)
			dwRetVal = ERROR_SUCCESS;
		else
			dwRetVal = GetLastError();
	}

	CloseHandle(hFile);

	return dwRetVal;
}

// Prints the usage/help and an optional error message
static void ShowUsage(LPCTSTR lpszErrorMessage = NULL) 
{
	if(lpszErrorMessage)
		_tprintf(_T("Error: %s\n"), lpszErrorMessage);
	_tprintf(_T("Usage: touch [-h] [-a] [-C] [-m] [-c] [-r file] [-d] [-t [[CC]YY]MMDDhhmm[.SS]] <file> [file 2] [file 3] ... [file N]\n"));
	_tprintf(_T("   -a: Modify access time\n"));
	_tprintf(_T("   -C: Modify creation time\n"));
	_tprintf(_T("   -m: Modify modification time\n"));
	_tprintf(_T("   -c: Do not create file if not found\n"));
	_tprintf(_T("   -r: Use file or directory as time source\n"));
	_tprintf(_T("   -d: If target doesn't exist, create a directory\n"));
	_tprintf(_T("   -t: Use specified time instead of current time\n"));
	_tprintf(_T("   -h: This help text\n"));
	_tprintf(_T("\n"));
	_tprintf(_T("Options -r and -t are mutually exclusive.\n"));
	_tprintf(_T("By default, all times will be modified (access, creation and modification).\n"));
}

// Prints a message translated from a Windows Error code, then prints the usage 
static void PrintError(LPCTSTR lpszInfo, DWORD err)
{
	_tprintf(_T("Error: %s: "), lpszInfo);
	LPVOID lpMsgBuf;
    FormatMessage(
		FORMAT_MESSAGE_ALLOCATE_BUFFER |
        FORMAT_MESSAGE_FROM_SYSTEM |
        FORMAT_MESSAGE_IGNORE_INSERTS,
        NULL,
        err,
        MAKELANGID(LANG_NEUTRAL, SUBLANG_DEFAULT), // Default language
        (LPTSTR) &lpMsgBuf,
        0,
        NULL);
    
	_tprintf(_T("%s\n"), (LPCTSTR)lpMsgBuf);
    LocalFree( lpMsgBuf );
}

static bool IsGlobPattern(LPCTSTR lpszGlob)
{
	return _tcschr(lpszGlob, _T('*')) != NULL || _tcschr(lpszGlob, _T('?'));
}

// Parses the command line arguments. This function expects
// that "argv[0]" is not included. (argv[0] = name of called executable)
static bool ParseOptions(int argc,  LPTSTR argv[], 
						 int&       iFileArg, 
						 LPTSTR&    lpszTemplateFile,
						 LPTSTR&    lpszTime,
						 WORD&      wOpts)
{
	lpszTime = lpszTemplateFile = NULL;
	wOpts = 0;

	if(!argc) {
		ShowUsage(_T("No file(s) specified"));
		return false;
	} else if(argc == 1) {
		if(!_tcscmp(argv[0], _T("-h"))) {
			ShowUsage(_T("Help Requested"));
			return false;
		} else if(argv[0][0] == _T('-')) {
			ShowUsage(_T("Not enough arguments"));
			return false;
		}
		iFileArg = 0;
	} else {
		int i;
		for(i = 0; i < argc; ++i) {
			LPTSTR curr_arg = argv[i];
			LPTSTR next_arg = (i + 1 < (argc - 1) ? argv[i + 1] : NULL);

			if(curr_arg[0] == '-') {
				if(!_tcscmp(curr_arg, _T("-a"))) {
					wOpts |= OPT_MODIFY_ATIME;
				} else if(!_tcscmp(curr_arg, _T("-C"))) {
					wOpts |= OPT_MODIFY_CTIME;
				} else if(!_tcscmp(curr_arg, _T("-m"))) {
					wOpts |= OPT_MODIFY_MTIME;
				} else if(!_tcscmp(curr_arg, _T("-c"))) {
					wOpts |= OPT_NO_CREATE;
				} else if(!_tcscmp(curr_arg, _T("-d"))) {
					wOpts |= OPT_DIR_IF_NOT_EXIST;
				} else if(!_tcscmp(curr_arg, _T("-r"))) {
					wOpts |= OPT_USE_TEMPLATE;
					lpszTemplateFile = next_arg;
					++i;
				} else if(!_tcscmp(curr_arg, _T("-t"))) {
					wOpts |= OPT_USER_TIME;
					lpszTime = next_arg;
					++i;
				} else if(!_tcscmp(curr_arg, _T("-h"))) {
					ShowUsage(_T("Help Requested"));
					return false;
				} else {
					ShowUsage(_T("Unknown argument"));
					return false;
				}
			} else
				break;
		}

		iFileArg = i;
	}

	if(iFileArg >= argc) {
		ShowUsage(_T("No file(s) specified!"));
		return false;
	}

	if((wOpts & OPT_USE_TEMPLATE) && (wOpts & OPT_USER_TIME)) {
		ShowUsage(_T("You may not specify both -r and -t"));
		return false;
	}
	
	if((wOpts & OPT_USE_TEMPLATE) && !lpszTemplateFile) {
		ShowUsage(_T("You must specify a file name together with -r"));
		return false;
	}

	if((wOpts & OPT_USER_TIME) && !lpszTime) {
		ShowUsage(_T("You must specify a timestamp together with -t"));
		return false;
	}

	// If -a, -C and -m wasn't specified, then all of them are implied
	if(!(wOpts & OPT_MODIFY_ATIME) && 
	   !(wOpts & OPT_MODIFY_MTIME) && 
	   !(wOpts & OPT_MODIFY_CTIME))
	   wOpts |= (OPT_MODIFY_ATIME | OPT_MODIFY_MTIME | OPT_MODIFY_CTIME);

	return true;
}

// Gets file times from a file
static bool GetFileTimes(LPCTSTR    lpszTemplateFileName,
						 FILETIME*  tsAtime,
						 FILETIME*  tsMtime,
						 FILETIME*  tsCtime) 
{
	HANDLE hFile = CreateFile(
		lpszTemplateFileName, 
		GENERIC_READ, 
		FILE_SHARE_READ | FILE_SHARE_WRITE, 
		NULL, 
		OPEN_EXISTING, 
		FILE_ATTRIBUTE_NORMAL | FILE_FLAG_BACKUP_SEMANTICS, 
		0);

	if(INVALID_HANDLE_VALUE == hFile) {
		PrintError(lpszTemplateFileName, GetLastError());
		ShowUsage();
		return false;
	}

	if(!GetFileTime(hFile, tsCtime, tsAtime, tsMtime)) {
		PrintError(lpszTemplateFileName, GetLastError());
		ShowUsage();
		CloseHandle(hFile);
		return false;
	}

	CloseHandle(hFile);

	return true;
}

// Parses a date/time string into a FILETIME structure
// Parses the syntax: [[CC]YY]MMDDhhmm[.SS]
// Warning: This is no masterpiece. No regular expressions or any other
// fancy features used. 
static bool ParseTime(LPCTSTR lpszTime, FILETIME* ts) 
{
	size_t nLen = _tcslen(lpszTime);
    
	// Verify length
	if(nLen < 8 || nLen > 15) {
		ShowUsage(_T("Invalid time format"));
		return false;
	}

	// Verify format
	bool   bSeenDot = false;
	size_t nDotIndex = size_t(-1);
	for(size_t i = 0; i < nLen; ++i) {
		if(lpszTime[i] == _T('.')) {
			if(bSeenDot) {
				ShowUsage(_T("Invalid time format"));
				return false;
			} else {
				bSeenDot = true;
				nDotIndex = i;
			}
		} else if(!_istdigit(lpszTime[i])) {
			ShowUsage(_T("Invalid time format"));
			return false;
		}
	}
	
	size_t nBeforeDotLength;
	size_t nAfterDotLength;

	if(bSeenDot) {
		nBeforeDotLength = nDotIndex;
		nAfterDotLength = nLen - (nDotIndex + 1);
	} else {
		nBeforeDotLength = nLen;
		nAfterDotLength = 0;
	}

	if(bSeenDot && nAfterDotLength != 2 ||
	   nBeforeDotLength != 8 && nBeforeDotLength != 10 && nBeforeDotLength != 12) {
		ShowUsage(_T("Invalid time format"));
		return false;
	}

	WORD nCentury;
	WORD nYear;
	WORD nMonth;
	WORD nDay;
	WORD nHour;
	WORD nMinute;
	WORD nSeconds;
	int nIndex = 0;

	// First get defaults for century and year (= this year) in case uses did not specify them
	SYSTEMTIME    st;
	GetSystemTime(&st);
	nCentury = st.wYear / 100;
	nYear    = st.wYear % 100;

	switch(nBeforeDotLength) { // Cases fall through... (no typo!)
		case 12:
			nCentury  = (lpszTime[nIndex++] - _T('0')) * 10;
			nCentury += (lpszTime[nIndex++] - _T('0'));
		case 10:
			nYear     = (lpszTime[nIndex++] - _T('0')) * 10;
			nYear    += (lpszTime[nIndex++] - _T('0'));
		case 8:
			nMonth    = (lpszTime[nIndex++] - _T('0')) * 10;
			nMonth   += (lpszTime[nIndex++] - _T('0'));
			nDay      = (lpszTime[nIndex++] - _T('0')) * 10;
			nDay     += (lpszTime[nIndex++] - _T('0'));
			nHour     = (lpszTime[nIndex++] - _T('0')) * 10;
			nHour    += (lpszTime[nIndex++] - _T('0'));
			nMinute   = (lpszTime[nIndex++] - _T('0')) * 10;
			nMinute  += (lpszTime[nIndex++] - _T('0'));
			break;
		default: // Should not end up here!
			ShowUsage(_T("Invalid time format"));
			return false;
	}

	if(2 == nAfterDotLength) {
		nSeconds  = (lpszTime[nDotIndex + 1] - _T('0')) * 10;
		nSeconds += (lpszTime[nDotIndex + 2] - _T('0'));
	} else {
		nSeconds = 0;
	}

	st.wYear = nCentury * 100 + nYear;
	st.wMonth = nMonth;
	st.wDay = nDay;
	st.wHour = nHour;
	st.wMinute = nMinute;
	st.wSecond = nSeconds;
	st.wMilliseconds = 0;

	
	FILETIME ft;
	if(!SystemTimeToFileTime(&st, &ft)) {
		PrintError(_T("Specified date"), GetLastError());
		ShowUsage();
		return false;
	}

	if(!LocalFileTimeToFileTime(&ft, ts)) {
		PrintError(_T("Specified date"), GetLastError());
		ShowUsage();
		return false;
	}

	return true;
}

int _tmain(int argc, LPTSTR argv[])
{
	LPTSTR    lpszTemplateFile;
	LPTSTR    lpszTime;
	int       iFileArg;
	WORD      wOpts;
	FILETIME  tsUserTime;
	FILETIME  tsATime;
	FILETIME  tsMTime;
	FILETIME  tsCTime;

	if(!ParseOptions(argc - 1, argv + 1, iFileArg, lpszTemplateFile, lpszTime, wOpts))
		return -1;

	++iFileArg; // Adjust for executable
	
	if(wOpts & OPT_USER_TIME) {
		if(!ParseTime(lpszTime, &tsUserTime)) 
			return -1;

		tsATime = tsMTime = tsCTime = tsUserTime;
	} else if(wOpts & OPT_USE_TEMPLATE) {
		if(!GetFileTimes(lpszTemplateFile, &tsATime, &tsMTime, &tsCTime))
			return -1;
	} else {
		SYSTEMTIME  now;

		GetSystemTime(&now);
		if(!SystemTimeToFileTime(&now, &tsUserTime)) {
			PrintError(_T("SystemTimeToFileTime()"), GetLastError());
			ShowUsage();
			return -1;
		}

		tsATime = tsMTime = tsCTime = tsUserTime;
	}

	for(int i = iFileArg; i < argc; ++i) {
		LPCTSTR lpszFile = argv[i];

		TCHAR szPath[_MAX_PATH];
		TCHAR szDrive[_MAX_DRIVE];
		TCHAR szDir[_MAX_DIR];
		TCHAR szFname[_MAX_FNAME];
		TCHAR szExt[_MAX_EXT];
#ifdef _UNICODE
		struct _wfinddata_t sFile;
#else
		struct _finddata_t sFile;
#endif

		if(IsGlobPattern(lpszFile)) {
			_tfullpath(szPath, lpszFile, _MAX_PATH);
			_tsplitpath(szPath, szDrive, szDir, szFname, szExt);
			intptr_t hFile = _tfindfirst(szPath, &sFile);
			if(hFile != -1L) {
				do {
					if(sFile.attrib & _A_SUBDIR) continue;
					TCHAR szIterPath[_MAX_PATH];
					TCHAR szIterFname[_MAX_FNAME];
					TCHAR szIterExt[_MAX_EXT];

					_tsplitpath(sFile.name, NULL, NULL, szIterFname, szIterExt);
					_tmakepath(szIterPath, szDrive, szDir, szIterFname, szIterExt);
					DWORD dwResult = touch(
						szIterPath,
						(wOpts & OPT_MODIFY_ATIME) ? &tsATime : NULL,
						(wOpts & OPT_MODIFY_MTIME) ? &tsMTime : NULL,
						(wOpts & OPT_MODIFY_CTIME) ? &tsCTime : NULL,
						wOpts
					);

					if(ERROR_SUCCESS != dwResult) {
						PrintError(szIterPath, dwResult);
					}
				} while(_tfindnext(hFile, &sFile) == 0);
				_findclose(hFile);
			}
		} else {
			DWORD dwResult = touch(
				lpszFile,
				(wOpts & OPT_MODIFY_ATIME) ? &tsATime : NULL,
				(wOpts & OPT_MODIFY_MTIME) ? &tsMTime : NULL,
				(wOpts & OPT_MODIFY_CTIME) ? &tsCTime : NULL,
				wOpts
			);

			if(ERROR_SUCCESS != dwResult) {
				PrintError(lpszFile, dwResult);
			}
		}
	}

	return 0;
}

