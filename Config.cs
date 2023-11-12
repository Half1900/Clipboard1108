using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Clipboards
{
    public class Config
    {
        [DllImport("kernel32.dll")]
        private static extern long WritePrivateProfileString(string section, string key, string value, string filepath);

        [DllImport("kernel32.dll")]
        private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);

        private string config_path;
        private string exe_path;
        public Config()
        {
            config_path = System.Environment.CurrentDirectory + "\\config.ini";
            exe_path = System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName;
        }



        public int readint(string key)
        {
           string str=readstring(Convert.ToString(key));
           return int.Parse(str);
        }

        public bool readbool(string key)
        {
            string str = readstring(Convert.ToString(key));
            bool b=str=="True"?true:false;
            return b;
        }

        public String readstring(string key)
        {
            StringBuilder v = new StringBuilder(1024);
            string v2;
            GetPrivateProfileString("config", key, "0", v, 1024, config_path);
            v2 = v.ToString();
            return v2;
        }

        public void write(string key, bool value)
        {
            string b= value ? "True" : "False";
            write(key, Convert.ToString(value));
        }

        public void write(string key, int value)
        {
            write(key, Convert.ToString(value));
        }

        public void write(string key,string value)
        {
            WritePrivateProfileString("config", key, value, config_path);
        }
        public bool read_auto_start()
        {
            RegistryKey registryKey = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
            string str=(string)registryKey.GetValue("Clipboards");
            Trace.WriteLine("GetValue:"+str);
            bool b= str==exe_path ? true:false;
            registryKey.Close();
            return b;
        }
        public void add_auto_start()
        {
            RegistryKey registryKey = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
            registryKey.SetValue("Clipboards", exe_path);
            registryKey.Close();
        }
        public void del_auto_start()
        {
            RegistryKey registryKey = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
            try
            {
                registryKey.DeleteValue("Clipboards");
            }
            catch (Exception)
            {
            }
            registryKey.Close();
        }
    }
}
