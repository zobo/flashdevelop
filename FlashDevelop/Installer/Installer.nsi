;--------------------------------

!include "MUI.nsh"
!include "Sections.nsh"
!include "FileAssoc.nsh"
!include "LogicLib.nsh"
!include "WordFunc.nsh"
!addplugindir "Plugins"

;--------------------------------

; Define version info
!define VERSION "4.5.3"

; Define AIR SDK vars
!define AIR "3.9.0.1030"
!define AIR_URL "http://airdownload.adobe.com/air/win/download/3.9/AdobeAIRSDK.zip"

; Define AIR+ASC SDK vars
!define ASC "3.9.0.1030"
!define ASC_URL "http://airdownload.adobe.com/air/win/download/3.9/AIRSDK_Compiler.zip"

; Define Flex SDK vars
!define FLEX "4.6.0.23201B"
!define FLEX_URL "http://fpdownload.adobe.com/pub/flex/sdk/builds/flex4.6/flex_sdk_${FLEX}.zip"

; Define Flash player vars
!define FLASH "11.9.900.117"
!define FLASH_URL "http://download.macromedia.com/pub/flashplayer/updaters/11/flashplayer_11_sa_debug.exe"
!define FLASH_DIR "11.9"

; Define Closure compiler vars
!define CLOSURE_URL http://closure-compiler.googlecode.com/files/compiler-latest.zip

;--------------------------------

; Installer details
VIAddVersionKey "CompanyName" "FlashDevelop.org"
VIAddVersionKey "ProductName" "FlashDevelop Installer"
VIAddVersionKey "LegalCopyright" "FlashDevelop.org 2005-2013"
VIAddVersionKey "FileDescription" "FlashDevelop Installer"
VIAddVersionKey "ProductVersion" "${VERSION}.0"
VIAddVersionKey "FileVersion" "${VERSION}.0"
VIProductVersion "${VERSION}.0"

; The name of the installer
Name "FlashDevelop ${VERSION}"

; The captions of the installer
Caption "FlashDevelop ${VERSION} Setup"
UninstallCaption "FlashDevelop ${VERSION} Uninstall"

; The file to write
OutFile "Binary\FlashDevelop.exe"

; Default installation folder
InstallDir "$PROGRAMFILES\FlashDevelop\"

; Define executable files
!define EXECUTABLE "$INSTDIR\FlashDevelop.exe"
!define WIN32RES "$INSTDIR\Tools\winres\winres.exe"
!define ASDOCGEN "$INSTDIR\Tools\asdocgen\ASDocGen.exe"

; Get installation folder from registry if available
InstallDirRegKey HKLM "Software\FlashDevelop" ""

; Define the AIR+ASC SDK extract path
!define ASCPATH "$INSTDIR\Tools\ascsdk"

; Define the Flex SDK extract path
!define SDKPATH "$INSTDIR\Tools\flexsdk"

; Define the JS Compiler extract path
!define GPATH "$INSTDIR\Tools\google"

; Vista redirects $SMPROGRAMS to all users without this
RequestExecutionLevel admin

; Use replace and version compare
!insertmacro WordReplace
!insertmacro VersionCompare

; Required props
SetFont /LANG=${LANG_ENGLISH} "Tahoma" 8
SetCompressor /SOLID lzma
CRCCheck on
XPStyle on

;--------------------------------

; Interface Configuration

!define MUI_HEADERIMAGE
!define MUI_ABORTWARNING
!define MUI_COMPONENTSPAGE_SMALLDESC
!define MUI_HEADERIMAGE_BITMAP "Graphics\Banner.bmp"
!define MUI_WELCOMEFINISHPAGE_BITMAP "Graphics\Wizard.bmp"
!define MUI_UNWELCOMEFINISHPAGE_BITMAP "Graphics\Wizard.bmp"
!define MUI_PAGE_HEADER_SUBTEXT "Please view the licence before installing FlashDevelop ${VERSION}."
!define MUI_WELCOMEPAGE_TEXT "This wizard will guide you through the installation of $(^NameDA).\r\n\r\nIt is recommended that you close all other applications before starting Setup. This will make it possible to update relevant system files without having to reboot your computer. Hover the installer options for more info.\r\n\r\nTo get everything out of FlashDevelop you should have 32-bit Java Runtime (1.6 or later) and debug Flash Player (ActiveX for IE) installed before using FlashDevelop.\r\n\r\n$_CLICK"
!define MUI_FINISHPAGE_SHOWREADME "http://www.flashdevelop.org/wikidocs/index.php?title=Getting_Started"
!define MUI_FINISHPAGE_SHOWREADME_TEXT "See online guide to get started"

;--------------------------------

; Pages

!insertmacro MUI_PAGE_WELCOME
!insertmacro MUI_PAGE_COMPONENTS
!insertmacro MUI_PAGE_DIRECTORY
!insertmacro MUI_PAGE_INSTFILES
!insertmacro MUI_PAGE_FINISH
!insertmacro MUI_UNPAGE_WELCOME
!insertmacro MUI_UNPAGE_CONFIRM
!insertmacro MUI_UNPAGE_COMPONENTS
!insertmacro MUI_UNPAGE_INSTFILES
!insertmacro MUI_UNPAGE_FINISH
!insertmacro MUI_LANGUAGE "English"

;--------------------------------

; InstallTypes

InstType "Default"
InstType "Standalone/Portable"
InstType "un.Default"
InstType "un.Full"

;--------------------------------

; Functions

Function RefreshConfig
	
	SetOverwrite on
	IfFileExists "$INSTDIR\.local" Local 0
	IfFileExists "$LOCALAPPDATA\FlashDevelop\*.*" User Done
	Local:
	SetOutPath "$INSTDIR"
	File "/oname=.reconfig" "..\Bin\Debug\.local"
	User:
	SetOutPath "$LOCALAPPDATA\FlashDevelop"
	File "/oname=.reconfig" "..\Bin\Debug\.local"
	Done:
	
