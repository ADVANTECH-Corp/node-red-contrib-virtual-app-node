using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Reflection;
using System.IO;

namespace VAN_Demo
{
    public class EmbeddedDll
    {
        [DllImport("kernel32", SetLastError = true, CharSet = CharSet.Unicode)]
        static extern IntPtr LoadLibrary(string lpFileName);

        public static Assembly LoadDll(string dllname)
        {
            Assembly Target = Assembly.GetExecutingAssembly();
            string resourceName = string.Format("{0}.Resources.{1}.dll", Target.GetName().Name, dllname);
            byte[] assemblyData = null;

            // Managed
            using (Stream stream = Target.GetManifestResourceStream(resourceName))
            {
                assemblyData = new byte[stream.Length];
                stream.Read(assemblyData, 0, assemblyData.Length);

                try
                {
                    return Assembly.Load(assemblyData);
                }
                catch
                {
                    // Purposely do nothing
                    // Unmanaged dll or assembly cannot be loaded directly from byte[]
                    // Let the process fall through for next part
                }
            }

            // Unmanaged
            bool fileOk = false;
            string tempFile = Path.GetTempPath() + dllname + ".dll";

            if (File.Exists(tempFile))
            {
                byte[] bb = File.ReadAllBytes(tempFile);

                using (SHA1CryptoServiceProvider sha1 = new SHA1CryptoServiceProvider())
                {
                    string fileHash = BitConverter.ToString(sha1.ComputeHash(assemblyData)).Replace("-", string.Empty); ;
                    string fileHash2 = BitConverter.ToString(sha1.ComputeHash(bb)).Replace("-", string.Empty);

                    fileOk = (fileHash == fileHash2);
                }
            }

            if (!fileOk)
            {
                File.WriteAllBytes(tempFile, assemblyData);
            }

            //return Assembly.LoadFile(tempFile);
            LoadLibrary(tempFile);
            return null;
        }
    }
}
