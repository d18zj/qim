using Qim.AspNetCore.Mvc.FluentMetadata;
using QimErp.ServiceContracts.Dto;

namespace QimErp.Web.Model
{
    public class RegisterUserInputConfig: ModelMetadataConfiguration<RegisterUserInput>
    {
        public RegisterUserInputConfig()
        {
            Configure(a => a.UserName).Required().DisplayName("联系人");
            Configure(a => a.CellPhone).Required().DisplayName("手机号码");
            Configure(a => a.TenantName).DisplayName("公司名称");
            Configure<string>("TenantName").Required();
        }
    }
}