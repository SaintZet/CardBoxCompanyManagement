using CardBox.ApiClient.Contracts;
using CardBox.ApiClient.Models;
using CardBox.CompanyManagement.Wrapper;
using CardBox.MPortalDataBaseClient.Contracts;
using System.Collections.Generic;
using System.Linq;

namespace CardBox.CompanyManagement.DataProvider
{
    internal class CompanyDataProvider : ICompanyDataProvider
    {
        private readonly ICompaniesService _companies;
        private readonly ICompanyLicensesService _companyLicenses;

        public CompanyDataProvider(ICompaniesService companies, ICompanyLicensesService companiesLicense)
        {
            _companies = companies;
            _companyLicenses = companiesLicense;
        }

        public void Delete(Company company) => _companies.Delete(company);

        public List<CompanyWrapper> GetCompanyWrappers()
        {
            var result = new List<CompanyWrapper>();

            var companies = _companies.GetCompaniesAsync().Result;

            var companiesID = companies.Select(c => c.ID).ToList();

            var companyLicenses = _companyLicenses.GetCompaniesLicense(companiesID);

            result.AddRange(companies.Select(company => new CompanyWrapper(company, companyLicenses[company.ID])));

            return result;
        }

        public void Post(Company company) => _companies.Post(company);

        public void Put(Company company) => _companies.Put(company);
    }
}