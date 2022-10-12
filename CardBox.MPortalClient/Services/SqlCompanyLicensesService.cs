using CardBox.MPortalDataBaseClient.Contracts;
using CardBox.MPortalDataBaseClient.Models;
using System.Data.SqlClient;

namespace CardBox.MPortalDataBaseClient.Services;

public class SqlCompanyLicensesService : ICompanyLicensesService
{
    public Dictionary<string, CompanyLicense> GetCompaniesLicense(List<string> companiesID)
    {
        Dictionary<string, CompanyLicense> result = new();

        string mportalDbQuery = @"select SerialNumber, UserUniqueNumber from [dbo].Dongles d LEFT JOIN[Identity].[Users] u ON d.UserUniqueNumber = u.UniqueNumber WHERE u.TaxId = @TaxId";

        string donglesDbQuery = @"SELECT [ExpirationDate] FROM [Dongles].[dbo].[ProductsExpiration] WHERE ProductID = 6 AND DongleID = @DongleID";

        using (var mportalDbCon = new SqlConnection("Server= User.microinvest.net; Database=Mportal.DB; User id=dongles; password=Micr0!nvest_6380;"))
        {
            mportalDbCon.Open();

            using (var donglesDbCon = new SqlConnection("Server= 192.168.0.248; Database=Dongles; User id=dongles; password=Micr0!nvest_6380;"))
            {
                donglesDbCon.Open();

                using (var mportalDbCom = new SqlCommand(mportalDbQuery, mportalDbCon))
                {
                    using var donglesDbCom = new SqlCommand(donglesDbQuery, donglesDbCon);

                    foreach (var id in companiesID)
                    {
                        string userUniqueNumber = string.Empty;
                        string serialNumber = string.Empty;
                        object? dateTime = null;

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

                                dateTime = donglesDbCom.ExecuteScalar();

                                donglesDbCom.Parameters.Clear();
                            }
                        }

                        result.Add(id, new()
                        {
                            UserUniqueNumber = userUniqueNumber,
                            SerialNumber = serialNumber,
                            DateTime = dateTime != null ? (DateTime)dateTime : null,
                        });
                    }
                }

                donglesDbCon.Close();
            }
            mportalDbCon.Close();
        }

        return result;
    }
}