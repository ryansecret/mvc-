using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Collections;

using System.Linq.Dynamic;
using System.Reflection;

namespace MVC.Controls
{
    internal class SearchScriptSerializer : JavaScriptConverter
    {
        public override IEnumerable<Type> SupportedTypes
        {
            get { return new List<Type>() { typeof(SearchModel) }; }
        }

        public override object Deserialize(IDictionary<string, object> dictionary, Type type, JavaScriptSerializer serializer)
        {
            string val;
            //groupOp
            SearchModel res = new SearchModel();
            
            val = (string)dictionary["groupOp"];
            if (val == "AND")
                res.ConjunctionOp = ConjunctionOperator.AND;
            else
                res.ConjunctionOp = ConjunctionOperator.OR;

            if (dictionary["rules"] != null)
            {
                res.Filters = new List<FilterModel>();
                IEnumerable rulesList = dictionary["rules"] as IEnumerable;
                foreach (IDictionary<string, object> rawRule in rulesList)
                {
                    FilterModel f = new FilterModel();
                    f.Value = (string)rawRule["data"];
                    f.SetOp((string)rawRule["op"]);
                    f.ColumnName = (string)rawRule["field"];
                    res.Filters.Add(f);
                }
            }

            return res;
        }

        public override IDictionary<string, object> Serialize(object obj, JavaScriptSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }


    public class SearchModelBinder : DefaultModelBinder
    {

        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            SearchModel model = new SearchModel();
            bool search = false;

            search = (bool)bindingContext.ValueProvider.GetValue("_search").ConvertTo(typeof(Boolean));

            if (search)
            {
                ValueProviderResult filters = bindingContext.ValueProvider.GetValue("filters");
                if (filters != null)
                {
                    JavaScriptSerializer serializer = new JavaScriptSerializer();
                    serializer.RegisterConverters(new JavaScriptConverter[] { new SearchScriptSerializer() });
                    model = serializer.Deserialize<SearchModel>((string)filters.ConvertTo(typeof(String)));
                }
                else
                {
                    FilterModel filter = new FilterModel();

                    filter.ColumnName = (string)bindingContext.ValueProvider.GetValue("searchField").ConvertTo(typeof(String));
                    filter.Value = (string)bindingContext.ValueProvider.GetValue("searchString").ConvertTo(typeof(String));
                    filter.SetOp((string)bindingContext.ValueProvider.GetValue("searchOper").ConvertTo(typeof(String)));
                    
                    model.Filters.Add(filter);
                }
            }

            model.Page = (int)bindingContext.ValueProvider.GetValue("page").ConvertTo(typeof(Int32));
            model.Rows = (int)bindingContext.ValueProvider.GetValue("rows").ConvertTo(typeof(Int32));
            model.SortColumnName = (string)bindingContext.ValueProvider.GetValue("sidx").ConvertTo(typeof(String));
            model.SortOrder = (string)bindingContext.ValueProvider.GetValue("sord").ConvertTo(typeof(string));

            //model.PageIndex = (int)bindingContext.ValueProvider.GetValue("page").ConvertTo(typeof(Int32)) - 1;
            //model.RecordsCount = (int)bindingContext.ValueProvider.GetValue("rows").ConvertTo(typeof(Int32));

            return model;
        }

    }



    public enum ConjunctionOperator
    {
        AND,
        OR
    }



    [ModelBinder(typeof(SearchModelBinder))]
    public class SearchModel
    {
        public ConjunctionOperator ConjunctionOp { get; set; }
        public List<FilterModel> Filters { get; set; }

        public SearchModel()
        {
            this.Filters = new List<FilterModel>();
        }

        public bool IsSearch
        {
            get
            {
                return this.Filters.Count > 0;
            }
        }

        public int Page { get; set; }
        public int Rows { get; set; }
        public string SortColumnName { get; set; }
        public string SortOrder { get; set; }

        /// <summary>
        /// Applies the filters and orderings from the grid to the IQueryable datasource
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="modelQuery"></param>
        /// <returns></returns>
        public IQueryable<T> ApplyFilters<T>(IQueryable<T> modelQuery)
        {
            IQueryable<T> res = modelQuery;

            if (this.Filters.Count > 0)
            {
                string temp = this.FiltersToString(typeof(T));
                res = modelQuery.Where(temp);
            }

            if (!string.IsNullOrEmpty(this.SortColumnName))
                res = res.OrderBy(this.SortColumnName + " " + this.SortOrder);

            return res;
        }

