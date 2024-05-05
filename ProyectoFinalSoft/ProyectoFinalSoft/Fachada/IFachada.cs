using Microsoft.AspNetCore.Mvc;

namespace ProyectoFinalSoft.Fachada
{
    public interface IFachada
    {
        void obtenerProgramas();
        void obtenerDocentes();
        void obtenerPeridosAcademicos();
        void obtenerAmbientes();
        void obtenerCompetencias();

        Task<IActionResult> guardarDatosProgComp();
    }
}
