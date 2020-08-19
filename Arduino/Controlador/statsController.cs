using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data.Sql;
using System.Data;
using System.Windows.Forms;

namespace Arduino.Controlador
{
    class statsController
    {
        private DataTable dt = new DataTable();
        private SqlCommand cmd = new SqlCommand();
        private Helpers.dbConnect ejecutar = new Helpers.dbConnect();
        private Modelos.stats modeloStat = new Modelos.stats();

        public DataTable getAllStats()
        {
            dt = modeloStat.getAllStats();
            if(dt.Rows.Count == 0)
            {
                return null;
            }
            

            return dt;
        }

        public DataTable getAllStatsWithName(string nombre, string latitudInicial, string latitudFinal,string longitudInicial, string longitudFinal, string alturaInicial, string alturaFinal, string medicionInicial, string medicionFinal)
        {
            dt = modeloStat.getAllStatsWithName(nombre,latitudInicial,latitudFinal,longitudInicial,longitudFinal, alturaInicial, alturaFinal, medicionInicial, medicionFinal);
            if (dt.Rows.Count == 0)
            {
                return null;
            }
            return dt;
        }


            public int addStats(string nombre, double medicion, double altura, double latitud, double longitud)
        {
            int status;
            Modelos.stats modelo = new Modelos.stats();
            status = modelo.addStats(nombre, medicion, altura, latitud, longitud);
            return status;
        }

        public DataTable datosGrafica(string nombre, string distanciaInicial, string distanciaFinal)
        {
            dt = modeloStat.datosGrafica(nombre, distanciaInicial, distanciaFinal);
            return dt;

        }
    }

   
}
