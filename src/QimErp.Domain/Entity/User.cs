using System;
using Qim;
using Qim.Domain.Entity;
using Qim.Security;
using QimErp.Infrastructure.DomainModel;

namespace QimErp.Domain.Entity
{
    public class User : BaseUser, ISoftDelete, ICreationAndModificationLog<User>
    {
        private const int SALTSIZE = 16;

        /// <summary>
        ///     密码(加密)
        /// </summary>
        public virtual string Password { get; protected set; }

        /// <summary>
        ///     密码加盐值
        /// </summary>
        public virtual string Salt { get; protected set; }

        /// <summary>
        ///     手机号码
        /// </summary>
        public virtual string CellPhone { get; set; }

        /// <summary>
        ///     描述
        /// </summary>
        public virtual string Description { get; set; }


        /// <summary>
        ///     是否删除
        /// </summary>
        public bool IsDeleted { get; set; }

        #region 导航属性

        /// <summary>
        ///     创建者
        /// </summary>
        public User CreatorUser { get; set; }

        /// <summary>
        ///     最后修改人员
        /// </summary>
        public User LastModifierUser { get; set; }

        #endregion

        #region Method

        /// <summary>
        ///     重设用户密码
        /// </summary>
        /// <param name="password">要设置的新密码</param>
        public void ResetPassword(string password)
        {
            Salt = EncryptHelper.CreateSaltKey(SALTSIZE);
            Password = EncryptHelper.Hash(password + Salt, HashType.Sha256);
        }

        /// <summary>
        ///     检验密码是否与用户的密码相符
        /// </summary>
        /// <param name="password">待检验的密码</param>
        /// <returns>相符返回:true，否则返回:false</returns>
        private bool ValidPassword(string password)
        {
            return !string.IsNullOrEmpty(password) &&
                   Password.Equals(EncryptHelper.Hash(password + Salt, HashType.Sha256));
        }

        /// <summary>
        ///     更改用户的密码
        /// </summary>
        /// <param name="oldPassword">旧密码</param>
        /// <param name="newPassword">要设置的新密码</param>
        /// <returns>更改成功返回:true,返回:false,旧密码不正确</returns>
        public bool ChangePassword(string oldPassword, string newPassword)
        {
            if (!ValidPassword(oldPassword)) return false;
            ResetPassword(newPassword);
            return true;
        }

        /// <summary>
        ///     用户登录
        /// </summary>
        /// <param name="password">用户的密码</param>
        /// <param name="output"></param>
        public void UserLogin(string password, LoginOutput output)
        {
            Ensure.NotNull(output, nameof(output));
            if (!ValidPassword(password))
            {
                output.LoginResult = LoginResult.InvalidPassword;
                return;
            }
            if (!IsActive)
            {
                output.LoginResult = LoginResult.UserIsNotActive;
                return;
            }


            LastLoginTime = DateTime.Now;
            LoginCount++;
            output.LoginResult = LoginResult.Success;
            output.UserAccount = UserAccount;
            output.UserName = UserName;
        }

        public static User Create(string userAccount, string userName, string password,
            string cellPhone = "", string email = "", string description = "")
        {
            var user = new User
            {
                UserAccount = userAccount,
                UserName = userName,
                Email = email,
                CellPhone = cellPhone,
                Description = description
            };
            user.ResetPassword(password);
            return user;
        }


        public static User Register(string cellPhone, string userName)
        {
            Ensure.NotNullOrEmpty(cellPhone, nameof(cellPhone));
            if (cellPhone.Length < 11)
            {
                throw new AppException($"手机号码：{cellPhone}不正确！");
            }
            //取手机号码后4位做密码
            var password = cellPhone.Substring(cellPhone.Length - 4, 4);
            var user = Create(cellPhone, userName, password, cellPhone);
            user.CreateBy = user.PId;
            return user;
        }

        #endregion
    }
}