using System;

namespace ExplorerHub.Framework.Domain
{
    public abstract class Entity
    {
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