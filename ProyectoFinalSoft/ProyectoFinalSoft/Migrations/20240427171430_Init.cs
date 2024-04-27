using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProyectoFinalSoft.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Ambientes",
                columns: table => new
                {
                    ambienteId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Ambiente_Nombre = table.Column<string>(type: "varchar(30)", nullable: true),
                    Ambiente_ubicacion = table.Column<string>(type: "varchar(100)", nullable: true),
                    Ambiente_tipo = table.Column<string>(type: "varchar(30)", nullable: true),
                    Ambiente_capacidad = table.Column<decimal>(type: "numeric(3,0)", nullable: false),
                    Ambiente_estado = table.Column<decimal>(type: "numeric(1,0)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ambientes", x => x.ambienteId);
                });

            migrationBuilder.CreateTable(
                name: "Docentes",
                columns: table => new
                {
                    docenteId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Docente_Nombre = table.Column<string>(type: "varchar(30)", nullable: true),
                    Docente_Apellido = table.Column<string>(type: "varchar(30)", nullable: true),
                    Docente_TipoId = table.Column<string>(type: "varchar(20)", nullable: true),
                    Docente_NumId = table.Column<string>(type: "varchar(15)", nullable: true),
                    Docente_Tipo = table.Column<string>(type: "varchar(15)", nullable: true),
                    Docente_TipoContraro = table.Column<string>(type: "varchar(5)", nullable: true),
                    Docente_Area = table.Column<string>(type: "varchar(100)", nullable: true),
                    Docente_estado = table.Column<decimal>(type: "numeric(1,0)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Docentes", x => x.docenteId);
                });

            migrationBuilder.CreateTable(
                name: "PeriodosAcademicos",
                columns: table => new
                {
                    periodoId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    periodoFechaInicio = table.Column<DateOnly>(type: "date", nullable: false),
                    periodoFechaFin = table.Column<DateOnly>(type: "date", nullable: false),
                    Periodo_Nombre = table.Column<string>(type: "varchar(100)", nullable: true),
                    Periodo_estado = table.Column<decimal>(type: "numeric(1,0)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PeriodosAcademicos", x => x.periodoId);
                });

            migrationBuilder.CreateTable(
                name: "Usuarios",
                columns: table => new
                {
                    usuarioId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Usuario_login = table.Column<string>(type: "varchar(30)", nullable: true),
                    Usuario_password = table.Column<string>(type: "varchar(30)", nullable: true),
                    usuario_estado = table.Column<decimal>(type: "numeric(1,0)", nullable: false),
                    docenteId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuarios", x => x.usuarioId);
                    table.ForeignKey(
                        name: "FK_Usuarios_Docentes_docenteId",
                        column: x => x.docenteId,
                        principalTable: "Docentes",
                        principalColumn: "docenteId");
                });

            migrationBuilder.CreateTable(
                name: "Horarios",
                columns: table => new
                {
                    horarioId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Horario_dia = table.Column<string>(type: "varchar(15)", nullable: true),
                    Horario_hora_inicio = table.Column<decimal>(type: "numeric(2,0)", nullable: false),
                    Horario_hora_fin = table.Column<decimal>(type: "numeric(2,0)", nullable: false),
                    Horario_duracion = table.Column<decimal>(type: "numeric(1,0)", nullable: false),
                    Horario_estado = table.Column<decimal>(type: "numeric(1,0)", nullable: false),
                    ProgramaId = table.Column<int>(type: "int", nullable: false),
                    CompetenciaId = table.Column<int>(type: "int", nullable: false),
                    ambienteId = table.Column<int>(type: "int", nullable: true),
                    docenteId = table.Column<int>(type: "int", nullable: true),
                    periodoAcademicoId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Horarios", x => x.horarioId);
                    table.ForeignKey(
                        name: "FK_Horarios_Ambientes_ambienteId",
                        column: x => x.ambienteId,
                        principalTable: "Ambientes",
                        principalColumn: "ambienteId");
                    table.ForeignKey(
                        name: "FK_Horarios_Docentes_docenteId",
                        column: x => x.docenteId,
                        principalTable: "Docentes",
                        principalColumn: "docenteId");
                    table.ForeignKey(
                        name: "FK_Horarios_PeriodosAcademicos_periodoAcademicoId",
                        column: x => x.periodoAcademicoId,
                        principalTable: "PeriodosAcademicos",
                        principalColumn: "periodoId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Horarios_ambienteId",
                table: "Horarios",
                column: "ambienteId");

            migrationBuilder.CreateIndex(
                name: "IX_Horarios_docenteId",
                table: "Horarios",
                column: "docenteId");

            migrationBuilder.CreateIndex(
                name: "IX_Horarios_periodoAcademicoId",
                table: "Horarios",
                column: "periodoAcademicoId");

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_docenteId",
                table: "Usuarios",
                column: "docenteId",
                unique: true,
                filter: "[docenteId] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Horarios");

            migrationBuilder.DropTable(
                name: "Usuarios");

            migrationBuilder.DropTable(
                name: "Ambientes");

            migrationBuilder.DropTable(
                name: "PeriodosAcademicos");

            migrationBuilder.DropTable(
                name: "Docentes");
        }
    }
}
