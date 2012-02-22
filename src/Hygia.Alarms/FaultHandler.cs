namespace Hygia.Alarms
{
    //public class FaultHandler : IHandleMessages<FaultRegistered>
    //{
    //    private readonly IBus _bus;

    //    public FaultHandler(IBus bus)
    //    {
    //        _bus = bus;
    //    }

    //    public void Handle(FaultRegistered message)
    //    {
    //        _bus.Send(new FaultAlarm
    //                      {
    //                          MessageTypeId = message.Fault.ContainedMessages.Select(x => x.MessageTypeId),
    //                          ExceptionMessage = message.Fault.Exception.Message,
    //                          ExceptionReason = message.Fault.Exception.Reason,
    //                          ExceptionSource = message.Fault.Exception.Source,
    //                          ExceptionStackTrace = message.Fault.Exception.StackTrace,
    //                          ExceptionType = message.Fault.Exception.ExceptionType,
    //                          MessageBody = message.Fault.Body,
    //                          TimeOfFailure = message.Fault.TimeOfFailure,
    //                          MessageId = message.Fault.Id
    //                      });
    //    }
    //}
}
