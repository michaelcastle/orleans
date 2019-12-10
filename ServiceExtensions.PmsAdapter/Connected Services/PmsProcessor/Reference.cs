﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     //
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System.Runtime.Serialization;

namespace ServiceExtensions.PmsAdapter.Connected_Services.PmsProcessor
{
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [DataContract(Name="InterfaceReturn", Namespace="http://schemas.datacontract.org/2004/07/Optii.PMS.Server")]
    public partial class InterfaceReturn : object
    {
        
        private string FailureReasonField;
        
        private int ReceiptNoField;
        
        private PmsProcessor.InterfaceReturn.enReturnType ReturnTypeField;
        
        private System.DateTimeOffset TimestampField;
        
        [DataMember()]
        public string FailureReason
        {
            get
            {
                return this.FailureReasonField;
            }
            set
            {
                this.FailureReasonField = value;
            }
        }
        
        [DataMember()]
        public int ReceiptNo
        {
            get
            {
                return this.ReceiptNoField;
            }
            set
            {
                this.ReceiptNoField = value;
            }
        }
        
        [DataMember()]
        public PmsProcessor.InterfaceReturn.enReturnType ReturnType
        {
            get
            {
                return this.ReturnTypeField;
            }
            set
            {
                this.ReturnTypeField = value;
            }
        }
        
        [DataMember()]
        public System.DateTimeOffset Timestamp
        {
            get
            {
                return this.TimestampField;
            }
            set
            {
                this.TimestampField = value;
            }
        }
        
