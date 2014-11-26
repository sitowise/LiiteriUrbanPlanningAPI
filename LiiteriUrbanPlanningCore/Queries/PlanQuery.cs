using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;

using System.Diagnostics;

using LiiteriUrbanPlanningCore.Util;

namespace LiiteriUrbanPlanningCore.Queries
{
    public class PlanQuery : SqlQuery, ISqlQuery
    {
        public enum PlanTypes : int
        {
            Normal = 1,
            WithUndergroundAreas = 2,
            BeachPlan = 3,
        };

        private List<string> whereList;
        private List<string> areaWhereList;

        public PlanQuery()
        {
            this.whereList = new List<string>();
            this.areaWhereList = new List<string>();
        }

        #region expressions

        public string NameIs
        {
            get {
                return (string) this.GetParameter("@NameIs");
            }
            set {
                if (value == null) return;
                this.whereList.Add("A.Nimi = @NameIs");
                this.AddParameter("@NameIs", value);
            }
        }

        public string NameLike
        {
            get {
                return (string) this.GetParameter("@NameLike");
            }
            set {
                if (value == null) return;
                this.whereList.Add(
                    "(A.Nimi LIKE @NameLike OR A.NimiRUO LIKE @NameLike)");
                this.AddParameter("@NameLike", value);
            }

        }

        public int IdIs
        {
            get {
                return (int) this.GetParameter("@IdIs");
            }
            set {
                this.whereList.Add("A.Asemakaava_Id = @IdIs");
                this.AddParameter("@IdIs", value);
            }
        }

        public string GeneratedPlanIdIs
        {
            get {
                return (string) this.GetParameter("@GeneratedPlanIdIs");
            }
            set {
                if (value == null) return;
                this.whereList.Add("A.GenKaavaTunnus = @GeneratedPlanIdIs");
                this.AddParameter("@GeneratedPlanIdIs", value);
            }
        }

        private string[] _GeneratedPlanIdIn;
        public string[] GeneratedPlanIdIn
        {
            get {
                return this._GeneratedPlanIdIn;
            }
            set {
                if (value == null) return;
                if (this._GeneratedPlanIdIn != null) {
                    throw new ArgumentException("Value already set!");
                }
                this._GeneratedPlanIdIn = value;

                string[] paramNames =
                    this.PushParameters("GeneratedPlanIdIn", value);
                this.whereList.Add(string.Format(
                    "A.GenKaavaTunnus IN ({0})",
                    string.Join(", ", paramNames)));
            }
        }

        public int? TyviIdIs
        {
            get {
                return (int?) this.GetParameter("@TyviIdIs");
            }
            set {
                if (value == null) return;
                this.whereList.Add("A.Tyvi_id = @TyviIdIs");
                this.AddParameter("@TyviIdIs", value);
            }
        }

        private int[] _TyviIdIn;
        public int[] TyviIdIn
        {
            get {
                return this._TyviIdIn;
            }
            set {
                if (value == null) return;
                if (this._TyviIdIn != null) {
                    throw new ArgumentException("Value already set!");
                }
                this._TyviIdIn = value;

                string[] paramNames = this.PushParameters("TyviIdIn", value);
                this.whereList.Add(string.Format(
                    "A.Tyvi_Id IN ({0})",
                    string.Join(", ", paramNames)));
            }
        }

        public string MunicipalityPlanIdIs
        {
            get {
                return (string) this.GetParameter("@MunicipalityPlanIdIs");
            }
            set {
                if (value == null) return;
                this.whereList.Add("A.KuntaKaavaTunnus = @MunicipalityPlanIdIs");
                this.AddParameter("@MunicipalityPlanIdIs", value);
            }
        }

        private string[] _MunicipalityPlanIdIn;
        public string[] MunicipalityPlanIdIn
        {
            get {
                return this._MunicipalityPlanIdIn;
            }
            set {
                if (value == null) return;
                if (this._MunicipalityPlanIdIn != null) {
                    throw new ArgumentException("Value already set!");
                }
                this._MunicipalityPlanIdIn = value;

                string[] paramNames =
                    this.PushParameters("MunicipalityPlanIdIn", value);
                this.whereList.Add(string.Format(
                    "A.KuntaKaavaTunnus IN ({0})",
                    string.Join(", ", paramNames)));
            }
        }

        public string ApproverIs
        {
            get {
                return (string) this.GetParameter("@ApproverIs");
            }
            set {
                if (value == null) return;
                this.whereList.Add(
                    "(H.Hyvaksyja = @ApproverIs OR H.Selite = @ApproverIs)");
                this.AddParameter("@ApproverIs", value);
            }
        }

