﻿namespace Sistema
{
    public class Comentario: Publicacion
    {
        /// <summary>
        /// Constructor de la clase Comentario
        /// </summary>
        /// <param name="titulo"></param>
        /// <param name="contenido"></param>
        /// <param name="autor"></param>
        public Comentario(string titulo, string contenido, Miembro autor) :base(titulo, contenido, autor) 
        {
        
        }

        /// <summary>
        /// Verifica si el autor y el id de un comentario son iguales
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object? obj)
        {
            return obj is Comentario uncomentario && uncomentario.Autor == Autor && uncomentario.Id == Id;
        }
    }
}
