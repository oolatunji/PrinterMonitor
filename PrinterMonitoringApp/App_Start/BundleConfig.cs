using System.Web;
using System.Web.Optimization;

namespace PrinterMonitoringApp
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/common_libraries").Include(
                      "~/Scripts/jquery.js",
                      "~/Scripts/jquery-ui.min.js",
                      "~/Scripts/bootstrap.min.js",
                      "~/Scripts/jquery.slimscroll.min.js",
                      "~/Scripts/jquery.dataTables.min.js",
                      "~/Scripts/dataTables.buttons.min.js",
                      "~/Scripts/jszip.min.js",
                      "~/Scripts/pdfmake.min.js",
                      "~/Scripts/vfs_fonts.js",
                      "~/Scripts/buttons.html5.min.js",
                      "~/Scripts/jquery.knob.js",
                      "~/Scripts/owl.carousel.min.js",
                      "~/Scripts/d3.min.js",
                      "~/Scripts/moment.min.js",
                      "~/Scripts/xcharts.min.js",
                      "~/Scripts/skycons.js",
                      "~/Scripts/clndr.min.js",
                      "~/Scripts/toastr.min.js",
                      "~/Scripts/placeholders.min.js",
                      "~/Scripts/bootstrapValidator.min.js",
                      "~/Scripts/nav.js",
                      "~/Scripts/Utility/configFile.js",
                      "~/Scripts/Utility/messageBox.js",
                      "~/Scripts/Utility/SystemConfiguration.js",
                      "~/Scripts/custom.js",
                      "~/Scripts/Login/Login.js"
                      ));

            bundles.Add(new ScriptBundle("~/bundles/layout").Include(
                    "~/Scripts/Login/Layout.js",
                    "~/Scripts/DynamicMenu/Layout.js"
                    ));

            bundles.Add(new ScriptBundle("~/bundles/dashboard").Include(
                    "~/Scripts/DynamicMenu/Dashboard.js"
                    ));

            bundles.Add(new ScriptBundle("~/bundles/changepassword").Include(
                    "~/Scripts/Password/ChangePassword.js"
                    ));

            bundles.Add(new ScriptBundle("~/bundles/forgotpassword").Include(
                    "~/Scripts/Password/ForgotPassword.js"
                    ));

            bundles.Add(new ScriptBundle("~/bundles/resetpassword").Include(
                    "~/Scripts/Password/PasswordReset.js"
                    ));   

            bundles.Add(new ScriptBundle("~/bundles/configureSystem").Include(
                     "~/Scripts/Utility/ConfigureSystem.js"
                     ));            

            bundles.Add(new ScriptBundle("~/bundles/addfunctions").Include(
                      "~/Scripts/Function/AddFunction.js"                    
                      ));            

            bundles.Add(new ScriptBundle("~/bundles/viewfunctions").Include(
                      "~/Scripts/Function/ViewFunction.js"
                      ));

            bundles.Add(new ScriptBundle("~/bundles/addroles").Include(
                      "~/Scripts/Role/AddRole.js"
                      ));

            bundles.Add(new ScriptBundle("~/bundles/viewroles").Include(
                      "~/Scripts/Role/ViewRole.js"
                      ));

            bundles.Add(new ScriptBundle("~/bundles/addbranch").Include(
                      "~/Scripts/Branch/AddBranch.js"
                      ));

            bundles.Add(new ScriptBundle("~/bundles/viewbranch").Include(
                      "~/Scripts/Branch/ViewBranch.js"
                      ));

            bundles.Add(new ScriptBundle("~/bundles/addusers").Include(
                     "~/Scripts/User/AddUser.js"
                     ));

            bundles.Add(new ScriptBundle("~/bundles/viewusers").Include(
                      "~/Scripts/User/ViewUser.js"
                      ));

            bundles.Add(new ScriptBundle("~/bundles/viewsmartcards").Include(
                      "~/Scripts/SmartCard/ViewSmartCard.js"
                      ));

            bundles.Add(new ScriptBundle("~/bundles/addprinters").Include(
                     "~/Scripts/Printer/AddPrinter.js"
                     ));

            bundles.Add(new ScriptBundle("~/bundles/viewprinters").Include(
                      "~/Scripts/Printer/ViewPrinter.js"
                      ));
         
            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.min.css",
                      "~/Content/font-awesome.min.css",
                      "~/Content/ionicons.min.css",
                      "~/Content/animate.css",
                      "~/Content/xcharts.min.css",
                      "~/Content/owl.carousel.css",
                      "~/Content/owl.transitions.css",
                       "~/Content/jquery.dataTables.min.css",
                      "~/Content/buttons.dataTables.min.css",
                      "~/Content/clndr.css",
                      "~/Content/toastr.min.css",
                      "~/Content/style.css",
                      "~/Content/Wobblebar.css"));

            // Set EnableOptimizations to false for debugging. For more information,
            // visit http://go.microsoft.com/fwlink/?LinkId=301862
            BundleTable.EnableOptimizations = true;
        }
    }
}