FunctionEnd

Function GetDotNETVersion
	
	Push $0
	Push $1
	System::Call "mscoree::GetCORVersion(w .r0, i ${NSIS_MAX_STRLEN}, *i) i .r1"
	StrCmp $1 "error" 0 +2
	StrCpy $0 "not_found"
	Pop $1
	Exch $0
	
FunctionEnd


Function GetFlashVersion
	
	Push $0
	ClearErrors
	ReadRegStr $0 HKLM "Software\Macromedia\FlashPlayer" "CurrentVersion"
	IfErrors 0 +5
	ClearErrors
	ReadRegStr $0 HKCU "Software\Macromedia\FlashPlayer" "FlashPlayerVersion"
	IfErrors 0 +2
	StrCpy $0 "not_found"
	${WordReplace} $0 "," "." "+" $1
	Exch $1
	
FunctionEnd

Function GetJavaVersion
	
	Push $0
	ClearErrors
	ReadRegStr $0 HKLM "Software\JavaSoft\Java Runtime Environment" "CurrentVersion"
	IfErrors 0 +2
	StrCpy $0 "not_found"
	Exch $0
	
FunctionEnd

Function GetFDVersion
	
	Push $0
	ClearErrors
	ReadRegStr $0 HKLM Software\FlashDevelop "CurrentVersion"
	IfErrors 0 +2
	StrCpy $0 "not_found"
	Exch $0
	
FunctionEnd

Function GetAirSDKVersion
	
	Push $0
	ClearErrors
	IfFileExists "$INSTDIR\.local" +3 0
	ReadRegStr $0 HKLM Software\FlashDevelop "AirSDKVersion"
	IfErrors 0 +2
	StrCpy $0 "not_found"
	Exch $0
	
FunctionEnd

Function GetAscSDKVersion
	
	Push $0
	ClearErrors
	IfFileExists "$INSTDIR\.local" +3 0
	ReadRegStr $0 HKLM Software\FlashDevelop "AscSDKVersion"
	IfErrors 0 +2
	StrCpy $0 "not_found"
	Exch $0
	
FunctionEnd

Function GetFlexSDKVersion
	
	Push $0
	ClearErrors
	IfFileExists "$INSTDIR\.local" +3 0
	ReadRegStr $0 HKLM Software\FlashDevelop "FlexSDKVersion"
	IfErrors 0 +2
	StrCpy $0 "not_found"
	Exch $0
	
FunctionEnd

Function GetFlashDebugVersion
	
	Push $0
	ClearErrors
	IfFileExists "$INSTDIR\.local" +3 0
	ReadRegStr $0 HKLM Software\FlashDevelop "FlashDebugVersion"
	IfErrors 0 +2
	StrCpy $0 "not_found"
	Exch $0
	
FunctionEnd

Function GetFDInstDir
	
	Push $0
	ClearErrors
	ReadRegStr $0 HKLM Software\FlashDevelop ""
	IfErrors 0 +2
	StrCpy $0 "not_found"
	Exch $0
	
FunctionEnd

Function GetNeedsReset
	
	Call GetFDVersion
	Pop $1
	Push $2
	${VersionCompare} $1 "4.0.0" $3
	${If} $1 == "not_found"
	StrCpy $2 "do_reset"
	${ElseIf} $3 == 2
	StrCpy $2 "do_reset"
	${ElseIf} $1 == "4.0.0-Beta"
	StrCpy $2 "do_reset"
	${ElseIf} $1 == "4.0.0-Beta2"
	StrCpy $2 "do_reset"
	${ElseIf} $1 == "4.0.0-Beta3"
	StrCpy $2 "do_reset"
	${Else}
	StrCpy $2 "is_ok"
	${EndIf}
	Exch $2
	
FunctionEnd

Function ConnectInternet

	Push $R0
	ClearErrors
	Dialer::AttemptConnect
	IfErrors NoIE3
	Pop $R0
	StrCmp $R0 "online" Connected
	MessageBox MB_OK|MB_ICONSTOP "Cannot connect to the internet."
	NoIE3:
	MessageBox MB_OK|MB_ICONINFORMATION "Please connect to the internet now."
	Connected:
	Pop $R0
	
FunctionEnd

;--------------------------------

; Install Sections

Section "FlashDevelop" Main
	
	SectionIn 1 2 RO
	SetOverwrite on
	
	SetOutPath "$INSTDIR"
	
	; Clean library
	RMDir /r "$INSTDIR\Library"

	; Clean old Flex PMD
	IfFileExists "$INSTDIR\Tools\flexpmd\flex-pmd-command-line-1.1.jar" 0 +2
	RMDir /r "$INSTDIR\Tools\flexpmd"
	
	; Copy all files
	File /r /x .svn /x .empty /x *.db /x Exceptions.log /x .local /x .multi /x *.pdb /x *.vshost.exe /x *.vshost.exe.config /x *.vshost.exe.manifest /x "..\Bin\Debug\Data\" /x "..\Bin\Debug\Settings\" /x "..\Bin\Debug\Snippets\" /x "..\Bin\Debug\Templates\" "..\Bin\Debug\*.*"
	
	SetOverwrite off
	
	IfFileExists "$INSTDIR\.local" +6 0
	RMDir /r "$INSTDIR\Data"
	RMDir /r "$INSTDIR\Settings"
	RMDir /r "$INSTDIR\Snippets"
	RMDir /r "$INSTDIR\Templates"
	RMDir /r "$INSTDIR\Projects"
	
	SetOutPath "$INSTDIR\Settings"
	File /r /x .svn /x .empty /x *.db /x LayoutData.fdl /x SessionData.fdb /x SettingData.fdb "..\Bin\Debug\Settings\*.*"
	
	SetOutPath "$INSTDIR\Snippets"
	File /r /x .svn /x .empty /x *.db "..\Bin\Debug\Snippets\*.*"
	
	SetOutPath "$INSTDIR\Templates"
	File /r /x .svn /x .empty /x *.db "..\Bin\Debug\Templates\*.*"

	SetOutPath "$INSTDIR\Projects"
	File /r /x .svn /x .empty /x *.db "..\Bin\Debug\Projects\*.*"

	; Remove PluginCore from plugins...
	Delete "$INSTDIR\Plugins\PluginCore.dll"
	
