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

namespace IronSmarkets.Proto.Eto
{
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"Payload")]
  [global::System.Runtime.Serialization.DataContract(Name=@"Payload")]
  public partial class Payload : global::ProtoBuf.IExtensible, global::System.Runtime.Serialization.ISerializable
  {
    public Payload() {}
    
    private ulong _seq;
    [global::ProtoBuf.ProtoMember(1, IsRequired = true, Name=@"seq", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.Runtime.Serialization.DataMember(Name=@"seq", Order = 1, IsRequired = true)]
    
    public ulong Seq
    {
      get { return _seq; }
      set { _seq = value; }
    }

    private IronSmarkets.Proto.Eto.PayloadType _type = IronSmarkets.Proto.Eto.PayloadType.PAYLOADNONE;
    [global::ProtoBuf.ProtoMember(2, IsRequired = false, Name=@"type", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.ComponentModel.DefaultValue(IronSmarkets.Proto.Eto.PayloadType.PAYLOADNONE)]
    [global::System.Runtime.Serialization.DataMember(Name=@"type", Order = 2, IsRequired = false)]
    
    public IronSmarkets.Proto.Eto.PayloadType Type
    {
      get { return _type; }
      set { _type = value; }
    }

    private bool _isReplay = (bool)false;
    [global::ProtoBuf.ProtoMember(3, IsRequired = false, Name=@"is_replay", DataFormat = global::ProtoBuf.DataFormat.Default)]
    [global::System.ComponentModel.DefaultValue((bool)false)]
    [global::System.Runtime.Serialization.DataMember(Name=@"is_replay", Order = 3, IsRequired = false)]
    
    public bool IsReplay
    {
      get { return _isReplay; }
      set { _isReplay = value; }
    }

    private IronSmarkets.Proto.Eto.Replay _replay = null;
    [global::ProtoBuf.ProtoMember(4, IsRequired = false, Name=@"replay", DataFormat = global::ProtoBuf.DataFormat.Default)]
    [global::System.ComponentModel.DefaultValue(null)]
    [global::System.Runtime.Serialization.DataMember(Name=@"replay", Order = 4, IsRequired = false)]
    
    public IronSmarkets.Proto.Eto.Replay Replay
    {
      get { return _replay; }
      set { _replay = value; }
    }

    private IronSmarkets.Proto.Eto.Login _login = null;
    [global::ProtoBuf.ProtoMember(5, IsRequired = false, Name=@"login", DataFormat = global::ProtoBuf.DataFormat.Default)]
    [global::System.ComponentModel.DefaultValue(null)]
    [global::System.Runtime.Serialization.DataMember(Name=@"login", Order = 5, IsRequired = false)]
    
    public IronSmarkets.Proto.Eto.Login Login
    {
      get { return _login; }
      set { _login = value; }
    }

    private IronSmarkets.Proto.Eto.LoginResponse _loginResponse = null;
    [global::ProtoBuf.ProtoMember(6, IsRequired = false, Name=@"login_response", DataFormat = global::ProtoBuf.DataFormat.Default)]
    [global::System.ComponentModel.DefaultValue(null)]
    [global::System.Runtime.Serialization.DataMember(Name=@"login_response", Order = 6, IsRequired = false)]
    
    public IronSmarkets.Proto.Eto.LoginResponse LoginResponse
    {
      get { return _loginResponse; }
      set { _loginResponse = value; }
    }

    private IronSmarkets.Proto.Eto.Logout _logout = null;
    [global::ProtoBuf.ProtoMember(7, IsRequired = false, Name=@"logout", DataFormat = global::ProtoBuf.DataFormat.Default)]
    [global::System.ComponentModel.DefaultValue(null)]
    [global::System.Runtime.Serialization.DataMember(Name=@"logout", Order = 7, IsRequired = false)]
    
    public IronSmarkets.Proto.Eto.Logout Logout
    {
      get { return _logout; }
      set { _logout = value; }
    }
    protected Payload(global::System.Runtime.Serialization.SerializationInfo info, global::System.Runtime.Serialization.StreamingContext context)
      : this() { global::ProtoBuf.Serializer.Merge(info, this); }
    void global::System.Runtime.Serialization.ISerializable.GetObjectData(global::System.Runtime.Serialization.SerializationInfo info, global::System.Runtime.Serialization.StreamingContext context)
      { global::ProtoBuf.Serializer.Serialize(info, this); }
    
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"Replay")]
  [global::System.Runtime.Serialization.DataContract(Name=@"Replay")]
  public partial class Replay : global::ProtoBuf.IExtensible, global::System.Runtime.Serialization.ISerializable
  {
    public Replay() {}
    
    private ulong _seq;
    [global::ProtoBuf.ProtoMember(1, IsRequired = true, Name=@"seq", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.Runtime.Serialization.DataMember(Name=@"seq", Order = 1, IsRequired = true)]
    
    public ulong Seq
    {
      get { return _seq; }
      set { _seq = value; }
    }
    protected Replay(global::System.Runtime.Serialization.SerializationInfo info, global::System.Runtime.Serialization.StreamingContext context)
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
    
    private string _session;
    [global::ProtoBuf.ProtoMember(1, IsRequired = true, Name=@"session", DataFormat = global::ProtoBuf.DataFormat.Default)]
    [global::System.Runtime.Serialization.DataMember(Name=@"session", Order = 1, IsRequired = true)]
    
    public string Session
    {
      get { return _session; }
      set { _session = value; }
    }
    protected Login(global::System.Runtime.Serialization.SerializationInfo info, global::System.Runtime.Serialization.StreamingContext context)
      : this() { global::ProtoBuf.Serializer.Merge(info, this); }
    void global::System.Runtime.Serialization.ISerializable.GetObjectData(global::System.Runtime.Serialization.SerializationInfo info, global::System.Runtime.Serialization.StreamingContext context)
      { global::ProtoBuf.Serializer.Serialize(info, this); }
    
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"LoginResponse")]
  [global::System.Runtime.Serialization.DataContract(Name=@"LoginResponse")]
  public partial class LoginResponse : global::ProtoBuf.IExtensible, global::System.Runtime.Serialization.ISerializable
  {
    public LoginResponse() {}
    
    private string _session;
    [global::ProtoBuf.ProtoMember(1, IsRequired = true, Name=@"session", DataFormat = global::ProtoBuf.DataFormat.Default)]
    [global::System.Runtime.Serialization.DataMember(Name=@"session", Order = 1, IsRequired = true)]
    
    public string Session
    {
      get { return _session; }
      set { _session = value; }
    }

    private ulong _reset = default(ulong);
    [global::ProtoBuf.ProtoMember(2, IsRequired = false, Name=@"reset", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.ComponentModel.DefaultValue(default(ulong))]
    [global::System.Runtime.Serialization.DataMember(Name=@"reset", Order = 2, IsRequired = false)]
    
    public ulong Reset
    {
      get { return _reset; }
      set { _reset = value; }
    }
    protected LoginResponse(global::System.Runtime.Serialization.SerializationInfo info, global::System.Runtime.Serialization.StreamingContext context)
      : this() { global::ProtoBuf.Serializer.Merge(info, this); }
    void global::System.Runtime.Serialization.ISerializable.GetObjectData(global::System.Runtime.Serialization.SerializationInfo info, global::System.Runtime.Serialization.StreamingContext context)
      { global::ProtoBuf.Serializer.Serialize(info, this); }
    
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"Logout")]
  [global::System.Runtime.Serialization.DataContract(Name=@"Logout")]
  public partial class Logout : global::ProtoBuf.IExtensible, global::System.Runtime.Serialization.ISerializable
  {
    public Logout() {}
    

    private IronSmarkets.Proto.Eto.LogoutReason _reason = IronSmarkets.Proto.Eto.LogoutReason.LOGOUTNONE;
    [global::ProtoBuf.ProtoMember(1, IsRequired = false, Name=@"reason", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.ComponentModel.DefaultValue(IronSmarkets.Proto.Eto.LogoutReason.LOGOUTNONE)]
    [global::System.Runtime.Serialization.DataMember(Name=@"reason", Order = 1, IsRequired = false)]
    
    public IronSmarkets.Proto.Eto.LogoutReason Reason
    {
      get { return _reason; }
      set { _reason = value; }
    }
    protected Logout(global::System.Runtime.Serialization.SerializationInfo info, global::System.Runtime.Serialization.StreamingContext context)
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
            
      [global::ProtoBuf.ProtoEnum(Name=@"PAYLOAD_NONE", Value=1)]
      [global::System.Runtime.Serialization.EnumMember(Value=@"PAYLOAD_NONE")]
      PAYLOADNONE = 1,
            
      [global::ProtoBuf.ProtoEnum(Name=@"PAYLOAD_PING", Value=2)]
      [global::System.Runtime.Serialization.EnumMember(Value=@"PAYLOAD_PING")]
      PAYLOADPING = 2,
            
      [global::ProtoBuf.ProtoEnum(Name=@"PAYLOAD_PONG", Value=3)]
      [global::System.Runtime.Serialization.EnumMember(Value=@"PAYLOAD_PONG")]
      PAYLOADPONG = 3,
            
      [global::ProtoBuf.ProtoEnum(Name=@"PAYLOAD_GAPFILL", Value=4)]
      [global::System.Runtime.Serialization.EnumMember(Value=@"PAYLOAD_GAPFILL")]
      PAYLOADGAPFILL = 4,
            
      [global::ProtoBuf.ProtoEnum(Name=@"PAYLOAD_HEARTBEAT", Value=5)]
      [global::System.Runtime.Serialization.EnumMember(Value=@"PAYLOAD_HEARTBEAT")]
      PAYLOADHEARTBEAT = 5,
            
      [global::ProtoBuf.ProtoEnum(Name=@"PAYLOAD_REPLAY", Value=6)]
      [global::System.Runtime.Serialization.EnumMember(Value=@"PAYLOAD_REPLAY")]
      PAYLOADREPLAY = 6,
            
      [global::ProtoBuf.ProtoEnum(Name=@"PAYLOAD_LOGIN", Value=7)]
      [global::System.Runtime.Serialization.EnumMember(Value=@"PAYLOAD_LOGIN")]
      PAYLOADLOGIN = 7,
            
      [global::ProtoBuf.ProtoEnum(Name=@"PAYLOAD_LOGIN_RESPONSE", Value=8)]
      [global::System.Runtime.Serialization.EnumMember(Value=@"PAYLOAD_LOGIN_RESPONSE")]
      PAYLOADLOGINRESPONSE = 8,
            
      [global::ProtoBuf.ProtoEnum(Name=@"PAYLOAD_LOGOUT", Value=9)]
      [global::System.Runtime.Serialization.EnumMember(Value=@"PAYLOAD_LOGOUT")]
      PAYLOADLOGOUT = 9
    }
  
    [global::ProtoBuf.ProtoContract(Name=@"LogoutReason")]
    [global::System.Runtime.Serialization.DataContract(Name=@"LogoutReason")]
    public enum LogoutReason
    {
            
      [global::ProtoBuf.ProtoEnum(Name=@"LOGOUT_NONE", Value=1)]
      [global::System.Runtime.Serialization.EnumMember(Value=@"LOGOUT_NONE")]
      LOGOUTNONE = 1,
            
      [global::ProtoBuf.ProtoEnum(Name=@"LOGOUT_HEARTBEAT_TIMEOUT", Value=2)]
      [global::System.Runtime.Serialization.EnumMember(Value=@"LOGOUT_HEARTBEAT_TIMEOUT")]
      LOGOUTHEARTBEATTIMEOUT = 2,
            
      [global::ProtoBuf.ProtoEnum(Name=@"LOGOUT_CONFIRMATION", Value=3)]
      [global::System.Runtime.Serialization.EnumMember(Value=@"LOGOUT_CONFIRMATION")]
      LOGOUTCONFIRMATION = 3,
            
      [global::ProtoBuf.ProtoEnum(Name=@"LOGOUT_LOGIN_TIMEOUT", Value=4)]
      [global::System.Runtime.Serialization.EnumMember(Value=@"LOGOUT_LOGIN_TIMEOUT")]
      LOGOUTLOGINTIMEOUT = 4,
            
      [global::ProtoBuf.ProtoEnum(Name=@"LOGOUT_LOGIN_NOT_FIRST_SEQ", Value=5)]
      [global::System.Runtime.Serialization.EnumMember(Value=@"LOGOUT_LOGIN_NOT_FIRST_SEQ")]
      LOGOUTLOGINNOTFIRSTSEQ = 5,
            
      [global::ProtoBuf.ProtoEnum(Name=@"LOGOUT_UNKNOWN_SESSION", Value=6)]
      [global::System.Runtime.Serialization.EnumMember(Value=@"LOGOUT_UNKNOWN_SESSION")]
      LOGOUTUNKNOWNSESSION = 6,
            
      [global::ProtoBuf.ProtoEnum(Name=@"LOGOUT_UNAUTHORISED", Value=7)]
      [global::System.Runtime.Serialization.EnumMember(Value=@"LOGOUT_UNAUTHORISED")]
      LOGOUTUNAUTHORISED = 7,
            
      [global::ProtoBuf.ProtoEnum(Name=@"LOGOUT_SERVICE_TEMPORARILY_UNAVAILABLE", Value=8)]
      [global::System.Runtime.Serialization.EnumMember(Value=@"LOGOUT_SERVICE_TEMPORARILY_UNAVAILABLE")]
      LOGOUTSERVICETEMPORARILYUNAVAILABLE = 8
    }
  
}
