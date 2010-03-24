/* 
 
Copyright (c) 2003-2005 Futronic Technology Company Ltd. All rights reserved. 
 
Abstract: 
 
Definitions and prototypes for the Futronic Scanner API. 
 
Example Code: 
 
FTRHANDLE hDevice; 
PVOID pBuffer; 
FTRSCAN_IMAGE_SIZE ImageSize; 
 
hDevice = ftrScanOpenDevice(); 
if( hDevice == NULL ) 
{ 
// process error 
// ... 
} else { 
 
ftrScanGetImageSize( hDevice, &amt;ImageSize ); 
pBuffer = new BYTE [ImageSize.nImageSize]; 
 
if( ftrScanGetFrame( hDevice, pBuffer, NULL ) ) 
{ 
// success 
// ... 
} else { 
switch( GetLastError() ) 
{ 
case FTR_ERROR_EMPTY_FRAME: // or ERROR_EMPTY 
// fingerprint is not present 
// ... 
break; 
case FTR_ERROR_MOVABLE_FINGER: 
// Finger is movable. Repeat the scan process. 
// ... 
break; 
case FTR_ERROR_NO_FRAME: 
// "Fake replica" detected 
// ... 
break; 
default: 
// process error 
// ... 
} 
} 
 
delete pBuffer; 
ftrScanCloseDevice( hDevice ); 
} 
 
*/ 
 
#ifndef __FUTRONIC_SCAN_API_H__ 
#define __FUTRONIC_SCAN_API_H__ 
 
#include "windows.h" 
 
