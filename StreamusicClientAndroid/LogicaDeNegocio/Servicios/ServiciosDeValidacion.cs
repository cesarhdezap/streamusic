using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Logica.Servicios
{
    public class ServiciosDeValidacion
    {
        /// <summary>
        /// Expresión regular que valida que la cadena sea un entero de 10 de longitud
        /// que puede utilizar paréntesis para los primeros tres digitos.
        /// </summary>
        private static readonly Regex RegexTelefono = new Regex(@"^(1\s*[-\/\.]?)?(\((\d{3})\)|(\d{3}))\s*[-\/\.]?\s*(\d{3})\s*[-\/\.]?\s*(\d{4})\s*(([xX]|[eE][xX][tT])\.?\s*(\d+))*$");
        /// <summary>
        /// Expresión regular que valida que la cadena tenga al menos una letra seguida
        /// de un arroba, al menos otras dos letras, un punto y al menos otras dos letras.
        /// </summary>
        private static readonly Regex RegexCorreoElectronico = new Regex(@"^(\D)+(\w)*((\.(\w)+)?)+@(\D)+(\w)*((\.(\D)+(\w)*)+)?(\.)[a-z]{2,}$");
        /// <summary>
        /// Expresión regular que valida que la cadena sean solo letras y letras modificadas.
        /// </summary>
        private static readonly Regex RegexNombre = new Regex(@"^[a-zA-Z àáâäãåąčćęèéêëėįìíîïłńòóôöõøùúûüųūÿýżźñçčšžÀÁÂÄÃÅĄĆČĖĘÈÉÊËÌÍÎÏĮŁŃÒÓÔÖÕØÙÚÛÜŲŪŸÝŻŹÑßÇŒÆČŠŽ∂ð,.'-]+$");
        /// <summary>
        /// Expresión regular que valida que la cadena no tenga espacios en blanco y sea de 6 a 255 de longitud.
        /// </summary>
        private static readonly Regex RegexContraseña = new Regex(@"^\S{6,255}$");
        private static readonly Regex RegexNuevaEntradaACamposDeSoloNumerosDecimales = new Regex(@"^[0-9]$|^\.$");
        private static readonly Regex RegexNuevaEntradaACampoDeSoloNumeros = new Regex(@"^[0-9]$");
        private static readonly Regex RegexNumeroDecimal = new Regex(@"^([0-9]*\.)?[0-9]+$");

        public const int TAMAÑO_MAXIMO_VARCHAR = 255;
        public const int VALOR_ENTERO_MINIMO_PERMITIDO = 0;
        private const int VALOR_ENTERO_MAXIMO_PERMITIDO = 255;
        private const int VALOR_AÑO_MAXIMO_PERMITIDO = 9999;

        /// <summary>
        /// Valida la estructura de la cadena del correo del usuario.
        /// </summary>
        /// <param name="correoElectronico">Correo del usuario.</param>
        /// <returns>Si la cadena cumple con la validación.</returns>
		public static bool ValidarCorreoElectronico(string correoElectronico)
        {
            bool resultadoDeValidacion = false;

            if (correoElectronico.Length <= TAMAÑO_MAXIMO_VARCHAR)
            {
                if (RegexCorreoElectronico.IsMatch(correoElectronico))
                {
                    resultadoDeValidacion = true;
                }
            }

            return resultadoDeValidacion;
        }

        /// <summary>
        /// Valida la estructura de la cadena del telefono del usuario.
        /// </summary>
        /// <param name="telefono">Telefono del usuario.</param>
        /// <returns>Si la cadena cumple con la validación.</returns>
		public static bool ValidarTelefono(string telefono)
        {
            bool resultadoDeValidacion = false;

            if (RegexTelefono.IsMatch(telefono))
            {
                resultadoDeValidacion = true;
            }

            return resultadoDeValidacion;
        }

        public static bool ValidarNumeroDecimal(string numero)
        {
            bool resultadoDeValidacion = false;

            if (RegexNumeroDecimal.IsMatch(numero))
            {
                resultadoDeValidacion = true;
            }

            return resultadoDeValidacion;
        }

        public static bool ValidarEntradaDeDatosSoloDecimal(string cadenaNumerica)
        {
            bool resultadoDeValidacion = false;

            if (RegexNuevaEntradaACamposDeSoloNumerosDecimales.IsMatch(cadenaNumerica))
            {
                resultadoDeValidacion = true;
            }

            return resultadoDeValidacion;
        }

        public static bool ValidarEntradaDeDatosSoloEntero(string cadenaNumerica)
        {
            bool resultadoDeValidacion = false;

            if (RegexNuevaEntradaACampoDeSoloNumeros.IsMatch(cadenaNumerica))
            {
                resultadoDeValidacion = true;
            }

            return resultadoDeValidacion;
        }

        /// <summary>
        /// Valida la estructura de la cadena del nombre del usuario.
        /// </summary>
        /// <param name="nombre">Nombre del usuario.</param>
        /// <returns>Si la cadena cumple con la validación.</returns>
		public static bool ValidarNombre(string nombre)
        {
            bool resultadoDeValidacion = false;

            if (nombre.Length <= TAMAÑO_MAXIMO_VARCHAR)
            {
                if (RegexNombre.IsMatch(nombre))
                {
                    resultadoDeValidacion = true;
                }
            }

            return resultadoDeValidacion;
        }

        /// <summary>
        /// Valida la estructura de la cadena de la contraseña del usuario.
        /// </summary>
        /// <param name="contraseña">Contraseña del usuario.</param>
        /// <returns>Si la cadena cumple con la validación.</returns>
        public static bool ValidarContraseña(string contraseña)
        {
            bool resultadoDeValidacion = false;

            if (RegexContraseña.IsMatch(contraseña))
            {
                resultadoDeValidacion = true;
            }

            return resultadoDeValidacion;
        }

        public static bool ValidarCadenaVacioPermitido(string cadena)
        {
            bool resultadoDeValidacion = false;

            if (!string.IsNullOrWhiteSpace(cadena) && cadena.Length <= TAMAÑO_MAXIMO_VARCHAR)
            {
                resultadoDeValidacion = true;
            }

            return resultadoDeValidacion;
        }

        /// <summary>
        /// Valida una cadena para la entrada a la base de datos.
        /// </summary>
        /// <param name="cadena">Cadena de carácteres.</param>
        /// <returns>Si la cadena cumple con la validación.</returns>
        public static bool ValidarCadena(string cadena)
        {
            bool resultadoDeValidacion = false;

            if (!string.IsNullOrEmpty(cadena) && cadena.Length <= TAMAÑO_MAXIMO_VARCHAR)
            {
                resultadoDeValidacion = true;
            }

            return resultadoDeValidacion;
        }

        /// <summary>
        /// Valida si la cadena es convertible a entero y tiene la estructura para insertar a la base de datos.
        /// </summary>
        /// <param name="numeroEntero">Cadena con un numero.</param>
        /// <returns>Si la cadena es convertiblea entero.</returns>
        public static bool ValidarEntero(string numeroEntero)
        {
            bool resultadoDeValidacion = false;

            if (int.TryParse(numeroEntero, out int numeroConvertido) && numeroConvertido > VALOR_ENTERO_MINIMO_PERMITIDO && numeroConvertido <= VALOR_ENTERO_MAXIMO_PERMITIDO)
            {
                resultadoDeValidacion = true;
            }

            return resultadoDeValidacion;
        }

        public static bool ValidarAño(string año)
        {
            bool resultadoDeValidacion = false;

            if (int.TryParse(año, out int numeroConvertido) && numeroConvertido > VALOR_ENTERO_MINIMO_PERMITIDO && numeroConvertido <= VALOR_AÑO_MAXIMO_PERMITIDO)
            {
                resultadoDeValidacion = true;
            }

            return resultadoDeValidacion;
        }
    }
}
