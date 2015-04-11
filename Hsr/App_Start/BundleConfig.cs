using System;
using System.Linq;
using System.Web.Optimization;

namespace Hsr.App_Start
{
    public class BundleConfig
    {
        // 有关 Bundling 的详细信息，请访问 http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.IgnoreList.Clear();
            AddDefaultIgnorePatterns(bundles.IgnoreList);
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            
            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        //"~/Scripts/jquery.unobtrusive*",
                        "~/Scripts/jquery.validate*"));

            // 使用 Modernizr 的开发版本进行开发和了解信息。然后，当你做好
            // 生产准备时，请使用 http://modernizr.com 上的生成工具来仅选择所需的测试。
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));
            bundles.Add(new ScriptBundle(JsTree).Include("~/Scripts/JsTree/jstree.js"));

           

            bundles.Add(new StyleBundle(LoginStylePath).Include("~/Content/Login/normalize.css").Include("~/Content/Login/style.css"));
          
            bundles.Add(new StyleBundle(JsTreeCss).Include("~/Content/JsTree/style.css"));

            bundles.Add(new ScriptBundle(Md5ScriptPath).Include("~/Scripts/jsmd5.js"));

            bundles.Add(new ScriptBundle(MvcControlScriptPath).Include("~/Scripts/MVCControls/Grid/grid.locale-cn.js").
                Include("~/Scripts/MVCControls/Grid/jquery.jqGrid.src.js").
                Include("~/Scripts/MVCControls/Grid/grid.jqueryui.js").
                Include("~/Scripts/MVCControls/mvc.controls.manager.js"));



            bundles.Add(new StyleBundle(MvcControlStylePath).Include("~/Content/MVCControls/mvc.controls.css").
                Include("~/Content/MVCControls/Grid/ui.jqgrid.css"));

       

            bundles.Add(new StyleBundle("~/Content/hint").Include("~/Content/base/hint.css"));

  
            bundles.Add(new ScriptBundle(MusachesJsPath)
                .Include("~/Scripts/mustache.js")
                .Include("~/Scripts/handlebars.js"));

          
            bundles.Add(new ScriptBundle(ChartJsPath).Include("~/Scripts/Chart.js"));
            bundles.Add(new ScriptBundle(MetaDataJsPath).Include("~/Scripts/jquery.metadata.js"));
            bundles.Add(new ScriptBundle(SingleFileUpLoadJsPath).Include("~/Scripts/UpLoad/SingleFileUpLoad.js"));

            bundles.Add(new ScriptBundle(MessageJsPath)
                .Include("~/Scripts/Message/messenger.js")
                .Include("~/Scripts/Message/messenger-theme-flat.js")
                .Include("~/Scripts/Common.js"));

            bundles.Add(new StyleBundle(MessageStylePath)
                .Include("~/Content/Message/messenger.css")
                .Include("~/Content/Message/messenger-theme-flat.css"));

            bundles.Add(new ScriptBundle(UiJs).Include("~/Scripts/bootstrap.js"));
            bundles.Add(new StyleBundle(UiCss).Include("~/Content/bootstrap.css"));
            bundles.Add(new ScriptBundle(TypeAheadJs).Include("~/Scripts/typeahead.bundle.js"));
            bundles.Add(new ScriptBundle(WizardJs).Include("~/Scripts/wizard.js"));

            bundles.Add(new ScriptBundle(ModernizrJs).Include("~/Scripts/modernizr.custom.js"));
            bundles.Add(new ScriptBundle(DatePickerJs).Include("~/Scripts/DatePicker/bootstrap-datepicker.js"));
            bundles.Add(new StyleBundle(DatePikerCss).Include("~/Content/DatePicker/bootstrap-datepicker.min.css"));
             
            bundles.Add(new ScriptBundle(ComTestPlanJs).Include("~/Scripts/System/comtestPlan.js")); 

            MobileBundle.RegisterBundles(bundles);
        }
        public static void AddDefaultIgnorePatterns(IgnoreList ignoreList)
        {
            if (ignoreList == null)
                throw new ArgumentNullException("ignoreList");
            ignoreList.Ignore("*.intellisense.js");
            ignoreList.Ignore("*-vsdoc.js");
            ignoreList.Ignore("*.debug.js", OptimizationMode.WhenEnabled);
            ignoreList.Ignore("*.min.js", OptimizationMode.Always);
            //ignoreList.Ignore("*.min.css", OptimizationMode.Always);
        }
        public const string WizardJs = "~/bundles/WizardJs";
        public const string TypeAheadJs = "~/bundles/TypeAheadJs";
        public const string UiJs = "~/bundles/UIjs";
        public const string UiCss = "~/Content/UICss";

        public const string JsTree = "~/bundles/jstree";
        public const string JsTreeCss = "~/Content/jstreeCss";
        public const string DatePickerJs = "~/bundles/datePickerJs";
        public const string DatePikerCss = "~/Content/datePickerCss";
        
      
        public const string LoginStylePath = "~/bundles/login";
        public const string Md5ScriptPath = "~/bundles/md5";

        public const string MvcControlScriptPath = "~/bundles/mvcControl";
        public const string MvcControlStylePath = "~/Content/mvcControl";

    

        public const string MusachesJsPath = "~/bundles/Mustache";
       

        public const string ChartJsPath = "~/bundles/ChartJs";

        public const string MetaDataJsPath = "~/bundles/MetaDataJs";

        public const string SingleFileUpLoadJsPath = "~/bundles/SingleFileUpLoadJs";

        public const string MessageJsPath = "~/bundles/MessageJs";
        public const string MessageStylePath = "~/Content/MessageCss";

        public const string ModernizrJs = "~/bundles/ModernizrJs";

        public const string ComTestPlanJs = "~/bundles/ComTestPlanJs";
    }

    public class MobileBundle
    {
        public const string JmDatepickerScriptPath = "~/bundles/Datepicker";
        public const string JmDatepickerStylePath = "~/Content/Datepicker";

        public const string JmFilterScriptPath = "~/bundles/JmFilter";
        public const string JmFilterStylePath = "~/Content/JmFilter";
        public const string HsrSelectMenu = "~/bundles/HsrSelectMenu";
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new StyleBundle("~/Content/css").Include("~/Content/Mobile/site.css")
           );
            bundles.Add(new ScriptBundle("~/bundles/jqueryMobile").Include(
                  "~/Scripts/Mobile/jquery.mobile-{version}.js"));
            bundles.Add(new ScriptBundle(HsrSelectMenu).Include("~/Scripts/System/SelectMenu.js"));
            bundles.Add(new ScriptBundle(JmDatepickerScriptPath)
                            .Include("~/Scripts/JmDatepicker/external/jquery-ui/datepicker.js")
                            .Include("~/Scripts/JmDatepicker/jquery.mobile.datepicker.js"));

            bundles.Add(new StyleBundle(JmDatepickerStylePath)
                .Include("~/Scripts/JmDatepicker/jquery.mobile.datepicker.css")
                .Include("~/Scripts/JmDatepicker/jquery.mobile.datepicker.theme.css"));





            bundles.Add(new ScriptBundle(JmFilterScriptPath)
                          .Include("~/Scripts/Mobile/JmFilterList/JmFilter.js"));

            bundles.Add(new StyleBundle(JmDatepickerStylePath)
                .Include("~/Scripts/Mobile/JmFilterList/JmFilter.css"));

            bundles.Add(new StyleBundle("~/Content/Mobile/themes/base/css")
                .Include("~/Content/Mobile/site.css")
                .Include("~/Content/Mobile/themes/jquery.mobile.theme.css")
                .Include("~/Content/Mobile/themes/jquery.mobile-{version}.css")
                .Include("~/Content/Mobile/themes/jquery.mobile.icons-{version}.css"));

            bundles.Add(new StyleBundle("~/Content/Validator").Include("~/Content/Mobile/JmGather/Validator.css"));
     
            bundles.Add(new StyleBundle("~/Content/Loading").Include("~/Content/Mobile/JmGather/Loading.css"));
            #region

            bundles.Add(new ScriptBundle("~/bundles/sysRole").Include("~/Scripts/System/sysRole.js"));
            bundles.Add(new ScriptBundle("~/bundles/dateTime").Include("~/Scripts/My97DatePicker/WdatePicker.js"));
            bundles.Add(new ScriptBundle("~/bundles/organizeInfo").Include("~/Scripts/System/organizeInfo.js"));
            bundles.Add(new ScriptBundle("~/bundles/menu").Include("~/Scripts/System/menu.js"));
            

            bundles.Add(new ScriptBundle("~/bundles/usersummary").Include("~/Scripts/System/usersummaryinfo.js"));
            bundles.Add(new ScriptBundle("~/bundles/Comp").Include("~/Scripts/System/CompTree.js"));
            bundles.Add(new ScriptBundle("~/bundles/Dept").Include("~/Scripts/System/DeptList.js"));
            bundles.Add(new ScriptBundle("~/bundles/RoleTree").Include("~/Scripts/System/RoleTree.js"));
            bundles.Add(new ScriptBundle("~/bundles/Authority").Include("~/Scripts/System/Authority.js"));
            bundles.Add(new ScriptBundle("~/bundles/Navigation").Include("~/Scripts/System/Navigation.js"));
            bundles.Add(new ScriptBundle("~/bundles/LoadAnima").Include("~/Scripts/System/LoadAnima.js"));

            #endregion
        }
      }
}