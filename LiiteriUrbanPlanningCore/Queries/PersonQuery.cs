using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

using System.Configuration;

namespace LiiteriUrbanPlanningCore.Queries
{
    public class PersonQuery : SqlQuery, ISqlQuery
    {
        private List<string> whereListContacts;
        private List<string> whereListConsults;

        public int QueryType { get; set; }

        #region expressions

        // Ely
        public int? ElyIs
        {
            get
            {
                return (int) this.GetParameter("@ElyIs");
            }
            set
            {
                if (value == null) return;
                this.whereListContacts.Add("K1.YmpVastuuEly_Id = @ElyIs");
                this.whereListConsults.Add("K2.YmpVastuuEly_Id = @ElyIs");
                this.AddParameter("@ElyIs", (int) value);
            }
        }

        // Seutukunta
        public int? SubRegionIs
        {
            get
            {
                return (int) this.GetParameter("@SubRegionIs");
            }
            set
            {
                if (value == null) return;
                this.whereListContacts.Add("K1.Seutukunta_Id = @SubRegionIs");
                this.whereListConsults.Add("K2.Seutukunta_Id = @SubRegionIs");
                this.AddParameter("@SubRegionIs", (int) value);
            }
        }

        // Maakunta
        public int? CountyIs
        {
            get
            {
                return (int) this.GetParameter("@CountyIs");
            }
            set
            {
                if (value == null) return;
                this.whereListContacts.Add("K1.Maakunta_Id = @CountyIs");
                this.whereListConsults.Add("K2.Maakunta_Id = @CountyIs");
                this.AddParameter("@CountyIs", (int) value);
            }
        }

        // Suuralue
        public int? GreaterAreaIs
        {
            get
            {
                return (int) this.GetParameter("@GreaterAreaIs");
            }
            set
            {
                if (value == null) return;
                this.whereListContacts.Add("K1.Suuralue_Id = @GreaterAreaIs");
                this.whereListConsults.Add("K2.Suuralue_Id = @GreaterAreaIs");
                this.AddParameter("@GreaterAreaIs", (int) value);
            }
        }

        // Hallinto-oikeus
        public int? AdministrativeCourtIs
        {
            get
            {
                return (int) this.GetParameter("@AdministrativeCourtIs");
            }
            set
            {
                if (value == null) return;
                this.whereListContacts.Add(
                    "K1.HallintoOikeus_Id = @AdministrativeCourtIs");
                this.whereListConsults.Add(
                    "K2.HallintoOikeus_Id = @AdministrativeCourtIs");
                this.AddParameter("@AdministrativeCourtIs", (int) value);
            }
        }

        // Kunta
        public int? MunicipalityIs
        {
            get
            {
                return (int) this.GetParameter("@MunicipalityIs");
            }
            set
            {
                if (value == null) return;
                this.whereListContacts.Add("K1.Kunta_Id = @MunicipalityIs");
                this.whereListConsults.Add("K2.Kunta_Id = @MunicipalityIs");
                this.AddParameter("@MunicipalityIs", (int) value);
            }
        }

