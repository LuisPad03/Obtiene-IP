using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Timers;
using System.Runtime.InteropServices;
using Timer = System.Timers.Timer;
using System.Net;
using System.IO;
using System.Xml;
using Microsoft.Win32;

namespace Obtiene_IP
{
    public partial class Form1 : Form
    {
        string archivoip = Application.StartupPath + "\\GuardaIP.txt";
        string archivoxml2 = Application.StartupPath + "\\MDatos.xml";
        string subkey = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Run";
        //Equipo\HKEY_LOCAL_MACHINE\SOFTWARE\WOW6432Node\Microsoft\Windows\CurrentVersion\Run

        public Form1()
        {
            InitializeComponent();
        }

        
        private void Form1_Load(object sender, EventArgs e)
        {
            ConfiguracionInicial();

            Recarga();

            Timer timer = new Timer(30000);
            timer.Elapsed += Timer_Elapsed;
            timer.Start();

        }

        private void ConfiguracionInicial()
        {
            Correo oCorreo = new Correo();

            if (File.Exists(archivoxml2)) correoTextBox.Text = oCorreo.ObtieneCorreo();
            else
            {
                DataSet1 oDato = new DataSet1();

                DataRow registro = oDato.Dato.NewRow();
                registro[0] = "Agrega correo";
                oDato.Dato.Rows.Add(registro);
                datoBindingSource.EndEdit();

                DataRow registroequipo = oDato.Equipo.NewRow();
                registroequipo[0] = "Agrega nombre del equipo";
                registroequipo[1] = "Agrega empresa";
                oDato.Equipo.Rows.Add(registroequipo);
                equipoBindingSource.EndEdit();
                oDato.WriteXml(archivoxml2);
            }

            if (!File.Exists(archivoip))
            {
                using (StreamWriter sw = File.CreateText(archivoip))
                {
                    sw.Close();
                }

                try
                {
                    Registry.LocalMachine.CreateSubKey(subkey).SetValue("Obtiene-IP", Application.StartupPath + "\\Obtiene-IP.exe");
                    //MessageBox.Show("registro creado");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
 
            }
        }

        public void Recarga()
        {
            ObtieneIP oObIP = new ObtieneIP();
            if (oObIP.ObtienIP() != "")
            {
                Lbl_texto_ip.Enabled = true;
                Lbl_ip.Text = oObIP.ObtienIP();
            }
            else
            {
                Lbl_texto_ip.Enabled = false;
                Lbl_ip.Text = "No hay conexión";
            }

            Configuracion oConfig = new Configuracion();
            correoTextBox.Text = oConfig.ObtieneDato(archivoxml2, "Correo", "Dato");
        }

        private static void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            ObtieneIP oObtiene = new ObtieneIP();
            oObtiene.ComparaIP();
            
            //MessageBox.Show("se gaurdo");
        }

        private void btn_guardar_Click(object sender, EventArgs e)
        {
            Correo oCorreo = new Correo();
            oCorreo.datoBindingSource = datoBindingSource;
                        
            if (correoTextBox.Text != "")
            {
                if (File.Exists(archivoxml2))
                {
                    if (correoTextBox.Text != oCorreo.ObtieneCorreo())
                    {
                        oCorreo.Guarda(correoTextBox.Text);
                    }
                    else MessageBox.Show("Este correo ya ha sido registrado.","", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else oCorreo.Guarda(correoTextBox.Text);

                correoTextBox.Refresh();
            }
            else MessageBox.Show("Es necesario ingresar un dato","", MessageBoxButtons.OK, MessageBoxIcon.Information);

        }

        private void iconrestaurar_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;    //Asinga el estado de la ventana como minimizado
            this.Hide();
            notifyIcon1.BalloonTipText = "Trabajando en segundo plano";
            notifyIcon1.ShowBalloonTip(3000);
        }


        #region Movimiento a la barra det titulo

        // Codigo para dar movimiento en la pantalla

        [DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]
        private extern static void ReleaseCapture();
        [DllImport("user32.DLL", EntryPoint = "SendMessage")]
        private extern static void SendMessage(System.IntPtr hwnd, int wmsg, int wparam, int lparam);

        // Codigo para dar movimiento en la pantalla


        // evento MouseDown de la herramienta(s) que tendra movimiento
        private void BarraTitulo_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0x112, 0xf012, 0);
        }
        // evento MouseDown de la herramienta(s) que tendra movimiento


        #endregion

        private void Timer_recarga_Tick(object sender, EventArgs e)
        {
            Recarga();
        }


        private void restaurarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Show();
            this.WindowState = FormWindowState.Normal;
        }

        private void notifyIcon1_Click(object sender, EventArgs e)
        {
            this.Show();
            this.WindowState = FormWindowState.Normal;
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            Configuracion oConfiguracion = new Configuracion();
            oConfiguracion.Show();
        }
    }
}
