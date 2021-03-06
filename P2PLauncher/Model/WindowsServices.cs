﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace P2PLauncher.Model
{
    public class WindowsServices
    {
        public List<WindowsService> GetServices()
        {
            List<WindowsService> windowsServices = new List<WindowsService>();

            ServiceController[] services = ServiceController.GetServices();

            foreach(ServiceController service in services)
            {
                windowsServices.Add(new WindowsService().FromServiceController(service));
            }
            return windowsServices;
        }

        public WindowsService GetServiceByName(string serviceName)
        {
            ServiceController controller = ServiceController.GetServices().FirstOrDefault(serviceController => serviceController.ServiceName.Equals(serviceName));
            if(controller == null)
            {
                return null;
            }
            return new WindowsService().FromServiceController(controller);
        }

        public List<WindowsService> GetServicesWithType(ServiceType serviceType)
        {
            List<WindowsService> withType = new List<WindowsService>();

            foreach(WindowsService service in GetServices())
            {
                if(service.Type == serviceType)
                {
                    withType.Add(service);
                }
            }
            return withType;
        }

        public List<WindowsService> GetServicesToDisable()
        {
            string[] toDisable = GetServiceNamesToDisable();
            List<WindowsService> toDisableList = new List<WindowsService>();
            foreach(WindowsService w in GetServices())
            {
                if(toDisable.Contains(w.Name))
                {
                    toDisableList.Add(w);
                }
            }
            return toDisableList;

        }

        public void SaveServicesToDisable(List<WindowsService> services)
        {
            string toSave = "";
            for(int i =0; i< services.Count; i++)
            {
                toSave += services[i].Name;
                if(i != services.Count - 1)
                {
                    toSave += ",";
                }
            }
            Properties.Settings.Default.ServicesToDisable = toSave;
            Properties.Settings.Default.Save();
            Properties.Settings.Default.Upgrade();
            Properties.Settings.Default.Reload();
        }
        public string[] GetServiceNamesToDisable()
        {
            string saved = Properties.Settings.Default.ServicesToDisable;
            if(saved == null || saved.Length == 0)
            {
                return new string[0];
            }
            return saved.Split(',');
        }

    }
}
