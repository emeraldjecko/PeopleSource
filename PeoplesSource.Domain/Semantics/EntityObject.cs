namespace PeoplesSource.Domain
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class EntityObject
    {
        /// <summary>
        /// Unique identifier for all instances of the most derived type.
        /// A >0 value indicates an Entity that is stored.
        /// A 0 value indicates an Entity that is not stored.
        /// </summary>
        public virtual int Id { get; protected internal set; }
        // TODO: Investigate whether this actually needs to be virtual for NHibernate?


        public override bool Equals(object obj)
        {
            var other = obj as EntityObject;
            if ((object)other == null || GetType() != other.GetType()) return false;

            return Id > 0 && other.Id > 0 && Id == other.Id;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool operator ==(EntityObject a, EntityObject b)
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
        public static bool operator !=(EntityObject a, EntityObject b)
        {
            return !(a == b);
        }
    }
}
