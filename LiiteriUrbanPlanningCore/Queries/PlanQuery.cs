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

        public PlanQuery()
        {
            this.whereList = new List<string>();
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
                this.whereList.Add("A.Nimi LIKE @NameLike");
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
                this.whereList.Add("K.Ely_Id = @ElyIs");
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
                this.whereList.Add("K.Seutukunta_Id = @SubRegionIs");
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
                this.whereList.Add("K.Maakunta_Id = @CountyIs");
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
                this.whereList.Add("K.Suuralue_Id = @GreaterAreaIs");
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
                this.whereList.Add("K.HallintoOikeus_Id = @AdministrativeCourtIs");
                this.AddParameter("@AdministrativeCourtIs", (int) value);
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
                this.whereList.Add("K.Kunta_Id = @MunicipalityIs");
                this.AddParameter("@MunicipalityIs", (int) value);
            }
        }

        #endregion

        public override string GetQueryString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT ");

            sb.Append("A.Nimi AS Name, ");
            sb.Append("A.Asemakaava_ID AS Id, ");
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
            sb.Append("A.Pinala AS PlanArea, ");
            sb.Append("A.MaanalainenPinala AS UndergroundArea, ");
            sb.Append("A.UusiPinala AS PlanAreaNew, ");
            sb.Append("A.MuutosPinala AS PlanAreaChange, ");
            sb.Append("R.Rantaviiva AS CoastlineLength, ");
            sb.Append("R.RakennusPaikkaOma AS BuildingCountOwn, ");
            sb.Append("R.RakennusPaikkaMuu AS BuildingCountOther, ");
            sb.Append("R.RakennusPaikkaOmaLoma AS BuildingCountOwnHoliday, ");
            sb.Append("R.RakennusPaikkaMuuLoma AS BuildingCountOtherHoliday ");

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

            if (this.whereList.Count > 0) {
                sb.Append("WHERE ");
                sb.Append(string.Join(" AND ", whereList));
            }

            Debug.WriteLine(sb.ToString());
            return sb.ToString();
        }
    }
}