SectionEnd

Section "Install Flex SDK" InstallFlexSDK

	SectionIn 1
	SetOverwrite on
	SetShellVarContext all
	
	Call GetFlexSDKVersion
	Pop $0
	
	${If} $0 != ${FLEX}
	
	; Connect to internet
	Call ConnectInternet

	; If the Flex SDK exists in the installer directory then copy that to $TEMP for bulk silent deployments.
	IfFileExists "$EXEDIR\flex_sdk_${FLEX}.zip" 0 +2
	CopyFiles "$EXEDIR\flex_sdk_${FLEX}.zip" $TEMP
	
	; Download Flex SDK zip file. If the extract failed previously, use the old file.
	IfFileExists "$TEMP\flex_sdk_${FLEX}.zip" +7 0
	NSISdl::download /TIMEOUT=30000 ${FLEX_URL} "$TEMP\flex_sdk_${FLEX}.zip"
	Pop $R0
	StrCmp $R0 "success" +4
	DetailPrint "Flex SDK download cancel details: $R0"
	MessageBox MB_OK "Download cancelled. The installer will now continue normally."
	Goto Finish
	
	; Delete SDK dir on update
	RMDir /r "${SDKPATH}"
	
	; Force AIR SDK update
	DeleteRegValue HKLM "Software\FlashDevelop" "AirSDKVersion"
	
	; Create SDK dir if not found
	IfFileExists "${SDKPATH}\*.*" +2 0
	CreateDirectory "${SDKPATH}"
	
	; Extract the Flex SDK zip
	DetailPrint "Extracting Flex SDK..."
	nsisunz::Unzip "$TEMP\flex_sdk_${FLEX}.zip" "${SDKPATH}"
	Pop $R0
	StrCmp $R0 "success" +3
	MessageBox MB_OK "Archive extraction failed. The installer will now continue normally."
	Goto Finish
	
	SetOverwrite off
	
	; Add the missing SWC files
	SetOutPath "${SDKPATH}\frameworks\libs\player"
	File /r /x .svn /x *.db "..\Bin\Debug\Tools\flexlibs\frameworks\libs\player\*.*"
	
	; Write the notice file
	ClearErrors
	FileOpen $1 ${SDKPATH}\frameworks\libs\player\FlashDevelopNotice.txt w
	IfErrors Done
	FileWrite $1 "FlashDevelop added the missing 'playerglobal.swc' files here."
	FileClose $1
	Done:
	
	; Delete temporary Flex SDK zip file
	Delete "$TEMP\flex_sdk_${FLEX}.zip"

	; Notify FD about the update
	Call RefreshConfig
	
	Finish:
	
	${EndIf}

SectionEnd

Section "Install AIR SDK" InstallAirSDK

	SectionIn 1
	SetOverwrite on
	SetShellVarContext all
	
	Call GetAirSDKVersion
	Pop $0
	
	${If} $0 != ${AIR}

	; Connect to internet
	Call ConnectInternet

	; If the AIR SDK exists in the installer directory then copy that to $TEMP for bulk silent deployments.
	IfFileExists "$EXEDIR\air_sdk_${AIR}.zip" 0 +2
	CopyFiles "$EXEDIR\air_sdk_${AIR}.zip" $TEMP
	
	; Download AIR SDK zip file. If the extract failed previously, use the old file.
	IfFileExists "$TEMP\air_sdk_${AIR}.zip" +7 0
	NSISdl::download /TIMEOUT=30000 ${AIR_URL} "$TEMP\air_sdk_${AIR}.zip"
	Pop $R0
	StrCmp $R0 "success" +4
	DetailPrint "AIR SDK download cancel details: $R0"
	MessageBox MB_OK "Download cancelled. The installer will now continue normally."
	Goto Finish
	
	; Extract the AIR SDK zip
	IfFileExists "${SDKPATH}\*.*" +2 0
	CreateDirectory "${SDKPATH}"
	DetailPrint "Extracting AIR SDK..."
	nsisunz::Unzip "$TEMP\air_sdk_${AIR}.zip" "${SDKPATH}"
	Pop $R0
	StrCmp $R0 "success" +3
	MessageBox MB_OK "Archive extraction failed. The installer will now continue normally."
	Goto Finish
	
	; Delete temporary AIR SDK zip file
	Delete "$TEMP\air_sdk_${AIR}.zip"

	; Notify FD about the update
	Call RefreshConfig
	
	Finish:
	
	${EndIf}

SectionEnd

