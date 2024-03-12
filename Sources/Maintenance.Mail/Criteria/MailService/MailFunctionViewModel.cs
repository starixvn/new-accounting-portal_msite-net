namespace Maintenance.Mail.Criteria.MailService
{
    using System.ComponentModel.DataAnnotations;
    using Fanex.MailDeliver.MailClient.Model;

    public class MailFunctionViewModel
    {
        public MailFunctionViewModel()
        {
        }

        public MailFunctionViewModel(MailFunction mailFunction)
        {
            ConfigId = mailFunction.Config.AppID;
            SettingId = mailFunction.Setting.SettingID;
            FunctionId = mailFunction.FuncID;
            FunctionName = mailFunction.FuncName;
        }

        public int FunctionId { get; set; }

        [Required]
        [Display(Name = "Setting", ResourceType = typeof(ViewResource))]
        public int SettingId { get; set; }

        [Required]
        [Display(Name = "Config", ResourceType = typeof(ViewResource))]
        public int ConfigId { get; set; }

        [Required]
        [Display(Name = "Function_Name", ResourceType = typeof(ViewResource))]
        [StringLength(50, MinimumLength = 2,
            ErrorMessageResourceName = "InputLengthValidationMsg",
            ErrorMessageResourceType = typeof(ViewResource))]
        [RegularExpression(@"^[\w\s]{1,}$",
            ErrorMessageResourceName = "InvalidNameValidationMsg",
            ErrorMessageResourceType = typeof(ViewResource))]
        public string FunctionName { get; set; }

        public MailFunction GetMailFunction()
        {
            var mailFunction = new MailFunction
            {
                Config = new MailConfig
                {
                    AppID = ConfigId
                },
                Setting = new MailSetting
                {
                    SettingID = SettingId
                },
                FuncName = FunctionName,
                FuncID = FunctionId
            };

            return mailFunction;
        }
    }
}
