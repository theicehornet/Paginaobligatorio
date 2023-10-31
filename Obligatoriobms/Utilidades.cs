using Sistema;

namespace Obligatoriobms
{
    public class Utilidades
    {
        public static int PedirNumero()
        {
            int numero = 0;
            bool exito = false;
            while (!exito)
            {
                Console.Write("Ingrese un número: ");
                exito = int.TryParse(Console.ReadLine(), out numero);
                if (!exito)
                    Console.WriteLine("Ingrese un número entero");
            }
            return numero;
        }

        public static string PedirEmail()
        {
            string email = "";
            bool exito = false;
            while (!exito)
            {
                Console.Write("Ingrese su email: \n");
                email = Console.ReadLine();
                if (ValidarEmail(email))
                    exito = true;
            }
            return email;
        }

        public static bool ValidarEmail(string email)
        {
            if (email.IndexOf(".com") == -1)
                return false;
            if(email.Substring(email.IndexOf('@')+1,3).Contains('.'))
                return false;
            if(email == "0")
                return true;
            return true;
        }

        public static DateTime PedirFecha(string mensaje)
        {
            bool exito = false;
            DateTime fechadevolver = DateTime.Now;
            while (!exito)
            {
                Console.WriteLine($"{mensaje} en formato DD/MM/YYYY");
                string fecha = Console.ReadLine();
                exito = DateTime.TryParse(fecha, out fechadevolver);
                if (!exito)
                    Console.WriteLine("Ingrese una fecha valida");
            }
            return fechadevolver;
        }

        //Muestra en consola todos los post dado una lista de posts
        public static void MostrarPosts(List<Post> posts)
        {
            if(posts.Count == 0)
            {
                Console.WriteLine("No hay registros de posts :/");
                return;
            }
                
            foreach (Post unpost in posts)
            {
                Console.WriteLine(unpost.MostrarContenidoRecortado());
            }
        }

        //Muestra en consola todos los comentarios dado una lista de comentarios
        public static void MostrarComentarios(List<Comentario> comentarios)
        {
            if (comentarios.Count == 0)
            {
                Console.WriteLine("No hay registros de comentarios :/");
                return;
            }
            foreach (Comentario comentario in comentarios)
            {
                Console.WriteLine(comentario.MostrarContenidoRecortado());
            }
        }

        public static List<Post> BuscarPostsporFechas(DateTime fecha1, DateTime fecha2, RedSocial unared)
        {
            return unared.FiltrarPostsporFechas(fecha1,fecha2);
        }

        public static void MostrarMiembros(RedSocial unared)
        {
            foreach(Miembro unmiembro in unared.CopiadeListaMiembros())
                Console.WriteLine(unmiembro);
        }

        public static void MostrarMiembrosConMuchasPublicaciones(List<Miembro> listamiembro)
        {
            if (listamiembro.Count == 0)
            {
                Console.WriteLine("No hay registros");
                return;
            }
            foreach (Miembro unmiembro in listamiembro)
            {
                Console.WriteLine(unmiembro);
            }
        }

    }
}
