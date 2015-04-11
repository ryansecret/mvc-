#region

using System;
using System.Runtime.InteropServices;

#endregion

namespace Hsr.Service.FileOperate
{
    public class UncAccessWithCredentials : IDisposable
    {
        private bool disposed;
        private int iLastError;
        private string sDomain;
        private string sPassword;

        private string sUNCPath;
        private string sUser;

        /// <summary>
        ///     返回最后系统错误（NetUseAdd 或 NetUseDel），成功返回空字符串
        ///     修改原方法，返回错误对应的描述
        /// </summary>
        public string LastError
        {
            get
            {
                if (iLastError == 0)
                {
                    return string.Empty;
                }

                string rtnMessage = string.Empty;
                switch (iLastError)
                {
                    case 67:
                        rtnMessage = "找不到网络名";
                        break;
                    case 2250:
                        rtnMessage = "此网络连接不存在";
                        break;
                    case 1219:
                        rtnMessage = "在同一台电脑上使用不同得凭据连接到同一共享资源";
                        break;
                    case 53:
                        rtnMessage = "网络路径不存在";
                        break;
                    default:
                        rtnMessage = "未知错误";
                        break;
                }
                return string.Format("{0}-（Win32 Error Code:{1}）。", rtnMessage, iLastError);
            }
        }

        public void Dispose()
        {
            if (!disposed)
            {
                //NetUseDelete();
            }
            disposed = true;
            GC.SuppressFinalize(this);
        }

        [DllImport("NetApi32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        internal static extern UInt32 NetUseAdd(
            String UncServerName,
            UInt32 Level,
            ref UseInfo2 Buf,
            out UInt32 ParmError);

        [DllImport("NetApi32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        internal static extern UInt32 NetUseDel(
            String UncServerName,
            String UseName,
            UInt32 ForceCond);

        /// <summary>
        ///     Connects to a UNC path using the credentials supplied.
        /// </summary>
        /// <param name="UNCPath">Fully qualified domain name UNC path</param>
        /// <param name="User">A user with sufficient rights to access the path.</param>
        /// <param name="Domain">Domain of User.</param>
        /// <param name="Password">Password of User</param>
        /// <returns>True if mapping succeeds.  Use LastError to get the system error code.</returns>
        public bool NetUseWithCredentials(string UNCPath, string User, string Domain, string Password)
        {
            sUNCPath = UNCPath;
            sUser = User;
            sPassword = Password;
            sDomain = Domain;
            return NetUseWithCredentials();
        }

        private bool NetUseWithCredentials()
        {
            uint returncode;
            try
            {
                var useinfo = new UseInfo2();

                useinfo.ui2_remote = sUNCPath;
                useinfo.ui2_username = sUser;
                useinfo.ui2_domainname = sDomain;
                useinfo.ui2_password = sPassword;
                useinfo.ui2_asg_type = 0;
                useinfo.ui2_usecount = 1;
                uint paramErrorIndex;
                returncode = NetUseAdd(null, 2, ref useinfo, out paramErrorIndex);
                iLastError = (int) returncode;
                returncode = 0;
                return returncode == 0;
            }
            catch
            {
                iLastError = Marshal.GetLastWin32Error();
                return false;
            }
        }

        /// <summary>
        ///     Ends the connection to the remote resource
        /// </summary>
        /// <returns>True if it succeeds.  Use LastError to get the system error code</returns>
        public bool NetUseDelete()
        {
            uint returncode;
            try
            {
                returncode = NetUseDel(null, sUNCPath, 2);
                iLastError = (int) returncode;
                return (returncode == 0);
            }
            catch
            {
                iLastError = Marshal.GetLastWin32Error();
                return false;
            }
        }

        ~UncAccessWithCredentials()
        {
            Dispose();
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        internal struct UseInfo2
        {
            internal String ui2_local;
            internal String ui2_remote;
            internal String ui2_password;
            internal UInt32 ui2_status;
            internal UInt32 ui2_asg_type;
            internal UInt32 ui2_refcount;
            internal UInt32 ui2_usecount;
            internal String ui2_username;
            internal String ui2_domainname;
        }
    }
}