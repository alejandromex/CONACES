using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting.Channels;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using System.Data.SqlClient;
using System.Data.Sql;

namespace Arduino
{
    public partial class Form1 : Form
    {
        System.IO.Ports.SerialPort Port;
        bool isClosed = true, startClock = false;
        DateTime fechaInicial;
        public int xClick = 0, yClick = 0;
        Helpers.dbConnect ejecutar = new Helpers.dbConnect();
        SqlCommand cmd = new SqlCommand();
        DataTable dt = new DataTable();
        private Controlador.statsController statsController = new Controlador.statsController();



        public Form1()
        {
            InitializeComponent();
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            Port = new System.IO.Ports.SerialPort();
            Port.BaudRate = 9600;
            Port.PortName = "COM4";
            Port.ReadTimeout = 500;
            Port.Open();
            

        }



        private void Form1_Load(object sender, EventArgs e)
        {
            ocultarPaneles();
            MostrarPanelesPrincipales();
            Thread Hilo;
            Thread Hora;
            Hora = new Thread(MostrarHora);
            Hora.Start();
            Hilo = new Thread(LecturaArduino);
            Hilo.Start();


        }

        private void MostrarHora()
        {
            while (isClosed)
            {
                try
                {
                    lblHora.Invoke(new MethodInvoker(
                        delegate
                        {
                            lblHora.Text = DateTime.Now.ToString("hh:mm:ss");
                            lblFecha.Text = DateTime.Now.ToShortDateString();
                            //if(!cbFiltro.Checked)
                            //{
                            //    RellenarDataGridView();
                            //}
                            if (!startClock)
                            {
                                fechaInicial = DateTime.Now;
                            }
                            if (startClock)
                            {
                                var horas = (DateTime.Now - fechaInicial).ToString(@"hh\h\ mm\m\ ss\s\ ");
                                lblHoras.Text = horas;
                            }


                        }
                        ));
                }
                catch
                {

                }
            }
        }

        private void LecturaArduino()
        {
            while (isClosed)
            {
                try
                {
                    string cadena = Port.ReadLine();
                    lblPm.Invoke(new MethodInvoker(
                        delegate
                        {
                            if (cadena.Contains("PM2.5"))
                            {
                                cadena = cadena.Replace("PM2.5", "");
                                cadena = cadena.Trim();
                                lblPm.Text = cadena;
                                statsController.addStats(
                                    "PM2.5",
                                    double.Parse(lblPm.Text),
                                    double.Parse(lblAltura.Text),
                                    double.Parse(lblAltitud.Text),
                                    double.Parse(lblLongitud.Text));
                            }
                        }
                        ));

                    lblPm10.Invoke(new MethodInvoker(
                        delegate
                        {
                            if (cadena.Contains("PM10"))
                            {
                                cadena = cadena.Replace("PM10", "");
                                cadena = cadena.Trim();
                                lblPm10.Text = cadena;
                                statsController.addStats(
                                    "PM10",
                                    double.Parse(lblPm.Text),
                                    double.Parse(lblAltura.Text),
                                    double.Parse(lblAltitud.Text),
                                    double.Parse(lblLongitud.Text));

                            }
                        }
                        ));

                    lblPresion.Invoke(new MethodInvoker(
                        delegate
                        {
                            if (cadena.Contains("Presion"))
                            {
                                cadena = cadena.Replace("Presion", "");
                                cadena = cadena.Trim();
                                lblPresion.Text = cadena;
                                statsController.addStats(
                                    "Presion",
                                    double.Parse(lblPm.Text),
                                    double.Parse(lblAltura.Text),
                                    double.Parse(lblAltitud.Text),
                                    double.Parse(lblLongitud.Text));
                            }
                        }
                        ));

                    lblTemperatura.Invoke(new MethodInvoker(
                        delegate
                        {
                            if (cadena.Contains("Temperatura"))
                            {
                                cadena = cadena.Replace("Temperatura", "");
                                cadena = cadena.Trim();
                                lblTemperatura.Text = cadena;
                                statsController.addStats(
                                    "Temperatura",
                                    double.Parse(lblPm.Text),
                                    double.Parse(lblAltura.Text),
                                    double.Parse(lblAltitud.Text),
                                    double.Parse(lblLongitud.Text));
                            }
                        }
                        ));
                    lblAltura.Invoke(new MethodInvoker(
                        delegate
                        {
                            if (cadena.Contains("Altura"))
                            {
                                cadena = cadena.Replace("Altura", "");
                                cadena = cadena.Trim();
                                lblAltura.Text = cadena;
                            }
                        }
                        ));

                    lblLongitud.Invoke(new MethodInvoker(
                        delegate
                        {
                            if (cadena.Contains("Longitud"))
                            {
                                cadena = cadena.Replace("Longitud", "");
                                cadena = cadena.Trim();
                                lblLongitud.Text = cadena;
                            }
                        }
                        ));

                    lblAltitud.Invoke(new MethodInvoker(
                        delegate
                        {
                            if (cadena.Contains("Latitud"))
                            {
                                cadena = cadena.Replace("Latitud", "");
                                cadena = cadena.Trim();
                                lblAltitud.Text = cadena;
                            }
                        }
                        ));

                }
                catch
                {
                }
            }
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            Port.Write("c");

            if (Port.IsOpen)
                {
                Port.Close();
                }
            
            isClosed = false;
           
        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            MoverFormulario(sender, e);
        }

