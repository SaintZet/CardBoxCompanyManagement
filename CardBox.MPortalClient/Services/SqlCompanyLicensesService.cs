using CardBox.MPortalDataBaseClient.Constants;
using CardBox.MPortalDataBaseClient.Contracts;
using CardBox.MPortalDataBaseClient.Models;
using System.Configuration;
using System.Data.SqlClient;

namespace CardBox.MPortalDataBaseClient.Services;

public class SqlCompanyLicensesService : ICompanyLicensesService
{
    public Dictionary<string, List<CompanyLicense>> GetCompaniesLicense(List<string> companiesID)
    {
        Dictionary<string, List<CompanyLicense>> result = new();

        using (var mportalDbCon = new SqlConnection(ConfigurationManager.ConnectionStrings["MPortalDB"].ConnectionString))
        {
            using (var donglesDbCon = new SqlConnection(ConfigurationManager.ConnectionStrings["DonglesDB"].ConnectionString))
            {
                mportalDbCon.Open();
                donglesDbCon.Open();

                using (var mportalDbCom = new SqlCommand(Queries.PMportal, mportalDbCon))
                {
                    using var donglesDbCom = new SqlCommand(Queries.Dongles, donglesDbCon);

                    foreach (var id in companiesID)
                    {
                        List<CompanyLicense> companyLicenses = new();
                        string userUniqueNumber;
                        string serialNumber;

                        mportalDbCom.Parameters.AddWithValue("@TaxId", id);

                        using var reader = mportalDbCom.ExecuteReader();
                        {
                            mportalDbCom.Parameters.Clear();

                            while (reader.Read())
                            {
                                userUniqueNumber = reader["UserUniqueNumber"].ToString()!;
                                serialNumber = reader["SerialNumber"].ToString()!;

                                if (string.IsNullOrEmpty(serialNumber))
                                {
                                    break;
                                }

                                donglesDbCom.Parameters.AddWithValue("@DongleID", serialNumber);
                                object dateTime = donglesDbCom.ExecuteScalar();
                                donglesDbCom.Parameters.Clear();

                                companyLicenses.Add(new()
                                {
                                    UserUniqueNumber = userUniqueNumber,
                                    SerialNumber = serialNumber,
                                    Date = dateTime == null ? string.Empty : ((DateTime)dateTime).ToString("dd.MM.yyyy"),
                                });
                            }
                        }

                        result.Add(id, companyLicenses);
                    }
                }
            }
        }

        return result;
    }
}