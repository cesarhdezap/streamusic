using Grpc.Core;
using Grpc.Net.Client;
using MS_Archivo;
using MS_DescargaDeCanciones.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace MS_DescargaDeCanciones.Services
{
    public class DescargaDeCancionesService
    {
        public async Task<byte[]> DescargarCancion(string idCancion)
        {
            //GrpcChannelOptions grpcChannelOptions = new GrpcChannelOptions();
            //grpcChannelOptions.Credentials = ChannelCredentials.Insecure;
            AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);
            var httpHandler = new HttpClientHandler();
            httpHandler.ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;

            using var channel = GrpcChannel.ForAddress(DatosDeServicios.URLMSArchivo, new GrpcChannelOptions { HttpHandler = httpHandler, Credentials = ChannelCredentials.Insecure, MaxReceiveMessageSize = 15 * 1024 * 1024 });
            var client = new Archivador.ArchivadorClient(channel);

            var respuesta = await client.SolicitarDescargarArchivoAsync(new IdArchivo { Id = idCancion});
            return respuesta.Datos.ToArray();
        }
    }
}