        public string FiltersToString(Type entityType)
        {
            if (this.Filters == null) return "";

            string conOp = " && ";
            if (ConjunctionOp == ConjunctionOperator.OR) conOp = " || ";

            StringBuilder sb = new StringBuilder();

            /* Enuerate through all the filters to generate a complete Where statement */
            foreach (FilterModel f in this.Filters)
            {

                /* Since a column could be bound to a child property (e.g City.Id), the property discovery is recursive */
                IEnumerable<PropertyInfo> query;
                Type columnType = entityType;

                string[] propertyHeirarch = f.ColumnName.Split('.');

                foreach (string prop in propertyHeirarch)
                {
                    PropertyInfo[] props = columnType.GetProperties();

                    query = from p in props
                            where p.Name == prop
                            select p;

                    columnType = query.First().PropertyType;
                }


                sb.Append(conOp + f.ToString(columnType));
            }

            if (sb.Length > 0) sb.Remove(0, 4);

            return sb.ToString();
        }
    }

    public class FilterModel
    {
        public string ColumnName { get; set; }
        public FilterOp Op { get; set; }
        public string Value { get; set; }

        public string ToString(Type columnType)
        {
            string newValue;
            string columnPrefix = "";

            if (isNumericType(columnType))
                newValue = this.Value;
            else
            {
                if (columnType != typeof(string))
                    columnPrefix = ".ToString()";

                newValue = "\"" + this.Value + "\"";
            }
            //TODO: Check type
            switch (this.Op)
            {
                case FilterOp.BeginWith: return this.ColumnName + columnPrefix + ".StartsWith(" + newValue + ")";
                case FilterOp.Contain: return this.ColumnName + columnPrefix + ".Contains(" + newValue + ")";
                case FilterOp.EndWith: return this.ColumnName + columnPrefix + ".EndsWith(" + newValue + ")";
                case FilterOp.Equal: return this.ColumnName + columnPrefix + "==" + newValue;
                case FilterOp.NotContain: return this.ColumnName + columnPrefix + ".Contains(" + newValue + ")==false";
                case FilterOp.Greater: return this.ColumnName + ">" + newValue;
                case FilterOp.GreaterOrEqual: return this.ColumnName + ">=" + newValue;
                case FilterOp.In: return newValue + ".Contains(" + this.ColumnName + columnPrefix + ")"; //TODO: Test
                case FilterOp.Less: return this.ColumnName + "<" + newValue;
                case FilterOp.LessOrEqual: return this.ColumnName + "<=" + newValue;
                case FilterOp.NotBeginWith: return this.ColumnName + columnPrefix + ".StartWith(" + newValue + ")==false";
                case FilterOp.NotIn: return newValue + ".Contains(" + this.ColumnName + columnPrefix + ")==false"; //TODO: Test
                case FilterOp.NotEndWith: return this.ColumnName + columnPrefix + ".EndsWith(" + newValue + ")==false";
                case FilterOp.NotEqual: return this.ColumnName + columnPrefix + "!=" + newValue;
                default: return "";
            }
        }

        public void SetOp(string opName)
        {
            switch (opName)
            {
                case "eq": this.Op = FilterOp.Equal; break;
                case "ne": this.Op = FilterOp.NotEqual; break;
                case "lt": this.Op = FilterOp.Less; break;
                case "le": this.Op = FilterOp.LessOrEqual; break;
                case "gt": this.Op = FilterOp.Greater; break;
                case "ge": this.Op = FilterOp.GreaterOrEqual; break;
                case "bw": this.Op = FilterOp.BeginWith; break;
                case "bn": this.Op = FilterOp.NotBeginWith; break;
                case "in": this.Op = FilterOp.In; break;
                case "ni": this.Op = FilterOp.NotIn; break;
                case "ew": this.Op = FilterOp.EndWith; break;
                case "en": this.Op = FilterOp.NotEndWith; break;
                case "cn": this.Op = FilterOp.Contain; break;
                case "nc": this.Op = FilterOp.NotContain; break;
                default: throw new Exception("FilterOp " + opName + " Unknown");
            }
        }

        private bool isNumericType(Type t)
        {
            string name = t.Name;
            switch (name)
            {
                case "Int16": return true;
                case "Int32": return true;
                case "Int64": return true;
                case "UInt16": return true;
                case "UInt32": return true;
                case "UInt64": return true;
                case "Double": return true;
                case "Single": return true;
            }

            return false;
        }
    }

    public enum FilterOp
    {
        Equal, NotEqual, Less, LessOrEqual, Greater, GreaterOrEqual,
        BeginWith, NotBeginWith, In, NotIn, EndWith, NotEndWith,
        Contain, NotContain
    }
}
