using System;

namespace Logica.Utilerias
{
    public static class UtileriasDeExcepciones
    {
        public static string ObtenerMensajesDeAggregateException(AggregateException aggregateException)
        {
            string mensaje = string.Empty;
            foreach (Exception excepcion in aggregateException.InnerExceptions)
            {
                if (excepcion.Message == null)
                {
                    mensaje += ObtenerMensajeDeExcepcion(excepcion);
                }
                else
                {
                    mensaje += excepcion.GetType().ToString() + Environment.NewLine;
                    mensaje += excepcion.Message + Environment.NewLine;
                }
            }
            return mensaje;
        }

        private static string ObtenerMensajeDeExcepcion(Exception exception)
        {
            string mensaje = string.Empty;
            if (exception.Message == null)
            {
                if (exception.InnerException != null)
                {
                    ObtenerMensajeDeExcepcion(exception);
                }
            }
            else
            {
                mensaje += exception.GetType().ToString() + Environment.NewLine;
                mensaje += exception.Message + Environment.NewLine;
            }

            return mensaje;
        }
    }
}
