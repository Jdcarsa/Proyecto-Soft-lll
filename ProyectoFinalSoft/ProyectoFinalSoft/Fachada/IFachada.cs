using Microsoft.AspNetCore.Mvc;
using ProyectoFinalSoft.Models;

namespace ProyectoFinalSoft.Fachada
{
    public interface IFachada
    {
       
        void obtenerDocentes();
        void obtenerPeridosAcademicos();
        void obtenerAmbientes();
        void obtenerCompetencias();

        void obtenerTodos(Horario horario);
        void guardarDatosProgComp();
    }
}
