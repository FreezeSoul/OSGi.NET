using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Management;
using System.Net;
using System.Text;

namespace OSGiClientAgent.Helper
{
    class ClientDeviceHelper
    {

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static string GetMacAddress()
        {
            try
            {
                //获取网卡硬件地址 
                string mac = "";
                var mc = new ManagementClass("Win32_NetworkAdapterConfiguration");
                ManagementObjectCollection moc = mc.GetInstances();
                foreach (var o in moc)
                {
                    var mo = (ManagementObject)o;
                    if ((bool)mo["IPEnabled"] == true)
                    {
                        mac = mo["MacAddress"].ToString();
                        break;
                    }
                }
                return mac;
            }
            catch
            {
                return "unknow";
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static string GetIpAddress()
        {
            try
            {
                //获取IP地址 
                string st = "";
                var mc = new ManagementClass("Win32_NetworkAdapterConfiguration");
                ManagementObjectCollection moc = mc.GetInstances();
                foreach (var o in moc)
                {
                    var mo = (ManagementObject)o;
                    if ((bool)mo["IPEnabled"] == true)
                    {
                        Array ar = (Array)(mo.Properties["IpAddress"].Value);
                        st = ar.GetValue(0).ToString();
                        break;
                    }
                }
                return st;
            }
            catch
            {
                return "unknow";
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static string GetApplicationName()
        {
            try
            {
                var app = Process.GetCurrentProcess().MainModule.FileName;
                return Path.GetFileNameWithoutExtension(app);
            }
            catch
            {
                return "unknow";
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static string GetSystemInfo()
        {
            try
            {
                var sb = new System.Text.StringBuilder();
                var sysdir = System.Environment.GetEnvironmentVariable("systemroot");

                if (sysdir != null)
                {
                    var systeminfoFile = System.IO.Path.Combine(sysdir, @"system32\systeminfo.exe");
                    if (File.Exists(systeminfoFile))
                    {
                        var psinfo = new System.Diagnostics.ProcessStartInfo
                        {
                            FileName = systeminfoFile,
                            UseShellExecute = false,
                            RedirectStandardOutput = true,
                            CreateNoWindow = true
                        };
                        var p = System.Diagnostics.Process.Start(psinfo);
                        var sr = p.StandardOutput;
                        sb.Append(sr.ReadToEnd());
                    }
                    sb.AppendLine();
                }
                sb.AppendLine("进程列表");
                foreach (System.Diagnostics.Process proc in
                    System.Diagnostics.Process.GetProcesses().OrderBy(p => p.ProcessName))
                {
                    try
                    {
                        sb.AppendLine(String.Format("{0,6} {1,-20} {2,12:n0} {3,4} {4,25}",
                                                    proc.Id,
                                                    proc.ProcessName,
                                                    proc.WorkingSet64,
                                                    proc.Threads.Count,
                                                    proc.StartTime
                                                    ));

                    }
                    catch (Exception ex)
                    {
                        sb.AppendLine(proc.ProcessName + " " + ex.Message);
                    }
                }

                //sb.AppendLine();
                //sb.AppendLine("Env vars:");
                //foreach (string envvar in
                //    (from string env in System.Environment.GetEnvironmentVariables().Keys
                //     orderby env
                //     select env))
                //{
                //    sb.AppendLine(envvar + "=" + System.Environment.GetEnvironmentVariable(envvar));
                //}
                return sb.ToString();
            }
            catch
            {
                return "unknow";
            }
            
        }
    }
}
