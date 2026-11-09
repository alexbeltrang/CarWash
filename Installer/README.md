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
5197. Solo habilita acceso LAN desde la subred local en perfil Privado.
Rechaza interferir con una tarea CarWashApi de otra ubicacion o un puerto ocupado.
No habilita redes publicas. Si un prerrequisito pide reinicio, la API iniciara
con Windows despues de reiniciar.

Actualizaciones conservan appsettings y otros ajustes existentes; el usuario
selecciona explicitamente URL y ruta de base. Binding redirects del cliente se
actualizan desde la distribucion compilada. No seleccionar base nueva para
actualizar una instalacion existente. Hacer respaldo de datos antes de actualizar.
Desinstalacion elimina solo la tarea propia y su regla LAN, pero conserva bases
y configuraciones. Cambiar componentes no sustituye una desinstalacion previa.

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