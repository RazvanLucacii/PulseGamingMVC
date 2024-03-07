using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using PulseGamingMVC.Data;
using PulseGamingMVC.Helpers;
using PulseGamingMVC.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

#region PROCEDIMIENTOS ALMACENADOS

//create procedure SP_TODOS_USUARIOS
//as
//	select * from Usuarios
//go

//create procedure SP_DETALLES_USUARIO
//(@IDUsuario int)
//as
//	select * from Usuarios where IDUsuario = @IDUsuario
//go

#endregion

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

        public async Task RegisterUser(string nombre, string apellidos, string email, string password, int telefono, int IDRole)
        {
            Usuario user = new Usuario();
            user.IdUsuario = await this.GetMaxIdUsuarioAsync();
            user.Apellidos = apellidos;
            user.Nombre = nombre;
            user.Email = email;
            user.Telefono = telefono;
            user.IDRole = IDRole;
            //CADA USUARIO TENDRA UN SALT DISTINTO
            user.Salt = HelperJuegos.GenerateSalt();
            //GUARDAMOS EL PASSWORD EN BYTE[]
            user.Password =
                HelperJuegos.EncryptPassword(password, user.Salt);
            this.context.Usuarios.Add(user);
            await this.context.SaveChangesAsync();
        }

        public async Task<Usuario> LogInUserAsync(string email, string password)
        {
            Usuario user = await this.context.Usuarios.FirstOrDefaultAsync(x => x.Email == email);
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

        public List<Usuario> GetUsuarios()
        {
            string sql = "SP_TODOS_USUARIOS";
            var consulta = this.context.Usuarios.FromSqlRaw(sql);
            return consulta.ToList();
        }

        public Usuario FindUsuarioById(int idUsuario)
        {
            string sql = "SP_DETALLES_USUARIO @IDUsuario";
            SqlParameter pamId = new SqlParameter("@IDUsuario", idUsuario);
            var consulta = this.context.Usuarios.FromSqlRaw(sql, pamId);
            Usuario usuario = consulta.AsEnumerable().FirstOrDefault();
            return usuario;
        }

        public async Task ModificarUsuario(int idUsuario, string nombre, string apellidos, string email, string password, int telefono, int IDRole)
        {
            Usuario usuario = FindUsuarioById(idUsuario);
            if (usuario == null)
            {
                throw new ArgumentNullException("El usuario no existe.");
            }

            // Actualiza los datos del usuario con los nuevos valores
            usuario.Nombre = nombre;
            usuario.Apellidos = apellidos;
            usuario.Email = email;
            usuario.Telefono = telefono;
            usuario.IDRole = IDRole;

            // Si se proporciona una nueva contraseña, la actualiza
            if (!string.IsNullOrEmpty(password))
            {
                usuario.Salt = HelperJuegos.GenerateSalt();
                usuario.Password = HelperJuegos.EncryptPassword(password, usuario.Salt);
            }

            // Guarda los cambios en la base de datos
            this.context.Entry(usuario).State = EntityState.Modified;
            await this.context.SaveChangesAsync();
        }

        public void DeleteUsuario(int idUsuario)
        {
            Usuario usuario = this.FindUsuarioById(idUsuario);
            this.context.Usuarios.Remove(usuario);
            this.context.SaveChanges();
        }
    }
}