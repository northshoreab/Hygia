using System.Linq;

namespace Hygia.LaunchPad.Specs
{
    using System.Collections.Generic;
    using Commands;
    using Inspectors;
    using LogicalMonitoring.Commands;
    using LogicalMonitoring.Inspectors;
    using Machine.Specifications;
    using NServiceBus.Unicast;
    using NServiceBus.Unicast.Subscriptions;
    using NServiceBus.Unicast.Transport;

    [Subject("Message registration")]
    public class When_registering_a_transport_message_with_multiple_messages:WithInspector<MessageTypesInspector>
    {
        static TransportMessage envelopeToInspect;

        Establish context = () => {
           envelopeToInspect= new TransportMessage
                                  {
                                      Headers=new Dictionary<string, string>()
                                  };
                                      envelopeToInspect.Headers[EnclosedMessageTypesMutator.EnclosedMessageTypes] =
                                          typeof (TestMessage).AssemblyQualifiedName + ";" +
                                          typeof (TestMessage).AssemblyQualifiedName;
        };

        Because of = () => MessageInspection(envelopeToInspect);


        It should_register_the_types_for_all_of_them = () => Results.Count().ShouldEqual(2);

        It should_assign_a_id = () => AssertFirst<RegisterMessageType>(m => m.MessageTypeId == m.MessageType.ToGuid());
    
        It should_parse_the_message_type = () => AssertFirst<RegisterMessageType>(m=> m.MessageType == messageType.TypeName);
        
        It should_parse_the_message_version = () => AssertFirst<RegisterMessageType>(m => m.MessageVersion == messageType.Version.ToString());
        
        static readonly MessageType messageType = new MessageType(typeof(TestMessage));

        class TestMessage
        {
             
        }
    }

}
