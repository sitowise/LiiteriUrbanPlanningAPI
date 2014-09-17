using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

using System.Diagnostics;

namespace LiiteriUrbanPlanningCore.Queries
{
    public class BuildingConservationQuery : SqlQuery, ISqlQuery
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

        public BuildingConservationQuery(int? planId = null)
        {
            this.whereList = new List<string>();
            this.PlanIdIs = planId;
        }

        public override string GetQueryString()
        {
            string queryStringMain = @"
SELECT
    1 AS prio,
    NULL AS ConservationTypeId,
    'Yhteensä' As ConservationTypeName,
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
{1}

UNION ALL

SELECT
    2 AS prio,
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
{1}
GROUP BY
    RST.RakennusSuojTyyppi_Id,
    RST.RakennusSuojTyyppi

ORDER BY
    prio,
    ConservationTypeId
";

            string queryStringSub = @"
SELECT
    1 AS prio,
    NULL AS ConservationTypeId,
    'Yhteensä' As ConservationTypeName,
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
{1}

UNION ALL

SELECT
    2 AS prio,
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
    INNER JOIN (
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
    {1}

    GROUP BY
        RS.RakennusSuojTyyppi_Id
    ) AS RSSUM ON
        RST.RakennusSuojTyyppi_Id = RSSUM.RakennusSuojTyyppi_Id

ORDER BY
    prio,
    ConservationTypeId
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
            Debug.WriteLine(queryString);
            return queryString;
        }
    }
}