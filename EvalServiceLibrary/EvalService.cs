﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.ServiceModel.Syndication;
using System.ServiceModel.Channels;

namespace EvalServiceLibrary
{
    [DataContract(Namespace = "http://pluralsight.com/evals")]
    public class Eval
    {
        [DataMember]
        public string Id;
        [DataMember]
        public string Submitter;
        [DataMember]
        public DateTime Timesent;
        [DataMember]
        public string Comments;
    }

    [ServiceContract]
    public interface IEvalService
    {

        [WebInvoke(Method = "POST", UriTemplate = "evals")]
        [OperationContract]
        string SubmitEval(Eval eval);

        [WebGet(UriTemplate = "eval/{id}")]
        [OperationContract]
        Eval GetEval(string id);

        [WebGet(UriTemplate = "evals" /*, ResponseFormat=WebMessageFormat.Json*/)]
        [OperationContract]
        List<Eval> GetAllEvals();

        [WebGet(UriTemplate = "evals/{submitter}")]
        [OperationContract]
        List<Eval> GetEvalsBySubmitter(string submitter);

        [WebInvoke(Method = "DELETE", UriTemplate = "eval/{id}")]
        [OperationContract]
        void RemoveEval(string id);

        [ServiceKnownType(typeof(Atom10FeedFormatter))]
        [ServiceKnownType(typeof(Rss20FeedFormatter))]
        [WebGet(UriTemplate = "evals/feed/{format}")]
        [OperationContract]
        SyndicationFeedFormatter GetFeed(string format);
    }

    [AttributeUsage(AttributeTargets.Class)]
    public class PrintRequestResponseAttribute : Attribute
    {
        public string test;

        public PrintRequestResponseAttribute()
        {
            test = "Alex";
        }
    }

    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    [PrintRequestResponse()]
    public class EvalService : IEvalService
    {
        List<Eval> evals = new List<Eval>();
        int evalCount = 0;

        #region IEvalService Members

        
        public string SubmitEval(Eval eval)
        {
            eval.Id = (++evalCount).ToString();
            evals.Add(eval);

            var cctx = WebOperationContext.Current;

            //return "Service : \n" +
            //    "Incoming request verb is " + WebOperationContext.Current.IncomingRequest.Method +
            //    " , Outgoing responce status is " + cctx.OutgoingResponse.StatusCode.ToString();

            System.Reflection.MemberInfo info = typeof(EvalService);
            foreach (PrintRequestResponseAttribute attrib in info.GetCustomAttributes(true))
            {
               return attrib.test;
            }

            return "";
        }

        public Eval GetEval(string id)
        {
            return evals.First(e => e.Id.Equals(id));
        }

        public List<Eval> GetAllEvals()
        {
            return this.GetEvalsBySubmitter(null);
        }

        public List<Eval> GetEvalsBySubmitter(string submitter)
        {
            if (submitter == null || submitter.Equals(""))
                return evals;
            else
                return evals.Where(e => e.Submitter.ToLower().Equals(submitter.ToLower())).ToList<Eval>();
        }

        public void RemoveEval(string id)
        {
            evals.RemoveAll(e => e.Id.Equals(id));
        }

        public SyndicationFeedFormatter GetFeed(string format)
        {
            List<Eval> evals = this.GetAllEvals();
            SyndicationFeed feed = new SyndicationFeed()
            {
                Title = new TextSyndicationContent("Pluralsight Evaluation Summary"),
                Description = new TextSyndicationContent("Recent student eval summary"),
                Items = from eval in evals
                        select new SyndicationItem()
                        {
                            Title = new TextSyndicationContent(eval.Submitter),
                            Content = new TextSyndicationContent(eval.Comments)
                        }
            };
            if (format.Equals("atom"))
                return new Atom10FeedFormatter(feed);
            else
                return new Rss20FeedFormatter(feed);
        }

        #endregion
    }
}
