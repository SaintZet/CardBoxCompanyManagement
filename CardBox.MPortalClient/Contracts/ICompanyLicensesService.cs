using CardBox.MPortalDataBaseClient.Models;

namespace CardBox.MPortalDataBaseClient.Contracts
{
    public interface ICompanyLicensesService
    {
        Dictionary<string, CompanyLicense> GetCompaniesLicense(List<string> companiesID);
    }
}