using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Diagnostics;

namespace LiiteriUrbanPlanningCore.Queries
{
    public class UndergroundAreaQuery : SqlQuery, ISqlQuery
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


        public UndergroundAreaQuery(int? planId = null)
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
    [LiiteriKatse]..[Asemakaava] A
    INNER JOIN [LiiteriKatse]..[Aluevaraus] AV ON
        A.Asemakaava_Id = AV.Asemakaava_Id
{1}

SELECT
    'Yhteensä' AS Description,
    CAST(SUM(MT.Pinala) AS DECIMAL(20,4)) AS AreaSize,
    CAST(SUM((CASE
        WHEN @TotalAreaSize > 0
        THEN (MT.Pinala / @TotalAreaSize * 100.0)
        ELSE 0 END)) AS DECIMAL(4,1)) AS AreaPercent,
    SUM(MT.Kerrosala) AS FloorSpace,
    CAST(SUM(MT.PinalaMuutos) AS DECIMAL(20,4)) AS AreaChange,
    SUM(MT.KerrosalaMuutos) AS FloorSpaceChange
FROM
    [{0}]..[Asemakaava] A
    INNER JOIN [{0}]..[MaanalaisetTilat] MT ON
        MT.Asemakaava_Id = A.Asemakaava_Id
{1}";

            string queryStringSub = @"
SELECT
    0 AS OrderNumber,
    'Yhteensä' AS Description,
    sum(MT.Pinala) AS AreaSize,
    sum(MT.PinalaPros) AS AreaPercent,
    sum(MT.Kerrosala) AS FloorSpace,
    sum(MT.PinalaMuutos) AS AreaChange,
    sum(MT.KerrosalaMuutos) AS FloorSpaceChange
FROM
    [{0}]..[Asemakaava] A
    INNER JOIN [{0}]..[MaanalaisetTilat] MT ON
        MT.Asemakaava_Id = A.Asemakaava_Id
{1}

UNION ALL

SELECT
    VMKM.JarjNro AS OrderNumber,
    MT.Kaavamerkinta AS Description,
    sum(MT.Pinala) AS AreaSize,
    sum(MT.PinalaPros) AS AreaPercent,
    sum(MT.Kerrosala) AS FloorSpace,
    sum(MT.PinalaMuutos) AS AreaChange,
    sum(MT.KerrosalaMuutos) AS FloorSpaceChange
FROM
    [{0}]..[Asemakaava] A
    INNER JOIN [{0}]..[MaanalaisetTilat] MT ON
        MT.Asemakaava_Id = A.Asemakaava_Id
    LEFT OUTER JOIN [{0}]..[VW_MaanalaisetKaavamerkit] VMKM ON
        VMKM.Kaavamerkinta = MT.Kaavamerkinta
{1}

GROUP BY
    MT.Kaavamerkinta,
    VMKM.JarjNro,
    VMKM.Kaavamerkinta
ORDER BY
    OrderNumber,
    Description
";

            string queryString;
            switch (this.QueryType) {
                case (int) QueryTypes.Main:
                    queryString = queryStringMain;
                    break;
                case (int) QueryTypes.Sub:
                    queryString = queryStringSub;
                    this.whereList.Add("MT.Kaavamerkinta IS NOT NULL");
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
            Debug.WriteLine(queryString);
            return queryString;
        }
    }
}