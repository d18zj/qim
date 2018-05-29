using System;

namespace Qim.Domain.Repositories
{
    public class EntityNotExistsException : AppException
    {
        public EntityNotExistsException(string formatMsg, params object[] args)
            : this(string.Format(formatMsg, args))
        {
        }

        /// <summary>
        /// </summary>
        /// <param name="message">错误信息</param>
        public EntityNotExistsException(string message)
            : base(message)
        {
        }

        public EntityNotExistsException(Type entityType, object id)
            : this(
                "There is no such an entity with given primary key. Entity type:{0},PId:{1}.",
                entityType.FullName, id)
        {

        }


        public EntityNotExistsException()
            : base("The entity does not exist, may have been deleted！")
        {
        }
    }
}