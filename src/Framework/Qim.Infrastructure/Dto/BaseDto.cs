using System.Collections.Generic;
using System.Reflection;

namespace Qim.Dto
{
    public abstract class BaseDto<TPkey> : IDto<TPkey>
    {
        public virtual TPkey PId { get; set; }

        private static bool IsTransient(BaseDto<TPkey> dto)
        {
            if (EqualityComparer<TPkey>.Default.Equals(dto.PId, default(TPkey)))
            {
                return true;
            }
            return false;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is BaseDto<TPkey>))
            {
                return false;
            }

            //Same instances must be considered as equal
            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            //Transient objects are not considered as equal
            var other = (BaseDto<TPkey>) obj;
            if (IsTransient(this) || IsTransient(other))
            {
                return false;
            }

            //Must have a IS-A relation of types or must be same type
            var typeOfThis = GetType();
            var typeOfOther = other.GetType();
            if (!typeOfThis.IsAssignableFrom(typeOfOther) && !typeOfOther.IsAssignableFrom(typeOfThis))
            {
                return false;
            }

            return PId.Equals(other.PId);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            return Equals(PId, default(TPkey)) ? base.GetHashCode() : PId.GetHashCode();
        }

        /// <inheritdoc />
        public static bool operator ==(BaseDto<TPkey> left, BaseDto<TPkey> right)
        {
            if (Equals(left, null))
            {
                return Equals(right, null);
            }

            return left.Equals(right);
        }

        /// <inheritdoc />
        public static bool operator !=(BaseDto<TPkey> left, BaseDto<TPkey> right)
        {
            return !(left == right);
        }
    }


    public abstract class BaseDto : BaseDto<string>
    {
        
    }
}