#ifdef __cplusplus 
extern "C" { /* assume C declarations for C++ */ 
#endif 
 
typedef void * FTRHANDLE; 
 
#define FTR_MAX_INTERFACE_NUMBER 128 
 
#define FTR_OPTIONS_CHECK_FAKE_REPLICA 0x00000001 
#define FTR_OPTIONS_FAST_FINGER_DETECT_METHOD 0x00000002 
 
#define FTR_ERROR_BASE 0x20000000 
#define FTR_ERROR_CODE( x ) (FTR_ERROR_BASE | (x)) 
 
#define FTR_ERROR_EMPTY_FRAME ERROR_EMPTY 
#define FTR_ERROR_MOVABLE_FINGER FTR_ERROR_CODE( 0x0001 ) 
#define FTR_ERROR_NO_FRAME FTR_ERROR_CODE( 0x0002 ) 
#define FTR_ERROR_USER_CANCELED FTR_ERROR_CODE( 0x0003 ) 
#define FTR_ERROR_HARDWARE_INCOMPATIBLE FTR_ERROR_CODE( 0x0004 ) 
#define FTR_ERROR_FIRMWARE_INCOMPATIBLE FTR_ERROR_CODE( 0x0005 ) 
#define FTR_ERROR_INVALID_AUTHORIZATION_CODE FTR_ERROR_CODE( 0x0006 ) 
 
#define FTR_CONST_COLOR_SMALL_IMAGE_WIDTH 256 
#define FTR_CONST_COLOR_SMALL_IMAGE_HEIGHT 150 
#define FTR_CONST_COLOR_SMALL_IMAGE_SIZE (FTR_CONST_COLOR_SMALL_IMAGE_WIDTH * FTR_CONST_COLOR_SMALL_IMAGE_HEIGHT) 
 
#define FTR_CONST_CALIBRATION_SKIP_IR 0x00000001 
#define FTR_CONST_CALIBRATION_SKIP_FUZZY 0x00000002 
 
#define FTR_CONST_DIODE_OFF ((BYTE)0) 
#define FTR_CONST_DIODE_ON ((BYTE)255) 
 
#pragma pack(push, 1) 
typedef struct _FTRSCAN_IMAGE_SIZE 
{ 
int nWidth; 
int nHeight; 
int nImageSize; 
} FTRSCAN_IMAGE_SIZE, *PFTRSCAN_IMAGE_SIZE; 
 
typedef struct _FTRSCAN_FAKE_REPLICA_PARAMETERS { 
BOOL bCalculated; 
int nCalculatedSum1; 
int nCalculatedSumFuzzy; 
int nCalculatedSumEmpty; 
int nCalculatedSum2; 
double dblCalculatedTremor; 
double dblCalculatedValue; 
} FTRSCAN_FAKE_REPLICA_PARAMETERS, *PFTRSCAN_FAKE_REPLICA_PARAMETERS; 
 
typedef struct _FTRSCAN_FRAME_PARAMETERS 
{ 
int nContrastOnDose2; 
int nContrastOnDose4; 
int nDose; 
int nBrightnessOnDose1; 
int nBrightnessOnDose2; 
int nBrightnessOnDose3; 
int nBrightnessOnDose4; 
FTRSCAN_FAKE_REPLICA_PARAMETERS FakeReplicaParams; 
BYTE Reserved[64-sizeof(FTRSCAN_FAKE_REPLICA_PARAMETERS)]; 
} FTRSCAN_FRAME_PARAMETERS, *PFTRSCAN_FRAME_PARAMETERS; 
 
typedef enum __FTRSCAN_INTERFACE_STATUS { 
FTRSCAN_INTERFACE_STATUS_CONNECTED, 
FTRSCAN_INTERFACE_STATUS_DISCONNECTED 
} FTRSCAN_INTERFACE_STATUS, *PFTRSCAN_INTERFACE_STATUS; 
 
typedef struct __FTRSCAN_INTERFACES_LIST { 
FTRSCAN_INTERFACE_STATUS InterfaceStatus[FTR_MAX_INTERFACE_NUMBER]; 
} FTRSCAN_INTERFACES_LIST, *PFTRSCAN_INTERFACES_LIST; 
#pragma pack(pop) 
 
FTRHANDLE WINAPI ftrScanOpenDevice(); 
FTRHANDLE WINAPI ftrScanOpenDeviceOnInterface( int nInterface ); 
void WINAPI ftrScanCloseDevice( FTRHANDLE ftrHandle ); 
BOOL WINAPI ftrScanSetOptions( FTRHANDLE ftrHandle, DWORD dwMask, DWORD dwFlags ); 
BOOL WINAPI ftrScanGetOptions( FTRHANDLE ftrHandle, LPDWORD lpdwFlags ); 
 
BOOL WINAPI ftrScanGetInterfaces( PFTRSCAN_INTERFACES_LIST pInterfaceList ); 
BOOL WINAPI ftrSetBaseInterface( int nBaseInterface ); 
int WINAPI ftrGetBaseInterfaceNumber(); 
 
BOOL WINAPI ftrScanGetFakeReplicaInterval( double *pdblMinFakeReplicaValue, double *pdblMaxFakeReplicaValue ); 
void WINAPI ftrScanSetFakeReplicaInterval( double dblMinFakeReplicaValue, double dblMaxFakeReplicaValue ); 
 
BOOL WINAPI ftrScanGetImageSize( FTRHANDLE ftrHandle, PFTRSCAN_IMAGE_SIZE pImageSize ); 
BOOL WINAPI ftrScanGetImage( FTRHANDLE ftrHandle, int nDose, PVOID pBuffer ); 
BOOL WINAPI ftrScanGetFuzzyImage( FTRHANDLE ftrHandle, PVOID pBuffer ); 
BOOL WINAPI ftrScanGetBacklightImage( FTRHANDLE ftrHandle, PVOID pBuffer ); 
BOOL WINAPI ftrScanGetDarkImage( FTRHANDLE ftrHandle, PVOID pBuffer ); 
BOOL WINAPI ftrScanGetColourImage( FTRHANDLE ftrHandle, PVOID pDoubleSizeBuffer ); 
BOOL WINAPI ftrScanGetSmallColourImage( FTRHANDLE ftrHandle, PVOID pSmallBuffer ); 
BOOL WINAPI ftrScanGetColorDarkImage( FTRHANDLE ftrHandle, PVOID pDoubleSizeBuffer ); 
 
BOOL WINAPI ftrScanIsFingerPresent( FTRHANDLE ftrHandle, PFTRSCAN_FRAME_PARAMETERS pFrameParameters ); 
//BOOL WINAPI ftrScanGetFrame( FTRHANDLE ftrHandle, PVOID pBuffer, PFTRSCAN_FRAME_PARAMETERS pFrameParameters ); 
 
BOOL WINAPI ftrScanSave7Bytes( FTRHANDLE ftrHandle, PVOID pBuffer ); 
BOOL WINAPI ftrScanRestore7Bytes( FTRHANDLE ftrHandle, PVOID pBuffer ); 
 
typedef BOOL (*PFTRCALIBRATEFNCB)( LPVOID pContext, PVOID pParams ); 
BOOL WINAPI ftrScanZeroCalibration( PFTRCALIBRATEFNCB pfnCallbackProc, LPVOID pContext ); 
BOOL WINAPI ftrScanZeroCalibration2( DWORD dwOptions, PFTRCALIBRATEFNCB pfnCallbackProc, LPVOID pContext ); 
BOOL WINAPI ftrScanGetCalibrationConstants( FTRHANDLE ftrHandle, PBYTE pbyIRConst, PBYTE pbyFuzzyConst ); 
BOOL WINAPI ftrScanStoreCalibrationConstants( FTRHANDLE ftrHandle, BYTE byIRConst, BYTE byFuzzyConst, BOOL bBurnInFlash ); 
BOOL WINAPI ftrScanGetFakeReplicaParameters( FTRHANDLE ftrHandle, PFTRSCAN_FAKE_REPLICA_PARAMETERS pFakeReplicaParams ); 
 
BOOL WINAPI ftrScanSetNewAuthorizationCode( FTRHANDLE ftrHandle, PVOID pSevenBytesAuthorizationCode ); 
//BOOL WINAPI ftrScanSaveSecret7Bytes( FTRHANDLE ftrHandle, PVOID pSevenBytesAuthorizationCode, PVOID pBuffer ); 
BOOL WINAPI ftrScanRestoreSecret7Bytes( FTRHANDLE ftrHandle, PVOID pSevenBytesAuthorizationCode, PVOID pBuffer ); 
 
//BOOL WINAPI ftrScanSetDiodesStatus( FTRHANDLE ftrHandle, BYTE byGreenDiodeStatus, BYTE byRedDiodeStatus ); 
BOOL WINAPI ftrScanGetDiodesStatus( FTRHANDLE ftrHandle, PBOOL pbIsGreenDiodeOn, PBOOL pbIsRedDiodeOn ); 
 
#ifdef __cplusplus 
} 
#endif 
 
#endif // __FUTRONIC_SCAN_API_H__ 
