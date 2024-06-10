using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace celestia_api.Controllers
{
    public class BoardsController : ApiController
    {
        private readonly Models.celestia celestia = new Models.celestia();
        public IHttpActionResult GetBoards()
        {
            try
            {
                var boards = celestia.Boards.ToList();
                if(boards.Count>0)
                {
                    return Ok(boards);
                }
                else
                {
                    return NotFound();
                }
                
            }
            catch(Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        public IHttpActionResult GetPopularThreads()
        {
            try
            {
                return Ok();
            }
            catch(Exception ex)
            {
                return InternalServerError(ex);
            }
        }



    }
}
