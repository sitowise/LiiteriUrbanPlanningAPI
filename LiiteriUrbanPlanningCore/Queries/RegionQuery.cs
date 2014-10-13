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

        #region expressions

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
                this.whereList.Add("T.YmpVastuuEly_Id = @ElyIs");
                this.AddParameter("@ElyIs", (int) value);
            }
        }

        private int[] _ElyIn;
        public int[] ElyIn
        {
            get {
                return this._ElyIn;
            }
            set {
                if (value == null) return;
                if (this._ElyIn != null) {
                    throw new ArgumentException("Value already set!");
                }
                this._ElyIn = value;

                string[] paramNames =
                    this.PushParameters("ElyIn", value);
                this.whereList.Add(string.Format(
                    "T.YmpVastuuEly_ID IN ({0})",
                    string.Join(", ", paramNames)));
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

        private int[] _SubRegionIn;
        public int[] SubRegionIn
        {
            get {
                return this._SubRegionIn;
            }
            set {
                if (value == null) return;
                if (this._SubRegionIn != null) {
                    throw new ArgumentException("Value already set!");
                }
                this._SubRegionIn = value;

                string[] paramNames =
                    this.PushParameters("SubRegionIn", value);
                this.whereList.Add(string.Format(
                    "T.Seutukunta_Id IN ({0})",
                    string.Join(", ", paramNames)));
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

        private int[] _CountyIn;
        public int[] CountyIn
        {
            get {
                return this._CountyIn;
            }
            set {
                if (value == null) return;
                if (this._CountyIn != null) {
                    throw new ArgumentException("Value already set!");
                }
                this._CountyIn = value;

                string[] paramNames =
                    this.PushParameters("CountyIn", value);
                this.whereList.Add(string.Format(
                    "T.Maakunta_Id IN ({0})",
                    string.Join(", ", paramNames)));
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

        private int[] _GreaterAreaIn;
        public int[] GreaterAreaIn
        {
            get {
                return this._GreaterAreaIn;
            }
            set {
                if (value == null) return;
                if (this._GreaterAreaIn != null) {
                    throw new ArgumentException("Value already set!");
                }
                this._GreaterAreaIn = value;

                string[] paramNames =
                    this.PushParameters("GreaterAreaIn", value);
                this.whereList.Add(string.Format(
                    "T.Suuralue_Id IN ({0})",
                    string.Join(", ", paramNames)));
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

        private int[] _AdministrativeCourtIn;
        public int[] AdministrativeCourtIn
        {
            get {
                return this._AdministrativeCourtIn;
            }
            set {
                if (value == null) return;
                if (this._AdministrativeCourtIn != null) {
                    throw new ArgumentException("Value already set!");
                }
                this._AdministrativeCourtIn = value;

                string[] paramNames =
                    this.PushParameters("AdministrativeCourtIn", value);
                this.whereList.Add(string.Format(
                    "T.HallintoOikeus_Id IN ({0})",
                    string.Join(", ", paramNames)));
            }
        }

        #endregion

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
                    sb.Append("T.YmpVastuuEly_Id AS regionId, ");
                    sb.Append("T.YmparistoElyNimi AS name, ");
                    sb.Append("'ely' AS regionType ");
                    sb.Append(string.Format(
                        "FROM [{0}].[dbo].[Ely] T ", dbName));
                    this.whereList.Add("YmparistoEly = 1");
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
                        "FROM [{0}].[dbo].[Seutukunta] T ", dbName));
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
                sb.Append("\nWHERE ");
                sb.Append(string.Join(" AND ", whereList));
            }
            Debug.WriteLine(sb.ToString());

            return sb.ToString();
        }
    }
}