Section "Install AIR SDK (ASC2)" InstallAscSDK

	SectionIn 1
	SetOverwrite on
	SetShellVarContext all
	
	Call GetAscSDKVersion
	Pop $0
	
	${If} $0 != ${ASC}

	; Connect to internet
	Call ConnectInternet

	; If the AIR+ASC SDK exists in the installer directory then copy that to $TEMP for bulk silent deployments.
	IfFileExists "$EXEDIR\asc_sdk_${ASC}.zip" 0 +2
	CopyFiles "$EXEDIR\asc_sdk_${ASC}.zip" $TEMP
	
	; Download AIR+ASC SDK zip file. If the extract failed previously, use the old file.
	IfFileExists "$TEMP\asc_sdk_${ASC}.zip" +7 0
	NSISdl::download /TIMEOUT=30000 ${ASC_URL} "$TEMP\asc_sdk_${ASC}.zip"
	Pop $R0
	StrCmp $R0 "success" +4
	DetailPrint "AIR SDK (ASC2) download cancel details: $R0"
	MessageBox MB_OK "Download cancelled. The installer will now continue normally."
	Goto Finish
	
	; Delete AIR+ASC dir on update
	RMDir /r "${ASCPATH}"
	
	; Create SDK dir if not found
	IfFileExists "${ASCPATH}\*.*" +2 0
	CreateDirectory "${ASCPATH}"
	
	; Extract the AIR+ASC SDK zip
	IfFileExists "${ASCPATH}\*.*" +2 0
	CreateDirectory "${ASCPATH}"
	DetailPrint "Extracting AIR SDK (ASC2)..."
	nsisunz::Unzip "$TEMP\asc_sdk_${ASC}.zip" "${ASCPATH}"
	Pop $R0
	StrCmp $R0 "success" +3
	MessageBox MB_OK "Archive extraction failed. The installer will now continue normally."
	Goto Finish
	
	SetOverwrite off
	
	; Add the missing SWC files
	SetOutPath "${ASCPATH}\frameworks\libs\player"
	File /r /x .svn /x *.db "..\Bin\Debug\Tools\flexlibs\frameworks\libs\player\*.*"
	
	; Write the notice file
	ClearErrors
	FileOpen $1 ${ASCPATH}\frameworks\libs\player\FlashDevelopNotice.txt w
	IfErrors Done
	FileWrite $1 "FlashDevelop added the missing 'playerglobal.swc' files here."
	FileClose $1
	Done:
	
	; Delete temporary AIR+ASC SDK zip file
	Delete "$TEMP\asc_sdk_${ASC}.zip"

	; Notify FD about the update
	Call RefreshConfig
	
	Finish:
	
	${EndIf}

SectionEnd

Section "Install Flash Player" InstallFlashPlayer

	SectionIn 1
	SetOverwrite on
	SetShellVarContext all
	
	Call GetFlashDebugVersion
	Pop $0
	
	${If} $0 != ${FLASH}
	
	; Connect to internet
	Call ConnectInternet
	
	; Create player dir if not found
	IfFileExists "$INSTDIR\Tools\flexlibs\runtimes\player\${FLASH_DIR}\win\*.*" +2 0
	CreateDirectory "$INSTDIR\Tools\flexlibs\runtimes\player\${FLASH_DIR}\win\"
	
	; If the debug player exists in the installer directory then use that for bulk silent deployments.
	IfFileExists "$EXEDIR\flashplayer_11_sa_debug.exe" 0 +3
	CopyFiles "$EXEDIR\flashplayer_11_sa_debug.exe" "$INSTDIR\Tools\flexlibs\runtimes\player\${FLASH_DIR}\win\FlashPlayerDebugger.exe"
	Goto Skip

	; Download Flash debug player
	NSISdl::download /TIMEOUT=30000 ${FLASH_URL} "$INSTDIR\Tools\flexlibs\runtimes\player\${FLASH_DIR}\win\FlashPlayerDebugger.exe"
	Pop $R0
	StrCmp $R0 "success" +4
	DetailPrint "Flash debug player download cancel details: $R0"
	MessageBox MB_OK "Download cancelled. The installer will now continue normally."
	Goto Finish
	
	Skip:
	
	; Notify FD about the update
	Call RefreshConfig
	
	Finish:
	
	${EndIf}

SectionEnd

Section "Install JS Compiler" InstallClosureCompiler

	SetOverwrite on
	SetShellVarContext all

	; Connect to internet
	Call ConnectInternet

	; If the Closure Compiler exists in the installer directory then copy that to $TEMP for bulk silent deployments.
	IfFileExists "$EXEDIR\closure_compiler.zip" 0 +2
	CopyFiles "$EXEDIR\closure_compiler.zip" $TEMP
	
	; Download Closure Compiler zip file. If the extract failed previously, use the old file.
	IfFileExists "$TEMP\closure_compiler.zip" +7 0
	NSISdl::download /TIMEOUT=30000 ${CLOSURE_URL} "$TEMP\closure_compiler.zip"
	Pop $R0
	StrCmp $R0 "success" +4
	DetailPrint "Closure Compiler download cancel details: $R0"
	MessageBox MB_OK "Download cancelled. The installer will now continue normally."
	Goto Finish
	
	; Extract the Closure Compiler zip
	IfFileExists "${GPATH}\*.*" +2 0
	CreateDirectory "${GPATH}"
	DetailPrint "Extracting Closure Compiler..."
	nsisunz::Unzip "$TEMP\closure_compiler.zip" "${GPATH}"
	Pop $R0
	StrCmp $R0 "success" +3
	MessageBox MB_OK "Archive extraction failed. The installer will now continue normally."
	Goto Finish
	
	; Delete temporary Closure Compiler zip file
	Delete "$TEMP\closure_compiler.zip"
	
	Finish:

SectionEnd

SectionGroup "Language" LanguageGroup

Section "No changes" NoChangesLocale
	
	; Don't change the locale
	
SectionEnd

Section "English" EnglishLocale
	
	SetOverwrite on
	IfFileExists "$INSTDIR\.local" Local 0
	IfFileExists "$LOCALAPPDATA\FlashDevelop\*.*" User Done
	Local:
	ClearErrors
	FileOpen $1 "$INSTDIR\.locale" w
	IfErrors Done
	FileWrite $1 "en_US"
	FileClose $1
	User:
	ClearErrors
	FileOpen $1 "$LOCALAPPDATA\FlashDevelop\.locale" w
	IfErrors Done
	FileWrite $1 "en_US"
	FileClose $1
	Done:
	
SectionEnd

