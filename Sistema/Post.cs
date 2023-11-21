using System.Text.RegularExpressions;

namespace Sistema
{
    public class Post: Publicacion,IValidable
    {
        private string _imagen;
        private List<Comentario> _comentarios;
        private bool _isprivado;
        private bool _iscensurado;

        /// <summary>
        /// Constructor de la clase Post
        /// </summary>
        /// <param name="titulo"></param>
        /// <param name="contenido"></param>
        /// <param name="autor"></param>
        /// <param name="imagen"></param>
        /// <param name="isprivado"></param>
        public Post(string titulo, string contenido, Miembro autor, string imagen, bool isprivado=false) : base(titulo, contenido, autor) 
        {
            _imagen = imagen;
            _comentarios = new List<Comentario>();  
            _isprivado = isprivado;
            _iscensurado = false;
        }

        public override int ValorDeAceptacion()
        {
            int resultado = base.ValorDeAceptacion();
            if (!_isprivado)
            {
                return resultado + 10 ;
            }
            return resultado;
        }

        public Comentario BuscarComentario(int idcom)
        {
            int i = 0;
            Comentario? c = null;
            while (c == null && i < _comentarios.Count)
            {
                if (_comentarios[i].Id == idcom)
                    c = _comentarios[i];
                i++;
            }
            if (c == null)
                throw new Exception("Comentario no encontrado, quizas este en un lugar mejor");
            return c;
        }

        public Post():base()
        {

        }

        public string? Imagen { get { return _imagen; } set { _imagen = value; } }

        public bool IsPrivado { get { return _isprivado; } set { _isprivado = value; } }

        public bool IsCensurado { get { return _iscensurado; } set { _iscensurado = value; } }

        public List<Comentario>? Comentarios { get { return _comentarios; } set { _comentarios = value; } }

        /// <summary>
        /// Crea un comentario, verifica que sus datos sean corrector y si lo son lo agrega a su lista de comentarios
        /// </summary>
        /// <param name="titulo"></param>
        /// <param name="contenido"></param>
        /// <param name="autor"></param>
        /// <exception cref="Exception"></exception>
        public void AltaComentarPost(string titulo, string contenido, Miembro autor)
        {
            if (_iscensurado)
                throw new Exception("El post se encuentra censurado, no se puede comentar");
            Comentario uncomentario = new Comentario(titulo, contenido, autor);
            uncomentario.Validar();
            _comentarios.Add(uncomentario);
        }

        /// <summary>
        /// Dado un miembro busca comentarios que tenga su autoria y devuelve una lista con los comentarios que haya hecho
        /// </summary>
        /// <param name="unmiembro"></param>
        /// <returns>Lista de comentarios</returns>
        public List<Comentario> HallarComentariosdeMiembro(Miembro unmiembro)
        {
            List<Comentario> comentariosdemiembro = new List<Comentario>();
            foreach (Comentario comentario in _comentarios)
            {
                if(comentario.Autor == unmiembro)
                    comentariosdemiembro.Add(comentario);
            }
            return comentariosdemiembro;
        }


        /// <summary>
        /// Usa la validacion de la herencia de la clase publicacion y agrega la validacion de la imagen
        /// </summary>
        public override void Validar()
        {
            base.Validar();
            ValidarImagen();
        }

        /// <summary>
        /// Valida que la imagen sea valida que sus ultimos caracteres sean .jpg o .png y que no sea nula o vacia
        /// </summary>
        /// <exception cref="Exception"></exception>
        public void ValidarImagen()
        {
            if (string.IsNullOrEmpty(_imagen))
                throw new Exception("Debe haber una imagen");
            if (_imagen.Substring(_imagen.Length - 4, 4) != ".jpg" && _imagen.Substring(_imagen.Length - 4, 4) != ".png")
                throw new Exception("La extencion de la imagen no es valida");
        }

        /// <summary>
        /// Dado una lista de comentarios verifica si es que alguno de esos comentario pertenece al post
        /// </summary>
        /// <param name="comentarios"></param>
        /// <returns>true si encontro un comentario en el post,false en caso contrario</returns>
        public bool IsComentariosenPost(List<Comentario> comentarios)
        {    
            int cantitems = comentarios.Count;
            int i = 0;
            bool encontrado = false;
            while (!encontrado && i < comentarios.Count)
            {
                if (IsComentarioenPost(comentarios[i]))
                    encontrado = true;
                i++;
            }
            return encontrado;
        }


        /// <summary>
        /// Verifica si un comentario se encuentra en su lista de comentarios
        /// </summary>
        /// <param name="uncomentario"></param>
        /// <returns>retorna true si un comentario pertenece a la lista de comentario si no false</returns>
        private bool IsComentarioenPost(Comentario uncomentario)
        {
            return _comentarios.Contains(uncomentario);

        }

        /// <summary>
        /// Retorna un comentario random de la lista de comentarios
        /// </summary>
        /// <returns>un comentario</returns>
        public Comentario ComentarioRandom()
        {
            Random random = new Random();
            List<Comentario> comentarios = CopiaListaComentarios();
            int ran = random.Next(comentarios.Count);
            return comentarios[ran];
        }

        /// <summary>
        /// Devuelve una copia de la lista de los comentarios
        /// </summary>
        /// <returns></returns>
        public List<Comentario> CopiaListaComentarios()
        {
            List<Comentario> comentarios = new List<Comentario>();
            foreach(Comentario comentario in _comentarios)
            {
                comentarios.Add(comentario);
            }
            return comentarios;
        }

        public void AgregarComentariosALista(List<Comentario> comentarios)
        {
            foreach (Comentario comentario in _comentarios)
            {
                comentarios.Add(comentario);
            }
        }

        /// <summary>
        /// Override de equals verifica que el autor y el id sean iguales
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object? obj)
        {
            return obj is Post unpost && Autor == unpost.Autor && unpost.Id == Id;
        }        
    }
}
