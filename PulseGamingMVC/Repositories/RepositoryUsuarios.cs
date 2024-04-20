using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
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
    public class RepositoryUsuarios : IRepositoryUsuarios
    {
        private PulseGamingContext context;

        public RepositoryUsuarios(PulseGamingContext context)
        {
            this.context = context;
        }

        public async Task<int> GetMaxIdUsuarioAsync()
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

        public async Task RegistrarUsuario(string password, string nombre, string apellidos, string email, int telefono, int IDRole)
        {
            Usuario user = new Usuario();
            user.IdUsuario = await this.GetMaxIdUsuarioAsync();
            user.Password = password;
            user.Apellidos = apellidos;
            user.Nombre = nombre;
            user.Email = email;
            user.Telefono = telefono;
            user.IDRole = IDRole;
            this.context.Usuarios.Add(user);
            await this.context.SaveChangesAsync();
        }

        public async Task<Usuario> LogInUserAsync(string email, string password)
        {
            
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