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