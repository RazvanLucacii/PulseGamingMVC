using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Hosting.Server.Features;

namespace PulseGamingMVC.Helpers
{
    public enum Folders { Images = 0 }
    public class HelperPathProvider
    {
        private IServer server;
        private IWebHostEnvironment environment;

        public HelperPathProvider(IServer server, IWebHostEnvironment environment)
        {
            this.server = server;
            this.environment = environment;
        }

        public string GetFolderPath(Folders folder)
        {
            string carpeta = "";
            if (folder == Folders.Images)
            {
                carpeta = "images";
            }
            return carpeta;
        }

        public string MapPath(string fileName, Folders folder)
        {
            string carpeta = this.GetFolderPath(folder);
            string rootPath = this.environment.WebRootPath;
            string path = Path.Combine(rootPath, carpeta, fileName);
            return path;
        }

        public string MapPathURLProvider()
        {
            var address = this.server.Features.Get<IServerAddressesFeature>().Addresses;
            string url = address.FirstOrDefault();
            return url;
        }

        public string MapPathURLProvider(string fileName, Folders folder)
        {
            string carpeta = this.GetFolderPath(folder);
            var address = this.server.Features.Get<IServerAddressesFeature>().Addresses;
            string url = address.FirstOrDefault();
            string urlPath = url + "/" + carpeta + "/" + fileName;
            return urlPath;
        }

    }
}
