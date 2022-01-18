using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EasyNetworking.Messages
{
    public class MessageWithParameters : MessageBase
    {
        private Type[] _parameterTypes;
        
        public MessageWithParameters(Type[] parameterTypes)
        {
            _parameterTypes = parameterTypes;
        }


    }
}

