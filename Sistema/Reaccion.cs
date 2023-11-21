namespace Sistema
{
    public class Reaccion: IValidable
    {
        private Miembro _miembro;
        private bool _islike;

        public Reaccion(Miembro unmiembro, bool reaccion)
        {
            _miembro = unmiembro;
            _islike = reaccion;
        }

        /// <summary>
        /// Devuelve la reaccion que hizo el miembro y permite modificarla
        /// </summary>
        public bool Islike { get { return _islike; } set { _islike = value; } }

        public Miembro Miembro { get { return _miembro; } }

        /// <summary>
        /// Valida que la reaccion sea valida.
        /// </summary>
        /// <exception cref="Exception"></exception>
        public void Validar()
        {
            if (_miembro == null)
                throw new Exception("El miembro no puede ser nulo");
        }
        /// <summary>
        /// Verifica que las reacciones sean la misma
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object? obj)
        {
            return obj is Reaccion unareaccion && unareaccion._miembro.Equals(_miembro);

        }

    }
}