Section "Chinese" ChineseLocale
	
	SetOverwrite on
	IfFileExists "$INSTDIR\.local" Local 0
	IfFileExists "$LOCALAPPDATA\FlashDevelop\*.*" User Done
	Local:
	ClearErrors
	FileOpen $1 "$INSTDIR\.locale" w
	IfErrors Done
	FileWrite $1 "zh_CN"
	FileClose $1
	User:
	ClearErrors
	FileOpen $1 "$LOCALAPPDATA\FlashDevelop\.locale" w
	IfErrors Done
	FileWrite $1 "zh_CN"
	FileClose $1
	Done:
	
SectionEnd

Section "Japanese" JapaneseLocale
	
	SetOverwrite on
	IfFileExists "$INSTDIR\.local" Local 0
	IfFileExists "$LOCALAPPDATA\FlashDevelop\*.*" User Done
	Local:
	ClearErrors
	FileOpen $1 "$INSTDIR\.locale" w
	IfErrors Done
	FileWrite $1 "ja_JP"
	FileClose $1
	User:
	ClearErrors
	FileOpen $1 "$LOCALAPPDATA\FlashDevelop\.locale" w
	IfErrors Done
	FileWrite $1 "ja_JP"
	FileClose $1
	Done:
	
SectionEnd

Section "German" GermanLocale
	
	SetOverwrite on
	IfFileExists "$INSTDIR\.local" Local 0
	IfFileExists "$LOCALAPPDATA\FlashDevelop\*.*" User Done
	Local:
	ClearErrors
	FileOpen $1 "$INSTDIR\.locale" w
	IfErrors Done
	FileWrite $1 "de_DE"
	FileClose $1
	User:
	ClearErrors
	FileOpen $1 "$LOCALAPPDATA\FlashDevelop\.locale" w
	IfErrors Done
	FileWrite $1 "de_DE"
	FileClose $1
	Done:
	
SectionEnd

Section "Basque" BasqueLocale
	
	SetOverwrite on
	IfFileExists "$INSTDIR\.local" Local 0
	IfFileExists "$LOCALAPPDATA\FlashDevelop\*.*" User Done
	Local:
	ClearErrors
	FileOpen $1 "$INSTDIR\.locale" w
	IfErrors Done
	FileWrite $1 "eu_ES"
	FileClose $1
	User:
	ClearErrors
	FileOpen $1 "$LOCALAPPDATA\FlashDevelop\.locale" w
	IfErrors Done
	FileWrite $1 "eu_ES"
	FileClose $1
	Done:
	
SectionEnd

SectionGroupEnd

SectionGroup "Shortcuts"

Section "Desktop Shortcut" DesktopShortcut
	
	SetOverwrite on
	SetShellVarContext all
	
	CreateShortCut "$DESKTOP\FlashDevelop.lnk" "${EXECUTABLE}" "" "${EXECUTABLE}" 0
	
SectionEnd

Section "Quick Launch Item" QuickShortcut
	
	SetOverwrite on
	SetShellVarContext all
	
	CreateShortCut "$QUICKLAUNCH\FlashDevelop.lnk" "${EXECUTABLE}" "" "${EXECUTABLE}" 0
	
SectionEnd

SectionGroupEnd

SectionGroup "Advanced"

Section "Start Menu Group" StartMenuGroup
	
	SectionIn 1	
	SetOverwrite on
	SetShellVarContext all
	
	CreateDirectory "$SMPROGRAMS\FlashDevelop"
	CreateShortCut "$SMPROGRAMS\FlashDevelop\FlashDevelop.lnk" "${EXECUTABLE}" "" "${EXECUTABLE}" 0
	WriteINIStr "$SMPROGRAMS\FlashDevelop\Documentation.url" "InternetShortcut" "URL" "http://www.flashdevelop.org/wikidocs/"
	WriteINIStr "$SMPROGRAMS\FlashDevelop\Community.url" "InternetShortcut" "URL" "http://www.flashdevelop.org/community/"
	CreateShortCut "$SMPROGRAMS\FlashDevelop\Uninstall.lnk" "$INSTDIR\Uninstall.exe" "" "$INSTDIR\Uninstall.exe" 0
	
SectionEnd

