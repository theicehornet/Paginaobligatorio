namespace Sistema
{
    public abstract class Usuario : IValidable
    {
        private string _email;
        private string _password;

        /// <summary>
        /// Constructor de la clase Usuario recibe como parametro un email y una password
        /// </summary>
        /// <param name="email"></param>
        /// <param name="passowrd"></param>
        public Usuario(string email, string passowrd)
        {
            _email = email.ToLower();
            _password = passowrd;
        }

        /// <summary>
        /// Propiedad que devuelve el email del usuario
        /// </summary>
        public string Email { get { return _email; } }
        public string Password { get { return _password; } }


        /// <summary>
        /// Permite Validar el email, que no sea nulo,vacio, sea de gmail o hotmail y que cuente con un '.com'
        /// </summary>
        /// <exception cref="Exception"></exception>
        public void ValidarEmail()
        {
            char[] caracteresEspeciales = { '!', '@', '#', '$', '%', '|', '¬', '°','&','/','¿','?','=','-','_',',','.',';','\\'};
            if (string.IsNullOrEmpty(_email))
                throw new Exception("Email no puede ser vacio");
            if (_email.Substring(_email.Length-3,3) != ".uy" && _email.Substring(_email.Length - 4, 4) != ".com")
                throw new Exception("El correo no es valido");
            if(_email.IndexOf('@') == -1)
                throw new Exception("El correo no es valido, no tiene '@'");
            if (caracteresEspeciales.Contains(_email[0]))
                throw new Exception("El email no es valido, no puede comenzar con caracteres especiales");
        }

        public virtual void Validar()
        {
            ValidarEmail();
            ValidarPassword();
        }

        /// <summary>
        /// Valida que la password no sea vacia o nula
        /// </summary>
        /// <exception cref="Exception"></exception>
        public void ValidarPassword()
        {
            if (string.IsNullOrEmpty(_password))
                throw new Exception("Contraseña no puede ser vacio");
        }

        /// <summary>
        /// Override Equals, verifica que el email del objeto pasado por parametro sea igual al email del objeto que ha casteado el metodo
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object? obj)
        {
            return obj is Usuario unusuario && unusuario.Email == Email;
        }
        /// <summary>
        /// Metodo polimorifico para que dependiendo del usuario devuelva su rol.
        /// </summary>
        public abstract string Rol();
    }
}
