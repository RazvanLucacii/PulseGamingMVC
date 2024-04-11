using PulseGamingMVC.Models;

namespace PulseGamingMVC.Repositories
{
    public interface IRepositoryUsuarios
    {
        Task<int> GetMaxIdUsuarioAsync();
        Task RegisterUser(string nombre, string apellidos, string email, string password, int telefono, int IDRole);
        Task<Usuario> LogInUserAsync(string email, string password);
        List<Usuario> GetUsuarios();
        Usuario FindUsuarioById(int idUsuario);
        Task ModificarUsuario(int idUsuario, string nombre, string apellidos, string email, string password, int telefono, int IDRole);
        void DeleteUsuario(int idUsuario);
    }
}