        private string[] _ApproverIn;
        public string[] ApproverIn
        {
            get {
                return this._ApproverIn;
            }
            set {
                if (value == null) return;
                if (this._ApproverIn != null) {
                    throw new ArgumentException("Value already set!");
                }
                this._ApproverIn = value;

                string[] paramNames =
                    this.PushParameters("ApproverIn", value);
                this.whereList.Add(string.Format(
                    "H.Hyvaksyja IN ({0})",
                    string.Join(", ", paramNames)));
            }
        }

        private int[] _PlanTypeIn;
        public int[] PlanTypeIn
        {
            get {
                return this._PlanTypeIn;
            }
            set {
                if (value == null) return;
                this._PlanTypeIn = value;
                var expL = new List<string>();
                if (this._PlanTypeIn.Contains(
                        (int) PlanTypes.Normal)) { // tavallinen
                    expL.Add("(R.Asemakaava_Id IS NULL AND M.Asemakaava_Id IS NULL)");
                }
                if (this._PlanTypeIn.Contains(
                        (int) PlanTypes.WithUndergroundAreas)) { // Maanalaista tilaa sis.
                    expL.Add("(M.Asemakaava_Id IS NOT NULL)");
                }
                if (this._PlanTypeIn.Contains(
                        (int) PlanTypes.BeachPlan)) { // Ranta-asemakaava
                    expL.Add("(R.Asemakaava_Id IS NOT NULL)");
                }
                this.whereList.Add(string.Format(
                    "({0})", string.Join(" OR ", expL)));
            }
        }

        public DateRange ApprovalDateWithin
        {
            get
            {
                return new DateRange(
                    (DateTime) this.GetParameter("@ApprovalDateWithinStart"),
                    (DateTime) this.GetParameter("@ApprovalDateWithinEnd"));
            }
            set
            {
                if (value == null) return;
                this.whereList.Add(
                    "(A.HyvPvm BETWEEN @ApprovalDateWithinStart AND @ApprovalDateWithinEnd)");
                this.AddParameter("@ApprovalDateWithinStart", value.Start);
                this.AddParameter("@ApprovalDateWithinEnd", value.End);
            }
        }

        public DateRange ProposalDateWithin
        {
            get
            {
                return new DateRange(
                    (DateTime) this.GetParameter("@ProposalDateWithinStart"),
                    (DateTime) this.GetParameter("@ProposalDateWithinEnd"));
            }
            set
            {
                if (value == null) return;
                this.whereList.Add(
                    "(A.EhdotusPvm BETWEEN @ProposalDateWithinStart AND @ProposalDateWithinEnd)");
                this.AddParameter("@ProposalDateWithinStart", value.Start);
                this.AddParameter("@ProposalDateWithinEnd", value.End);
            }
        }

        public DateRange InitialDateWithin
        {
            get
            {
                return new DateRange(
                    (DateTime) this.GetParameter("@InitialDateWithinStart"),
                    (DateTime) this.GetParameter("@InitialDateWithinEnd"));
            }
            set
            {
                if (value == null) return;
                this.whereList.Add(
                    "(A.VireillePvm BETWEEN @InitialDateWithinStart AND @InitialDateWithinEnd)");
                this.AddParameter("@InitialDateWithinStart", value.Start);
                this.AddParameter("@InitialDateWithinEnd", value.End);
            }
        }

