using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Grpc.Core;
using System.IO;
using Google.Protobuf;

namespace MS_Archivo.Services
{
    public class ArchivoService : Archivador.ArchivadorBase
    {
        private readonly ILogger<ArchivoService> _logger;
        private const string RutaArchivos = "archivos";

        public ArchivoService(ILogger<ArchivoService> logger)
        {
            Directory.CreateDirectory(RutaArchivos);
            _logger = logger;
        }

        public override Task<IdArchivo> SolicitarGuardarArchivo(Archivo archivo, ServerCallContext context)
        {
            return Task.FromResult(new IdArchivo
            {
                Id = GuardarArchivo(archivo.Datos.ToArray())
            });
        }

        public override Task<Archivo> SolicitarDescargarArchivo(IdArchivo idArchivo, ServerCallContext context)
        {
            return Task.FromResult(new Archivo
            {
                Datos = ByteString.CopyFrom(LeerArchivoPorId(idArchivo.Id))
            });
        }

        private byte[] LeerArchivoPorId(string id)
        {
            byte[] fileBytes;
            try
            {
                string path = RutaArchivos + "/" + id + ".mp3";
                fileBytes = File.ReadAllBytes(@path);
            }
            catch (FileNotFoundException)
            {
                fileBytes = new byte[0];
            }
            return fileBytes;
        }

        private string GuardarArchivo(byte[] datos)
        {
            string id = Guid.NewGuid().ToString();
            string path = RutaArchivos + "/" + id + ".mp3";
            File.WriteAllBytes(@path, datos);
            return id;
        }

    }
}
