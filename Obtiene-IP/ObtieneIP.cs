using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using System.Timers;
using System.Net.Mail;
using System.Runtime.CompilerServices;
using System.Diagnostics;
using System.ComponentModel;
using System.Windows.Forms;



namespace Obtiene_IP
{
    public class ObtieneIP
    {

        string archivoxml2 = Application.StartupPath + "\\MDatos.xml";
        public string ComparaIP()
        {
            DateTime Hoy = DateTime.Now;
            string fecha = Hoy.ToString();

            Correo oCorreo = new Correo();

            StreamReader leer = new StreamReader("GuardaIP.txt");
            string ipguardada = leer.ReadLine();
            leer.Close();

            string ipactual = ObtienIP();
            string ipnueva = "";

            Configuracion oConfig = new Configuracion();
            string equipo = oConfig.ObtieneDato(archivoxml2, "Nombre", "Equipo");
            string empresa = oConfig.ObtieneDato(archivoxml2, "Empresa", "Equipo");

            try
            {
                if (ipactual != ipguardada & ipactual!="")
                {
                    GuardaIP(ipactual);
                    oCorreo.EnviaCorreo(fecha, ipactual, equipo, empresa);
                    ipnueva = ipactual;
                }
                else ipnueva = ipguardada;

            }
            catch (Exception ex)
            {
                MessageBox.Show(  ex.Message,"Error COMPARAIP: ", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            return ipnueva;
        }

        public string ObtienIP()
        {
            string DireccioIP = "";
            try
            {
                WebRequest request = WebRequest.Create("http://checkip.dyndns.org/");
                using (WebResponse response = request.GetResponse())
                using (StreamReader stream = new StreamReader(response.GetResponseStream()))
                {
                    DireccioIP = stream.ReadToEnd();
                }

                int primero = DireccioIP.IndexOf("Address: ") + 9;
                int ultimo = DireccioIP.IndexOf("</body>");
                DireccioIP = DireccioIP.Substring(primero, ultimo - primero);
            }
            catch (Exception ex)
            {
               // MessageBox.Show("Error: " + ex.Message);
            }

            return DireccioIP;
        }

        static void GuardaIP(string ip)
        {
            try
            {
                TextWriter GuardaIP;
                GuardaIP = new StreamWriter("GuardaIP.txt");
                GuardaIP.WriteLine(ip);
                GuardaIP.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al gaurdar direccion IP: " + ex.Message);
            }
            
        }

    }
}
