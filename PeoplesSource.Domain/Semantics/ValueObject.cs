using System.Linq;

namespace PeoplesSource.Domain
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class ValueObject
    {
        public override bool Equals(object obj)
        {
            var other = obj as ValueObject;
            if ((object)other == null || GetType() != other.GetType()) return false;

            return GetHashCode() != 0 && GetHashCode() == other.GetHashCode();
        }

        /// <summary>
        /// A ValueObject's state is comprised of all public properties. GetHashCode returns a representation
        /// that is a concatenation of all of these members.
        /// </summary>
        public override int GetHashCode()
        {
            int result = 0;
            foreach (var prop in GetType().GetProperties().Where(p => p.PropertyType.IsPublic))
                if (prop.GetValue(this, null) != null)
                    result ^= prop.GetValue(this, null).GetHashCode() * 13;
            return result;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool operator ==(ValueObject a, ValueObject b)
        {
            // If both are null, or both are same instance, return true.
            if (ReferenceEquals(a, b))
                return true;

            // If one is null, but not both, return false.
            if (((object)a == null) || ((object)b == null))
                return false;

            return a.Equals(b);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool operator !=(ValueObject a, ValueObject b)
        {
            return !(a == b);
        }
    }
}
