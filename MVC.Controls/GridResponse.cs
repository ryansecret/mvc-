using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace MVC.Controls
{
    public class GridResponse
    {
        public static List<object> CreateSuccess(string newId = null)
        {
            return new GridResponse(newId).ToArray();
        }

        public static List<object> Create(ModelStateDictionary modelStateDictionary)
        {
            return new GridResponse(modelStateDictionary).ToArray();
        }

        public static List<object> Create(Controller controller)
        {
            return new GridResponse(controller.ModelState).ToArray();
        }

        public static List<object> Create(bool success, string message = null, string newId = null)
        {
            return new GridResponse { Success = success, Message = message, NewId = newId }.ToArray();
        }

        public static List<object> CreateFailure(string message = null, string newId = null)
        {
            return Create(false, message, newId);
        }

        private GridResponse(string newId = null)
        {
            Success = true;
            NewId = newId;
        }

        private GridResponse(ModelStateDictionary modelStateDictionary)
            : this()
        {
            Success = modelStateDictionary.IsValid;
            List<string> errors = new List<string>();
            foreach (string key in modelStateDictionary.Keys)
            {
                ModelState modelState;
                if (modelStateDictionary.TryGetValue(key, out modelState))
                {
                    if (modelState.Errors.Any())
                    {
                        string error = string.Format("<b>{0}</b>: ", key);
                        error += string.Join(", ", modelState.Errors.Select(err => err.ErrorMessage));
                        errors.Add(error);
                    }
                }
            }

            Message = string.Join("<br />", errors);
        }

        public bool Success { get; set; }
        public string Message { get; set; }
        public string NewId { get; set; }
        public List<object> ToArray()
        {
            if (NewId != null)
            {
                return new List<object> { Success, Message, NewId };
            }
            else
            {
                return new List<object> { Success, Message };
            }
        }
    }
}
