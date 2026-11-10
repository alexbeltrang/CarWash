# Instalador CarWash x64

Proyecto `CarWash.Installer` integrado en `CarWash.slnx`. Genera
`Installer/output/CarWash-Setup-x64.exe`, un ejecutable con todos los archivos
y prerrequisitos incluidos. No ejecuta la instalacion al compilar.

## Generar

En Visual Studio para Windows: restaurar NuGet, seleccionar Release/x64 y
compilar CarWash.Installer. Debug no genera el paquete para no ralentizar el
desarrollo habitual. Tambien se puede ejecutar:

```powershell
.\Installer\build.ps1
```

El equipo generador requiere Visual Studio/MSBuild con desarrollo de escritorio
.NET y developer pack .NET Framework4.8.1, SDK .NET9 o posterior e Inno Setup6.
Los repositorios CarWash, CarWashApi y CarWash.Mobile deben ser hermanos.
La primera generacion descarga los redistribuibles oficiales Microsoft en cache
y verifica Authenticode. Generaciones posteriores reutilizan esas descargas.
No versionar staging, cache ni output. El paquete no esta firmado con certificado
de la empresa; Windows puede mostrar editor desconocido hasta firmar la entrega.

## Equipo Destino

Windows10 22H2 o Windows11 x64 actualizado. No requiere Visual Studio, SDK,
SQL Server, IIS ni Internet durante la instalacion. Requiere administrador
para instalar componentes, runtime y tarea de inicio. El cliente se ejecuta
como usuario normal.

El asistente permite:

- Solo escritorio: indicar URL de una API existente.
- Solo API local: elegir base existente compatible o marcar crear base nueva.
- Escritorio y API local: cliente conectado a localhost:5197.
- Base nueva: subcomponente de API, desmarcado por defecto. Archivo inexistente
  y usuario/contrasena elegidos durante instalacion, sin clave predeterminada.

Desde1.0.3, al seleccionar API siempre se pide usuario/contrasena inicial, igual
que el instalador API original, pero sin su clave fija. Reutiliza el mismo
CarWashSeeder: crea el usuario en base nueva o existente compatible sin usuarios
activos; si ya hay usuarios activos conserva todos y no restablece contrasenas.

Incluye .NET Framework4.8.1 offline para el escritorio, Visual C++ x64,
DLL administradas, SQLite nativo x64, API .NET9 autocontenida y esquema SQLite
vacio generado desde SQL versionado. No distribuye bases de trabajo, usuarios,
SMTP, certificados privados ni configuracion de produccion.

## Datos Y Actualizaciones

La base nueva rechaza sobrescritura, incluso si el archivo aparece despues de
la validacion. Solo crea el administrador solicitado; no inventa empresa,
servicios, tarifas ni configuracion fiscal. Deben configurarse con datos reales.
El archivo local heredado del escritorio nuevo usa
`%LOCALAPPDATA%/CarWash/CarWash.db`, separado de la base del servidor.

API usa una tarea programada CarWashApi como SYSTEM al iniciar Windows y puerto
5197. Ejecuta Kestrel mediante run-api.ps1, no crea sitio ni application pool
en IIS y no se registra como servicio Windows. Se administra en el Programador
de tareas, Biblioteca, CarWashApi. Solo habilita acceso LAN desde la subred
local en perfil Privado.
Rechaza interferir con una tarea CarWashApi de otra ubicacion o un puerto ocupado.
No habilita redes publicas. Si un prerrequisito pide reinicio, la API iniciara
con Windows despues de reiniciar.

Actualizaciones conservan appsettings y otros ajustes existentes; el usuario
selecciona explicitamente URL y ruta de base. Binding redirects del cliente se
actualizan desde la distribucion compilada. No seleccionar base nueva para
actualizar una instalacion existente. Hacer respaldo de datos antes de actualizar.
Desde1.0.2, desinstalacion elimina permanentemente tarea/regla propias, base API
seleccionada y auxiliares SQLite, configuraciones, logs y datos locales CarWash
de todos los perfiles de usuario. Pide confirmacion con No predeterminado.
Hacer respaldo FUERA de carpetas CarWash, incluidos los respaldos existentes
dentro de AppData/Local/CarWash, porque tambien se borraran. La carpeta de una
base personalizada se conserva si contiene otros archivos; solo se elimina la
base elegida y sus auxiliares. Se rechazan enlaces/uniones y otra tarea/API
activa de otra instalacion para no borrar datos ajenos. El cliente solo no
elimina el servidor externo. .NET/Visual C++ compartidos no se desinstalan.
Los accesos directos, registro de desinstalacion y carpeta final los retira
Inno Setup. Esto no borra el historial/auditoria propios de Windows.
Cambiar componentes no sustituye una desinstalacion previa.

Correo, WhatsApp y servidor de impresion salen deshabilitados/sin credenciales;
requieren configuracion real posterior. Los controladores especificos de la
impresora los proporciona su fabricante; no se incluyen controladores arbitrarios.

## Verificacion

```powershell
.\Installer\test.ps1 -VerifyPayload
```

Comprueba selecciones, configuracion conservada, hashes, esquema vacio y
autenticacion de una API temporal con administrador sintetico; no registra tareas,
no envia mensajes ni toca bases reales. La prueba compilada existente de escritorio
verifica SQLite x64 y expansion de la ruta local en un directorio temporal.
La instalacion, reinicio, actualizacion y desinstalacion completas deben probarse
en una VM Windows limpia. No ejecutar el instalador sobre el servidor operativo
solo para probarlo.

