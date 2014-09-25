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

        private bool add_region_join = false;

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
                this.whereListContacts.Add("K1.Ely_Id = @ElyIs");
                this.whereListConsults.Add("K2.Ely_Id = @ElyIs");
                this.AddParameter("@ElyIs", (int) value);
                this.add_region_join = true;
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
                this.add_region_join = true;
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
                this.add_region_join = true;
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
                this.add_region_join = true;
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
                this.add_region_join = true;
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
                this.add_region_join = true;
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
H2.Toimisto LIKE @SearchKeyword)
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
            string joinString;

            string queryStringContacts = @"
SELECT
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
    [{2}]..[KuntaMetadata] H1
{0}
{1}
";
            this.whereListContacts.Add("H1.PaaKayttajaSukunimi IS NOT NULL");

            string whereStringContacts = "";
            if (this.whereListContacts.Count > 0) {
                whereStringContacts = string.Format(" WHERE {0}",
                    string.Join(" AND ", this.whereListContacts));
            }


            joinString = "";
            if (this.add_region_join) {
                joinString += @"
LEFT OUTER JOIN [{3}]..[Kunta] K1 ON
    K1.Kunta_Id = H1.H_Kunta_Id
";
            }

            queryStringContacts = string.Format(queryStringContacts,
                joinString,
                whereStringContacts,
                ConfigurationManager.AppSettings["DbKatse"],
                ConfigurationManager.AppSettings["DbHakemisto"]);

            string queryStringConsults = @"
SELECT
    DISTINCT
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
    [{2}]..[Konsultti] AS H2
{0}
{1}
";
            string whereStringConsults = "";
            if (this.whereListConsults.Count > 0) {
                whereStringConsults = string.Format(" WHERE {0}",
                    string.Join(" AND ", this.whereListConsults));
            }

            joinString = "";
            if (this.add_region_join) {
                joinString += @"
INNER JOIN [{2}]..[KuntaKonsultti] KK ON
    H2.KayttajaTunnus = KK.KayttajaTunnus
INNER JOIN [{3}]..[Kunta] K2 ON
    K2.Kunta_Id = KK.H_Kunta_Id
";
            }

            queryStringConsults = string.Format(queryStringConsults,
                joinString,
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