using System.ComponentModel;

namespace Sistema
{
    public class RedSocial
    {
        #region singleton
        private static RedSocial _instancia;
        public static RedSocial Instancia
        {
            get
            {
                if (_instancia == null)
                    _instancia = new RedSocial();
                return _instancia;
            }
        }

        //atributos
        private List<Post> _posts;
        private List<Usuario> _usuarios;
        private List<Solicitud> _relaciones;

        /// <summary>
        /// Constructor del sistema
        /// </summary>
        private RedSocial()
        {
            _posts = new List<Post>();
            _usuarios = new List<Usuario>();
            _relaciones = new List<Solicitud>();           
            DatosPreCargados();
        }
        #endregion

        #region Metodos De Posts

        public Post GetPostporId(int id)
        {
            int i = 0;
            Post? devolver = null;
            while (i < _posts.Count && devolver == null)
            {
                if (_posts[i].Id == id)
                {
                    devolver = _posts[i];
                }
                i++;
            }
            if (devolver == null)
                throw new Exception($"No se ha encontrado al Post con id {id}");
            return devolver;
        }

        /// <summary>
        /// Crea un nuevo post, lo valida y lo agrega a la lista de posts
        /// </summary>
        /// <param name="titulo"></param>
        /// <param name="autor"></param>
        /// <param name="contenido"></param>
        /// <param name="imagen"></param>
        /// <param name="isprivado"></param>
        public void AltaPost(string titulo, Miembro autor, string contenido, string imagen, bool isprivado = false)
        {
            if (autor.Bloqueado)
                throw new Exception($"El miembro {autor.NombreCompleto()} se encuentra bloqueado, no puede realizar esta acción");
            Post nuevopost = new Post(titulo, contenido, autor, imagen, isprivado);
            nuevopost.Validar();
            _posts.Add(nuevopost);
        }

        public List<Post> GetPostPublicos()
        {
            List<Post> dev = new List<Post>();
            foreach (Post post in _posts)
            {
                if (!post.IsPrivado)
                    dev.Add(post);
            }
            return dev;
        }
        public List<Post> GetPostPrivados()
        {
            List<Post> dev = new List<Post>();
            foreach (Post post in _posts)
            {
                if (post.IsPrivado)
                    dev.Add(post);
            }
            return dev;
        }

        public List<Post> GetPostVisiblesDeMiembro(Miembro? miembro)
        {
            List<Post> dev = GetPostPublicos();
            foreach (Post post in GetPostPrivados())
            {
                if (IsAmigo(miembro, post.Autor))
                {
                    dev.Add(post);
                }
            }
            return dev;
        }


        /// <summary>
        /// Dado un miembro busca los post de este mismo
        /// </summary>
        /// <param name="miembro"></param>
        /// <returns>Una lista con los posts del miembro</returns>
        public List<Post> BuscarPostsdeMiembro(Miembro miembro)
        {
            List<Post> listadevolver = new List<Post>();
            foreach (Post post in _posts)
            {
                if (post.Autor == miembro)
                    listadevolver.Add(post);
            }
            return listadevolver;
        }

        public List<Post> BuscarPostsPublicosdeMiembro(Miembro miembro)
        {
            List<Post> listadevolver = new List<Post>();
            foreach (Post post in GetPostPublicos())
            {
                if (post.Autor.Equals(miembro))
                    listadevolver.Add(post);
            }
            return listadevolver;
        }

        /// <summary>
        /// Busca posts dado una lista de comentarios
        /// </summary>
        /// <param name="comentarios"></param>
        /// <returns>Una lista de posts que cuentan con al menos un comentario de la lista</returns>
        public List<Post> BuscarPostsporComentarios(List<Comentario> comentarios)
        {
            List<Post> listpost = new List<Post>();
            foreach (Post unpost in _posts)
            {
                if (unpost.IsComentariosenPost(comentarios))
                    listpost.Add(unpost);
            }
            return listpost;
        }

