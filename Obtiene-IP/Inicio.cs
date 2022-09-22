using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace Obtiene_IP
{
    public partial class Inicio : Form
    {
        string archivoxml2 = Application.StartupPath + "\\MDatos.xml";
        int contador = 0;

        public Inicio()
        {
            InitializeComponent();
        }

        private void Inicio_Load(object sender, EventArgs e)
        {
            
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (File.Exists(archivoxml2))
            {
                Form1 oForm = new Form1();
                oForm.Show();
                timer1.Stop();
            }
            else
            {
                Configuracion oConfiguracion = new Configuracion();
                oConfiguracion.Show();
                timer1.Stop();
            }

            this.Hide();
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            timer3.Stop();
            this.Opacity -= 0.05;

            if (this.Opacity == 0)
            {
                timer2.Stop();
            }

        }

        private void timer3_Tick(object sender, EventArgs e)
        {
            timer2.Start();
        }
    }
}