### Correccion 1.0.1

Se separan las variables XML del cliente y JSON de la API para permitir la
configuracion conjunta. La prueba tambien recorre ambos componentes con base
existente/nueva, conserva ajustes y simula tareas/firewall. Ejecutada ademas en
Windows PowerShell5.1, igual que el asistente.
La entrega corregida se genero como CarWash-Setup-x64-1.0.1.exe para no
sobrescribir el instalador anterior abierto; futuras compilaciones conservan
el nombre habitual CarWash-Setup-x64.exe. Cerrar asistentes anteriores antes
de instalar y no seleccionar base nueva si el archivo ya existe.

### Limpieza Completa 1.0.2

Entrega CarWash-Setup-x64-1.0.2.exe. El desinstalador antiguo no cambia
automaticamente: actualizar en la misma ruta con esta version antes de
desinstalar desde Windows. Actualizar conserva datos; desinstalar los borra.
installation.json registra componentes, base validada y directorios creados,
sin credenciales, para limpiar tambien rutas personalizadas y carpetas vacias.
La limpieza falla visiblemente si no puede terminar; no declara exito silencioso.
test.ps1 cubre borrado temporal completo/escritorio solo, archivos SQLite
auxiliares, datos de usuario, carpetas vacias, procesos ya terminados, tarea
externa y junctions; tareas/procesos/firewall completamente simulados.
Validado con Windows PowerShell5.1. No se ha ejecutado borrado real en produccion;
confirmacion UAC y ciclo completo Inno siguen pendientes en una VM limpia.

### Usuario Inicial 1.0.3

Entrega CarWash-Setup-x64-1.0.3.exe. Mismo sembrador y arranque con tarea que el
instalador API anterior; no se cambia a IIS. Probar en Windows PowerShell5.1
base nueva, existente sin usuarios y existente poblada: las dos primeras crean
usuario, la ultima conserva su hash. La API temporal autentica el usuario
creado por configuracion sobre la base existente vacia. No distribuye usuarios
de trabajo ni credenciales predeterminadas. No reiniciar servidores reales
como parte de las pruebas.

## Respaldos Automaticos 1.1.1

Entrega CarWash-Setup-x64-1.1.1.exe. Al seleccionar API, el asistente permite
elegir una carpeta local dedicada; predeterminada `%ProgramData%/CarWashBackups`.
No puede estar dentro de la instalacion, SplashCar o AppData/CarWash, contener
la base/instalacion ni usar enlaces. Una carpeta no vacia requiere el marcador
backup-owner.json de la misma instalacion. Se valida antes de modificar datos.
La carpeta queda protegida para SYSTEM y Administradores y no se elimina al
desinstalar. Elegir otro disco y mantener una copia externa: el destino
predeterminado no protege contra falla del disco del sistema.

CarWashBackup ejecuta cada hora CarWashSeeder --backup como SYSTEM, tambien
al terminar la instalacion si no hay reinicio pendiente. No permite ejecuciones
solapadas y tiene limite de una hora. Usa SQLite BackupDatabase con la API
activa, incluso con WAL; no copia directamente el archivo abierto. Comprueba
integridad, calcula SHA256 y publica cada carpeta solo al terminar. Incluye
CarWash.db y copias de appsettings.json, email.json y printing.json si existen.
No respalda binarios ni otros archivos personalizados. Desde1.1.1 conserva
todas las copias del dia actual y solo la ultima de cada uno de los 30 dias
calendario anteriores, segun la hora local del equipo. No conserva copias
semanales adicionales. Solo despues de publicar un respaldo valido elimina
duplicadas de dias anteriores y copias mas antiguas; no borra archivos ajenos.
Los dias sin respaldos no se rellenan con copias de mas de 30 dias. La regla
se aplica tambien a las copias propias existentes en la siguiente ejecucion
exitosa, una vez actualizado el motor; no basta con conservar la version1.1.0.

En el Programador de tareas revisar CarWashBackup y su ultimo resultado:
0 indica exito, 1 fallo del motor. backup-status.json guarda Success,
AttemptUtc y LastSuccessUtc; comprobar que este ultimo siga siendo reciente.
La ausencia de un estado reciente tambien requiere revision. No hay alertas
por correo, cifrado adicional ni sincronizacion a nube automaticos. Los
archivos de configuracion pueden contener secretos: proteger tambien sus
copias externas. La desinstalacion retira ambas tareas propias pero conserva
la carpeta externa de respaldo y sus copias.

Para ensayar restauracion, usar un directorio aislado, comprobar los hashes
de backup.json y ejecutar `CarWashSeeder.exe --verify-schema <copia.db>`;
abrir esa copia sin conectarla a la API operativa. Para una restauracion real,
programar mantenimiento, detener API/respaldos y clientes, preservar el estado
actual, restaurar CarWash.db y revisar configuraciones antes de reabrir.
No mezclar una base restaurada con WAL/SHM antiguos. No ejecutar este proceso
en produccion como prueba automatizada.

Verificado con pruebas temporales de WAL activo, escritura no confirmada,
integridad, hashes, retencion, bloqueo concurrente, fallos y estado truncado.
La suite PowerShell5.1 simula tareas/firewall y prueba rutas y conservacion
de respaldos externos. Registro real de la tarea SYSTEM, permisos efectivos,
reinicio y ciclo Inno completo siguen pendientes en una VM limpia.