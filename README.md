# Obtiene-IP
 Programa que monitoria la IP de una computadora y la envia a un correo.

 * Se requiere ejecutar como adminstrador ya que crea un archivo de registro para ejecuarse al iniciar windows.


 * Para enviar un correo es necesario configurar la siguiente clase:
  Correo.cs

   LINEA 91
    msg.From = new MailAddress("CORREO DE SALIDA", "Cambio de IP " + "\" " + equipo.ToLower() + " \"" + " " + empresa);
    
     cambiar "CORREO DE SALIDA" por el correo del cual se enviaran los correo ejemplo luis@hotmail.com

   LINEA 105
    SmtpClient server = new SmtpClient("SMTP.Office365.com", 587);
    
     esta linea no es necesario cambiarse si el correo es outlook o hotmail, en caso de ser diferente se requiere cambiar el SMTP y el puerto por los del porveedor.


   LINEA 108
    server.Credentials = new System.Net.NetworkCredential("CORREO DE SALIDA", "CONTRASEÑA DEL CORREO DE SALIDA");
    
     cambiar "CORREO DE SALIDA" por el correo del cual se enviaran los correo ejemplo luis@hotmail.com
     
     cambiar "CONTRASEÑA DEL CORREO DE SALIDA" por la contraseña del correo del cual se enviaran los correos
