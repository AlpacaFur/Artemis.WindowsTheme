using Artemis.Core.DataModelExpansions;
using System;
using SystemTheme.DataModels;
using System.Management;
using Microsoft.Win32;

namespace SystemTheme
{
    public class ThemeDataModelExpansion : DataModelExpansion<ThemeDataModel>
    {

        private static readonly string userID = System.Security.Principal.WindowsIdentity.GetCurrent().User.Value;
        private static readonly string wmiRegistryPath = String.Format(@"{0}\\Software\\Microsoft\\Windows\\CurrentVersion\\Themes\\Personalize", userID);

        private ManagementEventWatcher watcher;

        public override void Enable()
        {
            Console.WriteLine("Test");

            WqlEventQuery query = new WqlEventQuery(String.Format(
                    System.Globalization.CultureInfo.InvariantCulture,
                    @"SELECT * FROM RegistryValueChangeEvent WHERE Hive = 'HKEY_USERS' AND Keypath = '{0}' AND (ValueName='SystemUsesLightTheme' OR ValueName='AppsUseLightTheme')", wmiRegistryPath));

            watcher = new ManagementEventWatcher(query);

            watcher.EventArrived += new EventArrivedEventHandler(RegistryKeyChanged);

            watcher.Start();

            RefreshThemeInfo();

        }

        public void RefreshThemeInfo()
        {
            try
            {
                RegistryKey registryKey = Registry.Users;
                RegistryKey subKey = registryKey.CreateSubKey(wmiRegistryPath);
                DataModel.AppTheme = (ThemeDataModel.Theme)(int)subKey.GetValue("AppsUseLightTheme");
                DataModel.SystemTheme = (ThemeDataModel.Theme)(int)subKey.GetValue("SystemUsesLightTheme");
            }
            catch (Exception e)
            {
                Console.WriteLine("Error reading key!");
                Console.WriteLine(e);
                DataModel.AppTheme = ThemeDataModel.Theme.Dark;
                DataModel.SystemTheme = ThemeDataModel.Theme.Dark;
            }
        }

        public void RegistryKeyChanged(object sender, EventArrivedEventArgs e)
        {
            RefreshThemeInfo();
        }

        public override void Disable()
        {
            watcher.Dispose();
        }

        public override void Update(double deltaTime)
        {
          
        }
    }
}