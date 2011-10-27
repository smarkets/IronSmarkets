// Copyright (c) 2011 Smarkets Limited
//
// Permission is hereby granted, free of charge, to any person
// obtaining a copy of this software and associated documentation
// files (the "Software"), to deal in the Software without
// restriction, including without limitation the rights to use, copy,
// modify, merge, publish, distribute, sublicense, and/or sell copies
// of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS
// BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN
// ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN
// CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.

namespace IronSmarkets.Proto.Seto
{
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"event")]
  [global::System.Runtime.Serialization.DataContract(Name=@"event")]
  public partial class Event : global::ProtoBuf.IExtensible, global::System.Runtime.Serialization.ISerializable
  {
    public Event() {}
    
    private ulong _low;
    [global::ProtoBuf.ProtoMember(1, IsRequired = true, Name=@"low", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.Runtime.Serialization.DataMember(Name=@"low", Order = 1, IsRequired = true)]
    
    public ulong Low
    {
      get { return _low; }
      set { _low = value; }
    }

    private ulong _high = (ulong)0;
    [global::ProtoBuf.ProtoMember(2, IsRequired = false, Name=@"high", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.ComponentModel.DefaultValue((ulong)0)]
    [global::System.Runtime.Serialization.DataMember(Name=@"high", Order = 2, IsRequired = false)]
    
    public ulong High
    {
      get { return _high; }
      set { _high = value; }
    }
    protected Event(global::System.Runtime.Serialization.SerializationInfo info, global::System.Runtime.Serialization.StreamingContext context)
      : this() { global::ProtoBuf.Serializer.Merge(info, this); }
    void global::System.Runtime.Serialization.ISerializable.GetObjectData(global::System.Runtime.Serialization.SerializationInfo info, global::System.Runtime.Serialization.StreamingContext context)
      { global::ProtoBuf.Serializer.Serialize(info, this); }
    
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"market")]
  [global::System.Runtime.Serialization.DataContract(Name=@"market")]
  public partial class Market : global::ProtoBuf.IExtensible, global::System.Runtime.Serialization.ISerializable
  {
    public Market() {}
    
    private ulong _low;
    [global::ProtoBuf.ProtoMember(1, IsRequired = true, Name=@"low", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.Runtime.Serialization.DataMember(Name=@"low", Order = 1, IsRequired = true)]
    
    public ulong Low
    {
      get { return _low; }
      set { _low = value; }
    }

    private ulong _high = (ulong)0;
    [global::ProtoBuf.ProtoMember(2, IsRequired = false, Name=@"high", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.ComponentModel.DefaultValue((ulong)0)]
    [global::System.Runtime.Serialization.DataMember(Name=@"high", Order = 2, IsRequired = false)]
    
    public ulong High
    {
      get { return _high; }
      set { _high = value; }
    }
    protected Market(global::System.Runtime.Serialization.SerializationInfo info, global::System.Runtime.Serialization.StreamingContext context)
      : this() { global::ProtoBuf.Serializer.Merge(info, this); }
    void global::System.Runtime.Serialization.ISerializable.GetObjectData(global::System.Runtime.Serialization.SerializationInfo info, global::System.Runtime.Serialization.StreamingContext context)
      { global::ProtoBuf.Serializer.Serialize(info, this); }
    
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"contract")]
  [global::System.Runtime.Serialization.DataContract(Name=@"contract")]
  public partial class Contract : global::ProtoBuf.IExtensible, global::System.Runtime.Serialization.ISerializable
  {
    public Contract() {}
    
    private ulong _low;
    [global::ProtoBuf.ProtoMember(1, IsRequired = true, Name=@"low", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.Runtime.Serialization.DataMember(Name=@"low", Order = 1, IsRequired = true)]
    
    public ulong Low
    {
      get { return _low; }
      set { _low = value; }
    }

    private ulong _high = (ulong)0;
    [global::ProtoBuf.ProtoMember(2, IsRequired = false, Name=@"high", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.ComponentModel.DefaultValue((ulong)0)]
    [global::System.Runtime.Serialization.DataMember(Name=@"high", Order = 2, IsRequired = false)]
    
    public ulong High
    {
      get { return _high; }
      set { _high = value; }
    }
    protected Contract(global::System.Runtime.Serialization.SerializationInfo info, global::System.Runtime.Serialization.StreamingContext context)
      : this() { global::ProtoBuf.Serializer.Merge(info, this); }
    void global::System.Runtime.Serialization.ISerializable.GetObjectData(global::System.Runtime.Serialization.SerializationInfo info, global::System.Runtime.Serialization.StreamingContext context)
      { global::ProtoBuf.Serializer.Serialize(info, this); }
    
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"order")]
  [global::System.Runtime.Serialization.DataContract(Name=@"order")]
  public partial class Order : global::ProtoBuf.IExtensible, global::System.Runtime.Serialization.ISerializable
  {
    public Order() {}
    
    private ulong _low;
    [global::ProtoBuf.ProtoMember(1, IsRequired = true, Name=@"low", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.Runtime.Serialization.DataMember(Name=@"low", Order = 1, IsRequired = true)]
    
    public ulong Low
    {
      get { return _low; }
      set { _low = value; }
    }

    private ulong _high = (ulong)0;
    [global::ProtoBuf.ProtoMember(2, IsRequired = false, Name=@"high", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.ComponentModel.DefaultValue((ulong)0)]
    [global::System.Runtime.Serialization.DataMember(Name=@"high", Order = 2, IsRequired = false)]
    
    public ulong High
    {
      get { return _high; }
      set { _high = value; }
    }
    protected Order(global::System.Runtime.Serialization.SerializationInfo info, global::System.Runtime.Serialization.StreamingContext context)
      : this() { global::ProtoBuf.Serializer.Merge(info, this); }
    void global::System.Runtime.Serialization.ISerializable.GetObjectData(global::System.Runtime.Serialization.SerializationInfo info, global::System.Runtime.Serialization.StreamingContext context)
      { global::ProtoBuf.Serializer.Serialize(info, this); }
    
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"entity")]
  [global::System.Runtime.Serialization.DataContract(Name=@"entity")]
  public partial class Entity : global::ProtoBuf.IExtensible, global::System.Runtime.Serialization.ISerializable
  {
    public Entity() {}
    
    private ulong _low;
    [global::ProtoBuf.ProtoMember(1, IsRequired = true, Name=@"low", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.Runtime.Serialization.DataMember(Name=@"low", Order = 1, IsRequired = true)]
    
    public ulong Low
    {
      get { return _low; }
      set { _low = value; }
    }

    private ulong _high = (ulong)0;
    [global::ProtoBuf.ProtoMember(2, IsRequired = false, Name=@"high", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.ComponentModel.DefaultValue((ulong)0)]
    [global::System.Runtime.Serialization.DataMember(Name=@"high", Order = 2, IsRequired = false)]
    
    public ulong High
    {
      get { return _high; }
      set { _high = value; }
    }
    protected Entity(global::System.Runtime.Serialization.SerializationInfo info, global::System.Runtime.Serialization.StreamingContext context)
      : this() { global::ProtoBuf.Serializer.Merge(info, this); }
    void global::System.Runtime.Serialization.ISerializable.GetObjectData(global::System.Runtime.Serialization.SerializationInfo info, global::System.Runtime.Serialization.StreamingContext context)
      { global::ProtoBuf.Serializer.Serialize(info, this); }
    
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"Payload")]
  [global::System.Runtime.Serialization.DataContract(Name=@"Payload")]
  public partial class Payload : global::ProtoBuf.IExtensible, global::System.Runtime.Serialization.ISerializable
  {
    public Payload() {}
    
    private IronSmarkets.Proto.Seto.PayloadType _type;
    [global::ProtoBuf.ProtoMember(1, IsRequired = true, Name=@"type", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.Runtime.Serialization.DataMember(Name=@"type", Order = 1, IsRequired = true)]
    
    public IronSmarkets.Proto.Seto.PayloadType Type
    {
      get { return _type; }
      set { _type = value; }
    }
    private IronSmarkets.Proto.Eto.Payload _etoPayload;
    [global::ProtoBuf.ProtoMember(2, IsRequired = true, Name=@"eto_payload", DataFormat = global::ProtoBuf.DataFormat.Default)]
    [global::System.Runtime.Serialization.DataMember(Name=@"eto_payload", Order = 2, IsRequired = true)]
    
    public IronSmarkets.Proto.Eto.Payload EtoPayload
    {
      get { return _etoPayload; }
      set { _etoPayload = value; }
    }

    private IronSmarkets.Proto.Seto.Login _login = null;
    [global::ProtoBuf.ProtoMember(3, IsRequired = false, Name=@"login", DataFormat = global::ProtoBuf.DataFormat.Default)]
    [global::System.ComponentModel.DefaultValue(null)]
    [global::System.Runtime.Serialization.DataMember(Name=@"login", Order = 3, IsRequired = false)]
    
    public IronSmarkets.Proto.Seto.Login Login
    {
      get { return _login; }
      set { _login = value; }
    }

    private IronSmarkets.Proto.Seto.OrderCreate _orderCreate = null;
    [global::ProtoBuf.ProtoMember(4, IsRequired = false, Name=@"order_create", DataFormat = global::ProtoBuf.DataFormat.Default)]
    [global::System.ComponentModel.DefaultValue(null)]
    [global::System.Runtime.Serialization.DataMember(Name=@"order_create", Order = 4, IsRequired = false)]
    
    public IronSmarkets.Proto.Seto.OrderCreate OrderCreate
    {
      get { return _orderCreate; }
      set { _orderCreate = value; }
    }

    private IronSmarkets.Proto.Seto.OrderRejected _orderRejected = null;
    [global::ProtoBuf.ProtoMember(5, IsRequired = false, Name=@"order_rejected", DataFormat = global::ProtoBuf.DataFormat.Default)]
    [global::System.ComponentModel.DefaultValue(null)]
    [global::System.Runtime.Serialization.DataMember(Name=@"order_rejected", Order = 5, IsRequired = false)]
    
    public IronSmarkets.Proto.Seto.OrderRejected OrderRejected
    {
      get { return _orderRejected; }
      set { _orderRejected = value; }
    }

    private IronSmarkets.Proto.Seto.OrderAccepted _orderAccepted = null;
    [global::ProtoBuf.ProtoMember(6, IsRequired = false, Name=@"order_accepted", DataFormat = global::ProtoBuf.DataFormat.Default)]
    [global::System.ComponentModel.DefaultValue(null)]
    [global::System.Runtime.Serialization.DataMember(Name=@"order_accepted", Order = 6, IsRequired = false)]
    
    public IronSmarkets.Proto.Seto.OrderAccepted OrderAccepted
    {
      get { return _orderAccepted; }
      set { _orderAccepted = value; }
    }

    private IronSmarkets.Proto.Seto.OrderExecuted _orderExecuted = null;
    [global::ProtoBuf.ProtoMember(7, IsRequired = false, Name=@"order_executed", DataFormat = global::ProtoBuf.DataFormat.Default)]
    [global::System.ComponentModel.DefaultValue(null)]
    [global::System.Runtime.Serialization.DataMember(Name=@"order_executed", Order = 7, IsRequired = false)]
    
    public IronSmarkets.Proto.Seto.OrderExecuted OrderExecuted
    {
      get { return _orderExecuted; }
      set { _orderExecuted = value; }
    }

    private IronSmarkets.Proto.Seto.OrderCancel _orderCancel = null;
    [global::ProtoBuf.ProtoMember(8, IsRequired = false, Name=@"order_cancel", DataFormat = global::ProtoBuf.DataFormat.Default)]
    [global::System.ComponentModel.DefaultValue(null)]
    [global::System.Runtime.Serialization.DataMember(Name=@"order_cancel", Order = 8, IsRequired = false)]
    
    public IronSmarkets.Proto.Seto.OrderCancel OrderCancel
    {
      get { return _orderCancel; }
      set { _orderCancel = value; }
    }

    private IronSmarkets.Proto.Seto.OrderCancelled _orderCancelled = null;
    [global::ProtoBuf.ProtoMember(9, IsRequired = false, Name=@"order_cancelled", DataFormat = global::ProtoBuf.DataFormat.Default)]
    [global::System.ComponentModel.DefaultValue(null)]
    [global::System.Runtime.Serialization.DataMember(Name=@"order_cancelled", Order = 9, IsRequired = false)]
    
    public IronSmarkets.Proto.Seto.OrderCancelled OrderCancelled
    {
      get { return _orderCancelled; }
      set { _orderCancelled = value; }
    }

    private IronSmarkets.Proto.Seto.OrderInvalid _orderInvalid = null;
    [global::ProtoBuf.ProtoMember(10, IsRequired = false, Name=@"order_invalid", DataFormat = global::ProtoBuf.DataFormat.Default)]
    [global::System.ComponentModel.DefaultValue(null)]
    [global::System.Runtime.Serialization.DataMember(Name=@"order_invalid", Order = 10, IsRequired = false)]
    
    public IronSmarkets.Proto.Seto.OrderInvalid OrderInvalid
    {
      get { return _orderInvalid; }
      set { _orderInvalid = value; }
    }

    private IronSmarkets.Proto.Seto.MarketSubscribe _marketSubscribe = null;
    [global::ProtoBuf.ProtoMember(11, IsRequired = false, Name=@"market_subscribe", DataFormat = global::ProtoBuf.DataFormat.Default)]
    [global::System.ComponentModel.DefaultValue(null)]
    [global::System.Runtime.Serialization.DataMember(Name=@"market_subscribe", Order = 11, IsRequired = false)]
    
    public IronSmarkets.Proto.Seto.MarketSubscribe MarketSubscribe
    {
      get { return _marketSubscribe; }
      set { _marketSubscribe = value; }
    }

    private IronSmarkets.Proto.Seto.MarketUnsubscribe _marketUnsubscribe = null;
    [global::ProtoBuf.ProtoMember(12, IsRequired = false, Name=@"market_unsubscribe", DataFormat = global::ProtoBuf.DataFormat.Default)]
    [global::System.ComponentModel.DefaultValue(null)]
    [global::System.Runtime.Serialization.DataMember(Name=@"market_unsubscribe", Order = 12, IsRequired = false)]
    
    public IronSmarkets.Proto.Seto.MarketUnsubscribe MarketUnsubscribe
    {
      get { return _marketUnsubscribe; }
      set { _marketUnsubscribe = value; }
    }

    private IronSmarkets.Proto.Seto.MarketQuotesRequest _marketQuotesRequest = null;
    [global::ProtoBuf.ProtoMember(13, IsRequired = false, Name=@"market_quotes_request", DataFormat = global::ProtoBuf.DataFormat.Default)]
    [global::System.ComponentModel.DefaultValue(null)]
    [global::System.Runtime.Serialization.DataMember(Name=@"market_quotes_request", Order = 13, IsRequired = false)]
    
    public IronSmarkets.Proto.Seto.MarketQuotesRequest MarketQuotesRequest
    {
      get { return _marketQuotesRequest; }
      set { _marketQuotesRequest = value; }
    }

    private IronSmarkets.Proto.Seto.MarketQuotes _marketQuotes = null;
    [global::ProtoBuf.ProtoMember(14, IsRequired = false, Name=@"market_quotes", DataFormat = global::ProtoBuf.DataFormat.Default)]
    [global::System.ComponentModel.DefaultValue(null)]
    [global::System.Runtime.Serialization.DataMember(Name=@"market_quotes", Order = 14, IsRequired = false)]
    
    public IronSmarkets.Proto.Seto.MarketQuotes MarketQuotes
    {
      get { return _marketQuotes; }
      set { _marketQuotes = value; }
    }

    private IronSmarkets.Proto.Seto.ContractQuotes _contractQuotes = null;
    [global::ProtoBuf.ProtoMember(15, IsRequired = false, Name=@"contract_quotes", DataFormat = global::ProtoBuf.DataFormat.Default)]
    [global::System.ComponentModel.DefaultValue(null)]
    [global::System.Runtime.Serialization.DataMember(Name=@"contract_quotes", Order = 15, IsRequired = false)]
    
    public IronSmarkets.Proto.Seto.ContractQuotes ContractQuotes
    {
      get { return _contractQuotes; }
      set { _contractQuotes = value; }
    }

    private IronSmarkets.Proto.Seto.EventsRequest _eventsRequest = null;
    [global::ProtoBuf.ProtoMember(16, IsRequired = false, Name=@"events_request", DataFormat = global::ProtoBuf.DataFormat.Default)]
    [global::System.ComponentModel.DefaultValue(null)]
    [global::System.Runtime.Serialization.DataMember(Name=@"events_request", Order = 16, IsRequired = false)]
    
    public IronSmarkets.Proto.Seto.EventsRequest EventsRequest
    {
      get { return _eventsRequest; }
      set { _eventsRequest = value; }
    }

    private IronSmarkets.Proto.Seto.HttpFound _httpFound = null;
    [global::ProtoBuf.ProtoMember(17, IsRequired = false, Name=@"http_found", DataFormat = global::ProtoBuf.DataFormat.Default)]
    [global::System.ComponentModel.DefaultValue(null)]
    [global::System.Runtime.Serialization.DataMember(Name=@"http_found", Order = 17, IsRequired = false)]
    
    public IronSmarkets.Proto.Seto.HttpFound HttpFound
    {
      get { return _httpFound; }
      set { _httpFound = value; }
    }

    private IronSmarkets.Proto.Seto.InvalidRequest _invalidRequest = null;
    [global::ProtoBuf.ProtoMember(18, IsRequired = false, Name=@"invalid_request", DataFormat = global::ProtoBuf.DataFormat.Default)]
    [global::System.ComponentModel.DefaultValue(null)]
    [global::System.Runtime.Serialization.DataMember(Name=@"invalid_request", Order = 18, IsRequired = false)]
    
    public IronSmarkets.Proto.Seto.InvalidRequest InvalidRequest
    {
      get { return _invalidRequest; }
      set { _invalidRequest = value; }
    }

    private IronSmarkets.Proto.Seto.OrderCancelRejected _orderCancelRejected = null;
    [global::ProtoBuf.ProtoMember(19, IsRequired = false, Name=@"order_cancel_rejected", DataFormat = global::ProtoBuf.DataFormat.Default)]
    [global::System.ComponentModel.DefaultValue(null)]
    [global::System.Runtime.Serialization.DataMember(Name=@"order_cancel_rejected", Order = 19, IsRequired = false)]
    
    public IronSmarkets.Proto.Seto.OrderCancelRejected OrderCancelRejected
    {
      get { return _orderCancelRejected; }
      set { _orderCancelRejected = value; }
    }

    private IronSmarkets.Proto.Seto.OrdersForAccountRequest _ordersForAccountRequest = null;
    [global::ProtoBuf.ProtoMember(20, IsRequired = false, Name=@"orders_for_account_request", DataFormat = global::ProtoBuf.DataFormat.Default)]
    [global::System.ComponentModel.DefaultValue(null)]
    [global::System.Runtime.Serialization.DataMember(Name=@"orders_for_account_request", Order = 20, IsRequired = false)]
    
    public IronSmarkets.Proto.Seto.OrdersForAccountRequest OrdersForAccountRequest
    {
      get { return _ordersForAccountRequest; }
      set { _ordersForAccountRequest = value; }
    }

    private IronSmarkets.Proto.Seto.OrdersForAccount _ordersForAccount = null;
    [global::ProtoBuf.ProtoMember(21, IsRequired = false, Name=@"orders_for_account", DataFormat = global::ProtoBuf.DataFormat.Default)]
    [global::System.ComponentModel.DefaultValue(null)]
    [global::System.Runtime.Serialization.DataMember(Name=@"orders_for_account", Order = 21, IsRequired = false)]
    
    public IronSmarkets.Proto.Seto.OrdersForAccount OrdersForAccount
    {
      get { return _ordersForAccount; }
      set { _ordersForAccount = value; }
    }

    private IronSmarkets.Proto.Seto.OrdersForMarketRequest _ordersForMarketRequest = null;
    [global::ProtoBuf.ProtoMember(22, IsRequired = false, Name=@"orders_for_market_request", DataFormat = global::ProtoBuf.DataFormat.Default)]
    [global::System.ComponentModel.DefaultValue(null)]
    [global::System.Runtime.Serialization.DataMember(Name=@"orders_for_market_request", Order = 22, IsRequired = false)]
    
    public IronSmarkets.Proto.Seto.OrdersForMarketRequest OrdersForMarketRequest
    {
      get { return _ordersForMarketRequest; }
      set { _ordersForMarketRequest = value; }
    }

    private IronSmarkets.Proto.Seto.OrdersForMarket _ordersForMarket = null;
    [global::ProtoBuf.ProtoMember(23, IsRequired = false, Name=@"orders_for_market", DataFormat = global::ProtoBuf.DataFormat.Default)]
    [global::System.ComponentModel.DefaultValue(null)]
    [global::System.Runtime.Serialization.DataMember(Name=@"orders_for_market", Order = 23, IsRequired = false)]
    
    public IronSmarkets.Proto.Seto.OrdersForMarket OrdersForMarket
    {
      get { return _ordersForMarket; }
      set { _ordersForMarket = value; }
    }

    private IronSmarkets.Proto.Seto.AccountStateRequest _accountStateRequest = null;
    [global::ProtoBuf.ProtoMember(24, IsRequired = false, Name=@"account_state_request", DataFormat = global::ProtoBuf.DataFormat.Default)]
    [global::System.ComponentModel.DefaultValue(null)]
    [global::System.Runtime.Serialization.DataMember(Name=@"account_state_request", Order = 24, IsRequired = false)]
    
    public IronSmarkets.Proto.Seto.AccountStateRequest AccountStateRequest
    {
      get { return _accountStateRequest; }
      set { _accountStateRequest = value; }
    }

    private IronSmarkets.Proto.Seto.AccountState _accountState = null;
    [global::ProtoBuf.ProtoMember(25, IsRequired = false, Name=@"account_state", DataFormat = global::ProtoBuf.DataFormat.Default)]
    [global::System.ComponentModel.DefaultValue(null)]
    [global::System.Runtime.Serialization.DataMember(Name=@"account_state", Order = 25, IsRequired = false)]
    
    public IronSmarkets.Proto.Seto.AccountState AccountState
    {
      get { return _accountState; }
      set { _accountState = value; }
    }
    protected Payload(global::System.Runtime.Serialization.SerializationInfo info, global::System.Runtime.Serialization.StreamingContext context)
      : this() { global::ProtoBuf.Serializer.Merge(info, this); }
    void global::System.Runtime.Serialization.ISerializable.GetObjectData(global::System.Runtime.Serialization.SerializationInfo info, global::System.Runtime.Serialization.StreamingContext context)
      { global::ProtoBuf.Serializer.Serialize(info, this); }
    
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"Uuid128")]
  [global::System.Runtime.Serialization.DataContract(Name=@"Uuid128")]
  public partial class Uuid128 : global::ProtoBuf.IExtensible, global::System.Runtime.Serialization.ISerializable
  {
    public Uuid128() {}
    
    private ulong _low;
    [global::ProtoBuf.ProtoMember(1, IsRequired = true, Name=@"low", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.Runtime.Serialization.DataMember(Name=@"low", Order = 1, IsRequired = true)]
    
    public ulong Low
    {
      get { return _low; }
      set { _low = value; }
    }

    private ulong _high = (ulong)0;
    [global::ProtoBuf.ProtoMember(2, IsRequired = false, Name=@"high", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.ComponentModel.DefaultValue((ulong)0)]
    [global::System.Runtime.Serialization.DataMember(Name=@"high", Order = 2, IsRequired = false)]
    
    public ulong High
    {
      get { return _high; }
      set { _high = value; }
    }
    protected Uuid128(global::System.Runtime.Serialization.SerializationInfo info, global::System.Runtime.Serialization.StreamingContext context)
      : this() { global::ProtoBuf.Serializer.Merge(info, this); }
    void global::System.Runtime.Serialization.ISerializable.GetObjectData(global::System.Runtime.Serialization.SerializationInfo info, global::System.Runtime.Serialization.StreamingContext context)
      { global::ProtoBuf.Serializer.Serialize(info, this); }
    
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"Login")]
  [global::System.Runtime.Serialization.DataContract(Name=@"Login")]
  public partial class Login : global::ProtoBuf.IExtensible, global::System.Runtime.Serialization.ISerializable
  {
    public Login() {}
    

    private string _username = "";
    [global::ProtoBuf.ProtoMember(1, IsRequired = false, Name=@"username", DataFormat = global::ProtoBuf.DataFormat.Default)]
    [global::System.ComponentModel.DefaultValue("")]
    [global::System.Runtime.Serialization.DataMember(Name=@"username", Order = 1, IsRequired = false)]
    
    public string Username
    {
      get { return _username; }
      set { _username = value; }
    }

    private string _password = "";
    [global::ProtoBuf.ProtoMember(2, IsRequired = false, Name=@"password", DataFormat = global::ProtoBuf.DataFormat.Default)]
    [global::System.ComponentModel.DefaultValue("")]
    [global::System.Runtime.Serialization.DataMember(Name=@"password", Order = 2, IsRequired = false)]
    
    public string Password
    {
      get { return _password; }
      set { _password = value; }
    }

    private byte[] _cookie = null;
    [global::ProtoBuf.ProtoMember(3, IsRequired = false, Name=@"cookie", DataFormat = global::ProtoBuf.DataFormat.Default)]
    [global::System.ComponentModel.DefaultValue(null)]
    [global::System.Runtime.Serialization.DataMember(Name=@"cookie", Order = 3, IsRequired = false)]
    
    public byte[] Cookie
    {
      get { return _cookie; }
      set { _cookie = value; }
    }
    protected Login(global::System.Runtime.Serialization.SerializationInfo info, global::System.Runtime.Serialization.StreamingContext context)
      : this() { global::ProtoBuf.Serializer.Merge(info, this); }
    void global::System.Runtime.Serialization.ISerializable.GetObjectData(global::System.Runtime.Serialization.SerializationInfo info, global::System.Runtime.Serialization.StreamingContext context)
      { global::ProtoBuf.Serializer.Serialize(info, this); }
    
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"OrderCreate")]
  [global::System.Runtime.Serialization.DataContract(Name=@"OrderCreate")]
  public partial class OrderCreate : global::ProtoBuf.IExtensible, global::System.Runtime.Serialization.ISerializable
  {
    public OrderCreate() {}
    
    private IronSmarkets.Proto.Seto.OrderCreateType _type;
    [global::ProtoBuf.ProtoMember(1, IsRequired = true, Name=@"type", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.Runtime.Serialization.DataMember(Name=@"type", Order = 1, IsRequired = true)]
    
    public IronSmarkets.Proto.Seto.OrderCreateType Type
    {
      get { return _type; }
      set { _type = value; }
    }
    private IronSmarkets.Proto.Seto.Uuid128 _market;
    [global::ProtoBuf.ProtoMember(2, IsRequired = true, Name=@"market", DataFormat = global::ProtoBuf.DataFormat.Default)]
    [global::System.Runtime.Serialization.DataMember(Name=@"market", Order = 2, IsRequired = true)]
    
    public IronSmarkets.Proto.Seto.Uuid128 Market
    {
      get { return _market; }
      set { _market = value; }
    }
    private IronSmarkets.Proto.Seto.Uuid128 _contract;
    [global::ProtoBuf.ProtoMember(3, IsRequired = true, Name=@"contract", DataFormat = global::ProtoBuf.DataFormat.Default)]
    [global::System.Runtime.Serialization.DataMember(Name=@"contract", Order = 3, IsRequired = true)]
    
    public IronSmarkets.Proto.Seto.Uuid128 Contract
    {
      get { return _contract; }
      set { _contract = value; }
    }
    private IronSmarkets.Proto.Seto.Side _side;
    [global::ProtoBuf.ProtoMember(4, IsRequired = true, Name=@"side", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.Runtime.Serialization.DataMember(Name=@"side", Order = 4, IsRequired = true)]
    
    public IronSmarkets.Proto.Seto.Side Side
    {
      get { return _side; }
      set { _side = value; }
    }
    private IronSmarkets.Proto.Seto.QuantityType _quantityType;
    [global::ProtoBuf.ProtoMember(5, IsRequired = true, Name=@"quantity_type", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.Runtime.Serialization.DataMember(Name=@"quantity_type", Order = 5, IsRequired = true)]
    
    public IronSmarkets.Proto.Seto.QuantityType QuantityType
    {
      get { return _quantityType; }
      set { _quantityType = value; }
    }
    private uint _quantity;
    [global::ProtoBuf.ProtoMember(6, IsRequired = true, Name=@"quantity", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.Runtime.Serialization.DataMember(Name=@"quantity", Order = 6, IsRequired = true)]
    
    public uint Quantity
    {
      get { return _quantity; }
      set { _quantity = value; }
    }

    private IronSmarkets.Proto.Seto.PriceType _priceType = IronSmarkets.Proto.Seto.PriceType.PRICEPERCENTODDS;
    [global::ProtoBuf.ProtoMember(7, IsRequired = false, Name=@"price_type", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.ComponentModel.DefaultValue(IronSmarkets.Proto.Seto.PriceType.PRICEPERCENTODDS)]
    [global::System.Runtime.Serialization.DataMember(Name=@"price_type", Order = 7, IsRequired = false)]
    
    public IronSmarkets.Proto.Seto.PriceType PriceType
    {
      get { return _priceType; }
      set { _priceType = value; }
    }

    private uint _price = default(uint);
    [global::ProtoBuf.ProtoMember(8, IsRequired = false, Name=@"price", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.ComponentModel.DefaultValue(default(uint))]
    [global::System.Runtime.Serialization.DataMember(Name=@"price", Order = 8, IsRequired = false)]
    
    public uint Price
    {
      get { return _price; }
      set { _price = value; }
    }
    protected OrderCreate(global::System.Runtime.Serialization.SerializationInfo info, global::System.Runtime.Serialization.StreamingContext context)
      : this() { global::ProtoBuf.Serializer.Merge(info, this); }
    void global::System.Runtime.Serialization.ISerializable.GetObjectData(global::System.Runtime.Serialization.SerializationInfo info, global::System.Runtime.Serialization.StreamingContext context)
      { global::ProtoBuf.Serializer.Serialize(info, this); }
    
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"OrderRejected")]
  [global::System.Runtime.Serialization.DataContract(Name=@"OrderRejected")]
  public partial class OrderRejected : global::ProtoBuf.IExtensible, global::System.Runtime.Serialization.ISerializable
  {
    public OrderRejected() {}
    
    private ulong _seq;
    [global::ProtoBuf.ProtoMember(1, IsRequired = true, Name=@"seq", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.Runtime.Serialization.DataMember(Name=@"seq", Order = 1, IsRequired = true)]
    
    public ulong Seq
    {
      get { return _seq; }
      set { _seq = value; }
    }
    private IronSmarkets.Proto.Seto.OrderRejectedReason _reason;
    [global::ProtoBuf.ProtoMember(2, IsRequired = true, Name=@"reason", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.Runtime.Serialization.DataMember(Name=@"reason", Order = 2, IsRequired = true)]
    
    public IronSmarkets.Proto.Seto.OrderRejectedReason Reason
    {
      get { return _reason; }
      set { _reason = value; }
    }
    protected OrderRejected(global::System.Runtime.Serialization.SerializationInfo info, global::System.Runtime.Serialization.StreamingContext context)
      : this() { global::ProtoBuf.Serializer.Merge(info, this); }
    void global::System.Runtime.Serialization.ISerializable.GetObjectData(global::System.Runtime.Serialization.SerializationInfo info, global::System.Runtime.Serialization.StreamingContext context)
      { global::ProtoBuf.Serializer.Serialize(info, this); }
    
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"OrderCancelRejected")]
  [global::System.Runtime.Serialization.DataContract(Name=@"OrderCancelRejected")]
  public partial class OrderCancelRejected : global::ProtoBuf.IExtensible, global::System.Runtime.Serialization.ISerializable
  {
    public OrderCancelRejected() {}
    
    private ulong _seq;
    [global::ProtoBuf.ProtoMember(1, IsRequired = true, Name=@"seq", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.Runtime.Serialization.DataMember(Name=@"seq", Order = 1, IsRequired = true)]
    
    public ulong Seq
    {
      get { return _seq; }
      set { _seq = value; }
    }
    private IronSmarkets.Proto.Seto.OrderCancelRejectedReason _reason;
    [global::ProtoBuf.ProtoMember(2, IsRequired = true, Name=@"reason", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.Runtime.Serialization.DataMember(Name=@"reason", Order = 2, IsRequired = true)]
    
    public IronSmarkets.Proto.Seto.OrderCancelRejectedReason Reason
    {
      get { return _reason; }
      set { _reason = value; }
    }
    protected OrderCancelRejected(global::System.Runtime.Serialization.SerializationInfo info, global::System.Runtime.Serialization.StreamingContext context)
      : this() { global::ProtoBuf.Serializer.Merge(info, this); }
    void global::System.Runtime.Serialization.ISerializable.GetObjectData(global::System.Runtime.Serialization.SerializationInfo info, global::System.Runtime.Serialization.StreamingContext context)
      { global::ProtoBuf.Serializer.Serialize(info, this); }
    
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"OrderAccepted")]
  [global::System.Runtime.Serialization.DataContract(Name=@"OrderAccepted")]
  public partial class OrderAccepted : global::ProtoBuf.IExtensible, global::System.Runtime.Serialization.ISerializable
  {
    public OrderAccepted() {}
    
    private ulong _seq;
    [global::ProtoBuf.ProtoMember(1, IsRequired = true, Name=@"seq", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.Runtime.Serialization.DataMember(Name=@"seq", Order = 1, IsRequired = true)]
    
    public ulong Seq
    {
      get { return _seq; }
      set { _seq = value; }
    }
    private IronSmarkets.Proto.Seto.Uuid128 _order;
    [global::ProtoBuf.ProtoMember(2, IsRequired = true, Name=@"order", DataFormat = global::ProtoBuf.DataFormat.Default)]
    [global::System.Runtime.Serialization.DataMember(Name=@"order", Order = 2, IsRequired = true)]
    
    public IronSmarkets.Proto.Seto.Uuid128 Order
    {
      get { return _order; }
      set { _order = value; }
    }
    protected OrderAccepted(global::System.Runtime.Serialization.SerializationInfo info, global::System.Runtime.Serialization.StreamingContext context)
      : this() { global::ProtoBuf.Serializer.Merge(info, this); }
    void global::System.Runtime.Serialization.ISerializable.GetObjectData(global::System.Runtime.Serialization.SerializationInfo info, global::System.Runtime.Serialization.StreamingContext context)
      { global::ProtoBuf.Serializer.Serialize(info, this); }
    
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"OrderExecuted")]
  [global::System.Runtime.Serialization.DataContract(Name=@"OrderExecuted")]
  public partial class OrderExecuted : global::ProtoBuf.IExtensible, global::System.Runtime.Serialization.ISerializable
  {
    public OrderExecuted() {}
    
    private IronSmarkets.Proto.Seto.Uuid128 _order;
    [global::ProtoBuf.ProtoMember(1, IsRequired = true, Name=@"order", DataFormat = global::ProtoBuf.DataFormat.Default)]
    [global::System.Runtime.Serialization.DataMember(Name=@"order", Order = 1, IsRequired = true)]
    
    public IronSmarkets.Proto.Seto.Uuid128 Order
    {
      get { return _order; }
      set { _order = value; }
    }
    private uint _price;
    [global::ProtoBuf.ProtoMember(2, IsRequired = true, Name=@"price", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.Runtime.Serialization.DataMember(Name=@"price", Order = 2, IsRequired = true)]
    
    public uint Price
    {
      get { return _price; }
      set { _price = value; }
    }
    private IronSmarkets.Proto.Seto.QuantityType _quantityType;
    [global::ProtoBuf.ProtoMember(3, IsRequired = true, Name=@"quantity_type", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.Runtime.Serialization.DataMember(Name=@"quantity_type", Order = 3, IsRequired = true)]
    
    public IronSmarkets.Proto.Seto.QuantityType QuantityType
    {
      get { return _quantityType; }
      set { _quantityType = value; }
    }
    private uint _quantity;
    [global::ProtoBuf.ProtoMember(4, IsRequired = true, Name=@"quantity", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.Runtime.Serialization.DataMember(Name=@"quantity", Order = 4, IsRequired = true)]
    
    public uint Quantity
    {
      get { return _quantity; }
      set { _quantity = value; }
    }
    protected OrderExecuted(global::System.Runtime.Serialization.SerializationInfo info, global::System.Runtime.Serialization.StreamingContext context)
      : this() { global::ProtoBuf.Serializer.Merge(info, this); }
    void global::System.Runtime.Serialization.ISerializable.GetObjectData(global::System.Runtime.Serialization.SerializationInfo info, global::System.Runtime.Serialization.StreamingContext context)
      { global::ProtoBuf.Serializer.Serialize(info, this); }
    
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"OrderCancel")]
  [global::System.Runtime.Serialization.DataContract(Name=@"OrderCancel")]
  public partial class OrderCancel : global::ProtoBuf.IExtensible, global::System.Runtime.Serialization.ISerializable
  {
    public OrderCancel() {}
    
    private IronSmarkets.Proto.Seto.Uuid128 _order;
    [global::ProtoBuf.ProtoMember(1, IsRequired = true, Name=@"order", DataFormat = global::ProtoBuf.DataFormat.Default)]
    [global::System.Runtime.Serialization.DataMember(Name=@"order", Order = 1, IsRequired = true)]
    
    public IronSmarkets.Proto.Seto.Uuid128 Order
    {
      get { return _order; }
      set { _order = value; }
    }
    protected OrderCancel(global::System.Runtime.Serialization.SerializationInfo info, global::System.Runtime.Serialization.StreamingContext context)
      : this() { global::ProtoBuf.Serializer.Merge(info, this); }
    void global::System.Runtime.Serialization.ISerializable.GetObjectData(global::System.Runtime.Serialization.SerializationInfo info, global::System.Runtime.Serialization.StreamingContext context)
      { global::ProtoBuf.Serializer.Serialize(info, this); }
    
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"OrderCancelled")]
  [global::System.Runtime.Serialization.DataContract(Name=@"OrderCancelled")]
  public partial class OrderCancelled : global::ProtoBuf.IExtensible, global::System.Runtime.Serialization.ISerializable
  {
    public OrderCancelled() {}
    
    private IronSmarkets.Proto.Seto.Uuid128 _order;
    [global::ProtoBuf.ProtoMember(1, IsRequired = true, Name=@"order", DataFormat = global::ProtoBuf.DataFormat.Default)]
    [global::System.Runtime.Serialization.DataMember(Name=@"order", Order = 1, IsRequired = true)]
    
    public IronSmarkets.Proto.Seto.Uuid128 Order
    {
      get { return _order; }
      set { _order = value; }
    }
    private IronSmarkets.Proto.Seto.OrderCancelledReason _reason;
    [global::ProtoBuf.ProtoMember(2, IsRequired = true, Name=@"reason", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.Runtime.Serialization.DataMember(Name=@"reason", Order = 2, IsRequired = true)]
    
    public IronSmarkets.Proto.Seto.OrderCancelledReason Reason
    {
      get { return _reason; }
      set { _reason = value; }
    }
    protected OrderCancelled(global::System.Runtime.Serialization.SerializationInfo info, global::System.Runtime.Serialization.StreamingContext context)
      : this() { global::ProtoBuf.Serializer.Merge(info, this); }
    void global::System.Runtime.Serialization.ISerializable.GetObjectData(global::System.Runtime.Serialization.SerializationInfo info, global::System.Runtime.Serialization.StreamingContext context)
      { global::ProtoBuf.Serializer.Serialize(info, this); }
    
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"OrderInvalid")]
  [global::System.Runtime.Serialization.DataContract(Name=@"OrderInvalid")]
  public partial class OrderInvalid : global::ProtoBuf.IExtensible, global::System.Runtime.Serialization.ISerializable
  {
    public OrderInvalid() {}
    
    private ulong _seq;
    [global::ProtoBuf.ProtoMember(1, IsRequired = true, Name=@"seq", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.Runtime.Serialization.DataMember(Name=@"seq", Order = 1, IsRequired = true)]
    
    public ulong Seq
    {
      get { return _seq; }
      set { _seq = value; }
    }
    private readonly global::System.Collections.Generic.List<IronSmarkets.Proto.Seto.OrderInvalidReason> _reasons = new global::System.Collections.Generic.List<IronSmarkets.Proto.Seto.OrderInvalidReason>();
    [global::ProtoBuf.ProtoMember(2, Name=@"reasons", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.Runtime.Serialization.DataMember(Name=@"reasons", Order = 2, IsRequired = false)]
    
    public global::System.Collections.Generic.List<IronSmarkets.Proto.Seto.OrderInvalidReason> Reasons
    {
      get { return _reasons; }
    }
  
    protected OrderInvalid(global::System.Runtime.Serialization.SerializationInfo info, global::System.Runtime.Serialization.StreamingContext context)
      : this() { global::ProtoBuf.Serializer.Merge(info, this); }
    void global::System.Runtime.Serialization.ISerializable.GetObjectData(global::System.Runtime.Serialization.SerializationInfo info, global::System.Runtime.Serialization.StreamingContext context)
      { global::ProtoBuf.Serializer.Serialize(info, this); }
    
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"MarketSubscribe")]
  [global::System.Runtime.Serialization.DataContract(Name=@"MarketSubscribe")]
  public partial class MarketSubscribe : global::ProtoBuf.IExtensible, global::System.Runtime.Serialization.ISerializable
  {
    public MarketSubscribe() {}
    
    private IronSmarkets.Proto.Seto.Uuid128 _market;
    [global::ProtoBuf.ProtoMember(1, IsRequired = true, Name=@"market", DataFormat = global::ProtoBuf.DataFormat.Default)]
    [global::System.Runtime.Serialization.DataMember(Name=@"market", Order = 1, IsRequired = true)]
    
    public IronSmarkets.Proto.Seto.Uuid128 Market
    {
      get { return _market; }
      set { _market = value; }
    }
    protected MarketSubscribe(global::System.Runtime.Serialization.SerializationInfo info, global::System.Runtime.Serialization.StreamingContext context)
      : this() { global::ProtoBuf.Serializer.Merge(info, this); }
    void global::System.Runtime.Serialization.ISerializable.GetObjectData(global::System.Runtime.Serialization.SerializationInfo info, global::System.Runtime.Serialization.StreamingContext context)
      { global::ProtoBuf.Serializer.Serialize(info, this); }
    
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"MarketUnsubscribe")]
  [global::System.Runtime.Serialization.DataContract(Name=@"MarketUnsubscribe")]
  public partial class MarketUnsubscribe : global::ProtoBuf.IExtensible, global::System.Runtime.Serialization.ISerializable
  {
    public MarketUnsubscribe() {}
    
    private IronSmarkets.Proto.Seto.Uuid128 _market;
    [global::ProtoBuf.ProtoMember(1, IsRequired = true, Name=@"market", DataFormat = global::ProtoBuf.DataFormat.Default)]
    [global::System.Runtime.Serialization.DataMember(Name=@"market", Order = 1, IsRequired = true)]
    
    public IronSmarkets.Proto.Seto.Uuid128 Market
    {
      get { return _market; }
      set { _market = value; }
    }
    protected MarketUnsubscribe(global::System.Runtime.Serialization.SerializationInfo info, global::System.Runtime.Serialization.StreamingContext context)
      : this() { global::ProtoBuf.Serializer.Merge(info, this); }
    void global::System.Runtime.Serialization.ISerializable.GetObjectData(global::System.Runtime.Serialization.SerializationInfo info, global::System.Runtime.Serialization.StreamingContext context)
      { global::ProtoBuf.Serializer.Serialize(info, this); }
    
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"MarketQuotesRequest")]
  [global::System.Runtime.Serialization.DataContract(Name=@"MarketQuotesRequest")]
  public partial class MarketQuotesRequest : global::ProtoBuf.IExtensible, global::System.Runtime.Serialization.ISerializable
  {
    public MarketQuotesRequest() {}
    
    private IronSmarkets.Proto.Seto.Uuid128 _market;
    [global::ProtoBuf.ProtoMember(1, IsRequired = true, Name=@"market", DataFormat = global::ProtoBuf.DataFormat.Default)]
    [global::System.Runtime.Serialization.DataMember(Name=@"market", Order = 1, IsRequired = true)]
    
    public IronSmarkets.Proto.Seto.Uuid128 Market
    {
      get { return _market; }
      set { _market = value; }
    }
    protected MarketQuotesRequest(global::System.Runtime.Serialization.SerializationInfo info, global::System.Runtime.Serialization.StreamingContext context)
      : this() { global::ProtoBuf.Serializer.Merge(info, this); }
    void global::System.Runtime.Serialization.ISerializable.GetObjectData(global::System.Runtime.Serialization.SerializationInfo info, global::System.Runtime.Serialization.StreamingContext context)
      { global::ProtoBuf.Serializer.Serialize(info, this); }
    
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"MarketQuotes")]
  [global::System.Runtime.Serialization.DataContract(Name=@"MarketQuotes")]
  public partial class MarketQuotes : global::ProtoBuf.IExtensible, global::System.Runtime.Serialization.ISerializable
  {
    public MarketQuotes() {}
    
    private IronSmarkets.Proto.Seto.Uuid128 _market;
    [global::ProtoBuf.ProtoMember(1, IsRequired = true, Name=@"market", DataFormat = global::ProtoBuf.DataFormat.Default)]
    [global::System.Runtime.Serialization.DataMember(Name=@"market", Order = 1, IsRequired = true)]
    
    public IronSmarkets.Proto.Seto.Uuid128 Market
    {
      get { return _market; }
      set { _market = value; }
    }
    private readonly global::System.Collections.Generic.List<IronSmarkets.Proto.Seto.ContractQuotes> _contractQuotes = new global::System.Collections.Generic.List<IronSmarkets.Proto.Seto.ContractQuotes>();
    [global::ProtoBuf.ProtoMember(2, Name=@"contract_quotes", DataFormat = global::ProtoBuf.DataFormat.Default)]
    [global::System.Runtime.Serialization.DataMember(Name=@"contract_quotes", Order = 2, IsRequired = false)]
    
    public global::System.Collections.Generic.List<IronSmarkets.Proto.Seto.ContractQuotes> ContractQuotes
    {
      get { return _contractQuotes; }
    }
  
    private IronSmarkets.Proto.Seto.PriceType _priceType;
    [global::ProtoBuf.ProtoMember(3, IsRequired = true, Name=@"price_type", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.Runtime.Serialization.DataMember(Name=@"price_type", Order = 3, IsRequired = true)]
    
    public IronSmarkets.Proto.Seto.PriceType PriceType
    {
      get { return _priceType; }
      set { _priceType = value; }
    }
    private IronSmarkets.Proto.Seto.QuantityType _quantityType;
    [global::ProtoBuf.ProtoMember(4, IsRequired = true, Name=@"quantity_type", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.Runtime.Serialization.DataMember(Name=@"quantity_type", Order = 4, IsRequired = true)]
    
    public IronSmarkets.Proto.Seto.QuantityType QuantityType
    {
      get { return _quantityType; }
      set { _quantityType = value; }
    }
    protected MarketQuotes(global::System.Runtime.Serialization.SerializationInfo info, global::System.Runtime.Serialization.StreamingContext context)
      : this() { global::ProtoBuf.Serializer.Merge(info, this); }
    void global::System.Runtime.Serialization.ISerializable.GetObjectData(global::System.Runtime.Serialization.SerializationInfo info, global::System.Runtime.Serialization.StreamingContext context)
      { global::ProtoBuf.Serializer.Serialize(info, this); }
    
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"ContractQuotes")]
  [global::System.Runtime.Serialization.DataContract(Name=@"ContractQuotes")]
  public partial class ContractQuotes : global::ProtoBuf.IExtensible, global::System.Runtime.Serialization.ISerializable
  {
    public ContractQuotes() {}
    
    private IronSmarkets.Proto.Seto.Uuid128 _contract;
    [global::ProtoBuf.ProtoMember(1, IsRequired = true, Name=@"contract", DataFormat = global::ProtoBuf.DataFormat.Default)]
    [global::System.Runtime.Serialization.DataMember(Name=@"contract", Order = 1, IsRequired = true)]
    
    public IronSmarkets.Proto.Seto.Uuid128 Contract
    {
      get { return _contract; }
      set { _contract = value; }
    }
    private readonly global::System.Collections.Generic.List<IronSmarkets.Proto.Seto.Quote> _bids = new global::System.Collections.Generic.List<IronSmarkets.Proto.Seto.Quote>();
    [global::ProtoBuf.ProtoMember(2, Name=@"bids", DataFormat = global::ProtoBuf.DataFormat.Default)]
    [global::System.Runtime.Serialization.DataMember(Name=@"bids", Order = 2, IsRequired = false)]
    
    public global::System.Collections.Generic.List<IronSmarkets.Proto.Seto.Quote> Bids
    {
      get { return _bids; }
    }
  
    private readonly global::System.Collections.Generic.List<IronSmarkets.Proto.Seto.Quote> _offers = new global::System.Collections.Generic.List<IronSmarkets.Proto.Seto.Quote>();
    [global::ProtoBuf.ProtoMember(3, Name=@"offers", DataFormat = global::ProtoBuf.DataFormat.Default)]
    [global::System.Runtime.Serialization.DataMember(Name=@"offers", Order = 3, IsRequired = false)]
    
    public global::System.Collections.Generic.List<IronSmarkets.Proto.Seto.Quote> Offers
    {
      get { return _offers; }
    }
  
    private readonly global::System.Collections.Generic.List<IronSmarkets.Proto.Seto.Execution> _executions = new global::System.Collections.Generic.List<IronSmarkets.Proto.Seto.Execution>();
    [global::ProtoBuf.ProtoMember(4, Name=@"executions", DataFormat = global::ProtoBuf.DataFormat.Default)]
    [global::System.Runtime.Serialization.DataMember(Name=@"executions", Order = 4, IsRequired = false)]
    
    public global::System.Collections.Generic.List<IronSmarkets.Proto.Seto.Execution> Executions
    {
      get { return _executions; }
    }
  

    private IronSmarkets.Proto.Seto.Execution _lastExecution = null;
    [global::ProtoBuf.ProtoMember(5, IsRequired = false, Name=@"last_execution", DataFormat = global::ProtoBuf.DataFormat.Default)]
    [global::System.ComponentModel.DefaultValue(null)]
    [global::System.Runtime.Serialization.DataMember(Name=@"last_execution", Order = 5, IsRequired = false)]
    
    public IronSmarkets.Proto.Seto.Execution LastExecution
    {
      get { return _lastExecution; }
      set { _lastExecution = value; }
    }
    protected ContractQuotes(global::System.Runtime.Serialization.SerializationInfo info, global::System.Runtime.Serialization.StreamingContext context)
      : this() { global::ProtoBuf.Serializer.Merge(info, this); }
    void global::System.Runtime.Serialization.ISerializable.GetObjectData(global::System.Runtime.Serialization.SerializationInfo info, global::System.Runtime.Serialization.StreamingContext context)
      { global::ProtoBuf.Serializer.Serialize(info, this); }
    
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"Quote")]
  [global::System.Runtime.Serialization.DataContract(Name=@"Quote")]
  public partial class Quote : global::ProtoBuf.IExtensible, global::System.Runtime.Serialization.ISerializable
  {
    public Quote() {}
    
    private uint _price;
    [global::ProtoBuf.ProtoMember(1, IsRequired = true, Name=@"price", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.Runtime.Serialization.DataMember(Name=@"price", Order = 1, IsRequired = true)]
    
    public uint Price
    {
      get { return _price; }
      set { _price = value; }
    }
    private uint _quantity;
    [global::ProtoBuf.ProtoMember(2, IsRequired = true, Name=@"quantity", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.Runtime.Serialization.DataMember(Name=@"quantity", Order = 2, IsRequired = true)]
    
    public uint Quantity
    {
      get { return _quantity; }
      set { _quantity = value; }
    }
    protected Quote(global::System.Runtime.Serialization.SerializationInfo info, global::System.Runtime.Serialization.StreamingContext context)
      : this() { global::ProtoBuf.Serializer.Merge(info, this); }
    void global::System.Runtime.Serialization.ISerializable.GetObjectData(global::System.Runtime.Serialization.SerializationInfo info, global::System.Runtime.Serialization.StreamingContext context)
      { global::ProtoBuf.Serializer.Serialize(info, this); }
    
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"Execution")]
  [global::System.Runtime.Serialization.DataContract(Name=@"Execution")]
  public partial class Execution : global::ProtoBuf.IExtensible, global::System.Runtime.Serialization.ISerializable
  {
    public Execution() {}
    
    private uint _price;
    [global::ProtoBuf.ProtoMember(1, IsRequired = true, Name=@"price", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.Runtime.Serialization.DataMember(Name=@"price", Order = 1, IsRequired = true)]
    
    public uint Price
    {
      get { return _price; }
      set { _price = value; }
    }
    private uint _quantity;
    [global::ProtoBuf.ProtoMember(2, IsRequired = true, Name=@"quantity", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.Runtime.Serialization.DataMember(Name=@"quantity", Order = 2, IsRequired = true)]
    
    public uint Quantity
    {
      get { return _quantity; }
      set { _quantity = value; }
    }
    private IronSmarkets.Proto.Seto.Side _liquidity;
    [global::ProtoBuf.ProtoMember(3, IsRequired = true, Name=@"liquidity", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.Runtime.Serialization.DataMember(Name=@"liquidity", Order = 3, IsRequired = true)]
    
    public IronSmarkets.Proto.Seto.Side Liquidity
    {
      get { return _liquidity; }
      set { _liquidity = value; }
    }
    private ulong _microseconds;
    [global::ProtoBuf.ProtoMember(4, IsRequired = true, Name=@"microseconds", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.Runtime.Serialization.DataMember(Name=@"microseconds", Order = 4, IsRequired = true)]
    
    public ulong Microseconds
    {
      get { return _microseconds; }
      set { _microseconds = value; }
    }
    protected Execution(global::System.Runtime.Serialization.SerializationInfo info, global::System.Runtime.Serialization.StreamingContext context)
      : this() { global::ProtoBuf.Serializer.Merge(info, this); }
    void global::System.Runtime.Serialization.ISerializable.GetObjectData(global::System.Runtime.Serialization.SerializationInfo info, global::System.Runtime.Serialization.StreamingContext context)
      { global::ProtoBuf.Serializer.Serialize(info, this); }
    
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"EventsRequest")]
  [global::System.Runtime.Serialization.DataContract(Name=@"EventsRequest")]
  public partial class EventsRequest : global::ProtoBuf.IExtensible, global::System.Runtime.Serialization.ISerializable
  {
    public EventsRequest() {}
    
    private IronSmarkets.Proto.Seto.EventsRequestType _type;
    [global::ProtoBuf.ProtoMember(1, IsRequired = true, Name=@"type", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.Runtime.Serialization.DataMember(Name=@"type", Order = 1, IsRequired = true)]
    
    public IronSmarkets.Proto.Seto.EventsRequestType Type
    {
      get { return _type; }
      set { _type = value; }
    }
    private IronSmarkets.Proto.Seto.ContentType _contentType;
    [global::ProtoBuf.ProtoMember(2, IsRequired = true, Name=@"content_type", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.Runtime.Serialization.DataMember(Name=@"content_type", Order = 2, IsRequired = true)]
    
    public IronSmarkets.Proto.Seto.ContentType ContentType
    {
      get { return _contentType; }
      set { _contentType = value; }
    }

    private IronSmarkets.Proto.Seto.SportByDate _sportByDate = null;
    [global::ProtoBuf.ProtoMember(3, IsRequired = false, Name=@"sport_by_date", DataFormat = global::ProtoBuf.DataFormat.Default)]
    [global::System.ComponentModel.DefaultValue(null)]
    [global::System.Runtime.Serialization.DataMember(Name=@"sport_by_date", Order = 3, IsRequired = false)]
    
    public IronSmarkets.Proto.Seto.SportByDate SportByDate
    {
      get { return _sportByDate; }
      set { _sportByDate = value; }
    }
    protected EventsRequest(global::System.Runtime.Serialization.SerializationInfo info, global::System.Runtime.Serialization.StreamingContext context)
      : this() { global::ProtoBuf.Serializer.Merge(info, this); }
    void global::System.Runtime.Serialization.ISerializable.GetObjectData(global::System.Runtime.Serialization.SerializationInfo info, global::System.Runtime.Serialization.StreamingContext context)
      { global::ProtoBuf.Serializer.Serialize(info, this); }
    
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"SportByDate")]
  [global::System.Runtime.Serialization.DataContract(Name=@"SportByDate")]
  public partial class SportByDate : global::ProtoBuf.IExtensible, global::System.Runtime.Serialization.ISerializable
  {
    public SportByDate() {}
    
    private IronSmarkets.Proto.Seto.SportByDateType _type;
    [global::ProtoBuf.ProtoMember(1, IsRequired = true, Name=@"type", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.Runtime.Serialization.DataMember(Name=@"type", Order = 1, IsRequired = true)]
    
    public IronSmarkets.Proto.Seto.SportByDateType Type
    {
      get { return _type; }
      set { _type = value; }
    }
    private IronSmarkets.Proto.Seto.Date _date;
    [global::ProtoBuf.ProtoMember(2, IsRequired = true, Name=@"date", DataFormat = global::ProtoBuf.DataFormat.Default)]
    [global::System.Runtime.Serialization.DataMember(Name=@"date", Order = 2, IsRequired = true)]
    
    public IronSmarkets.Proto.Seto.Date Date
    {
      get { return _date; }
      set { _date = value; }
    }
    protected SportByDate(global::System.Runtime.Serialization.SerializationInfo info, global::System.Runtime.Serialization.StreamingContext context)
      : this() { global::ProtoBuf.Serializer.Merge(info, this); }
    void global::System.Runtime.Serialization.ISerializable.GetObjectData(global::System.Runtime.Serialization.SerializationInfo info, global::System.Runtime.Serialization.StreamingContext context)
      { global::ProtoBuf.Serializer.Serialize(info, this); }
    
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"Events")]
  [global::System.Runtime.Serialization.DataContract(Name=@"Events")]
  public partial class Events : global::ProtoBuf.IExtensible, global::System.Runtime.Serialization.ISerializable
  {
    public Events() {}
    
    private readonly global::System.Collections.Generic.List<IronSmarkets.Proto.Seto.EventInfo> _withMarkets = new global::System.Collections.Generic.List<IronSmarkets.Proto.Seto.EventInfo>();
    [global::ProtoBuf.ProtoMember(1, Name=@"with_markets", DataFormat = global::ProtoBuf.DataFormat.Default)]
    [global::System.Runtime.Serialization.DataMember(Name=@"with_markets", Order = 1, IsRequired = false)]
    
    public global::System.Collections.Generic.List<IronSmarkets.Proto.Seto.EventInfo> WithMarkets
    {
      get { return _withMarkets; }
    }
  
    private readonly global::System.Collections.Generic.List<IronSmarkets.Proto.Seto.EventInfo> _parents = new global::System.Collections.Generic.List<IronSmarkets.Proto.Seto.EventInfo>();
    [global::ProtoBuf.ProtoMember(2, Name=@"parents", DataFormat = global::ProtoBuf.DataFormat.Default)]
    [global::System.Runtime.Serialization.DataMember(Name=@"parents", Order = 2, IsRequired = false)]
    
    public global::System.Collections.Generic.List<IronSmarkets.Proto.Seto.EventInfo> Parents
    {
      get { return _parents; }
    }
  
    protected Events(global::System.Runtime.Serialization.SerializationInfo info, global::System.Runtime.Serialization.StreamingContext context)
      : this() { global::ProtoBuf.Serializer.Merge(info, this); }
    void global::System.Runtime.Serialization.ISerializable.GetObjectData(global::System.Runtime.Serialization.SerializationInfo info, global::System.Runtime.Serialization.StreamingContext context)
      { global::ProtoBuf.Serializer.Serialize(info, this); }
    
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"EventInfo")]
  [global::System.Runtime.Serialization.DataContract(Name=@"EventInfo")]
  public partial class EventInfo : global::ProtoBuf.IExtensible, global::System.Runtime.Serialization.ISerializable
  {
    public EventInfo() {}
    
    private IronSmarkets.Proto.Seto.Uuid128 _event;
    [global::ProtoBuf.ProtoMember(1, IsRequired = true, Name=@"event", DataFormat = global::ProtoBuf.DataFormat.Default)]
    [global::System.Runtime.Serialization.DataMember(Name=@"event", Order = 1, IsRequired = true)]
    
    public IronSmarkets.Proto.Seto.Uuid128 Event
    {
      get { return _event; }
      set { _event = value; }
    }
    private IronSmarkets.Proto.Seto.EventType _type;
    [global::ProtoBuf.ProtoMember(2, IsRequired = true, Name=@"type", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.Runtime.Serialization.DataMember(Name=@"type", Order = 2, IsRequired = true)]
    
    public IronSmarkets.Proto.Seto.EventType Type
    {
      get { return _type; }
      set { _type = value; }
    }
    private IronSmarkets.Proto.Seto.EventCategory _category;
    [global::ProtoBuf.ProtoMember(3, IsRequired = true, Name=@"category", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.Runtime.Serialization.DataMember(Name=@"category", Order = 3, IsRequired = true)]
    
    public IronSmarkets.Proto.Seto.EventCategory Category
    {
      get { return _category; }
      set { _category = value; }
    }
    private string _slug;
    [global::ProtoBuf.ProtoMember(4, IsRequired = true, Name=@"slug", DataFormat = global::ProtoBuf.DataFormat.Default)]
    [global::System.Runtime.Serialization.DataMember(Name=@"slug", Order = 4, IsRequired = true)]
    
    public string Slug
    {
      get { return _slug; }
      set { _slug = value; }
    }
    private string _name;
    [global::ProtoBuf.ProtoMember(5, IsRequired = true, Name=@"name", DataFormat = global::ProtoBuf.DataFormat.Default)]
    [global::System.Runtime.Serialization.DataMember(Name=@"name", Order = 5, IsRequired = true)]
    
    public string Name
    {
      get { return _name; }
      set { _name = value; }
    }

    private IronSmarkets.Proto.Seto.Uuid128 _parent = null;
    [global::ProtoBuf.ProtoMember(6, IsRequired = false, Name=@"parent", DataFormat = global::ProtoBuf.DataFormat.Default)]
    [global::System.ComponentModel.DefaultValue(null)]
    [global::System.Runtime.Serialization.DataMember(Name=@"parent", Order = 6, IsRequired = false)]
    
    public IronSmarkets.Proto.Seto.Uuid128 Parent
    {
      get { return _parent; }
      set { _parent = value; }
    }

    private IronSmarkets.Proto.Seto.Date _startDate = null;
    [global::ProtoBuf.ProtoMember(7, IsRequired = false, Name=@"start_date", DataFormat = global::ProtoBuf.DataFormat.Default)]
    [global::System.ComponentModel.DefaultValue(null)]
    [global::System.Runtime.Serialization.DataMember(Name=@"start_date", Order = 7, IsRequired = false)]
    
    public IronSmarkets.Proto.Seto.Date StartDate
    {
      get { return _startDate; }
      set { _startDate = value; }
    }

    private IronSmarkets.Proto.Seto.Time _startTime = null;
    [global::ProtoBuf.ProtoMember(8, IsRequired = false, Name=@"start_time", DataFormat = global::ProtoBuf.DataFormat.Default)]
    [global::System.ComponentModel.DefaultValue(null)]
    [global::System.Runtime.Serialization.DataMember(Name=@"start_time", Order = 8, IsRequired = false)]
    
    public IronSmarkets.Proto.Seto.Time StartTime
    {
      get { return _startTime; }
      set { _startTime = value; }
    }

    private IronSmarkets.Proto.Seto.Date _endDate = null;
    [global::ProtoBuf.ProtoMember(9, IsRequired = false, Name=@"end_date", DataFormat = global::ProtoBuf.DataFormat.Default)]
    [global::System.ComponentModel.DefaultValue(null)]
    [global::System.Runtime.Serialization.DataMember(Name=@"end_date", Order = 9, IsRequired = false)]
    
    public IronSmarkets.Proto.Seto.Date EndDate
    {
      get { return _endDate; }
      set { _endDate = value; }
    }

    private IronSmarkets.Proto.Seto.Time _endTime = null;
    [global::ProtoBuf.ProtoMember(10, IsRequired = false, Name=@"end_time", DataFormat = global::ProtoBuf.DataFormat.Default)]
    [global::System.ComponentModel.DefaultValue(null)]
    [global::System.Runtime.Serialization.DataMember(Name=@"end_time", Order = 10, IsRequired = false)]
    
    public IronSmarkets.Proto.Seto.Time EndTime
    {
      get { return _endTime; }
      set { _endTime = value; }
    }
    private readonly global::System.Collections.Generic.List<IronSmarkets.Proto.Seto.EntityRelationship> _entities = new global::System.Collections.Generic.List<IronSmarkets.Proto.Seto.EntityRelationship>();
    [global::ProtoBuf.ProtoMember(11, Name=@"entities", DataFormat = global::ProtoBuf.DataFormat.Default)]
    [global::System.Runtime.Serialization.DataMember(Name=@"entities", Order = 11, IsRequired = false)]
    
    public global::System.Collections.Generic.List<IronSmarkets.Proto.Seto.EntityRelationship> Entities
    {
      get { return _entities; }
    }
  
    private readonly global::System.Collections.Generic.List<IronSmarkets.Proto.Seto.MarketInfo> _markets = new global::System.Collections.Generic.List<IronSmarkets.Proto.Seto.MarketInfo>();
    [global::ProtoBuf.ProtoMember(12, Name=@"markets", DataFormat = global::ProtoBuf.DataFormat.Default)]
    [global::System.Runtime.Serialization.DataMember(Name=@"markets", Order = 12, IsRequired = false)]
    
    public global::System.Collections.Generic.List<IronSmarkets.Proto.Seto.MarketInfo> Markets
    {
      get { return _markets; }
    }
  

    private string _description = "";
    [global::ProtoBuf.ProtoMember(13, IsRequired = false, Name=@"description", DataFormat = global::ProtoBuf.DataFormat.Default)]
    [global::System.ComponentModel.DefaultValue("")]
    [global::System.Runtime.Serialization.DataMember(Name=@"description", Order = 13, IsRequired = false)]
    
    public string Description
    {
      get { return _description; }
      set { _description = value; }
    }
    protected EventInfo(global::System.Runtime.Serialization.SerializationInfo info, global::System.Runtime.Serialization.StreamingContext context)
      : this() { global::ProtoBuf.Serializer.Merge(info, this); }
    void global::System.Runtime.Serialization.ISerializable.GetObjectData(global::System.Runtime.Serialization.SerializationInfo info, global::System.Runtime.Serialization.StreamingContext context)
      { global::ProtoBuf.Serializer.Serialize(info, this); }
    
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"MarketInfo")]
  [global::System.Runtime.Serialization.DataContract(Name=@"MarketInfo")]
  public partial class MarketInfo : global::ProtoBuf.IExtensible, global::System.Runtime.Serialization.ISerializable
  {
    public MarketInfo() {}
    
    private IronSmarkets.Proto.Seto.Uuid128 _market;
    [global::ProtoBuf.ProtoMember(1, IsRequired = true, Name=@"market", DataFormat = global::ProtoBuf.DataFormat.Default)]
    [global::System.Runtime.Serialization.DataMember(Name=@"market", Order = 1, IsRequired = true)]
    
    public IronSmarkets.Proto.Seto.Uuid128 Market
    {
      get { return _market; }
      set { _market = value; }
    }
    private readonly global::System.Collections.Generic.List<IronSmarkets.Proto.Seto.ContractInfo> _contracts = new global::System.Collections.Generic.List<IronSmarkets.Proto.Seto.ContractInfo>();
    [global::ProtoBuf.ProtoMember(2, Name=@"contracts", DataFormat = global::ProtoBuf.DataFormat.Default)]
    [global::System.Runtime.Serialization.DataMember(Name=@"contracts", Order = 2, IsRequired = false)]
    
    public global::System.Collections.Generic.List<IronSmarkets.Proto.Seto.ContractInfo> Contracts
    {
      get { return _contracts; }
    }
  
    private string _slug;
    [global::ProtoBuf.ProtoMember(3, IsRequired = true, Name=@"slug", DataFormat = global::ProtoBuf.DataFormat.Default)]
    [global::System.Runtime.Serialization.DataMember(Name=@"slug", Order = 3, IsRequired = true)]
    
    public string Slug
    {
      get { return _slug; }
      set { _slug = value; }
    }
    private string _name;
    [global::ProtoBuf.ProtoMember(4, IsRequired = true, Name=@"name", DataFormat = global::ProtoBuf.DataFormat.Default)]
    [global::System.Runtime.Serialization.DataMember(Name=@"name", Order = 4, IsRequired = true)]
    
    public string Name
    {
      get { return _name; }
      set { _name = value; }
    }

    private IronSmarkets.Proto.Seto.Date _startDate = null;
    [global::ProtoBuf.ProtoMember(5, IsRequired = false, Name=@"start_date", DataFormat = global::ProtoBuf.DataFormat.Default)]
    [global::System.ComponentModel.DefaultValue(null)]
    [global::System.Runtime.Serialization.DataMember(Name=@"start_date", Order = 5, IsRequired = false)]
    
    public IronSmarkets.Proto.Seto.Date StartDate
    {
      get { return _startDate; }
      set { _startDate = value; }
    }

    private IronSmarkets.Proto.Seto.Time _startTime = null;
    [global::ProtoBuf.ProtoMember(6, IsRequired = false, Name=@"start_time", DataFormat = global::ProtoBuf.DataFormat.Default)]
    [global::System.ComponentModel.DefaultValue(null)]
    [global::System.Runtime.Serialization.DataMember(Name=@"start_time", Order = 6, IsRequired = false)]
    
    public IronSmarkets.Proto.Seto.Time StartTime
    {
      get { return _startTime; }
      set { _startTime = value; }
    }

    private IronSmarkets.Proto.Seto.Date _endDate = null;
    [global::ProtoBuf.ProtoMember(7, IsRequired = false, Name=@"end_date", DataFormat = global::ProtoBuf.DataFormat.Default)]
    [global::System.ComponentModel.DefaultValue(null)]
    [global::System.Runtime.Serialization.DataMember(Name=@"end_date", Order = 7, IsRequired = false)]
    
    public IronSmarkets.Proto.Seto.Date EndDate
    {
      get { return _endDate; }
      set { _endDate = value; }
    }

    private IronSmarkets.Proto.Seto.Time _endTime = null;
    [global::ProtoBuf.ProtoMember(8, IsRequired = false, Name=@"end_time", DataFormat = global::ProtoBuf.DataFormat.Default)]
    [global::System.ComponentModel.DefaultValue(null)]
    [global::System.Runtime.Serialization.DataMember(Name=@"end_time", Order = 8, IsRequired = false)]
    
    public IronSmarkets.Proto.Seto.Time EndTime
    {
      get { return _endTime; }
      set { _endTime = value; }
    }
    private readonly global::System.Collections.Generic.List<IronSmarkets.Proto.Seto.EntityRelationship> _entities = new global::System.Collections.Generic.List<IronSmarkets.Proto.Seto.EntityRelationship>();
    [global::ProtoBuf.ProtoMember(9, Name=@"entities", DataFormat = global::ProtoBuf.DataFormat.Default)]
    [global::System.Runtime.Serialization.DataMember(Name=@"entities", Order = 9, IsRequired = false)]
    
    public global::System.Collections.Generic.List<IronSmarkets.Proto.Seto.EntityRelationship> Entities
    {
      get { return _entities; }
    }
  

    private string _shortname = "";
    [global::ProtoBuf.ProtoMember(10, IsRequired = false, Name=@"shortname", DataFormat = global::ProtoBuf.DataFormat.Default)]
    [global::System.ComponentModel.DefaultValue("")]
    [global::System.Runtime.Serialization.DataMember(Name=@"shortname", Order = 10, IsRequired = false)]
    
    public string Shortname
    {
      get { return _shortname; }
      set { _shortname = value; }
    }
    protected MarketInfo(global::System.Runtime.Serialization.SerializationInfo info, global::System.Runtime.Serialization.StreamingContext context)
      : this() { global::ProtoBuf.Serializer.Merge(info, this); }
    void global::System.Runtime.Serialization.ISerializable.GetObjectData(global::System.Runtime.Serialization.SerializationInfo info, global::System.Runtime.Serialization.StreamingContext context)
      { global::ProtoBuf.Serializer.Serialize(info, this); }
    
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"ContractInfo")]
  [global::System.Runtime.Serialization.DataContract(Name=@"ContractInfo")]
  public partial class ContractInfo : global::ProtoBuf.IExtensible, global::System.Runtime.Serialization.ISerializable
  {
    public ContractInfo() {}
    
    private IronSmarkets.Proto.Seto.Uuid128 _contract;
    [global::ProtoBuf.ProtoMember(1, IsRequired = true, Name=@"contract", DataFormat = global::ProtoBuf.DataFormat.Default)]
    [global::System.Runtime.Serialization.DataMember(Name=@"contract", Order = 1, IsRequired = true)]
    
    public IronSmarkets.Proto.Seto.Uuid128 Contract
    {
      get { return _contract; }
      set { _contract = value; }
    }
    private IronSmarkets.Proto.Seto.ContractType _type;
    [global::ProtoBuf.ProtoMember(2, IsRequired = true, Name=@"type", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.Runtime.Serialization.DataMember(Name=@"type", Order = 2, IsRequired = true)]
    
    public IronSmarkets.Proto.Seto.ContractType Type
    {
      get { return _type; }
      set { _type = value; }
    }
    private string _slug;
    [global::ProtoBuf.ProtoMember(3, IsRequired = true, Name=@"slug", DataFormat = global::ProtoBuf.DataFormat.Default)]
    [global::System.Runtime.Serialization.DataMember(Name=@"slug", Order = 3, IsRequired = true)]
    
    public string Slug
    {
      get { return _slug; }
      set { _slug = value; }
    }
    private string _name;
    [global::ProtoBuf.ProtoMember(4, IsRequired = true, Name=@"name", DataFormat = global::ProtoBuf.DataFormat.Default)]
    [global::System.Runtime.Serialization.DataMember(Name=@"name", Order = 4, IsRequired = true)]
    
    public string Name
    {
      get { return _name; }
      set { _name = value; }
    }
    private readonly global::System.Collections.Generic.List<IronSmarkets.Proto.Seto.EntityRelationship> _entities = new global::System.Collections.Generic.List<IronSmarkets.Proto.Seto.EntityRelationship>();
    [global::ProtoBuf.ProtoMember(5, Name=@"entities", DataFormat = global::ProtoBuf.DataFormat.Default)]
    [global::System.Runtime.Serialization.DataMember(Name=@"entities", Order = 5, IsRequired = false)]
    
    public global::System.Collections.Generic.List<IronSmarkets.Proto.Seto.EntityRelationship> Entities
    {
      get { return _entities; }
    }
  

    private string _shortname = "";
    [global::ProtoBuf.ProtoMember(6, IsRequired = false, Name=@"shortname", DataFormat = global::ProtoBuf.DataFormat.Default)]
    [global::System.ComponentModel.DefaultValue("")]
    [global::System.Runtime.Serialization.DataMember(Name=@"shortname", Order = 6, IsRequired = false)]
    
    public string Shortname
    {
      get { return _shortname; }
      set { _shortname = value; }
    }
    protected ContractInfo(global::System.Runtime.Serialization.SerializationInfo info, global::System.Runtime.Serialization.StreamingContext context)
      : this() { global::ProtoBuf.Serializer.Merge(info, this); }
    void global::System.Runtime.Serialization.ISerializable.GetObjectData(global::System.Runtime.Serialization.SerializationInfo info, global::System.Runtime.Serialization.StreamingContext context)
      { global::ProtoBuf.Serializer.Serialize(info, this); }
    
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"EntityRelationship")]
  [global::System.Runtime.Serialization.DataContract(Name=@"EntityRelationship")]
  public partial class EntityRelationship : global::ProtoBuf.IExtensible, global::System.Runtime.Serialization.ISerializable
  {
    public EntityRelationship() {}
    
    private IronSmarkets.Proto.Seto.Uuid128 _entity;
    [global::ProtoBuf.ProtoMember(1, IsRequired = true, Name=@"entity", DataFormat = global::ProtoBuf.DataFormat.Default)]
    [global::System.Runtime.Serialization.DataMember(Name=@"entity", Order = 1, IsRequired = true)]
    
    public IronSmarkets.Proto.Seto.Uuid128 Entity
    {
      get { return _entity; }
      set { _entity = value; }
    }
    private IronSmarkets.Proto.Seto.EntityRelationshipType _relationship;
    [global::ProtoBuf.ProtoMember(2, IsRequired = true, Name=@"relationship", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.Runtime.Serialization.DataMember(Name=@"relationship", Order = 2, IsRequired = true)]
    
    public IronSmarkets.Proto.Seto.EntityRelationshipType Relationship
    {
      get { return _relationship; }
      set { _relationship = value; }
    }
    protected EntityRelationship(global::System.Runtime.Serialization.SerializationInfo info, global::System.Runtime.Serialization.StreamingContext context)
      : this() { global::ProtoBuf.Serializer.Merge(info, this); }
    void global::System.Runtime.Serialization.ISerializable.GetObjectData(global::System.Runtime.Serialization.SerializationInfo info, global::System.Runtime.Serialization.StreamingContext context)
      { global::ProtoBuf.Serializer.Serialize(info, this); }
    
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"HttpFound")]
  [global::System.Runtime.Serialization.DataContract(Name=@"HttpFound")]
  public partial class HttpFound : global::ProtoBuf.IExtensible, global::System.Runtime.Serialization.ISerializable
  {
    public HttpFound() {}
    
    private ulong _seq;
    [global::ProtoBuf.ProtoMember(1, IsRequired = true, Name=@"seq", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.Runtime.Serialization.DataMember(Name=@"seq", Order = 1, IsRequired = true)]
    
    public ulong Seq
    {
      get { return _seq; }
      set { _seq = value; }
    }
    private string _url;
    [global::ProtoBuf.ProtoMember(2, IsRequired = true, Name=@"url", DataFormat = global::ProtoBuf.DataFormat.Default)]
    [global::System.Runtime.Serialization.DataMember(Name=@"url", Order = 2, IsRequired = true)]
    
    public string Url
    {
      get { return _url; }
      set { _url = value; }
    }
    protected HttpFound(global::System.Runtime.Serialization.SerializationInfo info, global::System.Runtime.Serialization.StreamingContext context)
      : this() { global::ProtoBuf.Serializer.Merge(info, this); }
    void global::System.Runtime.Serialization.ISerializable.GetObjectData(global::System.Runtime.Serialization.SerializationInfo info, global::System.Runtime.Serialization.StreamingContext context)
      { global::ProtoBuf.Serializer.Serialize(info, this); }
    
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"InvalidRequest")]
  [global::System.Runtime.Serialization.DataContract(Name=@"InvalidRequest")]
  public partial class InvalidRequest : global::ProtoBuf.IExtensible, global::System.Runtime.Serialization.ISerializable
  {
    public InvalidRequest() {}
    
    private ulong _seq;
    [global::ProtoBuf.ProtoMember(1, IsRequired = true, Name=@"seq", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.Runtime.Serialization.DataMember(Name=@"seq", Order = 1, IsRequired = true)]
    
    public ulong Seq
    {
      get { return _seq; }
      set { _seq = value; }
    }
    private IronSmarkets.Proto.Seto.InvalidRequestType _type;
    [global::ProtoBuf.ProtoMember(2, IsRequired = true, Name=@"type", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.Runtime.Serialization.DataMember(Name=@"type", Order = 2, IsRequired = true)]
    
    public IronSmarkets.Proto.Seto.InvalidRequestType Type
    {
      get { return _type; }
      set { _type = value; }
    }
    protected InvalidRequest(global::System.Runtime.Serialization.SerializationInfo info, global::System.Runtime.Serialization.StreamingContext context)
      : this() { global::ProtoBuf.Serializer.Merge(info, this); }
    void global::System.Runtime.Serialization.ISerializable.GetObjectData(global::System.Runtime.Serialization.SerializationInfo info, global::System.Runtime.Serialization.StreamingContext context)
      { global::ProtoBuf.Serializer.Serialize(info, this); }
    
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"Date")]
  [global::System.Runtime.Serialization.DataContract(Name=@"Date")]
  public partial class Date : global::ProtoBuf.IExtensible, global::System.Runtime.Serialization.ISerializable
  {
    public Date() {}
    
    private uint _year;
    [global::ProtoBuf.ProtoMember(1, IsRequired = true, Name=@"year", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.Runtime.Serialization.DataMember(Name=@"year", Order = 1, IsRequired = true)]
    
    public uint Year
    {
      get { return _year; }
      set { _year = value; }
    }
    private uint _month;
    [global::ProtoBuf.ProtoMember(2, IsRequired = true, Name=@"month", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.Runtime.Serialization.DataMember(Name=@"month", Order = 2, IsRequired = true)]
    
    public uint Month
    {
      get { return _month; }
      set { _month = value; }
    }
    private uint _day;
    [global::ProtoBuf.ProtoMember(3, IsRequired = true, Name=@"day", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.Runtime.Serialization.DataMember(Name=@"day", Order = 3, IsRequired = true)]
    
    public uint Day
    {
      get { return _day; }
      set { _day = value; }
    }
    protected Date(global::System.Runtime.Serialization.SerializationInfo info, global::System.Runtime.Serialization.StreamingContext context)
      : this() { global::ProtoBuf.Serializer.Merge(info, this); }
    void global::System.Runtime.Serialization.ISerializable.GetObjectData(global::System.Runtime.Serialization.SerializationInfo info, global::System.Runtime.Serialization.StreamingContext context)
      { global::ProtoBuf.Serializer.Serialize(info, this); }
    
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"Time")]
  [global::System.Runtime.Serialization.DataContract(Name=@"Time")]
  public partial class Time : global::ProtoBuf.IExtensible, global::System.Runtime.Serialization.ISerializable
  {
    public Time() {}
    
    private uint _hour;
    [global::ProtoBuf.ProtoMember(1, IsRequired = true, Name=@"hour", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.Runtime.Serialization.DataMember(Name=@"hour", Order = 1, IsRequired = true)]
    
    public uint Hour
    {
      get { return _hour; }
      set { _hour = value; }
    }
    private uint _minute;
    [global::ProtoBuf.ProtoMember(2, IsRequired = true, Name=@"minute", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.Runtime.Serialization.DataMember(Name=@"minute", Order = 2, IsRequired = true)]
    
    public uint Minute
    {
      get { return _minute; }
      set { _minute = value; }
    }
    protected Time(global::System.Runtime.Serialization.SerializationInfo info, global::System.Runtime.Serialization.StreamingContext context)
      : this() { global::ProtoBuf.Serializer.Merge(info, this); }
    void global::System.Runtime.Serialization.ISerializable.GetObjectData(global::System.Runtime.Serialization.SerializationInfo info, global::System.Runtime.Serialization.StreamingContext context)
      { global::ProtoBuf.Serializer.Serialize(info, this); }
    
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"orders_for_account_request")]
  [global::System.Runtime.Serialization.DataContract(Name=@"orders_for_account_request")]
  public partial class OrdersForAccountRequest : global::ProtoBuf.IExtensible, global::System.Runtime.Serialization.ISerializable
  {
    public OrdersForAccountRequest() {}
    
    protected OrdersForAccountRequest(global::System.Runtime.Serialization.SerializationInfo info, global::System.Runtime.Serialization.StreamingContext context)
      : this() { global::ProtoBuf.Serializer.Merge(info, this); }
    void global::System.Runtime.Serialization.ISerializable.GetObjectData(global::System.Runtime.Serialization.SerializationInfo info, global::System.Runtime.Serialization.StreamingContext context)
      { global::ProtoBuf.Serializer.Serialize(info, this); }
    
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"orders_for_account")]
  [global::System.Runtime.Serialization.DataContract(Name=@"orders_for_account")]
  public partial class OrdersForAccount : global::ProtoBuf.IExtensible, global::System.Runtime.Serialization.ISerializable
  {
    public OrdersForAccount() {}
    
    private readonly global::System.Collections.Generic.List<IronSmarkets.Proto.Seto.OrdersForMarket> _markets = new global::System.Collections.Generic.List<IronSmarkets.Proto.Seto.OrdersForMarket>();
    [global::ProtoBuf.ProtoMember(1, Name=@"markets", DataFormat = global::ProtoBuf.DataFormat.Default)]
    [global::System.Runtime.Serialization.DataMember(Name=@"markets", Order = 1, IsRequired = false)]
    
    public global::System.Collections.Generic.List<IronSmarkets.Proto.Seto.OrdersForMarket> Markets
    {
      get { return _markets; }
    }
  
    protected OrdersForAccount(global::System.Runtime.Serialization.SerializationInfo info, global::System.Runtime.Serialization.StreamingContext context)
      : this() { global::ProtoBuf.Serializer.Merge(info, this); }
    void global::System.Runtime.Serialization.ISerializable.GetObjectData(global::System.Runtime.Serialization.SerializationInfo info, global::System.Runtime.Serialization.StreamingContext context)
      { global::ProtoBuf.Serializer.Serialize(info, this); }
    
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"orders_for_market_request")]
  [global::System.Runtime.Serialization.DataContract(Name=@"orders_for_market_request")]
  public partial class OrdersForMarketRequest : global::ProtoBuf.IExtensible, global::System.Runtime.Serialization.ISerializable
  {
    public OrdersForMarketRequest() {}
    
    private IronSmarkets.Proto.Seto.Uuid128 _market;
    [global::ProtoBuf.ProtoMember(1, IsRequired = true, Name=@"market", DataFormat = global::ProtoBuf.DataFormat.Default)]
    [global::System.Runtime.Serialization.DataMember(Name=@"market", Order = 1, IsRequired = true)]
    
    public IronSmarkets.Proto.Seto.Uuid128 Market
    {
      get { return _market; }
      set { _market = value; }
    }
    protected OrdersForMarketRequest(global::System.Runtime.Serialization.SerializationInfo info, global::System.Runtime.Serialization.StreamingContext context)
      : this() { global::ProtoBuf.Serializer.Merge(info, this); }
    void global::System.Runtime.Serialization.ISerializable.GetObjectData(global::System.Runtime.Serialization.SerializationInfo info, global::System.Runtime.Serialization.StreamingContext context)
      { global::ProtoBuf.Serializer.Serialize(info, this); }
    
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"orders_for_market")]
  [global::System.Runtime.Serialization.DataContract(Name=@"orders_for_market")]
  public partial class OrdersForMarket : global::ProtoBuf.IExtensible, global::System.Runtime.Serialization.ISerializable
  {
    public OrdersForMarket() {}
    
    private IronSmarkets.Proto.Seto.Uuid128 _market;
    [global::ProtoBuf.ProtoMember(1, IsRequired = true, Name=@"market", DataFormat = global::ProtoBuf.DataFormat.Default)]
    [global::System.Runtime.Serialization.DataMember(Name=@"market", Order = 1, IsRequired = true)]
    
    public IronSmarkets.Proto.Seto.Uuid128 Market
    {
      get { return _market; }
      set { _market = value; }
    }
    private IronSmarkets.Proto.Seto.PriceType _priceType;
    [global::ProtoBuf.ProtoMember(2, IsRequired = true, Name=@"price_type", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.Runtime.Serialization.DataMember(Name=@"price_type", Order = 2, IsRequired = true)]
    
    public IronSmarkets.Proto.Seto.PriceType PriceType
    {
      get { return _priceType; }
      set { _priceType = value; }
    }
    private readonly global::System.Collections.Generic.List<IronSmarkets.Proto.Seto.OrdersForContract> _contracts = new global::System.Collections.Generic.List<IronSmarkets.Proto.Seto.OrdersForContract>();
    [global::ProtoBuf.ProtoMember(3, Name=@"contracts", DataFormat = global::ProtoBuf.DataFormat.Default)]
    [global::System.Runtime.Serialization.DataMember(Name=@"contracts", Order = 3, IsRequired = false)]
    
    public global::System.Collections.Generic.List<IronSmarkets.Proto.Seto.OrdersForContract> Contracts
    {
      get { return _contracts; }
    }
  
    protected OrdersForMarket(global::System.Runtime.Serialization.SerializationInfo info, global::System.Runtime.Serialization.StreamingContext context)
      : this() { global::ProtoBuf.Serializer.Merge(info, this); }
    void global::System.Runtime.Serialization.ISerializable.GetObjectData(global::System.Runtime.Serialization.SerializationInfo info, global::System.Runtime.Serialization.StreamingContext context)
      { global::ProtoBuf.Serializer.Serialize(info, this); }
    
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"orders_for_contract")]
  [global::System.Runtime.Serialization.DataContract(Name=@"orders_for_contract")]
  public partial class OrdersForContract : global::ProtoBuf.IExtensible, global::System.Runtime.Serialization.ISerializable
  {
    public OrdersForContract() {}
    
    private IronSmarkets.Proto.Seto.Uuid128 _contract;
    [global::ProtoBuf.ProtoMember(1, IsRequired = true, Name=@"contract", DataFormat = global::ProtoBuf.DataFormat.Default)]
    [global::System.Runtime.Serialization.DataMember(Name=@"contract", Order = 1, IsRequired = true)]
    
    public IronSmarkets.Proto.Seto.Uuid128 Contract
    {
      get { return _contract; }
      set { _contract = value; }
    }
    private readonly global::System.Collections.Generic.List<IronSmarkets.Proto.Seto.OrdersForPrice> _bids = new global::System.Collections.Generic.List<IronSmarkets.Proto.Seto.OrdersForPrice>();
    [global::ProtoBuf.ProtoMember(2, Name=@"bids", DataFormat = global::ProtoBuf.DataFormat.Default)]
    [global::System.Runtime.Serialization.DataMember(Name=@"bids", Order = 2, IsRequired = false)]
    
    public global::System.Collections.Generic.List<IronSmarkets.Proto.Seto.OrdersForPrice> Bids
    {
      get { return _bids; }
    }
  
    private readonly global::System.Collections.Generic.List<IronSmarkets.Proto.Seto.OrdersForPrice> _offers = new global::System.Collections.Generic.List<IronSmarkets.Proto.Seto.OrdersForPrice>();
    [global::ProtoBuf.ProtoMember(3, Name=@"offers", DataFormat = global::ProtoBuf.DataFormat.Default)]
    [global::System.Runtime.Serialization.DataMember(Name=@"offers", Order = 3, IsRequired = false)]
    
    public global::System.Collections.Generic.List<IronSmarkets.Proto.Seto.OrdersForPrice> Offers
    {
      get { return _offers; }
    }
  
    protected OrdersForContract(global::System.Runtime.Serialization.SerializationInfo info, global::System.Runtime.Serialization.StreamingContext context)
      : this() { global::ProtoBuf.Serializer.Merge(info, this); }
    void global::System.Runtime.Serialization.ISerializable.GetObjectData(global::System.Runtime.Serialization.SerializationInfo info, global::System.Runtime.Serialization.StreamingContext context)
      { global::ProtoBuf.Serializer.Serialize(info, this); }
    
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"orders_for_price")]
  [global::System.Runtime.Serialization.DataContract(Name=@"orders_for_price")]
  public partial class OrdersForPrice : global::ProtoBuf.IExtensible, global::System.Runtime.Serialization.ISerializable
  {
    public OrdersForPrice() {}
    
    private uint _price;
    [global::ProtoBuf.ProtoMember(1, IsRequired = true, Name=@"price", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.Runtime.Serialization.DataMember(Name=@"price", Order = 1, IsRequired = true)]
    
    public uint Price
    {
      get { return _price; }
      set { _price = value; }
    }
    private readonly global::System.Collections.Generic.List<IronSmarkets.Proto.Seto.OrderState> _orders = new global::System.Collections.Generic.List<IronSmarkets.Proto.Seto.OrderState>();
    [global::ProtoBuf.ProtoMember(2, Name=@"orders", DataFormat = global::ProtoBuf.DataFormat.Default)]
    [global::System.Runtime.Serialization.DataMember(Name=@"orders", Order = 2, IsRequired = false)]
    
    public global::System.Collections.Generic.List<IronSmarkets.Proto.Seto.OrderState> Orders
    {
      get { return _orders; }
    }
  
    protected OrdersForPrice(global::System.Runtime.Serialization.SerializationInfo info, global::System.Runtime.Serialization.StreamingContext context)
      : this() { global::ProtoBuf.Serializer.Merge(info, this); }
    void global::System.Runtime.Serialization.ISerializable.GetObjectData(global::System.Runtime.Serialization.SerializationInfo info, global::System.Runtime.Serialization.StreamingContext context)
      { global::ProtoBuf.Serializer.Serialize(info, this); }
    
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"OrderState")]
  [global::System.Runtime.Serialization.DataContract(Name=@"OrderState")]
  public partial class OrderState : global::ProtoBuf.IExtensible, global::System.Runtime.Serialization.ISerializable
  {
    public OrderState() {}
    
    private IronSmarkets.Proto.Seto.Uuid128 _order;
    [global::ProtoBuf.ProtoMember(1, IsRequired = true, Name=@"order", DataFormat = global::ProtoBuf.DataFormat.Default)]
    [global::System.Runtime.Serialization.DataMember(Name=@"order", Order = 1, IsRequired = true)]
    
    public IronSmarkets.Proto.Seto.Uuid128 Order
    {
      get { return _order; }
      set { _order = value; }
    }
    private IronSmarkets.Proto.Seto.OrderCreateType _type;
    [global::ProtoBuf.ProtoMember(2, IsRequired = true, Name=@"type", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.Runtime.Serialization.DataMember(Name=@"type", Order = 2, IsRequired = true)]
    
    public IronSmarkets.Proto.Seto.OrderCreateType Type
    {
      get { return _type; }
      set { _type = value; }
    }
    private IronSmarkets.Proto.Seto.OrderStatus _status;
    [global::ProtoBuf.ProtoMember(3, IsRequired = true, Name=@"status", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.Runtime.Serialization.DataMember(Name=@"status", Order = 3, IsRequired = true)]
    
    public IronSmarkets.Proto.Seto.OrderStatus Status
    {
      get { return _status; }
      set { _status = value; }
    }
    private IronSmarkets.Proto.Seto.QuantityType _quantityType;
    [global::ProtoBuf.ProtoMember(4, IsRequired = true, Name=@"quantity_type", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.Runtime.Serialization.DataMember(Name=@"quantity_type", Order = 4, IsRequired = true)]
    
    public IronSmarkets.Proto.Seto.QuantityType QuantityType
    {
      get { return _quantityType; }
      set { _quantityType = value; }
    }
    private uint _quantity;
    [global::ProtoBuf.ProtoMember(5, IsRequired = true, Name=@"quantity", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.Runtime.Serialization.DataMember(Name=@"quantity", Order = 5, IsRequired = true)]
    
    public uint Quantity
    {
      get { return _quantity; }
      set { _quantity = value; }
    }
    private ulong _createdMicroseconds;
    [global::ProtoBuf.ProtoMember(6, IsRequired = true, Name=@"created_microseconds", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.Runtime.Serialization.DataMember(Name=@"created_microseconds", Order = 6, IsRequired = true)]
    
    public ulong CreatedMicroseconds
    {
      get { return _createdMicroseconds; }
      set { _createdMicroseconds = value; }
    }

    private uint _quantityFilled = (uint)0;
    [global::ProtoBuf.ProtoMember(7, IsRequired = false, Name=@"quantity_filled", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.ComponentModel.DefaultValue((uint)0)]
    [global::System.Runtime.Serialization.DataMember(Name=@"quantity_filled", Order = 7, IsRequired = false)]
    
    public uint QuantityFilled
    {
      get { return _quantityFilled; }
      set { _quantityFilled = value; }
    }
    protected OrderState(global::System.Runtime.Serialization.SerializationInfo info, global::System.Runtime.Serialization.StreamingContext context)
      : this() { global::ProtoBuf.Serializer.Merge(info, this); }
    void global::System.Runtime.Serialization.ISerializable.GetObjectData(global::System.Runtime.Serialization.SerializationInfo info, global::System.Runtime.Serialization.StreamingContext context)
      { global::ProtoBuf.Serializer.Serialize(info, this); }
    
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"AccountStateRequest")]
  [global::System.Runtime.Serialization.DataContract(Name=@"AccountStateRequest")]
  public partial class AccountStateRequest : global::ProtoBuf.IExtensible, global::System.Runtime.Serialization.ISerializable
  {
    public AccountStateRequest() {}
    

    private IronSmarkets.Proto.Seto.Uuid128 _account = null;
    [global::ProtoBuf.ProtoMember(1, IsRequired = false, Name=@"account", DataFormat = global::ProtoBuf.DataFormat.Default)]
    [global::System.ComponentModel.DefaultValue(null)]
    [global::System.Runtime.Serialization.DataMember(Name=@"account", Order = 1, IsRequired = false)]
    
    public IronSmarkets.Proto.Seto.Uuid128 Account
    {
      get { return _account; }
      set { _account = value; }
    }
    protected AccountStateRequest(global::System.Runtime.Serialization.SerializationInfo info, global::System.Runtime.Serialization.StreamingContext context)
      : this() { global::ProtoBuf.Serializer.Merge(info, this); }
    void global::System.Runtime.Serialization.ISerializable.GetObjectData(global::System.Runtime.Serialization.SerializationInfo info, global::System.Runtime.Serialization.StreamingContext context)
      { global::ProtoBuf.Serializer.Serialize(info, this); }
    
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"AccountState")]
  [global::System.Runtime.Serialization.DataContract(Name=@"AccountState")]
  public partial class AccountState : global::ProtoBuf.IExtensible, global::System.Runtime.Serialization.ISerializable
  {
    public AccountState() {}
    
    private IronSmarkets.Proto.Seto.Uuid128 _account;
    [global::ProtoBuf.ProtoMember(1, IsRequired = true, Name=@"account", DataFormat = global::ProtoBuf.DataFormat.Default)]
    [global::System.Runtime.Serialization.DataMember(Name=@"account", Order = 1, IsRequired = true)]
    
    public IronSmarkets.Proto.Seto.Uuid128 Account
    {
      get { return _account; }
      set { _account = value; }
    }
    private IronSmarkets.Proto.Seto.Currency _currency;
    [global::ProtoBuf.ProtoMember(2, IsRequired = true, Name=@"currency", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.Runtime.Serialization.DataMember(Name=@"currency", Order = 2, IsRequired = true)]
    
    public IronSmarkets.Proto.Seto.Currency Currency
    {
      get { return _currency; }
      set { _currency = value; }
    }
    private IronSmarkets.Proto.Seto.Decimal _balance;
    [global::ProtoBuf.ProtoMember(3, IsRequired = true, Name=@"balance", DataFormat = global::ProtoBuf.DataFormat.Default)]
    [global::System.Runtime.Serialization.DataMember(Name=@"balance", Order = 3, IsRequired = true)]
    
    public IronSmarkets.Proto.Seto.Decimal Balance
    {
      get { return _balance; }
      set { _balance = value; }
    }
    private IronSmarkets.Proto.Seto.Decimal _bonus;
    [global::ProtoBuf.ProtoMember(4, IsRequired = true, Name=@"bonus", DataFormat = global::ProtoBuf.DataFormat.Default)]
    [global::System.Runtime.Serialization.DataMember(Name=@"bonus", Order = 4, IsRequired = true)]
    
    public IronSmarkets.Proto.Seto.Decimal Bonus
    {
      get { return _bonus; }
      set { _bonus = value; }
    }
    private IronSmarkets.Proto.Seto.Decimal _exposure;
    [global::ProtoBuf.ProtoMember(5, IsRequired = true, Name=@"exposure", DataFormat = global::ProtoBuf.DataFormat.Default)]
    [global::System.Runtime.Serialization.DataMember(Name=@"exposure", Order = 5, IsRequired = true)]
    
    public IronSmarkets.Proto.Seto.Decimal Exposure
    {
      get { return _exposure; }
      set { _exposure = value; }
    }
    protected AccountState(global::System.Runtime.Serialization.SerializationInfo info, global::System.Runtime.Serialization.StreamingContext context)
      : this() { global::ProtoBuf.Serializer.Merge(info, this); }
    void global::System.Runtime.Serialization.ISerializable.GetObjectData(global::System.Runtime.Serialization.SerializationInfo info, global::System.Runtime.Serialization.StreamingContext context)
      { global::ProtoBuf.Serializer.Serialize(info, this); }
    
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"account")]
  [global::System.Runtime.Serialization.DataContract(Name=@"account")]
  public partial class Account : global::ProtoBuf.IExtensible, global::System.Runtime.Serialization.ISerializable
  {
    public Account() {}
    
    private ulong _low;
    [global::ProtoBuf.ProtoMember(1, IsRequired = true, Name=@"low", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.Runtime.Serialization.DataMember(Name=@"low", Order = 1, IsRequired = true)]
    
    public ulong Low
    {
      get { return _low; }
      set { _low = value; }
    }

    private ulong _high = (ulong)0;
    [global::ProtoBuf.ProtoMember(2, IsRequired = false, Name=@"high", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.ComponentModel.DefaultValue((ulong)0)]
    [global::System.Runtime.Serialization.DataMember(Name=@"high", Order = 2, IsRequired = false)]
    
    public ulong High
    {
      get { return _high; }
      set { _high = value; }
    }
    protected Account(global::System.Runtime.Serialization.SerializationInfo info, global::System.Runtime.Serialization.StreamingContext context)
      : this() { global::ProtoBuf.Serializer.Merge(info, this); }
    void global::System.Runtime.Serialization.ISerializable.GetObjectData(global::System.Runtime.Serialization.SerializationInfo info, global::System.Runtime.Serialization.StreamingContext context)
      { global::ProtoBuf.Serializer.Serialize(info, this); }
    
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"Decimal")]
  [global::System.Runtime.Serialization.DataContract(Name=@"Decimal")]
  public partial class Decimal : global::ProtoBuf.IExtensible, global::System.Runtime.Serialization.ISerializable
  {
    public Decimal() {}
    
    private long _value;
    [global::ProtoBuf.ProtoMember(1, IsRequired = true, Name=@"value", DataFormat = global::ProtoBuf.DataFormat.ZigZag)]
    [global::System.Runtime.Serialization.DataMember(Name=@"value", Order = 1, IsRequired = true)]
    
    public long Value
    {
      get { return _value; }
      set { _value = value; }
    }

    private uint _exponent = (uint)2;
    [global::ProtoBuf.ProtoMember(2, IsRequired = false, Name=@"exponent", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.ComponentModel.DefaultValue((uint)2)]
    [global::System.Runtime.Serialization.DataMember(Name=@"exponent", Order = 2, IsRequired = false)]
    
    public uint Exponent
    {
      get { return _exponent; }
      set { _exponent = value; }
    }
    protected Decimal(global::System.Runtime.Serialization.SerializationInfo info, global::System.Runtime.Serialization.StreamingContext context)
      : this() { global::ProtoBuf.Serializer.Merge(info, this); }
    void global::System.Runtime.Serialization.ISerializable.GetObjectData(global::System.Runtime.Serialization.SerializationInfo info, global::System.Runtime.Serialization.StreamingContext context)
      { global::ProtoBuf.Serializer.Serialize(info, this); }
    
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
    [global::ProtoBuf.ProtoContract(Name=@"PayloadType")]
    [global::System.Runtime.Serialization.DataContract(Name=@"PayloadType")]
    public enum PayloadType
    {
            
      [global::ProtoBuf.ProtoEnum(Name=@"PAYLOAD_ETO", Value=1)]
      [global::System.Runtime.Serialization.EnumMember(Value=@"PAYLOAD_ETO")]
      PAYLOADETO = 1,
            
      [global::ProtoBuf.ProtoEnum(Name=@"PAYLOAD_LOGIN", Value=2)]
      [global::System.Runtime.Serialization.EnumMember(Value=@"PAYLOAD_LOGIN")]
      PAYLOADLOGIN = 2,
            
      [global::ProtoBuf.ProtoEnum(Name=@"PAYLOAD_ORDER_CREATE", Value=3)]
      [global::System.Runtime.Serialization.EnumMember(Value=@"PAYLOAD_ORDER_CREATE")]
      PAYLOADORDERCREATE = 3,
            
      [global::ProtoBuf.ProtoEnum(Name=@"PAYLOAD_ORDER_REJECTED", Value=4)]
      [global::System.Runtime.Serialization.EnumMember(Value=@"PAYLOAD_ORDER_REJECTED")]
      PAYLOADORDERREJECTED = 4,
            
      [global::ProtoBuf.ProtoEnum(Name=@"PAYLOAD_ORDER_ACCEPTED", Value=5)]
      [global::System.Runtime.Serialization.EnumMember(Value=@"PAYLOAD_ORDER_ACCEPTED")]
      PAYLOADORDERACCEPTED = 5,
            
      [global::ProtoBuf.ProtoEnum(Name=@"PAYLOAD_ORDER_EXECUTED", Value=6)]
      [global::System.Runtime.Serialization.EnumMember(Value=@"PAYLOAD_ORDER_EXECUTED")]
      PAYLOADORDEREXECUTED = 6,
            
      [global::ProtoBuf.ProtoEnum(Name=@"PAYLOAD_ORDER_CANCEL", Value=7)]
      [global::System.Runtime.Serialization.EnumMember(Value=@"PAYLOAD_ORDER_CANCEL")]
      PAYLOADORDERCANCEL = 7,
            
      [global::ProtoBuf.ProtoEnum(Name=@"PAYLOAD_ORDER_CANCELLED", Value=8)]
      [global::System.Runtime.Serialization.EnumMember(Value=@"PAYLOAD_ORDER_CANCELLED")]
      PAYLOADORDERCANCELLED = 8,
            
      [global::ProtoBuf.ProtoEnum(Name=@"PAYLOAD_ORDER_INVALID", Value=9)]
      [global::System.Runtime.Serialization.EnumMember(Value=@"PAYLOAD_ORDER_INVALID")]
      PAYLOADORDERINVALID = 9,
            
      [global::ProtoBuf.ProtoEnum(Name=@"PAYLOAD_MARKET_SUBSCRIBE", Value=10)]
      [global::System.Runtime.Serialization.EnumMember(Value=@"PAYLOAD_MARKET_SUBSCRIBE")]
      PAYLOADMARKETSUBSCRIBE = 10,
            
      [global::ProtoBuf.ProtoEnum(Name=@"PAYLOAD_MARKET_UNSUBSCRIBE", Value=11)]
      [global::System.Runtime.Serialization.EnumMember(Value=@"PAYLOAD_MARKET_UNSUBSCRIBE")]
      PAYLOADMARKETUNSUBSCRIBE = 11,
            
      [global::ProtoBuf.ProtoEnum(Name=@"PAYLOAD_MARKET_QUOTES_REQUEST", Value=12)]
      [global::System.Runtime.Serialization.EnumMember(Value=@"PAYLOAD_MARKET_QUOTES_REQUEST")]
      PAYLOADMARKETQUOTESREQUEST = 12,
            
      [global::ProtoBuf.ProtoEnum(Name=@"PAYLOAD_MARKET_QUOTES", Value=13)]
      [global::System.Runtime.Serialization.EnumMember(Value=@"PAYLOAD_MARKET_QUOTES")]
      PAYLOADMARKETQUOTES = 13,
            
      [global::ProtoBuf.ProtoEnum(Name=@"PAYLOAD_CONTRACT_QUOTES", Value=14)]
      [global::System.Runtime.Serialization.EnumMember(Value=@"PAYLOAD_CONTRACT_QUOTES")]
      PAYLOADCONTRACTQUOTES = 14,
            
      [global::ProtoBuf.ProtoEnum(Name=@"PAYLOAD_EVENTS_REQUEST", Value=15)]
      [global::System.Runtime.Serialization.EnumMember(Value=@"PAYLOAD_EVENTS_REQUEST")]
      PAYLOADEVENTSREQUEST = 15,
            
      [global::ProtoBuf.ProtoEnum(Name=@"PAYLOAD_HTTP_FOUND", Value=16)]
      [global::System.Runtime.Serialization.EnumMember(Value=@"PAYLOAD_HTTP_FOUND")]
      PAYLOADHTTPFOUND = 16,
            
      [global::ProtoBuf.ProtoEnum(Name=@"PAYLOAD_INVALID_REQUEST", Value=17)]
      [global::System.Runtime.Serialization.EnumMember(Value=@"PAYLOAD_INVALID_REQUEST")]
      PAYLOADINVALIDREQUEST = 17,
            
      [global::ProtoBuf.ProtoEnum(Name=@"PAYLOAD_ORDER_CANCEL_REJECTED", Value=18)]
      [global::System.Runtime.Serialization.EnumMember(Value=@"PAYLOAD_ORDER_CANCEL_REJECTED")]
      PAYLOADORDERCANCELREJECTED = 18,
            
      [global::ProtoBuf.ProtoEnum(Name=@"PAYLOAD_ORDERS_FOR_ACCOUNT_REQUEST", Value=19)]
      [global::System.Runtime.Serialization.EnumMember(Value=@"PAYLOAD_ORDERS_FOR_ACCOUNT_REQUEST")]
      PAYLOADORDERSFORACCOUNTREQUEST = 19,
            
      [global::ProtoBuf.ProtoEnum(Name=@"PAYLOAD_ORDERS_FOR_ACCOUNT", Value=20)]
      [global::System.Runtime.Serialization.EnumMember(Value=@"PAYLOAD_ORDERS_FOR_ACCOUNT")]
      PAYLOADORDERSFORACCOUNT = 20,
            
      [global::ProtoBuf.ProtoEnum(Name=@"PAYLOAD_ORDERS_FOR_MARKET_REQUEST", Value=21)]
      [global::System.Runtime.Serialization.EnumMember(Value=@"PAYLOAD_ORDERS_FOR_MARKET_REQUEST")]
      PAYLOADORDERSFORMARKETREQUEST = 21,
            
      [global::ProtoBuf.ProtoEnum(Name=@"PAYLOAD_ORDERS_FOR_MARKET", Value=22)]
      [global::System.Runtime.Serialization.EnumMember(Value=@"PAYLOAD_ORDERS_FOR_MARKET")]
      PAYLOADORDERSFORMARKET = 22,
            
      [global::ProtoBuf.ProtoEnum(Name=@"PAYLOAD_ACCOUNT_STATE_REQUEST", Value=23)]
      [global::System.Runtime.Serialization.EnumMember(Value=@"PAYLOAD_ACCOUNT_STATE_REQUEST")]
      PAYLOADACCOUNTSTATEREQUEST = 23,
            
      [global::ProtoBuf.ProtoEnum(Name=@"PAYLOAD_ACCOUNT_STATE", Value=24)]
      [global::System.Runtime.Serialization.EnumMember(Value=@"PAYLOAD_ACCOUNT_STATE")]
      PAYLOADACCOUNTSTATE = 24
    }
  
    [global::ProtoBuf.ProtoContract(Name=@"OrderCreateType")]
    [global::System.Runtime.Serialization.DataContract(Name=@"OrderCreateType")]
    public enum OrderCreateType
    {
            
      [global::ProtoBuf.ProtoEnum(Name=@"ORDER_CREATE_LIMIT", Value=1)]
      [global::System.Runtime.Serialization.EnumMember(Value=@"ORDER_CREATE_LIMIT")]
      ORDERCREATELIMIT = 1
    }
  
    [global::ProtoBuf.ProtoContract(Name=@"OrderRejectedReason")]
    [global::System.Runtime.Serialization.DataContract(Name=@"OrderRejectedReason")]
    public enum OrderRejectedReason
    {
            
      [global::ProtoBuf.ProtoEnum(Name=@"ORDER_REJECTED_INSUFFICIENT_FUNDS", Value=1)]
      [global::System.Runtime.Serialization.EnumMember(Value=@"ORDER_REJECTED_INSUFFICIENT_FUNDS")]
      ORDERREJECTEDINSUFFICIENTFUNDS = 1,
            
      [global::ProtoBuf.ProtoEnum(Name=@"ORDER_REJECTED_LIMIT_EXCEEDED", Value=2)]
      [global::System.Runtime.Serialization.EnumMember(Value=@"ORDER_REJECTED_LIMIT_EXCEEDED")]
      ORDERREJECTEDLIMITEXCEEDED = 2,
            
      [global::ProtoBuf.ProtoEnum(Name=@"ORDER_REJECTED_MARKET_NOT_OPEN", Value=3)]
      [global::System.Runtime.Serialization.EnumMember(Value=@"ORDER_REJECTED_MARKET_NOT_OPEN")]
      ORDERREJECTEDMARKETNOTOPEN = 3,
            
      [global::ProtoBuf.ProtoEnum(Name=@"ORDER_REJECTED_MARKET_SETTLED", Value=4)]
      [global::System.Runtime.Serialization.EnumMember(Value=@"ORDER_REJECTED_MARKET_SETTLED")]
      ORDERREJECTEDMARKETSETTLED = 4,
            
      [global::ProtoBuf.ProtoEnum(Name=@"ORDER_REJECTED_MARKET_HALTED", Value=5)]
      [global::System.Runtime.Serialization.EnumMember(Value=@"ORDER_REJECTED_MARKET_HALTED")]
      ORDERREJECTEDMARKETHALTED = 5,
            
      [global::ProtoBuf.ProtoEnum(Name=@"ORDER_REJECTED_CROSSED_SELF", Value=6)]
      [global::System.Runtime.Serialization.EnumMember(Value=@"ORDER_REJECTED_CROSSED_SELF")]
      ORDERREJECTEDCROSSEDSELF = 6,
            
      [global::ProtoBuf.ProtoEnum(Name=@"ORDER_REJECTED_MARKET_NOT_FOUND", Value=7)]
      [global::System.Runtime.Serialization.EnumMember(Value=@"ORDER_REJECTED_MARKET_NOT_FOUND")]
      ORDERREJECTEDMARKETNOTFOUND = 7,
            
      [global::ProtoBuf.ProtoEnum(Name=@"LOGOUT_SERVICE_TEMPORARILY_UNAVAILABLE", Value=8)]
      [global::System.Runtime.Serialization.EnumMember(Value=@"LOGOUT_SERVICE_TEMPORARILY_UNAVAILABLE")]
      LOGOUTSERVICETEMPORARILYUNAVAILABLE = 8
    }
  
    [global::ProtoBuf.ProtoContract(Name=@"OrderCancelRejectedReason")]
    [global::System.Runtime.Serialization.DataContract(Name=@"OrderCancelRejectedReason")]
    public enum OrderCancelRejectedReason
    {
            
      [global::ProtoBuf.ProtoEnum(Name=@"ORDER_CANCEL_REJECTED_NOT_FOUND", Value=1)]
      [global::System.Runtime.Serialization.EnumMember(Value=@"ORDER_CANCEL_REJECTED_NOT_FOUND")]
      ORDERCANCELREJECTEDNOTFOUND = 1,
            
      [global::ProtoBuf.ProtoEnum(Name=@"ORDER_CANCEL_REJECTED_NOT_LIVE", Value=2)]
      [global::System.Runtime.Serialization.EnumMember(Value=@"ORDER_CANCEL_REJECTED_NOT_LIVE")]
      ORDERCANCELREJECTEDNOTLIVE = 2
    }
  
    [global::ProtoBuf.ProtoContract(Name=@"OrderCancelledReason")]
    [global::System.Runtime.Serialization.DataContract(Name=@"OrderCancelledReason")]
    public enum OrderCancelledReason
    {
            
      [global::ProtoBuf.ProtoEnum(Name=@"ORDER_CANCELLED_MEMBER_REQUESTED", Value=1)]
      [global::System.Runtime.Serialization.EnumMember(Value=@"ORDER_CANCELLED_MEMBER_REQUESTED")]
      ORDERCANCELLEDMEMBERREQUESTED = 1,
            
      [global::ProtoBuf.ProtoEnum(Name=@"ORDER_CANCELLED_MARKET_HALTED", Value=2)]
      [global::System.Runtime.Serialization.EnumMember(Value=@"ORDER_CANCELLED_MARKET_HALTED")]
      ORDERCANCELLEDMARKETHALTED = 2,
            
      [global::ProtoBuf.ProtoEnum(Name=@"ORDER_CANCELLED_INSUFFICIENT_LIQUIDITY", Value=3)]
      [global::System.Runtime.Serialization.EnumMember(Value=@"ORDER_CANCELLED_INSUFFICIENT_LIQUIDITY")]
      ORDERCANCELLEDINSUFFICIENTLIQUIDITY = 3
    }
  
    [global::ProtoBuf.ProtoContract(Name=@"OrderInvalidReason")]
    [global::System.Runtime.Serialization.DataContract(Name=@"OrderInvalidReason")]
    public enum OrderInvalidReason
    {
            
      [global::ProtoBuf.ProtoEnum(Name=@"ORDER_INVALID_INVALID_PRICE", Value=1)]
      [global::System.Runtime.Serialization.EnumMember(Value=@"ORDER_INVALID_INVALID_PRICE")]
      ORDERINVALIDINVALIDPRICE = 1,
            
      [global::ProtoBuf.ProtoEnum(Name=@"ORDER_INVALID_INVALID_QUANTITY", Value=2)]
      [global::System.Runtime.Serialization.EnumMember(Value=@"ORDER_INVALID_INVALID_QUANTITY")]
      ORDERINVALIDINVALIDQUANTITY = 2
    }
  
    [global::ProtoBuf.ProtoContract(Name=@"QuantityType")]
    [global::System.Runtime.Serialization.DataContract(Name=@"QuantityType")]
    public enum QuantityType
    {
            
      [global::ProtoBuf.ProtoEnum(Name=@"QUANTITY_PAYOFF_CURRENCY", Value=1)]
      [global::System.Runtime.Serialization.EnumMember(Value=@"QUANTITY_PAYOFF_CURRENCY")]
      QUANTITYPAYOFFCURRENCY = 1
    }
  
    [global::ProtoBuf.ProtoContract(Name=@"PriceType")]
    [global::System.Runtime.Serialization.DataContract(Name=@"PriceType")]
    public enum PriceType
    {
            
      [global::ProtoBuf.ProtoEnum(Name=@"PRICE_PERCENT_ODDS", Value=1)]
      [global::System.Runtime.Serialization.EnumMember(Value=@"PRICE_PERCENT_ODDS")]
      PRICEPERCENTODDS = 1
    }
  
    [global::ProtoBuf.ProtoContract(Name=@"Side")]
    [global::System.Runtime.Serialization.DataContract(Name=@"Side")]
    public enum Side
    {
            
      [global::ProtoBuf.ProtoEnum(Name=@"SIDE_BUY", Value=1)]
      [global::System.Runtime.Serialization.EnumMember(Value=@"SIDE_BUY")]
      SIDEBUY = 1,
            
      [global::ProtoBuf.ProtoEnum(Name=@"SIDE_SELL", Value=2)]
      [global::System.Runtime.Serialization.EnumMember(Value=@"SIDE_SELL")]
      SIDESELL = 2
    }
  
    [global::ProtoBuf.ProtoContract(Name=@"ContentType")]
    [global::System.Runtime.Serialization.DataContract(Name=@"ContentType")]
    public enum ContentType
    {
            
      [global::ProtoBuf.ProtoEnum(Name=@"CONTENT_TYPE_PROTOBUF", Value=1)]
      [global::System.Runtime.Serialization.EnumMember(Value=@"CONTENT_TYPE_PROTOBUF")]
      CONTENTTYPEPROTOBUF = 1
    }
  
    [global::ProtoBuf.ProtoContract(Name=@"EventsRequestType")]
    [global::System.Runtime.Serialization.DataContract(Name=@"EventsRequestType")]
    public enum EventsRequestType
    {
            
      [global::ProtoBuf.ProtoEnum(Name=@"EVENTS_REQUEST_POLITICS", Value=1)]
      [global::System.Runtime.Serialization.EnumMember(Value=@"EVENTS_REQUEST_POLITICS")]
      EVENTSREQUESTPOLITICS = 1,
            
      [global::ProtoBuf.ProtoEnum(Name=@"EVENTS_REQUEST_CURRENT_AFFAIRS", Value=2)]
      [global::System.Runtime.Serialization.EnumMember(Value=@"EVENTS_REQUEST_CURRENT_AFFAIRS")]
      EVENTSREQUESTCURRENTAFFAIRS = 2,
            
      [global::ProtoBuf.ProtoEnum(Name=@"EVENTS_REQUEST_TV_AND_ENTERTAINMENT", Value=3)]
      [global::System.Runtime.Serialization.EnumMember(Value=@"EVENTS_REQUEST_TV_AND_ENTERTAINMENT")]
      EVENTSREQUESTTVANDENTERTAINMENT = 3,
            
      [global::ProtoBuf.ProtoEnum(Name=@"EVENTS_REQUEST_SPORT_BY_DATE", Value=4)]
      [global::System.Runtime.Serialization.EnumMember(Value=@"EVENTS_REQUEST_SPORT_BY_DATE")]
      EVENTSREQUESTSPORTBYDATE = 4,
            
      [global::ProtoBuf.ProtoEnum(Name=@"EVENTS_REQUEST_SPORT_OTHER", Value=5)]
      [global::System.Runtime.Serialization.EnumMember(Value=@"EVENTS_REQUEST_SPORT_OTHER")]
      EVENTSREQUESTSPORTOTHER = 5
    }
  
    [global::ProtoBuf.ProtoContract(Name=@"SportByDateType")]
    [global::System.Runtime.Serialization.DataContract(Name=@"SportByDateType")]
    public enum SportByDateType
    {
            
      [global::ProtoBuf.ProtoEnum(Name=@"SPORT_BY_DATE_FOOTBALL", Value=1)]
      [global::System.Runtime.Serialization.EnumMember(Value=@"SPORT_BY_DATE_FOOTBALL")]
      SPORTBYDATEFOOTBALL = 1,
            
      [global::ProtoBuf.ProtoEnum(Name=@"SPORT_BY_DATE_HORSE_RACING", Value=2)]
      [global::System.Runtime.Serialization.EnumMember(Value=@"SPORT_BY_DATE_HORSE_RACING")]
      SPORTBYDATEHORSERACING = 2,
            
      [global::ProtoBuf.ProtoEnum(Name=@"SPORT_BY_DATE_TENNIS", Value=3)]
      [global::System.Runtime.Serialization.EnumMember(Value=@"SPORT_BY_DATE_TENNIS")]
      SPORTBYDATETENNIS = 3
    }
  
    [global::ProtoBuf.ProtoContract(Name=@"ContractType")]
    [global::System.Runtime.Serialization.DataContract(Name=@"ContractType")]
    public enum ContractType
    {
            
      [global::ProtoBuf.ProtoEnum(Name=@"CONTRACT_HALF_TIME_FULL_TIME", Value=1)]
      [global::System.Runtime.Serialization.EnumMember(Value=@"CONTRACT_HALF_TIME_FULL_TIME")]
      CONTRACTHALFTIMEFULLTIME = 1,
            
      [global::ProtoBuf.ProtoEnum(Name=@"CONTRACT_CORRECT_SCORE", Value=2)]
      [global::System.Runtime.Serialization.EnumMember(Value=@"CONTRACT_CORRECT_SCORE")]
      CONTRACTCORRECTSCORE = 2,
            
      [global::ProtoBuf.ProtoEnum(Name=@"CONTRACT_GENERIC", Value=3)]
      [global::System.Runtime.Serialization.EnumMember(Value=@"CONTRACT_GENERIC")]
      CONTRACTGENERIC = 3,
            
      [global::ProtoBuf.ProtoEnum(Name=@"CONTRACT_WINNER", Value=4)]
      [global::System.Runtime.Serialization.EnumMember(Value=@"CONTRACT_WINNER")]
      CONTRACTWINNER = 4,
            
      [global::ProtoBuf.ProtoEnum(Name=@"CONTRACT_BINARY", Value=5)]
      [global::System.Runtime.Serialization.EnumMember(Value=@"CONTRACT_BINARY")]
      CONTRACTBINARY = 5,
            
      [global::ProtoBuf.ProtoEnum(Name=@"CONTRACT_OVER_UNDER", Value=6)]
      [global::System.Runtime.Serialization.EnumMember(Value=@"CONTRACT_OVER_UNDER")]
      CONTRACTOVERUNDER = 6
    }
  
    [global::ProtoBuf.ProtoContract(Name=@"EventType")]
    [global::System.Runtime.Serialization.DataContract(Name=@"EventType")]
    public enum EventType
    {
            
      [global::ProtoBuf.ProtoEnum(Name=@"EVENT_FOOTBALL_MATCH", Value=1)]
      [global::System.Runtime.Serialization.EnumMember(Value=@"EVENT_FOOTBALL_MATCH")]
      EVENTFOOTBALLMATCH = 1,
            
      [global::ProtoBuf.ProtoEnum(Name=@"EVENT_FOOTBALL_SEASON", Value=2)]
      [global::System.Runtime.Serialization.EnumMember(Value=@"EVENT_FOOTBALL_SEASON")]
      EVENTFOOTBALLSEASON = 2,
            
      [global::ProtoBuf.ProtoEnum(Name=@"EVENT_FOOTBALL", Value=3)]
      [global::System.Runtime.Serialization.EnumMember(Value=@"EVENT_FOOTBALL")]
      EVENTFOOTBALL = 3,
            
      [global::ProtoBuf.ProtoEnum(Name=@"EVENT_GENERIC", Value=4)]
      [global::System.Runtime.Serialization.EnumMember(Value=@"EVENT_GENERIC")]
      EVENTGENERIC = 4,
            
      [global::ProtoBuf.ProtoEnum(Name=@"EVENT_FOOTBALL_GENERIC", Value=5)]
      [global::System.Runtime.Serialization.EnumMember(Value=@"EVENT_FOOTBALL_GENERIC")]
      EVENTFOOTBALLGENERIC = 5,
            
      [global::ProtoBuf.ProtoEnum(Name=@"EVENT_GOLF_SEASON", Value=6)]
      [global::System.Runtime.Serialization.EnumMember(Value=@"EVENT_GOLF_SEASON")]
      EVENTGOLFSEASON = 6,
            
      [global::ProtoBuf.ProtoEnum(Name=@"EVENT_BOXING_SEASON", Value=7)]
      [global::System.Runtime.Serialization.EnumMember(Value=@"EVENT_BOXING_SEASON")]
      EVENTBOXINGSEASON = 7,
            
      [global::ProtoBuf.ProtoEnum(Name=@"EVENT_FORMULA_1_RACE", Value=8)]
      [global::System.Runtime.Serialization.EnumMember(Value=@"EVENT_FORMULA_1_RACE")]
      EVENTFORMULA1RACE = 8,
            
      [global::ProtoBuf.ProtoEnum(Name=@"EVENT_FORMULA_1_SEASON", Value=9)]
      [global::System.Runtime.Serialization.EnumMember(Value=@"EVENT_FORMULA_1_SEASON")]
      EVENTFORMULA1SEASON = 9,
            
      [global::ProtoBuf.ProtoEnum(Name=@"EVENT_HORSE_RACING_RACE", Value=10)]
      [global::System.Runtime.Serialization.EnumMember(Value=@"EVENT_HORSE_RACING_RACE")]
      EVENTHORSERACINGRACE = 10,
            
      [global::ProtoBuf.ProtoEnum(Name=@"EVENT_HORSE_RACING_COURSE", Value=11)]
      [global::System.Runtime.Serialization.EnumMember(Value=@"EVENT_HORSE_RACING_COURSE")]
      EVENTHORSERACINGCOURSE = 11,
            
      [global::ProtoBuf.ProtoEnum(Name=@"EVENT_HORSE_RACING", Value=12)]
      [global::System.Runtime.Serialization.EnumMember(Value=@"EVENT_HORSE_RACING")]
      EVENTHORSERACING = 12,
            
      [global::ProtoBuf.ProtoEnum(Name=@"EVENT_GOLF_GENERIC", Value=13)]
      [global::System.Runtime.Serialization.EnumMember(Value=@"EVENT_GOLF_GENERIC")]
      EVENTGOLFGENERIC = 13,
            
      [global::ProtoBuf.ProtoEnum(Name=@"EVENT_EUROVISION_SEASON", Value=14)]
      [global::System.Runtime.Serialization.EnumMember(Value=@"EVENT_EUROVISION_SEASON")]
      EVENTEUROVISIONSEASON = 14,
            
      [global::ProtoBuf.ProtoEnum(Name=@"EVENT_TENNIS_ROUND", Value=15)]
      [global::System.Runtime.Serialization.EnumMember(Value=@"EVENT_TENNIS_ROUND")]
      EVENTTENNISROUND = 15,
            
      [global::ProtoBuf.ProtoEnum(Name=@"EVENT_TENNIS_FORMAT", Value=16)]
      [global::System.Runtime.Serialization.EnumMember(Value=@"EVENT_TENNIS_FORMAT")]
      EVENTTENNISFORMAT = 16,
            
      [global::ProtoBuf.ProtoEnum(Name=@"EVENT_TENNIS_TOURNAMENT", Value=17)]
      [global::System.Runtime.Serialization.EnumMember(Value=@"EVENT_TENNIS_TOURNAMENT")]
      EVENTTENNISTOURNAMENT = 17,
            
      [global::ProtoBuf.ProtoEnum(Name=@"EVENT_CYCLING_SEASON", Value=18)]
      [global::System.Runtime.Serialization.EnumMember(Value=@"EVENT_CYCLING_SEASON")]
      EVENTCYCLINGSEASON = 18,
            
      [global::ProtoBuf.ProtoEnum(Name=@"EVENT_CYCLING_RACE", Value=19)]
      [global::System.Runtime.Serialization.EnumMember(Value=@"EVENT_CYCLING_RACE")]
      EVENTCYCLINGRACE = 19,
            
      [global::ProtoBuf.ProtoEnum(Name=@"EVENT_MOTOGP_SEASON", Value=20)]
      [global::System.Runtime.Serialization.EnumMember(Value=@"EVENT_MOTOGP_SEASON")]
      EVENTMOTOGPSEASON = 20,
            
      [global::ProtoBuf.ProtoEnum(Name=@"EVENT_BOXING_MATCH", Value=21)]
      [global::System.Runtime.Serialization.EnumMember(Value=@"EVENT_BOXING_MATCH")]
      EVENTBOXINGMATCH = 21,
            
      [global::ProtoBuf.ProtoEnum(Name=@"EVENT_AMERICAN_FOOTBALL_MATCH", Value=22)]
      [global::System.Runtime.Serialization.EnumMember(Value=@"EVENT_AMERICAN_FOOTBALL_MATCH")]
      EVENTAMERICANFOOTBALLMATCH = 22,
            
      [global::ProtoBuf.ProtoEnum(Name=@"EVENT_RUGBY_UNION_MATCH", Value=23)]
      [global::System.Runtime.Serialization.EnumMember(Value=@"EVENT_RUGBY_UNION_MATCH")]
      EVENTRUGBYUNIONMATCH = 23
    }
  
    [global::ProtoBuf.ProtoContract(Name=@"EntityRelationshipType")]
    [global::System.Runtime.Serialization.DataContract(Name=@"EntityRelationshipType")]
    public enum EntityRelationshipType
    {
            
      [global::ProtoBuf.ProtoEnum(Name=@"ENTITY_RELATIONSHIP_FOOTBALL_HOME_TEAM", Value=1)]
      [global::System.Runtime.Serialization.EnumMember(Value=@"ENTITY_RELATIONSHIP_FOOTBALL_HOME_TEAM")]
      ENTITYRELATIONSHIPFOOTBALLHOMETEAM = 1,
            
      [global::ProtoBuf.ProtoEnum(Name=@"ENTITY_RELATIONSHIP_FOOTBALL_AWAY_TEAM", Value=2)]
      [global::System.Runtime.Serialization.EnumMember(Value=@"ENTITY_RELATIONSHIP_FOOTBALL_AWAY_TEAM")]
      ENTITYRELATIONSHIPFOOTBALLAWAYTEAM = 2,
            
      [global::ProtoBuf.ProtoEnum(Name=@"ENTITY_RELATIONSHIP_FOOTBALL_COMPETITION", Value=3)]
      [global::System.Runtime.Serialization.EnumMember(Value=@"ENTITY_RELATIONSHIP_FOOTBALL_COMPETITION")]
      ENTITYRELATIONSHIPFOOTBALLCOMPETITION = 3,
            
      [global::ProtoBuf.ProtoEnum(Name=@"ENTITY_RELATIONSHIP_HORSE_RACING_COURSE", Value=4)]
      [global::System.Runtime.Serialization.EnumMember(Value=@"ENTITY_RELATIONSHIP_HORSE_RACING_COURSE")]
      ENTITYRELATIONSHIPHORSERACINGCOURSE = 4,
            
      [global::ProtoBuf.ProtoEnum(Name=@"ENTITY_RELATIONSHIP_HORSE_RACING_HORSE", Value=5)]
      [global::System.Runtime.Serialization.EnumMember(Value=@"ENTITY_RELATIONSHIP_HORSE_RACING_HORSE")]
      ENTITYRELATIONSHIPHORSERACINGHORSE = 5,
            
      [global::ProtoBuf.ProtoEnum(Name=@"ENTITY_RELATIONSHIP_HORSE_RACING_JOCKEY", Value=6)]
      [global::System.Runtime.Serialization.EnumMember(Value=@"ENTITY_RELATIONSHIP_HORSE_RACING_JOCKEY")]
      ENTITYRELATIONSHIPHORSERACINGJOCKEY = 6,
            
      [global::ProtoBuf.ProtoEnum(Name=@"ENTITY_RELATIONSHIP_CONTRACT_ASSOCIATED", Value=7)]
      [global::System.Runtime.Serialization.EnumMember(Value=@"ENTITY_RELATIONSHIP_CONTRACT_ASSOCIATED")]
      ENTITYRELATIONSHIPCONTRACTASSOCIATED = 7,
            
      [global::ProtoBuf.ProtoEnum(Name=@"ENTITY_RELATIONSHIP_GENERIC", Value=8)]
      [global::System.Runtime.Serialization.EnumMember(Value=@"ENTITY_RELATIONSHIP_GENERIC")]
      ENTITYRELATIONSHIPGENERIC = 8,
            
      [global::ProtoBuf.ProtoEnum(Name=@"ENTITY_RELATIONSHIP_TENNIS_PLAYER_A", Value=9)]
      [global::System.Runtime.Serialization.EnumMember(Value=@"ENTITY_RELATIONSHIP_TENNIS_PLAYER_A")]
      ENTITYRELATIONSHIPTENNISPLAYERA = 9,
            
      [global::ProtoBuf.ProtoEnum(Name=@"ENTITY_RELATIONSHIP_TENNIS_PLAYER_B", Value=10)]
      [global::System.Runtime.Serialization.EnumMember(Value=@"ENTITY_RELATIONSHIP_TENNIS_PLAYER_B")]
      ENTITYRELATIONSHIPTENNISPLAYERB = 10,
            
      [global::ProtoBuf.ProtoEnum(Name=@"ENTITY_RELATIONSHIP_HOME_TEAM", Value=11)]
      [global::System.Runtime.Serialization.EnumMember(Value=@"ENTITY_RELATIONSHIP_HOME_TEAM")]
      ENTITYRELATIONSHIPHOMETEAM = 11,
            
      [global::ProtoBuf.ProtoEnum(Name=@"ENTITY_RELATIONSHIP_AWAY_TEAM", Value=12)]
      [global::System.Runtime.Serialization.EnumMember(Value=@"ENTITY_RELATIONSHIP_AWAY_TEAM")]
      ENTITYRELATIONSHIPAWAYTEAM = 12
    }
  
    [global::ProtoBuf.ProtoContract(Name=@"InvalidRequestType")]
    [global::System.Runtime.Serialization.DataContract(Name=@"InvalidRequestType")]
    public enum InvalidRequestType
    {
            
      [global::ProtoBuf.ProtoEnum(Name=@"INVALID_REQUEST_DATE_OUT_OF_RANGE", Value=1)]
      [global::System.Runtime.Serialization.EnumMember(Value=@"INVALID_REQUEST_DATE_OUT_OF_RANGE")]
      INVALIDREQUESTDATEOUTOFRANGE = 1
    }
  
    [global::ProtoBuf.ProtoContract(Name=@"EventCategory")]
    [global::System.Runtime.Serialization.DataContract(Name=@"EventCategory")]
    public enum EventCategory
    {
            
      [global::ProtoBuf.ProtoEnum(Name=@"EVENT_CATEGORY_SPORT", Value=1)]
      [global::System.Runtime.Serialization.EnumMember(Value=@"EVENT_CATEGORY_SPORT")]
      EVENTCATEGORYSPORT = 1,
            
      [global::ProtoBuf.ProtoEnum(Name=@"EVENT_CATEGORY_POLITICS", Value=2)]
      [global::System.Runtime.Serialization.EnumMember(Value=@"EVENT_CATEGORY_POLITICS")]
      EVENTCATEGORYPOLITICS = 2,
            
      [global::ProtoBuf.ProtoEnum(Name=@"EVENT_CATEGORY_CURRENT_AFFAIRS", Value=3)]
      [global::System.Runtime.Serialization.EnumMember(Value=@"EVENT_CATEGORY_CURRENT_AFFAIRS")]
      EVENTCATEGORYCURRENTAFFAIRS = 3,
            
      [global::ProtoBuf.ProtoEnum(Name=@"EVENT_CATEGORY_TV_AND_ENTERTAINMENT", Value=4)]
      [global::System.Runtime.Serialization.EnumMember(Value=@"EVENT_CATEGORY_TV_AND_ENTERTAINMENT")]
      EVENTCATEGORYTVANDENTERTAINMENT = 4,
            
      [global::ProtoBuf.ProtoEnum(Name=@"EVENT_CATEGORY_GENERIC", Value=5)]
      [global::System.Runtime.Serialization.EnumMember(Value=@"EVENT_CATEGORY_GENERIC")]
      EVENTCATEGORYGENERIC = 5,
            
      [global::ProtoBuf.ProtoEnum(Name=@"EVENT_CATEGORY_FOOTBALL", Value=6)]
      [global::System.Runtime.Serialization.EnumMember(Value=@"EVENT_CATEGORY_FOOTBALL")]
      EVENTCATEGORYFOOTBALL = 6,
            
      [global::ProtoBuf.ProtoEnum(Name=@"EVENT_CATEGORY_TENNIS", Value=7)]
      [global::System.Runtime.Serialization.EnumMember(Value=@"EVENT_CATEGORY_TENNIS")]
      EVENTCATEGORYTENNIS = 7,
            
      [global::ProtoBuf.ProtoEnum(Name=@"EVENT_CATEGORY_HORSE_RACING", Value=8)]
      [global::System.Runtime.Serialization.EnumMember(Value=@"EVENT_CATEGORY_HORSE_RACING")]
      EVENTCATEGORYHORSERACING = 8,
            
      [global::ProtoBuf.ProtoEnum(Name=@"EVENT_CATEGORY_AMERICAN_FOOTBALL", Value=9)]
      [global::System.Runtime.Serialization.EnumMember(Value=@"EVENT_CATEGORY_AMERICAN_FOOTBALL")]
      EVENTCATEGORYAMERICANFOOTBALL = 9,
            
      [global::ProtoBuf.ProtoEnum(Name=@"EVENT_CATEGORY_RUGBY", Value=10)]
      [global::System.Runtime.Serialization.EnumMember(Value=@"EVENT_CATEGORY_RUGBY")]
      EVENTCATEGORYRUGBY = 10
    }
  
    [global::ProtoBuf.ProtoContract(Name=@"OrderStatus")]
    [global::System.Runtime.Serialization.DataContract(Name=@"OrderStatus")]
    public enum OrderStatus
    {
            
      [global::ProtoBuf.ProtoEnum(Name=@"ORDER_STATUS_LIVE", Value=1)]
      [global::System.Runtime.Serialization.EnumMember(Value=@"ORDER_STATUS_LIVE")]
      ORDERSTATUSLIVE = 1,
            
      [global::ProtoBuf.ProtoEnum(Name=@"ORDER_STATUS_PARTIALLY_FILLED", Value=2)]
      [global::System.Runtime.Serialization.EnumMember(Value=@"ORDER_STATUS_PARTIALLY_FILLED")]
      ORDERSTATUSPARTIALLYFILLED = 2,
            
      [global::ProtoBuf.ProtoEnum(Name=@"ORDER_STATUS_FILLED", Value=3)]
      [global::System.Runtime.Serialization.EnumMember(Value=@"ORDER_STATUS_FILLED")]
      ORDERSTATUSFILLED = 3,
            
      [global::ProtoBuf.ProtoEnum(Name=@"ORDER_STATUS_PARTIALLY_CANCELLED", Value=4)]
      [global::System.Runtime.Serialization.EnumMember(Value=@"ORDER_STATUS_PARTIALLY_CANCELLED")]
      ORDERSTATUSPARTIALLYCANCELLED = 4,
            
      [global::ProtoBuf.ProtoEnum(Name=@"ORDER_STATUS_CANCELLED", Value=5)]
      [global::System.Runtime.Serialization.EnumMember(Value=@"ORDER_STATUS_CANCELLED")]
      ORDERSTATUSCANCELLED = 5
    }
  
    [global::ProtoBuf.ProtoContract(Name=@"Currency")]
    [global::System.Runtime.Serialization.DataContract(Name=@"Currency")]
    public enum Currency
    {
            
      [global::ProtoBuf.ProtoEnum(Name=@"CURRENCY_GBP", Value=1)]
      [global::System.Runtime.Serialization.EnumMember(Value=@"CURRENCY_GBP")]
      CURRENCYGBP = 1,
            
      [global::ProtoBuf.ProtoEnum(Name=@"CURRENCY_EUR", Value=2)]
      [global::System.Runtime.Serialization.EnumMember(Value=@"CURRENCY_EUR")]
      CURRENCYEUR = 2
    }
  
}