        /// <summary>
        /// Permite hacer una reaccion a un post y verifica que la reaccion sea valida
        /// </summary>
        /// <param name="miembro"></param>
        /// <param name="reaccion"></param>
        /// <param name="post"></param>
        public void RealizarReaccionPost(Miembro miembro, bool reaccion, Post post)
        {
            post.AltaReaccion(miembro, reaccion);
        }

        public List<Post> BuscarPostsPorTextoyAceptacion(string texto, int aceptacion, Miembro unm)
        {
            List<Post> dev = new List<Post>();
            List<Post> analizar = null;
            if (unm == null)
                analizar = GetPostPublicos();
            else
                analizar = GetPostVisiblesDeMiembro(unm);
            foreach (Post p in analizar)
            {
                int valoracep = p.ValorDeAceptacion();
                if (valoracep > aceptacion && p.ExisteTexto(texto))
                    dev.Add(p);
            }
            return dev;
        }

        /// <summary>
        /// Filtra posts entre una fecha inicial y una final
        /// </summary>
        /// <param name="fechainicial"></param>
        /// <param name="fechafinal"></param>
        /// <returns>Una lista con los posts que cumplen con la condicion</returns>
        public List<Post> FiltrarPostsporFechas(DateTime fechainicial, DateTime fechafinal)
        {
            List<Post> listdevolver = new List<Post>();
            foreach (Post unpost in _posts)
            {
                if (unpost.Fecha <= fechafinal && unpost.Fecha >= fechainicial)
                    listdevolver.Add(unpost);
            }
            listdevolver.Sort();
            return listdevolver;
        }

        public List<Post> CopiadePosts()
        {
            List<Post> listadevolver = new List<Post>();
            foreach (Post post in _posts)
            {
                listadevolver.Add(post);
            }
            return listadevolver;
        }

        /// <summary>
        /// Dado un usuario que sea administrado se censurara un post.
        /// </summary>
        /// <param name="unpost"></param>
        /// <param name="usuario"></param>
        public void CensurarPost(Post unpost, Administrador usuario)
        {
            if (usuario is Administrador)
                unpost.IsCensurado = true;
        }

        public Post BuscarPostPorID(int id)
        {
            int i = 0;
            Post p = null;
            while (i < _posts.Count && p == null)
            {
                if (_posts[i].Id == id)
                    p = _posts[i];
                i++;
            }
            if (p == null)
                throw new Exception("No se ha encontrado el post, tal vez se fue volando.");
            return p;
        }

        #endregion

        #region Metodos de Comentarios

        public List<Comentario> BuscarComentarioPorTextoyAceptacion(string texto, int aceptacion, Miembro unm)
        {
            List<Post> posts = GetPostVisiblesDeMiembro(unm);
            List<Comentario> dev = new List<Comentario>();
            foreach (Comentario c in ComentariosDePosts(posts))
            {
                int valoracep = c.ValorDeAceptacion();
                if (valoracep > aceptacion && c.ExisteTexto(texto))
                    dev.Add(c);
            }
            return dev;
        }
        private List<Comentario> ComentariosDePosts(List<Post> posts)
        {
            List<Comentario> dev = new List<Comentario>();
            foreach (Post post in posts)
            {
                dev.AddRange(post.Comentarios);
            }
            return dev;
        }

        public List<Comentario> GetComentarios()
        {
            List<Comentario> dev = new List<Comentario>();
            foreach (Post p in _posts)
            {
                p.AgregarComentariosALista(dev);
            }
            return dev;
        }

        /// <summary>
        /// Realiza un comentario dado el post pasado por parametro
        /// </summary>
        /// <param name="post"></param>
        /// <param name="titulo"></param>
        /// <param name="comentario"></param>
        /// <param name="autor"></param>
        public void RealizarComentarioaPost(Post post, string titulo, string contenido, Miembro autor)
        {
            post.AltaComentarPost(titulo, contenido, autor);
        }

