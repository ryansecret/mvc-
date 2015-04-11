using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MVC.Controls
{
    public class ProgressBar
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="id">Id of the element</param>
        public ProgressBar(string id)
        {
            this.Id = id;
        }

        /// <summary>
        /// Settings value of the progress bar.
        /// </summary>
        /// <param name="value">Value (in percentages)</param>
        /// <returns></returns>
        public ProgressBar SetValue(int value)
        {
            this.Value = value;
            return this;
        }

        /// <summary>
        /// Progress' value
        /// </summary>
        public int Value { get; set; }

        /// <summary>
        /// Id of the element
        /// </summary>
        public string Id { get; set; }

        public string Render()
        {
            string element = string.Format("<div id=\"{0}\"></div>",
                this.Id);


            string js = string.Format("_progressBarBinder(\"{0}\", {1});",
                           "#" + this.Id,
                           this.Value);

            return element + string.Format(MVCControlsScriptManager.SCRIPT_TEMPLATE, js);
        }

    }
}
