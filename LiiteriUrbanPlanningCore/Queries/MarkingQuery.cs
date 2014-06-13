using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Diagnostics;

namespace LiiteriUrbanPlanningCore.Queries
{
    public class MarkingQuery : SqlQuery, ISqlQuery
    {
        public enum QueryTypes : int
        {
            AreaReservationsStandard = 1,
            AreaReservationsMunicipality = 2,
            UndergroundAreasStandard = 3,
            UndergroundAreasMunicipality = 4,
        }

        public int QueryType { get; set; }

        private List<string> whereList;

        public int? MunicipalityIdIs
        {
            get
            {
                return (int) this.GetParameter("@MunicipalityIdIs");
            }
            set
            {
                if (value == null) return;
                this.AddParameter("@MunicipalityIdIs", value);
                this.whereList.Add("K.Kunta_Id = @MunicipalityIdIs");
            }
        }

        public string MainMarkNameIs
        {
            get
            {
                return (string) this.GetParameter("@MainMarkNameIs");
            }
            set
            {
                if (value == null) return;
                this.AddParameter("@MainMarkNameIs", value);
                this.whereList.Add("KPL.PaaLuokka = @MainMarkNameIs");
            }
        }

        public string NameIs
        {
            get
            {
                return (string) this.GetParameter("@NameIs");
            }
            set
            {
                if (value == null) return;
                this.AddParameter("@NameIs", value);
                this.whereList.Add("KM.Kaavamerkinta = @NameIs");
            }
        }

        public MarkingQuery()
        {
            this.whereList = new List<string>();
        }

        public override string GetQueryString()
        {
            string queryStringAreaReservationsStandard = @"
SELECT
	NULL AS MunicipalityId,
	NULL AS MunicipalityName,
	KPL.PaaLuokka_Id AS MainMarkId,
	KPL.PaaLuokka AS MainMarkName,
	KM.Kaavamerkinta AS Name,
	KM.Selite AS Description
FROM
	[{0}]..[AsetusKaavaMerkinta] KM
	INNER JOIN [{0}]..[KaavaPaaLuokka] KPL ON
		KM.PaaLuokka_Id = KPL.PaaLuokka_Id
{2}
ORDER BY
	KM.Kaavamerkinta
";
            string queryStringAreaReservationsMunicipality = @"
SELECT
	KM.H_Kunta_Id AS MunicipalityId,
	K.Nimi AS MunicipalityName,
	KPL.PaaLuokka_Id AS MainMarkId,
	KPL.PaaLuokka AS MainMarkName,
	KM.Kaavamerkinta AS Name,
	KM.Selite AS Description
FROM
	[{0}]..[KuntaKaavaMerkinta] KM
	INNER JOIN [{0}]..[KaavaPaaLuokka] KPL ON
		KM.PaaLuokka_Id = KPL.PaaLuokka_Id
	INNER JOIN [{1}]..[Kunta] K ON
		KM.H_Kunta_Id = K.Kunta_Id
{2}
ORDER BY
	KM.Kaavamerkinta
";
            string queryStringUndergroundStandard = @"
SELECT
	NULL AS MunicipalityId,
	NULL AS MunicipalityName,
	NULL AS MainMarkId,
	NULL AS MainMarkName,
	KM.Kaavamerkinta AS Name,
	KM.Selite AS Description
FROM
	[{0}]..[MaanalaisetKaavaMerkinta] KM
{2}
ORDER BY
	KM.Kaavamerkinta
";
            string queryStringUndergroundMunicipality = @"
SELECT
	KM.H_Kunta_Id AS MunicipalityId,
	K.Nimi AS MunicipalityName,
	NULL AS MainMarkId,
	NULL AS MainMarkName,
	KM.Kaavamerkinta AS Name,
	KM.Selite AS Description
FROM
	[{0}]..[MaanalaisetKuntaKaavaMerkinta] KM
	INNER JOIN [{1}]..[Kunta] K ON
		KM.H_Kunta_Id = K.Kunta_Id
{2}
ORDER BY
	KM.Kaavamerkinta
";

            string queryString;

            switch (this.QueryType) {
                case (int) QueryTypes.AreaReservationsStandard:
                    queryString = queryStringAreaReservationsStandard;
                    break;
                case (int) QueryTypes.AreaReservationsMunicipality:
                    queryString = queryStringAreaReservationsMunicipality;
                    break;
                case (int) QueryTypes.UndergroundAreasStandard:
                    queryString = queryStringUndergroundStandard;
                    break;
                case (int) QueryTypes.UndergroundAreasMunicipality:
                    queryString = queryStringUndergroundMunicipality;
                    break;
                default:
                    throw new ArgumentException("Invalid QueryType!");
            }

            string whereString = "";
            if (this.whereList.Count > 0) {
                whereString = string.Format("WHERE {0}",
                    string.Join(" AND ", this.whereList));
            }

            queryString = string.Format(queryString,
                ConfigurationManager.AppSettings["DbKatse"],
                ConfigurationManager.AppSettings["DbHakemisto"],
                whereString);

            Debug.WriteLine(queryString);
            return queryString;
        }
    }
}
