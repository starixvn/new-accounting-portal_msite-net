namespace Maintenance.Letstalk.Services;

using System;
using System.Collections.Generic;
using System.Linq;
using Command;
using Criteria;
using Fanex.Data.Repository;
using Fanex.NAP.Client.Configuration;
using Fanex.NAP.Client.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Model;

public interface IManageLetstalkMessageService
{
    CommandResult InsertNotification(InsertNotificationCriteria criteria, IEnumerable<IFormFile> files);

    IEnumerable<SelectListItem> GetGroups();

    IEnumerable<SelectListItem> GetStatusList();

    IEnumerable<NotificationModel> GetNotificationList(GetNotificationListCriteria criteria);

    IEnumerable<NotificationModel> GetNotificationById(GetNotificationListCriteria criteria);

    CommandResult EditNotification(EditNotificationCriteria criteria, IEnumerable<IFormFile> files);

    CommandResult DeleteNotification(DeleteNotificationCriteria criteria);

    CommandResult ResendNotification(ResendNotificationCriteria criteria);
}

public class ManageLetstalkMessageService(
    IUserService userService, 
    IDynamicRepository dynamicRepository, 
    INapSettings napSettings) : IManageLetstalkMessageService
{
    private const string uatEnvironment = "UAT";

    public CommandResult InsertNotification(InsertNotificationCriteria criteria, IEnumerable<IFormFile> files)
    {
        criteria.IsTest = napSettings.Environment == uatEnvironment;

        dynamicRepository.Execute(criteria);

        return new CommandResult
        {
            Code = 0,
            Message = ""
        };
    }

    public IEnumerable<SelectListItem> GetGroups()
    {
        var criteria = new GetGroupCriteria();
        var result = new List<SelectListItem>();
        var recipientList = dynamicRepository.Fetch<RecipientModel>(criteria).ToList();

        foreach (var recipient in recipientList)
        {
            var environment = napSettings.Environment;
            if (recipient.Environment.Equals(environment, StringComparison.CurrentCultureIgnoreCase))
            {
                result.Add(new SelectListItem
                {
                    Selected = false,
                    Value = recipient.RecipientID.ToString(),
                    Text = recipient.ConversationID + " - " + recipient.RecipientName
                });
            }

        }
        return result;
    }

    public IEnumerable<SelectListItem> GetStatusList()
    {
        return new List<SelectListItem>
        {
            new() { Text = "Waiting" , Value = "0" },
            new() { Text = "Sending" , Value = "1" },
            new() { Text = "Sent" , Value = "2" },
            new() { Text = "Failed" , Value = "3" }
        };
    }

    public IEnumerable<NotificationModel> GetNotificationList(GetNotificationListCriteria criteria)
    {
        criteria.IsTest = napSettings.Environment == uatEnvironment;

        var (notificationList, notificationRecipientList) =dynamicRepository.FetchMultiple<NotificationModel, NotificationRecipientModel>(criteria);

        var result = UpdateModelView(notificationList, notificationRecipientList);
        return result;
    }

    private IEnumerable<NotificationModel> UpdateModelView(IEnumerable<NotificationModel> notificationList, IEnumerable<NotificationRecipientModel> notificationRecipientList)
    {
        var result = new List<NotificationModel>();
        foreach (var item in notificationList)
        {
            item.Creator = GetUserName(item.CreatedBy);
            item.LatestEditor = GetUserName(item.LastModifiedBy);
            foreach (var notificationRecipient in notificationRecipientList)
            {
                if (notificationRecipient.NotificationID == item.NotificationID)
                {
                    item.RecipientID = notificationRecipient.RecipientID;
                    item.RecipientName = notificationRecipient.RecipientName;
                    item.ConversationID = notificationRecipient.ConversationID;
                    item.Status = notificationRecipient.Status;
                    item.LastSentTime = notificationRecipient.LastSentTime;
                    result.Add(item);
                }
            }
        }
        return result;
    }

    private string GetUserName(int userId)
    {
        try
        {
            return userService.Get(userId)?.Username;
        }
        catch
        {
            return "";
        }
    }

    public IEnumerable<NotificationModel> GetNotificationById(GetNotificationListCriteria criteria)
    {
        criteria.IsTest = napSettings.Environment == uatEnvironment;

        return dynamicRepository.Fetch<NotificationModel>(criteria);
    }

    public CommandResult EditNotification(EditNotificationCriteria criteria, IEnumerable<IFormFile> files)
    {
        try
        {
            dynamicRepository.Execute(criteria);

            return new CommandResult
            {
                Code = 0,
                Message = ""
            };
        }
        catch
        {
            return new CommandResult
            {
                Code = 1,
                Message = "You are not allowed to update in-progress messages. All changes will be discarded."
            };
        }
    }

    public CommandResult DeleteNotification(DeleteNotificationCriteria criteria)
    {
        try
        {
            dynamicRepository.Execute(criteria);

            return new CommandResult
            {
                Code = 0,
                Message = "Message has been deleted successfully!"
            };
        }
        catch
        {
            return new CommandResult
            {
                Code = 1,
                Message = "You are not allowed to delete in-progress messages. All changes will be discarded."
            };
        }
    }

    public CommandResult ResendNotification(ResendNotificationCriteria criteria)
    {
        try
        {
            dynamicRepository.Execute(criteria);

            return new CommandResult
            {
                Code = 0,
                Message = "Message has been resent successfully!"
            };
        }
        catch
        {
            return new CommandResult
            {
                Code = 1,
                Message = "You are not allowed to resend in-progress messages. All changes will be discarded."
            };
        }
    }
}