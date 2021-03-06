﻿using System.Web.Optimization;
using DM.AbpZeroTemplate.Web.Bundling;

namespace DM.AbpZeroTemplate.Web.App.Startup
{
    public static class AppBundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            //LIBRARIES

            AddAppCssLibs(bundles, isRTL: false);
            AddAppCssLibs(bundles, isRTL: true);

            bundles.Add(
                new ScriptBundle("~/Bundles/App/libs/js")
                    .Include(
                        ScriptPaths.Json2,
                        ScriptPaths.JQuery,
                        ScriptPaths.JQuery_Migrate,
                        ScriptPaths.Bootstrap,
                        ScriptPaths.Bootstrap_Hover_Dropdown,
                        ScriptPaths.JQuery_Slimscroll,
                        ScriptPaths.JQuery_BlockUi,
                        ScriptPaths.JQuery_Cookie,
                        ScriptPaths.JQuery_Uniform,
                        ScriptPaths.SignalR,
                        ScriptPaths.Morris,
                        ScriptPaths.Morris_Raphael,
                        ScriptPaths.JQuery_Sparkline,
                        ScriptPaths.JQuery_Color,
                        ScriptPaths.JQuery_Jcrop,
                        ScriptPaths.JsTree,
                        ScriptPaths.Bootstrap_Switch,
                        ScriptPaths.SpinJs,
                        ScriptPaths.SpinJs_JQuery,
                        ScriptPaths.SweetAlert,
                        ScriptPaths.Toastr,
                        ScriptPaths.MomentJs,
                        ScriptPaths.QRCodeJs,
                        ScriptPaths.QRCodeUTF8Js,
                        ScriptPaths.Bootstrap_DateRangePicker,
                        ScriptPaths.Bootstrap_DateTimePicker,
                        ScriptPaths.Bootstrap_Select,
                        ScriptPaths.Underscore,
                        ScriptPaths.Angular,
                        ScriptPaths.Angular_Sanitize,
                        ScriptPaths.Angular_Touch,
                        ScriptPaths.Angular_Ui_Router,
                        ScriptPaths.Angular_Ui_Utils,
                        ScriptPaths.Angular_Ui_Bootstrap_Tpls,
                        ScriptPaths.Angular_Ui_Grid,
                        ScriptPaths.Angular_OcLazyLoad,
                        ScriptPaths.Angular_File_Upload,
                        ScriptPaths.Angular_DateRangePicker,
                        ScriptPaths.Angular_DateTimePicker,
                        ScriptPaths.Angular_Moment,
                        ScriptPaths.Angular_Bootstrap_Switch,


                        ScriptPaths.Angular_Editor_SimditorAll,
                        ScriptPaths.Angular_Editor_Uploader,
                        ScriptPaths.Angular_Editor,


                        ScriptPaths.Angular_QRCode,

                        ScriptPaths.Angular_Simple_Logger,
                        ScriptPaths.Angular_Google_Maps,

                        ScriptPaths.Abp,
                        ScriptPaths.Abp_JQuery,
                        ScriptPaths.Abp_Toastr,
                        ScriptPaths.Abp_BlockUi,
                        ScriptPaths.Abp_SpinJs,
                        ScriptPaths.Abp_SweetAlert,
                        ScriptPaths.Abp_Angular
                    ).ForceOrdered()
                );

            //METRONIC

            AddAppMetrinicCss(bundles, isRTL: false);
            AddAppMetrinicCss(bundles, isRTL: true);

            bundles.Add(
              new ScriptBundle("~/Bundles/App/metronic/js")
                  .Include(
                      "~/metronic/assets/global/scripts/app.js",
                      "~/metronic/assets/admin/layout4/scripts/layout.js"
                  ).ForceOrdered()
              );

            //APPLICATION

            bundles.Add(
                new StyleBundle("~/Bundles/App/css")
                    .IncludeDirectory("~/App", "*.css", true)
                    .ForceOrdered()
                );

            bundles.Add(
                new ScriptBundle("~/Bundles/App/js")
                    .IncludeDirectory("~/App", "*.js", true)
                    .ForceOrdered()
                );
        }

        private static void AddAppCssLibs(BundleCollection bundles, bool isRTL)
        {
            bundles.Add(
                new StyleBundle("~/Bundles/App/libs/css" + (isRTL ? "RTL" : ""))
                    .Include(StylePaths.FontAwesome, new CssRewriteUrlWithVirtualDirectoryTransform())
                    .Include(StylePaths.Simple_Line_Icons, new CssRewriteUrlWithVirtualDirectoryTransform())
                    .Include(StylePaths.FamFamFamFlags, new CssRewriteUrlWithVirtualDirectoryTransform())
                    .Include(isRTL ? StylePaths.BootstrapRTL : StylePaths.Bootstrap, new CssRewriteUrlWithVirtualDirectoryTransform())
                    .Include(StylePaths.JQuery_Uniform, new CssRewriteUrlWithVirtualDirectoryTransform())
                    .Include(StylePaths.Morris)
                    .Include(StylePaths.JsTree, new CssRewriteUrlWithVirtualDirectoryTransform())
                    .Include(StylePaths.SweetAlert)
                    .Include(StylePaths.Toastr)
                    .Include(StylePaths.Angular_Ui_Grid, new CssRewriteUrlWithVirtualDirectoryTransform())
                    .Include(StylePaths.Bootstrap_DateRangePicker)
                    .Include(StylePaths.Bootstrap_DateTimePicker)
                    .Include(StylePaths.Bootstrap_Select)
                    .Include(StylePaths.Bootstrap_Switch)
                    .Include(StylePaths.JQuery_Jcrop)
                    .Include(StylePaths.Angular_Editor)
                    .Include(StylePaths.Angular_Editor_Simditor)
                    .ForceOrdered()
                );
        }

        private static void AddAppMetrinicCss(BundleCollection bundles, bool isRTL)
        {
            bundles.Add(
                new StyleBundle("~/Bundles/App/metronic/css" + (isRTL ? "RTL" : ""))
                    .Include("~/metronic/assets/global/css/components-md" + (isRTL ? "-rtl" : "") + ".css", new CssRewriteUrlWithVirtualDirectoryTransform())
                    .Include("~/metronic/assets/global/css/plugins-md" + (isRTL ? "-rtl" : "") + ".css", new CssRewriteUrlWithVirtualDirectoryTransform())
                    .Include("~/metronic/assets/admin/layout4/css/layout" + (isRTL ? "-rtl" : "") + ".css", new CssRewriteUrlWithVirtualDirectoryTransform())
                    .Include("~/metronic/assets/admin/layout4/css/themes/light" + (isRTL ? "-rtl" : "") + ".css", new CssRewriteUrlWithVirtualDirectoryTransform())
                    .ForceOrdered()
                );
        }
    }
}