        public DateRange FillDateWithin
        {
            get
            {
                return new DateRange(
                    (DateTime) this.GetParameter("@FillDateWithinStart"),
                    (DateTime) this.GetParameter("@FillDateWithinEnd"));
            }
            set
            {
                if (value == null) return;
                this.whereList.Add(
                    "(A.TayttamisPvm BETWEEN @FillDateWithinStart AND @FillDateWithinEnd)");
                this.AddParameter("@FillDateWithinStart", value.Start);
                this.AddParameter("@FillDateWithinEnd", value.End);
            }
        }

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
                this.areaWhereList.Add("K.YmpVastuuEly_ID = @ElyIs");
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
                this.areaWhereList.Add(string.Format(
                    "K.YmpVastuuEly_ID IN ({0})",
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
                this.areaWhereList.Add("K.Seutukunta_Id = @SubRegionIs");
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
                this.areaWhereList.Add(string.Format(
                    "K.Seutukunta_Id IN ({0})",
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
                this.areaWhereList.Add("K.Maakunta_Id = @CountyIs");
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
                this.areaWhereList.Add(string.Format(
                    "K.Maakunta_Id IN ({0})",
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
                this.areaWhereList.Add("K.Suuralue_Id = @GreaterAreaIs");
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
                this.areaWhereList.Add(string.Format(
                    "K.Suuralue_Id IN ({0})",
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
                this.areaWhereList.Add("K.HallintoOikeus_Id = @AdministrativeCourtIs");
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
                this.areaWhereList.Add(string.Format(
                    "K.HallintoOikeus_Id IN ({0})",
                    string.Join(", ", paramNames)));
            }
        }

        // Kunta
        public int? MunicipalityIs
        {
            get
            {
                return (int) this.GetParameter("@MunicipalityIs");
            }
            set
            {
                if (value == null) return;
                this.areaWhereList.Add("K.Kunta_Id = @MunicipalityIs");
                this.AddParameter("@MunicipalityIs", (int) value);
            }
        }

        private int[] _MunicipalityIn;
        public int[] MunicipalityIn
        {
            get {
                return this._MunicipalityIn;
            }
            set {
                if (value == null) return;
                if (this._MunicipalityIn != null) {
                    throw new ArgumentException("Value already set!");
                }
                this._MunicipalityIn = value;

                string[] paramNames =
                    this.PushParameters("MunicipalityIn", value);
                this.areaWhereList.Add(string.Format(
                    "K.Kunta_Id IN ({0})",
                    string.Join(", ", paramNames)));
            }
        }

        #endregion

        public override string GetQueryString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT ");

            //sb.Append("A.Nimi AS Name, ");
            sb.Append(@"
CASE
    WHEN
        A.NimiRuo IS NOT NULL AND LEN(A.NimiRuo) > 0
    THEN
        COALESCE(A.Nimi + ' - ', '') + COALESCE(A.NimiRuo, '')
    ELSE
        COALESCE(A.Nimi, '') + COALESCE(A.NimiRuo, '')
    END AS Name,
");
            sb.Append("A.Asemakaava_ID AS Id, ");
            sb.Append("A.Tyvi_id AS TyviId, ");
            sb.Append("A.H_Kunta_Id AS MunicipalityId, ");
            sb.Append("A.KuntaKaavaTunnus AS MunicipalityPlanId, ");
            sb.Append("A.GenKaavaTunnus AS GeneratedPlanId, ");

            sb.Append("A.HyvPvm AS ApprovalDate, ");
            sb.Append("A.EhdotusPvm AS ProposalDate, ");
            sb.Append("A.VireillePvm AS InitialDate, ");
            sb.Append("A.TayttamisPvm AS FillDate, ");

            sb.Append("K.Nimi AS MunicipalityName, ");
            sb.Append("H.Hyvaksyja AS DecisionMaker, ");

            sb.Append("A.HyvPykala AS DecisionNumber, ");
            sb.Append("CAST(A.Pinala AS DECIMAL(20,4)) AS PlanArea, ");
            sb.Append("CAST(A.MaanalainenPinala AS DECIMAL(20,4)) AS UndergroundArea, ");
            sb.Append("CAST(A.UusiPinala AS DECIMAL(20,4)) AS PlanAreaNew, ");
            sb.Append("CAST(A.MuutosPinala AS DECIMAL(20,4)) AS PlanAreaChange, ");
            sb.Append("R.Rantaviiva AS CoastlineLength, ");
            sb.Append("R.RakennusPaikkaOma AS BuildingCountOwn, ");
            sb.Append("R.RakennusPaikkaMuu AS BuildingCountOther, ");
            sb.Append("R.RakennusPaikkaOmaLoma AS BuildingCountOwnHoliday, ");
            sb.Append("R.RakennusPaikkaMuuLoma AS BuildingCountOtherHoliday, ");

            sb.Append("CAST(DATEDIFF(day, A.VireillePvm, A.HyvPvm)/30.0 AS DECIMAL(10,1)) AS Duration ");

            sb.Append(string.Format(
                "FROM [{0}]..[Asemakaava] A ",
                ConfigurationManager.AppSettings["DbKatse"]));

            sb.Append(string.Format(
                "INNER JOIN [{0}]..[Kunta] K ",
                ConfigurationManager.AppSettings["DbHakemisto"]));
            //sb.Append("ON K.Nro = A.H_Kunta_Id ")
            sb.Append("ON K.Kunta_Id = A.H_Kunta_Id ");

            sb.Append(string.Format(
                "LEFT OUTER JOIN [{0}]..[Hyvaksyja] H ",
                ConfigurationManager.AppSettings["DbKatse"]));
            sb.Append("ON H.Hyvaksyja_Id = A.Hyvaksyja_Id ");

            sb.Append(string.Format(
                "LEFT OUTER JOIN [{0}]..[RantaAsemakaava] R ON ",
                ConfigurationManager.AppSettings["DbKatse"]));
            sb.Append("R.Asemakaava_Id = A.Asemakaava_Id ");

            sb.Append("OUTER APPLY (SELECT TOP 1 * ");
            sb.Append(string.Format(
                "FROM [{0}]..[MaanalaisetTilat] M ",
                ConfigurationManager.AppSettings["DbKatse"]));
            sb.Append("WHERE M.Asemakaava_Id = A.Asemakaava_Id) M ");
            if (this.areaWhereList.Count > 0) {
                this.whereList.Add(String.Format("({0})",
                    string.Join(" OR ", this.areaWhereList)));
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