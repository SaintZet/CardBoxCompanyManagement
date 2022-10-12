using CardBox.ApiClient.Models;
using CardBox.MPortalDataBaseClient.Models;

namespace CardBox.CompanyManagement.Wrapper
{
    internal class CompanyWrapper
    {
        public CompanyWrapper(Company company, CompanyLicense companyLicense)
        {
            Company = company;
            CompanyLicense = companyLicense;
        }

        public Company Company { get; private set; }
        public CompanyLicense CompanyLicense { get; private set; }
    }
}