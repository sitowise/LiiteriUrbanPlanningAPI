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
SELECT
	COUNT(A.Asemakaava_Id) AS PlanCount,
	SUM(A.Pinala) AS PlanArea,
	SUM(A.UusiPinala) AS PlanAreaNew,
	SUM(A.MaanalainenPinala) AS UndergroundArea,
	SUM(A.MuutosPinala) AS PlanAreaChange,
	CAST(AVG(DATEDIFF(DAY, vireillepvm, hyvpvm))/30.0 as decimal(8,1)) AS DurationAverage,
	SUM(RA.Rantaviiva) AS CoastlineLength,
	SUM(RA.RakennusPaikkaOma) AS BuildingCountOwn,
	SUM(RA.RakennusPaikkaMuu) AS BuildingCountOther,
	SUM(RA.RakennusPaikkaOmaLoma) AS BuildingCountOwnHoliday,
	sum(RA.RakennusPaikkaMuuLoma) AS BuildingCountOtherHoliday
FROM
	[{0}]..[Asemakaava] A
	LEFT OUTER JOIN [{0}]..[RantaAsemakaava] RA ON
		A.Asemakaava_Id = RA.Asemakaava_Id
";
            queryString = string.Format(queryString,
                ConfigurationManager.AppSettings["DbKatse"]);

            if (this.whereList.Count > 0) {
                queryString += " WHERE " + string.Join(" AND ", whereList);
            }

            Debug.WriteLine(queryString);
            return queryString;
        }
    }
}
