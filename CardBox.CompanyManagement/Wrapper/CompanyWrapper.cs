using CardBox.ApiClient.Models;
using CardBox.MPortalDataBaseClient.Models;
using System.Collections.Generic;
using System.Linq;

namespace CardBox.CompanyManagement.Wrapper
{
    internal class CompanyWrapper
    {
        public CompanyWrapper(Company company, List<CompanyLicense> companyLicense)
        {
            Company = company;

            CompanyLicense = new CompanyLicense
            {
                Date = string.Join("\n", companyLicense.Select(item => item.Date)),
                SerialNumber = string.Join("\n", companyLicense.Select(item => item.SerialNumber)),
                UserUniqueNumber = string.Join("\n", companyLicense.Select(item => item.UserUniqueNumber))
            };
        }

        public Company Company { get; }
        public CompanyLicense CompanyLicense { get; }
    }
}