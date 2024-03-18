using Microsoft.AspNetCore.Mvc;
using PulseGamingMVC.Models;
using PulseGamingMVC.Repositories;

namespace PulseGamingMVC.ViewComponents
{
    public class MenuCategoriasViewComponent : ViewComponent
    {
        private IRepositoryJuegos repo;

        public MenuCategoriasViewComponent(IRepositoryJuegos repo)
        {
            this.repo = repo;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            List<Genero> generos = await this.repo.GetGenerosAsync();
            return View(generos);
        }
    }
}
