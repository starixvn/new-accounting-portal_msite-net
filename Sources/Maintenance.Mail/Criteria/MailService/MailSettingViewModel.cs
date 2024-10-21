namespace Maintenance.Mail.Criteria.MailService;

using System.ComponentModel.DataAnnotations;
using Fanex.MailDeliver.MailClient.Model;
using Services;

public class MailSettingViewModel
{
    public MailSettingViewModel()
    {
    }

    public MailSettingViewModel(MailSetting mailSetting)
    {
        SettingName = mailSetting.SettingName;
        SettingId = mailSetting.SettingID;
        Server = mailSetting.Server;
        Port = mailSetting.Port;
        Username = mailSetting.Username;
        Password = !string.IsNullOrWhiteSpace(mailSetting.Password) ? Cryptography.CfsEnCrypt(mailSetting.Password) : string.Empty;
        IsAuth = mailSetting.IsAuth;
    }

    public int SettingId { get; set; }

    [Display(Name = "Setting_Name", ResourceType = typeof(ViewResource))]
    [StringLength(100, MinimumLength = 2,
        ErrorMessageResourceName = "InputLengthValidationMsg",
        ErrorMessageResourceType = typeof(ViewResource))]
    public string SettingName { get; set; }

    [Required]
    [Display(Name = "Server", ResourceType = typeof(ViewResource))]
    [StringLength(50, MinimumLength = 2,
        ErrorMessageResourceName = "InputLengthValidationMsg",
        ErrorMessageResourceType = typeof(ViewResource))]
    public string Server { get; set; }

    [Required]
    [Display(Name = "Port", ResourceType = typeof(ViewResource))]
    public int Port { get; set; }

    [Required]
    [Display(Name = "Username", ResourceType = typeof(ViewResource))]
    [StringLength(50, MinimumLength = 2,
        ErrorMessageResourceName = "InputLengthValidationMsg",
        ErrorMessageResourceType = typeof(ViewResource))]
    public string Username { get; set; }

    [Display(Name = "Password", ResourceType = typeof(ViewResource))]
    public string Password { get; set; }

    [Display(Name = "IsAuth", ResourceType = typeof(ViewResource))]
    public bool IsAuth { get; set; }

    public MailSetting GetMailSetting()
    {
        var mailSetting = new MailSetting
        {
            IsAuth = IsAuth,
            Password = Password,
            Port = Port,
            Server = Server,
            SettingID = SettingId,
            SettingName = SettingName,
            Username = Username
        };

        return mailSetting;
    }
}