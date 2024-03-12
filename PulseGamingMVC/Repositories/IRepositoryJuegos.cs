using PulseGamingMVC.Models;

namespace PulseGamingMVC.Repositories
{
    public interface IRepositoryJuegos
    {
        List<Juego> GetJuegos();

        Juego FindJuego(int IdJuego);

        void RegistrarJuego(string nombre, int idGenero, string imagen, double precio, string descripcion, int idEditor);

        Task<List<Editor>> GetEditoresAsync();

        Task<List<Genero>> GetGenerosAsync();

        void DeleteJuego(int idjuego);

        void ModificarJuego(int idJuego, string nombre, int idGenero, string imagen, double precio, string descripcion, int idEditor);
    }
}