Section "Registry Modifications" RegistryMods
	
	SectionIn 1
	SetOverwrite on
	SetShellVarContext all
	
	Delete "$INSTDIR\.multi"
	Delete "$INSTDIR\.local"
	
	DeleteRegKey /ifempty HKCR "Applications\FlashDevelop.exe"	
	DeleteRegKey /ifempty HKLM "Software\Classes\Applications\FlashDevelop.exe"
	DeleteRegKey /ifempty HKCU "Software\Classes\Applications\FlashDevelop.exe"
	
	!insertmacro APP_ASSOCIATE "fdp" "FlashDevelop.Project" "FlashDevelop Project" "${WIN32RES},2" "" "${EXECUTABLE}"
	!insertmacro APP_ASSOCIATE "fdproj" "FlashDevelop.GenericProject" "FlashDevelop Generic Project" "${WIN32RES},2" "" "${EXECUTABLE}"
	!insertmacro APP_ASSOCIATE "hxproj" "FlashDevelop.HaXeProject" "FlashDevelop Haxe Project" "${WIN32RES},2" "" "${EXECUTABLE}"
	!insertmacro APP_ASSOCIATE "as2proj" "FlashDevelop.AS2Project" "FlashDevelop AS2 Project" "${WIN32RES},2" "" "${EXECUTABLE}"
	!insertmacro APP_ASSOCIATE "as3proj" "FlashDevelop.AS3Project" "FlashDevelop AS3 Project" "${WIN32RES},2" "" "${EXECUTABLE}"
	!insertmacro APP_ASSOCIATE "docproj" "FlashDevelop.DocProject" "FlashDevelop Docs Project" "${WIN32RES},2" "" "${ASDOCGEN}"
	!insertmacro APP_ASSOCIATE "lsproj" "FlashDevelop.LoomProject" "FlashDevelop Loom Project" "${WIN32RES},2" "" "${EXECUTABLE}"

	!insertmacro APP_ASSOCIATE "fdi" "FlashDevelop.Theme" "FlashDevelop Theme File" "${WIN32RES},1" "" "${EXECUTABLE}"
	!insertmacro APP_ASSOCIATE "fdm" "FlashDevelop.Macros" "FlashDevelop Macros File" "${WIN32RES},1" "" "${EXECUTABLE}"
	!insertmacro APP_ASSOCIATE "fdt" "FlashDevelop.Template" "FlashDevelop Template File" "${WIN32RES},1" "" "${EXECUTABLE}"
	!insertmacro APP_ASSOCIATE "fda" "FlashDevelop.Arguments" "FlashDevelop Arguments File" "${WIN32RES},1" "" "${EXECUTABLE}"
	!insertmacro APP_ASSOCIATE "fds" "FlashDevelop.Snippet" "FlashDevelop Snippet File" "${WIN32RES},1" "" "${EXECUTABLE}"
	!insertmacro APP_ASSOCIATE "fdb" "FlashDevelop.Binary" "FlashDevelop Binary File" "${WIN32RES},1" "" "${EXECUTABLE}"
	!insertmacro APP_ASSOCIATE "fdl" "FlashDevelop.Layout" "FlashDevelop Layout File" "${WIN32RES},1" "" "${EXECUTABLE}"
	!insertmacro APP_ASSOCIATE "fdz" "FlashDevelop.Zip" "FlashDevelop Zip File" "${WIN32RES},1" "" "${EXECUTABLE}"
	
	!insertmacro APP_ASSOCIATE_REMOVEVERB "FlashDevelop.Project" "ShellNew"
	!insertmacro APP_ASSOCIATE_REMOVEVERB "FlashDevelop.GenericProject" "ShellNew"
	!insertmacro APP_ASSOCIATE_REMOVEVERB "FlashDevelop.HaXeProject" "ShellNew"
	!insertmacro APP_ASSOCIATE_REMOVEVERB "FlashDevelop.AS2Project" "ShellNew"
	!insertmacro APP_ASSOCIATE_REMOVEVERB "FlashDevelop.AS3Project" "ShellNew"
	!insertmacro APP_ASSOCIATE_REMOVEVERB "FlashDevelop.DocProject" "ShellNew"
	!insertmacro APP_ASSOCIATE_REMOVEVERB "FlashDevelop.LoomProject" "ShellNew"

	!insertmacro APP_ASSOCIATE_REMOVEVERB "FlashDevelop.Theme" "ShellNew"	
	!insertmacro APP_ASSOCIATE_REMOVEVERB "FlashDevelop.Macros" "ShellNew"
	!insertmacro APP_ASSOCIATE_REMOVEVERB "FlashDevelop.Template" "ShellNew"
	!insertmacro APP_ASSOCIATE_REMOVEVERB "FlashDevelop.Arguments" "ShellNew"
	!insertmacro APP_ASSOCIATE_REMOVEVERB "FlashDevelop.Snippet" "ShellNew"
	!insertmacro APP_ASSOCIATE_REMOVEVERB "FlashDevelop.Binary" "ShellNew"
	!insertmacro APP_ASSOCIATE_REMOVEVERB "FlashDevelop.Layout" "ShellNew"
	!insertmacro APP_ASSOCIATE_REMOVEVERB "FlashDevelop.Zip" "ShellNew"
	
	; Try to repair users registry
	DeleteRegKey /ifempty HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\Product"
	
	; Write uninstall section keys
	WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\FlashDevelop" "InstallLocation" "$INSTDIR"
	WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\FlashDevelop" "Publisher" "FlashDevelop.org"
	WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\FlashDevelop" "DisplayVersion" "${VERSION}"
	WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\FlashDevelop" "DisplayName" "FlashDevelop ${VERSION}"
	WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\FlashDevelop" "Comments" "Thank you for using FlashDevelop."
	WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\FlashDevelop" "HelpLink" "http://www.flashdevelop.org/community/"
	WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\FlashDevelop" "UninstallString" "$INSTDIR\Uninstall.exe"
	WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\FlashDevelop" "DisplayIcon" "${EXECUTABLE}"
	WriteRegDWORD HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\FlashDevelop" "NoModify" 1
	WriteRegDWORD HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\FlashDevelop" "NoRepair" 1
	WriteRegStr HKLM "Software\FlashDevelop" "CurrentVersion" ${VERSION}
	WriteRegStr HKLM "Software\FlashDevelop" "" $INSTDIR
	WriteUninstaller "$INSTDIR\Uninstall.exe"
	
	; Write component version numbers
	${If} ${SectionIsSelected} ${InstallFlexSDK}
	WriteRegStr HKLM "Software\FlashDevelop" "FlexSDKVersion" "${FLEX}"
	${EndIf}
	${If} ${SectionIsSelected} ${InstallAirSDK}
	WriteRegStr HKLM "Software\FlashDevelop" "AirSDKVersion" "${AIR}"
	${EndIf}
	${If} ${SectionIsSelected} ${InstallAscSDK}
	WriteRegStr HKLM "Software\FlashDevelop" "AscSDKVersion" "${ASC}"	
	${EndIf}
	${If} ${SectionIsSelected} ${InstallFlashPlayer}
	WriteRegStr HKLM "Software\FlashDevelop" "FlashDebugVersion" "${FLASH}"
	${EndIf}
	
	!insertmacro UPDATEFILEASSOC
	
SectionEnd

Section "Standalone/Portable" StandaloneMode
	
	SectionIn 2
	SetOverwrite on
	
	SetOutPath "$INSTDIR"
	File ..\Bin\Debug\.local
	
