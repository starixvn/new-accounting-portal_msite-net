using NUglify.Css;
using WebOptimizer;

namespace Maintenance.Sites.Utilities;

using WebOptimizer.Processors;

public static class Bundle
{
    public static void AddLayoutBundle(this IAssetPipeline pipeline, CssSettings cssSetting, JsSettings jsSettings)
    {
        pipeline.AddCssBundle(
            "/css/common.css",
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
        pipeline.AddLessBundle(
            "/css/common-less.css",
            "themes/manage/manage.less",
            "themes/manage/refreshcountdown.less",
            "themes/manage/letstalknotification.less");

        pipeline.AddJavaScriptBundle("/scripts/sharedscripts.js", jsSettings,
            "scripts/lib/jquery-1.8.0.min.js",
            "scripts/lib/jquery-ui-1.8.23.min.js",
            "scripts/lib/jquery.cookie.js",
            "scripts/lib/chosen/chosen.jquery.js",
            "scripts/lib/Fanex.Grid/moment.min.js",
            "scripts/lib/moment/moment-timezone-with-data.js",
            "scripts/lib/tipsy/tipsy-1.0.0.min.js",
            "scripts/lib/msgbox/msgbox-0.0.1.min.js",
            "scripts/ShareScripts/sharedscriptmanager.js",
            "scripts/Maintenance.js",
            "scripts/lib/Fanex.Grid/Fanex.Grid.js",
            "scripts/lib/Fanex.Grid/Fanex.Pagination.js",
            "scripts/lib/RefreshCountDown.js");
    }

    public static void AddRawLess(this IAssetPipeline pipeline)
    {
        pipeline.CompileLessFiles(
            "themes/shared/contextmenu.less",
            "themes/shared/formedit.less",
            "scripts/lib/InputCombobox/jquery.combobox-ext.less",
            "scripts/lib/Countdown/CountDown.less",
            "themes/manage/ManageLetstalkMessage.less",
            "themes/manage/bot-service/bot-service.less",
            "themes/manage/log/mail-config.less",
            "themes/manage/log/manage.less",
            "themes/manage/LetstalkReport.less",
            "mailstation/css/createmail.less",
            "mailstation/css/mailactionlog.less",
            "mailstation/css/managemail.less",
            "mailstation/css/updatemail.less",
            "mailstation/css/createmailgroup.less",
            "mailstation/css/managemailgroup.less",
            "mailstation/css/updatemailgroup.less",
            "mailstation/css/mailreport.less",
            "mailstation/css/mailreportdetail.less",
            "mailstation/css/maillog.less",
            "mailstation/css/ManageRecipient.less",
            "mailstation/css/CreateRecipient.less",
            "mailstation/css/UpdateRecipient.less");
    }

    public static void AddMailBundle(this IAssetPipeline pipeline)
    {
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