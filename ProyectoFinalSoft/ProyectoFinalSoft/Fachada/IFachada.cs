using Microsoft.AspNetCore.Mvc;

namespace ProyectoFinalSoft.Fachada
{
    public interface IFachada
    {
        Task<IActionResult> obtenerProgramas();
        Task<IActionResult> obtenerDocentes();
        Task<IActionResult> obtenerPeridosAcademicos();
        Task<IActionResult> obtenerAmbientes();
        Task<IActionResult>  obtenerCompetencias();

        Task<IActionResult> guardarDatosProgComp();
    }
}
