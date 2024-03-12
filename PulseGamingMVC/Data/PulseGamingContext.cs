﻿using Microsoft.EntityFrameworkCore;
using PulseGamingMVC.Models;

namespace PulseGamingMVC.Data
{
    public class PulseGamingContext: DbContext
    {
        public PulseGamingContext(DbContextOptions<PulseGamingContext> options): base(options) { }

        public DbSet<Juego> Juegos { get; set; }

        public DbSet<Usuario> Usuarios { get; set; }

        public DbSet<Genero> Generos { get; set; }

        public DbSet<Editor> Editores { get; set; }
    }
}
