using Microsoft.VisualStudio.TestTools.UnitTesting;
using CA.Quick.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Dynamic;
using System.Diagnostics;
using CA.Quick.Utils;

namespace CA.Quick.Models.Tests
{
    [TestClass()]
    public class CommonGroupsTests
    {
        [TestMethod()]
        public void AddTest()
        {
            dynamic data = new ExpandoObject();
            data.name = "张三";
            data.is_sys = 1;
            new CommonGroups().Add(data);
        }

        [TestMethod()]
        public void UpdateTest()
        {
            dynamic data = new ExpandoObject();
            data.name = "你好修改";
            new CommonGroups().Update(data, 3);
        }
        [TestMethod()]
        public void Update2Test()
        {
            dynamic data = new ExpandoObject();
            data.name = "你好修改";
            new CommonGroups().Update(data, CPQuery.New() + " id in (4,5)");
        }
        [TestMethod()]
        public void DeleteText()
        {

            new CommonGroups().Delete(4);
        }
        [TestMethod()]
        public void Delete3Text()
        {
            string id = "5";
            new CommonGroups().Delete(CPQuery.New() + " id > " + id.AsQueryParameter());
        }

        [TestMethod()]
        public void ListTest()
        {
            var result = new CommonGroups().List("员");
            foreach (var item in result)
            {
                string str = item + "";
                Debug.WriteLine(str);
            }

        }
        [TestMethod()]
        public void FindTest()
        {
            var result = new CommonGroups().Find(1);
            string str = result + "";
            Debug.WriteLine(str);
        }

        [TestMethod()]
        public void Find2Test()
        {
            var r=new ValidateCode().Make();
            r.Close();
        }

    }
}