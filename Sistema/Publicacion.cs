using System.Text.RegularExpressions;

namespace Sistema
{
    public abstract class Publicacion : IValidable, IComparable<Publicacion>
    {
        //atributos
        private int _id;
        private string? _titulo;
        private Miembro? _autor;
        private DateTime _fecha;
        private string? _contenido;               
        private static int s_ultid = 0;
        private List<Reaccion>? _reacciones;
        private int _aceptacion;


        //propiedades
        public int Id { get { return _id; } set { _id = s_ultid; } }
        public string? Titulo { get { return _titulo; } set { _titulo = value; } }
        public Miembro? Autor { get { return _autor; } set { _autor = value; } }
        public string? Contenido { get { return _contenido; } set { _contenido = value; } }
        public DateTime Fecha { get { return _fecha; } set { _fecha = value; } }
        public List<Reaccion>? Reacciones { get { return _reacciones; } set { _reacciones = value; } }

        public Publicacion()
        {
            s_ultid++;
        }

        /// <summary>
        /// Valida que la reaccion hecha por un miembro no se encuentre ya en la lista
        /// </summary>
        /// <param name="unareaccion"></param>
        /// <exception cref="Exception"></exception>
        public void ValidarReaccion(Reaccion unareaccion)
        {
            if (Reacciones.Contains(unareaccion))
                throw new Exception("Usted ya reacciono a este post");
        }

        public void AltaReaccion(Miembro miembro, bool islike)
        {
            Reaccion unareaccion = new Reaccion(miembro,islike);
            unareaccion.Validar();
            ValidarReaccion(unareaccion);
            _reacciones.Add(unareaccion);
        }

        /// <summary>
        /// Constructor de la clase Publicacion
        /// recibe como parametro un titulo, el contenido y el miembro que hace la publicacion
        /// </summary>
        /// <param name="titulo"></param>
        /// <param name="contenido"></param>
        /// <param name="autor"></param>
        public Publicacion(string titulo, string contenido, Miembro autor)
        {
            _titulo = titulo;
            _contenido = contenido;
            _autor = autor;
            _fecha = DateTime.Now;
            s_ultid++;
            _id = s_ultid;            
            _reacciones = new List<Reaccion>();
            _aceptacion = 0;
        }

        

        public bool ExisteTexto(string texto)
        {
            return _contenido.IndexOf(texto) != -1;
        }

        public bool AceptacionMayorA(int numero)
        {
            return _aceptacion > numero;
        }

        /// <summary>
        /// Valida el titulo, el contenido y el autor
        /// </summary>
        public virtual void Validar()
        {
            ValidarTitulo();
            ValidarContenido();
            ValidarAutor();
        }

        /// <summary>
        /// Recorta los espacios del titulo y solo deja los necesario, y verifica que no sea nulo, vacio o que su longitud sea menor que tres
        /// </summary>
        /// <exception cref="Exception"></exception>
        public void ValidarTitulo()
        {            
            if (string.IsNullOrEmpty(_titulo))
                throw new Exception("El titulo no puede ser vacio o menor de tres caracteres");
            _titulo = _titulo.Trim();
            _titulo = Regex.Replace(_titulo, @"\s+", " ");
            int cantletrastitulo = CantPalabras();
            if (cantletrastitulo < 3)
                throw new Exception("El titulo debe ser más largo");
        }

        private int CantPalabras()
        {
            int cantchar = 0;
            foreach(char ch in _titulo)
            {
                if(ch != ' ')
                    cantchar++;
            }
            return cantchar;
        }

        /// <summary>
        /// Verifica que el contenido de la publicacion no sea vacio o nulo, y recorta los espacios en blanco.
        /// </summary>
        /// <exception cref="Exception"></exception>
        public void ValidarContenido()
        {
            if (String.IsNullOrEmpty(Contenido))
                throw new Exception("El contenido no puede ser vacío");
            _contenido = _contenido.Trim();
            _contenido = Regex.Replace(_contenido, @"\s+", " ");
        }

        /// <summary>
        /// Valida que el autor no sea un objeto nulo
        /// </summary>
        /// <exception cref="Exception"></exception>
        public void ValidarAutor()
        {
            if (Autor == null)
                throw new Exception("El autor no puede ser un objeto nulo");
        }

        public string MostrarContenidoRecortado()
        {
            string contenidopost = (string)Contenido.Clone();
            if (contenidopost.Length > 50)
                contenidopost = contenidopost.Substring(0, 50);
            return $"El titulo es:{Titulo},\n" +
                $"La fecha cuando se subio es: {Fecha},\n" +
                $"El id de la publicacion es: {Id},\n" +
                $"El contenido es: {contenidopost},\n";
        }


        /// <summary>
        /// Override de tostring que devuelve datos importantes del post
        /// </summary>
        /// <returns>Datos importantes de la publicacion</returns>
        public override string ToString()
        {
            return $"El titulo es:{Titulo},\n" +
                $"La fecha cuando se subio es: {Fecha},\n" +
                $"El id de la publicacion es: {Id},\n" +
                $"El contenido es: {_contenido},\n";
        }

        public int CompareTo(Publicacion? otrapubli)
        {
            return _titulo.CompareTo(otrapubli._titulo) * -1;     
        }
    }
}