        /// <summary>
        /// Permite reaccionar a un comentario y verifica que la reaccion sea valida
        /// </summary>
        /// <param name="miembro"></param>
        /// <param name="reaccion"></param>
        /// <param name="comentario"></param>
        public void RealizarReaccionComentario(Miembro miembro, bool reaccion, Comentario comentario)
        {
            comentario.AltaReaccion(miembro, reaccion);
        }

        /// <summary>
        /// Dado un miembro busca todos los comentarios que haya hecho
        /// </summary>
        /// <param name="autor"></param>
        /// <returns>Retorna una lista de comentarios</returns>
        public List<Comentario> BuscarComentariosdeMiembro(Miembro autor)
        {
            List<Comentario> comentariosdevolver = new List<Comentario>();
            foreach (Post post in _posts)
            {
                List<Comentario> comentariosdemiembro = post.HallarComentariosdeMiembro(autor);
                foreach (Comentario comentario in comentariosdemiembro)
                    comentariosdevolver.Add(comentario);

            }
            return comentariosdevolver;
        }
        #endregion

        #region Metodos de Usuarios

        public Usuario AuthenticateUsuario(string email, string password)
        {
            int i = 0;
            while (i < _usuarios.Count)
            {
                Usuario user = _usuarios[i];
                if (user.Email == email)
                {
                    if (user.Password != password)
                        throw new Exception("La contraseña no es correcta!");
                    return user;
                }
                i++;
            }
            throw new Exception("No existe ningun usuario con ese email");
        }

        /// <summary>
        /// Da de alta a un administrador y verifica que no se encuentre registrado en la lista de usuarios
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <exception cref="Exception"></exception>
        public void AltaAdministrador(string email, string password)
        {
            Administrador unadmin = new Administrador(email, password);
            unadmin.Validar();
            if (_usuarios.Contains(unadmin))
            {
                throw new Exception("El correo ya se encuentra registrado, no pueden haber dos administradores con el mismo email");
            }
            _usuarios.Add(unadmin);
        }

