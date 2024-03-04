using System.Security.Cryptography;
using System.Text;

namespace PulseGamingMVC.Helpers
{
    public class HelperJuegos
    {
        //vamos a tener un par de metodos que no tienen nada que ver con criptografia
        public static string GenerateSalt()
        {
            Random random = new Random();
            string salt = "";
            for (int i = 1; i <= 50; i++)
            {
                int aleat = random.Next(1, 255);
                char letra = Convert.ToChar(aleat);
                salt += letra;
            }
            return salt;
        }

        //necesitamos metodo para comparar si los password son iguales. Debemos comparar a nivel de byte

        public static bool CompareArrays(byte[] a, byte[] b)
        {
            bool iguales = true;
            if (a.Length != b.Length)
            {
                iguales = false;
            }
            else
            {
                for (int i = 0; i < a.Length; i++)
                {
                    //preguntar el contenido de bytes si es distinto
                    if (a[i].Equals(b[i]) == false)
                    {
                        iguales = false;
                        break;
                    }
                }
            }
            return iguales;
        }

        //tenemos un metodo para cifrar el password
        //recibimos el password (string) y el salt(string)
        //devolvemos el array de bytes[] del resultado cifrado
        public static byte[] EncryptPassword(string password, string salt)
        {
            string contenido = password + salt;
            SHA512 sha = SHA512.Create();
            //convertimos contenido a bytes[]
            byte[] salida = Encoding.UTF8.GetBytes(contenido);
            //creamos las iteraciones
            for (int i = 1; i <= 114; i++)
            {
                salida = sha.ComputeHash(salida);
            }
            sha.Clear();
            return salida;
        }
    }
}
