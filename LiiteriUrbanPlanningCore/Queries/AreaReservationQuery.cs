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

DECLARE @PlanAreaSize FLOAT

SELECT
    @PlanAreaSize = SUM(A.Pinala)
FROM
    [{0}]..[Asemakaava] A
{1}

SELECT
    1 AS prio,
    NULL AS MainMarkId,
    'Yhteensä' AS Description,
    CAST(ROUND(sum(AV.Pinala),4) AS DECIMAL(20,4)) AS AreaSize,
    CAST((CASE
            WHEN @PlanAreaSize = 0 THEN NULL
            WHEN @PlanAreaSize IS NULL THEN NULL
            WHEN @PlanAreaSize <> 0 THEN
                ROUND(100 / @PlanAreaSize * SUM(AV.Pinala), 1)
        END) AS DECIMAL(20, 1)) AS AreaPercent,
    ROUND(sum(AV.Kerrosala),0) AS FloorSpace,
    CAST((case
        WHEN SUM(AV.Pinala) = 0 then NULL
        WHEN SUM(AV.Pinala) is NULL then NULL
        WHEN SUM(AV.Pinala) <> 0 then
            ROUND((SUM(AV.Kerrosala) / SUM(AV.Pinala)) / 10000, 2)
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
        WHEN (@TotalAreaSize IS NOT NULL AND @TotalAreaSize <> 0)
        THEN ROUND((S.area / @TotalAreaSize * 100.0), 1)
        ELSE NULL END) AS DECIMAL(6, 1)) AS AreaPercent,
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
DECLARE @PlanAreaSize FLOAT

SELECT
    @PlanAreaSize = SUM(A.Pinala)
FROM
    [{0}]..[Asemakaava] A
{1}

SELECT
    1 AS prio,
    NULL AS MainMarkId,
    'Yhteensä' AS Description,
    ROUND(sum(AV.Pinala),4) AS AreaSize,
    CAST((CASE
            WHEN @PlanAreaSize = 0 THEN NULL
            WHEN @PlanAreaSize IS NULL THEN NULL
            WHEN @PlanAreaSize <> 0 THEN
                ROUND(100 / @PlanAreaSize * SUM(AV.Pinala), 1)
        END) AS DECIMAL(20, 1)) AS AreaPercent,
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
    SUM(AV.Pinala) AS AreaSize,
    CAST((CASE
        WHEN (MMS.AreaSize IS NOT NULL AND MMS.AreaSize <> 0)
        THEN ROUND((SUM(AV.Pinala) / MMS.AreaSize * 100.0), 1)
        ELSE NULL END) AS DECIMAL(6, 1)) AS AreaPercent,
    SUM(AV.Kerrosala) AS FloorSpace,
    SUM(AV.Tehokkuus) AS Efficiency,
    SUM(AV.PinalaMuutos) AS AreaChange,
    SUM(AV.KerrosalaMuutos) AS FloorSpaceChange
FROM
    [{0}]..[Asemakaava] A 
    LEFT OUTER JOIN [{0}]..[Aluevaraus] AV ON
        A.Asemakaava_Id = AV.Asemakaava_Id
    LEFT OUTER JOIN (
        SELECT
            AV.PaaLuokka_Id AS MainMarkId,
            SUM(AV.Pinala) AS AreaSize
        FROM
            [{0}]..[Asemakaava] A
            LEFT OUTER JOIN [{0}]..[Aluevaraus] AV ON
                A.Asemakaava_Id = AV.Asemakaava_Id
        {1}
        GROUP BY
            AV.PaaLuokka_Id
        ) MMS ON MMS.MainMarkId = AV.PaaLuokka_Id
{1}
AND AV.Kaavamerkinta IS NOT NULL

GROUP BY
    AV.PaaLuokka_Id,
    MMS.AreaSize,
    AV.Kaavamerkinta

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