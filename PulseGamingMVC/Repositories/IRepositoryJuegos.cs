using PulseGamingMVC.Models;

namespace PulseGamingMVC.Repositories
{
    public interface IRepositoryJuegos
    {
        List<Juego> GetJuegos();

        List<Juego> GetJuegosPrecioAsce();

        List<Juego> GetJuegosPrecioDesc();

        List<Juego> GetJuegosBusqueda();

        Juego FindJuego(int IdJuego);

        void RegistrarJuego(string nombre, int idGenero, string imagen, double precio, string descripcion, int idEditor);

        Task<List<Editor>> GetEditoresAsync();

        Task<List<Genero>> GetGenerosAsync();

        void DeleteJuego(int idjuego);

        void ModificarJuego(int idJuego, string nombre, int idGenero, string imagen, double precio, string descripcion, int idEditor);

        void CrearEditor(string nombre);

        void CrearGenero(string nombre);

        void ModificarEditor(int idEditor, string nombre);

        void ModificarGenero(int idGenero, string nombre);

        void DeleteEditor(int idEditor);

        void DeleteGenero(int idGenero);

        Task<Editor> FindEditorAsync(int idEditor);

        Task<Genero> FindGeneroAsync(int idGenero);
    }
}
