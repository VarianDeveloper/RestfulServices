using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;

namespace EvalServiceLibrary
{
    [ServiceContract]
    public interface ICourseService
    {
        [OperationContract]
        List<string> GetCourseList();
    }

    public class CourseService : ICourseService
    {
        #region ICourseService Members

        public List<string> GetCourseList()
        {
            List<string> courses = new List<string>();
            courses.Add("WCF Fundamentals");
            courses.Add("WF Fundamentals");
            courses.Add("WPF Fundamentals");
            courses.Add("Silverlight Fundamentals");
            return courses;
        }

        #endregion
    }
}
