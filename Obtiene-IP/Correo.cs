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
using System.Xml;
using System.Data;
using System.Text.RegularExpressions;
using System.Globalization;

namespace Obtiene_IP
{
    class Correo
    {
        
        public BindingSource datoBindingSource;
        public BindingSource equipoBindingSource;
        string GuardaDato = Application.StartupPath + "\\MDatos.xml";
        string archivoxml2 = Application.StartupPath + "\\MDatos.xml";

        public string ObtieneCorreo()
        {
            string correoguardado = "";
            string archivoxml = Application.StartupPath + "\\MDatos.xml";

            try
            {
                XmlDocument xDoc = new XmlDocument();
                xDoc.Load(archivoxml);

                XmlNodeList xDato = xDoc.GetElementsByTagName("Dato");
                XmlNodeList xLista = ((XmlElement)xDato[0]).GetElementsByTagName("Correo");

                foreach (XmlElement nodo in xLista)
                {
                    correoguardado = nodo.InnerText;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error AL OBTENER CORREO: ", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }


            return correoguardado;

        }

        public void Guarda(string correonuevo)
        {
           

            if (VerificaCorreo(correonuevo))
            {
                Configuracion oconfiguracion = new Configuracion();

                string equipo = oconfiguracion.ObtieneDato(archivoxml2, "Nombre", "Equipo");
                string empresa = oconfiguracion.ObtieneDato(archivoxml2, "Empresa", "Equipo");

                oconfiguracion.Guarda(correonuevo, equipo, empresa);
            }
            else MessageBox.Show("Formato no valido del correo", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);

        }


        public bool EnviaCorreo(string fecha_mensaje, string ip,string equipo,string empresa)
        {
            bool correo = false;
            string destino;
            

            if (ObtieneCorreo() == "") destino = "luisedgar.cic@gmail.com";
            else destino = ObtieneCorreo();

            try
            {
                Correos oCorreo = new Correos();
                MailMessage msg = new MailMessage();
                msg.Subject = "Cambio de IP " + empresa;
                msg.To.Add(destino);
                msg.From = new MailAddress("CORREO DE SALIDA", "Cambio de IP " + "\" " + equipo.ToLower() + " \"" + " " + empresa);
                msg.Body = "La direccion IP a cambiado a: " + ip + "\n\n" + "Fecha de cambio: " + fecha_mensaje;
                oCorreo.EnviaCorreo(msg);
                correo = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show( ex.Message, "Error AL ENVIAR CORREO: ",MessageBoxButtons.OK,MessageBoxIcon.Information);
            }
            return correo;
        }

        internal class Correos
        {
            SmtpClient server = new SmtpClient("SMTP.Office365.com", 587);
            public Correos()
            {
                server.Credentials = new System.Net.NetworkCredential("CORREO DE SALIDA", "CONTRASEÑA DEL CORREO DE SALIDA");
                server.EnableSsl = true;
            }
            public void EnviaCorreo(MailMessage message)
            {
                server.Send(message);
            }
        }

        public bool VerificaCorreo1 (string correo)
        {
            String sFormato;
            sFormato = "\\w+([-+.']\\w+)*@\\w+([-.]\\w+)*\\.\\w+([-.]\\w+)*";
            if (Regex.IsMatch(correo, sFormato))
            {
                if (Regex.Replace(correo, sFormato, String.Empty).Length == 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }

        }

        public  bool VerificaCorreo(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            try
            {
                // Normalize the domain
                email = Regex.Replace(email, @"(@)(.+)$", DomainMapper,
                                      RegexOptions.None, TimeSpan.FromMilliseconds(200));

                // Examines the domain part of the email and normalizes it.
                string DomainMapper(Match match)
                {
                    // Use IdnMapping class to convert Unicode domain names.
                    var idn = new IdnMapping();

                    // Pull out and process domain name (throws ArgumentException on invalid)
                    var domainName = idn.GetAscii(match.Groups[2].Value);

                    return match.Groups[1].Value + domainName;
                }
            }
            catch (RegexMatchTimeoutException e)
            {
                return false;
            }
            catch (ArgumentException e)
            {
                return false;
            }

            try
            {
                return Regex.IsMatch(email,
                    @"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
                    @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-0-9a-z]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$",
                    RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
        }


    }
}
