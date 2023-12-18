using NUglify.Css;
using NUglify.JavaScript;
using WebOptimizer;

namespace Maintenance.Sites.Utilities;

public static class Bundle
{
    public static void AddLayoutBundle(this IAssetPipeline pipeline, CssSettings cssSetting, CodeSettings scriptSetting)
    {
        pipeline.AddCssBundle(
            "/css/common.cs",
            cssSetting,
            "themes/reset.css",
            "themes/fonts.css",
            "themes/jquery-ui/smoothness/jquery-ui-1.8.23.min.css",
            "themes/assets/css/prettify.css",
            "themes/assets/css/site.css",
            "scripts/lib/tipsy/tipsy-1.0.0.min.css",
            "scripts/lib/msgbox/msgbox-0.0.1.min.css",
            "scripts/lib/chosen/chosen.css",
            "scripts/lib/Fanex.Grid/Fanex.Grid.css",
            "scripts/lib/Fanex.Grid/Fanex.Pagination.css");

        pipeline.AddJavaScriptBundle("/scripts/sharedscripts.js",scriptSetting,
            "scripts/ShareScripts/SharedScriptManager.js",
            "scripts/lib/Fanex.Grid/Fanex.Grid.js",
            "scripts/lib/Fanex.Grid/Fanex.Pagination.js",
            "scripts/lib/jquery-1.8.0.min.js",
            "scripts/lib/jquery-ui-1.8.23.min.js",
            "scripts/lib/tipsy/tipsy-1.0.0.min.js",
            "scripts/lib/msgbox/msgbox-0.0.1.min.js",
            "scripts/Maintenance.js",
            "scripts/lib/chosen/chosen.jquery.js",
            "scripts/lib/Fanex.Grid/Fanex.Grid.js",
            "scripts/lib/Fanex.Grid/Fanex.Pagination.js",
            "scripts/lib/jquery.cookie.js",
            "scripts/lib/Fanex.Grid/moment.min.js",
            "scripts/lib/moment/moment-timezone-with-data.js",
            "scripts/lib/RefreshCountDown.js");
    }

    public static void AddMailBundle(this IAssetPipeline pipeline, CssSettings cssSetting, CodeSettings scriptSetting)
    {
        pipeline.AddLessBundle(
            "/css/mail-list.css",
            "themes/manage/manage.less",
            "themes/manage/RefreshCountDown.less",
            "themes/manage/LetstalkNotification.less");

        pipeline.AddLessBundle(
            "/css/mail-manage.css",
            "themes/manage/log/manage.less");

        pipeline.AddLessBundle(
            "/css/manage-letstalk-message.css",
            "themes/manage/ManageLetstalkMessage.less");

        pipeline.AddLessBundle(
            "/css/mail-config-log.css",
            "themes/manage/log/mail-config.less");


        pipeline.AddJavaScriptBundle("/scripts/mail-config.js",
            "scripts/Mail/MailConfig.js");

        pipeline.AddJavaScriptBundle("/scripts/get-mail-config-logs.js",
            "scripts/Mail/GetMailConfigLogs.js");

        pipeline.AddJavaScriptBundle("/scripts/add-mail-config.js",
            "scripts/Mail/AddMailConfig.js");

        pipeline.AddJavaScriptBundle("/scripts/add-mail-config-popup.js",
            "scripts/Mail/AddMailConfigPopUp.js");

        pipeline.AddJavaScriptBundle("/scripts/add-mail-setting-popup.js",
            "scripts/Mail/AddMailSettingPopUp.js");

        pipeline.AddJavaScriptBundle("/scripts/add-mail-function.js",
            "scripts/Mail/AddMailFunction.js");
    }
}