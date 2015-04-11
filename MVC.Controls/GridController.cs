using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using MVC.Controls.Grid;
using System.Linq.Expressions;

namespace MVC.Controls
{
    public abstract class GridController<TEntityType> :
        Controller, IGridController
        where TEntityType : class
    {
        private string _controllerName;
        private PropertyInfo _parentPrimaryKey;

        protected virtual string ControllerName
        {
            get
            {
                if (_controllerName == null)
                {
                    _controllerName = GetType().Name;
                    if (_controllerName.EndsWith("Controller"))
                    {
                        _controllerName = _controllerName.Substring(0, _controllerName.Length - "Controller".Length);
                    }
                }
                return _controllerName;
            }
        }


        [NonAction]
        public abstract GridColumnModelList<TEntityType> GetColumns();

        [NonAction]
        public IEnumerable<GridColumnModel> GetRawColumns()
        {
            List<GridColumnModel> columns = new List<GridColumnModel>();
            GetColumns().ForEach(col => columns.Add(col));
            return columns;
        }

        [NonAction]
        public abstract IQueryable<TEntityType> GetItems();

        public virtual JsonResult ListItems(SearchModel searchModel)
        {
            IQueryable<TEntityType> items = GetItems();

            GridData gridData =
                items
                    .ToGridData(searchModel);

            GridColumnModelList<TEntityType> columns = GetColumns();
            if (columns != null)
            {
                gridData
                    .UseValuesFromColumnExpressions(columns);
            }

            return Json(gridData, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Delete(TEntityType item)
        {
            try
            {
                ActionResult response = DeleteItem(item);
                if (response == null)
                {
                    response = Json(GridResponse.CreateSuccess(), JsonRequestBehavior.AllowGet);
                }
                return response;
            }
            catch (Exception e)
            {
                return Json(GridResponse.CreateFailure(e.ToString()), JsonRequestBehavior.AllowGet);
            }
        }

        [NonAction]
        public abstract ActionResult DeleteItem(TEntityType toDelete);

        [NonAction]
        public abstract ActionResult AddItem(TEntityType added, out string newid);

        [NonAction]
        public abstract ActionResult EditItem(TEntityType edited);

        /// <summary>
        /// Override this to remove validations that don't apply etc
        /// </summary>
        /// <param name="item"></param>        
        [NonAction]
        public virtual void BeforeAdd(TEntityType added)
        {
        }

        /// <summary>
        /// Override this to remove validations that don't apply etc
        /// </summary>
        /// <param name="item"></param>
        [NonAction]
        public virtual void BeforeEdit(TEntityType edited)
        {
        }

        // This is the external name of the web method that allows the grid to add/edit and delete items.
        public virtual ActionResult EditMethod(TEntityType item, string oper)
        {
            try
            {
                if (oper == "add")
                {
                    return InternalAdd(item);
                }
                else if (oper == "edit")
                {
                    return InternalEdit(item);
                }
                else if (oper == "del")
                {
                    return Delete(item);
                }
                else
                {
                    throw new InvalidOperationException("Unhandled oper: " + oper);
                }

            }
            catch (Exception e)
            {
                return Json(GridResponse.CreateFailure(e.ToString()), JsonRequestBehavior.AllowGet);
            }
        }

        [NonAction]
        private ActionResult InternalEdit(TEntityType item)
        {
            BeforeEdit(item);
            ActionResult response;
            if (ModelState.IsValid == false)
            {
                response = Json(GridResponse.Create(ModelState), JsonRequestBehavior.AllowGet);
            }
            else
            {
                response = EditItem(item);
                if (response == null)
                {
                    response = Json(GridResponse.CreateSuccess(), JsonRequestBehavior.AllowGet);
                }
            }

            return response;
        }

        [NonAction]
        private ActionResult InternalAdd(TEntityType item)
        {
            BeforeAdd(item);
            ActionResult response;
            if (ModelState.IsValid == false)
            {
                response = Json(GridResponse.Create(ModelState), JsonRequestBehavior.AllowGet);
            }
            else
            {
                string newid;
                response = AddItem(item, out newid);
                if (response == null)
                {
                    response = Json(GridResponse.CreateSuccess(newid), JsonRequestBehavior.AllowGet);
                }
            }

            return response;
        }

        [NonAction]
        protected string GetPrimaryKeyName()
        {
            return GetPrimaryKey(typeof(TEntityType)).Name;
        }

        [NonAction]
        protected virtual PropertyInfo GetPrimaryKey(Type type)
        {
            PropertyInfo pi =
                type
                    .GetProperties()
                    .FirstOrDefault(
                        member =>
                        {
                            if (GridControl.IsPrimaryKeyFunc != null)
                            {
                                return GridControl.IsPrimaryKeyFunc(member);
                            }
                            else
                            {
                                return AttributeHelper.HasMemberAttribute<KeyAttribute>(member);
                            }
                        });

            if (pi != null)
            {
                return pi;
            }
            else
            {
                throw new InvalidOperationException(
                        string.Format("{0} doesn't contain a primary key!",
                        type.Name));
            }
        }

        [NonAction]
        public virtual string GetEditUrl()
        {
            string url = GetAreaName() + "/" + ControllerName + "/EditMethod";
            if (url.StartsWith("~") == false)
            {
                url = "~" + url;
            }
            return VirtualPathUtility.ToAbsolute(url);
        }

        [NonAction]
        public virtual string GetListUrl()
        {
            string url = GetAreaName() + "/" + ControllerName + "/ListItems";
            if (url.StartsWith("~") == false)
            {
                url = "~" + url;
            }
            return VirtualPathUtility.ToAbsolute(url);
        }

        [NonAction]
        public virtual string GetAreaName()
        {
            return "";
        }

        [NonAction]
        public virtual string GetEditUrl(object parent)
        {
            PropertyInfo primaryKey = GetParentPrimaryKey(parent);
            return GetEditUrl() + "?" + primaryKey.Name + "=" + primaryKey.GetValue(parent, new object[] { }).ToString();
        }

        [NonAction]
        public virtual string GetListUrl(object parent)
        {
            PropertyInfo primaryKey = GetParentPrimaryKey(parent);
            return GetListUrl() + "?" + primaryKey.Name + "=" + primaryKey.GetValue(parent, new object[] { }).ToString();
        }

        [NonAction]
        protected virtual PropertyInfo GetParentPrimaryKey(object parent)
        {
            if (_parentPrimaryKey == null)
            {
                _parentPrimaryKey = GetPrimaryKey(parent.GetType());
            }
            return _parentPrimaryKey;
        }

        protected void RemoveModelStateErrorFor(Expression<Func<TEntityType, object>> expression)
        {
            ModelState.Remove(AttributeHelper.GetMemberName(expression));
        }
    }
}