        [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
        [DataContract(Name="InterfaceReturn.enReturnType", Namespace="http://schemas.datacontract.org/2004/07/Optii.PMS.Server")]
        public enum enReturnType : int
        {
            
            [EnumMember()]
            Success = 0,
            
            [EnumMember()]
            Failure = 1,
            
            [EnumMember()]
            SessionEnded = 2,
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [DataContract(Name="InterfaceToken", Namespace="http://schemas.datacontract.org/2004/07/Optii.PMS.Server")]
    public partial class InterfaceToken : object
    {
        
        private string InterfaceInboundTypeField;
        
        private string InterfaceOutboundTypeField;
        
        private System.Collections.Generic.Dictionary<string, string> InterfaceTokenPropertiesField;
        
        private System.Guid SessionIdField;
        
        [DataMember()]
        public string InterfaceInboundType
        {
            get
            {
                return this.InterfaceInboundTypeField;
            }
            set
            {
                this.InterfaceInboundTypeField = value;
            }
        }
        
        [DataMember()]
        public string InterfaceOutboundType
        {
            get
            {
                return this.InterfaceOutboundTypeField;
            }
            set
            {
                this.InterfaceOutboundTypeField = value;
            }
        }
        
        [DataMember()]
        public System.Collections.Generic.Dictionary<string, string> InterfaceTokenProperties
        {
            get
            {
                return this.InterfaceTokenPropertiesField;
            }
            set
            {
                this.InterfaceTokenPropertiesField = value;
            }
        }
        
        [DataMember()]
        public System.Guid SessionId
        {
            get
            {
                return this.SessionIdField;
            }
            set
            {
                this.SessionIdField = value;
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [DataContract(Name="InterfaceLogDetail", Namespace="http://schemas.datacontract.org/2004/07/Optii.PMS.Server")]
    public partial class InterfaceLogDetail : object
    {
        
        private string ContentsField;
        
        private int InterfaceLogIdField;
        
        [DataMember()]
        public string Contents
        {
            get
            {
                return this.ContentsField;
            }
            set
            {
                this.ContentsField = value;
            }
        }
        
        [DataMember()]
        public int InterfaceLogId
        {
            get
            {
                return this.InterfaceLogIdField;
            }
            set
            {
                this.InterfaceLogIdField = value;
            }
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [DataContract(Name="SystemAlertType", Namespace="http://schemas.datacontract.org/2004/07/Optii.Core.Enums")]
    public enum SystemAlertType : int
    {
        
        [EnumMember()]
        InterfaceUptimeAlert = 1,
        
        [EnumMember()]
        SystemConnectivityAlert = 2,
        
        [EnumMember()]
        MessageTooLarge = 3,
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="PmsProcessor.IPMSInterfaceContract")]
    public interface IPMSInterfaceContract
    {
        
        // CODEGEN: Generating message contract since the operation has multiple return values.
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IPMSInterfaceContract/Signin", ReplyAction="http://tempuri.org/IPMSInterfaceContract/SigninResponse")]
        System.Threading.Tasks.Task<PmsProcessor.SigninResponse> SigninAsync(PmsProcessor.SigninRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IPMSInterfaceContract/SubmitMessage", ReplyAction="http://tempuri.org/IPMSInterfaceContract/SubmitMessageResponse")]
        System.Threading.Tasks.Task<PmsProcessor.InterfaceReturn> SubmitMessageAsync(System.Guid SessionId, string MessageString);
        
        // CODEGEN: Generating message contract since the operation has multiple return values.
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IPMSInterfaceContract/GetOutboundMessage", ReplyAction="http://tempuri.org/IPMSInterfaceContract/GetOutboundMessageResponse")]
        System.Threading.Tasks.Task<PmsProcessor.GetOutboundMessageResponse> GetOutboundMessageAsync(PmsProcessor.GetOutboundMessageRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IPMSInterfaceContract/ProcessOutboundMessage", ReplyAction="http://tempuri.org/IPMSInterfaceContract/ProcessOutboundMessageResponse")]
        System.Threading.Tasks.Task<PmsProcessor.InterfaceReturn> ProcessOutboundMessageAsync(System.Guid SessionId, int QueueId, string Message);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IPMSInterfaceContract/PassOutboundMessage", ReplyAction="http://tempuri.org/IPMSInterfaceContract/PassOutboundMessageResponse")]
        System.Threading.Tasks.Task<PmsProcessor.InterfaceReturn> PassOutboundMessageAsync(System.Guid SessionId, int QueueId);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IPMSInterfaceContract/FailQueueItem", ReplyAction="http://tempuri.org/IPMSInterfaceContract/FailQueueItemResponse")]
        System.Threading.Tasks.Task<PmsProcessor.InterfaceReturn> FailQueueItemAsync(System.Guid SessionId, int QueueId, string Reason);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IPMSInterfaceContract/SubmitMessageWithPreviousInterfaceLogId", ReplyAction="http://tempuri.org/IPMSInterfaceContract/SubmitMessageWithPreviousInterfaceLogIdR" +
            "esponse")]
        System.Threading.Tasks.Task<PmsProcessor.InterfaceReturn> SubmitMessageWithPreviousInterfaceLogIdAsync(System.Guid SessionId, string MessageString, int InterfaceLogId);
        
        // CODEGEN: Generating message contract since the operation has multiple return values.
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IPMSInterfaceContract/FindNextInterfaceMessage", ReplyAction="http://tempuri.org/IPMSInterfaceContract/FindNextInterfaceMessageResponse")]
        System.Threading.Tasks.Task<PmsProcessor.FindNextInterfaceMessageResponse> FindNextInterfaceMessageAsync(PmsProcessor.FindNextInterfaceMessageRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IPMSInterfaceContract/RaiseInterfaceUptimeAlert", ReplyAction="http://tempuri.org/IPMSInterfaceContract/RaiseInterfaceUptimeAlertResponse")]
        System.Threading.Tasks.Task<PmsProcessor.InterfaceReturn> RaiseInterfaceUptimeAlertAsync(System.Guid sessionId, System.DateTimeOffset timeOfDetection, string errorInformation);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IPMSInterfaceContract/RaiseSystemAlert", ReplyAction="http://tempuri.org/IPMSInterfaceContract/RaiseSystemAlertResponse")]
        System.Threading.Tasks.Task<PmsProcessor.InterfaceReturn> RaiseSystemAlertAsync(System.Guid sessionId, System.DateTimeOffset timeOfDetection, PmsProcessor.SystemAlertType systemAlertType, string errorInformation);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IPMSInterfaceContract/PmsAdapterHealthCheck", ReplyAction="http://tempuri.org/IPMSInterfaceContract/PmsAdapterHealthCheckResponse")]
        System.Threading.Tasks.Task<PmsProcessor.InterfaceReturn> PmsAdapterHealthCheckAsync();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IPMSInterfaceContract/PMSAdapterHeartBeat", ReplyAction="http://tempuri.org/IPMSInterfaceContract/PMSAdapterHeartBeatResponse")]
        System.Threading.Tasks.Task<PmsProcessor.InterfaceReturn> PMSAdapterHeartBeatAsync(System.Guid sessionId);
        
        // CODEGEN: Generating message contract since the operation has multiple return values.
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IPMSInterfaceContract/GetConfigSetting", ReplyAction="http://tempuri.org/IPMSInterfaceContract/GetConfigSettingResponse")]
        System.Threading.Tasks.Task<PmsProcessor.GetConfigSettingResponse> GetConfigSettingAsync(PmsProcessor.GetConfigSettingRequest request);
        
        // CODEGEN: Generating message contract since the operation has multiple return values.
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IPMSInterfaceContract/GetLastCompleteQueueItem", ReplyAction="http://tempuri.org/IPMSInterfaceContract/GetLastCompleteQueueItemResponse")]
        System.Threading.Tasks.Task<PmsProcessor.GetLastCompleteQueueItemResponse> GetLastCompleteQueueItemAsync(PmsProcessor.GetLastCompleteQueueItemRequest request);
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ServiceModel.MessageContractAttribute(WrapperName="Signin", WrapperNamespace="http://tempuri.org/", IsWrapped=true)]
    public partial class SigninRequest
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://tempuri.org/", Order=0)]
        public string Username;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://tempuri.org/", Order=1)]
        public string Password;
        
        public SigninRequest()
        {
        }
        
        public SigninRequest(string Username, string Password)
        {
            this.Username = Username;
            this.Password = Password;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ServiceModel.MessageContractAttribute(WrapperName="SigninResponse", WrapperNamespace="http://tempuri.org/", IsWrapped=true)]
    public partial class SigninResponse
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://tempuri.org/", Order=0)]
        public PmsProcessor.InterfaceReturn SigninResult;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://tempuri.org/", Order=1)]
        public PmsProcessor.InterfaceToken InterfaceSigninValues;
        
        public SigninResponse()
        {
        }
        
        public SigninResponse(PmsProcessor.InterfaceReturn SigninResult, PmsProcessor.InterfaceToken InterfaceSigninValues)
        {
            this.SigninResult = SigninResult;
            this.InterfaceSigninValues = InterfaceSigninValues;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ServiceModel.MessageContractAttribute(WrapperName="GetOutboundMessage", WrapperNamespace="http://tempuri.org/", IsWrapped=true)]
    public partial class GetOutboundMessageRequest
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://tempuri.org/", Order=0)]
        public System.Guid SessionId;
        
        public GetOutboundMessageRequest()
        {
        }
        
        public GetOutboundMessageRequest(System.Guid SessionId)
        {
            this.SessionId = SessionId;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ServiceModel.MessageContractAttribute(WrapperName="GetOutboundMessageResponse", WrapperNamespace="http://tempuri.org/", IsWrapped=true)]
    public partial class GetOutboundMessageResponse
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://tempuri.org/", Order=0)]
        public PmsProcessor.InterfaceReturn GetOutboundMessageResult;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://tempuri.org/", Order=1)]
        public bool HasQueueItem;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://tempuri.org/", Order=2)]
        public int QueueId;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://tempuri.org/", Order=3)]
        public string Message;
        
        public GetOutboundMessageResponse()
        {
        }
        
        public GetOutboundMessageResponse(PmsProcessor.InterfaceReturn GetOutboundMessageResult, bool HasQueueItem, int QueueId, string Message)
        {
            this.GetOutboundMessageResult = GetOutboundMessageResult;
            this.HasQueueItem = HasQueueItem;
            this.QueueId = QueueId;
            this.Message = Message;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ServiceModel.MessageContractAttribute(WrapperName="FindNextInterfaceMessage", WrapperNamespace="http://tempuri.org/", IsWrapped=true)]
    public partial class FindNextInterfaceMessageRequest
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://tempuri.org/", Order=0)]
        public System.Guid SessionId;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://tempuri.org/", Order=1)]
        public int LastInterfaceLogId;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://tempuri.org/", Order=2)]
        public int LookbackPeriod;
        
        public FindNextInterfaceMessageRequest()
        {
        }
        
        public FindNextInterfaceMessageRequest(System.Guid SessionId, int LastInterfaceLogId, int LookbackPeriod)
        {
            this.SessionId = SessionId;
            this.LastInterfaceLogId = LastInterfaceLogId;
            this.LookbackPeriod = LookbackPeriod;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ServiceModel.MessageContractAttribute(WrapperName="FindNextInterfaceMessageResponse", WrapperNamespace="http://tempuri.org/", IsWrapped=true)]
    public partial class FindNextInterfaceMessageResponse
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://tempuri.org/", Order=0)]
        public PmsProcessor.InterfaceReturn FindNextInterfaceMessageResult;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://tempuri.org/", Order=1)]
        public PmsProcessor.InterfaceLogDetail InterfaceLog;
        
        public FindNextInterfaceMessageResponse()
        {
        }
        
        public FindNextInterfaceMessageResponse(PmsProcessor.InterfaceReturn FindNextInterfaceMessageResult, PmsProcessor.InterfaceLogDetail InterfaceLog)
        {
            this.FindNextInterfaceMessageResult = FindNextInterfaceMessageResult;
            this.InterfaceLog = InterfaceLog;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ServiceModel.MessageContractAttribute(WrapperName="GetConfigSetting", WrapperNamespace="http://tempuri.org/", IsWrapped=true)]
    public partial class GetConfigSettingRequest
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://tempuri.org/", Order=0)]
        public System.Guid sessionId;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://tempuri.org/", Order=1)]
        public int assetTreePropertyType;
        
        public GetConfigSettingRequest()
        {
        }
        
        public GetConfigSettingRequest(System.Guid sessionId, int assetTreePropertyType)
        {
            this.sessionId = sessionId;
            this.assetTreePropertyType = assetTreePropertyType;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ServiceModel.MessageContractAttribute(WrapperName="GetConfigSettingResponse", WrapperNamespace="http://tempuri.org/", IsWrapped=true)]
    public partial class GetConfigSettingResponse
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://tempuri.org/", Order=0)]
        public PmsProcessor.InterfaceReturn GetConfigSettingResult;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://tempuri.org/", Order=1)]
        public string settingValue;
        
        public GetConfigSettingResponse()
        {
        }
        
        public GetConfigSettingResponse(PmsProcessor.InterfaceReturn GetConfigSettingResult, string settingValue)
        {
            this.GetConfigSettingResult = GetConfigSettingResult;
            this.settingValue = settingValue;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ServiceModel.MessageContractAttribute(WrapperName="GetLastCompleteQueueItem", WrapperNamespace="http://tempuri.org/", IsWrapped=true)]
    public partial class GetLastCompleteQueueItemRequest
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://tempuri.org/", Order=0)]
        public System.Guid sessionId;
        
        public GetLastCompleteQueueItemRequest()
        {
        }
        
        public GetLastCompleteQueueItemRequest(System.Guid sessionId)
        {
            this.sessionId = sessionId;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ServiceModel.MessageContractAttribute(WrapperName="GetLastCompleteQueueItemResponse", WrapperNamespace="http://tempuri.org/", IsWrapped=true)]
    public partial class GetLastCompleteQueueItemResponse
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://tempuri.org/", Order=0)]
        public PmsProcessor.InterfaceReturn GetLastCompleteQueueItemResult;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://tempuri.org/", Order=1)]
        public System.Nullable<System.DateTimeOffset> lastQueueCompletedTimestamp;
        
        public GetLastCompleteQueueItemResponse()
        {
        }
        
        public GetLastCompleteQueueItemResponse(PmsProcessor.InterfaceReturn GetLastCompleteQueueItemResult, System.Nullable<System.DateTimeOffset> lastQueueCompletedTimestamp)
        {
            this.GetLastCompleteQueueItemResult = GetLastCompleteQueueItemResult;
            this.lastQueueCompletedTimestamp = lastQueueCompletedTimestamp;
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    public interface IPMSInterfaceContractChannel : PmsProcessor.IPMSInterfaceContract, System.ServiceModel.IClientChannel
    {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    public partial class PMSInterfaceContractClient : System.ServiceModel.ClientBase<PmsProcessor.IPMSInterfaceContract>, PmsProcessor.IPMSInterfaceContract
    {
        
    /// <summary>
    /// Implement this partial method to configure the service endpoint.
    /// </summary>
    /// <param name="serviceEndpoint">The endpoint to configure</param>
    /// <param name="clientCredentials">The client credentials</param>
    static partial void ConfigureEndpoint(System.ServiceModel.Description.ServiceEndpoint serviceEndpoint, System.ServiceModel.Description.ClientCredentials clientCredentials);
        
        public PMSInterfaceContractClient() : 
                base(PMSInterfaceContractClient.GetDefaultBinding(), PMSInterfaceContractClient.GetDefaultEndpointAddress())
        {
            this.Endpoint.Name = EndpointConfiguration.BasicHttpBinding_IPMSInterfaceContract.ToString();
            ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
        }
        
        public PMSInterfaceContractClient(EndpointConfiguration endpointConfiguration) : 
                base(PMSInterfaceContractClient.GetBindingForEndpoint(endpointConfiguration), PMSInterfaceContractClient.GetEndpointAddress(endpointConfiguration))
        {
            this.Endpoint.Name = endpointConfiguration.ToString();
            ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
        }
        
        public PMSInterfaceContractClient(EndpointConfiguration endpointConfiguration, string remoteAddress) : 
                base(PMSInterfaceContractClient.GetBindingForEndpoint(endpointConfiguration), new System.ServiceModel.EndpointAddress(remoteAddress))
        {
            this.Endpoint.Name = endpointConfiguration.ToString();
            ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
        }
        
        public PMSInterfaceContractClient(EndpointConfiguration endpointConfiguration, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(PMSInterfaceContractClient.GetBindingForEndpoint(endpointConfiguration), remoteAddress)
        {
            this.Endpoint.Name = endpointConfiguration.ToString();
            ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
        }
        
        public PMSInterfaceContractClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress)
        {
        }
        
        public System.Threading.Tasks.Task<PmsProcessor.SigninResponse> SigninAsync(PmsProcessor.SigninRequest request)
        {
            return base.Channel.SigninAsync(request);
        }
        
        public System.Threading.Tasks.Task<PmsProcessor.InterfaceReturn> SubmitMessageAsync(System.Guid SessionId, string MessageString)
        {
            return base.Channel.SubmitMessageAsync(SessionId, MessageString);
        }
        
        public System.Threading.Tasks.Task<PmsProcessor.GetOutboundMessageResponse> GetOutboundMessageAsync(PmsProcessor.GetOutboundMessageRequest request)
        {
            return base.Channel.GetOutboundMessageAsync(request);
        }
        
        public System.Threading.Tasks.Task<PmsProcessor.InterfaceReturn> ProcessOutboundMessageAsync(System.Guid SessionId, int QueueId, string Message)
        {
            return base.Channel.ProcessOutboundMessageAsync(SessionId, QueueId, Message);
        }
        
        public System.Threading.Tasks.Task<PmsProcessor.InterfaceReturn> PassOutboundMessageAsync(System.Guid SessionId, int QueueId)
        {
            return base.Channel.PassOutboundMessageAsync(SessionId, QueueId);
        }
        
        public System.Threading.Tasks.Task<PmsProcessor.InterfaceReturn> FailQueueItemAsync(System.Guid SessionId, int QueueId, string Reason)
        {
            return base.Channel.FailQueueItemAsync(SessionId, QueueId, Reason);
        }
        
        public System.Threading.Tasks.Task<PmsProcessor.InterfaceReturn> SubmitMessageWithPreviousInterfaceLogIdAsync(System.Guid SessionId, string MessageString, int InterfaceLogId)
        {
            return base.Channel.SubmitMessageWithPreviousInterfaceLogIdAsync(SessionId, MessageString, InterfaceLogId);
        }
        
        public System.Threading.Tasks.Task<PmsProcessor.FindNextInterfaceMessageResponse> FindNextInterfaceMessageAsync(PmsProcessor.FindNextInterfaceMessageRequest request)
        {
            return base.Channel.FindNextInterfaceMessageAsync(request);
        }
        
        public System.Threading.Tasks.Task<PmsProcessor.InterfaceReturn> RaiseInterfaceUptimeAlertAsync(System.Guid sessionId, System.DateTimeOffset timeOfDetection, string errorInformation)
        {
            return base.Channel.RaiseInterfaceUptimeAlertAsync(sessionId, timeOfDetection, errorInformation);
        }
        
        public System.Threading.Tasks.Task<PmsProcessor.InterfaceReturn> RaiseSystemAlertAsync(System.Guid sessionId, System.DateTimeOffset timeOfDetection, PmsProcessor.SystemAlertType systemAlertType, string errorInformation)
        {
            return base.Channel.RaiseSystemAlertAsync(sessionId, timeOfDetection, systemAlertType, errorInformation);
        }
        
        public System.Threading.Tasks.Task<PmsProcessor.InterfaceReturn> PmsAdapterHealthCheckAsync()
        {
            return base.Channel.PmsAdapterHealthCheckAsync();
        }
        
        public System.Threading.Tasks.Task<PmsProcessor.InterfaceReturn> PMSAdapterHeartBeatAsync(System.Guid sessionId)
        {
            return base.Channel.PMSAdapterHeartBeatAsync(sessionId);
        }
        
        public System.Threading.Tasks.Task<PmsProcessor.GetConfigSettingResponse> GetConfigSettingAsync(PmsProcessor.GetConfigSettingRequest request)
        {
            return base.Channel.GetConfigSettingAsync(request);
        }
        
        public System.Threading.Tasks.Task<PmsProcessor.GetLastCompleteQueueItemResponse> GetLastCompleteQueueItemAsync(PmsProcessor.GetLastCompleteQueueItemRequest request)
        {
            return base.Channel.GetLastCompleteQueueItemAsync(request);
        }
        
        public virtual System.Threading.Tasks.Task OpenAsync()
        {
            return System.Threading.Tasks.Task.Factory.FromAsync(((System.ServiceModel.ICommunicationObject)(this)).BeginOpen(null, null), new System.Action<System.IAsyncResult>(((System.ServiceModel.ICommunicationObject)(this)).EndOpen));
        }
        
        public virtual System.Threading.Tasks.Task CloseAsync()
        {
            return System.Threading.Tasks.Task.Factory.FromAsync(((System.ServiceModel.ICommunicationObject)(this)).BeginClose(null, null), new System.Action<System.IAsyncResult>(((System.ServiceModel.ICommunicationObject)(this)).EndClose));
        }
        
        private static System.ServiceModel.Channels.Binding GetBindingForEndpoint(EndpointConfiguration endpointConfiguration)
        {
            if ((endpointConfiguration == EndpointConfiguration.BasicHttpBinding_IPMSInterfaceContract))
            {
                System.ServiceModel.BasicHttpBinding result = new System.ServiceModel.BasicHttpBinding();
                result.MaxBufferSize = int.MaxValue;
                result.ReaderQuotas = System.Xml.XmlDictionaryReaderQuotas.Max;
                result.MaxReceivedMessageSize = int.MaxValue;
                result.AllowCookies = true;
                return result;
            }
            throw new System.InvalidOperationException(string.Format("Could not find endpoint with name \'{0}\'.", endpointConfiguration));
        }
        
        private static System.ServiceModel.EndpointAddress GetEndpointAddress(EndpointConfiguration endpointConfiguration)
        {
            if ((endpointConfiguration == EndpointConfiguration.BasicHttpBinding_IPMSInterfaceContract))
            {
                return new System.ServiceModel.EndpointAddress("http://localhost/Optii.PMS.Server/PMSProcessor.svc");
            }
            throw new System.InvalidOperationException(string.Format("Could not find endpoint with name \'{0}\'.", endpointConfiguration));
        }
        
        private static System.ServiceModel.Channels.Binding GetDefaultBinding()
        {
            return PMSInterfaceContractClient.GetBindingForEndpoint(EndpointConfiguration.BasicHttpBinding_IPMSInterfaceContract);
        }
        
        private static System.ServiceModel.EndpointAddress GetDefaultEndpointAddress()
        {
            return PMSInterfaceContractClient.GetEndpointAddress(EndpointConfiguration.BasicHttpBinding_IPMSInterfaceContract);
        }
        
        public enum EndpointConfiguration
        {
            
            BasicHttpBinding_IPMSInterfaceContract,
        }
    }
}
