using Microsoft.EntityFrameworkCore;
using PulseGamingMVC.Data;
using PulseGamingMVC.Helpers;
using PulseGamingMVC.Models;

namespace PulseGamingMVC.Repositories
{
    public class RepositoryUsuarios
    {
        private PulseGamingContext context;

        public RepositoryUsuarios(PulseGamingContext context)
        {
            this.context = context;
        }

        private async Task<int> GetMaxIdUsuarioAsync()
        {
            if (this.context.Usuarios.Count() == 0)
            {
                return 1;
            }
            else
            {
                return await
                    this.context.Usuarios.MaxAsync(z => z.IdUsuario) + 1;
            }
        }

        public async Task RegisterUser(string nombre, string apellidos, string email, string password, int telefono)
        {
            Usuario user = new Usuario();
            user.IdUsuario = await this.GetMaxIdUsuarioAsync();
            user.Apellidos = apellidos;
            user.Nombre = nombre;
            user.Email = email;
            user.Telefono = telefono;
            //CADA USUARIO TENDRA UN SALT DISTINTO
            user.Salt = HelperJuegos.GenerateSalt();
            //GUARDAMOS EL PASSWORD EN BYTE[]
            user.Password =
                HelperJuegos.EncryptPassword(password, user.Salt);
            this.context.Usuarios.Add(user);
            await this.context.SaveChangesAsync();
        }

        //NECESITAMOS UN METODO PARA VALIDAR AL USUARIO
        //DICHO METODO DEVOLVERA EL PROPIO USUARIO
        //COMO COMPARAMOS?? email CAMPO UNICO
        //password (12345)
        //1) RECUPERAR EL USER POR SU EMAIL
        //2) RECUPERAMOS EL SALT DEL USUARIO
        //3) CONVERTIMOS DE NUEVO EL PASSWORD CON EL SALT
        //4) RECUPERAMOS EL BYTE[] DE PASSWORD DE LA BBDD
        //5) COMPARAMOS LOS DOS ARRAYS (BBDD) Y EL GENERADO EN EL CODIGO
        public async Task<Usuario> LogInUserAsync(string email, string password)
        {
            Usuario user = await
                this.context.Usuarios.FirstOrDefaultAsync(x => x.Email == email);
            if (user == null)
            {
                return null;
            }
            else
            {
                string salt = user.Salt;
                byte[] temp =
                    HelperJuegos.EncryptPassword(password, salt);
                byte[] passUser = user.Password;
                bool response =
                    HelperJuegos.CompareArrays(temp, passUser);
                if (response == true)
                {
                    return user;
                }
                else
                {
                    return null;
                }
            }
        }
    }
}