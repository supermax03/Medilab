using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ServiceProcess;

namespace MediLab.Servicios
{
    
    public static class Servicios
    {
        private static String scUsuariosDisplayName = "UnlockUsersService";//"Unlock Users";
        private static ServiceController scUsuarios = new ServiceController(scUsuariosDisplayName);
        
        public static String getStatusServiceUsuarios()
        {
            scUsuarios.Refresh();
            return scUsuarios.Status.ToString();
        }
        public static void turnOffServiceUsuarios()
        {
            try
            {
                if ((!scUsuarios.Status.Equals(ServiceControllerStatus.Stopped)) &&
                      (!scUsuarios.Status.Equals(ServiceControllerStatus.StopPending)))
                {
                    
                    scUsuarios.Stop();
                    scUsuarios.Refresh();
                }
            }
            catch(Exception exc)
            { }
        }
        public static String getUUProcName()
        {
            return Servicios.scUsuariosDisplayName;
        }
        public static void turnOnServiceUsuarios()
        {
            try
            {

                if ((scUsuarios.Status.Equals(ServiceControllerStatus.Stopped)) ||
                       (scUsuarios.Status.Equals(ServiceControllerStatus.StopPending)))
                {

                    scUsuarios.Start();
                    scUsuarios.Refresh();
                }
            }
            catch(Exception ex)
            {

            }

        }
    }
}