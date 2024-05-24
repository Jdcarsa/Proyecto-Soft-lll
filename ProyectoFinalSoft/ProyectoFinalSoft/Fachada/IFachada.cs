using Microsoft.AspNetCore.Mvc;
using ProyectoFinalSoft.Models;

namespace ProyectoFinalSoft.Fachada
{
    public interface IFachada
    {
        void getAll();
        void getAll(Horario horario);
        Task<IActionResult> saveDataProgComp();
    }
}
