using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace LiiteriUrbanPlanningCore.Queries
{
    public class AreaReservationQuery : SqlQuery, ISqlQuery
    {
        public enum QueryTypes : int
        {
            Main = 1,
            Sub = 2,
        }

        private int PlanId;

        public int QueryType { get; set; }
        
        public AreaReservationQuery(int planId)
        {
            this.PlanId = planId;
            this.AddParameter("@PlanId", planId);
        }

        public override string GetQueryString()
        {
            string queryStringMain = @"
SELECT
	S.Paaluokka_Id AS MainMarkId,
	KPL.NaytonSelite AS Description,
	S.area AS AreaSize,
	S.areapros AS AreaPercent,
	S.floorspace AS FloorSpace,
	S.effectiveness AS Efficiency,
	S.areachange AS AreaChange,
	S.floorspacechange AS FloorSpaceChange
FROM
	(SELECT
		NaytonSelite,PaaLuokka_Id
	FROM
		[{0}]..[KaavaPaaLuokka]) AS KPL
		LEFT OUTER JOIN (
			SELECT
				AV.PaaLuokka_Id,
				area=ROUND(sum(AV.Pinala),4),
				areapros=ROUND(sum(AV.PinalaPros),1),
				floorspace=ROUND(sum(AV.Kerrosala),0),
				effectiveness=(
					case
						when sum(AV.Pinala)=0 then null
						when sum(AV.Pinala) is null then null
						when sum(AV.Pinala)<>0 then ROUND((sum(AV.Kerrosala)/sum(AV.Pinala))/10000,2)
					end),
				areachange=ROUND(sum(AV.PinalaMuutos),4),
				floorspacechange = ROUND(Sum(AV.KerrosalaMuutos),0) 
			FROM
				[{0}]..[Aluevaraus] AV
				LEFT OUTER JOIN [{0}]..[Asemakaava] A ON
					AV.Asemakaava_Id = A.Asemakaava_Id  
			WHERE
				A.Asemakaava_Id = @PlanId
			GROUP BY AV.PaaLuokka_Id
		) AS S ON
			KPL.PaaLuokka_Id = S.PaaLuokka_Id  
ORDER BY
	KPL.PaaLuokka_Id 
";

            string queryStringSub = @"
SELECT
	AV.Paaluokka_Id AS MainMarkId,
	AV.Kaavamerkinta AS Description,
	AV.Pinala AS AreaSize,
	AV.PinalaPros AS AreaPercent,
	AV.Kerrosala AS FloorSpace,
	AV.Tehokkuus AS Efficiency,
	AV.PinalaMuutos AS AreaChange,
	AV.KerrosalaMuutos AS FloorSpaceChange
FROM
	[{0}]..[Asemakaava] A 
	LEFT OUTER JOIN [{0}]..[Aluevaraus] AV ON
		A.Asemakaava_Id = AV.Asemakaava_Id
WHERE
	A.Asemakaava_Id = @PlanId AND
	AV.Kaavamerkinta is not NULL
ORDER BY AV.Kaavamerkinta 
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
