using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Configuration;

namespace LiiteriUrbanPlanningCore.Queries
{
    public class RegionQuery : SqlQuery, ISqlQuery
    {
        private string regionType;

        public RegionQuery(string regionType)
        {
            this.regionType = regionType;
        }

        public override string GetQueryString()
        {
            string queryString = null;
            var dbName = ConfigurationManager.AppSettings["DbHakemisto"];
            switch (this.regionType) {
                case "greaterArea": // Suuralue
                    queryString = @"
SELECT
    T.Suuralue_Id AS regionId,
    T.Selite AS name,
    'greaterArea' AS regionType
FROM
    [{0}].[dbo].[Suuralue] T
";
                    queryString = string.Format(queryString, dbName);
                    break;
                case "administrativeCourt": // Hallinto-oikeus
                    queryString = @"
SELECT
    T.HallintoOikeus_Id AS regionId,
    T.Nimi AS name,
    'administrativeCourt' AS regionType
FROM
    [{0}].[dbo].[HallintoOikeus] T
";
                    queryString = string.Format(queryString, dbName);
                    break;
                case "ely": // Ely
                    queryString = @"
SELECT
    T.Ely_Id AS regionId,
    T.Nimi AS name,
    'ely' AS regionType
FROM
    [{0}].[dbo].[Ely] T
";
                    queryString = string.Format(queryString, dbName);
                    break;
                case "county": // Maakunta
                    // Tämä voisi ainakin ottaa:
                    // Suuralue, Ely, HallintoOikeus
                    queryString = @"
SELECT
    T.Maakunta_Id AS regionId,
    T.Nimi AS name,
    'county' AS regionType
FROM
    [{0}].[dbo].[Maakunta] T
";
                    queryString = string.Format(queryString, dbName);
                    break;
                case "subRegion": // Seutukunta
                    // Tämä voisi ainakin ottaa:
                    // Maakunta
                    queryString = @"
SELECT
    T.Seutukunta_Id AS regionId,
    T.Nimi AS name,
    'subRegion' AS regionType
FROM
    [{0}].[dbo].[Seutukunta] T
";
                    queryString = string.Format(queryString, dbName);
                    break;
                case "municipality": // Kunta
                    // Tämä voisi ainakin ottaa:
                    // Ely, Seutukunta, Maakunta, Suuralue
                    // ehkä muutakin
                    queryString = @"
SELECT
    T.Kunta_Id AS regionId,
    T.Nimi AS name,
    'subRegion' AS regionType
FROM
    [{0}].[dbo].[Kunta] T
";
                    queryString = string.Format(queryString, dbName);
                    break;
            }

            return queryString;
        }
    }
}