SectionEnd

Section "Multi Instance Mode" MultiInstanceMode
	
	SetOverwrite on
	
	SetOutPath "$INSTDIR"
	File ..\Bin\Debug\.multi
	
SectionEnd

SectionGroupEnd

;--------------------------------

; Install section strings

!insertmacro MUI_FUNCTION_DESCRIPTION_BEGIN
!insertmacro MUI_DESCRIPTION_TEXT ${Main} "Installs the main program and other required files."
!insertmacro MUI_DESCRIPTION_TEXT ${RegistryMods} "Associates integral file types and adds the required uninstall configuration."
!insertmacro MUI_DESCRIPTION_TEXT ${StandaloneMode} "Runs as standalone using only local setting files. NOTE: Not for standard users and manual upgrade only."
!insertmacro MUI_DESCRIPTION_TEXT ${MultiInstanceMode} "Allows multiple instances of FlashDevelop to be executed. NOTE: There are some open issues with this."
!insertmacro MUI_DESCRIPTION_TEXT ${InstallAirSDK} "Downloads and installs, if needed, the Adobe AIR SDK with FlashDevelop."
!insertmacro MUI_DESCRIPTION_TEXT ${InstallAscSDK} "Downloads and installs, if needed, the Adobe AIR SDK (ASC2) with FlashDevelop."
!insertmacro MUI_DESCRIPTION_TEXT ${InstallFlexSDK} "Downloads and installs, if needed, the Adobe Flex SDK with FlashDevelop."
!insertmacro MUI_DESCRIPTION_TEXT ${InstallFlashPlayer} "Downloads and installs, if needed, the standalone Flash debug player with FlashDevelop."
!insertmacro MUI_DESCRIPTION_TEXT ${InstallClosureCompiler} "Downloads and installs the Google Closure Compiler with FlashDevelop."
!insertmacro MUI_DESCRIPTION_TEXT ${NoChangesLocale} "Keeps the current language on update and defaults to English on clean install."
!insertmacro MUI_DESCRIPTION_TEXT ${EnglishLocale} "Changes FlashDevelop's display language to English on next restart."
!insertmacro MUI_DESCRIPTION_TEXT ${ChineseLocale} "Changes FlashDevelop's display language to Chinese on next restart."
!insertmacro MUI_DESCRIPTION_TEXT ${JapaneseLocale} "Changes FlashDevelop's display language to Japanese on next restart."
!insertmacro MUI_DESCRIPTION_TEXT ${GermanLocale} "Changes FlashDevelop's display language to German on next restart."
!insertmacro MUI_DESCRIPTION_TEXT ${BasqueLocale} "Changes FlashDevelop's display language to Basque on next restart."
!insertmacro MUI_DESCRIPTION_TEXT ${StartMenuGroup} "Creates a start menu group and adds default FlashDevelop links to the group."
!insertmacro MUI_DESCRIPTION_TEXT ${QuickShortcut} "Installs a FlashDevelop shortcut to the Quick Launch bar."
!insertmacro MUI_DESCRIPTION_TEXT ${DesktopShortcut} "Installs a FlashDevelop shortcut to the desktop."
!insertmacro MUI_FUNCTION_DESCRIPTION_END

;--------------------------------

; Uninstall Sections

Section "un.FlashDevelop" UninstMain
	
	SectionIn 1 2 RO
	SetShellVarContext all
	
	Delete "$DESKTOP\FlashDevelop.lnk"
	Delete "$QUICKLAUNCH\FlashDevelop.lnk"
	Delete "$SMPROGRAMS\FlashDevelop\FlashDevelop.lnk"
	Delete "$SMPROGRAMS\FlashDevelop\Documentation.url"
	Delete "$SMPROGRAMS\FlashDevelop\Community.url"
	Delete "$SMPROGRAMS\FlashDevelop\Uninstall.lnk"
	RMDir "$SMPROGRAMS\FlashDevelop"
	
	RMDir /r "$INSTDIR\Docs"
	RMDir /r "$INSTDIR\Library"
	RMDir /r "$INSTDIR\Plugins"
	RMDir /r "$INSTDIR\StartPage"
	RMDir /r "$INSTDIR\Projects"
	RMDir /r "$INSTDIR\Tools"
	
	IfFileExists "$INSTDIR\.local" +5 0
	RMDir /r "$INSTDIR\Data"
	RMDir /r "$INSTDIR\Settings"
	RMDir /r "$INSTDIR\Snippets"
	RMDir /r "$INSTDIR\Templates"
	
	Delete "$INSTDIR\README.txt"
	Delete "$INSTDIR\FirstRun.fdb"
	Delete "$INSTDIR\Exceptions.log"
	Delete "$INSTDIR\FlashDevelop.exe"
	Delete "$INSTDIR\FlashDevelop.exe.config"
	Delete "$INSTDIR\PluginCore.dll"
	Delete "$INSTDIR\SciLexer.dll"
	Delete "$INSTDIR\Scripting.dll"
	Delete "$INSTDIR\Antlr3.dll"
	Delete "$INSTDIR\SwfOp.dll"
	Delete "$INSTDIR\Aga.dll"
	
	Delete "$INSTDIR\Uninstall.exe"
	RMDir "$INSTDIR"
	
	!insertmacro APP_UNASSOCIATE "fdp" "FlashDevelop.Project"
	!insertmacro APP_UNASSOCIATE "fdproj" "FlashDevelop.GenericProject"
	!insertmacro APP_UNASSOCIATE "hxproj" "FlashDevelop.HaXeProject"
	!insertmacro APP_UNASSOCIATE "as2proj" "FlashDevelop.AS2Project"
	!insertmacro APP_UNASSOCIATE "as3proj" "FlashDevelop.AS3Project"
	!insertmacro APP_UNASSOCIATE "docproj" "FlashDevelop.DocProject"
	!insertmacro APP_UNASSOCIATE "lsproj" "FlashDevelop.LoomProject"
	
	!insertmacro APP_UNASSOCIATE "fdi" "FlashDevelop.Theme"
	!insertmacro APP_UNASSOCIATE "fdm" "FlashDevelop.Macros"
	!insertmacro APP_UNASSOCIATE "fdt" "FlashDevelop.Template"
	!insertmacro APP_UNASSOCIATE "fda" "FlashDevelop.Arguments"
	!insertmacro APP_UNASSOCIATE "fds" "FlashDevelop.Snippet"
	!insertmacro APP_UNASSOCIATE "fdb" "FlashDevelop.Binary"
	!insertmacro APP_UNASSOCIATE "fdl" "FlashDevelop.Layout"
	!insertmacro APP_UNASSOCIATE "fdz" "FlashDevelop.Zip"
	
	DeleteRegKey /ifempty HKLM "Software\FlashDevelop"
	DeleteRegKey /ifempty HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\FlashDevelop"
	
	DeleteRegKey /ifempty HKCR "Applications\FlashDevelop.exe"	
	DeleteRegKey /ifempty HKLM "Software\Classes\Applications\FlashDevelop.exe"
	DeleteRegKey /ifempty HKCU "Software\Classes\Applications\FlashDevelop.exe"
	
	!insertmacro UPDATEFILEASSOC
	
