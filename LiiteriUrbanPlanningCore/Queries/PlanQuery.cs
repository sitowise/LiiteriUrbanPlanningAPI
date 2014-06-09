using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;

using LiiteriUrbanPlanningCore.Util;

namespace LiiteriUrbanPlanningCore.Queries
{
    public class PlanQuery : SqlQuery, ISqlQuery
    {
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

            sb.Append("A.Nimi, ");
            sb.Append("A.Asemakaava_ID, ");
            sb.Append("A.H_Kunta_Id, ");
            sb.Append("A.KuntaKaavaTunnus, ");
            sb.Append("A.GenKaavaTunnus, ");

            sb.Append("A.HyvPvm, ");
            sb.Append("A.EhdotusPvm, ");
            sb.Append("A.VireillePvm, ");
            sb.Append("A.TayttamisPvm ");

            sb.Append(string.Format(
                "FROM [{0}].[dbo].[Asemakaava] A ",
                ConfigurationManager.AppSettings["DbKatse"]));

            sb.Append(string.Format(
                "INNER JOIN [{0}].[dbo].[Kunta] K ",
                ConfigurationManager.AppSettings["DbHakemisto"]));
            //sb.Append("ON K.Nro = A.H_Kunta_Id ")
            sb.Append("ON K.Kunta_Id = A.H_Kunta_Id ");

            if (this.whereList.Count > 0) {
                sb.Append("WHERE ");
                sb.Append(string.Join(" AND ", whereList));
            }

            return sb.ToString();
        }
    }
}