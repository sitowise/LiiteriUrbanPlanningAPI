using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace LiiteriUrbanPlanningCore.Queries
{
    public class BuildingConservationQuery : SqlQuery, ISqlQuery
    {
        public enum QueryTypes : int
        {
            Main = 1,
            Sub = 2,
        }

        private int PlanId;

        public int QueryType { get; set; }

        public BuildingConservationQuery(int planId)
        {
            this.PlanId = planId;
            this.AddParameter("@PlanId", planId);
        }

        public override string GetQueryString()
        {
            string queryStringMain = @"
SELECT
	RST.RakennusSuojTyyppi_Id AS ConservationTypeId,
	RST.RakennusSuojTyyppi As ConservationTypeName,
	sum(RS.Lkm) AS BuildingCount,
	sum(RS.Kerrosala) AS FloorSpace,
	sum(RS.MuutosLkm) AS ChangeCount,
	sum(RS.MuutosKerrosala) AS ChangeFloorSpace
FROM
	[{0}]..[Asemakaava] A
	LEFT OUTER JOIN [{0}]..[RakennusSuoj] RS ON
		RS.Asemakaava_Id = A.Asemakaava_Id
	INNER JOIN [{0}]..[RakennusSuojTyyppi] RST ON
		RST.RakennusSuojTyyppi_Id = RS.RakennusSuojTyyppi_Id
WHERE
	A.Asemakaava_Id = @PlanId
GROUP BY
	RST.RakennusSuojTyyppi_Id,
	RST.RakennusSuojTyyppi
ORDER BY
	RST.RakennusSuojTyyppi
";

            string queryStringSub = @"
SELECT
	NULL AS ConservationTypeId,
	RST.RakennusSuojTyyppi AS ConservationTypeName,
	RSSUM.buildingcount AS BuildingCount,
	RSSUM.floorspace AS FloorSpace,
	RSSUM.changecount AS ChangeCount,
	RSSUM.changefloorspace AS ChangeFloorSpace
FROM
	(SELECT
		RakennusSuojTyyppi, RakennusSuojTyyppi_Id
	FROM
		[{0}]..RakennusSuojTyyppi
	WHERE
		RakennusSuojTyyppi_Id in(1,2)) AS RST
	LEFT OUTER JOIN (
		SELECT
			RS.RakennusSuojTyyppi_Id,
			buildingcount=sum(RS.Lkm),
			floorspace=sum(RS.Kerrosala),
			changecount=sum(RS.MuutosLkm),
			changefloorspace=sum(RS.MuutosKerrosala)
FROM
	[{0}]..[RakennusSuoj] RS
	LEFT OUTER JOIN [{0}]..[Asemakaava] A ON
		A.Asemakaava_Id = RS.Asemakaava_Id
WHERE
	A.Asemakaava_Id = @PlanId
GROUP BY
	RS.RakennusSuojTyyppi_Id) AS RSSUM ON
		RST.RakennusSuojTyyppi_Id = RSSUM.RakennusSuojTyyppi_Id
ORDER BY
	RST.RakennusSuojTyyppi
";
            string queryString;
            switch (this.QueryType) {
                case (int) QueryTypes.Main:
                    queryString = queryStringMain;
                    break;
                case (int) QueryTypes.Sub:
                    queryString = queryStringSub;
                    break;
                default:
                    throw new ArgumentException("QueryType not specified!");
            }
            queryString = string.Format(queryString,
                ConfigurationManager.AppSettings["DbKatse"]);
            return queryString;
        }
    }
}