#define AppVersion "1.1.1"

[Setup]
AppId={{D6E4A1A7-38C2-43E3-8A89-C954D7AE2D23}
AppName=CarWash
AppVersion={#AppVersion}
DefaultDirName={autopf}\CarWash
DefaultGroupName=CarWash
PrivilegesRequired=admin
ArchitecturesAllowed=x64os
ArchitecturesInstallIn64BitMode=x64os
MinVersion=10.0.19045
OutputDir=output
OutputBaseFilename=CarWash-Setup-x64
Compression=lzma2
SolidCompression=yes
WizardStyle=modern
CloseApplications=yes
RestartApplications=no
UninstallDisplayName=CarWash
SetupIconFile=..\splash_car_icon.ico

[Languages]
Name: "spanish"; MessagesFile: "compiler:Languages\Spanish.isl"

[Types]
Name: "complete"; Description: "Escritorio y API local"
Name: "custom"; Description: "Seleccionar componentes"; Flags: iscustom

[Components]
Name: "desktop"; Description: "Aplicacion de escritorio"; Types: complete
Name: "api"; Description: "API local (inicia con Windows)"; Types: complete
Name: "api\newdb"; Description: "Crear base nueva y administrador inicial (no reemplaza una existente)"; Flags: dontinheritcheck

[Files]
Source: "cache\net481.exe"; Flags: dontcopy
Source: "cache\vc_redist.x64.exe"; Flags: dontcopy
Source: "configure.ps1"; Flags: dontcopy
Source: "staging\desktop\*.exe"; DestDir: "{app}\Desktop"; Components: desktop; Flags: ignoreversion
Source: "staging\desktop\*.dll"; DestDir: "{app}\Desktop"; Components: desktop; Flags: ignoreversion
Source: "staging\desktop\CarWash.exe.config"; DestDir: "{app}\Desktop"; Components: desktop; Flags: onlyifdoesntexist
Source: "staging\desktop\CarWash.exe.config"; DestDir: "{app}\Templates"; DestName: "desktop.config"; Components: desktop; Flags: ignoreversion
Source: "staging\api\*"; DestDir: "{app}\Api"; Components: api; Excludes: "*.pdb,appsettings*.json,*.db,*.db-wal,*.db-shm"; Flags: recursesubdirs ignoreversion
Source: "staging\api\appsettings.json"; DestDir: "{app}\Api"; Components: api; Flags: onlyifdoesntexist
Source: "staging\empty.db"; DestDir: "{app}"; Components: api\newdb; Flags: ignoreversion
Source: "configure.ps1"; DestDir: "{app}"; Flags: ignoreversion
Source: "uninstall.ps1"; DestDir: "{app}"; Flags: ignoreversion
Source: "staging\payload-manifest.json"; DestDir: "{app}"; Flags: ignoreversion

[Icons]
Name: "{group}\CarWash"; Filename: "{app}\Desktop\CarWash.exe"; WorkingDir: "{app}\Desktop"; Components: desktop
Name: "{autodesktop}\CarWash"; Filename: "{app}\Desktop\CarWash.exe"; WorkingDir: "{app}\Desktop"; Components: desktop

[UninstallDelete]
Type: dirifempty; Name: "{app}"

[Code]
var
  DatabasePage, UserPage, UrlPage: TInputQueryWizardPage;
  BackupPage: TInputDirWizardPage;
  RestartPending: Boolean;

function JsonEscape(Value: String): String;
begin
  Result := Value;
  StringChangeEx(Result, '\', '\\', True);
  StringChangeEx(Result, '"', '\"', True);
end;

function JsonBoolean(Value: Boolean): String;
begin
  if Value then Result := 'true' else Result := 'false';
end;

procedure WriteSelection;
var
  Json, Url: String;
  ResultCode: Integer;
begin
  if not Exec(ExpandConstant('{sys}\icacls.exe'), '"' + ExpandConstant('{tmp}') +
    '" /inheritance:r /grant:r *S-1-5-18:(OI)(CI)F *S-1-5-32-544:(OI)(CI)F', '', SW_HIDE, ewWaitUntilTerminated, ResultCode) or (ResultCode <> 0) then
    RaiseException('No se pudo proteger el directorio temporal.');
  Url := UrlPage.Values[0];
  if WizardIsComponentSelected('api') then Url := 'http://localhost:5197';
  Json := '{"Desktop":' + JsonBoolean(WizardIsComponentSelected('desktop')) +
    ',"Api":' + JsonBoolean(WizardIsComponentSelected('api')) +
    ',"NewDatabase":' + JsonBoolean(WizardIsComponentSelected('api\newdb')) +
    ',"ApiUrl":"' + JsonEscape(Url) + '","DatabasePath":"' + JsonEscape(DatabasePage.Values[0]) +
    '","BackupDirectory":"' + JsonEscape(BackupPage.Values[0]) +
    '","Usuario":"' + JsonEscape(UserPage.Values[0]) + '","Clave":"' + JsonEscape(UserPage.Values[1]) + '"}';
  if not SaveStringToFile(ExpandConstant('{tmp}\selection.json'), Utf8Encode(Json), False) then
    RaiseException('No se pudo guardar la seleccion temporal.');
end;

function Configure(Mode: String; Installed: Boolean): Boolean;
var
  Script, Arguments: String;
  ResultCode: Integer;
begin
  if Installed then Script := ExpandConstant('{app}\configure.ps1')
  else Script := ExpandConstant('{tmp}\configure.ps1');
  Arguments := '-NoProfile -ExecutionPolicy Bypass -File "' + Script + '" -Mode ' + Mode +
    ' -SelectionPath "' + ExpandConstant('{tmp}\selection.json') + '" -InstallDir "' + ExpandConstant('{app}') + '"';
  if RestartPending then Arguments := Arguments + ' -SkipStart';
  Result := Exec(ExpandConstant('{sys}\WindowsPowerShell\v1.0\powershell.exe'), Arguments, '', SW_HIDE, ewWaitUntilTerminated, ResultCode) and (ResultCode = 0);
end;

procedure InitializeWizard;
begin
  DatabasePage := CreateInputQueryPage(wpSelectComponents, 'Base de datos de la API',
    'Archivo SQLite del equipo', 'Si no crea una base nueva, indique una base CarWash existente.');
  DatabasePage.Add('Ruta completa del archivo .db:', False);
  DatabasePage.Values[0] := ExpandConstant('{commonappdata}\SplashCar\CarWash.db');
  BackupPage := CreateInputDirPage(DatabasePage.ID, 'Respaldos automaticos',
    'Carpeta local dedicada para los respaldos de la API',
    'Se realiza un respaldo cada hora. Elija una carpeta fuera de la instalacion y de SplashCar. Las copias externas se conservan al desinstalar. Para protegerse de una falla del disco, use otro disco y mantenga ademas una copia externa.', False, 'CarWashBackups');
  BackupPage.Add('Carpeta de respaldo:');
  BackupPage.Values[0] := ExpandConstant('{commonappdata}\CarWashBackups');
  UserPage := CreateInputQueryPage(BackupPage.ID, 'Administrador inicial',
    'Usuario inicial de CarWash', 'Solo se crea si la base no tiene usuarios activos; los existentes se conservan. Use de 4 a 64 caracteres ASCII. No hay contrasena predeterminada.');
  UserPage.Add('Usuario:', False);
  UserPage.Add('Contrasena:', True);
  UserPage.Add('Confirmar contrasena:', True);
  UrlPage := CreateInputQueryPage(UserPage.ID, 'Conexion del escritorio',
    'Direccion de la API existente', 'Indique la URL base HTTP/HTTPS del servidor.');
  UrlPage.Add('URL de la API:', False);
  UrlPage.Values[0] := 'http://localhost:5197';
end;

function ShouldSkipPage(PageID: Integer): Boolean;
begin
  Result := ((PageID = DatabasePage.ID) and not WizardIsComponentSelected('api')) or
    ((PageID = BackupPage.ID) and not WizardIsComponentSelected('api')) or
    ((PageID = UserPage.ID) and not WizardIsComponentSelected('api')) or
    ((PageID = UrlPage.ID) and (not WizardIsComponentSelected('desktop') or WizardIsComponentSelected('api')));
end;

function ValidCredential(Value: String): Boolean;
var Index: Integer;
begin
  Result := (Length(Value) >= 4) and (Length(Value) <= 64) and (Value = Trim(Value));
  for Index := 1 to Length(Value) do
    if (Ord(Value[Index]) < 32) or (Ord(Value[Index]) > 126) then Result := False;
end;

function NextButtonClick(CurPageID: Integer): Boolean;
begin
  Result := True;
  if CurPageID = wpSelectComponents then
    Result := WizardIsComponentSelected('desktop') or WizardIsComponentSelected('api');
  if CurPageID = DatabasePage.ID then begin
    if WizardIsComponentSelected('api\newdb') then Result := not FileExists(DatabasePage.Values[0])
    else Result := FileExists(DatabasePage.Values[0]);
  end;
  if CurPageID = UserPage.ID then
    Result := ValidCredential(UserPage.Values[0]) and ValidCredential(UserPage.Values[1]) and (UserPage.Values[1] = UserPage.Values[2]);
  if CurPageID = BackupPage.ID then Result := Length(Trim(BackupPage.Values[0])) > 3;
  if not Result then MsgBox('Revise la seleccion: base nueva no puede reemplazar un archivo existente; API sin base nueva requiere una base existente, la carpeta de respaldo debe ser valida y las claves deben coincidir.', mbError, MB_OK);
end;

function RunPrerequisite(Name, Arguments: String): String;
var ResultCode: Integer;
begin
  Result := '';
  ExtractTemporaryFile(Name);
  if not Exec(ExpandConstant('{tmp}\') + Name, Arguments, '', SW_SHOW, ewWaitUntilTerminated, ResultCode) then
    Result := 'No se pudo iniciar el prerrequisito ' + Name
  else if ResultCode = 3010 then RestartPending := True
  else if (ResultCode <> 0) and (ResultCode <> 1638) then
    Result := 'No se pudo instalar ' + Name + '. Codigo: ' + IntToStr(ResultCode);
end;

function PrepareToInstall(var NeedsRestart: Boolean): String;
var Release: Cardinal;
begin
  Result := '';
  WriteSelection;
  ExtractTemporaryFile('configure.ps1');
  if not Configure('Validate', False) then begin
    Result := 'La configuracion no es valida. Revise URL, ruta de base, carpeta de respaldo y credenciales.';
    Exit;
  end;
  if WizardIsComponentSelected('desktop') and
    (not RegQueryDWordValue(HKLM, 'SOFTWARE\Microsoft\NET Framework Setup\NDP\v4\Full', 'Release', Release) or (Release < 533320)) then
    Result := RunPrerequisite('net481.exe', '/q /norestart');
  if Result = '' then Result := RunPrerequisite('vc_redist.x64.exe', '/install /quiet /norestart');
  if (Result = '') and not Configure('Stop', False) then
    Result := 'No se pudo preparar la API: revise si ya existe otra instalacion o si el puerto 5197 esta ocupado.';
  NeedsRestart := RestartPending;
end;

procedure CurStepChanged(CurStep: TSetupStep);
begin
  if CurStep = ssPostInstall then
    if not Configure('Install', True) then
      RaiseException('No se pudo configurar CarWash. Los datos existentes no se eliminan. Revise la instalacion antes de iniciar la aplicacion.');
end;

function NeedRestart: Boolean;
begin
  Result := RestartPending;
end;

function InitializeUninstall: Boolean;
begin
  Result := MsgBox('Desinstalar CarWash eliminara PERMANENTEMENTE su base de datos, configuracion y archivos locales de todos los usuarios. Las copias de la carpeta externa de respaldo se conservan. Haga un respaldo FUERA de las carpetas CarWash antes de continuar. Desea eliminar todo CarWash?',
    mbConfirmation, MB_YESNO or MB_DEFBUTTON2) = IDYES;
end;

procedure CurUninstallStepChanged(CurUninstallStep: TUninstallStep);
var
  ResultCode: Integer;
begin
  if CurUninstallStep = usUninstall then begin
    if not FileExists(ExpandConstant('{app}\uninstall.ps1')) then
      RaiseException('Falta el script de limpieza. Reinstale esta version antes de desinstalar.');
    if not Exec(ExpandConstant('{sys}\WindowsPowerShell\v1.0\powershell.exe'),
      '-NoProfile -ExecutionPolicy Bypass -File "' + ExpandConstant('{app}\uninstall.ps1') +
      '" -InstallDir "' + ExpandConstant('{app}') + '"', '', SW_HIDE, ewWaitUntilTerminated, ResultCode) or (ResultCode <> 0) then
      RaiseException('No se pudo completar la limpieza de CarWash. Revise permisos, archivos en uso u otras instalaciones. No se declara una desinstalacion completa.');
  end;
end;