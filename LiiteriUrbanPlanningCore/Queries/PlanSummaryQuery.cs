using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Configuration;

using System.Diagnostics;

namespace LiiteriUrbanPlanningCore.Queries
{
    public class PlanSummaryQuery : SqlQuery, ISqlQuery
    {
        private List<string> whereList;

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

        public PlanSummaryQuery()
        {
            this.whereList = new List<string>();
        }

        public override string GetQueryString()
        {
            string queryString = @"
DECLARE @PlanCount INT
DECLARE @DurationMedian FLOAT

SELECT
    @PlanCount = COUNT(A.Asemakaava_Id)
FROM
    [{0}]..[Asemakaava] A
{2}

;WITH
    durcte (RowNum, d)
AS
    (
    SELECT
        ROW_NUMBER()
            OVER (ORDER BY (DATEDIFF(DAY, vireillepvm, hyvpvm))) AS RowNum,
        DATEDIFF(DAY, vireillepvm, hyvpvm) AS Duration
    FROM
        [{0}]..[Asemakaava] A
    {2}
    )
SELECT
    @DurationMedian = AVG(d) / 30.0
FROM
    durcte AS D
WHERE
    RowNum IN ((@PlanCount + 1) / 2, (@PlanCount + 2) / 2)

SELECT
    COUNT(A.Asemakaava_Id) AS PlanCount,
    CAST(SUM(A.Pinala) AS DECIMAL(20,4)) AS PlanArea,
    CAST(SUM(A.UusiPinala) AS DECIMAL(20,4)) AS PlanAreaNew,
    CAST(SUM(A.MaanalainenPinala) AS DECIMAL(20,4)) AS UndergroundArea,
    CAST(SUM(A.MuutosPinala) AS DECIMAL(20,4)) AS PlanAreaChange,
    CAST(AVG(DATEDIFF(DAY, vireillepvm, hyvpvm))/30.0 as decimal(8,1)) AS DurationAverage,
    CAST(@DurationMedian AS DECIMAL(8,1)) AS DurationMedian,
    CAST(SUM(RA.Rantaviiva) AS DECIMAL(20,2)) AS CoastlineLength,
    SUM(RA.RakennusPaikkaOma) AS BuildingCountOwn,
    SUM(RA.RakennusPaikkaMuu) AS BuildingCountOther,
    SUM(RA.RakennusPaikkaOmaLoma) AS BuildingCountOwnHoliday,
    SUM(RA.RakennusPaikkaMuuLoma) AS BuildingCountOtherHoliday
FROM
    [{0}]..[Asemakaava] A
    LEFT OUTER JOIN [{0}]..[RantaAsemakaava] RA ON
        A.Asemakaava_Id = RA.Asemakaava_Id
{1}
";
            string whereString = "";
            if (this.whereList.Count > 0) {
                whereString = " WHERE " + string.Join(" AND ", whereList);
            }

            List<string> medianWhereList = this.whereList;
            medianWhereList.Add("A.VireillePvm IS NOT NULL");
            medianWhereList.Add("A.HyvPvm IS NOT NULL");
            string medianWhereString =
                " WHERE " + string.Join(" AND ", medianWhereList);

            queryString = string.Format(queryString,
                ConfigurationManager.AppSettings["DbKatse"],
                whereString,
                medianWhereString);

            foreach (var param in this.Parameters) {
                Debug.WriteLine("{0}: {1}", param.Key, param.Value);
            }
            Debug.WriteLine(queryString);
            return queryString;
        }
    }
}
