using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ServiceProcess;

namespace MediLab.Servicios
{
    
    public static class Servicios
    {
        private static String scUsuariosDisplayName = "Unlock Users";
        private static ServiceController scUsuarios = new ServiceController(scUsuariosDisplayName);
        
        public static String getStatusServiceUsuarios()
        {
            return scUsuarios.Status.ToString();
        }
        public static void turnOffServiceUsuarios()
        {
            if ((!scUsuarios.Status.Equals(ServiceControllerStatus.Stopped)) &&
                  (!scUsuarios.Status.Equals(ServiceControllerStatus.StopPending)))
            {
                scUsuarios.Stop();
                scUsuarios.Refresh();
            }
        }
        public static String getUUProcName()
        {
            return Servicios.scUsuariosDisplayName;
        }
        public static void turnOnServiceUsuarios()
        {

            if ((scUsuarios.Status.Equals(ServiceControllerStatus.Stopped)) ||
                   (scUsuarios.Status.Equals(ServiceControllerStatus.StopPending)))
            {           
                            
                scUsuarios.Start();
                scUsuarios.Refresh();
            }

        }
    }
}