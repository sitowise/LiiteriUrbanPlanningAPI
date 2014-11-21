using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

using System.Diagnostics;

namespace LiiteriUrbanPlanningCore.Queries
{
    public class AreaReservationQuery : SqlQuery, ISqlQuery
    {
        public enum QueryTypes : int
        {
            Main = 1,
            Sub = 2,
        }

        private List<string> whereList;

        public int QueryType { get; set; }

        public int? PlanIdIs
        {
            get
            {
                return (int?) this.GetParameter("@PlanIdIs");
            }
            set
            {
                if (value == null) return;
                this.AddParameter("@PlanIdIs", value);
                this.whereList.Add("A.Asemakaava_Id = @PlanIdIs");
            }
        }

        private int[] _PlanIdIn;
        public int[] PlanIdIn
        {
            get
            {
                return this._PlanIdIn;
            }
            set
            {
                if (value == null) return;
                if (this._PlanIdIn != null) {
                    throw new ArgumentException("Value already set!");
                }
                this._PlanIdIn = value;
                string[] paramNames = value.Select(
                        (s, i) => "@PlanIdIn_" + i.ToString()
                    ).ToArray();
                for (int i = 0; i < paramNames.Length; i++) {
                    this.AddParameter(paramNames[i], value[i]);
                }
                this.whereList.Add(string.Format(
                    "A.Asemakaava_Id IN ({0})",
                    string.Join(",", paramNames)));
            }
        }

        public AreaReservationQuery(int? planId = null)
        {
            this.whereList = new List<string>();
            this.PlanIdIs = planId;
        }

        public override string GetQueryString()
        {
            string queryStringMain = @"
DECLARE @TotalAreaSize FLOAT

SELECT
    @TotalAreaSize = SUM(AV.Pinala)
FROM
    [{0}]..[Asemakaava] A
    INNER JOIN [{0}]..[Aluevaraus] AV ON
        A.Asemakaava_Id = AV.Asemakaava_Id  
{1}

SELECT
    1 AS prio,
    NULL AS MainMarkId,
    'Yhteensä' AS Description,
    CAST(ROUND(sum(AV.Pinala),4) AS DECIMAL(20,4)) AS AreaSize,
    CAST(100 AS DECIMAL(4,1)) AS AreaPercent,
    ROUND(sum(AV.Kerrosala),0) AS FloorSpace,
    CAST((case
        when sum(AV.Pinala)=0 then null
        when sum(AV.Pinala) is null then null
        when sum(AV.Pinala)<>0 then
            ROUND((sum(AV.Kerrosala)/sum(AV.Pinala))/10000,2)
    end) AS DECIMAL(20,2)) AS Efficiency,
    CAST(ROUND(sum(AV.PinalaMuutos),4) AS DECIMAL(20,4)) AS AreaChange,
    ROUND(sum(AV.KerrosalaMuutos),0) AS FloorSpaceChange 
FROM
    [{0}]..[Asemakaava] A
    INNER JOIN [{0}]..[Aluevaraus] AV ON
        A.Asemakaava_Id = AV.Asemakaava_Id  
{1}

UNION ALL

SELECT
    2 AS prio,
    KPL.Paaluokka_Id AS MainMarkId,
    KPL.NaytonSelite AS Description,
    CAST(S.area AS DECIMAL(20,4)) AS AreaSize,
    CAST((CASE
        WHEN @TotalAreaSize > 0
        THEN ROUND((S.area / @TotalAreaSize * 100.0), 1)
        ELSE 0 END) AS DECIMAL(4,1)) AS AreaPercent,
    S.floorspace AS FloorSpace,
    CAST(S.effectiveness AS DECIMAL(20,2)) AS Efficiency,
    CAST(S.areachange AS DECIMAL(20, 4)) AS AreaChange,
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
                        when sum(AV.Pinala)<>0 then
                            ROUND((sum(AV.Kerrosala)/sum(AV.Pinala))/10000,2)
                    end),
                areachange=ROUND(sum(AV.PinalaMuutos),4),
                floorspacechange = ROUND(Sum(AV.KerrosalaMuutos),0) 
            FROM
                [{0}]..[Aluevaraus] AV
                LEFT OUTER JOIN [{0}]..[Asemakaava] A ON
                    AV.Asemakaava_Id = A.Asemakaava_Id  
                {1}
            GROUP BY
                AV.PaaLuokka_Id
        ) AS S ON
            KPL.PaaLuokka_Id = S.PaaLuokka_Id  
ORDER BY
    prio,
    MainMarkId
";

            string queryStringSub = @"
SELECT
    1 AS prio,
    NULL AS MainMarkId,
    'Yhteensä' AS Description,
    ROUND(sum(AV.Pinala),4) AS AreaSize,
    100 AS AreaPercent,
    ROUND(sum(AV.Kerrosala),0) AS FloorSpace,
    (case
        when sum(AV.Pinala)=0 then null
        when sum(AV.Pinala) is null then null
        when sum(AV.Pinala)<>0 then
            ROUND((sum(AV.Kerrosala)/sum(AV.Pinala))/10000,2)
    end) AS Efficiency,
    ROUND(sum(AV.PinalaMuutos),4) AS AreaChange,
    ROUND(sum(AV.KerrosalaMuutos),0) AS FloorSpaceChange 
FROM
    [{0}]..[Asemakaava] A
    INNER JOIN [{0}]..[Aluevaraus] AV ON
        A.Asemakaava_Id = AV.Asemakaava_Id  
{1}

UNION ALL

SELECT
    2 AS prio,
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
{1}
ORDER BY
    prio,
    Description
";
            string queryString;
            switch (this.QueryType) {
                case (int) QueryTypes.Main:
                    queryString = queryStringMain;
                    break;
                case (int) QueryTypes.Sub:
                    queryString = queryStringSub;
                    this.whereList.Add("AV.Kaavamerkinta IS NOT NULL");
                    break;
                default:
                    throw new ArgumentException("QueryType not specified!");
            }

            string whereExpr = "";
            if (this.whereList.Count > 0) {
                whereExpr = " WHERE " + string.Join(" AND ", whereList);
            }

            queryString = string.Format(queryString,
                ConfigurationManager.AppSettings["DbKatse"],
                whereExpr);

            foreach (var param in this.Parameters) {
                Debug.WriteLine("{0}: {1}", param.Key, param.Value);
            }
            Debug.WriteLine(queryString);
            return queryString;
        }
    }
}