using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using celestia_api.Models.Returns_Reponses;
namespace celestia_api.Controllers
{
    [RoutePrefix("Api/Threads")]
    public class ThreadsController : ApiController
    {
        private readonly Models.celestia celestia = new Models.celestia();
        [HttpGet]
        [Route("GetThreads")]
        public IHttpActionResult GetThreadsOfBoard(int boardid, int page = 1)
        {
            int pageSize = 10;
            var threads = celestia.Threads
                                .Where(e => e.board_id == boardid)
                                .OrderByDescending(e => e.created_at)
                                .Skip((page - 1) * pageSize)
                                .Take(pageSize)
                                .ToList();
            if(threads.Count>0)
            {
                return Ok(threads);
            }
            else
            {
                return NotFound();
            }
            
        }


        [HttpGet]
        [Route("GetChildThreadsOfMainThread")]
        public IHttpActionResult GetChildThreadsOfMainThread(int threadid)
        {
            var childthreads = celestia.Threads.Where(e => e.parent_thread == threadid).ToList();
            if(childthreads.Count>0)
            {
                return Ok(childthreads);
            }
            else
            {
                return Ok("not found");
            }
        }


        [HttpGet]
        [Route("GetThreadData")]
        public IHttpActionResult GetThreadData(int threadid)
        {
            var threaddata = celestia.Threads.Where(e => e.thread_id == threadid).FirstOrDefault();
            if(threaddata!=null)
            {
                return Ok(threaddata);
            }
            else
            {
                return Ok("No Thread Data");
            }
        }


        [HttpPost]
        [Route("CreateThread")]
        public IHttpActionResult CreateThread(ThreadCreationClass threadCreation)
        {
            try
            {                
                if (threadCreation.imagebytes != null && threadCreation.imagebytes.Length > 0)
                {
                     
                    long fileSize = threadCreation.imagebytes.Length;                    
                    Models.Thread newthread = new Models.Thread()
                    {


                        board_id = threadCreation.board_id,
                        user_cookie_id = Request.Headers.GetCookies("user_cookie").FirstOrDefault()?.Cookies.FirstOrDefault(c => c.Name == "user_cookie")?.Value,
                        thread_caption = threadCreation.thread_caption,
                        content = threadCreation.content,
                        image = threadCreation.imagebytes, 
                        image_file_name = threadCreation.imagefilename,
                        image_file_size = Convert.ToInt32(fileSize),
                        created_at = DateTime.Now,
                    };
                    celestia.Threads.Add(newthread);
                    celestia.SaveChanges();
                    return Ok("Thread Created Successfully");
                }
                else
                {
                    return BadRequest("Image bytes need to be attached");
                }
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }


        [HttpPost]
        [Route("ReplyToThread")]
        public IHttpActionResult ReplyToParentThreads(ChildThreadCreationClass childThread)
        {
            try
            {
                
                var parentThreadData = celestia.Threads.FirstOrDefault(e => e.thread_id == childThread.parent_thread);

                
                if (parentThreadData == null)
                {
                    return NotFound();
                }

                bool isReplyToOp = false;
                bool isOpReply = false;

                
                var userCookie = Request.Headers.GetCookies("user_cookie").FirstOrDefault()?.Cookies.FirstOrDefault(c => c.Name == "user_cookie")?.Value;

                
                isReplyToOp = childThread.parent_thread == parentThreadData.thread_id;

                
                if (userCookie == parentThreadData.user_cookie_id && childThread.parent_thread != 0)
                {
                    isOpReply = true;
                }

                Models.Thread childrenthread = new Models.Thread
                {
                    parent_thread=childThread.parent_thread,
                    board_id=parentThreadData.board_id,
                    user_cookie_id=userCookie,
                    thread_caption=childThread.thread_caption,
                    content=childThread.content,
                    image=childThread.imagebytes,
                    image_file_name=childThread.image_file_name,
                    image_file_size= childThread.imagebytes != null ? (int?)childThread.imagebytes.Length : null,
                    created_at =DateTime.Now,
                    is_reply_to_op=isReplyToOp,
                    is_op_reply=isOpReply
                };

                celestia.Threads.Add(childrenthread);
                celestia.SaveChanges();

                return Ok();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

    }
}
