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
        private static String scMailerDisplayName = "Medilab Mailer";
        private static ServiceController scUsuarios = new ServiceController(scUsuariosDisplayName);
        private static ServiceController scMedilabMailer = new ServiceController(scMailerDisplayName);
        
        public static String getStatusServiceUsuarios()
        {
            scUsuarios.Refresh();
            return scUsuarios.Status.ToString();
        }
        public static String getStatusMailerService()
        {

            return getStatusService(Servicios.scMedilabMailer);
        }
        private static String getStatusService(ServiceController service)
        {
            service.Refresh();
            return service.Status.ToString();
        }
        private static void turnOffService(ServiceController service)
        {
            try
            {
                if ((!service.Status.Equals(ServiceControllerStatus.Stopped)) &&
                      (!service.Status.Equals(ServiceControllerStatus.StopPending)))
                {

                    service.Stop();
                    service.Refresh();
                }
            }
            catch (Exception exc)
            { }
        }
        public static void turnOnService(ServiceController service)
        {
            try
            {

                if ((service.Status.Equals(ServiceControllerStatus.Stopped)) ||
                       (service.Status.Equals(ServiceControllerStatus.StopPending)))
                {

                    service.Start();
                    service.Refresh();
                }
            }
            catch (Exception ex)
            {

            }
        }
        public static void turnOffMailer()
        {
            turnOffService(scMedilabMailer);
        }
        public static void turnOnMailer()
        {
            turnOnService(scMedilabMailer);
        }
        public static void turnOnServiceUsuarios()
        {
            turnOnService(scUsuarios);

        }
        public static void turnOffServiceUsuarios()
        {
            turnOffService(scUsuarios);
        }

    }   
}