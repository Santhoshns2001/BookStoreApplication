using BusinessLayer.Interfaces;
using ModelLayer;
using RepositaryLayer.Entities;
using RepositaryLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Services
{
    public class FeedbackBusiness :IFeedbackBuss
    {
        private readonly IFeedbackRepo feedbackRepo;

        public FeedbackBusiness(IFeedbackRepo feedbackRepo)
        {
            this.feedbackRepo = feedbackRepo;
        }

       public Feedback AddFeedBack(int userid, FeedbackModel feedbackModel)
        {
           return feedbackRepo.AddFeedBack(userid, feedbackModel);
        }

       public bool RemoveFeedback(int feedbackid, int userid)
        {
           return feedbackRepo.RemoveFeedback(feedbackid, userid);
        }

       public List<Feedback> ViewAllFeedbacks()
        {
          return feedbackRepo.ViewAllFeedbacks();
        }

       public List<Feedback> ViewFeedbackByBookId(int bookid)
        {
           return feedbackRepo.ViewFeedbackByBookId(bookid);
        }

       public Feedback EditFeedback(int userid, EditFeedbackModel editFeedbackModel)
        {
              return feedbackRepo.EditFeedback(userid, editFeedbackModel);
        }
    }
}
