namespace Maintenance.Mail.Model
{
    using System.Configuration;

    public class Setting : ConfigurationSection
    {
        private const string __NotificationService1 = "NotificationService1";
        private const string __NotificationService2 = "NotificationService2";
        private const string __AuthenticationKey = "AuthenticationKey";
        private const string __TargetNameAlert = "TargetNameAlert";
        private const string __TargetNameMailing = "TargetNameMailing";
        private const string __TargetNameAlert1 = "TargetNameAlertUAT";
        private const string __TargetNameMailing1 = "TargetNameMailingUAT";
        private const string __ITALLAlert = "ITALLAlert";
        private const string __ITALLMail = "ITALLMail";

        [ConfigurationProperty(__NotificationService1, IsRequired = true)]
        public string NotificationService1
        {
            get => this[__NotificationService1].ToString();
            set => this[__NotificationService1] = value;
        }

        [ConfigurationProperty(__NotificationService2, IsRequired = true)]
        public string NotificationService2
        {
            get => this[__NotificationService2].ToString();
            set => this[__NotificationService2] = value;
        }

        [ConfigurationProperty(__AuthenticationKey, IsRequired = true)]
        public string AuthenticationKey
        {
            get => this[__AuthenticationKey].ToString();
            set => this[__AuthenticationKey] = value;
        }

        [ConfigurationProperty(__TargetNameAlert, IsRequired = true)]
        public string TargetNameAlert
        {
            get => this[__TargetNameAlert].ToString();
            set => this[__TargetNameAlert] = value;
        }

        [ConfigurationProperty(__TargetNameMailing, IsRequired = true)]
        public string TargetNameMailing
        {
            get => this[__TargetNameMailing].ToString();
            set => this[__TargetNameMailing] = value;
        }

        [ConfigurationProperty(__TargetNameAlert1, IsRequired = true)]
        public string TargetNameAlert1
        {
            get => this[__TargetNameAlert1].ToString();
            set => this[__TargetNameAlert1] = value;
        }

        [ConfigurationProperty(__TargetNameMailing1, IsRequired = true)]
        public string TargetNameMailing1
        {
            get => this[__TargetNameMailing1].ToString();
            set => this[__TargetNameMailing1] = value;
        }

        [ConfigurationProperty(__ITALLAlert, IsRequired = true)]
        public string ITALLAlert
        {
            get => this[__ITALLAlert].ToString();
            set => this[__ITALLAlert] = value;
        }

        [ConfigurationProperty(__ITALLMail, IsRequired = true)]
        public string ITALLMail
        {
            get => this[__ITALLMail].ToString();
            set => this[__ITALLMail] = value;
        }
    }
}
