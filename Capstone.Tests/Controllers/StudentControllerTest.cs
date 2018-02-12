using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.Entity;
using System.Text;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Capstone;
using Capstone.Controllers;
using Capstone.Models;


//Guide to setting up Unit Testing. Need to create access classes and an interface for Students
//http://www.c-sharpcorner.com/UploadFile/raj1979/unit-testing-in-mvc-4-using-entity-framework/

namespace Capstone.Tests.Controllers
{
    [TestClass]
    public class StudentControllerTest
    {

        private MasterModel db = new MasterModel();

        [TestMethod]
        public void Details()
        {
            // Arrange
            StudentsController controller = new StudentsController();

            // Act
            ViewResult result = controller.Details("b36aaa27-87f7-4737-88b2-01a1cb42d055") as ViewResult;

            // Assert
            Assert.AreEqual("Details", result.ViewBag.Title);
        }
    }
}
