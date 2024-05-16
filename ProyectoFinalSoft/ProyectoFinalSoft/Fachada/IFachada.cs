using Microsoft.AspNetCore.Mvc;
using ProyectoFinalSoft.Models;

namespace ProyectoFinalSoft.Fachada
{
    public interface IFachada
    {
        void obtenerTodos();
        void obtenerTodos(Horario horario);
        Task guardarDatosProgComp();
    }
}
