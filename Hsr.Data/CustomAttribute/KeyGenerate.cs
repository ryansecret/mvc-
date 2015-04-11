using System;

namespace Hsr.Data.CustomAttribute
{
    public  class KeyGenerate:Attribute
    {
        public KeyKind KeyKind { get; set; }

        public string SeqenceName { get; set; }
 
    }

    public enum KeyKind
    {
        Id,Guid
    }
}
