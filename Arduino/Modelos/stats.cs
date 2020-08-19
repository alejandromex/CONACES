using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data.Sql;
using System.Data;
using System.Windows.Forms;

namespace Arduino.Modelos
{
    class stats
    {
        private DataTable dt = new DataTable();
        private SqlCommand cmd = new SqlCommand();
        private Helpers.dbConnect ejecutar = new Helpers.dbConnect();


        private string nombre;
        private double medicion;
        private double latitud;
        private double longitud;

        public string getNombre()
        {
            return this.nombre;
        }

        public void setNombre(string nombre)
        {
            this.nombre = nombre;
        }

        public double getMedicion()
        {
            return this.medicion;
        }

        public void setMedicion(double medicion)
        {
            this.medicion = medicion;
        }

        public double getLatitud()
        {
            return this.latitud;
        }

        public void setLatitud(double latitud)
        {
            this.latitud = latitud;
        }

        public double getLongitud()
        {
            return this.longitud;
        }

        public void setLongitud(double longitud)
        {
            this.longitud = longitud;
        }
        
        public DataTable getAllStats()
        {

               string query = @"select * from stats";
               cmd.CommandText = query;
               dt = ejecutar.Ejecutar(cmd);
               return dt; 

        }

        public DataTable getAllStatsWithName(string nombre, string latitudInicia, string latitudFinal, string longitudInicial, string longitudFinal, string alturaInicial, string alturaFinal, string medicionInicial, string medicionFinal)
        {
            string result = @"select * from stats where nombre = @nombre and latitud >= @latitudMin and latitud <= @latitudMax and longitud >= @longitudMin and longitud <= @longitudMax and altura >= @alturaMin and altura <= @alturaMax and medicion >= @medicionMin and medicion <= @medicionMax";
            cmd.CommandText = result;
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@nombre", nombre);
            cmd.Parameters.AddWithValue("@latitudMin", latitudInicia);
            cmd.Parameters.AddWithValue("@latitudMax", latitudFinal);
            cmd.Parameters.AddWithValue("@longitudMin", longitudInicial);
            cmd.Parameters.AddWithValue("@longitudMax", longitudFinal);
            cmd.Parameters.AddWithValue("@alturaMin", alturaInicial);
            cmd.Parameters.AddWithValue("@alturaMax", alturaFinal);
            cmd.Parameters.AddWithValue("@medicionMin", medicionInicial);
            cmd.Parameters.AddWithValue("@medicionMax", medicionFinal);
            dt = ejecutar.Ejecutar(cmd);
            return dt;
        }

        public int addStats(string nombre, double medicion, double altura, double latitud, double longitud)
        {
            int status = 400;
            string query = @"Insert into stats values (@nombre,@medicion, @altura, @latitud,@longitud,@fecha)";
            cmd.CommandText = query;
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@nombre", nombre);
            cmd.Parameters.AddWithValue("@medicion", medicion);
            cmd.Parameters.AddWithValue("@latitud", latitud);
            cmd.Parameters.AddWithValue("@altura", altura);
            cmd.Parameters.AddWithValue("@longitud", longitud);
            cmd.Parameters.AddWithValue("@fecha", DateTime.Now);
            try
            {
                ejecutar.Ejecutar(cmd);
                status = 200;
                return status;
            }
            catch
            {
                return status;
            }
        }

        public DataTable datosGrafica(string nombre, string distanciaInicial, string distanciaFinal)
        {
            string query = @"select * from stats where nombre = @nombre and altura >= @distanciaMin and altura <= @distanciaMax";
            cmd.CommandText = query;
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@nombre", nombre);
            cmd.Parameters.AddWithValue("@distanciaMin", distanciaInicial);
            cmd.Parameters.AddWithValue("@distanciaMax", distanciaFinal);
            dt = ejecutar.Ejecutar(cmd);
            return dt;

        }
    }
}
