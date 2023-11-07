namespace Sistema
{
    public class Miembro : Usuario
    {
        private string _nombre;
        private string _apellido;
        private DateTime _fechanacimiento;
        private bool _isbloqueado;
        #region constructor

        /// <summary>
        /// Constructor de la clase Miembro
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <param name="nombre"></param>
        /// <param name="apellido"></param>
        /// <param name="fechanacimiento"></param>
        public Miembro(string email, string password, string nombre, string apellido, DateTime fechanacimiento) : base(email, password)
        {           
            _nombre = nombre;
            _apellido = apellido;
            _isbloqueado = false;
            _fechanacimiento = fechanacimiento;
        }
        #endregion

        /// <summary>
        /// Propiedad de la clase Miembro devuelve si esta bloqueado y permite modificarlo
        /// </summary>
        public bool Bloqueado { get { return _isbloqueado; } set { _isbloqueado = value; } }

        /// <summary>
        /// Metodo que retorna el nombre completo del miembro
        /// </summary>
        /// <returns>Nombre + Apellido</returns>
        public string NombreCompleto()
        {
            return _nombre + " " + _apellido;
        }


        #region Validacion
        /// <summary>
        /// Valida que todos los datos del miembro sean validos, password,email,nombre,apellido y la fecha de nacimiento
        /// </summary>
        public override void Validar()
        {
            base.Validar();
            ValidarNombre();
            ValidarApellido();
            ValidarFechaNac();
        }

        /// <summary>
        /// Valida que el nombre no sea nulo o vacio
        /// </summary>
        /// <exception cref="Exception">El nombre no puede estar vacio</exception>
        public void ValidarNombre()
        {
            if (string.IsNullOrEmpty(_nombre))
                throw new Exception("El nombre no puede estar vacio");
        }
        /// <summary>
        /// Valida que el apellido no sea vacio o nulo
        /// </summary>
        /// <exception cref="Exception"></exception>
        public void ValidarApellido()
        {
            if (string.IsNullOrEmpty(_apellido))
                throw new Exception("El apellido no puede estar vacio");
        }

        /// <summary>
        /// Valida que la fecha de nacimiento sea menor a la fecha actual
        /// </summary>
        /// <exception cref="Exception"></exception>
        public void ValidarFechaNac()
        {
            if (_fechanacimiento > DateTime.Now)
                throw new Exception("La fecha no puede ser mayor a la de hoy");
        }
#endregion

        /// <summary>
        /// Override tostring, devuelve datos importante del miembro
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"El nombre completo del usuario es: {_nombre} {_apellido}\n" +
                $"Su fecha de nacimiento es: {_fechanacimiento}\n" +
                $"Su email de contacto es: {Email}\n";
        }

        public override string Rol()
        {
            return "miembro";
        }
    }
}
