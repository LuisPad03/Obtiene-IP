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
using System.Xml;
using System.Runtime.InteropServices;

namespace Obtiene_IP
{
    public partial class Configuracion : Form
    {
        string archivoxml = Application.StartupPath + "\\GuardaIP.txt";
        string archivoxml2 = Application.StartupPath + "\\MDatos.xml";
        

        public Configuracion()
        {
            InitializeComponent();
        }

        private void Configuracion_Load(object sender, EventArgs e)
        {
            if (File.Exists(archivoxml2)) Recarga();
            else iconrestaurar.Enabled = false;
        }

        private void iconrestaurar_Click(object sender, EventArgs e)
        {
            if (!File.Exists(archivoxml))
            {
                Form1 oForm = new Form1();
                oForm.Show();
            }
            this.Close();
        }

        private void btn_guardar_Click(object sender, EventArgs e)
        {
            if (Ttx_correo.Text != "" & Txt_empresa.Text != "" & Txt_equipo.Text != "")
            {
                if(Guarda(Ttx_correo.Text, Txt_equipo.Text, Txt_empresa.Text))
                {
                    if (!File.Exists(archivoxml))
                    {
                        this.Close();
                        Form1 oForm = new Form1();
                        oForm.Show();
                    }
                    else
                    //iconrestaurar.Enabled = true;
                    Recarga();
                }
                
            }
            else MessageBox.Show("Es necesario ingresar todos los datos.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);

        }


        public bool Guarda(string correonuevo, string equipo, string empresa)
        {

            Correo oCorreo = new Correo();

            if (oCorreo.VerificaCorreo(correonuevo))
            {
                DataSet1 oDato = new DataSet1();

                try
                {
                    DataRow registrocorreo = oDato.Dato.NewRow();
                    registrocorreo[0] = correonuevo;
                    oDato.Dato.Rows.Add(registrocorreo);
                    datoBindingSource.EndEdit();

                    DataRow registroequipo = oDato.Equipo.NewRow();
                    registroequipo[0] = equipo;
                    registroequipo[1] = empresa;
                    oDato.Equipo.Rows.Add(registroequipo);
                    equipoBindingSource.EndEdit();
                    oDato.WriteXml(archivoxml2);

                    MessageBox.Show("Datos gaurdados correctamente", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error al guardar los datos", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return false;
                }
            }
            else
            {
                MessageBox.Show("Formato no valido del correo", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }


            


        }


        public string ObtieneDato(string archivo, string tipo, string tabla)
        {
            string datoguardado = "";

            try
            {
                XmlDocument xDoc = new XmlDocument();
                xDoc.Load(archivo);

                XmlNodeList xDato = xDoc.GetElementsByTagName(tabla);
                XmlNodeList xLista = ((XmlElement)xDato[0]).GetElementsByTagName(tipo);

                //datoguardado= XmlElement nodo in xLista.i
                foreach (XmlElement nodo in xLista)
                {
                    datoguardado = nodo.InnerText;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error AL OBTENER CORREO: ", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }


            return datoguardado;

        }


        public void Recarga()
        {
            Txt_equipo.Text = ObtieneDato(archivoxml2, "Nombre", "Equipo");
            Txt_empresa.Text = ObtieneDato(archivoxml2, "Empresa", "Equipo");
            Ttx_correo.Text = ObtieneDato(archivoxml2, "Correo", "Dato");
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

    }
}
