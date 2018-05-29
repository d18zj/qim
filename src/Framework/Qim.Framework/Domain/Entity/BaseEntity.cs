using System;
using System.Collections.Generic;
using System.Reflection;

namespace Qim.Domain.Entity
{
    public abstract class BaseEntity<TPkey> : IEntity<TPkey>
    {
        public virtual TPkey PId { get; set; }

        /// <summary>
        /// Checks if this entity is transient (it has not an Id).
        /// </summary>
        /// <returns>True, if this entity is transient</returns>
        public virtual bool IsTransient()
        {
           
            if (EqualityComparer<TPkey>.Default.Equals(PId, default(TPkey)))
            {
                return true;
            }

            //Workaround for EF Core since it sets int/long to min value when attaching to dbcontext
            if (typeof(TPkey) == typeof(int))
            {
                return Convert.ToInt32(PId) <= 0;
            }

            if (typeof(TPkey) == typeof(long))
            {
                return Convert.ToInt64(PId) <= 0;
            }

            return false;
        }

        /// <inheritdoc/>
        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is BaseEntity<TPkey>))
            {
                return false;
            }

            //Same instances must be considered as equal
            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            //Transient objects are not considered as equal
            var other = (BaseEntity<TPkey>) obj;
            if (IsTransient() || other.IsTransient())
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


            if (this is IMustHaveTenant && other is IMustHaveTenant &&
                ((IMustHaveTenant)this).TenantId != ((IMustHaveTenant)other).TenantId)
            {
                return false;
            }

            return PId.Equals(other.PId);
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            return IsTransient() ? base.GetHashCode() : PId.GetHashCode();
        }

        /// <inheritdoc/>
        public static bool operator ==(BaseEntity<TPkey> left, BaseEntity<TPkey> right)
        {
            if (Equals(left, null))
            {
                return Equals(right, null);
            }

            return left.Equals(right);
        }

        /// <inheritdoc/>
        public static bool operator !=(BaseEntity<TPkey> left, BaseEntity<TPkey> right)
        {
            return !(left == right);
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            return $"[{GetType().Name} {PId}]";
        }
    }

    public abstract class BaseEntity : BaseEntity<string>, IEntity
    {

    }
}