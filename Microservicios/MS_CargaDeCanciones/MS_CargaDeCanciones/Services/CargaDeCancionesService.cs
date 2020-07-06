using Google.Protobuf;
using Grpc.Core;
using Grpc.Net.Client;
using MS_Archivo;
using MS_CargaDeCanciones.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace MS_CargaDeCanciones.Services
{
    public class CargaDeCancionesService
    {
        public async Task<string> CargarCancion(byte[] cancion)
        {
            //GrpcChannelOptions grpcChannelOptions = new GrpcChannelOptions();

            //grpcChannelOptions.Credentials = ChannelCredentials.Insecure;
            AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);


            var httpHandler = new HttpClientHandler();
            httpHandler.ServerCertificateCustomValidationCallback =
                HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;

            ByteString datos = ByteString.CopyFrom(cancion);

            string idArchivo = null;

            using var channel = GrpcChannel.ForAddress(DatosDeServicios.URLMSArchivo, new GrpcChannelOptions { HttpHandler = httpHandler });
            var client = new Archivador.ArchivadorClient(channel);
            
            var respuesta = await client.SolicitarGuardarArchivoAsync(new Archivo { Datos = datos });
            idArchivo = respuesta.Id;
            return idArchivo;
        }
    }
}
