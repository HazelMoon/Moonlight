using Moonlight.App.Database.Entities;
using Moonlight.App.Database.Entities.Community;
using Moonlight.App.Database.Entities.Store;
using Moonlight.App.Database.Entities.Tickets;
using Moonlight.App.Event.Args;
using Moonlight.App.Helpers;

namespace Moonlight.App.Event;

public class Events
{
    public static SmartEventHandler<User> OnUserRegistered = new();
    public static SmartEventHandler<User> OnUserPasswordChanged = new();
    public static SmartEventHandler<User> OnUserTotpSet = new();
    public static SmartEventHandler<MailVerificationEventArgs> OnUserMailVerify = new();
    public static SmartEventHandler<Service> OnServiceOrdered = new();
    public static SmartEventHandler<TransactionCreatedEventArgs> OnTransactionCreated = new();
    public static SmartEventHandler<Post> OnPostCreated = new();
    public static SmartEventHandler<Post> OnPostUpdated = new();
    public static SmartEventHandler<Post> OnPostDeleted = new();
    public static SmartEventHandler<Post> OnPostLiked = new();
    public static SmartEventHandler<PostComment> OnPostCommentCreated = new();
    public static SmartEventHandler<PostComment> OnPostCommentDeleted = new();
    public static SmartEventHandler<Ticket> OnTicketCreated = new();
    public static SmartEventHandler<TicketMessageEventArgs> OnTicketMessage = new();
    public static SmartEventHandler<Ticket> OnTicketUpdated = new();
    public static SmartEventHandler OnMoonlightRestart = new();
}