SectionEnd

Section /o "un.Settings" UninstSettings
	
	SectionIn 2
	
	Delete "$INSTDIR\.multi"
	Delete "$INSTDIR\.local"
	Delete "$INSTDIR\.locale"
	
	RMDir /r "$INSTDIR\Data"
	RMDir /r "$INSTDIR\Settings"
	RMDir /r "$INSTDIR\Snippets"
	RMDir /r "$INSTDIR\Templates"
	RMDir /r "$LOCALAPPDATA\FlashDevelop"
	RMDir "$INSTDIR"
	
SectionEnd

;--------------------------------

; Uninstall section strings

!insertmacro MUI_UNFUNCTION_DESCRIPTION_BEGIN
!insertmacro MUI_DESCRIPTION_TEXT ${UninstMain} "Uninstalls the main program, other required files and registry modifications."
!insertmacro MUI_DESCRIPTION_TEXT ${UninstSettings} "Uninstalls all settings and snippets from the install directory and user's application data directory."
!insertmacro MUI_UNFUNCTION_DESCRIPTION_END

;--------------------------------

; Event functions

Function .onInit
	
	; Check if the installer is already running
	System::Call 'kernel32::CreateMutexA(i 0, i 0, t "FlashDevelop ${VERSION}") i .r1 ?e'
	Pop $0
	StrCmp $0 0 +3
	MessageBox MB_OK|MB_ICONSTOP "The FlashDevelop ${VERSION} installer is already running."
	Abort
	
	Call GetDotNETVersion
	Pop $0
	${If} $0 == "not_found"
	MessageBox MB_OK|MB_ICONSTOP "You need to install Microsoft.NET 2.0 runtime before installing FlashDevelop."
	Abort
	${EndIf}
	StrCpy $0 $0 "" 1 # skip "v"
	${VersionCompare} $0 "2.0.50727" $1
	${If} $1 == 2
	MessageBox MB_OK|MB_ICONSTOP "You need to install Microsoft.NET 2.0 runtime before installing FlashDevelop. You have $0."
	${EndIf}
	
	Call GetFDInstDir
	Pop $0
	Call GetNeedsReset
	Pop $2
	${If} $2 == "do_reset"
	${If} $0 != "not_found"
	MessageBox MB_OK|MB_ICONEXCLAMATION "You have a version of FlashDevelop installed that may make FlashDevelop unstable or you may miss new features if updated. You should backup you custom setting files and do a full uninstall before installing this one. After install customize the new setting files."
	${EndIf}
	${EndIf}
	
	Call GetFlashVersion
	Pop $0
	${If} $0 == "not_found"
	MessageBox MB_OK|MB_ICONEXCLAMATION "You should install Flash Player (ActiveX for IE) before installing FlashDevelop."
	${Else}
	${VersionCompare} $0 "9.0" $1
	${If} $1 == 2
	MessageBox MB_OK|MB_ICONEXCLAMATION "You should install Flash Player (ActiveX for IE) before installing FlashDevelop. You have $0."
	${EndIf}
	${EndIf}
	
	Call GetJavaVersion
	Pop $0
	${If} $0 == "not_found"
	MessageBox MB_OK|MB_ICONEXCLAMATION "You should install 32-bit Java Runtime (1.6 or later) before installing FlashDevelop."
	${Else}
	${VersionCompare} $0 "1.6" $1
	${If} $1 == 2
	MessageBox MB_OK|MB_ICONEXCLAMATION "You should install 32-bit Java Runtime (1.6 or later) before installing FlashDevelop. You have $0."
	${EndIf}
	${EndIf}
	
	; Default to English
	StrCpy $1 ${NoChangesLocale}
	call .onSelChange
	
FunctionEnd

Function .onSelChange

	${If} ${SectionIsSelected} ${LanguageGroup}
	!insertmacro UnSelectSection ${LanguageGroup}
	!insertmacro SelectSection $1
	${Else}
	!insertmacro StartRadioButtons $1
	!insertmacro RadioButton ${NoChangesLocale}
	!insertmacro RadioButton ${EnglishLocale}
	!insertmacro RadioButton ${ChineseLocale}
	!insertmacro RadioButton ${JapaneseLocale}
	!insertmacro RadioButton ${GermanLocale}
	!insertmacro RadioButton ${BasqueLocale}
	!insertmacro EndRadioButtons
	${EndIf}
	
FunctionEnd

;--------------------------------
