using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace LiiteriUrbanPlanningCore.Queries
{
    public class UndergroundAreaQuery : SqlQuery, ISqlQuery
    {
        public enum QueryTypes : int
        {
            Main = 1,
            Sub = 2,
        }

        private int PlanId;
        public int QueryType { get; set; }

        public UndergroundAreaQuery(int planId)
        {
            this.PlanId = planId;
            this.AddParameter("@PlanId", planId);
        }

        public override string GetQueryString()
        {
            string queryStringMain = @"
SELECT
	'Yhteensä' AS Description,
	sum(MT.Pinala) AS AreaSize,
	sum(MT.PinalaPros) AS AreaPercent,
	sum(MT.Kerrosala) AS FloorSpace,
	sum(MT.PinalaMuutos) AS AreaChange,
	sum(MT.KerrosalaMuutos) AS FloorSpaceChange
FROM
	[LiiteriKatse]..[Asemakaava] A
	INNER JOIN [LiiteriKatse]..[MaanalaisetTilat] MT ON
		MT.Asemakaava_Id = A.Asemakaava_Id  
WHERE
	A.Asemakaava_Id = @PlanId
";

            string queryStringSub = @"
SELECT
	VMKM.JarjNro AS OrderNumber,
	MT.Kaavamerkinta AS Description,
	sum(MT.Pinala) AS AreaSize,
	sum(MT.PinalaPros) AS AreaPercent,
	sum(MT.Kerrosala) AS FloorSpace,
	sum(MT.PinalaMuutos) AS AreaChange,
	sum(MT.KerrosalaMuutos) AS FloorSpaceChange
FROM
	[LiiteriKatse]..[Asemakaava] A
	INNER JOIN [LiiteriKatse]..[MaanalaisetTilat] MT ON
		MT.Asemakaava_Id = A.Asemakaava_Id  
	LEFT OUTER JOIN [LiiteriKatse]..[VW_MaanalaisetKaavamerkit] VMKM ON
		VMKM.Kaavamerkinta = MT.Kaavamerkinta
WHERE
	A.Asemakaava_Id = @PlanId AND
	MT.Kaavamerkinta is not NULL
GROUP BY
	MT.Kaavamerkinta,
	VMKM.JarjNro,
	VMKM.Kaavamerkinta  
ORDER BY
	VMKM.JarjNro,
	VMKM.Kaavamerkinta
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