using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MVC.Controls
{
    public enum ContentType
    {
        /// <summary>
        /// Means the content is rendered using a static string of HTML content
        /// </summary>
        Static,

        /// <summary>
        /// Means the content is rendered using the PartialView MVC mechanism
        /// </summary>
        PartialView,

        /// <summary>
        /// Means the content is rendered on-demand using an ajax request to a remote controller action
        /// </summary>
        RemoteAction
    }
}
