namespace Maintenance.Mail.Criteria.MailService
{
    using System.ComponentModel.DataAnnotations;
    using Fanex.MailDeliver.MailClient.Model;

    public class MailConfigViewModel
    {
        public MailConfigViewModel()
        {
        }

        public MailConfigViewModel(MailConfig mailSetting)
        {
            AppName = mailSetting.AppName;
            Description = mailSetting.Description;
            SettingId = mailSetting.Setting.SettingID;
            AppId = mailSetting.AppID;
            MailFrom = string.IsNullOrEmpty(mailSetting.MailFrom) ? mailSetting.Setting.Username : mailSetting.MailFrom;
            IsEncrypt = mailSetting.IsEncrypt;
        }

        public int AppId { get; set; }

        [Required]
        [Display(Name = "Setting", ResourceType = typeof(ViewResource))]
        public int SettingId { get; set; }

        [Required]
        [Display(Name = "Application_Name", ResourceType = typeof(ViewResource))]
        [StringLength(50, MinimumLength = 2,
            ErrorMessageResourceName = "InputLengthValidationMsg",
            ErrorMessageResourceType = typeof(ViewResource))]
        [RegularExpression(@"^[\w\s]{1,}$",
            ErrorMessageResourceName = "InvalidNameValidationMsg",
            ErrorMessageResourceType = typeof(ViewResource))]
        public string AppName { get; set; }

        [Display(Name = "Description", ResourceType = typeof(ViewResource))]
        public string Description { get; set; }

        [Display(Name = "MailFrom", ResourceType = typeof(ViewResource))]
        public string MailFrom { get; set; }

        [Display(Name = "IsEncrypt", ResourceType = typeof(ViewResource))]
        public bool IsEncrypt { get; set; }

        public MailConfig GetMailConfig(int userId)
        {
            var mailConfig = new MailConfig
            {
                AppID = AppId,
                AppName = AppName,
                Description = Description,
                MailFrom = MailFrom,
                IsEncrypt = IsEncrypt,
                Setting = new MailSetting
                {
                    SettingID = SettingId
                },
                UserId = userId
            };

            return mailConfig;
        }
    }
}
