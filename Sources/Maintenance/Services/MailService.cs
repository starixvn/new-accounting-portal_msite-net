namespace Maintenance.Services;

using System;
using System.Collections.Generic;
using System.Linq;
using Fanex.MailDeliver.MailClient.API;
using Fanex.MailDeliver.MailClient.Model;

public interface IMailService
{
    List<MailConfig> GetListConfig(string ip = "");

    IMailService WithApp(int appId);

    IMailService WithStatus(int status);

    IMailService FromDate(string date);

    IMailService ToDate(string date);

    IMailService WithPagination(int page, int counter);

    List<MailRecord> GetListMail();

    List<MailRecord> GetListMailByDate(ref int totalRows);

    MailRecord GetMail(int mailId);

    MailSetting GetSetting(int settingId);

    MailConfig GetConfig(int configId);

    MailFunction GetFunction(int functionId);

    List<MailSetting> GetListSetting();

    List<MailFunction> GetListFunction();

    bool SendMail(long mailID);

    bool StoreSetting(MailSetting setting);

    bool StoreConfig(MailConfig config);

    bool StoreFunction(MailFunction func);

    List<MailError> GetError(int mailId);
}

public class MailService : IMailService
{
    private int _appId;

    private string _fromDate;

    private string _toDate;

    private int _page;

    private int _counter;

    private MailStatus _mailStatus;

    public MailService()
    {

    }

    public List<MailConfig> GetListConfig(string ip = "")
    {
        var configs = new List<MailConfig>();

        using var client = new MailDeliver();
        var data = client.GetListConfig(ip);
        if (data != null)
        {
            configs = data.OrderBy(x => x.AppName).ToList();
        }

        return configs;
    }

    public IMailService WithApp(int appId)
    {
        _appId = appId;

        return this;
    }

    public IMailService FromDate(string date)
    {
        _fromDate = date;

        return this;
    }

    public IMailService ToDate(string date)
    {
        _toDate = date;

        return this;
    }

    public IMailService WithPagination(int page, int counter)
    {
        _page = page;
        _counter = counter;

        return this;
    }

    public List<MailRecord> GetListMail()
    {
        var mails = new List<MailRecord>();

        using var client = new MailDeliver();
        var dateTime = DateTime.Parse(_fromDate);
        var data = client.GetListMail(_appId, dateTime, _page, _counter);
        if (data != null)
        {
            mails = data.ToList();
        }

        return mails;
    }

    public List<MailRecord> GetListMailByDate(ref int totalRows)
    {
        var mails = new List<MailRecord>();

        using var client = new MailDeliver();
        var dateTime = DateTime.Parse(_fromDate);
        var dateTimeTo = DateTime.Parse(_toDate);
        var data = client.GetListMailByDate(_appId, dateTime, dateTimeTo, ref totalRows, _page, _counter, _mailStatus);

        if (data != null)
        {
            mails = data.Select(mailRecord =>
            {
                mailRecord.MailTo = mailRecord.MailTo.Replace(";", "<br />");
                mailRecord.MailBcc = mailRecord.MailBcc.Replace(";", "<br />");
                mailRecord.MailCc = mailRecord.MailCc.Replace(";", "<br />");

                return mailRecord;
            }).ToList();
        }

        return mails;
    }

    public MailRecord GetMail(int mailID)
    {
        using var client = new MailDeliver();
        return client.GetMail(mailID);
    }

    public List<MailSetting> GetListSetting()
    {
        var settings = new List<MailSetting>();

        using var client = new MailDeliver();
        var data = client.GetListSetting();
        if (data != null)
        {
            settings = data.OrderBy(x => x.Server).ToList();
        }

        return settings;
    }

    public List<MailFunction> GetListFunction()
    {
        var functions = new List<MailFunction>();

        using var client = new MailDeliver();
        var data = client.GetListFuntion();
        if (data != null)
        {
            functions = data.ToList();
        }

        return functions;
    }

    public bool SendMail(long mailID)
    {
        using var client = new MailDeliver();
        client.ReSendMail(mailID);

        return true;
    }

    public bool StoreSetting(MailSetting setting)
    {
        var result = false;

        if (setting != null)
        {
            using var client = new MailDeliver();
            client.StoreSetting(setting);
            result = true;
        }

        return result;
    }

    public bool StoreConfig(MailConfig config)
    {
        var result = false;

        if (config != null)
        {
            using var client = new MailDeliver();
            client.StoreConfig(config);
            result = true;
        }

        return result;
    }

    public bool StoreFunction(MailFunction func)
    {
        var result = false;

        if (func != null)
        {
            using var client = new MailDeliver();
            client.StoreFunction(func);
            result = true;
        }

        return result;
    }

    public MailSetting GetSetting(int settingId)
    {
        using var client = new MailDeliver();
        return client.GetSetting(settingId);
    }

    public MailConfig GetConfig(int configId)
    {
        using var client = new MailDeliver();
        return client.GetConfig(configId);
    }

    public MailFunction GetFunction(int functionId)
    {
        using var client = new MailDeliver();
        return client.GetFunction(functionId);
    }

    public IMailService WithStatus(int status)
    {
        _mailStatus = status switch
        {
            0 => MailStatus.All,
            -1 => MailStatus.Fail,
            1 => MailStatus.Success,
            _ => _mailStatus
        };

        return this;
    }

    public List<MailError> GetError(int mailId)
    {
        using var client = new MailDeliver();
        return client.GetListError(mailId).ToList();
    }
}