        public string SearchKeyword
        {
            get
            {
                return (string) this.GetParameter("@SearchKeyword");
            }
            set
            {
                if (value == null) return;
                this.AddParameter("@SearchKeyword",
                    string.Format("%{0}%", value));

                this.whereListContacts.Add(@"
(H1.PaaKayttajaEtunimi LIKE @SearchKeyword OR
H1.PaaKayttajaSukunimi LIKE @SearchKeyword OR
H1.SpostiOsoite LIKE @SearchKeyword OR
H1.Toimisto LIKE @SearchKeyword)
");
                this.whereListConsults.Add(@"
(H2.Nimi LIKE @SearchKeyword OR
H2.Etunimi LIKE @SearchKeyword OR
H2.Sukunimi LIKE @SearchKeyword OR
H2.Toimisto LIKE @SearchKeyword OR
H2.Yritystunnus LIKE @SearchKeyword)
");
            }
        }

        public string[] PersonTypes { get; set; }

        #endregion

        bool CheckVisibility(string personType)
        {
            if (this.PersonTypes.Contains("Any".ToLower())) {
                return true;
            }
            if (this.PersonTypes.Contains(personType.ToLower())) {
                return true;
            }
            return false;
    }

        public PersonQuery()
        {
            this.whereListContacts = new List<string>();
            this.whereListConsults = new List<string>();
        }

        public override string GetQueryString()
        {
            List<string> joinList = new List<string>();

            string queryStringContacts = @"
SELECT
    COALESCE(KV.authorise, 0) AS ConsultAuthorized,
    K1.Kunta_Id AS MunicipalityId,
    K1.Nimi AS MunicipalityName,
    H1.PaaKayttajaEtunimi AS FirstName,
    H1.PaaKayttajaSukunimi AS LastName,
    H1.SpostiOsoite AS Email,
    H1.Toimisto AS OrganizationName,
    H1.KatuOsoite AS StreetName,
    H1.PostiNumero AS ZipCode,
    H1.PostiToimipaikka AS City,
    H1.PuhelinNumero AS Phone,
    H1.FaxNumero AS Fax,
    H1.Toimisto AS Office,
    NULL AS VatNumber,
    'MunicipalityContact' AS PersonType
FROM
    [{1}]..[KuntaMetadata] H1

LEFT OUTER JOIN (
    SELECT
        DISTINCT
        H_Kunta_Id,
        1 as authorise
    FROM
        [{1}]..[KuntaKonsultti]
    ) KV ON
        KV.H_Kunta_Id = H1.H_Kunta_Id

LEFT OUTER JOIN [{2}]..[Kunta] K1 ON
    K1.Kunta_Id = H1.H_Kunta_Id

{0}
";
            this.whereListContacts.Add("H1.PaaKayttajaSukunimi IS NOT NULL");

            string whereStringContacts = "";
            if (this.whereListContacts.Count > 0) {
                whereStringContacts = string.Format(" WHERE {0}",
                    string.Join(" AND ", this.whereListContacts));
            }

            queryStringContacts = string.Format(queryStringContacts,
                whereStringContacts,
                ConfigurationManager.AppSettings["DbKatse"],
                ConfigurationManager.AppSettings["DbHakemisto"]);

            string queryStringConsults = @"
SELECT
    DISTINCT
    NULL AS ConsultAuthorized,
    NULL AS MunicipalityId,
    NULL AS MunicipalityName,
    H2.Nimi AS OrganizationName,
    H2.Etunimi AS FirstName,
    H2.Sukunimi AS LastName,
    H2.KatuOsoite AS StreetName,
    H2.PostiNumero AS ZipCode,
    H2.PostiToimipaikka AS City,
    H2.PuhelinNumero AS Phone,
    H2.FaxNumero AS Fax,
    H2.SpostiOsoite AS Email,
    H2.Toimisto AS Office,
    H2.YritysTunnus AS VatNumber,
    'MunicipalityConsult' AS PersonType
FROM
    [{1}]..[Konsultti] AS H2
    LEFT OUTER JOIN [{1}]..[KuntaKonsultti] KK ON
        H2.KayttajaTunnus = KK.KayttajaTunnus
    LEFT OUTER JOIN [{2}]..[Kunta] K2 ON
        K2.Kunta_Id = KK.H_Kunta_Id
{0}
";
            string whereStringConsults = "";
            if (this.whereListConsults.Count > 0) {
                whereStringConsults = string.Format(" WHERE {0}",
                    string.Join(" AND ", this.whereListConsults));
            }

            queryStringConsults = string.Format(queryStringConsults,
                whereStringConsults,
                ConfigurationManager.AppSettings["DbKatse"],
                ConfigurationManager.AppSettings["DbHakemisto"]);

            List<string> queryStrings = new List<string>();

            if (this.CheckVisibility("MunicipalityContact")) {
                queryStrings.Add(queryStringContacts);
            }
            if (this.CheckVisibility("MunicipalityConsult")) {
                queryStrings.Add(queryStringConsults);
            }

            string queryString = string.Join(" UNION ALL ", queryStrings);

            Debug.WriteLine(queryString);
            return queryString;
        }
    }
}