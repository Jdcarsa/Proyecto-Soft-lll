using System;
using Microsoft.EntityFrameworkCore.Metadata;
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
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Ambientes",
                columns: table => new
                {
                    ambienteId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Ambiente_Codigo = table.Column<string>(type: "varchar(30)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Ambiente_Nombre = table.Column<string>(type: "varchar(30)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Ambiente_ubicacion = table.Column<string>(type: "varchar(100)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Ambiente_tipo = table.Column<string>(type: "varchar(30)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Ambiente_capacidad = table.Column<decimal>(type: "numeric(3,0)", nullable: false),
                    Ambiente_estado = table.Column<decimal>(type: "numeric(1,0)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ambientes", x => x.ambienteId);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Competencias",
                columns: table => new
                {
                    competenciaId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Competencia_Nombre = table.Column<string>(type: "varchar(30)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Competencia_Tipo = table.Column<string>(type: "varchar(30)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Competencia_Estado = table.Column<decimal>(type: "numeric(1,0)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Competencias", x => x.competenciaId);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Coordinador",
                columns: table => new
                {
                    coordinadorId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Coordinador_Nombre = table.Column<string>(type: "varchar(30)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Coordinador_Estado = table.Column<decimal>(type: "numeric(1,0)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Coordinador", x => x.coordinadorId);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Docentes",
                columns: table => new
                {
                    docenteId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Docente_Nombre = table.Column<string>(type: "varchar(30)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Docente_Apellido = table.Column<string>(type: "varchar(30)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Docente_TipoId = table.Column<string>(type: "varchar(20)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Docente_NumId = table.Column<string>(type: "varchar(15)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Docente_Tipo = table.Column<string>(type: "varchar(15)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Docente_TipoContraro = table.Column<string>(type: "varchar(5)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Docente_Area = table.Column<string>(type: "varchar(100)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Docente_estado = table.Column<decimal>(type: "numeric(1,0)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Docentes", x => x.docenteId);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "PeriodosAcademicos",
                columns: table => new
                {
                    periodoId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Periodo_Fecha_Inicio = table.Column<DateOnly>(type: "date", nullable: false),
                    Periodo_Fecha_Fin = table.Column<DateOnly>(type: "date", nullable: false),
                    Periodo_Nombre = table.Column<string>(type: "varchar(100)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Periodo_estado = table.Column<decimal>(type: "numeric(1,0)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PeriodosAcademicos", x => x.periodoId);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Programas",
                columns: table => new
                {
                    programaId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Programa_Nombre = table.Column<string>(type: "varchar(30)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Programa_Estado = table.Column<decimal>(type: "numeric(1,0)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Programas", x => x.programaId);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Usuarios",
                columns: table => new
                {
                    usuarioId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Usuario_login = table.Column<string>(type: "varchar(30)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Usuario_password = table.Column<string>(type: "varchar(30)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    usuario_estado = table.Column<decimal>(type: "numeric(1,0)", nullable: false),
                    docenteId = table.Column<int>(type: "int", nullable: true),
                    coordinadorId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuarios", x => x.usuarioId);
                    table.ForeignKey(
                        name: "FK_Usuarios_Coordinador_coordinadorId",
                        column: x => x.coordinadorId,
                        principalTable: "Coordinador",
                        principalColumn: "coordinadorId");
                    table.ForeignKey(
                        name: "FK_Usuarios_Docentes_docenteId",
                        column: x => x.docenteId,
                        principalTable: "Docentes",
                        principalColumn: "docenteId");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "CompetenciaPrograma",
                columns: table => new
                {
                    CompetenciascompetenciaId = table.Column<int>(type: "int", nullable: false),
                    ProgramasprogramaId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompetenciaPrograma", x => new { x.CompetenciascompetenciaId, x.ProgramasprogramaId });
                    table.ForeignKey(
                        name: "FK_CompetenciaPrograma_Competencias_CompetenciascompetenciaId",
                        column: x => x.CompetenciascompetenciaId,
                        principalTable: "Competencias",
                        principalColumn: "competenciaId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CompetenciaPrograma_Programas_ProgramasprogramaId",
                        column: x => x.ProgramasprogramaId,
                        principalTable: "Programas",
                        principalColumn: "programaId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Horarios",
                columns: table => new
                {
                    horarioId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Horario_dia = table.Column<string>(type: "varchar(15)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Horario_hora_inicio = table.Column<decimal>(type: "numeric(2,0)", nullable: false),
                    Horario_hora_fin = table.Column<decimal>(type: "numeric(2,0)", nullable: false),
                    Horario_duracion = table.Column<decimal>(type: "numeric(1,0)", nullable: false),
                    Horario_estado = table.Column<decimal>(type: "numeric(1,0)", nullable: false),
                    ambienteId = table.Column<int>(type: "int", nullable: true),
                    docenteId = table.Column<int>(type: "int", nullable: true),
                    periodoAcademicoId = table.Column<int>(type: "int", nullable: true),
                    ProgramaId = table.Column<int>(type: "int", nullable: false),
                    CompetenciaId = table.Column<int>(type: "int", nullable: false)
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
                        name: "FK_Horarios_Competencias_CompetenciaId",
                        column: x => x.CompetenciaId,
                        principalTable: "Competencias",
                        principalColumn: "competenciaId",
                        onDelete: ReferentialAction.Cascade);
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
                    table.ForeignKey(
                        name: "FK_Horarios_Programas_ProgramaId",
                        column: x => x.ProgramaId,
                        principalTable: "Programas",
                        principalColumn: "programaId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_CompetenciaPrograma_ProgramasprogramaId",
                table: "CompetenciaPrograma",
                column: "ProgramasprogramaId");

            migrationBuilder.CreateIndex(
                name: "IX_Horarios_ambienteId",
                table: "Horarios",
                column: "ambienteId");

            migrationBuilder.CreateIndex(
                name: "IX_Horarios_CompetenciaId",
                table: "Horarios",
                column: "CompetenciaId");

            migrationBuilder.CreateIndex(
                name: "IX_Horarios_docenteId",
                table: "Horarios",
                column: "docenteId");

            migrationBuilder.CreateIndex(
                name: "IX_Horarios_periodoAcademicoId",
                table: "Horarios",
                column: "periodoAcademicoId");

            migrationBuilder.CreateIndex(
                name: "IX_Horarios_ProgramaId",
                table: "Horarios",
                column: "ProgramaId");

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_coordinadorId",
                table: "Usuarios",
                column: "coordinadorId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_docenteId",
                table: "Usuarios",
                column: "docenteId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CompetenciaPrograma");

            migrationBuilder.DropTable(
                name: "Horarios");

            migrationBuilder.DropTable(
                name: "Usuarios");

            migrationBuilder.DropTable(
                name: "Ambientes");

            migrationBuilder.DropTable(
                name: "Competencias");

            migrationBuilder.DropTable(
                name: "PeriodosAcademicos");

            migrationBuilder.DropTable(
                name: "Programas");

            migrationBuilder.DropTable(
                name: "Coordinador");

            migrationBuilder.DropTable(
                name: "Docentes");
        }
    }
}
