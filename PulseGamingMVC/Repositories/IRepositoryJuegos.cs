using PulseGamingMVC.Models;

namespace PulseGamingMVC.Repositories
{
    public interface IRepositoryJuegos
    {
        List<Juego> GetJuegos();
        Juego FindJuego(int IdJuego);

        void RegistrarJuego(string nombre, int idGenero, string imagen, double precio, string descripcion);
    }
}
