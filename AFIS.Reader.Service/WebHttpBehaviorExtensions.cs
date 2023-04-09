using System;
using System.Collections.Generic;
using System.Net;
using System.ServiceModel.Channels;
using System.ServiceModel.Configuration;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;

namespace AFIS.Reader.Service
{
    public class CustomHeaderMessageInspector : IDispatchMessageInspector
    {
        Dictionary<string, string> requiredHeaders;
        public CustomHeaderMessageInspector(Dictionary<string, string> headers)
        {
            requiredHeaders = headers ?? new Dictionary<string, string>();
        }

        public object AfterReceiveRequest(ref System.ServiceModel.Channels.Message request, System.ServiceModel.IClientChannel channel, System.ServiceModel.InstanceContext instanceContext)
        {
            //return null;
            var httpRequest = (HttpRequestMessageProperty)request.Properties[HttpRequestMessageProperty.Name];
            return new
            {
                origin = httpRequest.Headers["Origin"],
                handlePreflight = httpRequest.Method.Equals("OPTIONS",
                StringComparison.InvariantCultureIgnoreCase)
            };
        }

        public void BeforeSendReply(ref System.ServiceModel.Channels.Message reply, object correlationState)
        {
            var state = (dynamic)correlationState;
            if (state.handlePreflight)
            {
                reply = Message.CreateMessage(MessageVersion.None, "PreflightReturn");

                var httpResponse = new HttpResponseMessageProperty();
                reply.Properties.Add(HttpResponseMessageProperty.Name, httpResponse);

                httpResponse.SuppressEntityBody = true;
                httpResponse.StatusCode = HttpStatusCode.OK;
            }
            var httpHeader = reply.Properties["httpResponse"] as HttpResponseMessageProperty;
            foreach (var item in requiredHeaders)
            {
                httpHeader.Headers.Add(item.Key, item.Value);
            }
        }
    }

    class EnableCorsBehavior : BehaviorExtensionElement, IEndpointBehavior
    {
        public void AddBindingParameters(ServiceEndpoint endpoint, System.ServiceModel.Channels.BindingParameterCollection bindingParameters)
        { }

        public void ApplyClientBehavior(ServiceEndpoint endpoint, System.ServiceModel.Dispatcher.ClientRuntime clientRuntime)
        { }

        public void ApplyDispatchBehavior(ServiceEndpoint endpoint, System.ServiceModel.Dispatcher.EndpointDispatcher endpointDispatcher)
        {
            var requiredHeaders = new Dictionary<string, string>();

            requiredHeaders.Add("Access-Control-Allow-Origin", "*");
            requiredHeaders.Add("Allow", "TRACE,HEAD,POST,GET,PUT,DELETE,OPTIONS");
            requiredHeaders.Add("Access-Control-Allow-Methods", "TRACE,HEAD,POST,GET,PUT,DELETE,OPTIONS");
            requiredHeaders.Add("Access-Control-Request-Method", "POST,GET,PUT,DELETE,OPTIONS");
            requiredHeaders.Add("Access-Control-Allow-Headers", "Content-Type, Authorization, Accept, X-Requested-With");

            endpointDispatcher.DispatchRuntime.MessageInspectors.Add(new CustomHeaderMessageInspector(requiredHeaders));
        }

        public void Validate(ServiceEndpoint endpoint) { }

        public override Type BehaviorType
        {
            get { return typeof(EnableCorsBehavior); }
        }

        protected override object CreateBehavior()
        {
            return new EnableCorsBehavior();
        }
    }

}
