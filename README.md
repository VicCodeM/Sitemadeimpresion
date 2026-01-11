# Sistema de Control de Impresion Industrial

Este sistema proporciona una solucion centralizada para la gestion y autorizacion de impresiones industriales utilizando impresoras Zebra. El sistema esta diseñado bajo el principio de que imprimir es una decision del negocio validada por un motor de reglas.

## Arquitectura del Sistema

El proyecto se divide en tres componentes principales:

1. **API Backend**: Procesa las solicitudes de impresion, evalua las reglas de negocio y autoriza la generacion de codigo ZPL.
2. **Portal Web Admin**: Interfaz de gestion para administrar catalogos (Maquinas, Impresoras, Empleados, Etiquetas, Lotes y Usuarios), visualizar reportes y configurar reglas.
3. **App de Produccion**: Aplicacion de escritorio instalada en las terminales de planta donde los operadores solicitan las impresiones.

## Requisitos Previos

- .NET 10.0 SDK o superior.
- PostgreSQL 15 o superior.
- Impresora Zebra (para pruebas reales) o emulador ZPL.

## Instalacion y Configuracion

### 1. Clonar el repositorio

```bash
git clone https://github.com/usuario/SistemaImpresion.git
cd SistemaImpresion
```

### 2. Configuracion de la Base de Datos

Cree una base de datos en PostgreSQL llamada `sistema_impresion`.

Actualice la cadena de conexion en los archivos `appsettings.json` de los proyectos `SistemaImpresion.API` y `SistemaImpresion.Web`:

```json
"ConnectionStrings": {
  "DefaultConnection": "Host=localhost;Database=sistema_impresion;Username=postgres;Password=tu_contrasena"
}
```

### 3. Aplicar Migraciones

Desde la raiz del proyecto, ejecute:

```bash
dotnet ef database update --project src/SistemaImpresion.Datos --startup-project src/SistemaImpresion.Web
```

### 4. Inicializacion de Datos

Al ejecutar el proyecto por primera vez, el sistema creara automaticamente un usuario administrador:
- **Usuario**: admin
- **Contraseña**: Admin123

## Ejecucion del Sistema

Para que el sistema funcione correctamente, los componentes deben iniciarse en el siguiente orden:

### Paso 1: API Backend
```bash
cd src/SistemaImpresion.API
dotnet run
```
La API estara disponible en `http://localhost:5023`.

### Paso 2: Portal Web Admin
```bash
cd src/SistemaImpresion.Web
dotnet run
```
El portal estara disponible en `http://localhost:5068`.

### Paso 3: App de Produccion
```bash
cd src/SistemaImpresion.AppProduccion
dotnet run
```

## Guia de Uso

### Gestion Administrativa (Portal Web)
1. Acceda al Portal Web con las credenciales de administrador.
2. Registre las **Maquinas** por su nombre de red (Hostname).
3. Configure las **Impresoras** asociadas a cada maquina (Puerto USB).
4. Registre el catalogo de **Empleados**.
5. Cargue las plantillas de **Etiquetas** en formato ZPL.
6. Cree un **Lote** de produccion y asignelo a las maquinas correspondientes.
7. Defina las **Reglas** de autorizacion para permitir que ciertas maquinas impriman etiquetas especificas.

### Proceso de Impresion (App de PC)
1. El operador ingresa su numero de empleado.
2. El operador indica la cantidad de etiquetas a imprimir.
3. El sistema valida automaticamente si el empleado esta activo, si la maquina tiene un lote asignado y si existen reglas que autoricen la operacion.
4. Si se autoriza, la impresora Zebra recibe el codigo ZPL y ejecuta la impresion.
5. El resultado se registra en el modulo de Reportes del Portal Web para auditoria.

## Propuesta de Valor Comercial

Este sistema ha sido diseñado como una solucion escalable y robusta para la industria manufacturera, ofreciendo beneficios tangibles que facilitan su comercializacion:

- **Reduccion de Errores Desconocidos**: El motor de reglas impide impresiones no autorizadas, eliminando el desperdicio de material por etiquetas incorrectas.
- **Trazabilidad Total**: Historico detallado de quien, que y donde se imprimio, ideal para auditorias de calidad y cumplimiento de normas internacionales.
- **Arquitectura Centralizada**: Permite gestionar multiples lineas de produccion desde un solo punto, reduciendo costos de mantenimiento tecnico.
- **Independencia de Hardware**: Aunque optimizado para Zebra, la arquitectura permite adaptar otros protocolos de impresion industrial.
- **Seguridad**: Control de acceso granular para administradores y operadores de planta.

## Dependencias y Licenciamiento de Terceros

El sistema ha sido desarrollado utilizando exclusivamente tecnologias y librerias de codigo abierto (Open Source), lo que garantiza:

- **Cero Costos de Licencia**: No se requiere el pago de regalias o licencias mensuales por el uso de librerias de terceros.
- **Libertad de Comercializacion**: Las librerias utilizadas (como EF Core, Npgsql y BCrypt.Net) operan bajo licencias MIT y similares, permitiendo su integracion en productos comerciales sin restricciones.
- **Sostenibilidad**: El uso de estandares de la industria asegura que el sistema pueda ser mantenido sin depender de proveedores de software de pago.

## Estructura del Proyecto

- `src/SistemaImpresion.Dominio`: Entidades fundamentales y definiciones.
- `src/SistemaImpresion.Datos`: Capa de persistencia con Entity Framework Core.
- `src/SistemaImpresion.Negocio`: Lógica del motor de reglas de autorizacion.
- `src/SistemaImpresion.API`: Servicios web para la app de produccion.
- `src/SistemaImpresion.Web`: Interfaz de gestion administrativa.
- `src/SistemaImpresion.AppProduccion`: Cliente WPF de escritorio.

## Licencia

Uso interno industrial. Todos los derechos reservados por el autor. Consulte el archivo [LICENSE](LICENSE) para mas detalles.
