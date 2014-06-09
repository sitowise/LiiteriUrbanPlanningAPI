using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Configuration;

using System.Diagnostics;

namespace LiiteriUrbanPlanningCore.Queries
{
    public class RegionQuery : SqlQuery, ISqlQuery
    {
        private List<string> whereList;
        private string regionType;

        // Ely
        public int? ElyIs
        {
            get
            {
                return (int) this.GetParameter("@ElyIs");
            }
            set
            {
                if (value == null) return;
                this.whereList.Add("T.Ely_Id = @ElyIs");
                this.AddParameter("@ElyIs", (int) value);
            }
        }

        // Seutukunta
        public int? SubRegionIs
        {
            get
            {
                return (int) this.GetParameter("@SubRegionIs");
            }
            set
            {
                if (value == null) return;
                this.whereList.Add("T.Seutukunta_Id = @SubRegionIs");
                this.AddParameter("@SubRegionIs", (int) value);
            }
        }

        // Maakunta
        public int? CountyIs
        {
            get
            {
                return (int) this.GetParameter("@CountyIs");
            }
            set
            {
                if (value == null) return;
                this.whereList.Add("T.Maakunta_Id = @CountyIs");
                this.AddParameter("@CountyIs", (int) value);
            }
        }

        // Suuralue
        public int? GreaterAreaIs
        {
            get
            {
                return (int) this.GetParameter("@GreaterAreaIs");
            }
            set
            {
                if (value == null) return;
                this.whereList.Add("T.Suuralue_Id = @GreaterAreaIs");
                this.AddParameter("@GreaterAreaIs", (int) value);
            }
        }

        // Hallinto-oikeus
        public int? AdministrativeCourtIs
        {
            get
            {
                return (int) this.GetParameter("@AdministrativeCourtIs");
            }
            set
            {
                if (value == null) return;
                this.whereList.Add("T.HallintoOikeus_Id = @AdministrativeCourtIs");
                this.AddParameter("@AdministrativeCourtIs", (int) value);
            }
        }

        public RegionQuery(string regionType)
        {
            this.regionType = regionType;
            this.whereList = new List<string>();
        }

        public override string GetQueryString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT ");

            var dbName = ConfigurationManager.AppSettings["DbHakemisto"];
            switch (this.regionType) {
                case "greaterArea": // Suuralue
                    sb.Append("T.Suuralue_Id AS regionId, ");
                    sb.Append("T.Selite AS name, ");
                    sb.Append("'greaterArea' AS regionType ");
                    sb.Append(string.Format(
                        "FROM [{0}].[dbo].[Suuralue] T ", dbName));
                    break;
                case "administrativeCourt": // Hallinto-oikeus
                    sb.Append("T.HallintoOikeus_Id AS regionId, ");
                    sb.Append("T.Nimi AS name, ");
                    sb.Append("'administrativeCourt' AS regionType ");
                    sb.Append(string.Format(
                        "FROM [{0}].[dbo].[HallintoOikeus] T ", dbName));
                    break;
                case "ely": // Ely
                    sb.Append("T.Ely_Id AS regionId, ");
                    sb.Append("T.Nimi AS name, ");
                    sb.Append("'ely' AS regionType ");
                    sb.Append(string.Format(
                        "FROM [{0}].[dbo].[Ely] T ", dbName));
                    break;
                case "county": // Maakunta
                    // Tämä voisi ainakin ottaa:
                    // Suuralue, Ely, HallintoOikeus
                    sb.Append("T.Maakunta_Id AS regionId, ");
                    sb.Append("T.Nimi AS name, ");
                    sb.Append("'county' AS regionType ");
                    sb.Append(string.Format(
                        "FROM [{0}].[dbo].[Maakunta] T ", dbName));
                    break;
                case "subRegion": // Seutukunta
                    // Tämä voisi ainakin ottaa:
                    // Maakunta
                    sb.Append("T.Seutukunta_Id AS regionId, ");
                    sb.Append("T.Nimi AS name, ");
                    sb.Append("'subRegion' AS regionType ");
                    sb.Append(string.Format(
                        "FROM [{0}].[dbo].[Seutukunta] T", dbName));
                    break;
                case "municipality": // Kunta
                    // Tämä voisi ainakin ottaa:
                    // Ely, Seutukunta, Maakunta, Suuralue
                    // ehkä muutakin
                    sb.Append("T.Kunta_Id AS regionId, ");
                    sb.Append("T.Nimi AS name, ");
                    sb.Append("'subRegion' AS regionType ");
                    sb.Append(string.Format(
                        "FROM [{0}].[dbo].[Kunta] T ", dbName));
                    break;
            }

            if (this.whereList.Count > 0) {
                sb.Append("WHERE ");
                sb.Append(string.Join(" AND ", whereList));
            }
            Debug.WriteLine(sb.ToString());

            return sb.ToString();
        }
    }
}