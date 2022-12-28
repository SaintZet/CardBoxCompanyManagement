using CardBox.ApiClient.Models;
using CardBox.CompanyManagement.Wrapper;
using System.Collections.Generic;

namespace CardBox.CompanyManagement.DataProvider
{
    internal interface ICompanyDataProvider
    {
        List<CompanyWrapper> GetCompanyWrappers();

        void Post(Company company);

        void Delete(Company company);

        void Put(Company company);
    }
}