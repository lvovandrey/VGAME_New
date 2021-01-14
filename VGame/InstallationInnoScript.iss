; Script generated by the Inno Script Studio Wizard.
; SEE THE DOCUMENTATION FOR DETAILS ON CREATING INNO SETUP SCRIPT FILES!

#define MyAppName "VGame"
#define MyAppVersion "1.0"
#define MyAppPublisher "Lvov A.A."
#define MyAppExeName "VanyaGame.exe"

[Setup]
; NOTE: The value of AppId uniquely identifies this application.
; Do not use the same AppId value in installers for other applications.
; (To generate a new GUID, click Tools | Generate GUID inside the IDE.)
AppId={{D5805AB1-233A-4937-BB54-8EA3D60FD737}
AppName={#MyAppName}
AppVersion={#MyAppVersion}
;AppVerName={#MyAppName} {#MyAppVersion}
AppPublisher={#MyAppPublisher}
DefaultDirName={pf}\{#MyAppName}
DefaultGroupName={#MyAppName}
OutputDir=I:\VGameOutput
OutputBaseFilename=setup1
SetupIconFile=C:\Users\Professional\Pictures\book-icon.ico
Compression=lzma
SolidCompression=yes

[Languages]
Name: "russian"; MessagesFile: "compiler:Languages\Russian.isl"

[Tasks]
Name: "desktopicon"; Description: "{cm:CreateDesktopIcon}"; GroupDescription: "{cm:AdditionalIcons}"; Flags: unchecked

[Files]
Source: "I:\VGame\Union\VanyaGame.exe"; DestDir: "{app}"; Flags: ignoreversion
Source: "I:\VGame\Union\*"; DestDir: "{app}"; Flags: ignoreversion recursesubdirs createallsubdirs
Source: "NET Framework 4.7.2.exe"; DestDir: "{tmp}"; Flags: deleteafterinstall; Check: not IsRequiredDotNetDetectedSilence
Source: "I:\VGame\InstallationTools\*"; DestDir: "{tmp}"; Flags: ignoreversion recursesubdirs createallsubdirs deleteafterinstall


[Icons]
Name: "{group}\{#MyAppName}"; Filename: "{app}\VanyaGame.exe"
Name: "{group}\�������� �� ��������"; Filename: "{app}\CardsEditor.exe"
Name: "{commondesktop}\{#MyAppName}"; Filename: "{app}\VanyaGame.exe"; Tasks: desktopicon
Name: "{commondesktop}\�������� �� ��������"; Filename: "{app}\CardsEditor.exe"; Tasks: desktopicon
Name: "{group}\������� VGame"; Filename: "{uninstallexe}"; IconFilename: "{app}\uninstall-icon.ico"

[Run]
Filename: "{tmp}\NET Framework 4.7.2.exe"; Check: not IsRequiredDotNetDetected
Filename: "{tmp}\InstallationTools.exe"; Parameters: """{app}\VanyaGame.exe.Config"" ""{app}"" ""{app}\Data\Fruits.db"""

[Code]
//-----------------------------------------------------------------------------
//  �������� ������� ������� ����������
//-----------------------------------------------------------------------------
function IsDotNetDetected(version: string; release: cardinal): boolean;

var 
    reg_key: string; // ��������������� ��������� ���������� �������
    success: boolean; // ���� ������� ������������� ������ .NET
    release45: cardinal; // ����� ������ ��� ������ 4.5.x
    key_value: cardinal; // ����������� �� ������� �������� �����
    sub_key: string;

begin

    success := false;
    reg_key := 'SOFTWARE\Microsoft\NET Framework Setup\NDP\';
    
    // ������ 3.0
    if Pos('v3.0', version) = 1 then
      begin
          sub_key := 'v3.0';
          reg_key := reg_key + sub_key;
          success := RegQueryDWordValue(HKLM, reg_key, 'InstallSuccess', key_value);
          success := success and (key_value = 1);
      end;

    // ������ 3.5
    if Pos('v3.5', version) = 1 then
      begin
          sub_key := 'v3.5';
          reg_key := reg_key + sub_key;
          success := RegQueryDWordValue(HKLM, reg_key, 'Install', key_value);
          success := success and (key_value = 1);
      end;

     // ������ 4.0 ���������� �������
     if Pos('v4.0 Client Profile', version) = 1 then
      begin
          sub_key := 'v4\Client';
          reg_key := reg_key + sub_key;
          success := RegQueryDWordValue(HKLM, reg_key, 'Install', key_value);
          success := success and (key_value = 1);
      end;

     // ������ 4.0 ����������� �������
     if Pos('v4.0 Full Profile', version) = 1 then
      begin
          sub_key := 'v4\Full';
          reg_key := reg_key + sub_key;
          success := RegQueryDWordValue(HKLM, reg_key, 'Install', key_value);
          success := success and (key_value = 1);
      end;

     // ������ 4.5
     if Pos('v4.5', version) = 1 then
      begin
          sub_key := 'v4\Full';
          reg_key := reg_key + sub_key;
          success := RegQueryDWordValue(HKLM, reg_key, 'Release', release45);
          success := success and (release45 >= release);
      end;

    result := success;

end;


//-----------------------------------------------------------------------------
//  �������-������� ��� �������������� ���������� ������ ��� ������
//-----------------------------------------------------------------------------
function IsRequiredDotNetDetected(): boolean;
begin

  if not IsDotNetDetected('v4.5', 461814) then
    begin
    if MsgBox('{#MyAppName} ������� ��������� Microsoft .NET Framework 4.7.2 Full Profile.'#13#13
              '�� ����� ���������� ������ ����� �� ����������.'#13#13
              '�� ������ ���������� ��� � ������� ������� ������������ ������ ��� ����� ��������������'#13#13
              '���������� .NET Framework 4.7.2 ����� ������? ', mbConfirmation, MB_YESNO) = IDYES then
        begin
            result:= false;
            Exit;
        end;       
   end; 
   result := true;
end;

//-----------------------------------------------------------------------------
//  �������-������� ��� �������������� ���������� ������ ��� ������
//-----------------------------------------------------------------------------
function IsRequiredDotNetDetectedSilence(): boolean;
begin
  result:= IsDotNetDetected('v4.5', 461814); 
end;

//-----------------------------------------------------------------------------
//    Callback-�������, ���������� ��� ������������� ���������
//-----------------------------------------------------------------------------
function InitializeSetup(): boolean;
begin
  result := true;
end;
