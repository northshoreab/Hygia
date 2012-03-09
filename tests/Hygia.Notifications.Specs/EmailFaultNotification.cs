using System;
using System.Collections.Generic;
using FakeItEasy;
using Hygia.FaultManagement.Events;
using Hygia.Notifications.Domain;
using Hygia.Operations.Email.Commands;
using Machine.Specifications;

namespace Hygia.Notifications.Specs
{
    using Hygia.Specs;

    [Subject("Email notification")]
    public class When_sending_a_email_fault_notification : WithHandler<FaultNotificationHandler>
    {
        static readonly Guid FaultEnvelopeId = Guid.NewGuid();
        static readonly Guid MessageTypeId = Guid.NewGuid();

        Establish context = () =>
                                {
                                    Session.Store(new FaultNotificationSetting
                                                      {
                                                          AllMessages = true,
                                                          EmailAdress = "daniel@kjellqvist.nu",
                                                          NotificationType = NotificationTypes.Email
                                                      });

                                    ResultOfProvide = new
                                                          {
                                                              Message = "A fault occured wich caused the applicaton to blow up",
                                                              Reason = "A fault occured",
                                                              Body = "Here is a message body",
                                                              TimeOfFailure = DateTime.Now,
                                                              MessageTypeName = "Acme.OrderPlaced"
                                                          };
                                };

        Because of = () => Handle<FaultRegistered>(m =>
                                                       {
                                                           m.EnvelopeId = FaultEnvelopeId;
                                                           m.MessageTypes = new List<Guid> { MessageTypeId };
                                                       });


        It should_send_email_request = () => A.CallTo(() => FakeBus.Send(A<Action<SendEmailRequest>>.Ignored)).MustHaveHappened();
    }
}