        private void MoverFormulario(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left)
            { xClick = e.X; yClick = e.Y; }
            else
            { this.Left = this.Left + (e.X - xClick); this.Top = this.Top + (e.Y - yClick); }
        }

        private void btnMinimizar_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;

        }
        private void btnCerrar_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Desea salir?", "Quit", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                this.Close();
            }
        }

        private void btnEstadisctica_Click(object sender, EventArgs e)
        {
            ocultarPaneles();
            panelGraficas.Visible = true;
            //cmbGrafica.Items.Clear();
            panelGraficas.Size = new System.Drawing.Size(1191, 720);
        }
        private void panel1_Click(object sender, EventArgs e)
        {
            ocultarPaneles();
            MostrarPanelesPrincipales();
            RellenarDataGridView();

        }

        private void RellenarDataGridView()
        {
            dt = statsController.getAllStats();
            dgvRegistros.Rows.Clear();
            if(dt != null)
            {
                for (int i = 0; i<dt.Rows.Count; i++)
                {
                    string[] row = { dt.Rows[i]["nombre"].ToString(),
                                     dt.Rows[i]["medicion"].ToString(),
                                     dt.Rows[i]["altura"].ToString(),
                                     dt.Rows[i]["latitud"].ToString(),
                                     dt.Rows[i]["longitud"].ToString(),
                                     dt.Rows[i]["fecha"].ToString() };

                    dgvRegistros.Rows.Add(row);
                }
                lblResultados.Text = "Resultados: " + dt.Rows.Count.ToString();

            }
            else
            {
                lblResultados.Text = "Resultados 0";
            }
        }

        private void cmbGrafica_TextChanged(object sender, EventArgs e)
        {
            grafica.Series.Clear();
            //grafica.Series.Add(cmbGrafica.Text);
            grafica.Series[0].ChartType = SeriesChartType.Line;
            grafica.Series[0].Points.AddXY(double.Parse(lblAltitud.Text), double.Parse(lblPm.Text));

        }



        private void MostrarPanelesPrincipales()
        {
            panelPrincipal.Size = new System.Drawing.Size(1566, 744);
            panelPrincipal.Visible = true;
        }

        private void ocultarPaneles()
        {
            panelPrincipal.Visible = false;
            panelGraficas.Visible = false;
            panelRegistros.Visible = false;
        }

        private void panel12_Click(object sender, EventArgs e)
        {
            ocultarPaneles();
            panelRegistros.Visible = true; 
            panelRegistros.Size = new System.Drawing.Size(1200, 659);
        }

        private void cbFiltro_CheckedChanged(object sender, EventArgs e)
        {
            if(cbFiltro.Checked)
            {
                cmbRegistros.Enabled = true;
                cmbLatitudInicial.Enabled = true;
                cmbLongitudInicial.Enabled = true;
                cmbLongitudFinal.Enabled = true;
                cmbLatitudFinal.Enabled = true;
                txtAlturaFinal.Enabled = true;
                txtAlturaInicial.Enabled = true;
                txtMedicionInicial.Enabled = true;
                txtMedicionFinal.Enabled = true;
            }
            else
            {
                cmbRegistros.Enabled = false;
                cmbLatitudInicial.Enabled = false;
                cmbLongitudInicial.Enabled = false;
                cmbLongitudFinal.Enabled = false;
                cmbLatitudFinal.Enabled = false;
                txtAlturaFinal.Enabled = false;
                txtAlturaInicial.Enabled = false;
                txtMedicionInicial.Enabled = false;
                txtMedicionFinal.Enabled = false;
                RellenarDataGridView();
            }
        }
        private void RellenarDataGridViewConFiltro()
        {
            string latitudInicial = cmbLatitudInicial.Text;
            string latitudFinal = cmbLatitudFinal.Text;
            string longitudInicial = cmbLongitudInicial.Text;
            string longitudFinal = cmbLongitudFinal.Text;
            string alturaInicial = txtAlturaInicial.Text;
            string alturaFinal = txtAlturaFinal.Text;
            string medicionInicial = txtMedicionInicial.Text;
            string medicionFinal = txtMedicionFinal.Text;
            if (cmbLatitudInicial.Text == "")
            {
                latitudInicial = "0";
            }
            if (cmbLatitudFinal.Text == "")
            {
                latitudFinal = "300000";
            }
            if (cmbLongitudInicial.Text == "")
            {
                longitudInicial = "0";
            }
            if (cmbLongitudFinal.Text == "")
            {
                longitudFinal = "300000";
            }
            if (txtAlturaInicial.Text == "")
            {
                alturaInicial = "0";
            }
            if (txtAlturaFinal.Text == "")
            {
                alturaFinal = "300000";
            }
            if (txtMedicionInicial.Text == "")
            {
                medicionInicial = "0";
            }
            if (txtMedicionFinal.Text == "")
            {
                medicionFinal = "300000";
            }
                dt = statsController.getAllStatsWithName(cmbRegistros.Text, latitudInicial, latitudFinal, longitudInicial, longitudFinal, alturaInicial, alturaFinal, medicionInicial, medicionFinal);
                dgvRegistros.Rows.Clear();


                if (dt != null)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        string[] row = { dt.Rows[i]["nombre"].ToString(),
                                     dt.Rows[i]["medicion"].ToString(),
                                     dt.Rows[i]["altura"].ToString(),
                                     dt.Rows[i]["latitud"].ToString(),
                                     dt.Rows[i]["longitud"].ToString(),
                                     dt.Rows[i]["fecha"].ToString() };

                        dgvRegistros.Rows.Add(row);

                    }
                    lblResultados.Text = "Resultados: " + dt.Rows.Count.ToString();

                }
                else
                {
                    lblResultados.Text = "Resultados: 0";

                }
            }
        

        private void cmbRegistros_TextChanged(object sender, EventArgs e)
        {

            RellenarDataGridViewConFiltro();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            if (cbFiltro.Checked)
            {
                
                RellenarDataGridViewConFiltro();

            }
            else
            {
                RellenarDataGridView();
            }
        }

        private void btnInicio_Click(object sender, EventArgs e)
        {
            Port.Write("e");
            startClock = true;
        }

        private void btnRefreshGrafica_Click(object sender, EventArgs e)
        {
           grafica.Series.Clear();

            if (cmbGrafica.Text != "")
            {
                grafica.Series.Add(cmbGrafica.Text);
                grafica.Series[cmbGrafica.Text].ChartType = SeriesChartType.Column;
                grafica.Series[cmbGrafica.Text].XValueMember = "Medicion";
                grafica.Series[cmbGrafica.Text].YValueMembers = "Altura";
                grafica.DataSource = GetData();
            }
            
            

        }

        private DataTable GetData()
        {
            string distanciaInicial;
            string distanciaFinal;
            if(txtA.Text == "")
            {
                distanciaInicial = "0";
                distanciaFinal = "500";
                txtB.Text = "500";
            }
            else
            {
                distanciaInicial = txtA.Text;
                distanciaFinal = (int.Parse(txtA.Text) +500).ToString();
                txtB.Text = distanciaFinal;
            }
            dt = statsController.datosGrafica(cmbGrafica.Text, distanciaInicial, distanciaFinal);
            int totalDt = dt.Rows.Count;
           
            dt.Columns.Add("Medicion");
            dt.Columns.Add("Altura");

            DataRow row = dt.NewRow();
            if(dt.Rows.Count > 0)
            {
                lblTotalGrafica.Text = "Datos totales: "+totalDt.ToString();
                for (int i = 0; i < totalDt; i++)
                {
                    row = dt.NewRow();
                    row["Medicion"] = dt.Rows[i]["medicion"].ToString();
                    row["Altura"] = dt.Rows[i]["altura"].ToString();
                    dt.Rows.Add(row);
                }
            }
            else
            {
                lblTotalGrafica.Text = "Datos: 0";
            }
            

            //row["Medicion"] = 10;
            //row["Altura"] = 20;
            //dt.Rows.Add(row);

            //row = dt.NewRow();

            //row["Medicion"] = 20;
            //row["Altura"] = 60;
            //dt.Rows.Add(row);


            return dt;

        }

        private void btnDataBase_Click(object sender, EventArgs e)
        {
            
        }

        

    }
}
