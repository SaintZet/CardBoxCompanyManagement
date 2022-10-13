using CardBox.MPortalDataBaseClient.Models;

namespace CardBox.MPortalDataBaseClient.Contracts
{
    public interface ICompanyLicensesService
    {
        Dictionary<string, List<CompanyLicense>> GetCompaniesLicense(List<string> companiesID);
    }
}