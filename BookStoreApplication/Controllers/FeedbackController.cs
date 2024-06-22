using BusinessLayer.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ModelLayer;
using RepositaryLayer.Entities;

namespace BookStoreApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FeedbackController : ControllerBase
    {
        private readonly IFeedbackBuss feedbackBuss;

        public FeedbackController(IFeedbackBuss feedbackBuss)
        {
            this.feedbackBuss = feedbackBuss;
        }

        [Authorize]
        [HttpPost("AddFeedBack")]
        public IActionResult AddFeedBack(FeedbackModel model)
        {
               int userId = Convert.ToInt32(User.FindFirst("userId").Value);
            Feedback feedback = feedbackBuss.AddFeedBack(userId, model);
            if (feedback != null)
            {
                return Ok(new ResponseModel<Feedback> { IsSuccuss = true, Message = "feedbck added  succussfully", Data = feedback });
            }
            else
            {
                return BadRequest(new ResponseModel<Feedback> { IsSuccuss = false, Message = " failed to add feedback", Data = feedback });
            }

        }

        [HttpGet("ViewFeedbackByBookId")]
        public IActionResult ViewFeedbackByBookId(int bookid)
        {
            var feedbacks = feedbackBuss.ViewFeedbackByBookId(bookid);
            if (feedbacks != null)
            {
                return Ok(new ResponseModel<List<Feedback>> { IsSuccuss = true, Message = "list of feedbacks fetched succussfully", Data = feedbacks });
            }
            else
            {
                return BadRequest(new ResponseModel<string> { IsSuccuss = false, Message = " failed to fetch", Data = "something went wrong" });
            }
        }

        [Authorize]
        [HttpPut("EditFeedback")]
        public IActionResult EditFeedback(EditFeedbackModel model)
        {
            int userId = Convert.ToInt32(User.FindFirst("userId").Value);
            Feedback feedback = feedbackBuss.EditFeedback(userId, model);
            if (feedback != null)
            {
                return Ok(new ResponseModel<Feedback> { IsSuccuss = true, Message = "feedback updated succussfully", Data = feedback });
            }
            else
            {
                return BadRequest(new ResponseModel<string> { IsSuccuss = false, Message = " failed to update feedback", Data = "wrong input is been provided " });
            }
        }

        [Authorize]
        [HttpDelete("RemoveFeedback")]
        public ActionResult RemoveFeedback(int feedbackid)
        {
            int userId = Convert.ToInt32(User.FindFirst("userId").Value);
            var feedback = feedbackBuss.RemoveFeedback(feedbackid,userId);
            if (feedback)
            {
                return Ok(new ResponseModel<string> { IsSuccuss = true, Message = "feedback deleted  succussfully", Data = "deleted succussfully" });
            }
            else
            {
                return BadRequest(new ResponseModel<string> { IsSuccuss = false, Message = " failed to delete feedback", Data = "feedback delete unsuccuss" });
            }

        }

        [HttpGet("ViewAllFeedbacks")]
        public IActionResult ViewAllFeedbacks()
        {
            var feedbacks = feedbackBuss.ViewAllFeedbacks();
            if (feedbacks != null)
            {
                return Ok(new ResponseModel<List<Feedback>> { IsSuccuss = true, Message = "fetch all feedbacks is succuss", Data = feedbacks });
            }
            else
            {
                return BadRequest(new ResponseModel<string> { IsSuccuss = false, Message = " failed to fetch", Data = "fetch all feedbacks operation is unsuccuss" });
            }

        }

    }
}
