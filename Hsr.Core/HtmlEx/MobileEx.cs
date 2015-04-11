using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Web.Mvc;
using System.Web.Mvc.Html;
namespace Hsr.Core.HtmlEx
{
    public static class MobileEx
    {
        public static MvcHtmlString FlipSwichFor<TModel, TValue>(this HtmlHelper<TModel> html,
                                                                 Expression<Func<TModel, TValue>> expression,
                                                                 string onText = "是", string offText = "否",
                                                                 bool isChecked = false, string onValue = "true",
                                                                 string offValue = "false")
        {
            //html.ActionLink()
            string htmlFieldName = ExpressionHelper.GetExpressionText(expression);
            ModelMetadata metadata = ModelMetadata.FromLambdaExpression(expression, html.ViewData);
            string resolvedLabelText = metadata.DisplayName ??
                                       (metadata.PropertyName ?? htmlFieldName.Split(new[] {'.'}).Last());
            var builder = new TagBuilder("div");
            var tagLable = new TagBuilder("label");
            tagLable.Attributes.Add("for",
                                    TagBuilder.CreateSanitizedId(
                                        html.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldId(htmlFieldName)));
            resolvedLabelText = String.Concat(resolvedLabelText, ":");
            tagLable.SetInnerText(resolvedLabelText);
            var tagCheckBox = new TagBuilder("input");
            tagCheckBox.Attributes.Add("type", "checkbox");
            tagCheckBox.Attributes.Add("data-role", "flipswitch");
            tagCheckBox.Attributes.Add("name", htmlFieldName);
            tagCheckBox.Attributes.Add("value", onValue);
            tagCheckBox.Attributes.Add("data-mini", "true");
            tagCheckBox.Attributes.Add("data-on-text", onText);
            tagCheckBox.Attributes.Add("data-off-text", offText);
            if (isChecked)
            {
                tagCheckBox.Attributes.Add("checked", "checked");
            }
            var tagHidden = new TagBuilder("input");
            tagHidden.Attributes.Add("type", "hidden");
            tagHidden.Attributes.Add("value", offValue);
            tagHidden.Attributes.Add("name", htmlFieldName);

            builder.AddCssClass("ui-field-contain");


            builder.InnerHtml = string.Concat(tagLable.ToString(), tagCheckBox.ToString(), tagHidden.ToString());
            return MvcHtmlString.Create(builder.ToString());
        }

        public static MvcHtmlString SelectMenu<TModel, TValue>(this HtmlHelper<TModel> html,
                                                               Expression<Func<TModel, TValue>> expression, string id,
                                                               List<SelectListItem> values, bool iconpos = true,
                                                               bool multiple = true)
        {
           
            string htmlFieldName = ExpressionHelper.GetExpressionText(expression);
            ModelMetadata metadata = ModelMetadata.FromLambdaExpression(expression, html.ViewData);
            var builder = new TagBuilder("div");
            var tagLable = new TagBuilder("lable");
            var fullName = TagBuilder.CreateSanitizedId(
                html.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldId(htmlFieldName));
            tagLable.Attributes.Add("for", fullName);
            var tagSelect = new TagBuilder("select");
            tagSelect.Attributes.Add("name", htmlFieldName);

            if (multiple)
            {
                tagSelect.Attributes.Add("multiple", "multiple");
            }
            tagSelect.Attributes.Add("id", fullName);
            if (iconpos)
            {
                tagSelect.Attributes.Add("data-iconpos", "left");
            }
            else
            {
                tagSelect.Attributes.Add("data-iconpos", "right");
            }

            var str = new StringBuilder();
            if (values != null)
            {
                foreach (SelectListItem value in values)
                {
                    var op = new StringBuilder();
                    string isSelected = "";
                    if (value.Selected)
                    {
                        isSelected = "Selected";
                    }
                    op.AppendFormat("<option value='{1}' {0}>" + value.Text + "</option>", isSelected, value.Value);
                    str.Append(op);
                }
            }

            ModelState modelState;
            if (html.ViewData.ModelState.TryGetValue(htmlFieldName, out modelState))
            {
                if (modelState.Errors.Count > 0)
                {
                    tagSelect.AddCssClass(HtmlHelper.ValidationInputCssClassName);
                }
            }
            tagSelect.MergeAttributes(html.GetUnobtrusiveValidationAttributes(htmlFieldName, metadata)); 

            tagSelect.InnerHtml = str.ToString();
            builder.InnerHtml = string.Concat(tagLable.ToString(), tagSelect.ToString());

            return MvcHtmlString.Create(builder.ToString());
        }      
    }
}