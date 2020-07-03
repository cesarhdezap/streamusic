using Logica.Clases;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Logica.Utilerias
{
    public static class UtileriasDeArchivos
    {
        public static readonly string RUTA_ARCHIVOS = "Canciones/";
        public static byte[] LeerArchivoPorId(string idArchivo)
        {
            ValidarDirectorio();
            byte[] fileBytes;
            try
            {
                string path = RUTA_ARCHIVOS + idArchivo + ".mp3";
                fileBytes = File.ReadAllBytes(@path);
            }
            catch (FileNotFoundException)
            {
                throw new ArgumentException("No se encontro el archivo.");
            }
            return fileBytes;
        }

        public static string GuardarArchivo(string id, byte[] datos)
        {
            ValidarDirectorio();
            string path = RUTA_ARCHIVOS + id + ".mp3";
            File.WriteAllBytes(@path, datos);
            return id;
        }

        public static byte[] LeerArchivoPorURL(string path)
        {
            byte[] fileBytes;
            try
            {
                fileBytes = File.ReadAllBytes(@path);
            }
            catch (FileNotFoundException)
            {
                throw new ArgumentException("No se encontro el archivo.");
            }
            return fileBytes;
        }

        public static void BorrarArchivo(string idArchivo)
        {
            ValidarDirectorio();
            string path = RUTA_ARCHIVOS + idArchivo + ".mp3";
            File.Delete(@path);
        }

        public static bool CancionYaDescargada(string idArchivo)
        {
            ValidarDirectorio();
            string path = RUTA_ARCHIVOS + idArchivo + ".mp3";
            return File.Exists(path);
        }

        private static void ValidarDirectorio()
        {
            if (!Directory.Exists(@RUTA_ARCHIVOS))
            {
                Directory.CreateDirectory(RUTA_ARCHIVOS);
            }
        }
    }
}
