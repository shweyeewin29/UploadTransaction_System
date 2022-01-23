using System.Web;
using System.Web.Optimization;

namespace UploadTransaction_System
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*",
                        "~/Scripts/jquery.unobtrusive-ajax.js"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css"));

            bundles.Add(new StyleBundle("~/bundles/toolscss").Include(

                  //Loading
                  "~/vendor/Loading.css",
                  //Select2
                  "~/vendor/select2/select2.min.css",

                  //SweetAlert
                  "~/vendor/SweetAlert/sweetalert.css",

                  //DateTimePicker
                  "~/vendor/DateTimePicker/bootstrap-datetimepicker.css",

                  //DataTable
                  "~/vendor/DataTables/DataTables-1.11.3/css/dataTables.bootstrap.css",
                  "~/vendor/DataTables/Buttons-2.0.1/css/buttons.bootstrap.min.css"
              ));

            bundles.Add(new ScriptBundle("~/bundles/toolsjs").Include(
                    //Select2
                    "~/vendor/select2/select2.full.min.js",
                    "~/vendor/select2/select2-tab-fix.js",

                    //SweetAlert
                    "~/vendor/SweetAlert/sweetalert.min.js",

                    //date time moment
                    "~/vendor/DateTimePicker/moment.js",
                    "~/Scripts/dobpicker.js",

                    //DateTimePicker
                    "~/vendor/DateTimePicker/bootstrap-datetimepicker.js",

                    //DataTable
                    "~/vendor/DataTables/DataTables-1.11.3/js/jquery.dataTables.min.js",
                    "~/vendor/DataTables/DataTables-1.11.3/js/dataTables.bootstrap.min.js",
                    "~/vendor/DataTables/Buttons-2.0.1/js/dataTables.buttons.min.js",
                    "~/vendor/DataTables/Buttons-2.0.1/js/buttons.bootstrap.min.js",
                    "~/vendor/DataTables/Buttons-2.0.1/js/buttons.flash.min.js",
                    "~/vendor/DataTables/Buttons-2.0.1/js/buttons.html5.min.js",
                    "~/vendor/DataTables/Buttons-2.0.1/js/buttons.print.min.js",
                    "~/vendor/DataTables/Buttons-2.0.1/js/buttons.colVis.min.js",
                    "~/vendor/DataTables/Buttons-2.0.1/js/jszip.min.js"
                    ));
        }
    }
}
