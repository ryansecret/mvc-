using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MVC.Controls
{
    public class MVCControlsScriptManager
    {
        internal static readonly string SCRIPT_TEMPLATE = "<script language=\"javascript\" type=\"text/javascript\">$(function (){{{0}}})</script>";

        private static MVCControlsScriptManager _instance = null;
        private static object _lockKey = new object();

        private bool _useJqCustom = true;
        private string _version = "2.1.1";
        private Dictionary<string, string> _scripts = new Dictionary<string, string>();

        private MVCControlsScriptManager()
        {
        }


     
        public static MVCControlsScriptManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_lockKey)
                    {
                        _instance = new MVCControlsScriptManager();
                    }
                }

                return _instance;
            }
        }


      

    }
}
