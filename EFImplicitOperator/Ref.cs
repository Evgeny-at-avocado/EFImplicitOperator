using System;
using System.Collections.Generic;
using System.Text;

namespace EFImplicitOperator
{
    public class Ref
    {
        Guid _id;


        public Ref(Guid id)
        {
            _id = id;
        }

        public static implicit operator Guid? (Ref r) 
            => r == null || r._id == Guid.Empty 
                ? null 
                : (Guid?)r._id;

        public static implicit operator Guid(Ref r) 
            => r == null 
                ? Guid.Empty 
                : r._id;
    }
}
