using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MVC.Controls
{
    public class DatePicker
    {
        /// <summary>
        /// Creates a DatePicker control behaviour
        /// </summary>
        /// <param name="onSelectCallBack">onSelect js event handler</param>
        /// <param name="onBeforeShowCallBack">onBeforeShow js event handler</param>
        /// <param name="onCloseCallBack">onClose js event handler</param>
        /// <param name="addtionalParameters">additional unmapped parameters</param>
        /// <param name="isRTL">control alignment, defaults to no</param>
        /// <param name="showOn">when should the date picker be shown: focos (control focus) or button (show a dedicated button)</param>
        /// <param name="dateFormat">the format of the date</param>
        /// <param name="buttonText">if showOn=button, sets the text of the dedicated button</param>
        /// <param name="buttonImage">if showOn=button, sets the background image of the dedicated button</param>
        /// <param name="buttonImageOnly">if showOn=button, sets the dedicated button to be image-only</param>
        /// <param name="maxDate">sets the max date</param>
        /// <param name="minDate">sets the min date</param>
        public DatePicker(string onSelectCallBack = null,
                          string onBeforeShowCallBack = null,
                          string onCloseCallBack = null,
                          string addtionalParameters = null,
                          bool isRTL = false,
                          string showOn = "focus",
                          string dateFormat = "yy-mm-dd",
                          string buttonText = null,
                          string buttonImage = null,
                          bool buttonImageOnly = false,
                          string maxDate = null,
                          string minDate = null)
        {
            if (onSelectCallBack != null) OnSelectCallBack = onSelectCallBack;
            if (onBeforeShowCallBack != null) OnBeforeShowCallBack = onBeforeShowCallBack;
            if (onCloseCallBack != null) OnCloseCallBack = onCloseCallBack;
            if (addtionalParameters != null) AdditionalParameters = addtionalParameters;
            if (isRTL != null) IsRTL = isRTL;
            if (showOn != null) ShowOn = showOn;
            if (dateFormat != null) DateFormat = dateFormat;
            if (buttonImage != null) ButtonImage = buttonImage;
            if (buttonText != null) ButtonText = buttonText;
            if (buttonImageOnly != null) ButtonImageOnly = buttonImageOnly;
            if (minDate != null) MinDate = minDate;
            if (maxDate != null) MaxDate = maxDate;
        }
                          

        private Dictionary<string, string> _attributes = new Dictionary<string,string>();

        /// <summary>
        /// Sets an onselect js callback with the following prototype: fn(dateText,inst)
        /// </summary>
        public string OnSelectCallBack
        {
            get { return _attributes["onSelect"]; }
            set {_attributes["onSelect"] = value; }
        }

        /// <summary>
        /// Sets an onselect js callback with the following prototype: fn(dateText,inst)
        /// </summary>
        public string OnBeforeShowCallBack
        {
            get { return _attributes["beforeShow"]; }
            set { _attributes["beforeShow"] = value; }
        }

        /// <summary>
        /// Sets an onselect js callback with the following prototype: fn(dateText,inst)
        /// </summary>
        public string OnCloseCallBack
        {
            get { return _attributes["onClose"]; }
            set { _attributes["onClose"] = value; }
        }

        /// <summary>
        /// Can be used to add additional parameters not conifgurable using the predefined properties
        /// Format: paramName:\"paramValue\",paramName:\"paramValue\"
        /// </summary>
        public string AdditionalParameters { get; set; }

        /// <summary>
        /// Define when the calendar is visible:
        /// focus - When the field recieve focus
        /// button - A dedicated button
        /// </summary>
        public string ShowOn
        {
            get { return getString("showOn"); }
            set {setString("showOn",value); }
        }

        /// <summary>
        /// The date format, i.e: 'yy-mm-dd' 
        /// </summary>
        public string DateFormat
        {
            get { return getString("dateFormat"); }
            set {setString("dateFormat",value); }
        }

        /// <summary>
        /// Whether or not the text alignment should be rtl
        /// </summary>
        public bool IsRTL
        {
            get 
            {
                if (_attributes.ContainsKey("isRTL"))
                    return bool.Parse(_attributes["isRTL"]);
                else return false; 
            }
            set {_attributes["isRTL"] = value.ToString().ToLower(); }
        }

        /// <summary>
        /// The button's text caption
        /// Setting this property will automatically set {showOn=button
        /// </summary>
        public string ButtonText
        {
            get { return getString("buttonText"); }
            set {setString("buttonText",value); this.ShowOn = "button"; }
        }

        /// <summary>
        /// The url to the image for the button
        /// Setting this property will automatically set {showOn=button
        /// </summary>
        public string ButtonImage
        {
            get { return getString("buttonImage"); }
            set {setString("buttonImage",value); this.ShowOn = "button"; }
        }

        /// <summary>
        /// Whether the button should be image only
        /// Setting this property will automatically set {showOn=button
        /// </summary>
        public bool? ButtonImageOnly
        {
            get
            {
                if (_attributes.ContainsKey("buttonImageOnly"))
                    return bool.Parse(_attributes["buttonImageOnly"]);
                else return false;
            }
            set {_attributes["buttonImageOnly"] = value.ToString().ToLower(); }
        }

        /// <summary>
        /// The maximum date compared to today, i.e: +1m +1w = month and week max, or a static date
        /// </summary>
        public string MaxDate
        {
            get { return getString("maxDate"); }
            set {setString("maxDate",value); }
        }

        /// <summary>
        /// The minimum date compared to today, i.e: -1m -1w = month and week max, or a static date
        /// </summary>
        public string MinDate
        {
            get { return getString("minDate"); }
            set {setString("minDate",value); }
        }

        /// <summary>
        /// Renders the DatePicker extension for the specified control
        /// </summary>
        /// <param name="id">The control's html-id</param>
        /// <returns></returns>
        public string Render(string id)
        {
            StringBuilder sb = new StringBuilder();


          

            foreach (string key in _attributes.Keys)
                sb.AppendFormat(",{0}:{1}", key, _attributes[key]);

            if (!string.IsNullOrEmpty(this.AdditionalParameters))
                sb.AppendFormat(",{0}", this.AdditionalParameters);
            
            if (sb.Length > 0)
                sb.Remove(0, 1);

            sb.Append("});");

            sb.Insert(0, "$(\"#" + id + "\").datepicker({");

            var tem = string.Format(MVCControlsScriptManager.SCRIPT_TEMPLATE, sb.ToString());

            return string.Format(MVCControlsScriptManager.SCRIPT_TEMPLATE, sb.ToString());
        }

        private string getString(string key)
        {
            if (!_attributes.ContainsKey(key)) return null;
            string temp = _attributes[key];
            return temp.Substring(1, temp.Length - 2);
        }

        private void setString(string key, string value)
        {
            if (string.IsNullOrEmpty(value))
                _attributes[key] = null;
            else
                _attributes[key] = "\"" + value + "\"";
        }
    }
}
