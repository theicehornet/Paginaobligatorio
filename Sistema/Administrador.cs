﻿namespace Sistema
{
    public class Administrador : Usuario
    {
        /// <summary>
        /// Constructor de la clase Administrador
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        public Administrador(string email, string password):base(email,password) 
        {
        }
        /// <summary>
        /// Devuelve el rol de un Administrador
        /// </summary>
        /// <returns>El rol</returns>
        public override string Rol()
        {
            return "admin";
        }



    }
}
