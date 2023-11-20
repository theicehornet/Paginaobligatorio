namespace Sistema
{
    public class Solicitud
    {
        private int _id;
        private Miembro _miembrosolicitante;
        private Miembro _miembrosolicitado;
        private Status _estado;
        private static int s_ultid = 0;
        private DateTime _fechasolicitud;

        /// <summary>
        /// Constructor de la clase Solicitud
        /// recibe de parametro un id, un miembro solicitante,
        /// un miembro solicitado y un estado de solicitud
        /// </summary>
        /// <param name="id"></param>
        /// <param name="solicitante"></param>
        /// <param name="solicitado"></param>
        /// <param name="estado"></param>
        public Solicitud(Miembro solicitante, Miembro solicitado, Status estado)
        {
            _id = s_ultid;
            _miembrosolicitado = solicitado;
            _miembrosolicitante = solicitante;
            _estado = estado;
            _fechasolicitud = DateTime.Now;
            s_ultid ++;
        }

        /// <summary>
        /// Retorna el estado de la solicitud y puede ser cambiada
        /// </summary>
        public Status Estado { get { return _estado; } }

        /// <summary>
        /// Retorna la id de la solicitud
        /// </summary>
        public int ID { get { return _id; } }

        /// <summary>
        /// Propiedad de la clase Solicitud, devuelve el miembro que fue solicitado de la invitacion
        /// </summary>
        public Miembro Solicitado { get { return _miembrosolicitado; } }

        public Miembro Solicitante { get { return _miembrosolicitante; } }

        /// <summary>
        /// Acepta una solicitud
        /// </summary>
        public void SolicitudAceptada()
        {
            _estado = (Status)1;
        }

        public bool MiembrosEnRelacion(Miembro solicitante,Miembro solicitado)
        {
            return (_miembrosolicitante.Email == solicitante.Email && _miembrosolicitado.Email == solicitado.Email) || (_miembrosolicitante.Email == solicitado.Email && _miembrosolicitado.Email == solicitante.Email);
        }

        /// <summary>
        /// Rechaza una solicitud
        /// </summary>
        public void SolicitudRechazada()
        {
            _estado = (Status)2;
        }

        /// <summary>
        /// Verifica que el miembro solicitado no sea el solicitante del objeto que ha casteado y viceversa
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object? obj)
        {
            return obj is Solicitud unasoli && ((unasoli.Solicitado == _miembrosolicitante && unasoli._miembrosolicitante == _miembrosolicitado)||
                (unasoli.Solicitado == _miembrosolicitado && unasoli._miembrosolicitante == _miembrosolicitante));
        }
    }
}