        /// <summary>
        /// Da de alta a un miembro
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <param name="nombre"></param>
        /// <param name="apellido"></param>
        /// <param name="fechanacimiento"></param>
        /// <exception cref="Exception"></exception>
        public void Altamiembro(string email, string password, string nombre,
            string apellido, DateTime fechanacimiento)
        {
            email = email.ToLower();
            Miembro unmiembro = new Miembro(email, password, nombre, apellido, fechanacimiento);
            unmiembro.Validar();
            if (_usuarios.Contains(unmiembro))
            {
                throw new Exception("El usuario ya se encuentra registrado");
            }
            _usuarios.Add(unmiembro);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>Una lista de tipo Miembro solo con las personas con màs publicaciones(post y comentario)</returns>
        public List<Miembro> BuscarMiembrosConMuchasPubli()
        {
            List<Miembro> miembrosdevolver = new List<Miembro>();
            int maxpubli = 0;
            foreach (Miembro unmiembro in CopiadeListaMiembros())
            {
                int cantpost = BuscarPostsdeMiembro(unmiembro).Count;
                int cantcomentario = BuscarComentariosdeMiembro(unmiembro).Count;
                if (maxpubli < cantcomentario + cantpost)
                {
                    miembrosdevolver.Clear();
                    maxpubli = cantcomentario + cantpost;
                    miembrosdevolver.Add(unmiembro);
                }
                else if (maxpubli == cantcomentario + cantpost)
                {
                    miembrosdevolver.Add(unmiembro);
                }
            }
            if (maxpubli == 0)
                miembrosdevolver.Clear();
            return miembrosdevolver;
        }


        /// <summary>
        /// Dado un miembro y un usuario que sea administrado se bloqueara al miembro.
        /// </summary>
        /// <param name="unusuario"></param>
        /// <param name="miembro"></param>
        /// <exception cref="Exception"></exception>
        public void BloquearMiembro(Usuario unusuario, Miembro miembro)
        {
            if (unusuario is Administrador)
                miembro.Bloqueado = true;
        }

        /// <summary>
        /// Busca a un miembro dado un email
        /// </summary>
        /// <param name="email"></param>
        /// <returns>Retorna un miembro</returns>
        /// <exception cref="Exception"></exception>
        public Miembro BuscarMiembro(string email)
        {
            int i = 0;
            Miembro miembrobuscado = null;
            while (i < _usuarios.Count && miembrobuscado == null)
            {
                if (_usuarios[i] is Miembro unmiembro && unmiembro.Email == email)
                    miembrobuscado = unmiembro;
                i++;
            }
            return miembrobuscado;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>Una copia de la lista miembro</returns>
        public List<Miembro> CopiadeListaMiembros()
        {
            List<Miembro> listadevolver = new List<Miembro>();
            foreach (Usuario usuario in _usuarios)
            {
                if (usuario is Miembro)
                    listadevolver.Add((Miembro)usuario);
            }
            return listadevolver;
        }

        public List<Miembro> GetMiembrosPorNombre(string nombre, string apellido)
        {
            List<Miembro> listadevolver = new List<Miembro>();
            foreach (Miembro miembro in CopiadeListaMiembros())
            {
                if (miembro.Nombre.ToLower() == nombre.ToLower() || miembro.Apellido.ToLower() == apellido.ToLower())
                    listadevolver.Add(miembro);
            }
            return listadevolver;
        }

        #endregion

        #region Metodos de Solicitud

        public bool IsAmigo(Miembro? miembro1, Miembro? miembro2)
        {
            int i = 0;
            Solicitud encontrado = null;
            while (i < _relaciones.Count && encontrado == null)
            {
                Solicitud soli = _relaciones[i];
                if ((soli.Solicitado == miembro1 && soli.Solicitante == miembro2) || (soli.Solicitado == miembro2 && soli.Solicitante == miembro1))
                {
                    encontrado = soli;
                }
                i++;
            }
            if (encontrado != null && encontrado.Estado == (Status)1)
                return true;
            return false;
        }

        /// <summary>
        /// Permite hacer una relacion entre dos miembros y
        /// verifica que ninguno de los dos este bloqueado,  
        /// que el solicitante no sea igual al solicitado y
        /// que la nueva relacion no haya sido creada antes 
        /// y la agrega a la lista de relaciones
        /// </summary>
        /// <param name="id"></param>
        /// <param name="solicitante"></param>
        /// <param name="solicitado"></param>
        /// <exception cref="Exception"></exception>
        public void AltaRelacion(Miembro solicitante, Miembro solicitado)
        {
            if (solicitante.Bloqueado)
                throw new Exception("Usted se encuentra bloqueado, no puede enviar solicitudes");
            if (solicitado.Bloqueado)
                throw new Exception("el usuario se encuentra bloqueado, no puede recibir solicitudes");
            if (!(solicitante == solicitado))
            {
                Solicitud nuevarelacion = new Solicitud(solicitante, solicitado, (Status)3);
                if (!_relaciones.Contains(nuevarelacion))
                    _relaciones.Add(nuevarelacion);
            }
        }

        public List<Miembro> GetAmigos(Miembro miembro)
        {
            List<Miembro> dev = new List<Miembro>();
            foreach (Solicitud soli in _relaciones)
            {
                if (soli.Estado == (Status)1)
                {
                    if (soli.Solicitado == miembro)
                    {
                        dev.Add(soli.Solicitante);
                    }
                    else if (soli.Solicitante == miembro)
                    {
                        dev.Add(soli.Solicitado);
                    }
                }
            }
            return dev;
        }

        /// <summary>
        /// Busca una relacion dada el id de la solicitud
        /// </summary>
        /// <param name="idrelacion"></param>
        /// <returns>una solicitud que coincida con el id</returns>
        /// <exception cref="Exception"></exception>
        private Solicitud BuscarSolicitudporId(int idrelacion)
        {
            Solicitud solicitud = null;
            int i = 0;
            while (i < _relaciones.Count && solicitud == null)
            {
                if (_relaciones[i].ID == idrelacion)
                    solicitud = _relaciones[i];
                i++;
            }
            if (solicitud == null)
                throw new Exception("Solicitud no encontrada");
            return solicitud;
        }



        /// <summary>
        /// Dado un miembro busca las relaciones en las que el miembro sea el solicitado y que no haya aceptado aún
        /// </summary>
        /// <param name="solicitado"></param>
        /// <returns>retorna una lista con las solicitudes correspondientes</returns>
        public List<Solicitud> BuscarSolicitudesporMiembro(Miembro solicitado)
        {
            List<Solicitud> solicituds = new List<Solicitud>();
            foreach (Solicitud soli in _relaciones)
            {
                if (soli.Solicitado == solicitado)
                {
                    if (soli.Estado == (Status)3)
                        solicituds.Add(soli);
                }

            }
            return solicituds;
        }

        /// <summary>
        /// Dado un miembro busca todas las solicitudes que este miembro ha enviado
        /// </summary>
        /// <param name="solicitado"></param>
        /// <returns>Una lista con todas las solicitudes enviadas del miembro</returns>
        public List<Solicitud> GetTodasLasSolicitudesDe(Miembro solicitado)
        {
            List<Solicitud> solicituds = new List<Solicitud>();
            foreach (Solicitud soli in _relaciones)
            {
                if (soli.Solicitante == solicitado)
                {
                    if (soli.Estado == (Status)3)
                        solicituds.Add(soli);
                }
            }
            return solicituds;
        }




        /// <summary>
        /// Dado un id de solicitud y un miembro que es el solicitado y si este no esta bloqueado acepta una relacion
        /// </summary>
        /// <param name="idrelacion"></param>
        /// <param name="solicitado"></param>
        /// <exception cref="Exception"></exception>
        public void AceptarRelacion(Solicitud solicitudbuscada, Miembro solicitado)
        {
            if (solicitado.Bloqueado)
                throw new Exception("Miembro bloqueado no puedes aceptar ni rechazar solicitudes");
            if (!(solicitudbuscada.Solicitado == solicitado))
                throw new Exception("Usted no es el miembro solicitado no puede aceptar la solicitud");
            solicitudbuscada.SolicitudAceptada();
        }
        /// <summary>
        /// Dado un id de solicitud y un miembro que es el solicitado y si este no esta bloqueado rechaza una relacion
        /// </summary>
        /// <param name="idrelacion"></param>
        /// <param name="solicitado"></param>
        /// <exception cref="Exception"></exception>
        public void RechazarRelacion(Solicitud solicitudbuscada, Miembro solicitado)
        {
            if (solicitado.Bloqueado)
                throw new Exception("Miembro bloqueado no puedes aceptar ni rechazar solicitudes");
            if (!(solicitudbuscada.Solicitado == solicitado))
                throw new Exception("Usted no es el miembro solicitado no puede aceptar la solicitud");
            solicitudbuscada.SolicitudRechazada();
        }

        #endregion

        #region precarga
        /// <summary>
        /// Precarga de datos,miembros,administrador,posts,comentarios,solicitudes,relaciones aceptadas y reacciones
        /// </summary>
        private void DatosPreCargados()
        {
            MiembrosPrecargados();
            AdminPrecargado();
            PostPrecargado();
            ComentariosPrecargados();
            SolicitudesPrecargadas();
            RelacionesPrecargadas();
            ReaccionesPrecargadas();
        }


        /// <summary>
        /// Precargando miembros
        /// </summary>
        private void MiembrosPrecargados()
        {
            DateTime fecha = DateTime.Parse("13/05/2005");
            Altamiembro("fakeemail1@gmail.com", "P@ssw0rd123", "Juan", "Pérez", fecha);
            Altamiembro("testuser2@gmail.com", "SecurePwd!456", "María", "Rodríguez", fecha);
            Altamiembro("notreal3@gmail.com", "Random#Pwd789", "Luis", "González", fecha);
            Altamiembro("dummyemail4@gmail.com", "Ch0c0late&Banana", "Ana", "Martínez", fecha);
            Altamiembro("tempemail5@gmail.com", "2MuchC0ffee$!", "Pedro", "Sánchez", fecha);
            Altamiembro("bogusaddress6@gmail.com", "Bl@ckC@t$42", "Laura", "López", fecha);
            Altamiembro("fictitious7@gmail.com", "G3tUp&Go!", "Carlos", "Fernández", fecha);
            Altamiembro("pretendemail8@gmail.com", "Sunsh1ne#Day", "Isabel", "Torres", fecha);
            Altamiembro("madeup9@gmail.com", "Fr33B1rd$Fly", "Pablo", "Ramírez", fecha);
            Altamiembro("phantomemail10@gmail.com", "5t@rG@zer*", "Luis", "García", fecha);
            Altamiembro("phantomemail11@gmail.com", "5t@rG@zer*", "Luis", "Sánchez", fecha);
            Altamiembro("phantomemail12@gmail.com", "5t@rG@zer*", "Luis", "Torres", fecha);
            Altamiembro("phantomemail13@gmail.com", "5t@rG@zer*", "Luis", "Ramírez", fecha);
        }
        /// <summary>
        /// Precargando administrador
        /// </summary>
        private void AdminPrecargado()
        {
            AltaAdministrador("Alberto123@gmail.com", "Albertiño1234!");
            AltaAdministrador("ana@gmail.com", "1234");
        }

        /// <summary>
        /// PRECARGA LOS POST A LOS MIEMBROS PRECARGARDOS
        /// </summary>
        private void PostPrecargado()
        {
            
            List<Miembro> miembros = CopiadeListaMiembros();
            foreach(Miembro unmiembro in miembros)
            {
                int i = 0;
                AltaPost($"  titulo   {i}  hecho por: {unmiembro.NombreCompleto()}", unmiembro, $"   post   numero {i}  ", "ola.jpg");
                i++;
                AltaPost($"titulo {i} hecho por: {unmiembro.NombreCompleto()}", unmiembro, $"post numero {i}", "ola.jpg");
                i++;
                AltaPost($"titulo {i} hecho por: {unmiembro.NombreCompleto()}", unmiembro, $"post numero {i}", "ola.png");
                i++;
                AltaPost($"titulo {i} hecho por: {unmiembro.NombreCompleto()}", unmiembro, $"post numero {i}", "ola.jpg");
                i++;
                AltaPost($"titulo {i} hecho por: {unmiembro.NombreCompleto()}", unmiembro, $"post numero {i}", "ola.png");
            }
        }

        /// <summary>
        /// Escoge miembros de manera aleatoria
        /// </summary>
        /// <returns>Retorna una lista de tres miembros randoms distintos</returns>
        private List<Miembro> MiembrosRandom()
        {
            List<Miembro> miembrosrandom = new List<Miembro>();
            Random random = new Random();
            List<Miembro> miembrossistema = CopiadeListaMiembros();
            while (miembrosrandom.Count <= 3)
            {
                int randomsito = random.Next(miembrossistema.Count);
                if (!miembrosrandom.Contains(miembrossistema[randomsito]))
                    miembrosrandom.Add(miembrossistema[randomsito]);
            }
            return miembrosrandom;
        }
        /// <summary>
        /// Dado un miembro envia una solicitud a todos los miembros
        /// </summary>
        /// <param name="miembro"></param>
        private void EnviarSolicitudaTodos(Miembro miembro)
        {
            foreach(Miembro unmiembro in CopiadeListaMiembros())
            {
                AltaRelacion(miembro, unmiembro);
            }
        }
        /// <summary>
        /// Por cada miembro envia una solicitud a todos los miembros
        /// </summary>
        private void SolicitudesPrecargadas()
        {
            foreach(Miembro miembro in CopiadeListaMiembros())
            {
                EnviarSolicitudaTodos(miembro);
            }
        }

        /// <summary>
        /// Precarga las relaciones que tendran los miembros precargados con otros
        /// </summary>
        private void RelacionesPrecargadas()
        {
            List<Miembro> miembros = CopiadeListaMiembros();
            List<Solicitud> solicitudesm1 = BuscarSolicitudesporMiembro(miembros[miembros.Count - 1]);
            List<Solicitud> solicitudesm2 = BuscarSolicitudesporMiembro(miembros[miembros.Count - 2]);
            AceptarSolicitudes(solicitudesm1, miembros[miembros.Count - 1]);
            AceptarSolicitudes(solicitudesm2, miembros[miembros.Count - 2]);
            int i = miembros.Count - 3;
            bool shouldcontinue = true;
            while (i >= 0 && shouldcontinue)
            {
                List<Solicitud> todassolicitudes = BuscarSolicitudesporMiembro(miembros[i]);
                if (todassolicitudes.Count == 1)
                {
                    RechazarRelacion(todassolicitudes[0], miembros[i]);
                    shouldcontinue = false;
                }
                else
                {
                    AceptarRelacion(todassolicitudes[0], miembros[i]);
                    RechazarRelacion(todassolicitudes[1], miembros[i]);
                }                   
                i--;
            }
        }

        /// <summary>
        /// Acepta todas las solicitudes dado una lista de solicitudes
        /// </summary>
        /// <param name="solicitudes"></param>
        /// <param name="unmiembro"></param>
        private void AceptarSolicitudes(List<Solicitud> solicitudes,Miembro unmiembro)
        {
            foreach (Solicitud soli in solicitudes)
            {
                AceptarRelacion(soli, unmiembro);
            }
        }


        /// <summary>
        /// Precarga comentarios a posts
        /// </summary>
        private void ComentariosPrecargados()
        {
            foreach (Post post in _posts)
            {
                List<Miembro> miembrosrandom = MiembrosRandom();
                Miembro miembrorandom1 = miembrosrandom[0];
                Miembro miembrorandom2 = miembrosrandom[1];
                Miembro miembrorandom3 = miembrosrandom[2];
                RealizarComentarioaPost(post, $"Comentario hecho por: {miembrorandom1.NombreCompleto()}", $"Comentario del post: {post.Id}", miembrorandom1);
                RealizarComentarioaPost(post, $"Comentario hecho por: {miembrorandom2.NombreCompleto()}", $"Comentario del post: {post.Id}", miembrorandom2);
                RealizarComentarioaPost(post, $"Comentario hecho por: {miembrorandom3.NombreCompleto()}", $"Comentario del post: {post.Id}", miembrorandom3);
            }
        }

        /// <summary>
        /// Precarga las reacciones a post y a comentarios
        /// </summary>
        private void ReaccionesPrecargadas()
        {
            int i = 0;
            foreach(Post post in _posts)
            {
                List<Miembro> miembrosrandom = MiembrosRandom();
                Miembro miembrorandom1 = miembrosrandom[0];
                Miembro miembrorandom2 = miembrosrandom[2];
                RealizarReaccionPost(miembrorandom1, true, post);
                RealizarReaccionPost(miembrorandom2, false, post);
                if(i % 3 == 0)
                {
                    Comentario comentariorandom = post.ComentarioRandom();
                   RealizarReaccionComentario(miembrorandom1, true, comentariorandom);
                   RealizarReaccionComentario(miembrorandom2, false, comentariorandom);
                }
                i++;
            }            
        }
        #endregion
    }
}