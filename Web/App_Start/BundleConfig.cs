using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;

namespace Tazeyab.Web
{
    public class BundleConfig
    {


        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            BundleTable.EnableOptimizations = true;

            //bundles.UseCdn = true;

            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                      "~/Scripts/jquery/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery/jquery.validate*"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
                       "~/Scripts/jquery/jqueryui/jquery-ui-{version}.js"));

            bundles.Add(new ScriptBundle("~/scripts/bootstrap")
                                    .Include("~/scripts/Lib/bootstrap.js"));

            bundles.Add(new ScriptBundle("~/scripts/masterpage")
                                    .Include("~/scripts/layout/masterpage.js",
                                     "~/scripts/lib/jquery.autocomplete.js"));

            bundles.Add(new ScriptBundle("~/scripts/index")
                         .Include("~/Scripts/home/index.js"));

            bundles.Add(new StyleBundle("~/content/index")
                            .Include("~/" + Links.Content.jquery_bxslider_css));

            bundles.Add(new StyleBundle("~/content/bootstrap")
                         .Include("~/Content/bootstrap.css",
                         "~/Content/Themes/bootstrap-tazeyab.css"
                         ));

            bundles.Add(new StyleBundle("~/content/bootstrapLTR")
                        .Include("~/Content/bootstrap.css"));

            bundles.Add(new StyleBundle("~/content/layout")
                            .Include("~/content/_Layout/masterpage.css",
                            "~/" + Links.Content.PersianFonts_css,
                            "~/" + Links.Content.Share.global_css,
                            "~/content/_Layout/responsive.css",
                            "~/content/_Layout/topmenu.css",
                            "~/content/_Layout/treeMenu.css",
                            "~/content/feeditem.css",
                            "~/content/ads.css",
                            "~/content/imagethumbnail.min.css",
                            "~/" + Links.Content.Index_css
                            ));  

            bundles.Add(new ScriptBundle("~/scripts/kendo/js")
                    .Include("~/" + Links.Scripts.Kendo.kendo_all_min_js,
                             "~/" + Links.Scripts.Kendo.kendo_aspnetmvc_min_js));        

            bundles.Add(new StyleBundle("~/content/kendo/cssDark")
                         .Include("~/" + Links.Content.Kendo.kendo_common_material_core_min_css,
                         "~/" + Links.Content.Kendo.kendo_common_material_min_css,
                         "~/" + Links.Content.Kendo.kendo_materialblack_min_css,
                         "~/" + Links.Content.Kendo.kendo_rtl_min_css));

            bundles.Add(new StyleBundle("~/content/kendo/cssLight")
                           .Include("~/" + Links.Content.Kendo.kendo_common_min_css,
                           "~/" + Links.Content.Kendo.kendo_bootstrap_min_css,
                           "~/" + Links.Content.Kendo.kendo_rtl_min_css));


            //-----end page--site.page/------
            bundles.Add(new StyleBundle("~/content/pagecss")
                .Include("~/content/_Layout/topMenu.css",
                "~/" + Links.Content.PersianFonts_css,
                "~/content/_Layout/masterpage.css",
                "~/content/_layout/responsive.css",
                "~/content/page.chtml.css"));

            bundles.Add(new ScriptBundle("~/scripts/pjs")
                    .Include("~/scripts/layout/masterpage.js",
                            "~/scripts/pages/page.js"));

            //----mobile css------
            bundles.Add(new StyleBundle("~/content/cssMobile")
                .Include("~/Content/jquery-ui-1.9.2.custom.min.css",
                            "~/Content/_Layout/MasterPage.mobile.css",
                            "~/Content/_Layout/TreeMenu.mobile.css"));

            BundleTable.EnableOptimizations = true;
            // Clear all items from the ignore list to allow minified CSS and JavaScript files in debug mode
            bundles.IgnoreList.Clear();

            // Add back the default ignore list rules sans the ones which affect minified files and debug mode
            bundles.IgnoreList.Ignore("*.intellisense.js");
            bundles.IgnoreList.Ignore("*-vsdoc.js");
            bundles.IgnoreList.Ignore("*.debug.js", OptimizationMode.WhenEnabled);
        }
    }
}