using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient; // SE AÑADE EL using DE CONEXION DE DATOS DE SQLCLIENTE

namespace negocio
{
    public class AccesoDatos
    {
        // VAMOS A CREAR LOS OBJETOS PARA CREAR UNA CONEXION
        // SE CREAN LOS ATRIBUTOS//VARIABLES PARA CENTRALIZAR TODAS LAS CONEXIONES A BD
        // DESDE LAS DISTINTAS CLASES
        private SqlConnection conexion;
        private SqlCommand comando;
        private SqlDataReader lector;

        // COMO lector ES PRIVATE, SE CREA UNA PROPIEDAD PARA PODER SER LEIDA
        public SqlDataReader Lector
        {
            get { return lector; }
        }

        // CREAMOS EL CONTRUCTOR DE AccesoDatos Y LE ASIGNAMOS LAS REFERENCIAS PARA QUE
        // EN EL MOMENTO DE CREARSE EL OBJETO AccesoDatos, YA TENGA DATOS Y CENTRALIZADOS
        public AccesoDatos()
        {
            // SE AÑADE AL CONSTRUCTOR LA CADENA DE CONEXION DE LA BD DE SQL
            conexion = new SqlConnection("server=.\\SQLEXPRESS; database=POKEDEX_DB; integrated security=true");
            comando = new SqlCommand();


        }
            // CREAMOS UNA FUNCION/METODO PARA EL TIPO DE CONSULTA Y LA CONSULTA
        public void setearConsulta (string consulta)
        {
            
            comando.CommandType = System.Data.CommandType.Text;
            comando.CommandText = consulta;

        }
            // PARA LA CONEXION Y LECTURA DE DATO, GENERAMOS UNA FUNCION/METODO DE LECTURA
        public void ejecutarLectura()
        {
            comando.Connection = conexion;
            try
            {
                conexion.Open();
                lector = comando.ExecuteReader();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // PARA REALIZAR UNA CONSULTA SOBRE SQL QUE SEA AGREGAR, MODIFICAR, ELIMINAR UN POKEMON,
        // SE REALIZA SOBRE LA FUNCION ejecutarAccion, YA QUE NO ES UNA LECTURA SINO UNA ACCION
        // SOBRE SQL Y POR ESO SE USA EL EJECUTABLE ExecuteNonQuery()
        public void ejecutarAccion()
        {
            // CREAMON LAS CONEXION
            comando.Connection = conexion;
            try
            {
                conexion.Open(); // ABRIMOS LA CONEXION
                comando.ExecuteNonQuery(); // EJECUTAMOS LA INSERCION DE LOS DATOS EN LA DB
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }


        // Y PARA TERMINAR, HAY QUE CERRAR LA CONEXION CON BD Y EL LECTOR
        public void cerrarConexion()
        {
            if(lector != null) // ES PARA QUE NO HAYA OTRA CONEXION ABIERTA
                lector.Close();
            conexion.Close();
        }

    }
}
