using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using shareshare.Message;
using Microsoft.Practices.Unity;


namespace shareshare.PriceServer
{



    public class StaticMap
    {

        private Dictionary<string, RequestMessage> _dict = new Dictionary<string, RequestMessage>();
        public StaticMap(IUnityContainer myContainer) 
        {
            _dict[Constant.MSG_REGISTERREQ] = myContainer.Resolve<RegisterRequestMessage>();
            _dict[Constant.MSG_LOGINREQ] = myContainer.Resolve<LoginRequestMessage>();
            _dict[Constant.MSG_CREATE_ACCOUNT] =  myContainer.Resolve<CreateAccountRequestMessage>();
            _dict[Constant.MSG_RESET_ACCOUNT] = myContainer.Resolve<ResetAccountMessage>();
            _dict[Constant.MSG_SUBSCRIBE] = myContainer.Resolve<MarketRequestMessage>();
            _dict[Constant.MSG_UNSUBSCRIBE] = myContainer.Resolve<MarketCancelMessage>();
            _dict[Constant.MSG_NEW_ORDER_REQ] = myContainer.Resolve<NewOrderReqMessage>();
            _dict[Constant.MSG_UPDATE_ORDER_REQ] = myContainer.Resolve<UpdateOrderReqMessage>();
            _dict[Constant.MSG_CANCEL_ORDER_REQ] = myContainer.Resolve<CancelOrderReqMessage>();
            _dict[Constant.MSG_LOGOUTREQ] = myContainer.Resolve<LogoutRequestMessage>();
            _dict[Constant.MSG_ACCOUNT_TRANSFER_CASH] = myContainer.Resolve<AccountTransferMessage>();
             
           
  
        }

        public RequestMessage GetRequestMessage(string value)
        {
            if (_dict.ContainsKey(value))
            {
                return _dict[value];
            }
            return null;

        }

    }



    public class MessageConverter : JsonCreationConverter<RequestMessage>
    {
        StaticMap _staticmap = null;
        IUnityContainer _myContainer;
        public MessageConverter(IUnityContainer myContainer)
        {
            _myContainer = myContainer;
        }
       // 

        /*here we can pre-load to memory*/
        protected override RequestMessage Create(Type objectType, JObject jObject)
        {
            string value = ActionValue("action", jObject);

            switch (value)
            {
                case (Constant.MSG_REGISTERREQ):
                    return _myContainer.Resolve<RegisterRequestMessage>();
                case (Constant.MSG_LOGINREQ):
                    return _myContainer.Resolve<LoginRequestMessage>();
                case (Constant.MSG_CREATE_ACCOUNT):
                    return _myContainer.Resolve<CreateAccountRequestMessage>();
                case (Constant.MSG_RESET_ACCOUNT):
                    return _myContainer.Resolve<ResetAccountMessage>();
                case (Constant.MSG_SUBSCRIBE):
                    return _myContainer.Resolve<MarketRequestMessage>();
                case (Constant.MSG_UNSUBSCRIBE):
                    return _myContainer.Resolve<MarketCancelMessage>();
                case (Constant.MSG_NEW_ORDER_REQ):
                    return _myContainer.Resolve<NewOrderReqMessage>();
                case (Constant.MSG_UPDATE_ORDER_REQ):
                    return _myContainer.Resolve<UpdateOrderReqMessage>();
                case (Constant.MSG_CANCEL_ORDER_REQ):
                    return _myContainer.Resolve<CancelOrderReqMessage>();
                case (Constant.MSG_LOGOUTREQ):
                    return _myContainer.Resolve<LogoutRequestMessage>();
                case (Constant.MSG_ACCOUNT_TRANSFER_CASH):
                    return _myContainer.Resolve<AccountTransferMessage>();
                default:
                    return null;
            }


            return _staticmap.GetRequestMessage(value);
        }

        private string ActionValue(string fieldName, JObject jObject)
        {
            if (jObject[fieldName] != null)
            {
                return jObject[fieldName].Value<string>();
            }
            return "";
        }
    }

    public abstract class JsonCreationConverter<T> : JsonConverter
    {
        /// <summary>
        /// Create an instance of objectType, based properties in the JSON object
        /// </summary>
        /// <param name="objectType">type of object expected</param>
        /// <param name="jObject">
        /// contents of JSON object that will be deserialized
        /// </param>
        /// <returns></returns>
        protected abstract T Create(Type objectType, JObject jObject);

        public override bool CanConvert(Type objectType)
        {
            return typeof(T).IsAssignableFrom(objectType);
        }

        public override object ReadJson(JsonReader reader,
                                        Type objectType,
                                         object existingValue,
                                         JsonSerializer serializer)
        {
            // Load JObject from stream
            JObject jObject = JObject.Load(reader);

            // Create target object based on JObject
            T target = Create(objectType, jObject);

            // Populate the object properties
            serializer.Populate(jObject.CreateReader(), target);

            return target;
        }

        public override void WriteJson(JsonWriter writer,
                                       object value,
                                       JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
