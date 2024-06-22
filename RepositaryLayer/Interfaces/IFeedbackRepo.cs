using ModelLayer;
using RepositaryLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositaryLayer.Interfaces
{
    public interface IFeedbackRepo
    {
        public Feedback AddFeedBack(int userid, FeedbackModel feedbackModel);
        public Feedback EditFeedback(int userid, EditFeedbackModel editFeedbackModel);
        public bool RemoveFeedback(int feedbackid, int userid);
        public List<Feedback> ViewAllFeedbacks();
        public List<Feedback> ViewFeedbackByBookId(int bookid);
    }
}
