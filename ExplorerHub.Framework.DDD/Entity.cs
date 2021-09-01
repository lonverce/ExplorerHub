using System;

namespace ExplorerHub.Framework.Domain
{
    /// <summary>
    /// DDD概念中的领域实体
    /// </summary>
    public abstract class Entity
    {
        /// <summary>
        /// 获取领域实体Id
        /// </summary>
        public Guid Id { get; protected set; } = Guid.Empty;

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj is Entity entity)
            {
                return Equals(entity);
            }

            return false;
        }

        public bool Equals(Entity entity)
        {
            if (entity == null)
            {
                return false;
            }

            if (entity.GetType() != GetType())
            {
                return false;
            }

            return Id == entity.Id;
        }
    }
}