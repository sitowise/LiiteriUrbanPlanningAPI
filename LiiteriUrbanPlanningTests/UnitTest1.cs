using System;
using System.Data.SqlClient;
using System.Data.Common;
using System.Collections.Generic;
using System.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using LiiteriUrbanPlanningCore.Models;
using LiiteriUrbanPlanningCore.Queries;
using LiiteriUrbanPlanningCore.Repositories;

namespace LiiteriUrbanPlanningTests
{
    [TestClass]
    public class UnitTest1
    {
        private DbConnection GetDbConnection()
        {
            string connStr =
                ConfigurationManager.ConnectionStrings["urbanPlanningDB"].ToString();
            DbConnection db = new SqlConnection(connStr);
            db.Open();
            return db;
        }

        [TestMethod]
        public void TestPlanByNameLike()
        {
            List<Plan> l;
            using (DbConnection db = this.GetDbConnection()) {
                l = (List<Plan>) new PlanRepository(db).FindAll(
                    new PlanQuery() {
                        NameLike = "%ranta-asemakaavan%",
                    });
            }
            Assert.IsTrue(l.Count > 0);
        }

        [TestMethod]
        public void TestPlanByNameIs()
        {
            Plan p;
            string testName = "13. kaupunginosa kortteli 26, Pesulankatu 1, asemakaavan  muutos  ";
            using (DbConnection db = this.GetDbConnection()) {
                p = (Plan) new PlanRepository(db).First(
                    new PlanQuery() {
                        NameIs = testName,
                    });
            }
            Assert.AreEqual(testName, p.Name);
        }

        [TestMethod]
        public void TestPlanByIdIs()
        {
            Plan p;
            int testId = 893;
            using (DbConnection db = this.GetDbConnection()) {
                p = (Plan) new PlanRepository(db).Single(
                    new PlanQuery() { IdIs = testId });
            }
            Assert.AreEqual(testId, p.Id);
        }

        [TestMethod]
        public void TestPlanAll()
        {
            List<Plan> l;
            using (DbConnection db = this.GetDbConnection()) {
                l = (List<Plan>) new PlanRepository(db).GetAll();
            }
            Assert.IsTrue(l.Count > 0);
        }

        [TestMethod]
        public void TestPlanByNameAndIdIs()
        {
            Plan p;
            string testName = "13. kaupunginosa kortteli 26, Pesulankatu 1, asemakaavan  muutos  ";
            int testId = 893;
            using (DbConnection db = this.GetDbConnection()) {
                p = (Plan) new PlanRepository(db).First(
                    new PlanQuery() {
                        NameIs = testName,
                        IdIs = testId,
                    });
            }
            Assert.AreEqual(testName, p.Name);
            Assert.AreEqual(testId, p.Id);
        }
    }
}
