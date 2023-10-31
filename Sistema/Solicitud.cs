namespace Sistema
{
    public class Solicitud
    {
        private int _id;
        private Miembro _miembrosolicitante;
        private Miembro _miembrosolicitado;
        private Status _estado;
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
        public Solicitud(int id, Miembro solicitante, Miembro solicitado, Status estado)
        {
            _id = id;
            _miembrosolicitado = solicitado;
            _miembrosolicitante = solicitante;
            _estado = estado;
            _fechasolicitud = DateTime.Now;
        }

        /// <summary>
        /// Retorna el estado de la solicitud y puede ser cambiada
        /// </summary>
        public Status Estado { get { return _estado; } }
        /// <summary>
        /// Acepta una solicitud
        /// </summary>
        public void SolicitudAceptada()
        {
            _estado = (Status)1;
        }
        /// <summary>
        /// Rechaza una solicitud
        /// </summary>
        public void SolicitudRechazada()
        {
            _estado = (Status)2;
        }

        /// <summary>
        /// Retorna la id de la solicitud
        /// </summary>
        public int ID { get { return _id; } }

        /// <summary>
        /// Propiedad de la clase Solicitud, devuelve el miembro que fue solicitado de la invitacion
        /// </summary>
        public Miembro Solicitado { get{ return _miembrosolicitado; } }

        /// <summary>
        /// Verifica que el miembro solicitado no sea el solicitante del objeto que ha casteado y viceversa
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object? obj)
        {
            return obj is Solicitud unasoli && unasoli.Solicitado == _miembrosolicitante && unasoli._miembrosolicitante == _miembrosolicitado;
        }
    }
}
