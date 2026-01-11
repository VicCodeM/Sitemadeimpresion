using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace SistemaImpresion.Datos.Migrations
{
    /// <inheritdoc />
    public partial class MigracionInicial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Empleados",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    NumeroEmpleado = table.Column<string>(type: "text", nullable: false),
                    Nombre = table.Column<string>(type: "text", nullable: false),
                    Apellido = table.Column<string>(type: "text", nullable: false),
                    Departamento = table.Column<string>(type: "text", nullable: false),
                    Puesto = table.Column<string>(type: "text", nullable: true),
                    FechaAlta = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    FechaModificacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Activo = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Empleados", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Etiquetas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Codigo = table.Column<string>(type: "text", nullable: false),
                    Nombre = table.Column<string>(type: "text", nullable: false),
                    Descripcion = table.Column<string>(type: "text", nullable: true),
                    ContenidoZPL = table.Column<string>(type: "text", nullable: false),
                    AnchoMM = table.Column<int>(type: "integer", nullable: false),
                    AltoMM = table.Column<int>(type: "integer", nullable: false),
                    Categoria = table.Column<string>(type: "text", nullable: true),
                    Version = table.Column<string>(type: "text", nullable: true),
                    FechaCreacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    FechaModificacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Activo = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Etiquetas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Maquinas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Codigo = table.Column<string>(type: "text", nullable: false),
                    Nombre = table.Column<string>(type: "text", nullable: false),
                    Descripcion = table.Column<string>(type: "text", nullable: true),
                    IdentificadorRed = table.Column<string>(type: "text", nullable: true),
                    DireccionMAC = table.Column<string>(type: "text", nullable: true),
                    LimiteImpresionDiario = table.Column<int>(type: "integer", nullable: false),
                    FechaRegistro = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    FechaModificacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Activo = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Maquinas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nombre = table.Column<string>(type: "text", nullable: false),
                    Descripcion = table.Column<string>(type: "text", nullable: false),
                    Tipo = table.Column<int>(type: "integer", nullable: false),
                    PuedeImprimir = table.Column<bool>(type: "boolean", nullable: false),
                    PuedeAutorizar = table.Column<bool>(type: "boolean", nullable: false),
                    PuedeAdministrar = table.Column<bool>(type: "boolean", nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    FechaModificacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Activo = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Lotes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Numero = table.Column<string>(type: "text", nullable: false),
                    Descripcion = table.Column<string>(type: "text", nullable: false),
                    EtiquetaId = table.Column<int>(type: "integer", nullable: false),
                    CantidadMaxima = table.Column<int>(type: "integer", nullable: false),
                    FechaInicio = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    FechaFin = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CodigoProducto = table.Column<string>(type: "text", nullable: true),
                    Cliente = table.Column<string>(type: "text", nullable: true),
                    FechaCreacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    FechaModificacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Activo = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lotes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Lotes_Etiquetas_EtiquetaId",
                        column: x => x.EtiquetaId,
                        principalTable: "Etiquetas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Impresoras",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Codigo = table.Column<string>(type: "text", nullable: false),
                    Modelo = table.Column<string>(type: "text", nullable: false),
                    NumeroSerie = table.Column<string>(type: "text", nullable: false),
                    PuertoUSB = table.Column<string>(type: "text", nullable: false),
                    MaquinaId = table.Column<int>(type: "integer", nullable: false),
                    ResolucionDPI = table.Column<int>(type: "integer", nullable: true),
                    AnchoMaximoMM = table.Column<int>(type: "integer", nullable: true),
                    FechaInstalacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UltimoMantenimiento = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Notas = table.Column<string>(type: "text", nullable: true),
                    FechaCreacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    FechaModificacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Activo = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Impresoras", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Impresoras_Maquinas_MaquinaId",
                        column: x => x.MaquinaId,
                        principalTable: "Maquinas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Usuarios",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    NombreUsuario = table.Column<string>(type: "text", nullable: false),
                    HashContrasena = table.Column<string>(type: "text", nullable: false),
                    Nombre = table.Column<string>(type: "text", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false),
                    RolId = table.Column<int>(type: "integer", nullable: false),
                    UltimoAcceso = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IntentosFallidos = table.Column<int>(type: "integer", nullable: false),
                    Bloqueado = table.Column<bool>(type: "boolean", nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    FechaModificacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Activo = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuarios", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Usuarios_Roles_RolId",
                        column: x => x.RolId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "LotesMaquinas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    LoteId = table.Column<int>(type: "integer", nullable: false),
                    MaquinaId = table.Column<int>(type: "integer", nullable: false),
                    FechaAsignacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    FechaDesasignacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Prioridad = table.Column<int>(type: "integer", nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    FechaModificacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Activo = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LotesMaquinas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LotesMaquinas_Lotes_LoteId",
                        column: x => x.LoteId,
                        principalTable: "Lotes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LotesMaquinas_Maquinas_MaquinaId",
                        column: x => x.MaquinaId,
                        principalTable: "Maquinas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Auditorias",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UsuarioId = table.Column<int>(type: "integer", nullable: true),
                    TipoAccion = table.Column<int>(type: "integer", nullable: false),
                    Entidad = table.Column<string>(type: "text", nullable: false),
                    EntidadId = table.Column<int>(type: "integer", nullable: true),
                    ValoresAnteriores = table.Column<string>(type: "text", nullable: true),
                    ValoresNuevos = table.Column<string>(type: "text", nullable: true),
                    DireccionIP = table.Column<string>(type: "text", nullable: true),
                    UserAgent = table.Column<string>(type: "text", nullable: true),
                    Fecha = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Descripcion = table.Column<string>(type: "text", nullable: true),
                    Exitosa = table.Column<bool>(type: "boolean", nullable: false),
                    MensajeError = table.Column<string>(type: "text", nullable: true),
                    FechaCreacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    FechaModificacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Activo = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Auditorias", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Auditorias_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Impresiones",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    MaquinaId = table.Column<int>(type: "integer", nullable: false),
                    ImpresoraId = table.Column<int>(type: "integer", nullable: false),
                    EmpleadoId = table.Column<int>(type: "integer", nullable: false),
                    LoteId = table.Column<int>(type: "integer", nullable: false),
                    EtiquetaId = table.Column<int>(type: "integer", nullable: false),
                    Cantidad = table.Column<int>(type: "integer", nullable: false),
                    Estado = table.Column<int>(type: "integer", nullable: false),
                    FechaSolicitud = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    FechaAutorizacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    FechaEjecucion = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Resultado = table.Column<string>(type: "text", nullable: true),
                    MensajeError = table.Column<string>(type: "text", nullable: true),
                    MotivoDenegacion = table.Column<string>(type: "text", nullable: true),
                    AutorizadoPorUsuarioId = table.Column<int>(type: "integer", nullable: true),
                    DireccionIPOrigen = table.Column<string>(type: "text", nullable: true),
                    HashZPL = table.Column<string>(type: "text", nullable: true),
                    AutorizadoPorId = table.Column<int>(type: "integer", nullable: true),
                    FechaCreacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    FechaModificacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Activo = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Impresiones", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Impresiones_Empleados_EmpleadoId",
                        column: x => x.EmpleadoId,
                        principalTable: "Empleados",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Impresiones_Etiquetas_EtiquetaId",
                        column: x => x.EtiquetaId,
                        principalTable: "Etiquetas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Impresiones_Impresoras_ImpresoraId",
                        column: x => x.ImpresoraId,
                        principalTable: "Impresoras",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Impresiones_Lotes_LoteId",
                        column: x => x.LoteId,
                        principalTable: "Lotes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Impresiones_Maquinas_MaquinaId",
                        column: x => x.MaquinaId,
                        principalTable: "Maquinas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Impresiones_Usuarios_AutorizadoPorId",
                        column: x => x.AutorizadoPorId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ReglasImpresion",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    MaquinaId = table.Column<int>(type: "integer", nullable: false),
                    EtiquetaId = table.Column<int>(type: "integer", nullable: false),
                    Autorizada = table.Column<bool>(type: "boolean", nullable: false),
                    LimiteImpresiones = table.Column<int>(type: "integer", nullable: false),
                    CreadoPorUsuarioId = table.Column<int>(type: "integer", nullable: false),
                    Motivo = table.Column<string>(type: "text", nullable: true),
                    CreadoPorId = table.Column<int>(type: "integer", nullable: true),
                    FechaCreacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    FechaModificacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Activo = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReglasImpresion", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReglasImpresion_Etiquetas_EtiquetaId",
                        column: x => x.EtiquetaId,
                        principalTable: "Etiquetas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ReglasImpresion_Maquinas_MaquinaId",
                        column: x => x.MaquinaId,
                        principalTable: "Maquinas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ReglasImpresion_Usuarios_CreadoPorId",
                        column: x => x.CreadoPorId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Auditorias_Fecha_UsuarioId_TipoAccion",
                table: "Auditorias",
                columns: new[] { "Fecha", "UsuarioId", "TipoAccion" });

            migrationBuilder.CreateIndex(
                name: "IX_Auditorias_UsuarioId",
                table: "Auditorias",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_Empleados_NumeroEmpleado",
                table: "Empleados",
                column: "NumeroEmpleado",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Etiquetas_Codigo",
                table: "Etiquetas",
                column: "Codigo",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Impresiones_AutorizadoPorId",
                table: "Impresiones",
                column: "AutorizadoPorId");

            migrationBuilder.CreateIndex(
                name: "IX_Impresiones_EmpleadoId",
                table: "Impresiones",
                column: "EmpleadoId");

            migrationBuilder.CreateIndex(
                name: "IX_Impresiones_EtiquetaId",
                table: "Impresiones",
                column: "EtiquetaId");

            migrationBuilder.CreateIndex(
                name: "IX_Impresiones_FechaSolicitud_MaquinaId_Estado",
                table: "Impresiones",
                columns: new[] { "FechaSolicitud", "MaquinaId", "Estado" });

            migrationBuilder.CreateIndex(
                name: "IX_Impresiones_ImpresoraId",
                table: "Impresiones",
                column: "ImpresoraId");

            migrationBuilder.CreateIndex(
                name: "IX_Impresiones_LoteId",
                table: "Impresiones",
                column: "LoteId");

            migrationBuilder.CreateIndex(
                name: "IX_Impresiones_MaquinaId",
                table: "Impresiones",
                column: "MaquinaId");

            migrationBuilder.CreateIndex(
                name: "IX_Impresoras_Codigo",
                table: "Impresoras",
                column: "Codigo",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Impresoras_MaquinaId",
                table: "Impresoras",
                column: "MaquinaId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Lotes_EtiquetaId",
                table: "Lotes",
                column: "EtiquetaId");

            migrationBuilder.CreateIndex(
                name: "IX_Lotes_Numero",
                table: "Lotes",
                column: "Numero",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_LotesMaquinas_LoteId",
                table: "LotesMaquinas",
                column: "LoteId");

            migrationBuilder.CreateIndex(
                name: "IX_LotesMaquinas_MaquinaId",
                table: "LotesMaquinas",
                column: "MaquinaId");

            migrationBuilder.CreateIndex(
                name: "IX_Maquinas_Codigo",
                table: "Maquinas",
                column: "Codigo",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Maquinas_DireccionMAC",
                table: "Maquinas",
                column: "DireccionMAC");

            migrationBuilder.CreateIndex(
                name: "IX_Maquinas_IdentificadorRed",
                table: "Maquinas",
                column: "IdentificadorRed");

            migrationBuilder.CreateIndex(
                name: "IX_ReglasImpresion_CreadoPorId",
                table: "ReglasImpresion",
                column: "CreadoPorId");

            migrationBuilder.CreateIndex(
                name: "IX_ReglasImpresion_EtiquetaId",
                table: "ReglasImpresion",
                column: "EtiquetaId");

            migrationBuilder.CreateIndex(
                name: "IX_ReglasImpresion_MaquinaId",
                table: "ReglasImpresion",
                column: "MaquinaId");

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_NombreUsuario",
                table: "Usuarios",
                column: "NombreUsuario",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_RolId",
                table: "Usuarios",
                column: "RolId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Auditorias");

            migrationBuilder.DropTable(
                name: "Impresiones");

            migrationBuilder.DropTable(
                name: "LotesMaquinas");

            migrationBuilder.DropTable(
                name: "ReglasImpresion");

            migrationBuilder.DropTable(
                name: "Empleados");

            migrationBuilder.DropTable(
                name: "Impresoras");

            migrationBuilder.DropTable(
                name: "Lotes");

            migrationBuilder.DropTable(
                name: "Usuarios");

            migrationBuilder.DropTable(
                name: "Maquinas");

            migrationBuilder.DropTable(
                name: "Etiquetas");

            migrationBuilder.DropTable(
                name: "Roles");
        }
    }
}
