using Sistema;
namespace Obligatoriobms
{
    internal class Program
    {
        static void Main(string[] args)
        {
            RedSocial SocialNetwork = RedSocial.Instancia;
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Menu RedSocial");
                string[] opciones = {
                    "Alta a un Miembro",
                    "Mostrar todas las publicaciones de un miembro",
                    "Mostrar los posts comentados por un miembro",
                    "Listar todos los post entre fechas",
                    "Mostrar miembros con mas publicaciones"
                };

                Console.WriteLine("0) Salir");
                for (int i = 1; i <= opciones.Length; i++)
                {
                    Console.WriteLine(i + ") " + opciones[i - 1]);
                }
                int opcion = Utilidades.PedirNumero();
                switch (opcion)
                {
                    case 0:
                        Console.WriteLine("Saliendo...");
                        return;
                    case 1:
                        AltaMiembro(SocialNetwork);
                        break;
                    case 2:
                        MostrarPostyComentario(SocialNetwork);
                        break;
                    case 3:
                        MostrarPosts(SocialNetwork);
                        break;
                    case 4:
                        MostrarPostsporfechas(SocialNetwork);
                        break;
                    case 5:
                        MostrarMiembrosProActivos(SocialNetwork);
                        break;
                }
                Console.WriteLine("Presione una tecla para continuar");
                Console.ReadKey();
            }
        }
        static void AltaMiembro(RedSocial unaredsocial)
        {
            while (true)
            {
                Console.WriteLine("Presione 0 para salir.");
                string email = Utilidades.PedirEmail();
                if (email == "0")
                {
                    return;
                }
                Console.WriteLine("Ingrese una contraseña");
                string password = Console.ReadLine();
                Console.WriteLine("Ingrese su nombre");
                string nombre = Console.ReadLine();
                Console.WriteLine("Ingrese su apellido");
                string apellido = Console.ReadLine();
                DateTime fechafinal = Utilidades.PedirFecha("Ingrese su fecha de nacimiento: ");
                try
                {
                    unaredsocial.Altamiembro(email, password, nombre, apellido, fechafinal);
                    Console.WriteLine("Se ha dado de alta a la persona");
                    return;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

        }
        static void MostrarPostyComentario(RedSocial unaredsocial)
        {
            while(true)
            {
                Console.Write("Ingrese un email: ");
                string email = Console.ReadLine();
                try
                {
                    Miembro miembrobuscado = unaredsocial.BuscarMiembro(email);
                    List<Post> posts = unaredsocial.BuscarPostsdeMiembro(miembrobuscado);
                    List<Comentario> comentarios = unaredsocial.BuscarComentariosdeMiembro(miembrobuscado);                    
                    if (posts.Count == 0)
                        Utilidades.MostrarPosts(posts);
                    else
                    {
                        Console.WriteLine("Estos son los posts del miembro\n");
                        Utilidades.MostrarPosts(posts);
                    }
                    if (comentarios.Count == 0)
                        Utilidades.MostrarComentarios(comentarios);
                    else
                    {
                        Console.WriteLine("Estos son los comentarios del miembro\n");
                        Utilidades.MostrarComentarios(comentarios);
                    }
                    return;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            
            
        }

        static void MostrarPosts(RedSocial unaredsocial)
        {
            while (true)
            {
                Console.Write("Ingrese un email: ");
                string email = Console.ReadLine();
                Console.WriteLine();
                try
                {
                    Miembro miembrobuscado = unaredsocial.BuscarMiembro(email);
                    List<Comentario> comentariosmiembro = unaredsocial.BuscarComentariosdeMiembro(miembrobuscado);
                    List<Post> postcomentados = unaredsocial.BuscarPostsporComentarios(comentariosmiembro);
                    Utilidades.MostrarPosts(postcomentados);
                    return;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }
        static void MostrarPostsporfechas(RedSocial unaredsocial)
        {
            DateTime primerafecha = DateTime.Parse("13/05/2008");
            DateTime segundafecha = DateTime.Parse("13/05/2005");
            try
            {
                while (segundafecha < primerafecha)
                {
                    Console.WriteLine("Primera fecha");
                    primerafecha = Utilidades.PedirFecha("Fecha de inicio");
                    Console.WriteLine("segunda fecha");
                    segundafecha = Utilidades.PedirFecha("Fecha final");
                }
                List<Post> postfiltrados = Utilidades.BuscarPostsporFechas(primerafecha, segundafecha, unaredsocial);
                Utilidades.MostrarPosts(postfiltrados);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        static void MostrarMiembrosProActivos(RedSocial unaredsocial)
        {
            List<Miembro> listamiembro = unaredsocial.BuscarMiembrosConMuchasPubli();
            Utilidades.MostrarMiembrosConMuchasPublicaciones(listamiembro);
        }
    }
}